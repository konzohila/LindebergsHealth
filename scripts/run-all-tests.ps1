#!/usr/bin/env pwsh

<#
.SYNOPSIS
    Führt alle Tests für LindebergsHealth aus
.DESCRIPTION
    Dieses Script führt alle Unit-, Integration- und Performance-Tests aus,
    generiert Coverage-Reports und erstellt detaillierte Test-Berichte.
.PARAMETER TestType
    Art der Tests: All, Unit, Integration, Performance
.PARAMETER Coverage
    Ob Coverage-Reports generiert werden sollen
.PARAMETER Parallel
    Ob Tests parallel ausgeführt werden sollen
.PARAMETER Verbose
    Detaillierte Ausgabe
.EXAMPLE
    .\run-all-tests.ps1 -TestType All -Coverage -Verbose
#>

param(
    [Parameter(Mandatory = $false)]
    [ValidateSet("All", "Unit", "Integration", "Performance")]
    [string]$TestType = "All",
    
    [Parameter(Mandatory = $false)]
    [switch]$Coverage,
    
    [Parameter(Mandatory = $false)]
    [switch]$Parallel,
    
    [Parameter(Mandatory = $false)]
    [switch]$Verbose,
    
    [Parameter(Mandatory = $false)]
    [switch]$SkipBuild
)

# Farben für die Ausgabe
$Green = [System.ConsoleColor]::Green
$Red = [System.ConsoleColor]::Red
$Yellow = [System.ConsoleColor]::Yellow
$Blue = [System.ConsoleColor]::Blue
$Cyan = [System.ConsoleColor]::Cyan

function Write-ColoredOutput {
    param(
        [string]$Message,
        [System.ConsoleColor]$Color = [System.ConsoleColor]::White
    )
    $originalColor = $Host.UI.RawUI.ForegroundColor
    $Host.UI.RawUI.ForegroundColor = $Color
    Write-Host $Message
    $Host.UI.RawUI.ForegroundColor = $originalColor
}

function Write-Header {
    param([string]$Title)
    Write-Host ""
    Write-ColoredOutput "=" * 80 -Color $Cyan
    Write-ColoredOutput "  $Title" -Color $Cyan
    Write-ColoredOutput "=" * 80 -Color $Cyan
    Write-Host ""
}

function Write-Step {
    param([string]$Message)
    Write-ColoredOutput "▶ $Message" -Color $Blue
}

function Write-Success {
    param([string]$Message)
    Write-ColoredOutput "✓ $Message" -Color $Green
}

function Write-Error {
    param([string]$Message)
    Write-ColoredOutput "✗ $Message" -Color $Red
}

function Write-Warning {
    param([string]$Message)
    Write-ColoredOutput "⚠ $Message" -Color $Yellow
}

# Hauptfunktion
function Main {
    $startTime = Get-Date
    
    Write-Header "LindebergsHealth Test Suite"
    Write-ColoredOutput "Start Zeit: $($startTime.ToString('yyyy-MM-dd HH:mm:ss'))" -Color $Blue
    Write-ColoredOutput "Test Typ: $TestType" -Color $Blue
    Write-ColoredOutput "Coverage: $($Coverage.IsPresent)" -Color $Blue
    Write-ColoredOutput "Parallel: $($Parallel.IsPresent)" -Color $Blue
    
    # Arbeitsverzeichnis setzen
    $rootPath = Split-Path -Parent $PSScriptRoot
    Set-Location $rootPath
    Write-Step "Arbeitsverzeichnis: $rootPath"
    
    # Test-Ergebnisse Ordner erstellen
    $testResultsPath = Join-Path $rootPath "TestResults"
    if (Test-Path $testResultsPath) {
        Remove-Item $testResultsPath -Recurse -Force
    }
    New-Item -ItemType Directory -Path $testResultsPath -Force | Out-Null
    Write-Step "Test-Ergebnisse Ordner erstellt: $testResultsPath"
    
    # Build (falls nicht übersprungen)
    if (-not $SkipBuild) {
        Write-Header "Build Solution"
        Write-Step "Lösung wird gebaut..."
        
        $buildResult = dotnet build --configuration Release --no-restore
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Build fehlgeschlagen!"
            return 1
        }
        Write-Success "Build erfolgreich abgeschlossen"
    }
    
    # Test-Projekte finden
    $testProjects = @()
    
    switch ($TestType) {
        "All" {
            $testProjects = Get-ChildItem -Path "tests" -Filter "*.csproj" -Recurse | ForEach-Object { $_.FullName }
        }
        "Unit" {
            $testProjects = Get-ChildItem -Path "tests" -Filter "*Tests.csproj" -Recurse | 
                Where-Object { $_.Name -notmatch "(Integration|Performance)" } | 
                ForEach-Object { $_.FullName }
        }
        "Integration" {
            $testProjects = Get-ChildItem -Path "tests" -Filter "*Integration*Tests.csproj" -Recurse | 
                ForEach-Object { $_.FullName }
        }
        "Performance" {
            $testProjects = Get-ChildItem -Path "tests" -Filter "*Performance*Tests.csproj" -Recurse | 
                ForEach-Object { $_.FullName }
        }
    }
    
    if ($testProjects.Count -eq 0) {
        Write-Warning "Keine Test-Projekte für Typ '$TestType' gefunden"
        return 0
    }
    
    Write-Step "Gefundene Test-Projekte: $($testProjects.Count)"
    foreach ($project in $testProjects) {
        $projectName = [System.IO.Path]::GetFileNameWithoutExtension($project)
        Write-ColoredOutput "  - $projectName" -Color $Yellow
    }
    
    # Test-Ausführung
    Write-Header "Test Ausführung"
    
    $totalTests = 0
    $passedTests = 0
    $failedTests = 0
    $skippedTests = 0
    
    foreach ($project in $testProjects) {
        $projectName = [System.IO.Path]::GetFileNameWithoutExtension($project)
        Write-Step "Führe Tests aus für: $projectName"
        
        # Test-Kommando zusammenstellen
        $testArgs = @(
            "test"
            $project
            "--configuration", "Release"
            "--no-build"
            "--logger", "trx;LogFileName=$projectName.trx"
            "--results-directory", $testResultsPath
        )
        
        if ($Verbose) {
            $testArgs += "--verbosity", "detailed"
        }
        
        if ($Coverage) {
            $testArgs += "--collect", "XPlat Code Coverage"
            $testArgs += "--settings", "tests/coverlet.runsettings"
        }
        
        if ($Parallel) {
            $testArgs += "--parallel"
        }
        
        # Tests ausführen
        $testOutput = & dotnet @testArgs 2>&1
        $testExitCode = $LASTEXITCODE
        
        if ($Verbose) {
            Write-Host $testOutput
        }
        
        # Test-Ergebnisse parsen
        $testSummary = $testOutput | Where-Object { $_ -match "Total tests:|Passed:|Failed:|Skipped:" }
        if ($testSummary) {
            foreach ($line in $testSummary) {
                if ($line -match "Total tests: (\d+)") {
                    $totalTests += [int]$matches[1]
                }
                if ($line -match "Passed: (\d+)") {
                    $passedTests += [int]$matches[1]
                }
                if ($line -match "Failed: (\d+)") {
                    $failedTests += [int]$matches[1]
                }
                if ($line -match "Skipped: (\d+)") {
                    $skippedTests += [int]$matches[1]
                }
            }
        }
        
        if ($testExitCode -eq 0) {
            Write-Success "$projectName - Tests erfolgreich"
        } else {
            Write-Error "$projectName - Tests fehlgeschlagen (Exit Code: $testExitCode)"
        }
    }
    
    # Coverage-Report generieren
    if ($Coverage) {
        Write-Header "Coverage Report"
        Write-Step "Generiere Coverage-Report..."
        
        $coverageFiles = Get-ChildItem -Path $testResultsPath -Filter "coverage.cobertura.xml" -Recurse
        if ($coverageFiles.Count -gt 0) {
            # ReportGenerator installieren falls nicht vorhanden
            $reportGenerator = dotnet tool list -g | Select-String "reportgenerator"
            if (-not $reportGenerator) {
                Write-Step "Installiere ReportGenerator..."
                dotnet tool install -g dotnet-reportgenerator-globaltool
            }
            
            # HTML-Report generieren
            $coverageReportPath = Join-Path $testResultsPath "CoverageReport"
            $coverageInput = ($coverageFiles | ForEach-Object { $_.FullName }) -join ";"
            
            reportgenerator -reports:$coverageInput -targetdir:$coverageReportPath -reporttypes:"Html;Badges"
            
            Write-Success "Coverage-Report generiert: $coverageReportPath"
        } else {
            Write-Warning "Keine Coverage-Dateien gefunden"
        }
    }
    
    # Zusammenfassung
    Write-Header "Test Zusammenfassung"
    
    $endTime = Get-Date
    $duration = $endTime - $startTime
    
    Write-ColoredOutput "Ende Zeit: $($endTime.ToString('yyyy-MM-dd HH:mm:ss'))" -Color $Blue
    Write-ColoredOutput "Dauer: $($duration.ToString('hh\:mm\:ss'))" -Color $Blue
    Write-Host ""
    
    Write-ColoredOutput "Gesamt Tests: $totalTests" -Color $Blue
    Write-ColoredOutput "Erfolgreich: $passedTests" -Color $Green
    Write-ColoredOutput "Fehlgeschlagen: $failedTests" -Color $Red
    Write-ColoredOutput "Übersprungen: $skippedTests" -Color $Yellow
    
    if ($failedTests -eq 0) {
        Write-Success "Alle Tests erfolgreich! 🎉"
        return 0
    } else {
        Write-Error "$failedTests Test(s) fehlgeschlagen! ❌"
        return 1
    }
}

# Script ausführen
try {
    $exitCode = Main
    exit $exitCode
} catch {
    Write-Error "Unerwarteter Fehler: $($_.Exception.Message)"
    Write-Error $_.ScriptStackTrace
    exit 1
} 