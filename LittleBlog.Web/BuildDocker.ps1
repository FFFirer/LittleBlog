[CmdletBinding()]
param (
    # 发布模式：Prod:生产，Dev:开发
    [string]
    $Branch,

    # 是否构建Docker
    [string]
    $Tag
)

if ($Branch -eq "") {
    $Branch = "tmp"
}

if ($Tag -eq "") {
    $Tag = "0.0.0"
}

$DockerTag = "littleblog:" + $Branch + "-" + $Tag

docker build -t $DockerTag .