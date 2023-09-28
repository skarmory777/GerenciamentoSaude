using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class EstoqueKitDto : CamposPadraoCRUDDto
    {
        public List<EstoqueKitItemDto> ItensDto { get; set; }

        public static EstoqueKitDto Mapear(EstoqueKit estoqueKit)
        {
            EstoqueKitDto estoqueKitDto = new EstoqueKitDto();

            estoqueKitDto.Id = estoqueKit.Id;
            estoqueKitDto.Codigo = estoqueKit.Codigo;
            estoqueKitDto.Descricao = estoqueKit.Descricao;

            if (estoqueKit.Itens != null)
            {
                estoqueKitDto.ItensDto = new List<EstoqueKitItemDto>();

                foreach (var item in estoqueKit.Itens)
                {
                    estoqueKitDto.ItensDto.Add(null);
                }
            }


            return estoqueKitDto;
        }

        public static EstoqueKit Mapear(EstoqueKitDto estoqueKitDto)
        {
            EstoqueKit estoqueKit = new EstoqueKit();

            estoqueKit.Id = estoqueKitDto.Id;
            estoqueKit.Codigo = estoqueKitDto.Codigo;
            estoqueKit.Descricao = estoqueKitDto.Descricao;

            return estoqueKit;
        }

        public static List<EstoqueKitDto> Mapear(List<EstoqueKit> estoqueKits)
        {
            List<EstoqueKitDto> estoquesKitsDto = new List<EstoqueKitDto>();

            foreach (var item in estoqueKits)
            {
                estoquesKitsDto.Add(EstoqueKitDto.Mapear(item));
            }

            return estoquesKitsDto;
        }


        public static List<EstoqueKit> Mapear(List<EstoqueKitDto> estoqueKitsDto)
        {
            List<EstoqueKit> estoquesKits = new List<EstoqueKit>();

            foreach (var item in estoqueKitsDto)
            {
                estoquesKits.Add(EstoqueKitDto.Mapear(item));
            }

            return estoquesKits;
        }

    }
}
