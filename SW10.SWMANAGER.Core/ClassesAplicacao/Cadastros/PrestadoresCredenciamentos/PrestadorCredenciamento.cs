using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Prestadores;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.PrestadoresCredenciamentos
{
    [Table("PrestadorCredenciamento")]
    public class PrestadorCredenciamento : CamposPadraoCRUD
    {
        public long? PrestadorId { get; set; }
        [ForeignKey("PrestadorId")]
        public Prestador Prestador { get; set; }

        public long? ConvenioId { get; set; }
        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        [Index("Idx_DataInicio")]
        public DateTime DataInicio { get; set; }
        [Index("Idx_DataFim")]
        public DateTime DataFim { get; set; }

        public int CodCredenciamento { get; set; }
    }
}
