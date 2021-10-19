[CmdletBinding()]
param (
    # 发布模式：Prod:生产，Dev:开发
    [Parameter()]
    [string]
    $mode,

    # 是否构建Docker
    [Parameter()]
    [bool]
    $build_docker,

    # API地址
    [Parameter()]
    [string]
    $api_address,

    # admin路径前缀
    [Parameter()]
    [string]
    $admin_prefix
)

$NPM_BUILD_CMD = "build";

if ($api_address -eq "") {
    $api_address = "/"
}

if ($admin_prefix -eq "" ) {
    $admin_prefix = "/"
}

$TARGET_ENV_FILENAME = ".env.staging.local"

if ($mode -eq "") {
    $mode = "Prod"
}

if ($mode -eq "prod") {
    $NPM_BUILD_CMD = "build-prod"
    $TARGET_ENV_FILENAME = ".env.prod.local"
}

if ($mode -eq "dev") {
    $NPM_BUILD_CMD = "build-dev"
    $TARGET_ENV_FILENAME = "env.dev.local"
}

$CURRENT_DIR = ($PWD).Path
$WWWROOT_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Web/wwwroot/" 
$WEB_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Web/LittleBlog.Web.csproj"
$PUBLISHED_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/published"
$ADMIN_APP_NAME = "/admin"
$ADMIN_APP_DIR = Join-Path -Path $WWWROOT_DIR -ChildPath $ADMIN_APP_NAME
$ADMIN_APP_DIST_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Admin/dist/"
$ADMIN_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Admin"

# 进入后端admin的目录，准备将其发布到web的wwwroot下
Set-Location $ADMIN_DIR
Write-Host ("IN:" + $ADMIN_DIR)

# 构建新的环境配置文件，从.env.example
$ENV_EXAMPLE_FILE_PATH = Join-Path -Path $ADMIN_DIR -ChildPath ".env.example"
$TARGET_ENV_EXAMPLE_FILE_PATH = Join-Path -Path $ADMIN_DIR -ChildPath $TARGET_ENV_FILENAME

$ENV_EXAMPLE_CONTENT = Get-Content $ENV_EXAMPLE_FILE_PATH

# 替换内容
$ENV_EXAMPLE_CONTENT = $ENV_EXAMPLE_CONTENT -replace "@API_ADDRESS", $api_address
$ENV_EXAMPLE_CONTENT = $ENV_EXAMPLE_CONTENT -replace "@APP_NAME", $admin_prefix

Write-Host ("API Address: ", $api_address)
Write-Host ("Admin Prefix: ", $admin_prefix)

Set-Content -Path $TARGET_ENV_EXAMPLE_FILE_PATH -Value $ENV_EXAMPLE_CONTENT
Write-Host ("Generated: ", $TARGET_ENV_EXAMPLE_FILE_PATH)

npm install     # 拉取最新的库

$BuildVueCommand = "npm run " + $NPM_BUILD_CMD
Invoke-Expression -Command $BuildVueCommand -ErrorAction "Stop"     # 发生错误时退出

$HAS_DIR = (Test-Path $ADMIN_APP_DIR)
if (-not $HAS_DIR) {
    New-Item -Path $ADMIN_APP_DIR -ItemType Directory
    Write-Host ("Create:" + $ADMIN_APP_DIR)
}

Write-Host ("REMOVE IN:" + $ADMIN_APP_DIR)
Remove-Item -Path $ADMIN_APP_DIR -Recurse

Write-Host ("COPY TO:" + $ADMIN_APP_DIR)

# 拷贝dist的内容到
Copy-Item $ADMIN_APP_DIST_DIR $ADMIN_APP_DIR -Recurse

Set-Location $CURRENT_DIR
. ./base.ps1

# 发布站点到发布目录
$BuildWebCommand = "dotnet publish {0} -c Release -o {1}" -f $WEB_DIR, $PUBLISHED_DIR
Invoke-Expression -Command $BuildWebCommand -ErrorAction "Stop"

if ($build_docker) {
    Write-Host "START TO BUILD <docker>"

    # 获取GIT版本信息
    $BranchName = Get-GitBranchName

    Write-Host ("Branch: ", $BranchName)

    $TagName = Get-GitTag

    Write-Host ("Tag: ", $TagName)

    # 构建镜像
    Set-Location $PUBLISHED_DIR

    $BuildDockerCmd = ".\BuildDocker.ps1 -Branch " + $BranchName + " -Tag " + $TagName

    Write-Host ("Build docker command: ", $BuildDockerCmd)

    # 调用构建Docker的脚本
    Invoke-Expression -Command $BuildDockerCmd

    # 清理镜像
    Invoke-Expression -Command "docker image prune -y"
}

# 返回原来的目录
Set-Location $CURRENT_DIR
