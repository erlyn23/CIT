using AutoMapper;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Profiles
{
    public class DtosProfiles : Profile
    {
        public DtosProfiles()
        {
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<EntityInfoDto, Entitiesinfo>().ReverseMap();
            CreateMap<LenderBusinessDto, LenderBusiness>().ReverseMap();
            CreateMap<LogDto, Log>().ReverseMap();
            CreateMap<OperationDto, Operation>().ReverseMap();
            CreateMap<PageDto, Page>().ReverseMap();
            CreateMap<RoleDto, Role>().ReverseMap();
            CreateMap<RolePermissionDto, Rolepermission>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserRoleDto, Userrole>().ReverseMap();
            CreateMap<VehicleAssignmentDto, VehicleAssignment>().ReverseMap();
            CreateMap<VehicleDto, Vehicle>().ReverseMap();
            CreateMap<LoanDto, Loan>().ReverseMap();
        }
    }
}
