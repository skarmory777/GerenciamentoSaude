namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Dto
{
    using System;
    using System.Collections.Generic;


    public class ListaEvolucaoResultado
    {
        public IEnumerable<EvolucaoResultadoDto> Coletas { get; set; }
        
        public IEnumerable<EvolucaoResultadoDto> Culturas { get; set; }
    }
    public class EvolucaoResultadoDto : CamposPadraoCRUDDto
    {
        public string Referencia { get; set; }

        public int? Ordem { get; set; }

        public int? OrdemMapaResultado { get; set; }

        public string ItemDescricao { get; set; }

        public string ItemInfo { get; set; }

        public long? TipoResultadoId { get; set; }

        public bool HasAmbulatorioEmergencia { get; set; }

        public bool HasInternacao { get; set; }

        public List<EolucaoResultadoComparativoDto> Resultados { get; set; }

        public bool Numerico { get; set; }

        public object FormataOrdem { get;  set; }
    }

    public class EolucaoResultadoComparativoDto
    {
        /// <summary>
        /// Gets or sets the resultado.
        /// </summary>
        public string Resultado { get; set; }

        /// <summary>
        /// Gets or sets the data coleta.
        /// </summary>
        public DateTime? DataColeta { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether numerico.
        /// </summary>
        public bool Numerico { get; set; }

        /// <summary>
        /// Gets or sets the valor.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Gets or sets the comparativo vs anterior.
        /// </summary>
        public double ComparativoVsAnterior { get; set; }

        public string CorFundo { get; set; }

        public string CorTexto { get; set; }

        public string TooltipValor { get; set; }

    }
}
