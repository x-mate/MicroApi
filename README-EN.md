[中文](https://github.com/x-trip/AutoApi/blob/master/README.md)
# AutoApi
A dotnet middleware based on [SqlKata Query Builder](https://github.com/sqlkata/querybuilder) that can automatically generate restful API according to the database
## Supported databases：
1. MySql
2. SqlServer
3. PostgreSQL
4. Oracle
5. Sqlite
6. Firebird
## Sample
[AutoApi.Demo](https://github.com/x-trip/AutoApi/tree/master/AutoApi.Demo)
## How to use
1.  Add nuget package *[AutoApi.Core](https://www.nuget.org/packages/AutoApi.Core/)*

    ``` Install-Package AutoApi.Core -Version 1.0.2 ```

    or

    ``` dotnet add package AutoApi.Core --version 1.0.2 ```

2.  Add the following code to the **configureservices** method in the file *startup.cs*:
    ```
    services.AddAutoRestfulApi(new AutoApiOption()
            {
                DbConnectionString = dbConnectionString, //Your database connection string
                DbType = DataType.SqlServer, //Your database type
            });
    ```
3. Add the following code to the **Configure** method in the file *startup.cs*:
   ```
   app.UseAutoRestfulApi();
   ```
