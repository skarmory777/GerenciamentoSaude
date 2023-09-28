using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(Pessoa))]
    public abstract class PessoaDto : CamposPadraoCRUDDto
    {
        public string Cep { get; set; }

        public long? TipoLogradouroId { get; set; }

        public string Logradouro { get; set; }

        public string Complemento { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public long? CidadeId { get; set; }

        public long? EstadoId { get; set; }

        public long? PaisId { get; set; }

        public long? SexoId { get; set; }

        public long? TipoTelefoneId { get; set; }
        public TipoTelefoneDto TipoTelefone { get; set; }

        public long? TipoSanguineoId { get; set; }

        public long? ReligiaoId { get; set; }

        public long? CorPeleId { get; set; }

        public long? EstadoCivilId { get; set; }

        public long? EscolaridadeId { get; set; }

        public long? NaturalidadeId { get; set; }

        public long? NacionalidadeId { get; set; }

        public string Telefone1 { get; set; }

        public long? TipoTelefone1Id { get; set; }
        public TipoTelefoneDto TipoTelefone1Dto { get; set; }

        public string Telefone2 { get; set; }

        public long? TipoTelefone2Id { get; set; }
        public TipoTelefoneDto TipoTelefone2Dto { get; set; }

        public string Telefone3 { get; set; }

        public long? TipoTelefone3Id { get; set; }
        public TipoTelefoneDto TipoTelefone3Dto { get; set; }

        public string Telefone4 { get; set; }

        public long? TipoTelefone4Id { get; set; }
        public TipoTelefoneDto TipoTelefone4Dto { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public virtual CidadeDto Cidade { get; set; }

        public virtual EstadoDto Estado { get; set; }

        public virtual PaisDto Pais { get; set; }

        public virtual SexoDto Sexo { get; set; }

        public virtual TipoLogradouroDto TipoLogradouro { get; set; }

        //   public virtual TipoTelefoneDto TipoTelefone { get; set; }

        public virtual TipoSanguineoDto TipoSanguineo { get; set; }

        public virtual ReligiaoDto Religiao { get; set; }

        public virtual CorPeleDto CorPele { get; set; }

        public virtual EstadoCivilDto EstadoCivil { get; set; }

        public virtual EscolaridadeDto Escolaridade { get; set; }

        public virtual NaturalidadeDto Naturalidade { get; set; }

        public virtual NacionalidadeDto Nacionalidade { get; set; }

    }
}
