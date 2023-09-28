using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes.Dto
{
    [AutoMap(typeof(FaturamentoItemAutorizacao))]
    public class FaturamentoItemAutorizacaoDto : CamposPadraoCRUDDto
    {
        public long ConvenioId { get; set; }
        public long? FaturamentoItemId { get; set; }
        public long? FaturamentoGrupoId { get; set; }
        public long? FaturamentoSubGrupoId { get; set; }

        public ConvenioDto Convenio { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public FaturamentoGrupoDto FaturamentoGrupo { get; set; }
        public FaturamentoSubGrupoDto FaturamentoSubGrupo { get; set; }
    }
}
