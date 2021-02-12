using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Category
    {
        [Key]
        [MaxLength(18, ErrorMessage= "Erro de PK")]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Informe o título")]
        [MaxLength(60, ErrorMessage = "Tamanho do campo deve ser entre 3 e 60 caractéres")]
        [MinLength(3, ErrorMessage = "Tamanho do campo deve ser entre 3 e 60 caractéres")]        
        [DataType("nvarchar")]
        public string Title { get; set; }
    }
}