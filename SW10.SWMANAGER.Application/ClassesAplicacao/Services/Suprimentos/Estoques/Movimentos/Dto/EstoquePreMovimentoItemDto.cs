using Abp.AutoMapper;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(EstoquePreMovimentoItem))]
    public class EstoquePreMovimentoItemDto : CamposPadraoCRUDDto
    {
        public long PreMovimentoId { get; set; }
        public long ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public string NumeroSerie { get; set; }
        public decimal CustoUnitario { get; set; }
        public long? ProdutoUnidadeId { get; set; }
        public string CodigoBarra { get; set; }
        public decimal PerIPI { get; set; }
        public decimal ValorIPI { get; set; }
        public decimal ValorICMS { get;  set; }
        public decimal PerICMS { get; set; }
        public long? PreMovimentoItemEstadoId { get; set; }
        public long? LoteValidadeId { get; set; }
        public long? LaboratorioId { get; set; }
        public string Lote { get; set; }
        public long? EstoqueKitItemId { get; set; }
        public DateTime? Validade { get; set; }
        public EstoqueLaboratorio Laboratorio { get; set; }
        public LoteValidade LoteValidade { get; set; }

        public virtual ProdutoDto Produto { get; set; }
        public virtual EstoquePreMovimentoDto EstoquePreMovimento { get; set; }
        public virtual UnidadeDto ProdutoUnidade { get; set; }


        public long EstoquePreMovimentoLoteValidadeId { get; set; }
        public long EstoqueId { get; set; }
        public long? IdGrid { get; set; }
        public decimal? QuantidadeAtendida { get; set; }

        public string ItensLoteValidade { get; set; }
        public string ItensNumeroSerie { get; set; }

        public List<EstoquePreMovimentoLoteValidadeDto> PreMovimentoLotesValidades { get; set; }

        public List<LoteValidadeGridDto> ItensDtos { get; set; }
        
        public static EstoquePreMovimentoItemDto MapPreMovimentoItem(EstoquePreMovimentoItem preMovimentoItem)
        {
            if (preMovimentoItem == null) return null;

            var preMovimentoItemDto = MapearBase<EstoquePreMovimentoItemDto>(preMovimentoItem);

            preMovimentoItemDto.NumeroSerie = preMovimentoItem.NumeroSerie;
            preMovimentoItemDto.PreMovimentoId = preMovimentoItem.PreMovimentoId;
            preMovimentoItemDto.ProdutoId = preMovimentoItem.ProdutoId;
            preMovimentoItemDto.ProdutoUnidadeId = preMovimentoItem.ProdutoUnidadeId;
            preMovimentoItemDto.Quantidade = preMovimentoItem.Quantidade;
            preMovimentoItemDto.PerIPI = preMovimentoItem.PerIPI;
            preMovimentoItemDto.ValorIPI = preMovimentoItem.ValorIPI;
            preMovimentoItemDto.PerICMS = preMovimentoItem.PerICMS;
            preMovimentoItemDto.ValorICMS = preMovimentoItem.ValorICMS;
            preMovimentoItemDto.PreMovimentoItemEstadoId = preMovimentoItem.PreMovimentoItemEstadoId;
            preMovimentoItemDto.CustoUnitario = preMovimentoItem.CustoUnitario;

            if (preMovimentoItem.Produto != null)
            {
                preMovimentoItemDto.Produto = new ProdutoDto
                {
                    Id = preMovimentoItem.Produto.Id,
                    IsValidade = preMovimentoItem.Produto.IsValidade,
                    Descricao = preMovimentoItem.Produto.Descricao
                };
            }

            if (preMovimentoItem.ProdutoUnidade != null)
            {
                preMovimentoItemDto.ProdutoUnidade = UnidadeDto.Mapear(preMovimentoItem.ProdutoUnidade);
            }

            if(!preMovimentoItem.EstoquePreMovimentosLoteValidades.IsNullOrEmpty())
            {
                preMovimentoItemDto.PreMovimentoLotesValidades = preMovimentoItem.EstoquePreMovimentosLoteValidades.ToList().Select(EstoquePreMovimentoLoteValidadeDto.Mapear).ToList();
            }

            return preMovimentoItemDto;
        }

        public static EstoquePreMovimentoItem MapPreMovimentoItem(EstoquePreMovimentoItemDto preMovimentoItemDto)
        {
            var preMovimentoItem = MapearBase<EstoquePreMovimentoItem>(preMovimentoItemDto);

            preMovimentoItem.NumeroSerie = preMovimentoItemDto.NumeroSerie;
            preMovimentoItem.PreMovimentoId = preMovimentoItemDto.PreMovimentoId;
            preMovimentoItem.ProdutoId = preMovimentoItemDto.ProdutoId;
            preMovimentoItem.ProdutoUnidadeId = preMovimentoItemDto.ProdutoUnidadeId;
            preMovimentoItem.Quantidade = preMovimentoItemDto.Quantidade;
            preMovimentoItem.PerIPI = preMovimentoItemDto.PerIPI;
            preMovimentoItem.ValorIPI = preMovimentoItemDto.ValorIPI;
            preMovimentoItem.PerICMS = preMovimentoItemDto.PerICMS;
            preMovimentoItem.ValorICMS = preMovimentoItemDto.ValorICMS;
            preMovimentoItem.CustoUnitario = preMovimentoItemDto.CustoUnitario;
            preMovimentoItem.EstoqueKitItemId = preMovimentoItemDto.EstoqueKitItemId;

            return preMovimentoItem;
        }
        
        public static List<EstoquePreMovimentoItem> MapPreMovimentoItem(List<EstoquePreMovimentoItemDto> preMovimentoItensDto)
        {
            var preMovimentoItens = new List<EstoquePreMovimentoItem>();

            foreach (var item in preMovimentoItensDto)
            {
                preMovimentoItens.Add(MapPreMovimentoItem(item));
            }

            return preMovimentoItens;
        }

        public static List<EstoquePreMovimentoItemDto> MapPreMovimentoItem(List<EstoquePreMovimentoItem> preMovimentoItens)
        {
            var preMovimentoItensDto = new List<EstoquePreMovimentoItemDto>();

            foreach (var item in preMovimentoItens)
            {
                preMovimentoItensDto.Add(MapPreMovimentoItem(item));
            }

            return preMovimentoItensDto;
        }
        
    }
}
