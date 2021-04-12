using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phase3Section4._23.Models
{
    public class Marks
    {
        [Required]
        public int Student_ID { get; set; }

        [Required]
        public int Subject_ID { get; set; }

        [Required]
        public int Value { get; set; }
    }
}
