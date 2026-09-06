# Auto-generated build script by Sustainer Agent for version 1.0.1.0
$ErrorActionPreference = "Stop"
$csproj = "GTAVNews.csproj"
Write-Host "Building $csproj for version 1.0.1.0..."

$params = @(
    $csproj,
    "/p:Configuration=Release",
    "/p:Platform=x64",
    "/p:AppxBundle=Always",
    "/p:UapAppxPackageBuildMode=Store",
    "/p:AppxPackageSigningEnabled=false",
    "/verbosity:normal"
)

Write-Host "Running MSBuild..."
& "MSBuild.exe" @params
