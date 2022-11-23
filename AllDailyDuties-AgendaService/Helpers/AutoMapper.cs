using AllDailyDuties_AgendaService.Models.Tasks;
using AutoMapper;

namespace AllDailyDuties_AgendaService.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // CreateRequest -> User
            CreateMap<CreateRequest, TaskItem>();
        }
    }
}
