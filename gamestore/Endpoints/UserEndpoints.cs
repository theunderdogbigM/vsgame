using gamestore.DTOs;
using FluentValidation;
using gamestore.Data;
using gamestore.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using gamestore.Entities;
using Microsoft.IdentityModel.Tokens; // For token generation
using System.IdentityModel.Tokens.Jwt; // For JWT handling
using System.Threading.Tasks;
using gamestore.Security;

namespace gamestore.Endpoints
{
    public static class UserEndpoints
    {
        const string UserEndpointsName = "GetUser";

        public static void MapUserEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/users");

            // Get all users
            group.MapGet("/", async (GamestoreDBContext dbContext) =>
                await dbContext.User
                    .AsNoTracking()
                    .Select(user => user.ToUserSummary())
                    .ToListAsync()
            );

            // Get user by ID
            group.MapGet("/{id}", async (int id, GamestoreDBContext dbContext) =>
            {
                var user = await dbContext.User.FindAsync(id);
                return user == null ? Results.NotFound() : Results.Ok(user.ToUserDetails());
            }).WithName(UserEndpointsName);

            // Create new user
            group.MapPost("/", async (CreateUserDto newUser, GamestoreDBContext dbContext, IValidator<CreateUserDto> validator) =>
{
            var validationResult = validator.Validate(newUser);
            if (!validationResult.IsValid)
               {
        return Results.BadRequest(validationResult.Errors);
                 }

              var user = newUser.ToEntity();
    
                    // Hash the password before saving the user
                  user.Password = PasswordHasher.HashPassword(newUser.Password);

                    dbContext.User.Add(user);
                   await dbContext.SaveChangesAsync();
             return Results.CreatedAtRoute(UserEndpointsName, new { id = user.Id }, user.ToUserDetails());
            });


            // Update user
            group.MapPut("/{id}", async (int id, UpdateUserDto updateUser, IValidator<UpdateUserDto> validator, GamestoreDBContext dbContext) =>
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

            // Delete user
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
            });





// Rehash passwords for all users (Admin use only)
group.MapPost("/rehash-passwords", async (GamestoreDBContext dbContext) =>
{
    // Optionally, add authorization checks here to restrict access to admins
    var users = await dbContext.User.ToListAsync();

    foreach (var user in users)
    {
        // Rehash only if the password is not already hashed
        if (!IsPasswordHashed(user.Password)) // You'll need to implement this check
        {
            user.Password = PasswordHasher.HashPassword(user.Password);
        }
    }

    await dbContext.SaveChangesAsync();

    return Results.Ok("Passwords rehashed successfully.");
});




group.MapPost("/logout", async (HttpContext context, GamestoreDBContext dbContext) =>
{
    // Extract the token from the Authorization header
    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    
    if (string.IsNullOrEmpty(token))
    {
        return Results.BadRequest("No token provided.");
    }

    // Add the token to the blacklist with its expiration date
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);

    var expirationDate = jwtToken.ValidTo;
    dbContext.BlacklistedTokens.Add(new BlacklistedToken
    {
        Token = token,
        ExpirationDate = expirationDate
    });

    await dbContext.SaveChangesAsync();
    
    return Results.Ok("Logged out successfully.");
});



group.MapPost("/signin", async (SignInDto signInDto, GamestoreDBContext dbContext) =>
{
    // Log the request for debugging
    Console.WriteLine($"Sign-in attempt for: {signInDto.Email}");

    // Find the user by email
    var user = await dbContext.User.FirstOrDefaultAsync(u => u.Email == signInDto.Email);

    // Check if the user exists and if the password is correct
    if (user == null || !PasswordHasher.VerifyPassword(signInDto.Password, user.Password))
    {
        Console.WriteLine("Invalid email or password"); // Log failure
        return Results.BadRequest("Invalid email or password");
    }

    // Create a token (after fixing the error)
    var token = GenerateToken(user);

    // Log success
    Console.WriteLine($"User signed in: {user.Email}");

    // Return the token in response
    return Results.Ok(new { Token = token });
});



        }

        private static string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperLongSecretKeyForJWTGeneration123!"));
 // Use a secure key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static bool IsPasswordHashed(string password)
{
    // Check if the password string looks like a hashed password
    // For example, check the length or the format (e.g., Base64 string)
    return !string.IsNullOrEmpty(password) && 
           (password.Length == 88 || password.Length == 64); // Adjust based on your hashing method
}
        
    }
}
