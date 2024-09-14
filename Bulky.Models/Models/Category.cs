using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]  // by default ID is treated as primary key we have us [KEY] when name is not ID eg - EmpID
        public int Id { get; set; }  // type Prop then tab to get general syntax of table coloumn


        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }


        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage = "The field Display Order must be between 1 - 100.")]
        public int DisplayOrder { get; set; }
    }
}
