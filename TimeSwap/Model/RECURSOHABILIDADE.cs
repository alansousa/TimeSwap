//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace TimeSwap.Models
{

    public partial class RECURSOHABILIDADE
    {
        [Key]
        public string RECURSOID { get; set; }
        public int HABILIDADEID { get; set; }
        public int? NIVEL { get; set; }
    
        public virtual HABILIDADE HABILIDADE { get; set; }
        public virtual RECURSO RECURSO { get; set; }
    }
}