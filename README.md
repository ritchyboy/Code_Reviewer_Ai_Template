# CodeReviewerAI

CodeReviewerAI is a .NET-based backend utility designed to automate GitHub Pull Request code reviews using the Gemini API. It performs automated static analysis, identifies security and architectural flaws, and computes a risk assessment score before reporting feedback.

The primary objective of Phase 1 was to establish a resilient core pipeline using decoupled, production-grade design patterns.

---

## Prerequisites and Configuration

The application requires access to the GitHub API and the Google Gemini API. These must be configured via environment variables or your local `appsettings.json` file before launching the execution layer.

### Required Credentials

* **`GEMINI_API_KEY`**: A valid API key generated via Google AI Studio to authenticate requests to the Gemini 3.5 Flash engine.
* **`GITHUB_TOKEN`**: A GitHub Personal Access Token (PAT) with repository read/write permissions to fetch the pull request diffs and publish the completed review comments.

### Configuration Layout

Ensure your local configuration or environment block maps to the following structural schema:

```json
{
  "GeminiProvider": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  },
  "GitHubProvider": {
    "AuthToken": "YOUR_GITHUB_TOKEN"
  }
}

```

---

## Operational Model

Phase 1 operates strictly as a Command Line Interface (CLI) utility designed for direct execution or manual integration into a continuous integration (CI) pipeline. It is not an automated background listener.

### Local Execution

To invoke the review engine manually against a specific repository target, execute the compiled binary via the terminal:

```bash
dotnet run --project CodeReviewerAI.Worker --owner "your-github-username" --repo "target-repository" --pr 42

```

### CI/CD Pipeline Integration

To utilize this tool as a static analysis step within a GitHub Actions workflow, add the execution block directly into your repository's workflow configuration file:

```yaml
- name: Run Automated Code Review
  run: dotnet run --project CodeReviewerAI.Worker --owner ${{ github.repository_owner }} --repo ${{ github.event.repository.name }} --pr ${{ github.event.number }}
  env:
    GEMINI_API_KEY: ${{ secrets.GEMINI_API_KEY }}
    GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}

```

---

## Architectural Highlights

* **Strategy Pattern for Language Parsing:** Leverages an `ILanguageStrategy` abstraction to isolate language-specific syntax rules and review parameters. This ensures the core review engine is fully open-closed; support for new languages can be introduced without modifying the execution orchestration layer.
* **Resilience and Fault Tolerance:** Integrated a Polly resilience pipeline to handle distributed system volatility. The engine employs exponential backoff with randomized jitter to manage transient upstream network failures and rate limits safely without dropping the execution thread.
* **Strict Separation of Concerns:** Core static analysis, file system management, and API gateway logic are completely decoupled into dedicated services, ensuring testability via mock interfaces.

---

## Tech Stack

* **Runtime:** .NET 8.0 / C#
* **LLM Integration:** Google Gemini API (Targeting Gemini 3.5 Flash)
* **Testing:** xUnit / FluentAssertions

---

## Known Technical Debt and Core Limitations

Phase 1 deliberately leaves specific architectural boundaries unresolved to prioritize initial pipeline delivery. These constraints are documented below as engineering debt to be resolved in Phase 2.

### 1. Greedy Regex JSON Extraction

* **Limitation:** The current extraction mechanism uses an un-anchored regular expression running in single-line mode to isolate JSON structures from the LLM text stream.
* **Impact:** This approach is greedy. If the model outputs text outside of the primary JSON object that contains trailing curly braces, the regex window will overshoot, corrupting the payload and causing downstream deserialization failures.

### 2. Environmental File-Path Dependency

* **Limitation:** The `FileBaseStrategy` resolves prompt configuration files at runtime by referencing `AppDomain.CurrentDomain.BaseDirectory`.
* **Impact:** This introduces an environment dependency. If the binary is executed outside its native build output folder, or deployed via single-file publish/Linux containers, file resolution will fail with a `FileNotFoundException`.

### 3. Context Window Vulnerability

* **Limitation:** Pull request file diffs are aggregated synchronously into a single `StringBuilder` buffer before transmission to the API.
* **Impact:** Massive pull requests with large lines-of-code diffs will exceed the input token limit or model generation constraints, leading to truncated reviews or unhandled API gateway rejections.

---

## Phase 2 Roadmap

The next iteration of the system will migrate the architecture from a local utility to an enterprise-grade automated service:

* **Deterministic Boundary Parser:** Replace the regular expression extraction layer with a linear, character-by-character brace-balancing state machine to extract nested JSON without structural boundary failures.
* **Embedded Configuration Ingestion:** Migrate local text configuration assets into Embedded Resources, utilizing `GetManifestResourceStream` to ensure complete environmental independence.
* **Semantic Chunking Utility:** Implement an abstract context manager that calculates input tokens and programmatically slices extensive code diffs into logical, independent chunks before processing.
* **Asynchronous Background Processing:** Transition the application into a decoupled .NET Background Worker that processes incoming review requests asynchronously via a persistent data queue backed by SQLite/PostgreSQL.
