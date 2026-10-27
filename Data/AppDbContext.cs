using Microsoft.EntityFrameworkCore;
using second_project.Models;

namespace second_project.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Curso> Cursos => Set<Curso>();
}
