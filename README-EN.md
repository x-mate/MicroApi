[中文](https://github.com/x-trip/AutoApi/blob/master/README.md)
# AutoApi
A dotnet middleware based on [SqlKata Query Builder](https://github.com/sqlkata/querybuilder) that can automatically generate restful API according to the database
## Supported databases：
1. MySql *[AutoApi.MySql](https://www.nuget.org/packages/AutoApi.MySql/)*
2. SqlServer *[AutoApi.SqlServer](https://www.nuget.org/packages/AutoApi.SqlServer/)*
3. PostgreSQL *[AutoApi.PostgreSQL](https://www.nuget.org/packages/AutoApi.PostgreSQL/)*
4. Oracle *[AutoApi.Oracle](https://www.nuget.org/packages/AutoApi.Oracle/)*
5. Sqlite *[AutoApi.Sqlite](https://www.nuget.org/packages/AutoApi.Sqlite/)*
6. Firebird *[AutoApi.Firebird](https://www.nuget.org/packages/AutoApi.Firebird/)*
## Sample
[AutoApi.Demo](https://github.com/x-trip/AutoApi/tree/master/AutoApi.Demo)
## How to use
1.  Add nuget package *[AutoApi.Core](https://www.nuget.org/packages/AutoApi.Core/)*

    ``` Install-Package AutoApi.Core -Version 1.0.2 ```

    or

    ``` dotnet add package AutoApi.Core --version 1.0.2 ```
2. Add database nuget package, such as *[AutoApi.SqlServer](https://www.nuget.org/packages/AutoApi.SqlServer/)*
   ``` Install-Package AutoApi.SqlServer -Version 1.0.0 ```

   or

   ``` dotnet add package AutoApi.SqlServer --version 1.0.0 ```

3. Add the following code to the **configureservices** method in the file *startup.cs*:

   ```
   services.AddAutoRestfulApi()
           .UseSqlServer(connectionString);//Your sql server database connection string
   ```

4. Add the following code to the **Configure** method in the file *startup.cs*:
   ```
   app.UseAutoRestfulApi();
   ```
