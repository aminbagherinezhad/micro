using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTicTac.Cw.Models
{
    public class LogMessageReciver
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(400)]
        public string MessageLogs { get; set; }
    }
}
