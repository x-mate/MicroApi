[中文](https://github.com/x-trip/MicroApi/blob/master/README.md)
# MicroApi
A dotnet middleware based on [SqlKata Query Builder](https://github.com/sqlkata/querybuilder) that can automatically generate restful API according to the database
## Supported databases：
1. MySql *[MicroApi.MySql](https://www.nuget.org/packages/MicroApi.MySql/)*
2. SqlServer *[MicroApi.SqlServer](https://www.nuget.org/packages/MicroApi.SqlServer/)*
3. PostgreSQL *[MicroApi.PostgreSQL](https://www.nuget.org/packages/MicroApi.PostgreSQL/)*
4. Oracle *[MicroApi.Oracle](https://www.nuget.org/packages/MicroApi.Oracle/)*
5. Sqlite *[MicroApi.Sqlite](https://www.nuget.org/packages/MicroApi.Sqlite/)*
6. Firebird *[MicroApi.Firebird](https://www.nuget.org/packages/MicroApi.Firebird/)*
## Sample
[MicroApi.Demo](https://github.com/x-trip/MicroApi/tree/master/MicroApi.Demo)
## How to use
1.  Add nuget package *[MicroApi.Core](https://www.nuget.org/packages/MicroApi.Core/)*

    ``` Install-Package MicroApi.Core -Version 1.0.0 ```

    or

    ``` dotnet add package MicroApi.Core --version 1.0.0 ```
2. Add database nuget package, such as *[MicroApi.SqlServer](https://www.nuget.org/packages/MicroApi.SqlServer/)*
   
   ``` Install-Package MicroApi.SqlServer -Version 1.0.0 ```

   or

   ``` dotnet add package MicroApi.SqlServer --version 1.0.0 ```

3. Add the following code to the **configureservices** method in the file *startup.cs*:

   ```
   services.AddAutoRestfulApi()
           .UseSqlServer(connectionString);//Your sql server database connection string
   ```

4. Add the following code to the **Configure** method in the file *startup.cs*:
   ```
   app.UseAutoRestfulApi();
   ```
