#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using Newtonsoft.Json;

namespace IEIPaperSearch.DataExtractors.Bibtex
{
    public class BibtexJsonDto
    {
        [JsonProperty("type")]
        public string? Type { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("author")]
        public string Authors { get; internal set; }

        [JsonProperty("pages")]
        public string? Pages { get; internal set; }

        [JsonProperty("year")]
        public int Year { get; internal set; }

        [JsonProperty("publisher")]
        public string? Publisher { get; internal set; }

        [JsonProperty("booktitle")]
        public string? BookTitle { get; internal set; }

        [JsonProperty("journal")]
        public string? Journal { get; internal set; }

        [JsonProperty("volume")]
        public string? Volume { get; internal set; }

        [JsonProperty("number")]
        public string? Number { get; internal set; }
    }
}
