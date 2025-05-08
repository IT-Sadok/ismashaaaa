using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeupClone.Infrastructure.Data.Entities
{
    [Table("Categories")]
    public class CategoryEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<ProductEntity> Products { get; set; }
    }
}
