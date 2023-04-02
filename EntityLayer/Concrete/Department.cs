using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string? Name { get; set; }
        public bool Status { get; set; }


        public ICollection<AppUser>? AppUsers { get; set; }
    }
}
