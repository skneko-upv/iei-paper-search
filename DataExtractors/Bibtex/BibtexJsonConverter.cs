using BibTeXLibrary;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace IEIPaperSearch.DataExtractors.BDLP
{
    public class BibtexJsonConverter
    {
        public string BibtexToJson(string bibtex)
        {
            var parser = new BibParser(new StringReader(bibtex));
            var entries = parser.GetAllResult();
            return JsonConvert.SerializeObject(entries);
        }

        public string BibtexToJson(IEnumerable<string> bibtex) =>
            BibtexToJson(string.Join("\r\n\r\n", bibtex));
    }
}
