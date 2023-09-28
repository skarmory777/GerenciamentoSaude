using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto
{
    [AutoMap(typeof(KitExameItem))]
    public class KitExameItemDto : CamposPadraoCRUDDto
    {
        public long KitExameId { get; set; }
        public long FaturamentoItemId { get; set; }
        public bool IsLiberaKit { get; set; }

        public KitExameDto KitExame { get; set; }

        public FaturamentoItemDto FaturamentoItem { get; set; }
    }
}
