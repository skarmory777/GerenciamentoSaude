using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Modulos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes
{
    [Table("SisOperacao")]
    public class Operacao : CamposPadraoCRUD, IDescricao
    {
        public bool IsFormulario { get; set; }

        public bool IsEspecialidade { get; set; }

        [StringLength(256)]
        public string Name { get; set; }

        [ForeignKey("Modulo"), Column("SisModuloId")]
        public long? ModuloId { get; set; }

        public Modulo Modulo { get; set; }
    }
}
