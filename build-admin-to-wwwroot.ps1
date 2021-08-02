[CmdletBinding()]
param (
    # 发布模式：Prod:生产，Dev:开发
    [Parameter()]
    [string]
    $mode,

    # 是否构建Docker
    [Parameter()]
    [bool]
    $build_docker
)

$NPM_BUILD_CMD = "build";

if ($mode -eq "") {
    $mode = "Prod"
}

if ($mode -eq "Prod") {
    $NPM_BUILD_CMD = "build-prod"
}

if ($mode -eq "Dev") {
    $NPM_BUILD_CMD = "build-dev"
}

$CURRENT_DIR = ($PWD).Path
$WWWROOT_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Web/wwwroot/" 
$WEB_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Web/LittleBlog.Web.csproj"
$PUBLISHED_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/published"
$ADMIN_APP_NAME = "/admin"
$ADMIN_APP_DIR = Join-Path -Path $WWWROOT_DIR -ChildPath $ADMIN_APP_NAME
$ADMIN_APP_DIST_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Admin/dist/"
$ADMIN_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Admin"

Set-Location $ADMIN_DIR
Write-Output ("IN:" + $ADMIN_DIR)

npm run $NPM_BUILD_CMD

$HAS_DIR = (Test-Path $ADMIN_APP_DIR)
if (-not $HAS_DIR) {
    New-Item -Path $ADMIN_APP_DIR -ItemType Directory
    Write-Output ("Create:" + $ADMIN_APP_DIR)
}

Write-Output ("REMOVE IN:" + $ADMIN_APP_DIR)
Remove-Item -Path $ADMIN_APP_DIR -Recurse

Write-Output ("COPY TO:" + $ADMIN_APP_DIR)
# 拷贝dist的内容到
Copy-Item $ADMIN_APP_DIST_DIR $ADMIN_APP_DIR -Recurse

Set-Location $CURRENT_DIR
. .\base.ps1

# 发布
dotnet publish $WEB_DIR -c Release -o $PUBLISHED_DIR

if ($build_docker) {
    Write-Output "START TO BUILD <docker>"

    $BranchName = Get-GitBranchName

    $TagName = Get-GitTag

    # 构建镜像
    Set-Location $PUBLISHED_DIR

    $BuildDockerCmd = ".\BuildDocker.ps1 --Branch " + $BranchName + " --Tag " + $TagName

    # 调用构建Docker的脚本
    Invoke-Expression -Command $BuildDockerCmd
}