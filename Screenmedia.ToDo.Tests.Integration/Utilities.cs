using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Screenmedia.ToDo.Web.Data;
using Screenmedia.ToDo.Web.Data.Models;

namespace Screenmedia.ToDo.Tests.Integration
{
    public static class Utilities
    {
        private const string ApplicationUserId = "9d253132-0e1c-4787-bd51-2d0c6eb23855";
        private const string Email = "user1@test-server.com";

        public static void InitializeDbForTests(ApplicationDbContext db)
        {
            // Clear out the data before running each test
            // This could be improved by wrapping a transaction around each test
            // and rolling it back after execution
            db.Users.RemoveRange(db.Users);
            db.ToDoNotes.RemoveRange(db.ToDoNotes);
            db.SaveChanges();

            // Seed the test data
            db.Users.AddRange(GetSeedingIdentityUsers());
            db.ToDoNotes.AddRange(GetSeedingToDoNotes());
            db.SaveChanges();
        }

        public static List<IdentityUser> GetSeedingIdentityUsers()
        {
            return new List<IdentityUser>()
            {
                new IdentityUser
                {
                    ConcurrencyStamp = DateTime.Now.Ticks.ToString(),
                    Email = Email,
                    EmailConfirmed = true,
                    Id = ApplicationUserId,
                    NormalizedEmail = Email,
                    NormalizedUserName = Email,
                    PasswordHash = Guid.NewGuid().ToString(),
                    UserName = Email
                }
            };
        }

        public static List<ToDoNote> GetSeedingToDoNotes()
        {
            return new List<ToDoNote>()
            {
                 new ToDoNote
                 {
                     ApplicationUserId = ApplicationUserId,
                     Title = "Test Note 1",
                     Description = "Test Description 1"
                 },
                 new ToDoNote
                 {
                     ApplicationUserId = ApplicationUserId,
                     Title = "Test Note 2",
                     Description = "Test Description 2"
                 }
            };
        }
    }
}
