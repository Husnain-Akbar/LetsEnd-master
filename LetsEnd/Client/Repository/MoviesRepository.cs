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
            return await Get<IndexPageDTO>(url);
        }

        public async Task<DetailsMovieDTO> GetDetailsMovieDTO(int id)
        {
            return await Get<DetailsMovieDTO>($"{url}/{id}");
        }

        private async Task<T> Get<T>(string url)
        {
            var response = await httpService.Get<T>(url);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
            return response.Response;
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

    }
}
