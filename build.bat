dotnet "C:\Program Files\dotnet\sdk\7.0.401\MSBuild.dll" /p:Configuration=Release
mkdir Release
cd ./Release
mkdir Sucrose
cp ../Sucrose/bin/Release/Sucrose.dll ./Sucrose/Sucrose.dll

cp ../Info.json ./Sucrose/Info.json
tar -acf Sucrose-Release.zip Sucrose
mv Sucrose-Release.zip ../
cd ../
rm -rf Release
pause
