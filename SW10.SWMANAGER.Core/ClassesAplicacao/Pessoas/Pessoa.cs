using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    public abstract class Pessoa : CamposPadraoCRUD
    {
        [StringLength(9)]
        public string Cep { get; set; }

        //ACERTAR REFERENCIA
        public long? TipoLogradouroId { get; set; }

        [ForeignKey("TipoLogradouroId")]
        public TipoLogradouro TipoLogradouro { get; set; }

        [StringLength(80)]
        public string Logradouro { get; set; }

        [StringLength(30)]
        public string Complemento { get; set; }

        [StringLength(20)]
        public string Numero { get; set; }

        [StringLength(40)]
        public string Bairro { get; set; }

        [ForeignKey("CidadeId")]
        public Cidade Cidade { get; set; }
        public long? CidadeId { get; set; }

        [ForeignKey("EstadoId")]
        public Estado Estado { get; set; }
        public long? EstadoId { get; set; }

        [ForeignKey("PaisId")]
        public Pais Pais { get; set; }
        public long? PaisId { get; set; }

        [StringLength(20)]
        public string Telefone1 { get; set; }

        public long? TipoTelefone1Id { get; set; }

        [ForeignKey("TipoTelefone1Id")]
        public TipoTelefone TipoTelefone1 { get; set; }

        public int? DddTelefone1 { get; set; }

        [StringLength(20)]
        public string Telefone2 { get; set; }

        public long? TipoTelefone2Id { get; set; }

        [ForeignKey("TipoTelefone2Id")]
        public TipoTelefone TipoTelefone2 { get; set; }

        public int? DddTelefone2 { get; set; }

        [StringLength(20)]
        public string Telefone3 { get; set; }

        public long? TipoTelefone3Id { get; set; }

        [ForeignKey("TipoTelefone3Id")]
        public TipoTelefone TipoTelefone3 { get; set; }

        public int? DddTelefone3 { get; set; }

        [StringLength(20)]
        public string Telefone4 { get; set; }

        public long? TipoTelefone4Id { get; set; }

        [ForeignKey("TipoTelefone4Id")]
        public TipoTelefone TipoTelefone4 { set; get; }

        public int? DddTelefone4 { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Email4 { get; set; }



    }
}
