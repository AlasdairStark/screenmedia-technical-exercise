using System.ComponentModel.DataAnnotations;

namespace Screenmedia.ToDo.Web.Data.Models
{
    public class ToDoNote
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; } = default!;

        [MaxLength(200)]
        public string? Description { get; set; }

        public bool Done { get; set; } = false;

        public bool Deleted { get; set; } = false;

        public string ApplicationUserId { get; set; } = default!;

        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
