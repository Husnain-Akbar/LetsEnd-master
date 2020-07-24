using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetsEnd.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsEnd.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
        }
        [HttpGet]
        public async Task<ActionResult<List<Genre>>> Get()
        {
            return await context.Genres.ToListAsync();
        }


    }
}