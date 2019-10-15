using Microsoft.EntityFrameworkCore;
using Screenmedia.ToDo.Web.Data.Models;

namespace Screenmedia.ToDo.Web.Data
{
    public interface IApplicationDbContext
    {
        DbSet<ToDoNote> ToDoNotes { get; set; }

        int SaveChanges();
    }
}
