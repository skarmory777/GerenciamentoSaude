using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto
{
    [AutoMap(typeof(Medico))]
    public class MedicoDto : CamposPadraoCRUDDto //PessoaFisicaDto
    {
        public long NumeroConselho { get; set; }

        public byte[] AssinaturaDigital { get; set; }

        public string AssinaturaDigitalMimeType { get; set; }

        public string Cns { get; set; }

        public bool IsAgendaConsulta { get; set; }

        public bool IsAgendaCirurgia { get; set; }

        public bool IsAtendimentoConsulta { get; set; }

        public bool IsAtendimentoCirurgia { get; set; }

        public bool IsAtendimentoInternacao { get; set; }

        public bool IsEspecialista { get; set; }

        public bool IsExame { get; set; }

        public string CorAgendamentoConsulta { get; set; }

        public long? ConselhoId { get; set; }
        public virtual ConselhoDto Conselho { get; set; }

        public string CodigoCredenciamentoConvenio { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsCorpoClinico { get; set; }

        public string Apelido { get; set; }
        public long? IdGridMedicoEspecialidade { get; set; }

        //public virtual ICollection<MedicoEspecialidadeDto> MedicoEspecialidades { get; set; }

        public string MedicoEspecialidadeList { get; set; }

        public long? SisPessoaId { get; set; }
        public SisPessoaDto SisPessoa { get; set; }
        public bool IsIndeterminado { get; set; }


        #region Pessoa
        public string NomeCompleto { get; set; }

        public DateTime? Nascimento { get; set; }



        public SexoDto Sexo { get; set; }
        public long? SexoId { get; set; }


        public CorPeleDto CorPele { get; set; }
        public long? CorPeleId { get; set; }


        public ProfissaoDto Profissao { get; set; }
        public long? ProfissaoId { get; set; }

        public EscolaridadeDto Escolaridade { get; set; }
        public long? EscolaridadeId { get; set; }

        public string Rg { get; set; }

        public string Emissor { get; set; }

        public DateTime? Emissao { get; set; }

        public string Cpf { get; set; }

        public NaturalidadeDto Naturalidade { get; set; }

        public long? NaturalidadeId { get; set; }

        public long? NacionalidadeId { get; set; }

        public NacionalidadeDto Nacionalidade { get; set; }

        public EstadoCivilDto EstadoCivil { get; set; }
        public long? EstadoCivilId { get; set; }

        public string NomeMae { get; set; }

        public string NomePai { get; set; }

        public ReligiaoDto Religiao { get; set; }
        public long? ReligiaoId { get; set; }

        public byte[] Foto { get; set; }

        public string FotoMimeType { get; set; }


        public string Email { get; set; }

        public string Email2 { get; set; }

        public string Email3 { get; set; }

        public string Email4 { get; set; }

        public string Telefone1 { get; set; }

        public TipoTelefoneDto TipoTelefone1 { get; set; }
        public long? TipoTelefone1Id { get; set; }

        public int? DddTelefone1 { get; set; }

        public string Telefone2 { get; set; }

        public TipoTelefoneDto TipoTelefone2 { get; set; }
        public long? TipoTelefone2Id { get; set; }

        public int? DddTelefone2 { get; set; }

        public string Telefone3 { get; set; }

        public TipoTelefoneDto TipoTelefone3 { get; set; }
        public long? TipoTelefone3Id { get; set; }

        public int? DddTelefone3 { get; set; }

        public string Telefone4 { get; set; }

        public TipoTelefoneDto TipoTelefone4 { get; set; }
        public long? TipoTelefone4Id { get; set; }

        public int? DddTelefone4 { get; set; }
        public string Observacao { get; set; }



        #endregion

        #region Endereço

        public string Cep { get; set; }

        public CidadeDto Cidade { get; set; }
        public long? CidadeId { get; set; }

        public string Complemento { get; set; }

        public EstadoDto Estado { get; set; }
        public long? EstadoId { get; set; }

        public PaisDto Pais { get; set; }
        public long? PaisId { get; set; }

        public string Logradouro { get; set; }

        public string Numero { get; set; }


        public long? TipoLogradouroId { get; set; }

        public TipoLogradouroDto TipoLogradouro { get; set; }

        public string Bairro { get; set; }


        #endregion


        #region Mapeamento

        public static Medico Mapear(MedicoDto medicoDto)
        {
            if (medicoDto == null) return null;

            Medico medico = new Medico();

            medico.Codigo = medicoDto.Codigo;
            medico.NumeroConselho = medicoDto.NumeroConselho;
            medico.AssinaturaDigital = medicoDto.AssinaturaDigital;
            medico.AssinaturaDigitalMimeType = medicoDto.AssinaturaDigitalMimeType;
            medico.Cns = medicoDto.Cns;
            medico.IsAgendaConsulta = medicoDto.IsAgendaConsulta;
            medico.IsAgendaCirurgia = medicoDto.IsAgendaCirurgia;
            medico.IsAtendimentoConsulta = medicoDto.IsAtendimentoConsulta;
            medico.IsAtendimentoCirurgia = medicoDto.IsAtendimentoCirurgia;
            medico.IsAtendimentoInternacao = medicoDto.IsAtendimentoInternacao;
            medico.IsEspecialista = medicoDto.IsEspecialista;
            medico.IsExame = medicoDto.IsExame;
            medico.CorAgendamentoConsulta = medicoDto.CorAgendamentoConsulta;
            medico.ConselhoId = medicoDto.ConselhoId;
            medico.CodigoCredenciamentoConvenio = medicoDto.CodigoCredenciamentoConvenio;
            medico.IsAtivo = medicoDto.IsAtivo;
            medico.IsCorpoClinico = medicoDto.IsCorpoClinico;
            medico.Apelido = medicoDto.Apelido;
            medico.SisPessoaId = medicoDto.SisPessoaId;
            medico.Id = medicoDto.Id;
            medico.IsIndeterminado = medicoDto.IsIndeterminado;

            if (medicoDto.SisPessoa != null)
            {
                medico.SisPessoa = SisPessoaDto.Mapear(medicoDto.SisPessoa);
            }


            if (medicoDto.Conselho != null)
            {
                medico.Conselho = ConselhoDto.Mapear(medicoDto.Conselho);
            }

            medico.Cidade = CidadeDto.Mapear(medicoDto.Cidade);
            medico.Estado = EstadoDto.Mapear(medicoDto.Estado);
            medico.Nacionalidade = NacionalidadeDto.Mapear(medicoDto.Nacionalidade);
            medico.Naturalidade = NaturalidadeDto.Mapear(medicoDto.Naturalidade);
            medico.Pais = PaisDto.Mapear(medicoDto.Pais);
            medico.Profissao = ProfissaoDto.Mapear(medicoDto.Profissao);
            medico.Religiao = ReligiaoDto.Mapear(medicoDto.Religiao);
            medico.Sexo = SexoDto.Mapear(medicoDto.Sexo);
            medico.CorPele = CorPeleDto.Mapear(medicoDto.CorPele);
            medico.Religiao = ReligiaoDto.Mapear(medicoDto.Religiao);
            medico.EstadoCivil = EstadoCivilDto.Mapear(medicoDto.EstadoCivil);
            medico.Escolaridade = EscolaridadeDto.Mapear(medicoDto.Escolaridade);
            medico.TipoLogradouro = TipoLogradouroDto.Mapear(medicoDto.TipoLogradouro);
            medico.TipoTelefone1 = TipoTelefoneDto.Mapear(medicoDto.TipoTelefone1);
            medico.TipoTelefone2 = TipoTelefoneDto.Mapear(medicoDto.TipoTelefone2);
            medico.TipoTelefone3 = TipoTelefoneDto.Mapear(medicoDto.TipoTelefone3);
            medico.TipoTelefone4 = TipoTelefoneDto.Mapear(medicoDto.TipoTelefone4);

            //public virtual ICollection<MedicoEspecialidadeDto> MedicoEspecialidades { get; set; }

            return medico;
        }

        public static MedicoDto Mapear(Medico medico)
        {
            if (medico == null)
            {
                return null;
            }

            MedicoDto medicoDto = MapearBase<MedicoDto>(medico);

            medicoDto.Codigo = medico.Codigo;
            medicoDto.NumeroConselho = medico.NumeroConselho;
            medicoDto.AssinaturaDigital = medico.AssinaturaDigital;
            medicoDto.AssinaturaDigitalMimeType = medico.AssinaturaDigitalMimeType;
            medicoDto.Cns = medico.Cns;
            medicoDto.IsAgendaConsulta = medico.IsAgendaConsulta;
            medicoDto.IsAgendaCirurgia = medico.IsAgendaCirurgia;
            medicoDto.IsAtendimentoConsulta = medico.IsAtendimentoConsulta;
            medicoDto.IsAtendimentoCirurgia = medico.IsAtendimentoCirurgia;
            medicoDto.IsAtendimentoInternacao = medico.IsAtendimentoInternacao;
            medicoDto.IsEspecialista = medico.IsEspecialista;
            medicoDto.IsExame = medico.IsExame;
            medicoDto.CorAgendamentoConsulta = medico.CorAgendamentoConsulta;
            medicoDto.ConselhoId = medico.ConselhoId;
            medicoDto.CodigoCredenciamentoConvenio = medico.CodigoCredenciamentoConvenio;
            medicoDto.IsAtivo = medico.IsAtivo;
            medicoDto.IsCorpoClinico = medico.IsCorpoClinico;
            medicoDto.Apelido = medico.Apelido;
            medicoDto.SisPessoaId = medico.SisPessoaId;
            medicoDto.Id = medico.Id;

            medicoDto.NomeCompleto = medico.NomeCompleto;
            medicoDto.Nascimento = medico.Nascimento;
            medicoDto.SexoId = medico.SexoId;
            medicoDto.CorPeleId = medico.CorPeleId;
            medicoDto.ProfissaoId = medico.ProfissaoId;
            medicoDto.EscolaridadeId = medico.EscolaridadeId;
            medicoDto.Rg = medico.Rg;
            medicoDto.Emissor = medico.Emissor;
            medicoDto.Emissao = medico.Emissao;
            medicoDto.Cpf = medico.Cpf;
            medicoDto.NaturalidadeId = medico.NaturalidadeId;
            medicoDto.NacionalidadeId = medico.NacionalidadeId;
            medicoDto.EstadoCivilId = medico.EstadoCivilId;
            medicoDto.NomeMae = medico.NomeMae;
            medicoDto.NomePai = medico.NomePai;
            medicoDto.ReligiaoId = medico.ReligiaoId;
            medicoDto.Foto = medico.Foto;
            medicoDto.FotoMimeType = medico.FotoMimeType;
            medicoDto.Email = medico.Email;
            medicoDto.Email2 = medico.Email2;
            medicoDto.Email3 = medico.Email3;
            medicoDto.Email4 = medico.Email4;
            medicoDto.Telefone1 = medico.Telefone1;
            medicoDto.TipoTelefone1Id = medico.TipoTelefone1Id;
            medicoDto.DddTelefone1 = medico.DddTelefone1;
            medicoDto.Telefone2 = medico.Telefone2;
            medicoDto.TipoTelefone2Id = medico.TipoTelefone2Id;
            medicoDto.DddTelefone2 = medico.DddTelefone2;
            medicoDto.Telefone3 = medico.Telefone3;
            medicoDto.TipoTelefone3Id = medico.TipoTelefone3Id;
            medicoDto.DddTelefone3 = medico.DddTelefone3;
            medicoDto.Telefone4 = medico.Telefone4;
            medicoDto.TipoTelefone4Id = medico.TipoTelefone4Id;
            medicoDto.DddTelefone4 = medico.DddTelefone4;

            medicoDto.Cep = medico.Cep;
            medicoDto.CidadeId = medico.CidadeId;
            medicoDto.Complemento = medico.Complemento;
            medicoDto.EstadoId = medico.EstadoId;
            medicoDto.PaisId = medico.PaisId;
            medicoDto.Logradouro = medico.Logradouro;
            medicoDto.Numero = medico.Numero;
            medicoDto.TipoLogradouroId = medico.TipoLogradouroId;
            medicoDto.Bairro = medico.Bairro;

            medicoDto.IsIndeterminado = medico.IsIndeterminado;

            if (medico.SisPessoa != null)
            {
                medicoDto.SisPessoa = SisPessoaDto.Mapear(medico.SisPessoa);
            }


            if (medico.Conselho != null)
            {
                medicoDto.Conselho = ConselhoDto.Mapear(medico.Conselho);
            }

            medicoDto.Cidade = CidadeDto.Mapear(medico.Cidade);
            medicoDto.Estado = EstadoDto.Mapear(medico.Estado);
            medicoDto.Nacionalidade = NacionalidadeDto.Mapear(medico.Nacionalidade);
            medicoDto.Naturalidade = NaturalidadeDto.Mapear(medico.Naturalidade);
            medicoDto.Pais = PaisDto.Mapear(medico.Pais);
            medicoDto.Profissao = ProfissaoDto.Mapear(medico.Profissao);
            medicoDto.Religiao = ReligiaoDto.Mapear(medico.Religiao);
            medicoDto.Sexo = SexoDto.Mapear(medico.Sexo);
            medicoDto.CorPele = CorPeleDto.Mapear(medico.CorPele);
            medicoDto.Religiao = ReligiaoDto.Mapear(medico.Religiao);
            medicoDto.EstadoCivil = EstadoCivilDto.Mapear(medico.EstadoCivil);
            medicoDto.Escolaridade = EscolaridadeDto.Mapear(medico.Escolaridade);
            medicoDto.TipoLogradouro = TipoLogradouroDto.Mapear(medico.TipoLogradouro);
            medicoDto.TipoTelefone1 = TipoTelefoneDto.Mapear(medico.TipoTelefone1);
            medicoDto.TipoTelefone2 = TipoTelefoneDto.Mapear(medico.TipoTelefone2);
            medicoDto.TipoTelefone3 = TipoTelefoneDto.Mapear(medico.TipoTelefone3);
            medicoDto.TipoTelefone4 = TipoTelefoneDto.Mapear(medico.TipoTelefone4);

         

            return medicoDto;
        }

        public static IEnumerable<Medico> Mapear(List<MedicoDto> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }

        public static IEnumerable<MedicoDto> Mapear(List<Medico> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }

        #endregion

    }
}
