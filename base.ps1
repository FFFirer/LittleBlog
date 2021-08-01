function Get-GitBranchName {
    param (
        # 指定git库路径
        [string]
        $Path
    )

    $GitPath = "."

    if (-not $Path -eq "") {
        $GitPath = $Path
    }
    
    $HeadPath = Join-Path -Path $GitPath -ChildPath "/.git/HEAD"

    if (-not (Test-Path $HeadPath)) {
        Write-Error ("指定路径" + $Path + "中没有git存储库")
    }

    $GitHeadFile = Get-Content $HeadPath
    $GitHeadFileLine = [System.Environment]::NewLine
    $GitHeadFileContent = [string]::Join($GitHeadFileLine, $GitHeadFile)
    $GitHeadFIleContentSplits = $GitHeadFileContent.Split("/")
    
    $BranchName = $GitHeadFIleContentSplits[$GitHeadFIleContentSplits.Count - 1]

    Write-Output $BranchName
}