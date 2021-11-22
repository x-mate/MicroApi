using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroApi.Core
{
    /// <summary>
    /// 来自
    /// </summary>
    public enum DataType
    {
        MySql = 0,
        SqlServer = 1,
        PostgreSQL = 2,
        Oracle = 3,
        Sqlite = 4,
        //
        // 摘要:
        //     Firebird 是一个跨平台的关系数据库，能作为多用户环境下的数据库服务器运行，也提供嵌入式数据库的实现
        Firebird = 5,
    }
}
