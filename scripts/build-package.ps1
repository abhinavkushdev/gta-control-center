param(
    [string]$Configuration = "Release",
    [string]$Version = "0.1.0-beta.1",
    [string]$Runtime = "win-x64"
)

$ErrorActionPreference = "Stop"

$RepoRoot = Split-Path -Parent $PSScriptRoot
$Solution = Join-Path $RepoRoot "GtaControlCenter.sln"
$Project = Join-Path $RepoRoot "src/GtaControlCenter.App/GtaControlCenter.App.csproj"
$ArtifactsRoot = Join-Path $RepoRoot "artifacts"
$FrameworkDependent = Join-Path $ArtifactsRoot "framework-dependent"
$SelfContained = Join-Path $ArtifactsRoot "self-contained-$Runtime"

Remove-Item $FrameworkDependent -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item $SelfContained -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $FrameworkDependent | Out-Null
New-Item -ItemType Directory -Force -Path $SelfContained | Out-Null

dotnet restore $Solution
dotnet build $Solution -c $Configuration /p:Version=$Version

dotnet publish $Project -c $Configuration -o $FrameworkDependent --self-contained false /p:Version=$Version

dotnet publish $Project -c $Configuration -r $Runtime -o $SelfContained --self-contained true /p:PublishSingleFile=true /p:Version=$Version

Compress-Archive -Path "$FrameworkDependent/*" -DestinationPath (Join-Path $ArtifactsRoot "gta-control-center-$Version-framework-dependent.zip") -Force
Compress-Archive -Path "$SelfContained/*" -DestinationPath (Join-Path $ArtifactsRoot "gta-control-center-$Version-self-contained-$Runtime.zip") -Force

Write-Host "Packaging complete: $ArtifactsRoot"
