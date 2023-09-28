using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [Table("SisTarefa")]
    public class Tarefa : CamposPadraoCRUD
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public decimal? Ordem { get; set; }

        [ForeignKey("Projeto"), Column("SisProjetoId")]
        public long? ProjetoId { get; set; }
        public Projeto Projeto { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataRegistro { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataPrevistaInicio { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataPrevistaTermino { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataInicio { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataTermino { get; set; }

        public long? ResponsavelId { get; set; }

        [ForeignKey("Modulo"), Column("SisModuloId")]
        public long? ModuloId { get; set; }
        public DocRotulo Modulo { get; set; }

        [ForeignKey("Status"), Column("SisStatusId")]
        public long? StatusId { get; set; }
        public DocRotulo Status { get; set; }

        [ForeignKey("Prioridade"), Column("SisPrioridadeId")]
        public long? PrioridadeId { get; set; }
        public DocRotulo Prioridade { get; set; }

        [ForeignKey("TipoTarefa"), Column("SisTipoTarefaId")]
        public long? TipoTarefaId { get; set; }
        public DocRotulo TipoTarefa { get; set; }

        [ForeignKey("Nivel1"), Column("SisNivel1Id")]
        public long? Nivel1Id { get; set; }
        public DocRotulo Nivel1 { get; set; }

        [ForeignKey("Nivel2"), Column("SisNivel2Id")]
        public long? Nivel2Id { get; set; }
        public DocRotulo Nivel2 { get; set; }

        [ForeignKey("Nivel3"), Column("SisNivel3Id")]
        public long? Nivel3Id { get; set; }
        public DocRotulo Nivel3 { get; set; }

        public long? ClienteId { get; set; }
        public string Conteudo { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? IntervaloInicio { get; set; }

        //     public string TempoDecorrido { get; set; }
    }
}
