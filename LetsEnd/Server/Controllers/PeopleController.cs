using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LetsEnd.Server.Helpers;
using LetsEnd.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LetsEnd.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;

        public IMapper Mapper;

        public PeopleController(ApplicationDbContext context, 
            IFileStorageService fileStorageService,
            IMapper mapper)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
            Mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                person.Picture = await fileStorageService.SaveFile(personPicture,
                    "jpg", "people");
            }

            context.Add(person);
            await context.SaveChangesAsync();
            return person.Id;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await context.People.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
            {
                return NotFound();
            }
            return person;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            var personDB = await context.People.FirstOrDefaultAsync(x => x.Id == person.Id);
            if (personDB == null) { return NotFound();}

            personDB = Mapper.Map(person, personDB);
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                personDB.Picture = await fileStorageService.EditFile(personPicture
                    , "jpg", "people", personDB
                    .Picture);
            }
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Person>(); }
            return await context.People.Where(x => x.Name.Contains(searchText))
                .Take(5)
                .ToListAsync();
        }

    }
}