using System.Configuration;
using System.Data.Entity;
using Softcrafts.Jobs.Entities;

namespace Softcrafts.Jobs.Business.Data
{
    public class JobContext : DbContext
    {
        public IDbSet<Job> Jobs { get; set; }
        public IDbSet<APIUser> APIUser { get; set; }
        public IDbSet<Settings> Settings { get; set; }

        public JobContext() : base(ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString)
        {
            Jobs = new FilteredDbSet<Job>(this, c => !c.IsDeleted);
            APIUser = new FilteredDbSet<APIUser>(this, c => !c.IsDeleted);
            Settings = new FilteredDbSet<Settings>(this, c => !c.IsDeleted);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>().ToTable("Job");
            modelBuilder.Entity<APIUser>().ToTable("APIUser");
            modelBuilder.Entity<Settings>().ToTable("Settings");
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}
