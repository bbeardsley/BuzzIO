@echo off
if "%1" == "" goto error

nuget push bin\BuzzIO.%1.nupkg
goto end

:error
echo You need to specify the version number to push (ie: 1.0.2.0)

:end