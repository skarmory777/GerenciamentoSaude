using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class InventarioDto : CamposPadraoCRUDDto
    {
        public DateTime DataInventario { get; set; }
        public long TipoInventarioId { get; set; }
        public long StatusInventarioId { get; set; }
        public long EstoqueId { get; set; }
        public string Status { get; set; }
        public string ItensJson { get; set; }
        public string EstoqueDescricao { get; set; }

        public static InventarioDto Mapear(Inventario inventario)
        {
            var inventarioDto = MapearBase<InventarioDto>(inventario);

            inventarioDto.DataInventario = inventario.DataInventario;
            inventarioDto.TipoInventarioId = inventario.TipoInventarioId;
            inventarioDto.StatusInventarioId = inventario.StatusInventarioId;
            inventarioDto.EstoqueId = inventario.EstoqueId;
           // inventarioDto.EstoqueDescricao = inventario.EstoqueDescricao;

            return inventarioDto;
        }

    }
}
