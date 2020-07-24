using LetsEnd.Shared.DTOs;
using LetsEnd.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsEnd.Client.Repository
{
    public interface IMoviesRepository
    {
        Task<int> CreateMovie(Movie movie);
        Task<DetailsMovieDTO> GetDetailsMovieDTO(int id);
        Task<IndexPageDTO> GetIndexPageDTO();
    }
}
