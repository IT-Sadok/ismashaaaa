using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MakeupClone.Domain.Entities;

namespace MakeupClone.Infrastructure.Data.Entities
{
    [Table("Reviews")]
    public class ReviewEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [ForeignKey("ProductId")]
        public ProductEntity Product { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public string Comment { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
