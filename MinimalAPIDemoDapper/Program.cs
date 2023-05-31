using DataAccess.DbAccess;
using MinimalAPIDemoDapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Se agrega la dependencia del data access
builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddTransient<IUserData, UserData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Para la redirecci�n a la versi�n s del sitio
app.UseHttpsRedirection();

//M�todo de extensi�n creado en Api.cs
app.ConfigureApi();

app.Run();