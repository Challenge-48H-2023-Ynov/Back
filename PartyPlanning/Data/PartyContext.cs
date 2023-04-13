using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PartyPlanning.Data
{
    public class PartyContext : IdentityDbContext<PartyUser, IdentityRole<Guid>, Guid>
    {
        #region Properties

        public DbSet<Apport> Apport { get; set; }
        public DbSet<Participation> Participation { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Invitation> Invitation { get; set; }
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

            builder.Entity<Participation>(p =>
            {
                p.HasKey(p => new {p.IdParty,p.IdUser});
                p.Property(p => p.Role).HasMaxLength(255);

                p.HasMany(p => p.Apports).WithOne(p => p.Participation  ).HasForeignKey(p => new {p.IdParty,p.IdUser}).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Message>(m =>
            {
                m.HasKey(m => m.IdMessage);
            });
            
            builder.Entity<Invitation>(i =>
            {
                i.HasKey(i => new {i.IdParty,i.IdUser});
                i.HasOne(i => i.User).WithMany(i => i.Invitations).HasForeignKey(i => i.IdUser).OnDelete(DeleteBehavior.Cascade);
                i.HasOne(i => i.Party).WithMany(i => i.Invitations).HasForeignKey(i => i.IdParty).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Party>(p =>
            {
                p.HasKey(p => p.IdParty);
                p.Property(p => p.Name).HasMaxLength(255);
                p.Property(p => p.Description).HasMaxLength(255);
                p.Property(p => p.Adresse).HasMaxLength(255);

                p.HasMany(p => p.Participations).WithOne(p => p.Party).HasForeignKey(p => p.IdParty).OnDelete(DeleteBehavior.Cascade);
                p.HasMany(p => p.Besoins).WithOne(p => p.Party).HasForeignKey(p => p.IdParty).OnDelete(DeleteBehavior.Cascade);
                p.HasMany(p => p.Messages).WithOne(p => p.Party).HasForeignKey(p => p.IdParty).OnDelete(DeleteBehavior.Cascade);

            });

            builder.Entity<PartyUser>(u =>
            {
                u.Property(u => u.FirstName).HasMaxLength(255);
                u.Property(u => u.LastName).HasMaxLength(255);
                u.Property(u => u.Snap).HasMaxLength(255);
                u.Property(u => u.Insta).HasMaxLength(255);

                u.HasMany(u => u.Apports).WithOne(a => a.User).HasForeignKey(a => a.IdUser).OnDelete(DeleteBehavior.Cascade);
                u.HasMany(u => u.Participations).WithOne(u => u.User).HasForeignKey(u => u.IdUser).OnDelete(DeleteBehavior.Cascade);
                u.HasMany(u => u.Messages).WithOne(u => u.User).HasForeignKey(u => u.IdUser).OnDelete(DeleteBehavior.Cascade);
                u.HasMany(u => u.Parties).WithOne(u => u.User).HasForeignKey(u => u.IdUser).OnDelete(DeleteBehavior.NoAction);
            });
        }

        #endregion Methods
    }
}