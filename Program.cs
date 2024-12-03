using Microsoft.EntityFrameworkCore;
using MinimalApiProjectVideo.Data;
using MinimalApiProjectVideo.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Método listagem de usuários.
async Task<List<UsuarioModel>> GetUsuarios(AppDbContext context)
{
    return await context.Usuarios.ToListAsync();

}

//Obter usuários.
app.MapGet("/Usuarios", async (AppDbContext context) =>
{
    return await GetUsuarios(context);

});

//Obter usuários por Id.
app.MapGet("/Usuarios/{id}", async (AppDbContext context, int id) =>
{

    var usuario = await context.Usuarios.FindAsync(id);

    if (usuario == null)
    {
        return Results.NotFound("Usuário não encontrado.");
    }
    else
    {
        return Results.Ok(usuario);
    }


});

//Criar usuários.
app.MapPost("/Usuario", async (AppDbContext context, UsuarioModel usuario) =>
{
    context.Usuarios.Add(usuario);
    await context.SaveChangesAsync();

    return await GetUsuarios(context);

});

//Atualizar usuários.
app.MapPut("/Usuario", async (AppDbContext context, UsuarioModel usuario) =>
{
    var usuarioDb = await context.Usuarios.FindAsync(usuario.Id);

    if (usuarioDb == null) return Results.NotFound("Usuário não encontrado!.");

    usuarioDb.Username = usuario.Username;
    usuarioDb.Email = usuario.Email;
    usuarioDb.Nome = usuario.Nome;


    context.Usuarios.Update(usuarioDb);
    await context.SaveChangesAsync();

    return Results.Ok(await GetUsuarios(context));
});

//Deletar usuários.
app.MapDelete("/Usuario", async (AppDbContext context, int id) =>
{
    var usuarioDb = await context.Usuarios.FindAsync(id);

    if (usuarioDb == null) return Results.NotFound("Usuário não encontrado.");


    context.Usuarios.Remove(usuarioDb);
    await context.SaveChangesAsync();

    return Results.Ok(await GetUsuarios(context));

});





app.Run();



