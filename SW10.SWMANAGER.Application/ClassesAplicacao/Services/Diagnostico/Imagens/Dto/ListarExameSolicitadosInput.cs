using Abp.Extensions;
using Abp.Runtime.Validation;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    public class ListarExameSolicitadosInput : ListarInput, IShouldNormalize
    {
        public long? AtendimentoId { get; set; }
        public long? EmpresaId { get; set; }
        public DateTime? EmissaoDe { get; set; }
        public DateTime? EmissaoAte { get; set; }

        public long? MedicoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? UnidadeId { get; set; }

        public long? PacienteId { get; set; }

        public string TipoAtendimento { get; set; }



        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Paciente.NomeCompleto";
            }
        }
    }
}
