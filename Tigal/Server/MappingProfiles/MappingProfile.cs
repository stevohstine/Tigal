using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tigal.Shared.Models.Houses;

namespace Tigal.Server.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CreateMap<Users, GetUsersDTO>();
            // CreateMap<RegisterDTO, Users>();
            CreateMap<Houses, GetPropertyDTO>();
            CreateMap<GetPropertyDTO, Houses>();
            // CreateMap<Users, RegisterDTO>();
            // CreateMap<AddRoleDTO, UserRoles>();
            // CreateMap<AddTechnicianDTO, TechniciansData>();
            // CreateMap<RegisterDeviceOwnerDto, DeviceOwnerDataModel>();
            // CreateMap<SmsRequestBodyDTo,SmsRequestBody>();
            // CreateMap<PhoneNumbersDataRequests,SmsPhoneNumbersDTO>();
            // CreateMap<AddSwitchesDTO, SwitchesModel>();
            // CreateMap<CreatePhoneNumbersDTO, SmsPhoneNumbersDTO>();
            // CreateMap<SwitchesReceivedData, SwitchesData>();
        }

    }
}
