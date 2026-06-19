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
dotnet workload restore # android
```

``` shell
# asp dotnet
dotnet dev-certs https --trust
```

## 开发

- 设置环境变量 `NASCENTIA_FLOW_DATA_DIR_NAME=NascentiaFlow Dev` 以便让 dev build 使用与 regular build 不同的数据目录，避免影响生产数据。 
