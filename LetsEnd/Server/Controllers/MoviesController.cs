using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LetsEnd.Server.Helpers;
using LetsEnd.Shared.DTOs;
using LetsEnd.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsEnd.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;

        public IMapper Mapper { get; }

        public MoviesController(ApplicationDbContext context,
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
            Mapper = mapper;
        }

        [HttpGet]
         public async Task<ActionResult<IndexPageDTO>> Get()
        {
            var limit = 6;
            var moviesInTheaters = await context.Movies.Where(x => x.InTheaters)
                                        .Take(limit).
                                        OrderByDescending(x => x.ReleaseDate)
                                        .ToListAsync();
            var todaysDate = DateTime.Now;

            var upcommingReleases = await context.Movies.Where(x => x.ReleaseDate > todaysDate)
                .OrderBy(x => x.ReleaseDate).Take(limit).ToListAsync();

            var response = new IndexPageDTO();
            response.Intheaters = moviesInTheaters;
            response.UpcomingReleases = upcommingReleases;

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailsMovieDTO>> Get(int id)
        {
            var movie = await context.Movies.Where(x => x.Id == id)
                .Include(x => x.MoviesGenres)
                    .ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors)
                    .ThenInclude(x => x.Person)
                .FirstOrDefaultAsync();

            if (movie == null)
            {
                return NotFound();

            }
            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Order).ToList();

            var model = new DetailsMovieDTO();
            model.Movie = movie;
            model.Genres = movie.MoviesGenres.Select(x => x.Genre).ToList();
            model.Actors = movie.MoviesActors.Select(x =>

                new Person
                {
                    Name = x.Person.Name,
                    Picture = x.Person.Picture,
                    Character = x.Character,
                    Id = x.PersonId


                }).ToList();
            return model;

        }

        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDTO>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if(movieActionResult.Result is NoContentResult) { return NotFound(); }

            var movieDetailDTO = movieActionResult.Value;
            var selectedGenresIds = movieDetailDTO.Genres.Select(x => x.Id).ToList();
            var NotSelectedGenres = await context.Genres
                                .Where(x => !selectedGenresIds.Contains(x.Id))
                                .ToListAsync();
            var model = new MovieUpdateDTO();
            model.Movie = movieDetailDTO.Movie;
            model.SelectedGenres = movieDetailDTO.Genres;
            model.NotSelectedGenres = NotSelectedGenres;
            model.Actors = movieDetailDTO.Actors;
            return model;

         }

        [HttpPut]
        public async Task<ActionResult> Put(Movie movie)
        {
            var movieDB = await context.Movies.FirstOrDefaultAsync(x => x.Id == movie.Id);
            if (movieDB == null) { return NotFound(); }

            movieDB = Mapper.Map(movie, movieDB);
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var personPicture = Convert.FromBase64String(movie.Poster);
                movieDB.Poster = await fileStorageService.EditFile(personPicture
                    , "jpg", "movies", movieDB
                    .Poster);
            }
            await context.Database.ExecuteSqlInterpolatedAsync($"delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id}");

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            movieDB.MoviesActors = movie.MoviesActors;
            movieDB.MoviesGenres = movie.MoviesGenres;

            await context.SaveChangesAsync();
            return NoContent();

        }
        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var personPicture = Convert.FromBase64String(movie.Poster);
                movie.Poster = await fileStorageService.SaveFile(personPicture,
                    "jpg", "movies");
            }

            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }

    }
}