namespace SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto
{
    using Abp.Extensions;
    using System;

    public class FiltroDisparoDeMensagemViewModel : ListarInput
    {
        public DateTime? NascimentoInicio { get; set; }
        public DateTime? NascimentoFinal { get; set; }

        public string Filtro { get; set; }

        public long? PacienteId { get; set; }

        public long? SexoId { get; set; }

        public long? PaisId { get; set; }

        public long? EstadoId { get; set; }

        public long? CidadeId { get; set; }

        public string Bairro { get; set; }

        public bool HabilitarFiltroAtendimento { get; set; }

        public bool UltimoAtendimento { get; set; }

        public bool NaoEnviarPacienteObito { get; set; }

        public bool NaoInternado { get; set; }

        public DateTime? AtendimentoInicio { get; set; }
        public DateTime? AtendimentoFinal { get; set; }

        public DateTime? AtendimentoAltaFinal { get;  set; }
        public DateTime? AtendimentoAltaInicio { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public long? StatusAtendimentoId { get; set; }

        public long? ConvenioId { get; set; }

        public long? PlanoId { get; set; }

        public bool AllRows { get; set; }



        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Pessoa.NomeCompleto DESC";
            }
        }
    }
}
