﻿#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using Newtonsoft.Json;

namespace IEIPaperSearch.DataExtractors.BDLP
{
    internal class ArticleJsonDto
    {
        [JsonProperty("title")]
        public dynamic Title { get; internal set; }

        [JsonProperty("author")]
        public dynamic? Authors { get; internal set; }
        
        [JsonProperty("pages")] 
        public string? Pages { get; internal set; }
        
        [JsonProperty("year")]
        public int Year { get; internal set; }
        
        [JsonProperty("ee")]
        public dynamic? Ee { get; internal set; }
        
        [JsonProperty("journal")]
        public string Journal { get; internal set; }
        
        [JsonProperty("volume")]
        public string? Volume { get; internal set; }
        
        [JsonProperty("number")]
        public string? Number { get; internal set; }
    }
}
