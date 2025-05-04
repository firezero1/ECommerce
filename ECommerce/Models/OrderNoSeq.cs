using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
    public class OrderNoSeq
    {
        [Key]
        [MaxLength(8)]
        public string OrderDate { get; set; }

        [Required]
        public int Seq { get; set; }
    }
}
