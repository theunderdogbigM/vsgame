using gamestore.DTOs;
using gamestore.Entities;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace gamestore.Mapping;

public static class UserMapping
{
    public static User ToEntity(this CreateUserDto user)
    {
    return new User ()
    {
        Name = user.Name,
        Email = user.Email,
        Password = user.Password
    };
    }

    public static UserSummaryDto ToUserSummary( this User user)
    {
        return new (
            user.Id,
            user.Name,
            user.Email
        );
    }

    public static UserDetailsDto ToUserDetails(this User user)
    {
        return new (
     
         user.Id,
         user.Name,
         user.Email,
         user.Password,
         user.Activity


        );

    }

       public static User ToEntity(this UpdateUserDto newUser, int id)
       {
           return new User()
           {
            Id = id,
            Name = newUser.Name,
            Email = newUser.Email,
            Password = newUser.Password
           };
       }
}
