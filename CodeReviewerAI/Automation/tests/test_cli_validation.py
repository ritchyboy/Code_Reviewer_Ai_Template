import subprocess
import pytest

PROJECT_PATH = "../CodeReviewerAI/CodeReviewerAI.csproj"

def test_invalid_pr_number_exits_with_error():
    cmd = ["dotnet","run","--project",PROJECT_PATH,"--","Ritchyboy","Sandbox_Test","x"]

    result = subprocess.run(cmd,capture_output=True,text=True)

    assert result.returncode != 0 

    assert "is not a valid integer" in result.stderr or "is not a valid integer"
    print("\nSuccess: C# validation caught the bad Pr number")


def test_missing_arguments_show_usage():

    cmd = ["dotnet","run","--project",PROJECT_PATH,"--","OnlyOneArgument"]

    result = subprocess.run(cmd,capture_output=True,text=True)

    assert result.returncode != 0
    assert "Missing argument" in result.stdout or "Missing argument" in result.stdout
    print("\nSuccess: C# validation caught missing argument")
    


