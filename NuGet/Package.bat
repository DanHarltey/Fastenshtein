rmdir /s /q ".\Package\"
rmdir /s /q "..\Fastenshtein\bin\"
rmdir /s /q "..\FastenshteinPcl\bin\"

"C:\Program Files (x86)\MSBuild\14.0\bin\amd64\msbuild.exe" ..\Fastenshtein\Fastenshtein.csproj /p:Configuration=Release;TargetFrameworkVersion=v4.0;TargetFrameworkProfile=Client
xcopy ..\Fastenshtein\bin\Release\Fastenshtein.dll .\Package\lib\net40-client\

"C:\Program Files (x86)\MSBuild\14.0\bin\amd64\msbuild.exe" ..\FastenshteinPcl\FastenshteinPcl.csproj /p:Configuration=Release
xcopy ..\FastenshteinPcl\bin\Release\Fastenshtein.dll .\Package\lib\portable-net40+sl5+win8+wpa+wp8\

copy Fastenshtein.dll.nuspec .\Package\
C:\NuGet.exe pack .\Package\Fastenshtein.dll.nuspec
@pause