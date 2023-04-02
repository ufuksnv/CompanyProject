using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Advice
    {
        [Key]
        public int AdviceId { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public DateTime AdviceDate { get; set; }
        public bool Status { get; set; }
    }
}
