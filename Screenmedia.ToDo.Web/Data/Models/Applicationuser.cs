using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Screenmedia.ToDo.Web.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IList<ToDoNote> ToDoNotes { get; } = new List<ToDoNote>();
    }
}
