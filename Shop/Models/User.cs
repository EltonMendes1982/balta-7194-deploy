using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Informe o username")]
        [MaxLength(20, ErrorMessage = "Tamanho do campo deve ser entre 3 e 20 caractéres")]
        [MinLength(3, ErrorMessage = "Tamanho do campo deve ser entre 3 e 20 caractéres")]        
        [DataType("nvarchar")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Informe o passoword")]
        [MaxLength(20, ErrorMessage = "Tamanho do campo deve ser entre 3 e 20 caractéres")]
        [MinLength(3, ErrorMessage = "Tamanho do campo deve ser entre 3 e 20 caractéres")]
        [DataType("nvarchar")]
        public string PassWord { get; set; }

        [DataType("nvarchar")]
        public string Role { get; set; }
    }
}