using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    [AutoMap(typeof(FormConfigOperacao))]
    public class FormConfigOperacaoDto : CamposPadraoCRUDDto
    {
        public long? FormConfigId { get; set; }

        public long? OperacaoId { get; set; }

        public FormConfigDto FormConfig { get; set; }

        public OperacaoDto Operacao { get; set; }

        public string OperacoesIncluidas { get; set; }

        public string OperacoesRemovidas { get; set; }

        public ICollection<OperacaoDto> ListaDisponiveis { get; set; }

        public ICollection<OperacaoDto> ListaAssociadas { get; set; }
    }
}
