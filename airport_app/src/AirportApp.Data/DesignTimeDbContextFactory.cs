using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AirportApp.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlServer(@"Server=.\SQLEXPRESS;Initial Catalog=AirportDB;Integrated Security=true;TrustServerCertificate=True");
        return new AppDbContext(builder.Options);
    }
}
