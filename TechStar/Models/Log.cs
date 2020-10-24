using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStar.Models
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int IDWord { get; set; }
        public DateTime Date { get; set; }
        [ForeignKey("IDWord")]
        public Word Word { get; set; }
    }
}
