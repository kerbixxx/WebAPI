using System.ComponentModel.DataAnnotations;

namespace SimbirSoft.Models
{
    public class Animal
    {
        [Key]
        public long Id { get; set; }
        public string type { get; set; }
    }
}
