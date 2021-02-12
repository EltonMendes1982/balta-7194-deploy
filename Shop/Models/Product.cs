using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Informe o título")]
        [MaxLength(60, ErrorMessage = "Tamanho do campo deve ser entre 3 e 60 caractéres")]
        [MinLength(3, ErrorMessage = "Tamanho do campo deve ser entre 3 e 60 caractéres")]        
        [DataType("nvarchar")]
        public string Title { get; set; }

        [MaxLength(1024, ErrorMessage = "Tamanho máximo do campo deve ser entre 1024 caractéres")]
        [DataType("nvarchar")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Informe o preço")]
        [Range(1, int.MaxValue, ErrorMessage ="Preço deve ser maior que zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Informe a categoria")]
        [Range(1, int.MaxValue, ErrorMessage = "Categoria inválida")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}