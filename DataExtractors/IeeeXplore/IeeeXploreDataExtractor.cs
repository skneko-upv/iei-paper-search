using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.IeeeXplore
{
    internal class IeeeXploreDataExtractor : AbstractSubmissionDataExtractor<SubmissionJsonDto>
    {
        public IeeeXploreDataExtractor(PaperSearchContext context) : base(context)
        { }

        protected override ICollection<SubmissionJsonDto> DeserializeJson(JObject root)
        {
            IList<JToken> submissionsJson = root["articles"]!.Children().ToList();

            IList<SubmissionJsonDto> submissionsDtos = submissionsJson.Select(s => s.ToObject<SubmissionJsonDto>()!).ToList();

            return submissionsDtos;
        }

        protected override void ToCanonicalModel(ICollection<SubmissionJsonDto> dtos)
        {
            foreach (var dto in dtos)
            {
                switch (dto.ContentType)
                {
                    case "Early Access Articles":
                    case "Journals":
                        {
                            var article = new Article(
                                dto.Title,
                                dto.PublicationYear,
                                dto.PdfUrl,
                                startPage: dto.StartPage,
                                endPage: dto.EndPage)
                            {
                                Authors = MakeAuthors(ToAuthorsList(dto.Authors.Authors)),
                                PublishedIn = MakeIssueAndJournal(
                                    dto.Volume,
                                    dto.PublicationNumber,
                                    month: ToMonth(dto.PublicationDate),
                                    dto.PublicationTitle)
                            };

                            articles.Add(article);
                            break;
                        }
                    case "Conferences":
                        {
                            var inProceedingsElement = new InProceedings(
                                dto.Title,
                                dto.PublicationYear,
                                dto.PdfUrl,
                                conference: dto.PublicationTitle,
                                edition: dto.ConferenceDates,
                                dto.StartPage,
                                dto.EndPage)
                            {
                                Authors = MakeAuthors(ToAuthorsList(dto.Authors.Authors))
                            };

                            inProceedings.Add(inProceedingsElement);
                            break;
                        }
                }
            }
        }

        private static ICollection<string> ToAuthorsList(ICollection<AuthorJsonDto> authors) =>
            authors
                .Select(a => a.FullName)
                .ToList();

        private static string? ToMonth(string? month)
        {
            if (month is null)
            {
                return null;
            }

            return string.Join(", ", month.Split(new char[] { ' ', '-', '.' })
                .Where(s => s.All(c => char.IsLetter(c))));
        }
    }
}
