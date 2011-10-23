properties {
    $base_dir = resolve-path .
    $nuget = "$base_dir\packages\Nuget.CommandLine.1.5.21005.9019\tools\Nuget.exe"
    $msbuild = "$env:WINDIR\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
    $config = 'Release'
    $platform = 'AnyCPU'
    $build_dir = "$base_dir\bin\$config\"
    $release_dir = "$base_dir\release\"
    $project = "$base_dir\BuzzIO.csproj"
    $dll_file = "$build_dir\BuzzIO.dll"
    $nuspec_file = "$base_dir\BuzzIO.nuspec"
}

task default -depends Package

task Clean {
    remove-item -force -recurse $build_dir -ErrorAction SilentlyContinue
    remove-item -force -recurse $release_dir -ErrorAction SilentlyContinue
}

task Init -depends Clean {
    new-item $build_dir -itemType directory
    new-item $release_dir -itemType directory
}

task Build -depends Init {
    & $msbuild $project /p:OutDir=$build_dir /p:Configuration=$config /p:Platform=$platform
    if ($LastExitCode -ne 0) {
        Write-Error "build failed"
    }
}

task Package -depends Build {
    $version = (dir $dll_file).VersionInfo.FileVersion
    & $nuget pack $nuspec_file -Symbols -OutputDirectory $release_dir -Version $version
    if ($LastExitCode -ne 0) {
        Write-Error "package failed"
    }
}

