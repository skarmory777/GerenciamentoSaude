using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Imagens
{
    [AutoMapFrom(typeof(GetExameForEditOutput))]
    public class CreateOrEditExameModalViewModel : GetExameForEditOutput
    {
        public long? AtendimentoId { get; set; }
        public string AtendimentoCodigo { get; set; }
        public string PacienteNome { get; set; }
        public long? ConvenioId { get; set; }
        public string ConvenioNome { get; set; }
        public long? LeitoId { get; set; }
        public string LeitoNome { get; set; }
        public long? MedicoId { get; set; }
        public string MedicoNome { get; set; }
        public long? TecnicoId { get; set; }
        public string TecnicoNome { get; set; }
        public long? TipoLeitoId { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public long? LocalUtilizacaoId { get; set; }
        public string LocalUtilizacaoDescricao { get; set; }
        public long? CentroCustoId { get; set; }
        public string CentroCustoDescricao { get; set; }
        public long? TurnoId { get; set; }
        public string TurnoDescricao { get; set; }
        public bool IsContraste { get; set; }
        public string QtdContraste { get; set; }
        public string Lote { get; set; }
        public string Venoso { get; set; }
        public long? SolicitacaoExameId { get; set; }
        public string SolicitacaoExameDescricao { get; set; }

        public long? LaudoMovimentoStatusId { get; set; }
        public string LaudoMovimentoStatusDescricao { get; set; }


        // NOVO MODELO
        public long? FatItemId { get; set; }
        public long? SolicitacaoExameItemId { get; set; }
        // FIM - NOVO MODELO


        public CreateOrEditExameModalViewModel(GetExameForEditOutput output)
        {
            //if (output)
            //{

            //}

            //    output.MapTo(this);
            Exame = output.Exame;
            AtendimentoId = output.Exame != null ? (long?)output.Exame.Atendimento.Id : null;
            AtendimentoCodigo = output.Exame != null ? output.Exame.Atendimento.Codigo : null;
            PacienteNome = output.Exame != null ? output.Exame.Atendimento.Paciente.NomeCompleto : null;
            ConvenioId = output.Exame != null ? (long?)output.Exame.Convenio.Id : null;
            ConvenioNome = output.Exame != null ? output.Exame.Convenio.NomeFantasia : null;
            LeitoId = output.Exame != null ? (long?)output.Exame.Leito.Id : null;
            LeitoNome = output.Exame != null ? output.Exame.Leito.Descricao : null;
            MedicoId = output.Exame != null ? (long?)output.Exame.Atendimento.Medico.Id : null;
            MedicoNome = output.Exame != null ? output.Exame.Atendimento.Medico.NomeCompleto : null;
            TecnicoId = null;
            TecnicoNome = null;
            TipoLeitoId = null;
            TipoLeitoDescricao = output.Exame != null ? output.Exame.Leito.Descricao : null;
            LocalUtilizacaoId = null;
            LocalUtilizacaoDescricao = null;
            CentroCustoId = null;
            CentroCustoDescricao = null;
            TurnoId = null;
            TurnoDescricao = null;
            IsContraste = output.Exame != null ? output.Exame.IsContraste : false;
            QtdContraste = output.Exame != null ? output.Exame.QtdeConstraste : null;
            Lote = null;
            Venoso = null;
            SolicitacaoExameId = null; // output.Exame != null ? (long?)output.Exame.Leito.Id : null;  
            SolicitacaoExameDescricao = null; // output.Exame != null ? (long?)output.Exame.Leito.Id : null;
            LaudoMovimentoStatusId = output.Exame != null ? (long?)output.Exame.LaudoMovimentoStatusId : null;
            LaudoMovimentoStatusDescricao = null; //output.Exame != null ? (long?)output.Exame.Leito.Id : null;

            if (output.Exame == null)
            {
                Exame = new ExameEditDto();
                Exame.Id = 0;
            }


        }

        public bool IsEditMode
        {
            get
            {
                // temp
                if (Exame != null)
                    return Exame.Id.HasValue;
                else return false;
            }
        }
    }
}