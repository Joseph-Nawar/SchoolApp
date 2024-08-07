using Microsoft.EntityFrameworkCore;
using SchoolApp.Data;
using SchoolApp;
using SchoolApp.Repository;
using System.Text.Json.Serialization;
using SchoolApp.Interface;
using System.Reflection;
using SchoolApp.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>(); // Use the correct namespace here
}, Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();

builder.Services.AddScoped<ISchoolRepository, SchoolRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
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

app.UseAuthorization();

app.MapControllers();

app.Run();
