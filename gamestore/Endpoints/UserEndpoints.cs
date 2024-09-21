using gamestore.DTOs;
using FluentValidation;
using gamestore.Data;
using gamestore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace gamestore.Endpoints{

public static class UserEndpoints
{
    const string UserEndpointsName = "GetUser";

    
    public static void MapUserEndpoints(this WebApplication app)
    {
       var group = app.MapGroup("/users");

       group.MapGet("/", async (GamestoreDBContext dbContext) =>
    await dbContext.User
        .AsNoTracking()
        .Select(user => user.ToUserSummary())  // Project data into UserSummary DTO
        .ToListAsync()
);



        group.MapGet("/{id}", async (int id, GamestoreDBContext dBContext)=>
        
        {
                var user = await dBContext.User.FindAsync(id);
                return user == null ? Results.NotFound() : Results.Ok(user.ToUserDetails());
            }).WithName(UserEndpointsName);
     




        group.MapPost("/", async (CreateUserDto newUser, GamestoreDBContext dbContext,IValidator<CreateUserDto> validator)=>
        {
            var validationResult = validator.Validate(newUser);
            if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var user = newUser.ToEntity();
                dbContext.User.Add(user);
                await dbContext.SaveChangesAsync();
                return Results.CreatedAtRoute(UserEndpointsName, 
                new {id = user.Id}, user.ToUserDetails());
        });





        group.MapPut("/{id}", async(int id, UpdateUserDto updateUser,IValidator<UpdateUserDto> validator, GamestoreDBContext dbContext) =>
        {
            var validationResult = validator.Validate(updateUser);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                var existingUser = await dbContext.User.FindAsync(id);

                if (existingUser == null)
                {
                   return Results.NotFound();
                }

                dbContext.Entry(existingUser).CurrentValues.SetValues(updateUser.ToEntity(id));
                await dbContext.SaveChangesAsync();
                return Results.NoContent();

        });


        group.MapDelete("/{id}", async (int id, GamestoreDBContext dbContext) =>
        {
           var existingUser = await dbContext.User.FindAsync(id);

                if (existingUser == null)
                {
                   return Results.NotFound();
                }

                dbContext.User.Remove(existingUser);
                await dbContext.SaveChangesAsync();
                return Results.NoContent();
        }
        );

    }
}
}