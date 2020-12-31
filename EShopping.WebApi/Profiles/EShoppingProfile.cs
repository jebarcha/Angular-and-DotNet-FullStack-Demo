using AutoMapper;
using EShopping.Dtos;
using EShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopping.WebApi.Profiles
{
    public class EShoppingProfile: AutoMapper.Profile
    {
        public EShoppingProfile()
        {
            this.CreateMap<Product, ProductDto>().ReverseMap();

            this.CreateMap<Models.Profile, ProfileDto>().ReverseMap();

            this.CreateMap<Models.Order, OrderDto>()
                   .ForMember(u => u.User, p=>p.MapFrom(m=>m.User.Username))
                   .ReverseMap()
                   .ForMember(u=>u.User, p=>p.Ignore());

            this.CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(u => u.Product, p => p.MapFrom(u => u.Product.Name))
                .ReverseMap()
                .ForMember(u => u.Product, p => p.Ignore());

            this.CreateMap<User, UserRegisterDto>()
                .ForMember(u => u.Profile, p => p.MapFrom(m => m.Profile.Name))
                .ReverseMap()
                .ForMember(u => u.Profile, p => p.Ignore());

            this.CreateMap<User, UserUpdateDto>()
                .ReverseMap();

            this.CreateMap<User, UserListDto>()
                .ForMember(u => u.Profile, p => p.MapFrom(m => m.Profile.Name))
                .ForMember(u => u.FullName, p => p.MapFrom(m => string.Format("{0} {1}",
                        m.Name, m.LastName)))
                .ReverseMap();

            this.CreateMap<User, LoginModelDto>().ReverseMap();

            this.CreateMap<User, UserProfileDto>().ReverseMap();

        }
    }
}
