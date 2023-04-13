using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PartyPlanning.Data
{
    public class PartyContext : IdentityDbContext<PartyUser, IdentityRole<Guid>, Guid>
    {
        #region Properties

        public DbSet<Apport> Apport { get; set; }
        public DbSet<Car> Car { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Party> Party { get; set; }
        public DbSet<PartyUser> PartyUsers { get; set; }

        #endregion Properties

        #region Constructor

        public PartyContext()
        {
        }

        public PartyContext(DbContextOptions<PartyContext> options) : base(options)
        {
            Database.Migrate();
        }

        #endregion Constructor

        #region Methods

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Apport>(a =>
            {
                a.HasKey(a => a.IdApport);
                a.Property(a => a.Type).HasMaxLength(255);
                a.Property(a => a.Nom).HasMaxLength(255);
            });

            builder.Entity<Car>(c =>
            {
                c.HasKey(c => c.IdCar);
            });

            builder.Entity<Message>(m =>
            {
                m.HasKey(m => m.IdMessage);
            });

            builder.Entity<Party>(p =>
            {
                p.HasKey(p => p.IdParty);
                p.Property(p => p.Name).HasMaxLength(255);
                p.Property(p => p.Description).HasMaxLength(255);
                p.Property(p => p.Adresse).HasMaxLength(255);

                p.HasMany(p => p.Members).WithMany(u => u.Parties);
                p.HasMany(p => p.Messages).WithOne(m => m.Party).HasForeignKey(m => m.IdMessage).OnDelete(DeleteBehavior.Cascade);
                p.HasMany(p => p.Apports).WithMany(a => a.Parties);
            });

            builder.Entity<PartyUser>(u =>
            {
                u.Property(u => u.FirstName).HasMaxLength(255);
                u.Property(u => u.LastName).HasMaxLength(255);
                u.Property(u => u.Biography).HasMaxLength(255);
                u.Property(u => u.Snap).HasMaxLength(255);
                u.Property(u => u.Insta).HasMaxLength(255);

                u.HasOne(u => u.Car).WithOne(c => c.Owner).HasForeignKey<Car>(c => c.IdCar);
            });
        }

        #endregion Methods
    }
}