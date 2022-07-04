using System;
using System.Collections.Generic;
using System.Text;

namespace Tadrebat.Mapper
{
    public class BussinessToAPI : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<MasterList, ResponseMasterList>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
            CreateMap<MasterListDetails, ResponseMasterListDetails>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(y => y._id));
        }
    }
}
