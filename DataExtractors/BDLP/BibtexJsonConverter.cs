using Newtonsoft.Json;
using System.Collections.Generic;

namespace IEIPaperSearch.DataExtractors.BDLP
{
    public class BibtexJsonConverter
    {
        public string BibtexToJson(string bibtex)
        {
            var parsed = BibtexLibrary.BibtexImporter.FromString(bibtex);
            return JsonConvert.SerializeObject(parsed);
        }

        public string BibtexToJson(IEnumerable<string> bibtex) =>
            BibtexToJson(string.Join("\n", bibtex));
    }
}
