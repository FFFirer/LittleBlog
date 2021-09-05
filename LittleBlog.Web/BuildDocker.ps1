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

$DATE = Get-Date -Format "yyyyMMddHHmmssfff"

$DockerTag = "littleblog:" + $Branch + "-" + $Tag + "-" + $DATE

Write-Host ("DOCKER TAG: ", $DockerTag)

docker build -t $DockerTag .