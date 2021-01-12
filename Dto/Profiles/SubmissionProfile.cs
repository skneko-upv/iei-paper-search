using AutoMapper;
using IEIPaperSearch.Models;

namespace IEIPaperSearch.Dto.Profiles
{
    public class SubmissionProfile : Profile
    {
        public SubmissionProfile()
        {
            CreateMap<Submission, SubmissionDto>()
                .Include<Article, ArticleDto>()
                .Include<Book, BookDto>()
                .Include<InProceedings, InProceedingsDto>();

            CreateMap<Article, ArticleDto>();
            CreateMap<Book, BookDto>();
            CreateMap<InProceedings, InProceedingsDto>();
        }
    }
}
