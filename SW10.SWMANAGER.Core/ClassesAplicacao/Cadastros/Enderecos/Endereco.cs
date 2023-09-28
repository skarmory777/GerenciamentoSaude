using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos
{
    [Table("SisEndereco")]
    public class Endereco : CamposPadraoCRUD
    {
        public long? PesssoaId { get; set; }
        public long? TipoLogradouroId { get; set; }
        public string Cep { get; set; }

        [StringLength(80)]
        public string Logradouro { get; set; }

        [StringLength(30)]
        public string Complemento { get; set; }

        [StringLength(20)]
        public string Numero { get; set; }

        [StringLength(40)]
        public string Bairro { get; set; }
        public long? CidadeId { get; set; }
        public long? EstadoId { get; set; }
        public long? PaisId { get; set; }
        public long? TipoEnderecoId { get; set; }

        [ForeignKey("TipoEnderecoId")]
        public TipoEndereco TipoEndereco { get; set; }

        [ForeignKey("PesssoaId")]
        public SisPessoa Pessoa { get; set; }

        [ForeignKey("PaisId")]
        public Pais Pais { get; set; }

        [ForeignKey("TipoLogradouroId")]
        public TipoLogradouro TipoLogradouro { get; set; }

        [ForeignKey("CidadeId")]
        public Cidade Cidade { get; set; }

        [ForeignKey("EstadoId")]
        public Estado Estado { get; set; }

    }
}
