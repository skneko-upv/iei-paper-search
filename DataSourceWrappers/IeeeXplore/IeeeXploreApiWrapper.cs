using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace IEIPaperSearch.DataSourceWrappers.IeeeXplore
{
    public enum IeeeXploreSubmissionKind
    {
        Articles,
        Books,
        InProceedings
    }

    public class IeeeXploreApiWrapper
    {
        public const string IEEE_XPLORE_API_BASE_URI = "http://ieeexploreapi.ieee.org/api/v1/search/articles";
        public const string IEEE_XPLORE_API_KEY = "efv84mzqq6ydx4dbd59jhdcn";

        private readonly HttpClient client = new HttpClient();
        private readonly string apiKey = IEEE_XPLORE_API_KEY;

        public IeeeXploreApiWrapper(int? timeout = null)
        {
            client.BaseAddress = new Uri(IEEE_XPLORE_API_BASE_URI);
            if (timeout is not null)
            {
                client.Timeout = TimeSpan.FromSeconds((double)timeout);
            }
        }

        public async Task<string> ExtractFromApi(int maxRecords, IeeeXploreSubmissionKind submissionKind)
        {
            var contentType = ContentTypeForSubmissionKind(submissionKind);

            var query = $"?apikey={apiKey}&format=json&max_records={maxRecords}&content_type={contentType}";

            var response = await client.GetAsync(query);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new WebException($"IEEE Xplore API request failed: {(int)response.StatusCode} {response.StatusCode}");
            }
        }

        static string ContentTypeForSubmissionKind(IeeeXploreSubmissionKind submissionKind)
        {
            var contentType = submissionKind switch
            {
                IeeeXploreSubmissionKind.Books => "Books",
                IeeeXploreSubmissionKind.InProceedings => "Conferences",
                _ => "Journals",
            };
            return contentType;
        }
    }
}
