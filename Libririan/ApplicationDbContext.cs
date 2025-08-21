using Microsoft.EntityFrameworkCore;

namespace Libririan;

public class ApplicationDbContext:DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        }

}