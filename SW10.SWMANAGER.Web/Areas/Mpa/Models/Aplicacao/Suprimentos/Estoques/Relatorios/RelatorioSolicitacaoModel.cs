using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Relatorios
{

    [AutoMap(typeof(RelatorioSolicitacaoSaidaModelDto))]
    public class RelatorioSolicitacaoModel : RelatorioSolicitacaoSaidaModelDto
    {
        public RelatorioSolicitacaoModel(RelatorioSolicitacaoSaidaModelDto dto)
        {
            dto.MapTo(this);
        }
    }
}