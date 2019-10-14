using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Screenmedia.ToDo.Web.Data.Models
{
    public class ToDoNote
    {
        public int Id { get; set; }
        // TODO - Re-evaluate use of the null-forgiving operator
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public bool Deleted { get; set; } = false;
        // TODO - Re-evaluate use of the null-forgiving operator
        public string ApplicationUserId { get; set; } = default!;
        // TODO - Re-evaluate use of the null-forgiving operator
        public Applicationuser ApplicationUser { get; set; } = default!;
    }
}
