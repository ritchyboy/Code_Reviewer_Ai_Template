import subprocess

def test_csharp_app_builds():
 
    result = subprocess.run(["dotnet", "build", "../CodeReviewerAI/CodeReviewerAI.csproj"], 
                            capture_output=True, text=True)
    
    assert result.returncode == 0
    print("C# Build Successful from Python!")