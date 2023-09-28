using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [Table("DocItem")]
    public class DocItem : CamposPadraoCRUD
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public string Titulo { get; set; }
        public float Ordem { get; set; }
        [Index("Idx_DataPublicacao")]
        [DataType(DataType.DateTime)]
        public DateTime DataPublicacao { get; set; }
        public string Versao { get; set; }
        public long? Capitulo { get; set; }
        public long? Sessao { get; set; }
        public long? Assunto { get; set; }
        public string Conteudo { get; set; }
    }
}
