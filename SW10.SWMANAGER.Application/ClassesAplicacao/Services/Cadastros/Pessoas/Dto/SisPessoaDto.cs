using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Enderecos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto
{
    [AutoMap(typeof(SisPessoa))]
    public class SisPessoaDto : CamposPadraoCRUDDto
    {
        public override string Descricao
        {
            get
            {
                return this.FisicaJuridica == "F" ? this.NomeCompleto : this.NomeFantasia;
            }

            set
            {
                base.Descricao = value;
            }
        }

        public string NomeCompleto { get; set; }

        public long? TipoPessoaId { get; set; }

        public SisTipoPessoaDto TipoPessoa { get; set; }

        #region colunas Pessoa Física

        public string Rg { get; set; }

        public string Emissor { get; set; }

        public DateTime? EmissaoRg { get; set; }

        public DateTime? Nascimento { get; set; }

        public SexoDto Sexo { get; set; }
        public long? SexoId { get; set; }

        // public int? Sexo { get; set; }

        public CorPeleDto CorPele { get; set; }
        public long? CorPeleId { get; set; }

        public ProfissaoDto Profissao { get; set; }
        public long? ProfissaoId { get; set; }

        public EscolaridadeDto Escolaridade { get; set; }
        public long? EscolaridadeId { get; set; }

        public string Cpf { get; set; }

        public NaturalidadeDto Naturalidade { get; set; }
        public long? NaturalidadeId { get; set; }

        public long? NacionalidadeId { get; set; }

        public NacionalidadeDto Nacionalidade { get; set; }

        public EstadoCivilDto EstadoCivil { get; set; }
        public long? EstadoCivilId { get; set; }

        public string NomeMae { get; set; }

        public string NomePai { get; set; }

        public long? ReligiaoId { get; set; }

        public ReligiaoDto Religiao { get; set; }

        #endregion

        #region Colunas Pessoa Jurídica

        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string InscricaoMunicipal { get; set; }

        #endregion

        #region Telefones



        //RELACIONAMENTOS TELEFONES 1-4
        public TipoTelefoneDto TipoTelefone1 { get; set; }
        public long? TipoTelefone1Id { get; set; }

        public TipoTelefoneDto TipoTelefone2 { get; set; }
        public long? TipoTelefone2Id { get; set; }

        public TipoTelefoneDto TipoTelefone3 { get; set; }
        public long? TipoTelefone3Id { get; set; }

        public TipoTelefoneDto TipoTelefone4 { get; set; }
        public long? TipoTelefone4Id { get; set; }


        public string Telefone1 { get; set; }

        //public TipoTelefoneDto TipoTelefone1 { get; set; }
        //public long? TipoTelefone1Id { get; set; }


        public int? DddTelefone1 { get; set; }

        public string Telefone2 { get; set; }

        //public TipoTelefoneDto TipoTelefone2 { get; set; }
        //public long? TipoTelefone2Id { get; set; }

        public int? DddTelefone2 { get; set; }

        public string Telefone3 { get; set; }

        //public TipoTelefoneDto TipoTelefone3 { get; set; }
        //public long? TipoTelefone3Id { get; set; }

        public int? DddTelefone3 { get; set; }

        public string Telefone4 { get; set; }

        //public TipoTelefoneDto TipoTelefone4 { get; set; }
        //public long? TipoTelefone4Id { get; set; }

        public int? DddTelefone4 { get; set; }

        #endregion

        #region Emails

        public string Email { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Email4 { get; set; }

        #endregion

        public byte[] Foto { get; set; }

        public string FotoMimeType { get; set; }

        public string FisicaJuridica { get; set; }

        public bool IsAtivo { get; set; }

        public List<EnderecoDto> Enderecos { get; set; }

        public string EnderecosJson { get; set; }


        public TipoSanguineoDto TipoSanguineo { get; set; }
        public long? TipoSanguineoId { get; set; }

        public string Observacao { get; set; }


        #region Mapeamento


        public static SisPessoa Mapear(SisPessoaDto input)
        {
            SisPessoa result = new SisPessoa();


            result.Id = input.Id;
            result.Codigo = input.Codigo;
            result.Descricao = input.Descricao;
            result.NomeCompleto = input.NomeCompleto;
            result.TipoPessoaId = input.TipoPessoaId;

            result.Cnpj = input.Cnpj;
            result.CorPeleId = input.CorPeleId;
            result.Cpf = input.Cpf;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DddTelefone1 = input.DddTelefone1;
            result.DddTelefone2 = input.DddTelefone2;
            result.DddTelefone3 = input.DddTelefone3;
            result.DddTelefone4 = input.DddTelefone4;
            result.Email = input.Email;
            result.Email2 = input.Email2;
            result.Email3 = input.Email3;
            result.Email4 = input.Email4;
            result.EmissaoRg = input.EmissaoRg;
            result.Emissor = input.Emissor;
            result.EscolaridadeId = input.EscolaridadeId;
            result.EstadoCivilId = input.EstadoCivilId;
            result.FisicaJuridica = input.FisicaJuridica;
            result.Foto = input.Foto;
            result.FotoMimeType = input.FotoMimeType;
            result.InscricaoEstadual = input.InscricaoEstadual;
            result.InscricaoMunicipal = input.InscricaoMunicipal;
            result.IsAtivo = input.IsAtivo;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.NacionalidadeId = input.NacionalidadeId;
            result.Nascimento = input.Nascimento;
            result.NaturalidadeId = input.NaturalidadeId;
            result.NomeFantasia = input.NomeFantasia;
            result.NomeMae = input.NomeMae;
            result.NomePai = input.NomePai;
            result.ProfissaoId = input.ProfissaoId;
            result.RazaoSocial = input.RazaoSocial;
            result.ReligiaoId = input.ReligiaoId;
            result.Rg = input.Rg;
            result.SexoId = input.SexoId;
            result.Telefone1 = input.Telefone1;
            result.Telefone2 = input.Telefone2;
            result.Telefone3 = input.Telefone3;
            result.Telefone4 = input.Telefone4;
            result.TipoPessoaId = input.TipoPessoaId;
            result.TipoSanguineoId = input.TipoSanguineoId;
            result.TipoTelefone1Id = input.TipoTelefone1Id;
            result.TipoTelefone2Id = input.TipoTelefone2Id;
            result.TipoTelefone3Id = input.TipoTelefone3Id;
            result.TipoTelefone4Id = input.TipoTelefone4Id;

            return result;

        }

        public static SisPessoaDto Mapear(SisPessoa input)
        {
            if (input == null) return null;

            SisPessoaDto result = MapearBase<SisPessoaDto>(input);


            result.Id = input.Id;
            result.Codigo = input.Codigo;
            result.Descricao = input.Descricao;
            result.NomeCompleto = input.NomeCompleto;
            result.TipoPessoaId = input.TipoPessoaId;
            result.Nascimento = input.Nascimento;

            result.Cnpj = input.Cnpj;
            result.CorPeleId = input.CorPeleId;
            result.Cpf = input.Cpf;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DddTelefone1 = input.DddTelefone1;
            result.DddTelefone2 = input.DddTelefone2;
            result.DddTelefone3 = input.DddTelefone3;
            result.DddTelefone4 = input.DddTelefone4;
            result.Email = input.Email;
            result.Email2 = input.Email2;
            result.Email3 = input.Email3;
            result.Email4 = input.Email4;
            result.EmissaoRg = input.EmissaoRg;
            result.Emissor = input.Emissor;
            result.EscolaridadeId = input.EscolaridadeId;
            result.EstadoCivilId = input.EstadoCivilId;
            result.EstadoCivil = new EstadoCivilDto();
            result.FisicaJuridica = input.FisicaJuridica;
            result.Foto = input.Foto;
            result.FotoMimeType = input.FotoMimeType;
            result.InscricaoEstadual = input.InscricaoEstadual;
            result.InscricaoMunicipal = input.InscricaoMunicipal;
            result.IsAtivo = input.IsAtivo;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.NacionalidadeId = input.NacionalidadeId;
            result.Nascimento = input.Nascimento;
            result.NaturalidadeId = input.NaturalidadeId;
            result.NomeFantasia = input.NomeFantasia;
            result.NomeMae = input.NomeMae;
            result.NomePai = input.NomePai;
            result.ProfissaoId = input.ProfissaoId;
            result.Profissao = new ProfissaoDto() { Descricao = input.Profissao?.Descricao };
            result.RazaoSocial = input.RazaoSocial;
            result.ReligiaoId = input.ReligiaoId;
            result.Rg = input.Rg;
            result.SexoId = input.SexoId;
            result.Telefone1 = input.Telefone1;
            result.Telefone2 = input.Telefone2;
            result.Telefone3 = input.Telefone3;
            result.Telefone4 = input.Telefone4;
            result.TipoPessoaId = input.TipoPessoaId;
            result.TipoSanguineoId = input.TipoSanguineoId;
            result.TipoTelefone1Id = input.TipoTelefone1Id;
            result.TipoTelefone2Id = input.TipoTelefone2Id;
            result.TipoTelefone3Id = input.TipoTelefone3Id;
            result.TipoTelefone4Id = input.TipoTelefone4Id;

            result.Sexo = SexoDto.Mapear(input.Sexo);

            result.Observacao = input.Observacao;

            return result;

        }




        #endregion

    }
}
