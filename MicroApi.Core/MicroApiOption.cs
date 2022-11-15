using Newtonsoft.Json;

namespace MicroApi.Core
{
    public class MicroApiOption
    {
        public AuthorizeType AuthorizeType { get; set; } = AuthorizeType.None;

        public JsonSerializerSettings JsonSerializerSettings { get; set; }
    }
}
