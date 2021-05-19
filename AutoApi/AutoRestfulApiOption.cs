using System.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace AutoApi
{
    public class AutoRestfulApiOption
    {
        /// <summary>
        /// 是否启用限流
        /// </summary>
        public bool EnableRateLimit { get; set; } = true;

        public IConfiguration Configuration { get; set; }

        public RateLimitType RateLimitType { get; set; } = RateLimitType.Ip;

        public RateLimitStoreType RateLimitStoreType { get; set; } = RateLimitStoreType.Memcache;
    }

    public enum RateLimitType
    {
        /// <summary>
        /// 基于客户端IP速率限制
        /// </summary>
        [Description("基于客户端IP速率限制")]
        Ip,
        /// <summary>
        /// 基于客户端ID速率限制
        /// </summary>
        [Description("基于客户端ID速率限制")]
        Client
    }

    public enum RateLimitStoreType
    {
        /// <summary>
        /// Memcache
        /// </summary>
        [Description("Memcache")]
        Memcache,
        /// <summary>
        /// Redis, 适用于分布式场景
        /// </summary>
        [Description("Redis，适用于分布式场景")]
        Redis
    }
}
