using CompanyEmployees.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CompanyEmployees_Remastered.ContextFactory;

//  Since our RepositoryContext class is in the CompanyEmployees.Infrastructure.Persistence project and not in the main one,
//  this class will help our application create a derived DbContext instance during the design time, which will help us with our migrations:

//  We are using the IDesignTimeDbContextFactory<out TContext> interface that allows design-time services to discover implementations of this interface.
//  Of course, the TContext parameter is our RepositoryContext class.
public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(configuration.GetConnectionString("sqlConnection"),
            // We have to make this change because migration assembly is not in our main project but in the CompanyEmployees.Infrastructure.Persistence project.
            // So, we’ve just changed the project for the migration assembly.
            b => b.MigrationsAssembly("CompanyEmployees.Infrastructure.Persistence"));

        return new RepositoryContext(builder.Options);
    }
}

