using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

//ToDo: Use Custom User And Roles And Use Custom Classes for Other Need Generic Types in context
public class DataContext : IdentityDbContext<IdentityUser<long>,IdentityRole<long>,long>
{
    public DataContext(DbContextOptions<DataContext> options):base(options)
    {
    }
    
}