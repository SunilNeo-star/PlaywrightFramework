# Set environment variables
$env:TEST_USERNAME = "admin"
$env:TEST_PASSWORD = "admin"

# Generate timestamp for filename
$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$reportName = "TestResults_$timestamp.html"

# Run tests and generate HTML report
dotnet test --logger "html;LogFileName=$reportName" --results-directory TestResults

# Open the report automatically
start "TestResults\$reportName"

Write-Host "Report saved as: $reportName"