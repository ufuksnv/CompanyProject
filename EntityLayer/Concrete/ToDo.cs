using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class ToDo
    {
        public int ToDoId { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime TodoAddDate { get; set; }
        public DateTime TargetDate { get; set; }
        public bool Status { get; set; }

        public AppUser? AppUser { get; set; }
        public string? AppUserId { get; set; }
    }
}
