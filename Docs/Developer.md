# Developer

## 目标平台

- Windows 
- Android
- Linux 
  - Windows Subsystem Linux

## 技术栈

- .NET 10
  - EF Core
- Avalonia 12
- sqlite

## 开发依赖

```shell
dotnet workload install android wasm-tools
```

``` shell
# asp dotnet
dotnet dev-certs https --trust
```

## 发布

``` shell
cd Flow.Desktop
dotnet publish -c Release -r win-x64

# itch.io
cd bin/Release/net9.0/win-x64
butler push . lightyears1998/ego-primer:windows
```
