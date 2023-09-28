using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [Table("SisProjeto")]
    public class Projeto : CamposPadraoCRUD
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataCriacao { get; set; }

        public string Nivel1 { get; set; }
        public string Nivel2 { get; set; }
        public string Nivel3 { get; set; }

        public string Conteudo { get; set; }

        public long? ProjetoId { get; set; }
    }
}
