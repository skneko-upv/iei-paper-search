using BibTeXLibrary;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.BDLP
{
    public class BibtexJsonConverter
    {
        public string BibtexToJson(string bibtex)
        {
            var parser = new BibParser(new StringReader(bibtex));
            var entries = parser.GetAllResult()
                .Select(e => ToPropertyDictionary(e))
                // Adapt to expected JSON format:
                //  Filter out properties that are an empty string and make keys lowercase
                .Select(d => d.Where(p => !string.IsNullOrEmpty(p.Value))
                    .ToDictionary(p => p.Key.ToLower(), p => p.Value));

            var articles = entries
                .Where(e => e["type"] == "Article")
                .ToList();
            var books = entries
                .Where(e => e["type"] == "Book")
                .ToList();
            var inproceedings = entries
                .Where(e => e["type"] == "InProceedings")
                .ToList();

            return JsonConvert.SerializeObject(new {
                articles,
                books,
                inproceedings
            });
        }

        public string BibtexToJson(IEnumerable<string> bibtex) =>
            BibtexToJson(string.Join("\r\n\r\n", bibtex));

        static IDictionary<string, string?> ToPropertyDictionary(BibEntry entry) =>
           entry.GetType()
            .GetProperties()
            .Where(p => p.CanRead && p.PropertyType == typeof(string) && p.GetIndexParameters().Length == 0)
            .ToDictionary(p => p.Name, p => p.GetValue(entry, null) as string);
    }
}
