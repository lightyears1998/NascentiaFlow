# 发布

``` shell
cd NascentiaFlow.Desktop
dotnet publish -c Release -r win-x64

# itch.io
cd bin/Release/net10.0/win-x64/publish
butler push . lightyears1998/ego-primer:windows
```
