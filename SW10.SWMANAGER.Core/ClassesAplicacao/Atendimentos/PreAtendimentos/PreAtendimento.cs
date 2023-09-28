using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.PreAtendimentos
{
    [Table("PreAtendimento")]
    public class PreAtendimento : CamposPadraoCRUD
    {

        public string NomeCompleto { get; set; }

        [Index("Idx_Nascimento")]
        [DataType(DataType.DateTime)]
        public DateTime Nascimento { get; set; }

        [Index("Idx_DataRegistro")]
        [DataType(DataType.DateTime)]
        public DateTime DataRegistro { get; set; }

        public int? Sexo { get; set; }

        public string Telefone { get; set; }

        public string Observacao { get; set; }
    }
}
