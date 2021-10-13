[English](https://github.com/x-trip/AutoApi/blob/master/README-EN.md)

# AutoApi

 基于[SqlKata Query Builder](https://github.com/sqlkata/querybuilder)的可根据数据表自动生成Restful API的dotnet中间件

## 支持的数据库

1. MySql *[AutoApi.MySql](https://www.nuget.org/packages/AutoApi.MySql/)*
2. SqlServer *[AutoApi.SqlServer](https://www.nuget.org/packages/AutoApi.SqlServer/)*
3. PostgreSQL *[AutoApi.PostgreSQL](https://www.nuget.org/packages/AutoApi.PostgreSQL/)*
4. Oracle *[AutoApi.Oracle](https://www.nuget.org/packages/AutoApi.Oracle/)*
5. Sqlite *[AutoApi.Sqlite](https://www.nuget.org/packages/AutoApi.Sqlite/)*
6. Firebird *[AutoApi.Firebird](https://www.nuget.org/packages/AutoApi.Firebird/)*

## 示例

[AutoApi.Demo](https://github.com/x-trip/AutoApi/tree/master/AutoApi.Demo)

## 用法

1. 安装nuget包 *[AutoApi.Core](https://www.nuget.org/packages/AutoApi.Core/)*

   ``` Install-Package AutoApi.Core -Version 1.0.3 ```

   or

   ``` dotnet add package AutoApi.Core --version 1.0.3 ```
2. 安装数据库对应类型的nuget包，比SQL Server安装*[AutoApi.SqlServer](https://www.nuget.org/packages/AutoApi.SqlServer/)*
   ``` Install-Package AutoApi.SqlServer -Version 1.0.0 ```

   or

   ``` dotnet add package AutoApi.SqlServer --version 1.0.0 ```
3. 在文件Startup.cs中的ConfigureServices方法中添加如下代码：

   ```
   services.AddAutoRestfulApi()
           .UseSqlServer(connectionString);//Your sql server database connection string
   ```

4. 在文件Startup.cs中的Configure方法中添加如下代码：

   ```
   app.UseAutoRestfulApi();
   ```
