using System.Data.Entity.Migrations;

namespace Auth.Api.Migrations.OperationalConfiguration
{
    internal sealed class Configuration : DbMigrationsConfiguration<IdentityServer3.EntityFramework.OperationalDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\OperationalConfiguration";
        }

        protected override void Seed(IdentityServer3.EntityFramework.OperationalDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
