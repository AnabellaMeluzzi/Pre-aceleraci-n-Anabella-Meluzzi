using DisneyAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Context
{
    public class DisneyContext : DbContext
    {
        public const string Schema = "disney";
        public DisneyContext(DbContextOptions<DisneyContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema(Schema);
        }


        public DbSet<Character> Characters { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }

    }
}
