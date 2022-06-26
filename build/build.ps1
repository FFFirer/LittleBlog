[CmdletBinding()]
param (
    # 发布模式：Prod:生产，Dev:开发
    [Parameter()]
    [string]
    $mode,

    # 是否构建Docker
    [Parameter()]
    [switch]
    [bool]
    $buildDocker,

    # API地址
    [Parameter()]
    [string]
    $apiAddress,

    # admin路径前缀
    [Parameter()]
    [string]
    $adminPrefix
)

$NPM_BUILD_CMD = "build";

if ($apiAddress -eq "") {
    $apiAddress = "/"
}

if ($adminPrefix -eq "" ) {
    $adminPrefix = "/"
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

$OUTPUTS_FOLDER_NAME = "outputs"

$CURRENT_DIR = ($PWD).Path

$SLN_FOLDER = Join-Path -Path $CURRENT_DIR -ChildPath "../server/"
$PROJ = Join-Path $SLN_FOLDER "LittleBlog.Web/LittleBlog.Web.csproj"

$ADMIN_APP_DIST_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "../client/LittleBlog.Admin/dist/"
$ADMIN_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "../client/LittleBlog.Admin"

# 构建输出目录
$OUTPUTS_FOLDER = Join-Path $CURRENT_DIR $OUTPUTS_FOLDER_NAME

Remove-Item -Path $OUTPUTS_FOLDER -Force -Recurse -ErrorAction Ignore
New-Item -Path $OUTPUTS_FOLDER -ItemType Directory

# =======
# 发布后端
# =======
Set-Location $SLN_FOLDER

# 发布站点到发布目录
$BuildWebCommand = "dotnet publish {0} -c Release -o {1}" -f $PROJ, $OUTPUTS_FOLDER
Invoke-Expression -Command $BuildWebCommand -ErrorAction "Stop"

# 清理wwwroot/admin
$outputAdminFolder = Join-Path $OUTPUTS_FOLDER "wwwroot/admin"
Remove-Item -Path $outputAdminFolder -Force -Recurse -ErrorAction Ignore
New-Item -ItemType Directory -Path $outputAdminFolder

# ============================================
# 进入前端admin的目录，准备将其发布到web的wwwroot下
# ============================================
Set-Location $ADMIN_DIR
Write-Output "IN: $ADMIN_DIR"

# 构建新的环境配置文件，从.env.example
$ENV_EXAMPLE_FILE_PATH = Join-Path -Path $ADMIN_DIR -ChildPath ".env.example"
$TARGET_ENV_EXAMPLE_FILE_PATH = Join-Path -Path $ADMIN_DIR -ChildPath $TARGET_ENV_FILENAME

$ENV_EXAMPLE_CONTENT = Get-Content $ENV_EXAMPLE_FILE_PATH

# 替换内容
$ENV_EXAMPLE_CONTENT = $ENV_EXAMPLE_CONTENT -replace "@API_ADDRESS", $apiAddress
$ENV_EXAMPLE_CONTENT = $ENV_EXAMPLE_CONTENT -replace "@APP_NAME", $adminPrefix

Write-Output "API Address: $apiAddress"
Write-Output "Admin Prefix: $adminPrefix"

Set-Content -Path $TARGET_ENV_EXAMPLE_FILE_PATH -Value $ENV_EXAMPLE_CONTENT
Write-Output "Generated: $TARGET_ENV_EXAMPLE_FILE_PATH"

yarn     # 拉取最新的库

# $BuildVueCommand = "yarn run " + $NPM_BUILD_CMD + " -ErrorAction Stop"
# Invoke-Expression -Command $BuildVueCommand -ErrorAction "Stop"     # 发生错误时退出
yarn run $NPM_BUILD_CMD -ErrorAction "Stop"

# 拷贝dist的内容到
Copy-Item (Join-Path $ADMIN_APP_DIST_DIR "*") $outputAdminFolder -Recurse
Write-Output "[复制LittleBlog.Admin]"

Set-Location $CURRENT_DIR
. ./base.ps1

if ($buildDocker) {
    Write-Output "[构建Docker]"

    Set-Location $OUTPUTS_FOLDER

    Write-Output "START TO BUILD <docker>"

    Write-Output "Branch: $BranchName"

    $TagName = Get-GitTag

    Write-Output "Tag: $TagName"

    # 构建镜像
    Set-Location $OUTPUTS_FOLDER

    $imageName = "littleblog:$($TagName)"

    $BuildDockerCmd = "docker build -t $($imageName) ."

    Write-Output "Build docker command:  $BuildDockerCmd"

    # 调用构建Docker的脚本
    Invoke-Expression -Command $BuildDockerCmd

    # 清理镜像
    Invoke-Expression -Command "docker image prune -y"
}

# 返回原来的目录
Set-Location $CURRENT_DIR
