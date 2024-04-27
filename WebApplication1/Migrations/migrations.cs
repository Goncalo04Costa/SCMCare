using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            modelBuilder.Entity("WebApplication1.Models.UserFuncionario", b =>
            {
                b.Property<int>("FuncionariosId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                b.Property<string>("User")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.Property<string>("Passe")
                    .IsRequired()
                    .HasColumnType("nvarchar(max)");

                b.HasKey("FuncionariosId");

                b.ToTable("UserFuncionario");
            });
        }
    }
}
