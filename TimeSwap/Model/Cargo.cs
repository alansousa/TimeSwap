
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSwap.Models
{
    public partial class CARGO
    {
        public CARGO()
        {
            RECURSO = new List<RECURSO>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="C�digo")]
        public int ID { get; set; }

        [Display(Name ="Descri��o do Cargo")]
        [Required]
        public string CARGO1 { get; set; }
    
        public virtual ICollection<RECURSO> RECURSO { get; set; }
    }
}
