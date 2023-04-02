using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class AppUser : IdentityUser
    {
        public int Salary { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool UserStatus { get; set; }


        public Department? Department { get; set; }
        public int? DepartmentId { get; set; }

        public ICollection<ToDo>? ToDos { get; set; }
    }
}
