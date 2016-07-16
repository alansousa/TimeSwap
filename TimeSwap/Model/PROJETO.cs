using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Models
{
    public partial class PROJETO
    {
        public PROJETO()
        {
            FASE = new List<FASE>();
        }
    
        [Key]
        public string CODIGO { get; set; }
        [Required]
        public string NOME { get; set; }
        [Required]
        public string GERENTEID { get; set; }
        [Required]
        public string CAMINHO { get; set; }
    
        public virtual ICollection<FASE> FASE { get; set; }
        public virtual RECURSO RECURSO { get; set; }
    }
}
