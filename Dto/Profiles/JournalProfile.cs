using AutoMapper;
using IEIPaperSearch.Models;

namespace IEIPaperSearch.Dto.Profiles
{
    public class JournalProfile : Profile
    {
        public JournalProfile()
        {
            CreateMap<Journal, JournalDto>();
        }
    }
}
