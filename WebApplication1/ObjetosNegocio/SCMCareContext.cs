using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace WebApplication1.ObjetosNegocio;

public partial class SCMCareContext : IdentityUserContext<IdentityUser>
{
    public SCMCareContext()
    {
    }

    public SCMCareContext(DbContextOptions<SCMCareContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<Sobremesa> Sobremesas { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*
        modelBuilder.Entity<Sobremesa>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("Id");
            entity.Property(e => e.Nome).HasMaxLength(255);
        });
        */
    }
}
