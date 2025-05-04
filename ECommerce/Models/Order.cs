using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{

    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(16)]
        public string OrderNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Customer { get; set; }

        [Required]
        [MaxLength(2)]
        public string Status { get; set; }

        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? ShipDate { get; set; }

        [MaxLength(5000)]
        public string Remark { get; set; }
    }

}
