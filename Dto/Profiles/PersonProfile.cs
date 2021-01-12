using AutoMapper;
using IEIPaperSearch.Models;

namespace IEIPaperSearch.Dto.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDto>();
        }
    }
}
