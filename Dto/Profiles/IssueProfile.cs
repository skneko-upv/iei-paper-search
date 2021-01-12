using AutoMapper;
using IEIPaperSearch.Models;

namespace IEIPaperSearch.Dto.Profiles
{
    public class IssueProfile : Profile
    {
        public IssueProfile()
        {
            CreateMap<Issue, IssueDto>();
        }
    }
}
