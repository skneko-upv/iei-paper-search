#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using Newtonsoft.Json;
using System.Collections.Generic;

namespace IEIPaperSearch.DataExtractors.IeeeXplore
{
    internal class AuthorJsonDto
    {
        [JsonProperty("full_name")]
        public string FullName { get; internal set; }
    }

    internal class AuthorWrapperJsonDto
    {
        [JsonProperty("authors")]
        public ICollection<AuthorJsonDto> Authors { get; internal set; }
    }
}