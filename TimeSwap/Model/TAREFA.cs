
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSwap.Models
{


    public partial class TAREFA
    {
        public TAREFA()
        {
            RECURSOTAREFA = new HashSet<RECURSOTAREFA>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PROJETOID { get; set; }
        [Key]
        public int FASEID { get; set; }
        [Key]
        public string TAREFAID { get; set; }
        public string DESCRICAO { get; set; }
        public int? TIPO { get; set; }
        public int? LINHA { get; set; }
        public int? LINHADEP { get; set; }
        public double? DURACAO { get; set; }
        public DateTime? INICIOREAL { get; set; }
        public DateTime? FIMREAL { get; set; }
        public DateTime? INICIOBASE { get; set; }
        public DateTime? FIMBASE { get; set; }
        public double? PORCENTAGEM { get; set; }

        public virtual FASE FASE { get; set; }
        public virtual ICollection<RECURSOTAREFA> RECURSOTAREFA { get; set; }
    }
}
