using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.Bibtex
{
    internal class BibtexDataExtractor : AbstractSubmissionDataExtractor<SubmissionJsonDto>
    {
        public BibtexDataExtractor(PaperSearchContext context) : base(context)
        { }

        protected override ICollection<SubmissionJsonDto> DeserializeJson(JObject root)
        {
            var articlesJson = GetArrayOrObject(root["articles"]!);
            var booksJson = GetArrayOrObject(root["books"]!);
            var inProceedingsJson = GetArrayOrObject(root["inproceedings"]!);

            IList<SubmissionJsonDto> articlesDtos = articlesJson.Select(a => {
                var dto = a.ToObject<SubmissionJsonDto>()!;
                dto.Type = "article";
                return dto;
            }).ToList();
            IList<SubmissionJsonDto> booksDtos = booksJson.Select(a => {
                var dto = a.ToObject<SubmissionJsonDto>()!;
                dto.Type = "book";
                return dto;
            }).ToList();
            IList<SubmissionJsonDto> inProceedingsDtos = inProceedingsJson.Select(a => {
                var dto = a.ToObject<SubmissionJsonDto>()!;
                dto.Type = "inproceedings";
                return dto;
            }).ToList();

            IList<SubmissionJsonDto> dtos = articlesDtos.Union(booksDtos).Union(inProceedingsDtos).ToList();

            return dtos;
        }

        private static IList<JToken> GetArrayOrObject(dynamic jsonObject) => 
            jsonObject is JArray ? ((JToken)jsonObject).Children().ToList() : new List<JToken>() { jsonObject };

        protected override void ToCanonicalModel(ICollection<SubmissionJsonDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var pages = ToPagePair(dto.Pages);

                switch (dto.Type) {
                    case "article":
                        {
                            var article = new Article(
                                dto.Title,
                                dto.Year,
                                url: null,
                                startPage: pages.Item1,
                                endPage: pages.Item2)
                            {
                                Authors = MakeAuthors(ToAuthorList(dto.Authors)),
                                PublishedIn = MakeIssueAndJournal(dto.Volume, dto.Number, month: null, dto.Journal)
                            };

                            articles.Add(article);
                            break;
                        }
                    case "book":
                        {
                            var book = new Book(
                                dto.Title,
                                dto.Year,
                                url: null,
                                dto.Publisher!)
                            {
                                Authors = MakeAuthors(ToAuthorList(dto.Authors))
                            };

                            books.Add(book);
                            break;
                        }
                    case "inproceedings":
                        {
                            var inProceedingsElement = new InProceedings(
                                dto.Title,
                                dto.Year,
                                url: null,
                                dto.BookTitle!,
                                edition: null,
                                startPage: pages.Item1 is null ? null : pages.Item1,
                                endPage: pages.Item2 is null ? null : pages.Item2)
                            {
                                Authors = MakeAuthors(ToAuthorList(dto.Authors))
                            };

                            inProceedings.Add(inProceedingsElement);
                            break;
                        }
                }
            }
        }

        private static (string?, string?) ToPagePair(string? pages)
        {
            if (pages is null)
            {
                return (null, null);
            }

            var tokens = pages.Split("--")
                .Select(s => s.Trim())
                .ToList();

            return (tokens[0], tokens.Count > 1 ? tokens[1] : null);
        }

        private static ICollection<string> ToAuthorList(string authors) => 
            authors.Split(',')
                .Select(p => p.Trim())
                .ToList();
    }
}