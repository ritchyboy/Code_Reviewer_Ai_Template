# CodeReviewerAI

CodeReviewerAI is a .NET-based backend utility designed to automate GitHub Pull Request code reviews using the Gemini API. It performs automated static analysis, identifies security and architectural flaws, and computes a risk assessment score before reporting feedback directly back to the pull request.

The primary objective of Phase 1 was to establish a resilient core pipeline using decoupled, production-grade design patterns.

---

## Prerequisites and Configuration

The application requires configuration values to authenticate with the GitHub and Gemini APIs. The application leverages standard .NET hierarchical configuration mapping; keys can be provided either via a local `appsettings.json` file or injected as Environment Variables using the double-underscore (`__`) delimiter.

### Required Environment Configuration

If deploying via a container, shell, or CI pipeline, export the following environment variables:

* **`Gemini__ApiKey`**: A valid API key generated via Google AI Studio.
* **`GitHub__Token`**: A GitHub Personal Access Token (PAT) with write permissions for Pull Requests.
* **`Gemini__Model`**: Target model variant (e.g., `gemini-3.5-flash`).
* **`Gemini__Provider`**: The AI infrastructure provider (set to `Google`).
* **`Gemini__Temperature`**: Controls model output randomness (e.g., `1.0`).
* **`GitHub__AppName`**: Identifier string used in user-agent string metadata (set to `CodeReviewerAI`).

### Local Configuration Layout

For local development, create an `appsettings.json` file in the root execution directory mapping to this structural schema:

```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY",
    "Model": "gemini-3.5-flash",
    "Provider": "Google",
    "Temperature": 1.0
  },
  "GitHub": {
    "Token": "YOUR_GITHUB_TOKEN",
    "AppName": "CodeReviewerAI"
  }
}

```

---

## Operational Model

Phase 1 operates strictly as a Command Line Interface (CLI) utility optimized to be invoked manually or downstream of a Continuous Integration (CI) runner. It expects three positional arguments passed during execution: `owner`, `repository_name`, and `pull_request_number`.

### Local Execution

To run the application manually from the solution root directory, pass the targeted repository parameters:

```bash
dotnet run --project CodeReviewerAI/CodeReviewerAI.csproj <owner> <repository_name> <pull_request_number>

```

### CI/CD Pipeline Integration

To automate code reviews on every pull request action, integrate the utility directly into a GitHub Actions workflow. Create a file at `.github/workflows/code-reviewer-ci.yml` with the configuration below:

```yaml
name: Code Reviewer CI

on:
  pull_request:
    types: [opened, synchronize]

permissions:
  pull-requests: write
  contents: read

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    
    defaults:
      run:
        working-directory: CodeReviewerAI

    steps:
      - name: Checkout Code
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      - name: Restore and Build
        run: |
          dotnet restore CodeReviewerAI.sln
          dotnet build --configuration Release --no-restore

      - name: Run Unit Tests
        run: dotnet test CodeReviewerAI.Tests.Unit --configuration Release --no-build

      - name: Run Reviewer CLI
        run: >
          dotnet run --project CodeReviewerAI/CodeReviewerAI.csproj
          ${{ github.repository_owner }} 
          ${{ github.event.repository.name }} 
          ${{ github.event.pull_request.number }}
        env:
          Gemini__ApiKey: ${{ secrets.GEMINI_API_KEY }}
          GitHub__Token: ${{ secrets.GITHUB_TOKEN }}
          Gemini__Model: "gemini-3.5-flash"
          Gemini__Provider: "Google"
          Gemini__Temperature: "1.0"
          GitHub__AppName: "CodeReviewerAI"

```

---

## Architectural Highlights

* **Strategy Pattern for Language Parsing:** Leverages an `ILanguageStrategy` abstraction to isolate language-specific syntax rules and review parameters. This ensures the core review engine remains closed to modification but open to extension for new languages.
* **Resilience and Fault Tolerance:** Integrated a Polly resilience pipeline into the HTTP architecture. The client engine employs exponential backoff with randomized jitter to mitigate downstream service unavailability and transient capacity drops.
* **Strict Separation of Concerns:** Application orchestration, file system utilities, and API gateways communicate across explicit abstractions, allowing components to be fully isolated and tested using mocks.

---

## Tech Stack

* **Runtime:** .NET 8.0 / C#
* **LLM Integration:** Google Gemini API
* **Testing Engine:** xUnit / FluentAssertions

---

## Known Technical Debt and Core Limitations

Phase 1 deliberately leaves specific architectural boundaries unresolved to prioritize pipeline delivery. These constraints are documented below as engineering debt to be resolved in Phase 2.

### 1. Greedy Regex JSON Extraction

* **Limitation:** The parsing subsystem uses an un-anchored regular expression running in single-line mode to isolate JSON blocks from the LLM text output stream.
* **Impact:** The approach is greedy. If the model outputs text outside of the primary JSON block that contains closing braces, the matching window will overshoot, corrupting the payload and causing downstream deserialization failures.

### 2. Environmental File-Path Dependency

* **Limitation:** The asset loading implementation resolves raw text files by referencing `AppDomain.CurrentDomain.BaseDirectory`.
* **Impact:** This introduces an environment dependency. If the execution context shifts outside its native build layout folder (e.g., single-file publish formats or specific Linux environments), configuration resolution will throw a `FileNotFoundException`.

### 3. Context Window Vulnerability

* **Limitation:** Code diffs are aggregated synchronously into a single memory buffer before transmission to the network gateway.
* **Impact:** Massive pull requests containing excessive line-of-code changes risk exceeding the model's context window size, causing the API endpoint to drop the connection or return truncated payloads.

---

## Phase 2 Roadmap

The system design will scale from a local CLI execution harness into an enterprise-grade automated system:

* **Deterministic Boundary Parser:** Replace regular expression string extractions with a linear, character-by-character brace-balancing state machine to process nested structures safely.
* **Embedded Configuration Assets:** Migrate text assets into compilation payloads as Embedded Resources, reading them using `GetManifestResourceStream` to achieve absolute path independence.
* **Semantic Chunking Utility:** Introduce a token tracking algorithm to compute payload weights and split extensive code diffs into logical, separate requests before network serialization.
* **Asynchronous Background Processing:** Transition the execution layer into a persistent .NET Background Worker processing incoming payloads asynchronously via a persistent database queue (SQLite/PostgreSQL).
