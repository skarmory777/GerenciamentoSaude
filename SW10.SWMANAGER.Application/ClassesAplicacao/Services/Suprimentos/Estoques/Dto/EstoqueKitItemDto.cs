using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class EstoqueKitItemDto : CamposPadraoCRUDDto
    {
        public long? EstoqueKitId { get; set; }
        public EstoqueKitDto EstoqueKit { get; set; }
        public int Quantidade { get; set; }
        public long ProdutoId { get; set; }
        public ProdutoDto Produto { get; set; }
        public long? UnidadeId { get; set; }
        public UnidadeDto Unidade { get; set; }

        public static EstoqueKitItemDto Mapear(EstoqueKitItem estoqueKitItem)
        {
            EstoqueKitItemDto estoqueKitItemDto = new EstoqueKitItemDto();

            estoqueKitItemDto.Id = estoqueKitItem.Id;
            estoqueKitItemDto.Codigo = estoqueKitItem.Codigo;
            estoqueKitItemDto.Descricao = estoqueKitItem.Descricao;


            estoqueKitItemDto.EstoqueKitId = estoqueKitItem.EstoqueKitId;
            estoqueKitItemDto.Quantidade = estoqueKitItem.Quantidade;
            estoqueKitItemDto.ProdutoId = estoqueKitItem.ProdutoId;
            estoqueKitItemDto.UnidadeId = estoqueKitItem.UnidadeId;

            if (estoqueKitItem.Produto != null)
            {
                estoqueKitItemDto.Produto = ProdutoDto.Mapear(estoqueKitItem.Produto);
            }

            if (estoqueKitItem.EstoqueKit != null)
            {
                estoqueKitItemDto.EstoqueKit = EstoqueKitDto.Mapear(estoqueKitItem.EstoqueKit);
            }

            if (estoqueKitItem.Unidade != null)
            {
                estoqueKitItemDto.Unidade = UnidadeDto.Mapear(estoqueKitItem.Unidade);
            }

            return estoqueKitItemDto;
        }

        public static EstoqueKitItem Mapear(EstoqueKitItemDto estoqueKitItemDto)
        {
            EstoqueKitItem estoqueKitItem = new EstoqueKitItem();

            estoqueKitItem.Id = estoqueKitItemDto.Id;
            estoqueKitItem.Codigo = estoqueKitItemDto.Codigo;
            estoqueKitItem.Descricao = estoqueKitItemDto.Descricao;


            estoqueKitItem.EstoqueKitId = estoqueKitItemDto.EstoqueKitId;
            estoqueKitItem.Quantidade = estoqueKitItemDto.Quantidade;
            estoqueKitItem.ProdutoId = estoqueKitItemDto.ProdutoId;

            if (estoqueKitItemDto.Produto != null)
            {
                estoqueKitItem.Produto = ProdutoDto.Mapear(estoqueKitItemDto.Produto);
            }

            if (estoqueKitItemDto.EstoqueKit != null)
            {
                estoqueKitItem.EstoqueKit = EstoqueKitDto.Mapear(estoqueKitItemDto.EstoqueKit);
            }

            return estoqueKitItem;
        }

        public static List<EstoqueKitItemDto> Mapear(List<EstoqueKitItem> itens)
        {
            List<EstoqueKitItemDto> itensDto = new List<EstoqueKitItemDto>();

            foreach (var item in itens)
            {
                itensDto.Add(EstoqueKitItemDto.Mapear(item));
            }

            return itensDto;
        }

        public static List<EstoqueKitItem> Mapear(List<EstoqueKitItemDto> itensDto)
        {
            List<EstoqueKitItem> itens = new List<EstoqueKitItem>();

            foreach (var item in itensDto)
            {
                itens.Add(EstoqueKitItemDto.Mapear(item));
            }

            return itens;
        }
    }
}
