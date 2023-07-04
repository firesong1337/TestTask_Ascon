using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskAskon.Models
{
    internal class Hooks
    {
        [ForeignKey("Objects")]
        public int IdParent { get; set; }
        [ForeignKey("Attributes")]
        public int IdChild { get; set; }
        public string? LinkName { get; set; }
    }
}
