using AutoMapper;
using LetsEnd.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsEnd.Server.Helpers
{
    public class AutomapperProfiles:Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Person, Person>()
                .ForMember(x => x.Picture, option => option.Ignore());

            CreateMap<Movie, Movie>()
                .ForMember(x => x.Poster, option => option.Ignore());
            
        }

    }
}
