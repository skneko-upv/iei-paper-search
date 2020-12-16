#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

using Newtonsoft.Json;

namespace IEIPaperSearch.DataExtractors.IeeeXplore
{
    internal class SubmissionJsonDto
    {
        [JsonProperty("content_type")]
        public string ContentType { get; internal set; }

        [JsonProperty("title")]
        public string Title { get; internal set; }

        [JsonProperty("authors")]
        public AuthorWrapperJsonDto Authors { get; internal set; }

        [JsonProperty("start_page")]
        public string StartPage { get; internal set; }

        [JsonProperty("end_page")]
        public string EndPage { get; internal set; }

        [JsonProperty("publication_year")]
        public int PublicationYear { get; internal set; }

        [JsonProperty("publication_title")]
        public string PublicationTitle { get; internal set; }

        [JsonProperty("volume")]
        public string Volume { get; internal set; }

        [JsonProperty("publication_number")]
        public string PublicationNumber { get; internal set; }

        [JsonProperty("pdf_url")]
        public string PdfUrl { get; internal set; }

        [JsonProperty("conference_dates")]
        public string? ConferenceDates { get; internal set; }

        [JsonProperty("publication_date")]
        public string? PublicationDate { get; internal set; }

        [JsonProperty("publisher")]
        public string? Publisher { get; internal set; }
    }
}
