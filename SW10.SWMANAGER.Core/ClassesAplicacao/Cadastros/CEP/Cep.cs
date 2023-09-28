using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP
{
    [Table("SisCep")]
    public class Cep : CamposPadraoCRUD
    {
        [MaxLength(8)]
        public string CEP { get; set; }

        public string Logradouro { get; set; }

        public string Bairro { get; set; }

        public string Complemento { get; set; }

        public string Complemento2 { get; set; }

        public string UnidadePostagem { get; set; }

        [ForeignKey("Cidade"), Column("SisCidadeId")]
        public long? CidadeId { get; set; }

        [ForeignKey("Estado"), Column("SisEstadoId")]
        public long? EstadoId { get; set; }

        [ForeignKey("Pais"), Column("SisPaisId")]
        public long? PaisId { get; set; }

        public Pais Pais { get; set; }

        //ACERTER REFERENCIA
        [ForeignKey("TipoLogradouro"), Column("SisTipoLogradouroId")]
        public long? TipoLogradouroId { get; set; }
        public TipoLogradouro TipoLogradouro { get; set; }

        public Cidade Cidade { get; set; }

        public Estado Estado { get; set; }

    }
}
