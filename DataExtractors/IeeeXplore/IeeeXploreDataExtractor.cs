using IEIPaperSearch.Models;
using IEIPaperSearch.Persistence;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IEIPaperSearch.DataExtractors.IeeeXplore
{
    internal class IeeeXploreDataExtractor : IJsonDataExtractor<ICollection<Submission>>
    {
        private readonly PaperSearchContext context;

        public IeeeXploreDataExtractor(PaperSearchContext context)
        {
            this.context = context;
        }

        public ICollection<Submission> Extract(string source)
        {
            var dtos = DeserializeJson(source);
            return ToCanonicalModel(dtos);
        }

        ICollection<SubmissionJsonDto> DeserializeJson(string source)
        {
            var root = JObject.Parse(source);

            IList<JToken> submissionsJson = root["articles"]!.Children().ToList();

            IList<SubmissionJsonDto> submissionsDtos = submissionsJson.Select(s => s.ToObject<SubmissionJsonDto>()!).ToList();

            return submissionsDtos;
        }

        ICollection<Submission> ToCanonicalModel(ICollection<SubmissionJsonDto> dtos)
        {
            var submissions = new List<Submission>();

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
                                endPage: dto.EndPage);

                            article.Authors = ToPeopleList(dto.Authors.Authors);
                            article.PublishedIn = new Issue(
                                dto.Volume, 
                                dto.PublicationNumber, 
                                month: null,                            // TODO
                                new Journal(dto.PublicationTitle));

                            submissions.Add(article);
                            break;
                        }
                    case "Conferences":
                        {
                            var inProceedings = new InProceedings(
                                dto.Title,
                                dto.PublicationYear,
                                dto.PdfUrl,
                                conference: dto.PublicationTitle,
                                edition: dto.ConferenceDates,
                                dto.StartPage,
                                dto.EndPage);

                            inProceedings.Authors = ToPeopleList(dto.Authors.Authors);

                            submissions.Add(inProceedings);
                            break;
                        }
                }
            }

            return submissions;
        }

        ICollection<Person> ToPeopleList(ICollection<AuthorJsonDto> authors) =>
            authors
                .Select(a => new Person(a.FullName))
                .ToList();
    }
}
