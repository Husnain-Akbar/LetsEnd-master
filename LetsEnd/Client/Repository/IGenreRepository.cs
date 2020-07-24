using LetsEnd.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsEnd.Client.Repository
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
        Task<List<Genre>> GetGenres();
    }
}
