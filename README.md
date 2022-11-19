[English](https://github.com/x-trip/MicroApi/blob/master/README-EN.md)

# MicroApi

 基于[SqlKata Query Builder](https://github.com/sqlkata/querybuilder)的可根据数据表自动生成Restful API的dotnet中间件

## 支持的数据库

1. MySql *[MicroApi.MySql](https://www.nuget.org/packages/MicroApi.MySql/)*
2. SqlServer *[MicroApi.SqlServer](https://www.nuget.org/packages/MicroApi.SqlServer/)*
3. PostgreSQL *[MicroApi.PostgreSQL](https://www.nuget.org/packages/MicroApi.PostgreSQL/)*
4. Oracle *[MicroApi.Oracle](https://www.nuget.org/packages/MicroApi.Oracle/)*
5. Sqlite *[MicroApi.Sqlite](https://www.nuget.org/packages/MicroApi.Sqlite/)*
6. Firebird *[MicroApi.Firebird](https://www.nuget.org/packages/MicroApi.Firebird/)*

## 示例

[MicroApi.Demo](https://github.com/x-trip/MicroApi/tree/master/MicroApi.Demo)

## 用法

1. 安装nuget包 *[MicroApi.Core](https://www.nuget.org/packages/MicroApi.Core/)*

   ``` Install-Package MicroApi.Core -Version 1.0.0 ```

   or

   ``` dotnet add package MicroApi.Core --version 1.0.0 ```
2. 安装数据库对应类型的nuget包，比SQL Server安装 *[MicroApi.SqlServer](https://www.nuget.org/packages/MicroApi.SqlServer/)*
   
   ``` Install-Package MicroApi.SqlServer -Version 1.0.0 ```

   or

   ``` dotnet add package MicroApi.SqlServer --version 1.0.0 ```
3. 在文件Startup.cs中的ConfigureServices方法中添加如下代码：

   ```
   services.AddMicroApi()
           .UseSqlServer(connectionString);//Your sql server database connection string
   ```

4. 在文件Startup.cs中的Configure方法中添加如下代码：

   ```
   app.UseMicroApi();
   ```
## API内置格式
生成api的访问路径统一为：/api/{TableName}
### GET
**暂不支持/api/{TableName}/{PrimaryKey}这种路由**，后续版本考虑增加表名以及字段名称自定义映射功能后会支持。当前版本主要支持以下功能：

**分页查询**

接口路径为：/api/{TableName}?page=&size= 或者 /api/{TableName}?offset=&limit= 两种方式

**排序**

1. 升序接口路径为：/api/{TableName}?orderAsc={列名1,列名2,列名3,.....} 
2. 降序接口路径为：/api/{TableName}?orderDesc={列名1,列名2,列名3,.....}

**特殊条件查询，比如大于等于、小于等于、大于、小于、IN、LIKE查询**

1. 大于等于：/api/{TableName}?{列名}.ge={值}
2. 大于：/api/{TableName}?{列名}.gt={值}
3. 小于等于：/api/{TableName}?{列名}.le={值}
4. 小于：/api/{TableName}?{列名}.lt={值}
5. IN: /api/{TableName}?{列名}.in={值}
6. LIKE: /api/{TableName}?{列名}.like={值}

### POST
用于新增数据，访问路径后面跟的参数无效
### PUT
用于更新数据，可按查询条件进行批量更新。查询条件支持GET中的特殊条件查询，比如大于等于、小于等于、大于、小于、IN、LIKE查询
### DELETE
用于删除数据，可按查询条件进行批量删除。查询条件支持GET中的特殊条件查询，比如大于等于、小于等于、大于、小于、IN、LIKE查询
