using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Atestados
{
    [Table("AssModeloAtestado")]
    public class ModeloAtestado : CamposPadraoCRUD
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }
    }
}
