[CmdletBinding()]
param (
    # 代码分支
    [Parameter()]
    [string]
    $Branch,

    # 容器的标签
    [Parameter()]
    [string]
    $Tag
)

if ($Branch -eq "") {
    $Branch = "tmp"
}

if ($Tag -eq "") {
    $Tag = "0.0.0"
}

$DockerTag = "littleblog:{0}-{1}" -f $Branch, $Tag

Write-Host ("DOCKER TAG: ", $DockerTag)

docker build -t $DockerTag .