using System.Text.Json.Serialization;

namespace AntXafSiteTemplate.Authentications
{
    internal class ODataEnity<T>
    {
        [JsonPropertyName("@odata.context")]
        public string? OdataContext { get; set; }

        [JsonPropertyName("@odata.count")]
        public int Count { get; set; }

        [JsonPropertyName("value")]
        public List<T>? Value { get; set; }
    }
}
