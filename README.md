[English](https://github.com/x-trip/AutoApi/edit/master/README-EN.md)
# AutoApi
 基于[SqlKata Query Builder](https://github.com/sqlkata/querybuilder)的可根据数据表自动生成Restful API的dotnet中间件
## 支持的数据库：
1. MySql
2. SqlServer
3. PostgreSQL
4. Oracle
5. Sqlite
6. Firebird

## 示例
[AutoApi.Demo](https://github.com/x-trip/AutoApi/tree/master/AutoApi.Demo)
## 用法
1. 安装nuget包 *[AutoApi.Core](https://www.nuget.org/packages/AutoApi.Core/)*

   ``` Install-Package AutoApi.Core -Version 1.0.2 ```

   or

   ``` dotnet add package AutoApi.Core --version 1.0.2 ```

2. 在文件Startup.cs中的ConfigureServices方法中添加如下代码：
   ```
   services.AddAutoRestfulApi(new AutoApiOption()
            {
                DbConnectionString = dbConnectionString, //Your database connection string
                DbType = DataType.SqlServer, //Your database type
            });
3. 在文件Startup.cs中的Configure方法中添加如下代码：
   ```
   app.UseAutoRestfulApi();
   ```
