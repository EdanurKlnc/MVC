
using AutoMapper;
using PhoneBookEntityLayer.Entities;
using PhoneBookEntityLayer.ViewModels;

namespace PhoneBookEntityLayer.Mappings
{
    public class Maps : Profile
    {
        //Kim kime dönüşsün?
        public Maps()
        {
            CreateMap<Entities.Member, MemberViewModel>();
            CreateMap<MemberViewModel, Entities.Member>();

            CreateMap<PhoneType, PhoneTypeViewModel>().ReverseMap();
            CreateMap<MemberPhone, MemberPhoneViewModel>().ReverseMap();

        }
    }
}
