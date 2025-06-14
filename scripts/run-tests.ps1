# LindebergsHealth Test Runner
# Führt alle Tests aus und generiert einen Bericht

param(
    [string]$Configuration = "Debug",
    [switch]$Coverage,
    [switch]$Verbose
)

Write-Host "🧪 LindebergsHealth Test Suite" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan

$ErrorActionPreference = "Stop"
$startTime = Get-Date

try {
    # Prüfe ob dotnet verfügbar ist
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        throw ".NET SDK ist nicht installiert oder nicht im PATH verfügbar"
    }

    # Erstelle Test-Output-Verzeichnis
    $outputDir = "TestResults"
    if (Test-Path $outputDir) {
        Remove-Item $outputDir -Recurse -Force
    }
    New-Item -ItemType Directory -Path $outputDir | Out-Null

    Write-Host "📋 Konfiguration:" -ForegroundColor Yellow
    Write-Host "  - Configuration: $Configuration"
    Write-Host "  - Coverage: $($Coverage.IsPresent)"
    Write-Host "  - Verbose: $($Verbose.IsPresent)"
    Write-Host ""

    # Build Solution
    Write-Host "🔨 Building Solution..." -ForegroundColor Green
    dotnet build --configuration $Configuration --no-restore
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed"
    }

    # Test-Projekte definieren
    $testProjects = @(
        "tests/LindebergsHealth.Domain.Tests",
        "tests/LindebergsHealth.Infrastructure.Tests", 
        "tests/LindebergsHealth.Application.Tests"
    )

    $totalTests = 0
    $passedTests = 0
    $failedTests = 0
    $skippedTests = 0

    foreach ($project in $testProjects) {
        $projectName = Split-Path $project -Leaf
        Write-Host "🧪 Running tests for $projectName..." -ForegroundColor Blue
        
        $testArgs = @(
            "test"
            $project
            "--configuration"
            $Configuration
            "--no-build"
            "--logger"
            "trx;LogFileName=$projectName.trx"
            "--results-directory"
            $outputDir
        )

        if ($Coverage.IsPresent) {
            $testArgs += @(
                "--collect"
                "XPlat Code Coverage"
            )
        }

        if ($Verbose.IsPresent) {
            $testArgs += @("--verbosity", "detailed")
        }

        $result = & dotnet @testArgs
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  ✅ $projectName tests passed" -ForegroundColor Green
        } else {
            Write-Host "  ❌ $projectName tests failed" -ForegroundColor Red
            $failedTests++
        }

        # Parse test results from output
        $testOutput = $result -join "`n"
        if ($testOutput -match "Passed:\s+(\d+)") {
            $passedTests += [int]$matches[1]
        }
        if ($testOutput -match "Failed:\s+(\d+)") {
            $failedTests += [int]$matches[1]
        }
        if ($testOutput -match "Skipped:\s+(\d+)") {
            $skippedTests += [int]$matches[1]
        }
        if ($testOutput -match "Total:\s+(\d+)") {
            $totalTests += [int]$matches[1]
        }

        Write-Host ""
    }

    # Performance Tests (optional)
    Write-Host "⚡ Running Performance Tests..." -ForegroundColor Magenta
    $perfResult = dotnet test tests/LindebergsHealth.Infrastructure.Tests --filter "Category=Performance" --configuration $Configuration --no-build
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✅ Performance tests completed" -ForegroundColor Green
    } else {
        Write-Host "  ⚠️ Performance tests had issues (non-critical)" -ForegroundColor Yellow
    }

    # Integration Tests
    Write-Host "🔗 Running Integration Tests..." -ForegroundColor Magenta
    $integrationResult = dotnet test tests/LindebergsHealth.Infrastructure.Tests --filter "Category=Integration" --configuration $Configuration --no-build
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ✅ Integration tests completed" -ForegroundColor Green
    } else {
        Write-Host "  ⚠️ Integration tests had issues (non-critical)" -ForegroundColor Yellow
    }

    # Test Summary
    $endTime = Get-Date
    $duration = $endTime - $startTime

    Write-Host ""
    Write-Host "📊 Test Summary" -ForegroundColor Cyan
    Write-Host "===============" -ForegroundColor Cyan
    Write-Host "Total Tests:   $totalTests"
    Write-Host "Passed:        $passedTests" -ForegroundColor Green
    Write-Host "Failed:        $failedTests" -ForegroundColor $(if ($failedTests -gt 0) { "Red" } else { "Green" })
    Write-Host "Skipped:       $skippedTests" -ForegroundColor Yellow
    Write-Host "Duration:      $($duration.ToString('mm\:ss'))"
    Write-Host ""

    # Coverage Report
    if ($Coverage.IsPresent) {
        Write-Host "📈 Generating Coverage Report..." -ForegroundColor Blue
        
        # Install reportgenerator if not present
        if (-not (Get-Command reportgenerator -ErrorAction SilentlyContinue)) {
            Write-Host "Installing ReportGenerator..."
            dotnet tool install -g dotnet-reportgenerator-globaltool
        }

        $coverageFiles = Get-ChildItem -Path $outputDir -Filter "coverage.cobertura.xml" -Recurse
        if ($coverageFiles.Count -gt 0) {
            $coverageFileList = ($coverageFiles.FullName -join ";")
            reportgenerator -reports:$coverageFileList -targetdir:"$outputDir/CoverageReport" -reporttypes:Html
            Write-Host "  📋 Coverage report generated: $outputDir/CoverageReport/index.html" -ForegroundColor Green
        }
    }

    # Test Results Location
    Write-Host "📁 Test Results Location: $outputDir" -ForegroundColor Blue
    
    if ($failedTests -gt 0) {
        Write-Host ""
        Write-Host "❌ Some tests failed. Please check the test results for details." -ForegroundColor Red
        exit 1
    } else {
        Write-Host ""
        Write-Host "🎉 All tests passed successfully!" -ForegroundColor Green
        exit 0
    }

} catch {
    Write-Host ""
    Write-Host "💥 Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
} 