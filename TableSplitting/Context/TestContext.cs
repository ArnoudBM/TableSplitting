namespace TableSplitting.Context
{
    using System.Data.Entity;
    using Models.OneToZeroOrOne;
    using Models.TableSplitting;
    using Models.Combined;

    public class TestContext : DbContext
    {
        public TestContext() : base("TestContext")
        {
            // When enabling EF Code First Migrations (enable-migrations in the Package Manager Console), the location for |DataDirectory| must be known
            AppDomainValues.SetDataDirectory();
        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Laptop> Laptops { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAdresses { get; set; }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ConsumerProject> ConsumerProjects { get; set; }
        public virtual DbSet<BusinessProject> BusinessProjects { get; set; }
        public virtual DbSet<BusinessProjectOptions> BusinessProjectsOptions { get; set; }

        public virtual DbSet<ProjectComponent> ProjectComponents { get; set; }
    }
}