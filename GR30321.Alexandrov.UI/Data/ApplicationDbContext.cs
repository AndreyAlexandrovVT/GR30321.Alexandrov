using GR30321.Alexandrov.UI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GR30321.Domain.Entities;

namespace GR30321.Alexandrov.UI.Data
{
    public class ApplicationDbContext : IdentityDbContext <AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        //public DbSet<GR30321.Domain.Entities.Cart> Cart { get; set; } = default!;
    }
}
