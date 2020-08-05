using Newtonsoft.Json;

namespace JHipsterNet.Core.Pagination {
    public class PageResponse {
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("isFirst")]
        public bool IsFirst { get; set; }

        [JsonProperty("isLast")]
        public bool IsLast { get; set; }

        [JsonProperty("hasNext")]
        public bool HasNext { get; set; }

        [JsonProperty("hasPrevious")]
        public bool HasPrevious { get; set; }
    }
}
