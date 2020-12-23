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
        }
    }
}
