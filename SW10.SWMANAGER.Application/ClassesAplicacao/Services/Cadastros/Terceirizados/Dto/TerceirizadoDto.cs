using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Terceirizados.Dto
{
    public class TerceirizadoDto : CamposPadraoCRUDDto
    {

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        // Novo modelo SisPessoa
        public long? SisPessoaId { get; set; }
        public SisPessoaDto SisPessoa { get; set; }

        public string NomeCompleto
        {
            get { return this.SisPessoa?.NomeCompleto; }
            set { if (this.SisPessoa != null) this.SisPessoa.NomeCompleto = value; }
        }

        public DateTime? Nascimento { get; set; }

        public SexoDto Sexo
        {
            get { return this.SisPessoa?.Sexo; }
            set { if (this.SisPessoa != null) this.SisPessoa.Sexo = value; }
        }
        public long? SexoId
        {
            get { return this.SisPessoa?.SexoId; }
            set { if (this.SisPessoa != null) this.SisPessoa.SexoId = value; }
        }

        public ProfissaoDto Profissao
        {
            get { return this.SisPessoa?.Profissao; }
            set { if (this.SisPessoa != null) this.SisPessoa.Profissao = value; }
        }
        public long? ProfissaoId
        {
            get { return this.SisPessoa?.ProfissaoId; }
            set { if (this.SisPessoa != null) this.SisPessoa.ProfissaoId = value; }
        }


        public string Rg
        {
            get { return this.SisPessoa?.Rg; }
            set { if (this.SisPessoa != null) this.SisPessoa.Rg = value; }
        }

        public string Emissor
        {
            get { return this.SisPessoa?.Emissor; }
            set { if (this.SisPessoa != null) this.SisPessoa.Emissor = value; }
        }

        public DateTime? Emissao
        {
            get { return this.SisPessoa?.EmissaoRg; }
            set { if (this.SisPessoa != null) this.SisPessoa.EmissaoRg = value; }
        }

        public string Cpf
        {
            get { return this.SisPessoa?.Cpf; }
            set { if (this.SisPessoa != null) this.SisPessoa.Cpf = value; }
        }

        public NaturalidadeDto Naturalidade
        {
            get { return this.SisPessoa?.Naturalidade; }
            set { if (this.SisPessoa != null) this.SisPessoa.Naturalidade = value; }
        }

        public long? NaturalidadeId
        {
            get { return this.SisPessoa?.NaturalidadeId; }
            set { if (this.SisPessoa != null) this.SisPessoa.NaturalidadeId = value; }
        }

        public long? NacionalidadeId
        {
            get { return this.SisPessoa?.NacionalidadeId; }
            set { if (this.SisPessoa != null) this.SisPessoa.NacionalidadeId = value; }
        }

        public NacionalidadeDto Nacionalidade
        {
            get { return this.SisPessoa?.Nacionalidade; }
            set { if (this.SisPessoa != null) this.SisPessoa.Nacionalidade = value; }
        }

        public string Email
        {
            get { return this.SisPessoa?.Email; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email = value; }
        }

        public string Email2
        {
            get { return this.SisPessoa?.Email2; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email2 = value; }
        }

        public string Email3
        {
            get { return this.SisPessoa?.Email3; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email3 = value; }
        }

        public string Email4
        {
            get { return this.SisPessoa?.Email4; }
            set { if (this.SisPessoa != null) this.SisPessoa.Email4 = value; }
        }

        public string Telefone1
        {
            get { return this.SisPessoa?.Telefone1; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone1 = value; }
        }

        public TipoTelefoneDto TipoTelefone1
        {
            get { return this.SisPessoa?.TipoTelefone1; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone1 = value; }
        }

        public long? TipoTelefone1Id
        {
            get { return this.SisPessoa?.TipoTelefone1Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone1Id = value; }
        }

        public int? DddTelefone1
        {
            get { return this.SisPessoa?.DddTelefone1; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone1 = value; }
        }

        public string Telefone2
        {
            get { return this.SisPessoa?.Telefone2; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone2 = value; }
        }

        public TipoTelefoneDto TipoTelefone2
        {
            get { return this.SisPessoa?.TipoTelefone2; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone2 = value; }
        }

        public long? TipoTelefone2Id
        {
            get { return this.SisPessoa?.TipoTelefone2Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone2Id = value; }
        }

        public int? DddTelefone2
        {
            get { return this.SisPessoa?.DddTelefone2; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone2 = value; }
        }

        public string Telefone3
        {
            get { return this.SisPessoa?.Telefone3; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone3 = value; }
        }

        public TipoTelefoneDto TipoTelefone3
        {
            get { return this.SisPessoa?.TipoTelefone3; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone3 = value; }
        }
        public long? TipoTelefone3Id
        {
            get { return this.SisPessoa?.TipoTelefone3Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone3Id = value; }
        }

        public int? DddTelefone3
        {
            get { return this.SisPessoa?.DddTelefone3; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone3 = value; }
        }

        public string Telefone4
        {
            get { return this.SisPessoa?.Telefone4; }
            set { if (this.SisPessoa != null) this.SisPessoa.Telefone4 = value; }
        }

        public TipoTelefoneDto TipoTelefone4
        {
            get { return this.SisPessoa?.TipoTelefone4; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone4 = value; }
        }
        public long? TipoTelefone4Id
        {
            get { return this.SisPessoa?.TipoTelefone4Id; }
            set { if (this.SisPessoa != null) this.SisPessoa.TipoTelefone4Id = value; }
        }

        public int? DddTelefone4
        {
            get { return this.SisPessoa?.DddTelefone4; }
            set { if (this.SisPessoa != null) this.SisPessoa.DddTelefone4 = value; }
        }

        public string Cep
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Cep : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Cep = value; }
        }

        [ForeignKey("CidadeId")]
        public CidadeDto Cidade
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Cidade : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Cidade = value; }
        }
        public long? CidadeId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].CidadeId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].CidadeId = value; }
        }

        public string Complemento
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Complemento : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Complemento = value; }
        }

        public EstadoDto Estado
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Estado : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Estado = value; }
        }
        public long? EstadoId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].EstadoId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].EstadoId = value; }
        }

        public PaisDto Pais
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Pais : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Pais = value; }
        }
        public long? PaisId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].PaisId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].PaisId = value; }
        }

        public string Logradouro
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Logradouro : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Logradouro = value; }
        }

        public string Numero
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Numero : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Numero = value; }
        }


        public long? TipoLogradouroId
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].TipoLogradouroId : null; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].TipoLogradouroId = value; }
        }

        public TipoLogradouroDto TipoLogradouro
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].TipoLogradouro : null; }
            set
            {
                if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0)
                    this.SisPessoa.Enderecos[0].TipoLogradouro = value;
            }
        }

        public string Bairro
        {
            get { return (this.SisPessoa?.Enderecos != null && this.SisPessoa?.Enderecos.Count > 0) ? this.SisPessoa?.Enderecos[0].Bairro : string.Empty; }
            set { if (this.SisPessoa != null && this.SisPessoa?.Enderecos?.Count > 0) this.SisPessoa.Enderecos[0].Bairro = value; }
        }

        public static TerceirizadoDto Mapear(Terceirizado terceirizado)
        {
            var terceirizadoDto = new TerceirizadoDto
            {
                Id = terceirizado.Id,
                Codigo = terceirizado.Codigo,
                Descricao = terceirizado.Descricao
            };

            if (terceirizado.SisPessoa != null)
            {
                terceirizadoDto.SisPessoa = SisPessoaDto.Mapear(terceirizado.SisPessoa);
            }

            return terceirizadoDto;



        }
        
        public static Terceirizado Mapear(TerceirizadoDto dto)
        {
            var entity = new Terceirizado
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                Descricao = dto.Descricao
            };

            if (dto.SisPessoa != null)
            {
                entity.SisPessoa = SisPessoaDto.Mapear(dto.SisPessoa);
            }

            return entity;
        }
    }
}
