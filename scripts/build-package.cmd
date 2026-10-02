@echo off
setlocal

set VERSION=%1
if "%VERSION%"=="" set VERSION=0.1.0-beta.1

pwsh -ExecutionPolicy Bypass -File "%~dp0build-package.ps1" -Version %VERSION%
if errorlevel 1 exit /b %errorlevel%

echo Build/package completed.
endlocal
