using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext : IdentityDbContext<IdentityUser<long>,IdentityRole<long>,long>
{
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
    }
    
}