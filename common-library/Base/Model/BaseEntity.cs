using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common_library.Base.Model
{
    public class BaseEntity
    {
        [Required]
        public bool IsDeleted { get; set; } = false;
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string createBy { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string ModifiedBy { get; set; } = string.Empty;
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.Now;
        [Required]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}
