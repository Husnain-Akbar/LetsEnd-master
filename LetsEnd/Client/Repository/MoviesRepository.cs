using LetsEnd.Client.Helpers;
using LetsEnd.Shared.DTOs;
using LetsEnd.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetsEnd.Client.Repository
{
    public class MoviesRepository:IMoviesRepository
    { 
        private readonly IHttpService httpService;
        private string url = "api/movies";

        public MoviesRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<IndexPageDTO> GetIndexPageDTO()
        {
            return await httpService.GetHelper<IndexPageDTO>(url);
        }

        public async Task<DetailsMovieDTO> GetDetailsMovieDTO(int id)
        {
            return await httpService.GetHelper<DetailsMovieDTO>($"{url}/{id}");
        }

        public async Task<MovieUpdateDTO> GetMovieForUpdate(int id)
        {
            return await httpService.GetHelper<MovieUpdateDTO>($"{url}/update/{id}");
        }
        public async Task<int> CreateMovie(Movie movie)
        {
            var response = await httpService.Post<Movie,int>(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
        }

        public async Task UpdateMovie(Movie movie)
        {
            var response = await httpService.Put(url, movie);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }

        }
    }
}
