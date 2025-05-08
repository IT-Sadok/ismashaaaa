using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeupClone.Infrastructure.Data.Entities
{
    [Table("Products")]
    public class ProductEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public CategoryEntity Category { get; set; }

        [Required]
        public Guid BrandId { get; set; }

        [ForeignKey("BrandId")]
        public BrandEntity Brand { get; set; }
    }
}
