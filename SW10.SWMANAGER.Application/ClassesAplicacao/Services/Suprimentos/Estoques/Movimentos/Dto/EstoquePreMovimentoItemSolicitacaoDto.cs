using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class EstoquePreMovimentoItemSolicitacaoDto
    {
        public long Id { get; set; }
        public long PreMovimentoId { get; set; }
        public long? PreMovimentoItemId { get; set; }
        public long ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public string NumeroSerie { get; set; }
        public long? ProdutoUnidadeId { get; set; }
        public string Produto { get; set; }
        public string ProdutoUnidade { get; set; }
        public string CodigoProduto { get; set; }
        public long? IdGrid { get; set; }
        public decimal QuantidadeSolicitada { get; set; }
        public decimal? QuantidadeAtendida { get; set; }
        public string LotesValidadesJson { get; set; }
        public string NumerosSerieJson { get; set; }
        public long EstadoSolicitacaoItemId { get; set; }       
        public long LoteSugeridoId { get;  set; }
        public string LoteSugeridoName { get;  set; }
        public bool IsLote { get;  set; }
        public long? EstoqueKitItemId { get; set; }
        public static List<EstoquePreMovimentoItemSolicitacaoDto> Clonar(IEnumerable<EstoquePreMovimentoItemSolicitacaoDto> items)
        {
            string serialized = JsonConvert.SerializeObject(items);
            return JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(serialized);
        }

        public static long CalcularEstadoSolicitacaoItem(decimal qtdSolicitada, decimal? qtdAtendida)
        {
            if (!qtdAtendida.HasValue || qtdAtendida == 0)            
                return (long)EnumPreMovimentoEstado.Pendente;

            if (qtdSolicitada == qtdAtendida)
                return (long)EnumPreMovimentoEstado.TotalmenteAtendido;

            if (qtdSolicitada > qtdAtendida)
                return (long)EnumPreMovimentoEstado.ParcialmenteAtendido;

            return (long)EnumPreMovimentoEstado.Pendente;
        }

        internal static EstoquePreMovimentoItemSolicitacaoDto Mapear(EstoqueSolicitacaoItem entity)
        {
            if (entity == null) return null;

            var dto = new EstoquePreMovimentoItemSolicitacaoDto()
            {
                Id = entity.Id,
                ProdutoId = entity.ProdutoId,
                Quantidade = entity.Quantidade,
                EstadoSolicitacaoItemId = entity.EstadoSolicitacaoItemId,
                ProdutoUnidadeId = entity.ProdutoUnidadeId,
                PreMovimentoId = entity.SolicitacaoId,
                QuantidadeAtendida = entity.QuantidadeAtendida
            };

            return dto;
        }
    }
}
