using System.Security.AccessControl;
using static Store_Utility.SD;

namespace eStore.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string Token { get; set; }
        public string Search { get; set; }
        public int PageSize { get; set; } = 0;
        public int PageNumber { get; set; } = 1;
        //public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
