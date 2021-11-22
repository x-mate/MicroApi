namespace MicroApi.Core
{
    public class MicroApiOption
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DbType { get; set; } = DataType.SqlServer;
        /// <summary>
        /// 主数据库连接字符串
        /// </summary>
        public string DbConnectionString { get; set; }
    }
}
