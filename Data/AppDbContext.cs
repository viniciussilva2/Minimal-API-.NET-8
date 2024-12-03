using Microsoft.EntityFrameworkCore;
using MinimalApiProjectVideo.Models;

namespace MinimalApiProjectVideo.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //Criar tabela.
        public DbSet<UsuarioModel> Usuarios { get; set; }




    }
}
