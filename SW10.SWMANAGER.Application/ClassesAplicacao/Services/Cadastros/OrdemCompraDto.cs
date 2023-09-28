using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros
{
    [AutoMap(typeof(OrdemCompra))]
    public class OrdemCompraDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public static OrdemCompraDto Mapear(OrdemCompra ordemCompra)
        {
            var ordemCompraDto = new OrdemCompraDto();

            ordemCompraDto.Id = ordemCompra.Id;
            ordemCompraDto.Codigo = ordemCompra.Codigo;
            ordemCompraDto.Descricao = ordemCompra.Descricao;

            return ordemCompraDto;
        }

        public static OrdemCompra Mapear(OrdemCompraDto ordemCompraDto)
        {
            var ordemCompra = new OrdemCompra();

            ordemCompra.Id = ordemCompraDto.Id;
            ordemCompra.Codigo = ordemCompraDto.Codigo;
            ordemCompra.Descricao = ordemCompraDto.Descricao;

            return ordemCompra;
        }

        public static List<OrdemCompraDto> Mapear(List<OrdemCompra> ordensCompras)
        {
            if (ordensCompras != null)
            {
                return null;
            }

            var ordensDto = new List<OrdemCompraDto>();

            foreach (var item in ordensCompras)
            {
                ordensDto.Add(Mapear(item));
            }


            return ordensDto;
        }

    }
}
