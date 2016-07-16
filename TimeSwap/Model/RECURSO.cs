using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSwap.Models
{
    public class RECURSO
    {
        public RECURSO()
        {
            PROJETO = new List<PROJETO>();
            RECURSOHABILIDADE = new List<RECURSOHABILIDADE>();
            RECURSOTAREFA = new List<RECURSOTAREFA>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Matrícula")]
        public string MATRICULA { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string NOME { get; set; }

        [Required]
        [Display(Name = "Cargo")]
        public int CARGOID { get; set; }
        public string TELEFONE { get; set; }
        public string CPF { get; set; }
        public string LOGIN { get; set; }

        [DataType(DataType.Password)]
        public string SENHA { get; set; }

        [DataType(DataType.Currency)]
        public double? HOMEMHORA { get; set; }

        [DataType(DataType.EmailAddress)]
        public string EMAIL { get; set; }

        public virtual CARGO CARGO { get; set; }
        public virtual ICollection<PROJETO> PROJETO { get; set; }
        public virtual ICollection<RECURSOHABILIDADE> RECURSOHABILIDADE { get; set; }
        public virtual ICollection<RECURSOTAREFA> RECURSOTAREFA { get; set; }
    }
}
