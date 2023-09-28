using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Relatorios
{

    [AutoMap(typeof(RelatorioEntradaModelDto))]
    public class RelatorioEntradaModel : RelatorioEntradaModelDto
    {
        public RelatorioEntradaModel(RelatorioEntradaModelDto dto)
        {
            dto.MapTo(this);
        }
    }
}