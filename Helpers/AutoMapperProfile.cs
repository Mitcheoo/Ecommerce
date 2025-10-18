using AutoMapper;
using Ecommerce.Data;
using Ecommerce.ViewModels;

namespace Ecommerce.Helpers
{
    public class AutoMapperProfile: Profile 
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterVM, KhachHang>();
            /*    .ForMember(kh=> kh.HoTen, option => option.MapFrom(RegisterVM =>
                RegisterVM.HoTen));*/
            // Create your mapping configurations here
            // Example:
            // CreateMap<SourceModel, DestinationModel>();
        }
    }
}
