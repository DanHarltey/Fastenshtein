rmdir /s /q ".\Package\"
rmdir /s /q "..\Fastenshtein\bin\"
rmdir /s /q "..\FastenshteinFramework\bin\"
rmdir /s /q "..\FastenshteinPcl\bin\"

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" ..\Fastenshtein\Fastenshtein.csproj /p:Configuration=Release

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" ..\FastenshteinFramework\FastenshteinFramework.csproj /p:Configuration=Release;TargetFrameworkVersion=v4.0;TargetFrameworkProfile=Client

"C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\msbuild.exe" ..\FastenshteinPcl\FastenshteinPcl.csproj /p:Configuration=Release

C:\NuGet.exe pack Fastenshtein.dll.nuspec
@pause