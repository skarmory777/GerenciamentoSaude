using System;
using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    public class ValorCodigoTabela
    {
        public float Valor { get; set; }
        public long? TabelaId { get; set; }
        public long? FaturamentoItemCobradoId { get; set; }
        
        public float ValorTotal { get; set; }
        
        public ResumoDetalhamento ResumoDetalhamento { get; set; }
    }
}
