using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using TheTecniQ.API.Infrastructure.Extensions;
using TheTecniQ.Core.Domain.Grid;
using TheTecniQ.Core.Domain.User;
using TheTecniQ.Services.Extensions;
using TheTecniQ.Services.Users;
using System.Collections;
using System.Collections.Generic;
using Quartz;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

//Comnet:load application settings
builder.Services.ConfigureApplicationSettings(builder);

LoggingHelper.InitializeLogger(args,builder);
//Comnet:add services to the application and configure service provider
builder.Services.ConfigureApplicationServices(builder);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection(); //Comnet:Moved to ConfigureRequestPipeline()

//app.UseAuthorization(); //Comnet:Moved to ConfigureRequestPipeline()

app.MapControllers();

//Comnet:configure the application HTTP request pipeline
app.ConfigureRequestPipeline();
//app.MapGet("/", () => "Hello, World!");

//// Example 1: Get all books
//app.MapPost("/books/list", ([FromServices]IRoleService roleService, [FromBody]GridRequestModel objGrid) =>
//    TypedResults.Ok(roleService.GetAllAsync(objGrid)))
//    .WithName("GetBooks");

//// Example 2: Get a specific book by ID
//app.MapGet("/books/{id}", async ([FromServices] IRoleService roleService, int id) =>
//{
//    Role book = await roleService.GetByIdAsync(id);
//    return TypedResults.Ok(book);
//}).WithName("GetBookById");

//// Example 3: Add a new book
//app.MapPost("/books", async ([FromServices] IRoleService roleService, [FromBody]Role newBook) =>
//{
//    await roleService.InsertAsync(newBook);
//    return TypedResults.Created($"/books/{newBook.Id}", newBook);
//}).WithName("AddBook");

//// Example 4: Update an existing book
//app.MapPut("/books/{id}", async ([FromServices] IRoleService roleService, int id, [FromBody]Role updatedBook) =>
//{
//    await roleService.UpdateAsync(updatedBook);
//    return TypedResults.Ok();
//}).WithName("UpdateBook");

//// Example 5: Delete a book by ID
//app.MapDelete("/books/{id}", async ([FromServices] IRoleService roleService, int id) =>
//{
//    return TypedResults.NoContent();
//}).WithName("DeleteBook");

app.Run();
