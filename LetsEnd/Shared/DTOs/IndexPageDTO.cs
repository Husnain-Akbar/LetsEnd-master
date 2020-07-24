using LetsEnd.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetsEnd.Shared.DTOs
{
  public   class IndexPageDTO
    {
        public List<Movie> Intheaters { get; set; }
        public List<Movie> UpcomingReleases { get; set; }
    }
}
