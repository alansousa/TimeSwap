using System;
using TimeSwap.Models;

namespace TimeSwap.Model
{
    public class RecursoTarefaExecutado
    {
        public int id { get; set; }

        public string recursoId { get; set; }
        public string projetoId { get; set; }
        public int faseId { get; set; }
        public string tarefaId { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataHoraFim { get; set; }


        public RECURSOTAREFA RECURSOTAREFA { get; set; }
    }
}
