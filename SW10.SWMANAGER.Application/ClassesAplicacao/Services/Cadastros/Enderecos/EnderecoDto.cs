using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Enderecos.Dto
{
    [AutoMap(typeof(Endereco))]
    public class EnderecoDto : CamposPadraoCRUDDto
    {
        public long? PesssoaId { get; set; }
        public long? TipoLogradouroId { get; set; }
        public string Cep { get; set; }

        public string Logradouro { get; set; }

        public string Complemento { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }
        public long? CidadeId { get; set; }
        public long? EstadoId { get; set; }
        public long? PaisId { get; set; }
        public long? TipoEnderecoId { get; set; }

        // public TipoEnderecoDto TipoEndereco { get; set; }

        //     public SisPessoaDto Pessoa { get; set; }
        public PaisDto Pais { get; set; }
        public TipoLogradouroDto TipoLogradouro { get; set; }
        public CidadeDto Cidade { get; set; }
        public EstadoDto Estado { get; set; }

        public string TipoLogradouroDescricao { get; set; }
        public string CidadeDescricao { get; set; }
        public string EstadoDescricao { get; set; }
        public string PaisDescricao { get; set; }

        public long? IdGrid { get; set; }

    }
}
