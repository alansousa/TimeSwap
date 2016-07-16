
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSwap.Models
{
    public partial class FASE
    {
        public FASE()
        {
            TAREFA = new HashSet<TAREFA>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FASEID { get; set; }
        public string PROJETOID { get; set; }
        public string DESCRICAO { get; set; }
        public DateTime? INICIOREAL { get; set; }
        public DateTime? FIMREAL { get; set; }
        public DateTime? INICIOBASE { get; set; }
        public DateTime? FIMBASE { get; set; }
        public double? DURACAO { get; set; }
    
        public virtual PROJETO PROJETO { get; set; }
        public virtual ICollection<TAREFA> TAREFA { get; set; }
    }
}
