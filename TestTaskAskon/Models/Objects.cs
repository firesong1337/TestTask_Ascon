using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskAskon.Models
{
    internal class Objects
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Product { get; set; }
    }
}
