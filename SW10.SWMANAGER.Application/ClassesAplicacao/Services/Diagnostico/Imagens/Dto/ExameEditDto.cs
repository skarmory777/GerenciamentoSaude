using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    [AutoMapFrom(typeof(LaudoMovimento))]
    public class ExameEditDto : CamposPadraoCRUDDto
    {
        public new long? Id { get; set; }

        public long AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }

        public long LaudoMovimentoStatusId { get; set; }
        public LaudoMovimentoStatusDto LaudoMovimentoStatus { get; set; }

        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long? LeitoId { get; set; }
        public LeitoDto Leito { get; set; }

        public bool IsContraste { get; set; }
        public string QtdeConstraste { get; set; }

        [StringLength(255)]
        public string Obs { get; set; }

        public ExameEditDto()
        {
            Atendimento = new AtendimentoDto();
            LaudoMovimentoStatus = new LaudoMovimentoStatusDto();
            Convenio = new ConvenioDto();
            Leito = new LeitoDto();
        }
    }
}