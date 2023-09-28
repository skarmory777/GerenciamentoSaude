using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto
{
    [AutoMap(typeof(FaturamentoItemTabela))]
    public class FaturamentoItemTabelaDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }

        public override string Descricao { get; set; }

        public virtual FaturamentoTabelaDto Tabela { get; set; }
        public long? TabelaId { get; set; }

        public virtual FaturamentoItemDto Item { get; set; }
        public long? ItemId { get; set; }

        public virtual SisMoedaDto SisMoeda { get; set; }
        public long? SisMoedaId { get; set; }

        public DateTime VigenciaDataInicio { get; set; }

        public float? COCH { get; set; }

        public float? HMCH { get; set; }

        public float? ValorTotal { get; set; }

        public bool IsAtivo { get; set; }

        public int? Auxiliar { get; set; }

        public int? Porte { get; set; }

        public float? Filme { get; set; }

        public float Preco { get; set; }

        public bool IsInclusaoManual { get; set; }
    }
}
