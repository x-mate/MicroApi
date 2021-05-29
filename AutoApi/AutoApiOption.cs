using System.ComponentModel;
using FreeSql;
using Microsoft.Extensions.Configuration;

namespace AutoApi
{
    public class AutoApiOption
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataType DbType { get; set; } = DataType.SqlServer;
        /// <summary>
        /// 主数据库连接字符串
        /// </summary>
        public string DbMasterConnectionString { get; set; }
        /// <summary>
        /// 从数据库连接字符串数组
        /// </summary>
        public string[] DbSlaveConnectionStrings { get; set; }
        /// <summary>
        /// 开启GET请求返回值从redis缓存读取，默认关闭。
        /// </summary>
        public bool EnableGetCache { get; set; } = false;
        /// <summary>
        /// Redis连接字符串数组
        /// </summary>
        public string[] RedisConnectionStrings { get; set; }
        /// <summary>
        /// 缓存有效期，单位：秒，默认1800秒；
        /// </summary>
        public int CacheExpiredSeconds { get; set; } = 1800;
    }
}
