[CmdletBinding()]
param (
    # 发布模式：Prod:生产，Dev:开发
    [Parameter()]
    [string]
    $mode,

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

if ($admin_prefix -eq "") {
    $admin_prefix = "/"
}

$TARGET_ENV_FILENAME = ".env.staging.local"

if ($mode -eq "") {
    $mode = "Prod"
}

if ($mode -eq "Prod") {
    $NPM_BUILD_CMD = "build-prod"
    $TARGET_ENV_FILENAME = ".env.prod.local"
}

if ($mode -eq "Dev") {
    $NPM_BUILD_CMD = "build-dev"
    $TARGET_ENV_FILENAME = ".env.dev.local"
}

$CURRENT_DIR = ($PWD).Path
$WWWROOT_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Web/wwwroot/" 
# $WEB_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Web/LittleBlog.Web.csproj"
# $PUBLISHED_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/published"
$ADMIN_APP_NAME = "/admin"
$ADMIN_APP_DIR = Join-Path -Path $WWWROOT_DIR -ChildPath $ADMIN_APP_NAME
$ADMIN_APP_DIST_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Admin/dist/"
$ADMIN_DIR = Join-Path -Path $CURRENT_DIR -ChildPath "/LittleBlog.Admin"

Set-Location $ADMIN_DIR
Write-Output ("IN:" + $ADMIN_DIR)

# 构建环境配置文件
$TARGET_EXAMPLE_FILE_PATH = Join-Path -Path $ADMIN_DIR -ChildPath ".env.example"
$TARGET_ENV_EXAMPLE_FILE_PATH = Join-Path -Path $ADMIN_DIR -ChildPath $TARGET_ENV_FILENAME

$ENV_EXAMPLE_CONTENT = Get-Content $TARGET_EXAMPLE_FILE_PATH

# 替换内容
$ENV_EXAMPLE_CONTENT = $ENV_EXAMPLE_CONTENT -replace "@API_ADDRESS", $api_address
$ENV_EXAMPLE_CONTENT = $ENV_EXAMPLE_CONTENT -replace "@APP_NAME", $admin_prefix

Write-Host ("API Address: ", $api_address)
Write-Host ("Admin Prefix: ", $admin_prefix)

Set-Content -Path $TARGET_ENV_EXAMPLE_FILE_PATH -Value $ENV_EXAMPLE_CONTENT
Write-Host ("Generated: ", $TARGET_ENV_EXAMPLE_FILE_PATH)

npm install

$BuildVueCommand = "npm run " + $NPM_BUILD_CMD
Invoke-Expression -Command $BuildVueCommand -ErrorAction "Stop"

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
