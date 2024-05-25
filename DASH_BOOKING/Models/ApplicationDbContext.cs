using System.Data.Entity;

namespace DASH_BOOKING.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<EventRequest> EventRequests { get; set; }
        public DbSet<EventModel> EventModels { get; set; }
        public DbSet<EventImage> EventImages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring TPH inheritance for User and Admin
            modelBuilder.Entity<User>()
                .Map<Admin>(m => m.Requires("Discriminator").HasValue("Admin"));

            // Configuring the one-to-many relationship between EventModel and EventImages
            modelBuilder.Entity<EventImage>()
                .HasRequired(e => e.EventModel)
                .WithMany(e => e.Images)
                .HasForeignKey(e => e.EventModelId)
                .WillCascadeOnDelete(true);
        }
    }
}
