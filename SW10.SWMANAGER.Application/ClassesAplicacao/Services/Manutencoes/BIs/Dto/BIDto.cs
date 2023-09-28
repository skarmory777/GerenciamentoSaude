using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Manutencoes.BIs;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs.Dto
{
    [AutoMap(typeof(BI))]
    public class BIDto : CamposPadraoCRUDDto, IDescricao
    {
        public long? ModuloId { get; set; }
        public long? OperacaoId { get; set; }
        public string Url { get; set; }
        public bool IsPublico { get; set; }
        public bool IsPrincipal { get; set; }

        public ModuloDto Modulo { get; set; }

        public OperacaoDto Operacao { get; set; }
    }
}
