using Microsoft.EntityFrameworkCore;

namespace LockoutSample.Infrastructure.Data
{
    public class DatabaseHelper(LockoutDbContext context)
    {
        public void SetupDatabase()
        {
            context.Database.Migrate();
        }
    }
}
