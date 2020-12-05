dotnet restore ..\ || goto :error
dotnet build ..\src\Fastenshtein --configuration Release --framework netstandard1.0 --nologo --no-restore || goto :error
dotnet build ..\benchmarks\Fastenshtein.Benchmarking --configuration Release --framework net5.0 --nologo --no-restore || goto :error
dotnet build ..\tests\Fastenshtein.Tests --configuration Release --framework net5.0 --nologo --no-restore || goto :error
dotnet test ..\tests\Fastenshtein.Tests --configuration Release --framework net5.0 --nologo --no-build --no-restore --verbosity normal || goto :error
..\benchmarks\Fastenshtein.Benchmarking\bin\Release\net5.0\Fastenshtein.Benchmarking.exe || goto :error

goto :EOF

:error
echo Failed with error #%errorlevel%.
exit /b %errorlevel%