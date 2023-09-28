using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto
{
    [AutoMap(typeof(Paciente))]
    public class PacienteDto : CamposPadraoCRUDDto //PessoaFisicaDto
    {

        public PacienteDto()
        {
            if (this.SisPessoa == null)
            {
                this.SisPessoa = new SisPessoaDto();
                this.SisPessoa.Enderecos = new List<Enderecos.Dto.EnderecoDto>();
                this.SisPessoa.Enderecos.Add(new Enderecos.Dto.EnderecoDto());
            }
        }

        public long atendId { get; set; }

        public int CodigoPaciente { get; set; }

        public long? Prontuario { get; set; }

        public string Observacao { get; set; }

        public bool? IsDoador { get; set; }

        public long? Cns { get; set; }

        public string Indicacao { get; set; }

        public long? TipoSanguineoId { get; set; }

        public TipoSanguineoDto TipoSanguineo { get; set; }

        public ICollection<PacientePesoDto> PacientePesos { get; set; }

        public ICollection<PacienteDiagnosticosDto> PacienteDiagnosticos { get; set; }

        public ICollection<PacienteAlergiasDto> PacienteAlergias { get; set; }

        public Guid? AnexoListaId { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }

        // Novo modelo SisPessoa
        public long? SisPessoaId { get; set; }
        public SisPessoaDto SisPessoa { get; set; }


        public string NomeCompleto
        {
            get { return this.SisPessoa?.NomeCompleto; }
            set { if (this.SisPessoa != null) this.SisPessoa.NomeCompleto = value; }
        }

        public DateTime? Nascimento
        {
            get { return this.SisPessoa?.Nascimento; }
            set { if (this.SisPessoa != null) this.SisPessoa.Nascimento = value; }
        }

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

        public CorPeleDto CorPele
        {
            get { return this.SisPessoa?.CorPele; }
            set { if (this.SisPessoa != null) this.SisPessoa.CorPele = value; }
        }
        public long? CorPeleId
        {
            get { return this.SisPessoa?.CorPeleId; }
            set { if (this.SisPessoa != null) this.SisPessoa.CorPeleId = value; }
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

        public EscolaridadeDto Escolaridade
        {
            get { return this.SisPessoa?.Escolaridade; }
            set { if (this.SisPessoa != null) this.SisPessoa.Escolaridade = value; }
        }
        public long? EscolaridadeId
        {
            get { return this.SisPessoa?.EscolaridadeId; }
            set { if (this.SisPessoa != null) this.SisPessoa.EscolaridadeId = value; }
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

        public EstadoCivilDto EstadoCivil
        {
            get { return this.SisPessoa?.EstadoCivil; }
            set { if (this.SisPessoa != null) this.SisPessoa.EstadoCivil = value; }
        }
        public long? EstadoCivilId
        {
            get { return this.SisPessoa?.EstadoCivilId; }
            set { if (this.SisPessoa != null) this.SisPessoa.EstadoCivilId = value; }
        }


        public string NomeMae
        {
            get { return this.SisPessoa?.NomeMae; }
            set { if (this.SisPessoa != null) this.SisPessoa.NomeMae = value; }
        }

        public string NomePai
        {
            get { return this.SisPessoa?.NomePai; }
            set { if (this.SisPessoa != null) this.SisPessoa.NomePai = value; }
        }

        public ReligiaoDto Religiao
        {
            get { return this.SisPessoa?.Religiao; }
            set { if (this.SisPessoa != null) this.SisPessoa.Religiao = value; }
        }
        public long? ReligiaoId
        {
            get { return this.SisPessoa?.ReligiaoId; }
            set { if (this.SisPessoa != null) this.SisPessoa.ReligiaoId = value; }
        }



        public byte[] Foto
        {
            get { return this.SisPessoa?.Foto; }
            set { if (this.SisPessoa != null) this.SisPessoa.Foto = value; }
        }

        public string FotoMimeType
        {
            get { return this.SisPessoa?.FotoMimeType; }
            set { if (this.SisPessoa != null) this.SisPessoa.FotoMimeType = value; }
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


        #region Mapeamento

        public static PacienteDto Mapear(Paciente paciente)
        {
            if (paciente == null)
            {
                return null;
            }

            PacienteDto pacienteDto = MapearBase<PacienteDto>(paciente);
            pacienteDto.CodigoPaciente = paciente.CodigoPaciente;


            pacienteDto.Prontuario = paciente.Prontuario;
            pacienteDto.Observacao = paciente.Observacao;
            pacienteDto.IsDoador = paciente.IsDoador;
            pacienteDto.Cns = paciente.Cns;
            pacienteDto.Indicacao = paciente.Indicacao;
            pacienteDto.TipoSanguineoId = paciente.TipoSanguineoId;



            if (paciente.SisPessoa != null)
            {
                pacienteDto.SisPessoa = SisPessoaDto.Mapear(paciente.SisPessoa);
            }

            pacienteDto.SisPessoa.Enderecos = new List<Enderecos.Dto.EnderecoDto>();
            pacienteDto.SisPessoa.Enderecos.Add(new Enderecos.Dto.EnderecoDto());

            pacienteDto.Cidade = new CidadeDto()
            {
                Nome = paciente.Cidade?.Nome,
                Descricao = paciente.Cidade?.Descricao
            };
            pacienteDto.Estado = new EstadoDto()
            {
                Nome = paciente.Estado?.Nome,
                Descricao = paciente.Estado?.Descricao
            };

            pacienteDto.Sexo = new SexoDto()
            {
                Descricao = paciente.Sexo?.Descricao
            };

            if (paciente.EstadoCivil == null)
                pacienteDto.EstadoCivil = new EstadoCivilDto() { };
            else
                pacienteDto.EstadoCivil = new EstadoCivilDto()
                {
                    Id = paciente.EstadoCivil.Id,
                    Codigo = paciente.EstadoCivil.Codigo,
                    Descricao = paciente.EstadoCivil.Descricao
                };

            pacienteDto.Pais = new PaisDto()
            {
                Nome = paciente.Pais?.Nome,
                Sigla = paciente.Pais?.Sigla
            };


            if (paciente.PacientePesos == null)
            {
                pacienteDto.PacientePesos = new List<PacientePesoDto>();
            }
            else
            {
                pacienteDto.PacientePesos = paciente.PacientePesos.MapTo<List<PacientePesoDto>>();
            }

            if (paciente.PacienteDiagnosticos == null)
            {
                pacienteDto.PacienteDiagnosticos = new List<PacienteDiagnosticosDto>();
            }
            else
            {
                pacienteDto.PacienteDiagnosticos = paciente.PacienteDiagnosticos.MapTo<List<PacienteDiagnosticosDto>>();
            }

            if (paciente.PacienteAlergias == null)
            {
                pacienteDto.PacienteAlergias = new List<PacienteAlergiasDto>();
            }
            else
            {
                pacienteDto.PacienteAlergias = paciente.PacienteAlergias.MapTo<List<PacienteAlergiasDto>>();
            }

            if (paciente.Nacionalidade != null)
                pacienteDto.Nacionalidade = paciente.Nacionalidade.MapTo<NacionalidadeDto>();

            pacienteDto.SisPessoaId = paciente.SisPessoaId;
            pacienteDto.CodigoPaciente = paciente.CodigoPaciente;
            pacienteDto.Bairro = paciente.Bairro;
            pacienteDto.Logradouro = paciente.Logradouro;
            pacienteDto.Complemento = paciente.Complemento;
            pacienteDto.Numero = paciente.Numero;
            pacienteDto.Cep = paciente.Cep;

            return pacienteDto;
        }

        public static Paciente Mapear(PacienteDto pacienteDto)
        {
            if (pacienteDto == null) return null;

            Paciente paciente = new Paciente();

            paciente.CodigoPaciente = pacienteDto.CodigoPaciente;
            paciente.Prontuario = pacienteDto.Prontuario;
            paciente.Observacao = pacienteDto.Observacao;
            paciente.IsDoador = pacienteDto.IsDoador;
            paciente.Cns = pacienteDto.Cns;
            paciente.Indicacao = pacienteDto.Indicacao;
            paciente.TipoSanguineoId = pacienteDto.TipoSanguineoId;
            paciente.SisPessoaId = pacienteDto.SisPessoaId;

            if (pacienteDto.SisPessoa != null)
            {
                paciente.SisPessoa = SisPessoaDto.Mapear(pacienteDto.SisPessoa);
            }

            return paciente;
        }

        public static List<PacienteDto> Mapear(List<Paciente> pacientes)
        {
            var pacientesDto = new List<PacienteDto>();

            foreach (var item in pacientes)
            {
                pacientesDto.Add(Mapear(item));
            }

            return pacientesDto;
        }

        public static List<Paciente> Mapear(List<PacienteDto> pacientesDto)
        {
            var pacientes = new List<Paciente>();

            foreach (var item in pacientesDto)
            {
                pacientes.Add(Mapear(item));
            }

            return pacientes;
        }



        #endregion


    }
}
