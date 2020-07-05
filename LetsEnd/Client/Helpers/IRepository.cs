using LetsEnd.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsEnd.Client.Helpers
{
     public interface IRepository
    {
        List<Movie> GetMovies(); 
    }
}
