$currentFolder = ($PWD).Path
$wwwrootPath = Join-Path -Path $currentFolder -ChildPath "/LittleBlog.Web/wwwroot/" 
$webProjPath = Join-Path -Path $currentFolder -ChildPath "/LittleBlog.Web/LittleBlog.Web.csproj"
$outputPath = Join-Path -Path $currentFolder -ChildPath "/published"
$appName = "/admin"
$appPath = Join-Path -Path $wwwrootPath -ChildPath $appName
$distPath = Join-Path -Path $currentFolder -ChildPath "/LittleBlog.Admin/dist/"
$adminProjPath = Join-Path -Path $currentFolder -ChildPath "/LittleBlog.Admin"

Set-Location $adminProjPath
Write-Output ("IN:" + $adminProjPath)
# npm run build-dev

$HAS_DIR = (Test-Path $appPath)
if (-not $HAS_DIR) {
    New-Item -Path $appPath -ItemType Directory
    Write-Output ("Create:" + $appPath)
}

Write-Output ("REMOVE IN:" + $appPath)
Remove-Item -Path $appPath -Recurse

Write-Output ("COPY TO:" + $appPath)
# 拷贝dist的内容到
Copy-Item $distPath $appPath -Recurse

Set-Location $currentFolder

# 发布
dotnet publish $webProjPath -c Release -o $outputPath