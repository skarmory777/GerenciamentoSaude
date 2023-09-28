namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class V1_inicial_aplicacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgendamentoConsultaMedicoDisponibilidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MedicoId = c.Long(nullable: false),
                    MedicoEspecialidadeId = c.Long(nullable: false),
                    DataInicio = c.DateTime(nullable: false),
                    DataFim = c.DateTime(nullable: false),
                    HoraInicio = c.DateTime(nullable: false),
                    HoraFim = c.DateTime(nullable: false),
                    IntervaloId = c.Long(nullable: false),
                    Domingo = c.Boolean(nullable: false),
                    Segunda = c.Boolean(nullable: false),
                    Terca = c.Boolean(nullable: false),
                    Quarta = c.Boolean(nullable: false),
                    Quinta = c.Boolean(nullable: false),
                    Sexta = c.Boolean(nullable: false),
                    Sabado = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoConsultaMedicoDisponibilidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Intervalo", t => t.IntervaloId, cascadeDelete: false)
                .ForeignKey("dbo.Medico", t => t.MedicoId, cascadeDelete: false)
                .ForeignKey("dbo.MedicoEspecialidade", t => t.MedicoEspecialidadeId, cascadeDelete: false)
                .Index(t => t.MedicoId)
                .Index(t => t.MedicoEspecialidadeId)
                .Index(t => t.IntervaloId);

            CreateTable(
                "dbo.Intervalo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
                    IntervaloMinutos = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Intervalo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Medico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Conselho = c.String(),
                    NumeroConselho = c.Long(nullable: false),
                    AssinaturaDigital = c.Binary(),
                    AssinaturaDigitalMimeType = c.String(),
                    Cns = c.String(),
                    IsAgendaConsulta = c.Boolean(nullable: false),
                    IsAgendaCirurgia = c.Boolean(nullable: false),
                    IsAtendimentoConsulta = c.Boolean(nullable: false),
                    IsAtendimentoCirurgia = c.Boolean(nullable: false),
                    IsAtendimentoInternacao = c.Boolean(nullable: false),
                    IsEspecialista = c.Boolean(nullable: false),
                    IsExame = c.Boolean(nullable: false),
                    CorAgendamentoConsulta = c.String(),
                    NomeCompleto = c.String(nullable: false, maxLength: 100),
                    Nascimento = c.DateTime(nullable: false),
                    Sexo = c.Int(),
                    CorPele = c.Int(),
                    ProfissaoId = c.Long(),
                    Escolaridade = c.Int(),
                    Rg = c.String(maxLength: 20),
                    Emissor = c.String(maxLength: 20),
                    Emissao = c.DateTime(),
                    Cpf = c.String(nullable: false, maxLength: 14),
                    NaturalidadeId = c.Long(),
                    NacionalidadeId = c.Long(),
                    EstadoCivil = c.Int(),
                    NomeMae = c.String(maxLength: 100),
                    NomePai = c.String(maxLength: 100),
                    Religiao = c.Int(),
                    Foto = c.Binary(),
                    FotoMimeType = c.String(),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Medico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Nacionalidade", t => t.NacionalidadeId)
                .ForeignKey("dbo.Naturalidade", t => t.NaturalidadeId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.Profissao", t => t.ProfissaoId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .Index(t => t.ProfissaoId)
                .Index(t => t.NaturalidadeId)
                .Index(t => t.NacionalidadeId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.Cidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 120),
                    EstadoId = c.Long(nullable: false),
                    Capital = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Estado", t => t.EstadoId, cascadeDelete: false)
                .Index(t => t.EstadoId);

            CreateTable(
                "dbo.Estado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 75),
                    Uf = c.String(nullable: false, maxLength: 5),
                    PaisId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Estado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pais", t => t.PaisId, cascadeDelete: false)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.Pais",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 60),
                    Sigla = c.String(nullable: false, maxLength: 10),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Pais_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MedicoEspecialidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MedicoId = c.Long(nullable: false),
                    EspecialidadeId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especialidade", t => t.EspecialidadeId, cascadeDelete: false)
                .ForeignKey("dbo.Medico", t => t.MedicoId, cascadeDelete: false)
                .Index(t => t.MedicoId)
                .Index(t => t.EspecialidadeId);

            CreateTable(
                "dbo.Especialidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Nome = c.String(),
                    Cbo = c.String(),
                    CboSus = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Especialidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Nacionalidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Nacionalidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Naturalidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Naturalidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Profissao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Profissao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoLogradouro",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Abreviacao = c.String(maxLength: 5),
                    Descricao = c.String(maxLength: 255),
                    Codigo = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoLogradouro_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AgendamentoConsulta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AgendamentoConsultaMedicoDisponibilidadeId = c.Long(),
                    MedicoId = c.Long(nullable: false),
                    MedicoEspecialidadeId = c.Long(nullable: false),
                    PacienteId = c.Long(),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(),
                    DataAgendamento = c.DateTime(nullable: false),
                    HoraAgendamento = c.DateTime(nullable: false),
                    Notas = c.String(),
                    NomeReservante = c.String(),
                    DataNascimentoReservante = c.DateTime(),
                    TelefoneReservante = c.String(),
                    ConvenioReservante = c.Long(),
                    PlanoReservante = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoConsulta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgendamentoConsultaMedicoDisponibilidade", t => t.AgendamentoConsultaMedicoDisponibilidadeId)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Medico", t => t.MedicoId, cascadeDelete: false)
                .ForeignKey("dbo.MedicoEspecialidade", t => t.MedicoEspecialidadeId, cascadeDelete: false)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .ForeignKey("dbo.Plano", t => t.PlanoId)
                .Index(t => t.AgendamentoConsultaMedicoDisponibilidadeId)
                .Index(t => t.MedicoId)
                .Index(t => t.MedicoEspecialidadeId)
                .Index(t => t.PacienteId)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId);

            CreateTable(
                "dbo.Convenio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
                    IsAtivo = c.Boolean(nullable: false),
                    Logotipo = c.Binary(),
                    LogotipoMimeType = c.String(),
                    IsFilotranpica = c.Boolean(nullable: false),
                    LogradouroCobranca = c.String(maxLength: 255),
                    CepCobrancaId = c.Long(),
                    TipoLogradouroCobrancaId = c.Long(),
                    NumeroCobranca = c.String(),
                    ComplementoCobranca = c.String(),
                    BairroCobranca = c.String(),
                    CidadeCobrancaId = c.Long(),
                    EstadoCobrancaId = c.Long(),
                    Cargo = c.String(),
                    DataInicioContrato = c.DateTime(nullable: false),
                    Vigencia = c.Int(nullable: false),
                    DataProximaRenovacaoContratual = c.DateTime(nullable: false),
                    DataInicialContrato = c.DateTime(nullable: false),
                    DataUltimaRenovacaoContrato = c.DateTime(nullable: false),
                    RazaoSocial = c.String(nullable: false),
                    NomeFantasia = c.String(nullable: false),
                    Cnpj = c.String(nullable: false),
                    InscricaoEstadual = c.String(),
                    InscricaoMunicipal = c.String(),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Convenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cep", t => t.CepCobrancaId)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Cidade", t => t.CidadeCobrancaId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Estado", t => t.EstadoCobrancaId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroCobrancaId)
                .Index(t => t.CepCobrancaId)
                .Index(t => t.TipoLogradouroCobrancaId)
                .Index(t => t.CidadeCobrancaId)
                .Index(t => t.EstadoCobrancaId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.Cep",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CEP = c.String(nullable: false, maxLength: 8),
                    Logradouro = c.String(),
                    Bairro = c.String(),
                    Complemento = c.String(),
                    Complemento2 = c.String(),
                    UnidadePostagem = c.String(),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    TipoLogradouroId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cep_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId)
                .Index(t => t.TipoLogradouroId);

            CreateTable(
                "dbo.Paciente",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CodigoPaciente = c.Int(nullable: false),
                    Prontuario = c.Long(),
                    Observacao = c.String(),
                    IsDoador = c.Boolean(),
                    Cns = c.Long(),
                    Indicacao = c.String(),
                    TipoSanguineoId = c.Long(),
                    NomeCompleto = c.String(nullable: false, maxLength: 100),
                    Nascimento = c.DateTime(nullable: false),
                    Sexo = c.Int(),
                    CorPele = c.Int(),
                    ProfissaoId = c.Long(),
                    Escolaridade = c.Int(),
                    Rg = c.String(maxLength: 20),
                    Emissor = c.String(maxLength: 20),
                    Emissao = c.DateTime(),
                    Cpf = c.String(nullable: false, maxLength: 14),
                    NaturalidadeId = c.Long(),
                    NacionalidadeId = c.Long(),
                    EstadoCivil = c.Int(),
                    NomeMae = c.String(maxLength: 100),
                    NomePai = c.String(maxLength: 100),
                    Religiao = c.Int(),
                    Foto = c.Binary(),
                    FotoMimeType = c.String(),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Paciente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Nacionalidade", t => t.NacionalidadeId)
                .ForeignKey("dbo.Naturalidade", t => t.NaturalidadeId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.Profissao", t => t.ProfissaoId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .ForeignKey("dbo.TipoSanguineo", t => t.TipoSanguineoId)
                .Index(t => t.TipoSanguineoId)
                .Index(t => t.ProfissaoId)
                .Index(t => t.NaturalidadeId)
                .Index(t => t.NacionalidadeId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.PacientePeso",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PacienteId = c.Long(nullable: false),
                    DataPesagem = c.DateTime(nullable: false),
                    Valor = c.Double(nullable: false),
                    Altura = c.Double(nullable: false),
                    PerimetroCefalico = c.Double(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PacientePeso_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Paciente", t => t.PacienteId, cascadeDelete: false)
                .Index(t => t.PacienteId);

            CreateTable(
                "dbo.TipoSanguineo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoSanguineo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Plano",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    ConvenioId = c.Long(nullable: false),
                    IsDespesasAcompanhante = c.Boolean(nullable: false),
                    IsValidadeCarteiraIndeterminada = c.Boolean(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
                    IsPlanoEmpresa = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Plano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId, cascadeDelete: false)
                .Index(t => t.ConvenioId);

            CreateTable(
                "dbo.AssistencialAtendimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    DataRegistro = c.DateTime(nullable: false),
                    PacienteId = c.Long(),
                    MedicoId = c.Long(),
                    ConvenioId = c.Long(),
                    EmpresaId = c.Long(),
                    UnidadeOrganizacionalId = c.Long(),
                    TipoGuia = c.String(),
                    NumeroGuia = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AssistencialAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Medico", t => t.MedicoId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .ForeignKey("dbo.UnidadeOrganizacional", t => t.UnidadeOrganizacionalId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .Index(t => t.PacienteId)
                .Index(t => t.MedicoId)
                .Index(t => t.ConvenioId)
                .Index(t => t.EmpresaId)
                .Index(t => t.UnidadeOrganizacionalId);

            CreateTable(
                "dbo.UnidadeOrganizacional",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsInternacao = c.Boolean(nullable: false),
                    IsAmbulatorioEmergencia = c.Boolean(nullable: false),
                    Localizacao = c.String(maxLength: 255),
                    IsHospitalDia = c.Boolean(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
                    UnidadeInternacaoTipoId = c.Long(),
                    OrganizationUnitId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    CentroCusto_Id = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeOrganizacional_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpOrganizationUnits", t => t.OrganizationUnitId, cascadeDelete: false)
                .ForeignKey("dbo.UnidadeInternacaoTipo", t => t.UnidadeInternacaoTipoId)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCusto_Id)
                .Index(t => t.UnidadeInternacaoTipoId)
                .Index(t => t.OrganizationUnitId)
                .Index(t => t.CentroCusto_Id);

            CreateTable(
                "dbo.UnidadeInternacaoTipo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeInternacaoTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Empresa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Logotipo = c.Binary(),
                    LogotipoMimeType = c.String(),
                    CodigoSus = c.Int(nullable: false),
                    Cnes = c.Int(nullable: false),
                    IsAtiva = c.Boolean(nullable: false),
                    IsComprasUnificadas = c.Boolean(nullable: false),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(),
                    RazaoSocial = c.String(nullable: false),
                    NomeFantasia = c.String(nullable: false),
                    Cnpj = c.String(nullable: false),
                    InscricaoEstadual = c.String(),
                    InscricaoMunicipal = c.String(),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Empresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.Plano", t => t.PlanoId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.Atendimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    GuiaNumero = c.String(),
                    Matricula = c.String(),
                    Responsavel = c.String(),
                    RgResponsavel = c.String(),
                    Observacao = c.String(),
                    DataPreatendimento = c.DateTime(nullable: false),
                    DataPrevistaAtendimento = c.DateTime(nullable: false),
                    DataRegistro = c.DateTime(nullable: false),
                    DataAlta = c.DateTime(nullable: false),
                    PacienteId = c.Long(),
                    OrigemId = c.Long(),
                    MedicoId = c.Long(),
                    EspecialidadeId = c.Long(),
                    EmpresaId = c.Long(),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(),
                    AtendimentoStatusId = c.Long(),
                    UnidadeOrganizacionalId = c.Long(),
                    AtendimentoTipoId = c.Long(),
                    GuiaId = c.Long(),
                    LeitoId = c.Long(),
                    MotivoAltaId = c.Long(),
                    IsAmbulatorioEmergencia = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    IsHomeCare = c.Boolean(nullable: false),
                    IsPreatendimento = c.Boolean(nullable: false),
                    ServicoMedicoPrestadoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Atendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoAtendimento", t => t.AtendimentoTipoId)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.Especialidade", t => t.EspecialidadeId)
                .ForeignKey("dbo.Guia", t => t.GuiaId)
                .ForeignKey("dbo.Leito", t => t.LeitoId)
                .ForeignKey("dbo.Medico", t => t.MedicoId)
                .ForeignKey("dbo.MotivoAlta", t => t.MotivoAltaId)
                .ForeignKey("dbo.Origem", t => t.OrigemId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .ForeignKey("dbo.Plano", t => t.PlanoId)
                .ForeignKey("dbo.ServicoMedicoPrestado", t => t.ServicoMedicoPrestadoId)
                .ForeignKey("dbo.AbpOrganizationUnits", t => t.UnidadeOrganizacionalId)
                .Index(t => t.PacienteId)
                .Index(t => t.OrigemId)
                .Index(t => t.MedicoId)
                .Index(t => t.EspecialidadeId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId)
                .Index(t => t.UnidadeOrganizacionalId)
                .Index(t => t.AtendimentoTipoId)
                .Index(t => t.GuiaId)
                .Index(t => t.LeitoId)
                .Index(t => t.MotivoAltaId)
                .Index(t => t.ServicoMedicoPrestadoId);

            CreateTable(
                "dbo.TipoAtendimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CodTipoAtendimento = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Guia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    OriginariaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Guia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Guia", t => t.OriginariaId)
                .Index(t => t.OriginariaId);

            CreateTable(
                "dbo.RelacaoGuiaCampo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CoordenadaX = c.Single(nullable: false),
                    CoordenadaY = c.Single(nullable: false),
                    GuiaId = c.Long(nullable: false),
                    GuiaCampoId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RelacaoGuiaCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Guia", t => t.GuiaId, cascadeDelete: false)
                .ForeignKey("dbo.GuiaCampo", t => t.GuiaCampoId, cascadeDelete: false)
                .Index(t => t.GuiaId)
                .Index(t => t.GuiaCampoId);

            CreateTable(
                "dbo.GuiaCampo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    CoordenadaX = c.Single(nullable: false),
                    CoordenadaY = c.Single(nullable: false),
                    IsConjunto = c.Boolean(nullable: false),
                    IsSubItem = c.Boolean(nullable: false),
                    ConjuntoId = c.Long(),
                    MaximoElementos = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GuiaCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Leito",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    DescricaoResumida = c.String(),
                    LeitoAih = c.String(),
                    Ramal = c.String(),
                    Sexo = c.Int(),
                    UnidadeOrganizacionalId = c.Long(),
                    TipoAcomodacaoId = c.Long(),
                    TabelaItemTissId = c.Long(),
                    TabelaItemSusId = c.Long(),
                    LeitoStatusId = c.Long(),
                    DataAtualizacao = c.DateTime(nullable: false),
                    Extra = c.Boolean(nullable: false),
                    HospitalDia = c.Boolean(nullable: false),
                    Ativo = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Leito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LeitoStatus", t => t.LeitoStatusId)
                .ForeignKey("dbo.TabelaDominio", t => t.TabelaItemTissId)
                .ForeignKey("dbo.TipoAcomodacao", t => t.TipoAcomodacaoId)
                .ForeignKey("dbo.UnidadeOrganizacional", t => t.UnidadeOrganizacionalId)
                .Index(t => t.UnidadeOrganizacionalId)
                .Index(t => t.TipoAcomodacaoId)
                .Index(t => t.TabelaItemTissId)
                .Index(t => t.LeitoStatusId);

            CreateTable(
                "dbo.LeitoStatus",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    Cor = c.String(maxLength: 7),
                    IsBloqueioAtendimento = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LeitoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TabelaDominio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    TipoTabelaDominioId = c.Long(),
                    GrupoTipoTabelaDominioId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoTipoTabelaDominio", t => t.GrupoTipoTabelaDominioId)
                .ForeignKey("dbo.TipoTabelaDominio", t => t.TipoTabelaDominioId)
                .Index(t => t.TipoTabelaDominioId)
                .Index(t => t.GrupoTipoTabelaDominioId);

            CreateTable(
                "dbo.GrupoTipoTabelaDominio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    TipoTabelaDominioId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoTipoTabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoTabelaDominio", t => t.TipoTabelaDominioId)
                .Index(t => t.TipoTabelaDominioId);

            CreateTable(
                "dbo.TipoTabelaDominio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoTabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TabelaDominioVersaoTiss",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TabelaDominioId = c.Long(nullable: false),
                    VersaoTissId = c.Long(nullable: false),
                    Incluido = c.Boolean(nullable: false),
                    Excluido = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TabelaDominio", t => t.TabelaDominioId, cascadeDelete: false)
                .ForeignKey("dbo.VersaoTiss", t => t.VersaoTissId, cascadeDelete: false)
                .Index(t => t.TabelaDominioId)
                .Index(t => t.VersaoTissId);

            CreateTable(
                "dbo.VersaoTiss",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    DataInicio = c.DateTime(nullable: false),
                    DataFim = c.DateTime(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_VersaoTiss_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoAcomodacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CodTipoAcomodacao = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoAcomodacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MotivoAlta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    MotivoAltaTipoAltaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoAlta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MotivoAltaTipoAlta", t => t.MotivoAltaTipoAltaId, cascadeDelete: false)
                .Index(t => t.MotivoAltaTipoAltaId);

            CreateTable(
                "dbo.MotivoAltaTipoAlta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoAltaTipoAlta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Origem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    UnidadeOrganizacionalId = c.Long(),
                    IsAtivo = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Origem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UnidadeOrganizacional", t => t.UnidadeOrganizacionalId)
                .Index(t => t.UnidadeOrganizacionalId);

            CreateTable(
                "dbo.ServicoMedicoPrestado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    ModeloAnamnese = c.String(),
                    Caucao = c.Boolean(nullable: false),
                    EspecialidadeId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ServicoMedicoPrestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especialidade", t => t.EspecialidadeId)
                .Index(t => t.EspecialidadeId);

            CreateTable(
                "dbo.Atestado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataAtendimento = c.DateTime(nullable: false),
                    Conteudo = c.String(),
                    MedicoId = c.Long(),
                    PacienteId = c.Long(),
                    TipoAtestadoId = c.Long(),
                    ModeloAtestadoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Atestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medico", t => t.MedicoId)
                .ForeignKey("dbo.TipoAtestado", t => t.ModeloAtestadoId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .ForeignKey("dbo.TipoAtestado", t => t.TipoAtestadoId)
                .Index(t => t.MedicoId)
                .Index(t => t.PacienteId)
                .Index(t => t.TipoAtestadoId)
                .Index(t => t.ModeloAtestadoId);

            CreateTable(
                "dbo.TipoAtestado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoAtestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Banco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.Int(nullable: false),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Banco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CapituloCID",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CapituloCID_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CentroCusto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    GrupoCentroCustoId = c.Long(),
                    CodigoCentroCusto = c.Int(nullable: false),
                    IsReceberLancamento = c.Boolean(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoCentroCusto", t => t.GrupoCentroCustoId)
                .Index(t => t.GrupoCentroCustoId);

            CreateTable(
                "dbo.GrupoCentroCusto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 50),
                    TipoGrupoCentroCustosId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoGrupoCentroCusto", t => t.TipoGrupoCentroCustosId)
                .Index(t => t.TipoGrupoCentroCustosId);

            CreateTable(
                "dbo.TipoGrupoCentroCusto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 75),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoGrupoCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Cfop",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 350),
                    Numero = c.Long(nullable: false),
                    Tipo = c.Boolean(nullable: false),
                    Vigencia = c.DateTime(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cfop_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ClassificacaoRisco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Senha = c.String(),
                    Prioridade = c.Int(nullable: false),
                    PreAtendimentoId = c.Long(nullable: false),
                    PacienteId = c.Long(nullable: false),
                    FilaId = c.String(),
                    SetorId = c.String(),
                    EspecialidadeId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ClassificacaoRisco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Especialidade", t => t.EspecialidadeId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId, cascadeDelete: false)
                .ForeignKey("dbo.PreAtendimento", t => t.PreAtendimentoId, cascadeDelete: false)
                .Index(t => t.PreAtendimentoId)
                .Index(t => t.PacienteId)
                .Index(t => t.EspecialidadeId);

            CreateTable(
                "dbo.PreAtendimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    NomeCompleto = c.String(),
                    Nascimento = c.DateTime(nullable: false),
                    DataRegistro = c.DateTime(nullable: false),
                    Sexo = c.Int(),
                    Telefone = c.String(),
                    Observacao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PreAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ColConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(),
                    Label = c.String(),
                    Placeholder = c.String(),
                    Value = c.String(),
                    Type = c.String(),
                    Colspan = c.Boolean(nullable: false),
                    Readonly = c.Boolean(nullable: false),
                    Ordem = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    RowConfig_Id = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ColConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RowConfig", t => t.RowConfig_Id)
                .Index(t => t.RowConfig_Id);

            CreateTable(
                "dbo.ColMultiOption",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Opcao = c.String(),
                    Selecionado = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    ColConfig_Id = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ColMultiOption_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColConfig", t => t.ColConfig_Id)
                .Index(t => t.ColConfig_Id);

            CreateTable(
                "dbo.FormData",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Valor = c.String(),
                    ColConfigId = c.Long(nullable: false),
                    FormRespostaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormData_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ColConfig", t => t.ColConfigId, cascadeDelete: false)
                .ForeignKey("dbo.FormResposta", t => t.FormRespostaId, cascadeDelete: false)
                .Index(t => t.ColConfigId)
                .Index(t => t.FormRespostaId);

            CreateTable(
                "dbo.FormResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataResposta = c.DateTime(nullable: false),
                    FormConfigId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormConfig", t => t.FormConfigId, cascadeDelete: false)
                .Index(t => t.FormConfigId);

            CreateTable(
                "dbo.FormConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(),
                    DataAlteracao = c.DateTime(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.RowConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Ordem = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    FormConfig_Id = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RowConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FormConfig", t => t.FormConfig_Id)
                .Index(t => t.FormConfig_Id);

            CreateTable(
                "dbo.Conselho",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    Sigla = c.String(maxLength: 5),
                    Uf = c.String(maxLength: 5),
                    NomeEStado = c.String(maxLength: 75),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Conselho_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ConsultorOcorrencia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorOcorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ConsultorTabelaCampoRelacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConsultorTabelaId = c.Long(nullable: false),
                    ConsultorTabelaCampoId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConsultorTabela", t => t.ConsultorTabelaId, cascadeDelete: false)
                .ForeignKey("dbo.ConsultorTabelaCampo", t => t.ConsultorTabelaCampoId, cascadeDelete: false)
                .Index(t => t.ConsultorTabelaId)
                .Index(t => t.ConsultorTabelaCampoId);

            CreateTable(
                "dbo.ConsultorTabela",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Nome = c.String(maxLength: 50),
                    Descricao = c.String(maxLength: 500),
                    Observacao = c.String(maxLength: 500),
                    ItemMenu = c.String(maxLength: 50),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorTabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ConsultorTabelaCampo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Campo = c.String(maxLength: 70),
                    Descricao = c.String(maxLength: 255),
                    Ele = c.String(maxLength: 10),
                    Tamanho = c.String(),
                    Observacao = c.String(),
                    ConsultorTabelaId = c.Long(),
                    ConsultorTipoDadoNFId = c.Long(),
                    ConsultorOcorrenciaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorTabelaCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ConsultorOcorrencia", t => t.ConsultorOcorrenciaId)
                .ForeignKey("dbo.ConsultorTabela", t => t.ConsultorTabelaId)
                .ForeignKey("dbo.ConsultorTipoDadoNF", t => t.ConsultorTipoDadoNFId)
                .Index(t => t.ConsultorTabelaId)
                .Index(t => t.ConsultorTipoDadoNFId)
                .Index(t => t.ConsultorOcorrenciaId);

            CreateTable(
                "dbo.ConsultorTipoDadoNF",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorTipoDadoNF_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ControleProducao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 500),
                    Observacao = c.String(maxLength: 500),
                    Pontuacao = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    DataInicial = c.DateTime(nullable: false),
                    DataFinal = c.DateTime(nullable: false),
                    DataAprovacao = c.DateTime(nullable: false),
                    DesenvolvedorId = c.Long(nullable: false),
                    UsuarioAprovacaoId = c.Long(nullable: false),
                    TabelaSistemaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ControleProducao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AbpUsers", t => t.DesenvolvedorId, cascadeDelete: false)
                .ForeignKey("dbo.ConsultorTabela", t => t.TabelaSistemaId, cascadeDelete: false)
                .ForeignKey("dbo.AbpUsers", t => t.UsuarioAprovacaoId, cascadeDelete: false)
                .Index(t => t.DesenvolvedorId)
                .Index(t => t.UsuarioAprovacaoId)
                .Index(t => t.TabelaSistemaId);

            CreateTable(
                "dbo.UserEmpresas",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    UserId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId, cascadeDelete: false)
                .ForeignKey("dbo.AbpUsers", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.EmpresaId);

            CreateTable(
                "dbo.CorPele",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CorPele_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Est_DCB",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    CodigoCAS = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DCB_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Entrada",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EmpresaId = c.Long(nullable: false),
                    FornecedorId = c.Long(nullable: false),
                    TipoDocumentoId = c.Long(nullable: false),
                    CentroCustoId = c.Long(nullable: false),
                    CfopId = c.Long(nullable: false),
                    EstoqueId = c.Long(nullable: false),
                    NumeroDocumento = c.Long(nullable: false),
                    Data = c.DateTime(nullable: false),
                    AcrescimoDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Frete = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ValorDocumento = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Entrada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId, cascadeDelete: false)
                .ForeignKey("dbo.Cfop", t => t.CfopId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoEstoque", t => t.EstoqueId, cascadeDelete: false)
                .Index(t => t.CentroCustoId)
                .Index(t => t.CfopId)
                .Index(t => t.EstoqueId);

            CreateTable(
                "dbo.EntradaItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EntradaId = c.Long(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    Quantidade = c.Long(nullable: false),
                    CustoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EntradaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Entrada", t => t.EntradaId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .Index(t => t.EntradaId)
                .Index(t => t.ProdutoId);

            CreateTable(
                "dbo.Est_Produto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.Long(nullable: false),
                    CodigoBarra = c.String(),
                    Descricao = c.String(),
                    DescricaoResumida = c.String(),
                    IsKit = c.Boolean(nullable: false),
                    IsOPME = c.Boolean(nullable: false),
                    IsPrincipal = c.Boolean(nullable: false),
                    ProdutoPrincipalId = c.Long(),
                    GeneroId = c.Long(nullable: false),
                    CodigoSistemas = c.String(),
                    CodigoTISS = c.String(),
                    GrupoId = c.Long(nullable: false),
                    GrupoClasseId = c.Long(),
                    GrupoSubClasseId = c.Long(),
                    IsCurvaABC = c.Boolean(nullable: false),
                    IsSerie = c.Boolean(nullable: false),
                    IsLote = c.Boolean(nullable: false),
                    IsValidade = c.Boolean(nullable: false),
                    IsMedicamento = c.Boolean(nullable: false),
                    IsMedicamentoControlado = c.Boolean(nullable: false),
                    IsLiberadoMovimentacao = c.Boolean(nullable: false),
                    IsBloqueioCompra = c.Boolean(nullable: false),
                    DCBId = c.Long(),
                    EstoqueLocalizacaoId = c.Long(nullable: false),
                    Especificacao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Produto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_GrupoClasse", t => t.GrupoClasseId)
                .ForeignKey("dbo.Est_DCB", t => t.DCBId)
                .ForeignKey("dbo.Est_EstoqueLocalizacao", t => t.EstoqueLocalizacaoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Genero", t => t.GeneroId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Grupo", t => t.GrupoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoPrincipalId)
                .ForeignKey("dbo.Est_GrupoSubClasse", t => t.GrupoSubClasseId)
                .Index(t => t.ProdutoPrincipalId)
                .Index(t => t.GeneroId)
                .Index(t => t.GrupoId)
                .Index(t => t.GrupoClasseId)
                .Index(t => t.GrupoSubClasseId)
                .Index(t => t.DCBId)
                .Index(t => t.EstoqueLocalizacaoId);

            CreateTable(
                "dbo.Est_GrupoClasse",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    GrupoId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Grupo", t => t.GrupoId, cascadeDelete: false)
                .Index(t => t.GrupoId);

            CreateTable(
                "dbo.Est_Grupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Grupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Est_GrupoSubClasse",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    GrupoClasseId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoSubClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_GrupoClasse", t => t.GrupoClasseId, cascadeDelete: false)
                .Index(t => t.GrupoClasseId);

            CreateTable(
                "dbo.Est_EstoqueLocalizacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    EstoqueId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueLocalizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Estoque", t => t.EstoqueId, cascadeDelete: false)
                .Index(t => t.EstoqueId);

            CreateTable(
                "dbo.Est_Estoque",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Estoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Est_Genero",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Genero_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Est_ProdutoUnidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsAtivo = c.Boolean(nullable: false),
                    IsPrescricao = c.Boolean(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    UnidadeId = c.Long(nullable: false),
                    UnidadeTipoId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_UnidadeTipo", t => t.UnidadeTipoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Unidade", t => t.UnidadeId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.UnidadeId)
                .Index(t => t.UnidadeTipoId);

            CreateTable(
                "dbo.Est_UnidadeTipo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Est_Unidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Sigla = c.String(),
                    Descricao = c.String(),
                    Fator = c.Decimal(nullable: false, precision: 18, scale: 2),
                    UnidadeReferenciaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Unidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Unidade", t => t.UnidadeReferenciaId)
                .Index(t => t.UnidadeReferenciaId);

            CreateTable(
                "dbo.ProdutoEstoque",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    Tipo = c.String(maxLength: 12),
                    ProcessoCota = c.String(maxLength: 20),
                    IsCodigoDeBarra = c.Boolean(nullable: false),
                    IsCustoMedio = c.Boolean(nullable: false),
                    IsUtilizaCota = c.Boolean(nullable: false),
                    IsChecarSaldo = c.Boolean(nullable: false),
                    IsCalcularConsumoDemanda = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Escolaridade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Escolaridade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EstadoCivil",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstadoCivil_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EstoqueMovimentoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    MovimentoId = c.Long(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    Quantidade = c.Long(nullable: false),
                    NumeroSerie = c.String(),
                    CustoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ProdutoUnidadeId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueMovimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoqueMovimento", t => t.MovimentoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_ProdutoUnidade", t => t.ProdutoUnidadeId)
                .Index(t => t.MovimentoId)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoUnidadeId);

            CreateTable(
                "dbo.EstoqueMovimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Documento = c.String(),
                    Movimento = c.DateTime(nullable: false),
                    EstoqueId = c.Long(nullable: false),
                    TipoMovimentoId = c.Long(nullable: false),
                    FornecedorId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PreMovimentoEstadoId = c.Long(nullable: false),
                    TotalProduto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Frete = c.Decimal(nullable: false, precision: 18, scale: 2),
                    AcrescimoDecrescimo = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TotalDocumento = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CentroCustoId = c.Long(),
                    IsEntrada = c.Boolean(nullable: false),
                    Contabiliza = c.Boolean(nullable: false),
                    Consiginado = c.Boolean(nullable: false),
                    AplicacaoDireta = c.Boolean(nullable: false),
                    EntragaProduto = c.Boolean(nullable: false),
                    Serie = c.String(),
                    TipoDocumentoId = c.Long(),
                    OrdemId = c.Long(),
                    CFOPId = c.Long(),
                    ICMSPer = c.Decimal(precision: 18, scale: 2),
                    ValorICMS = c.Decimal(precision: 18, scale: 2),
                    DescontoPer = c.Decimal(precision: 18, scale: 2),
                    TipoFreteId = c.Long(),
                    InclusoNota = c.Boolean(nullable: false),
                    FretePer = c.Decimal(precision: 18, scale: 2),
                    ValorFrete = c.Decimal(precision: 18, scale: 2),
                    Frete_FornecedorId = c.Long(),
                    Emissao = c.DateTime(nullable: false),
                    EstoquePreMovimentoId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId)
                .ForeignKey("dbo.Cfop", t => t.CFOPId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId, cascadeDelete: false)
                .ForeignKey("dbo.EstoquePreMovimento", t => t.EstoquePreMovimentoId, cascadeDelete: false)
                .ForeignKey("dbo.Fornecedor", t => t.FornecedorId, cascadeDelete: false)
                .ForeignKey("dbo.Fornecedor", t => t.Frete_FornecedorId)
                .ForeignKey("dbo.OrdemCompra", t => t.OrdemId)
                .ForeignKey("dbo.EstoquePreMovimentoEstado", t => t.PreMovimentoEstadoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoEstoque", t => t.EstoqueId, cascadeDelete: false)
                .ForeignKey("dbo.TipoDocumento", t => t.TipoDocumentoId)
                .ForeignKey("dbo.TipoFrete", t => t.TipoFreteId)
                .ForeignKey("dbo.EstoqueTipoMovimento", t => t.TipoMovimentoId, cascadeDelete: false)
                .Index(t => t.EstoqueId)
                .Index(t => t.TipoMovimentoId)
                .Index(t => t.FornecedorId)
                .Index(t => t.EmpresaId)
                .Index(t => t.PreMovimentoEstadoId)
                .Index(t => t.CentroCustoId)
                .Index(t => t.TipoDocumentoId)
                .Index(t => t.OrdemId)
                .Index(t => t.CFOPId)
                .Index(t => t.TipoFreteId)
                .Index(t => t.Frete_FornecedorId)
                .Index(t => t.EstoquePreMovimentoId);

            CreateTable(
                "dbo.EstoquePreMovimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Documento = c.String(),
                    Movimento = c.DateTime(nullable: false),
                    EstoqueId = c.Long(nullable: false),
                    TipoMovimentoId = c.Long(nullable: false),
                    FornecedorId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    PreMovimentoEstadoId = c.Long(nullable: false),
                    TotalProduto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Frete = c.Decimal(nullable: false, precision: 18, scale: 2),
                    AcrescimoDecrescimo = c.Decimal(nullable: false, precision: 18, scale: 2),
                    TotalDocumento = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CentroCustoId = c.Long(),
                    IsEntrada = c.Boolean(nullable: false),
                    Contabiliza = c.Boolean(nullable: false),
                    Consiginado = c.Boolean(nullable: false),
                    AplicacaoDireta = c.Boolean(nullable: false),
                    EntragaProduto = c.Boolean(nullable: false),
                    Serie = c.String(),
                    TipoDocumentoId = c.Long(),
                    OrdemId = c.Long(),
                    CFOPId = c.Long(),
                    ICMSPer = c.Decimal(precision: 18, scale: 2),
                    ValorICMS = c.Decimal(precision: 18, scale: 2),
                    DescontoPer = c.Decimal(precision: 18, scale: 2),
                    TipoFreteId = c.Long(),
                    InclusoNota = c.Boolean(nullable: false),
                    FretePer = c.Decimal(precision: 18, scale: 2),
                    ValorFrete = c.Decimal(precision: 18, scale: 2),
                    Frete_FornecedorId = c.Long(),
                    Emissao = c.DateTime(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId)
                .ForeignKey("dbo.Cfop", t => t.CFOPId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId, cascadeDelete: false)
                .ForeignKey("dbo.Fornecedor", t => t.FornecedorId, cascadeDelete: false)
                .ForeignKey("dbo.Fornecedor", t => t.Frete_FornecedorId)
                .ForeignKey("dbo.OrdemCompra", t => t.OrdemId)
                .ForeignKey("dbo.EstoquePreMovimentoEstado", t => t.PreMovimentoEstadoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoEstoque", t => t.EstoqueId, cascadeDelete: false)
                .ForeignKey("dbo.TipoDocumento", t => t.TipoDocumentoId)
                .ForeignKey("dbo.TipoFrete", t => t.TipoFreteId)
                .ForeignKey("dbo.EstoqueTipoMovimento", t => t.TipoMovimentoId, cascadeDelete: false)
                .Index(t => t.EstoqueId)
                .Index(t => t.TipoMovimentoId)
                .Index(t => t.FornecedorId)
                .Index(t => t.EmpresaId)
                .Index(t => t.PreMovimentoEstadoId)
                .Index(t => t.CentroCustoId)
                .Index(t => t.TipoDocumentoId)
                .Index(t => t.OrdemId)
                .Index(t => t.CFOPId)
                .Index(t => t.TipoFreteId)
                .Index(t => t.Frete_FornecedorId);

            CreateTable(
                "dbo.Fornecedor",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsAtivo = c.Boolean(nullable: false),
                    TipoPessoaId = c.Long(nullable: false),
                    CadastroExistenteId = c.Long(),
                    TipoCadastroExistenteId = c.Long(),
                    FornecedorPessoaFisicaId = c.Long(),
                    FornecedorPessoaJuridicaId = c.Long(),
                    PacienteId = c.Long(),
                    MedicoId = c.Long(),
                    ConvenioId = c.Long(),
                    EmpresaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Fornecedor_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.FornecedorPessoaFisica", t => t.FornecedorPessoaFisicaId)
                .ForeignKey("dbo.FornecedorPessoaJuridica", t => t.FornecedorPessoaJuridicaId)
                .ForeignKey("dbo.Medico", t => t.MedicoId)
                .ForeignKey("dbo.Paciente", t => t.PacienteId)
                .ForeignKey("dbo.TipoCadastroExistente", t => t.TipoCadastroExistenteId)
                .ForeignKey("dbo.TipoPessoa", t => t.TipoPessoaId, cascadeDelete: false)
                .Index(t => t.TipoPessoaId)
                .Index(t => t.TipoCadastroExistenteId)
                .Index(t => t.FornecedorPessoaFisicaId)
                .Index(t => t.FornecedorPessoaJuridicaId)
                .Index(t => t.PacienteId)
                .Index(t => t.MedicoId)
                .Index(t => t.ConvenioId)
                .Index(t => t.EmpresaId);

            CreateTable(
                "dbo.FornecedorPessoaFisica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FornecedorId = c.Long(nullable: false),
                    NomeCompleto = c.String(nullable: false, maxLength: 100),
                    Nascimento = c.DateTime(nullable: false),
                    Sexo = c.Int(),
                    CorPele = c.Int(),
                    ProfissaoId = c.Long(),
                    Escolaridade = c.Int(),
                    Rg = c.String(maxLength: 20),
                    Emissor = c.String(maxLength: 20),
                    Emissao = c.DateTime(),
                    Cpf = c.String(nullable: false, maxLength: 14),
                    NaturalidadeId = c.Long(),
                    NacionalidadeId = c.Long(),
                    EstadoCivil = c.Int(),
                    NomeMae = c.String(maxLength: 100),
                    NomePai = c.String(maxLength: 100),
                    Religiao = c.Int(),
                    Foto = c.Binary(),
                    FotoMimeType = c.String(),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FornecedorPessoaFisica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Nacionalidade", t => t.NacionalidadeId)
                .ForeignKey("dbo.Naturalidade", t => t.NaturalidadeId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.Profissao", t => t.ProfissaoId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .Index(t => t.ProfissaoId)
                .Index(t => t.NaturalidadeId)
                .Index(t => t.NacionalidadeId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.FornecedorPessoaJuridica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FornecedorId = c.Long(nullable: false),
                    RazaoSocial = c.String(nullable: false),
                    NomeFantasia = c.String(nullable: false),
                    Cnpj = c.String(nullable: false),
                    InscricaoEstadual = c.String(),
                    InscricaoMunicipal = c.String(),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FornecedorPessoaJuridica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.TipoCadastroExistente",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoPessoa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.OrdemCompra",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OrdemCompra_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EstoquePreMovimentoEstado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoEstado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoDocumento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 30),
                    IsDocumentoFiscal = c.Boolean(nullable: false),
                    IsAtualizaPreco = c.Boolean(nullable: false),
                    TipoEntradaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoDocumento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoEntrada", t => t.TipoEntradaId, cascadeDelete: false)
                .Index(t => t.TipoEntradaId);

            CreateTable(
                "dbo.TipoEntrada",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 30),
                    IsAtivo = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoEntrada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoFrete",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoFrete_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EstoqueTipoMovimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueTipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EstoqueMovimentoLoteValidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstoqueMovimentoItemId = c.Long(nullable: false),
                    LoteValidadeId = c.Long(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueMovimentoLoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoqueMovimentoItem", t => t.EstoqueMovimentoItemId, cascadeDelete: false)
                .ForeignKey("dbo.LoteValidade", t => t.LoteValidadeId, cascadeDelete: false)
                .Index(t => t.EstoqueMovimentoItemId)
                .Index(t => t.LoteValidadeId);

            CreateTable(
                "dbo.LoteValidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Lote = c.String(),
                    Validade = c.DateTime(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoLaboratorioId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoLaboratorio", t => t.ProdutoLaboratorioId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoLaboratorioId);

            CreateTable(
                "dbo.ProdutoLaboratorio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    FornecedorId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoLaboratorio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fornecedor", t => t.FornecedorId, cascadeDelete: false)
                .Index(t => t.FornecedorId);

            CreateTable(
                "dbo.EstoquePreMovimentoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PreMovimentoId = c.Long(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    Quantidade = c.Long(nullable: false),
                    NumeroSerie = c.String(),
                    CustoUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ProdutoUnidadeId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoquePreMovimento", t => t.PreMovimentoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_ProdutoUnidade", t => t.ProdutoUnidadeId)
                .Index(t => t.PreMovimentoId)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoUnidadeId);

            CreateTable(
                "dbo.EstoquePreMovimentoLoteValidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstoquePreMovimentoItemId = c.Long(nullable: false),
                    LoteValidadeId = c.Long(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoLoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoquePreMovimentoItem", t => t.EstoquePreMovimentoItemId, cascadeDelete: false)
                .ForeignKey("dbo.LoteValidade", t => t.LoteValidadeId, cascadeDelete: false)
                .Index(t => t.EstoquePreMovimentoItemId)
                .Index(t => t.LoteValidadeId);

            CreateTable(
                "dbo.Favorito",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    UserId = c.Long(nullable: false),
                    DisplayName = c.String(),
                    Icon = c.String(),
                    Name = c.String(),
                    Url = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Favorito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Feriado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DiaMesAno = c.DateTime(nullable: false),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Feriado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GrauInstrucao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrauInstrucao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GrupoCID",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.Int(nullable: false),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoCID_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GrupoProcedimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsProibido = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.GuiaTipo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GuiaTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Indicacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Indicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.InstituicaoTransferencia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    CodigoSUS = c.String(maxLength: 10),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_InstituicaoTransferencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LeitoCaracteristica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    Ramal = c.String(maxLength: 10),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LeitoCaracteristica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.LeitoServico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    Ramal = c.String(maxLength: 10),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LeitoServico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MailingTemplate",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Name = c.String(nullable: false),
                    Titulo = c.String(nullable: false),
                    EmailSaida = c.String(nullable: false),
                    NomeSaida = c.String(nullable: false),
                    ContentTemplate = c.String(nullable: false),
                    CamposDisponiveis = c.String(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MailingTemplate_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ModeloAtestado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Titulo = c.String(),
                    Conteudo = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ModeloAtestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MotivoCancelamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsAtivo = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoCancelamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MotivoCaucao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoCaucao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MotivoTransferenciaLeito",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoTransferenciaLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalManifestacaoDestinatario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ChaveAcesso = c.String(),
                    DataOperacao = c.DateTime(nullable: false),
                    NumeroProtocolo = c.String(),
                    TipoEvento = c.Int(nullable: false),
                    DescricaoTipoEvento = c.String(),
                    DescricaoRetorno = c.String(),
                    NotaFiscalId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NotaFiscalManifestacaoDestinatario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId, cascadeDelete: false)
                .Index(t => t.NotaFiscalId);

            CreateTable(
                "dbo.NotaFiscal",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalCobrancaId = c.Long(),
                    ChaveAcesso = c.String(),
                    Cnpj = c.Long(nullable: false),
                    Cpf = c.Long(nullable: false),
                    Situacao = c.Byte(nullable: false),
                    DataEmissao = c.DateTime(nullable: false),
                    DataRecebimento = c.DateTime(),
                    DigitoValidacao = c.String(),
                    InscricaoEstadual = c.Long(nullable: false),
                    NumeroProtocolo = c.Long(nullable: false),
                    ProxyDataEmissao = c.String(),
                    TipoNota = c.Byte(nullable: false),
                    VersaoNota = c.Decimal(nullable: false, precision: 18, scale: 2),
                    ValorNota = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Nome = c.String(),
                    CStat = c.Byte(nullable: false),
                    Ambiente = c.Byte(nullable: false),
                    VersaoAplicacao = c.String(),
                    VersaoServico = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Motivo = c.String(),
                    Nsu = c.Short(nullable: false),
                    Schema = c.String(),
                    XmlNota = c.String(),
                    Numero = c.Long(nullable: false),
                    Modelo = c.Long(nullable: false),
                    Serie = c.Long(nullable: false),
                    IsManifestacaoDestinatario = c.Boolean(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    Avulsa_Id = c.Long(),
                    Cana_Id = c.Long(),
                    Compra_Id = c.Long(),
                    Destinatario_Id = c.Long(),
                    Emitente_Id = c.Long(),
                    Entrega_Id = c.Long(),
                    Exporta_Id = c.Long(),
                    Ide_Id = c.Long(),
                    InformacaoAdicional_Id = c.Long(),
                    Retirada_Id = c.Long(),
                    TotalNota_Id = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NotaFiscal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalAvulsa", t => t.Avulsa_Id)
                .ForeignKey("dbo.NotaFiscalcana", t => t.Cana_Id)
                .ForeignKey("dbo.NotaFiscalcompra", t => t.Compra_Id)
                .ForeignKey("dbo.NotaFiscalDestinatario", t => t.Destinatario_Id)
                .ForeignKey("dbo.NotaFiscalEmitente", t => t.Emitente_Id)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId, cascadeDelete: false)
                .ForeignKey("dbo.NotaFiscalEntrega", t => t.Entrega_Id)
                .ForeignKey("dbo.NotaFiscalexporta", t => t.Exporta_Id)
                .ForeignKey("dbo.NotaFiscalIdentificacao", t => t.Ide_Id)
                .ForeignKey("dbo.NotaFiscalInformacaoAdicional", t => t.InformacaoAdicional_Id)
                .ForeignKey("dbo.NotaFiscalRetirada", t => t.Retirada_Id)
                .ForeignKey("dbo.NotaFiscalTotal", t => t.TotalNota_Id)
                .Index(t => t.EmpresaId)
                .Index(t => t.Avulsa_Id)
                .Index(t => t.Cana_Id)
                .Index(t => t.Compra_Id)
                .Index(t => t.Destinatario_Id)
                .Index(t => t.Emitente_Id)
                .Index(t => t.Entrega_Id)
                .Index(t => t.Exporta_Id)
                .Index(t => t.Ide_Id)
                .Index(t => t.InformacaoAdicional_Id)
                .Index(t => t.Retirada_Id)
                .Index(t => t.TotalNota_Id);

            CreateTable(
                "dbo.NotaFiscalautXML",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId, cascadeDelete: false)
                .Index(t => t.NotaFiscalId);

            CreateTable(
                "dbo.NotaFiscalAvulsa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    xOrgao = c.String(),
                    matr = c.String(),
                    xAgente = c.String(),
                    fone = c.String(),
                    UF = c.String(),
                    nDAR = c.String(),
                    dEmi = c.String(),
                    vDAR = c.Decimal(nullable: false, precision: 18, scale: 2),
                    repEmi = c.String(),
                    dPag = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalcana",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    safra = c.String(),
                    _ref = c.String(name: "ref"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalcanadeduc",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcanaId = c.Long(nullable: false),
                    xDed = c.String(),
                    vDed = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFor = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vTotDed = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vLiqFor = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalcana", t => t.NotaFiscalcanaId, cascadeDelete: false)
                .Index(t => t.NotaFiscalcanaId);

            CreateTable(
                "dbo.NotaFiscalcanaforDia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcanaId = c.Long(nullable: false),
                    dia = c.Int(nullable: false),
                    qtde = c.Decimal(nullable: false, precision: 18, scale: 2),
                    qTotMes = c.Decimal(nullable: false, precision: 18, scale: 2),
                    qTotAnt = c.Decimal(nullable: false, precision: 18, scale: 2),
                    qTotGer = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalcana", t => t.NotaFiscalcanaId, cascadeDelete: false)
                .Index(t => t.NotaFiscalcanaId);

            CreateTable(
                "dbo.NotaFiscalCobranca",
                c => new
                {
                    NotaFiscalId = c.Long(nullable: false),
                    NotaFiscalCobrancaFaturaId = c.Long(),
                })
                .PrimaryKey(t => t.NotaFiscalId)
                .ForeignKey("dbo.NotaFiscalCobrancaFatura", t => t.NotaFiscalCobrancaFaturaId)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId)
                .Index(t => t.NotaFiscalId)
                .Index(t => t.NotaFiscalCobrancaFaturaId);

            CreateTable(
                "dbo.NotaFiscalCobrancaDuplicata",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalCobrancaId = c.Long(nullable: false),
                    nDup = c.String(),
                    dVenc = c.String(),
                    vDup = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCobranca", t => t.NotaFiscalCobrancaId, cascadeDelete: false)
                .Index(t => t.NotaFiscalCobrancaId);

            CreateTable(
                "dbo.NotaFiscalCobrancaFatura",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalCobrancaId = c.Long(nullable: false),
                    nFat = c.String(),
                    vOrig = c.Decimal(precision: 18, scale: 2),
                    vDesc = c.Decimal(precision: 18, scale: 2),
                    vLiq = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalcompra",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    xNEmp = c.String(),
                    xPed = c.String(),
                    xCont = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalDestinatario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    idEstrangeiro = c.String(),
                    xNome = c.String(),
                    NotaFiscalEnderecoDestinatarioId = c.Long(),
                    indIEDest = c.Int(),
                    IE = c.String(),
                    ISUF = c.String(),
                    IM = c.String(),
                    email = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalEnderecoDestinatario", t => t.NotaFiscalEnderecoDestinatarioId)
                .Index(t => t.NotaFiscalEnderecoDestinatarioId);

            CreateTable(
                "dbo.NotaFiscalEnderecoDestinatario",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDestinatarioId = c.Long(nullable: false),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                    CEP = c.String(),
                    cPais = c.Int(),
                    xPais = c.String(),
                    fone = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalDetalhe",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    nItem = c.Int(nullable: false),
                    infAdProd = c.String(),
                    imposto_Id = c.Long(),
                    impostoDevol_Id = c.Long(),
                    prod_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalImposto", t => t.imposto_Id)
                .ForeignKey("dbo.NotaFiscalImpostoDevolvido", t => t.impostoDevol_Id)
                .ForeignKey("dbo.NotaFiscalProduto", t => t.prod_Id)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId, cascadeDelete: false)
                .Index(t => t.NotaFiscalId)
                .Index(t => t.imposto_Id)
                .Index(t => t.impostoDevol_Id)
                .Index(t => t.prod_Id);

            CreateTable(
                "dbo.NotaFiscalImposto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDetalheId = c.Long(nullable: false),
                    vTotTrib = c.Decimal(precision: 18, scale: 2),
                    COFINS_Id = c.Long(),
                    COFINSST_Id = c.Long(),
                    ICMS_Id = c.Long(),
                    ICMSUFDest_Id = c.Long(),
                    II_Id = c.Long(),
                    IPI_Id = c.Long(),
                    ISSQN_Id = c.Long(),
                    PIS_Id = c.Long(),
                    PISST_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCOFINS", t => t.COFINS_Id)
                .ForeignKey("dbo.NotaFiscalCOFINSST", t => t.COFINSST_Id)
                .ForeignKey("dbo.NotaFiscalICMS", t => t.ICMS_Id)
                .ForeignKey("dbo.NotaFiscalICMSUFDest", t => t.ICMSUFDest_Id)
                .ForeignKey("dbo.NotaFiscalICMSImpostoImportacao", t => t.II_Id)
                .ForeignKey("dbo.NotaFiscalIPI", t => t.IPI_Id)
                .ForeignKey("dbo.NotaFiscalISSQN", t => t.ISSQN_Id)
                .ForeignKey("dbo.NotaFiscalPIS", t => t.PIS_Id)
                .ForeignKey("dbo.NotaFiscalPISST", t => t.PISST_Id)
                .Index(t => t.COFINS_Id)
                .Index(t => t.COFINSST_Id)
                .Index(t => t.ICMS_Id)
                .Index(t => t.ICMSUFDest_Id)
                .Index(t => t.II_Id)
                .Index(t => t.IPI_Id)
                .Index(t => t.ISSQN_Id)
                .Index(t => t.PIS_Id)
                .Index(t => t.PISST_Id);

            CreateTable(
                "dbo.NotaFiscalCOFINS",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    TipoCOFINS_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCOFINSBasico", t => t.TipoCOFINS_Id)
                .Index(t => t.TipoCOFINS_Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    TipoICMS_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.TipoICMS_Id)
                .Index(t => t.TipoICMS_Id);

            CreateTable(
                "dbo.NotaFiscalICMSBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSUFDest",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBCUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pFCPUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSInter = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSInterPart = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFCPUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSUFDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSUFRemet = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSImpostoImportacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vDespAdu = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vII = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vIOF = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    clEnq = c.String(),
                    CNPJProd = c.String(),
                    cSelo = c.String(),
                    qSelo = c.Int(),
                    cEnq = c.Int(nullable: false),
                    TipoIPI_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalIPIBasico", t => t.TipoIPI_Id)
                .Index(t => t.TipoIPI_Id);

            CreateTable(
                "dbo.NotaFiscalIPIBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalISSQN",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliq = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vISSQN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    cMunFG = c.Long(nullable: false),
                    cListServ = c.String(),
                    vDeducao = c.Decimal(precision: 18, scale: 2),
                    vOutro = c.Decimal(precision: 18, scale: 2),
                    vDescIncond = c.Decimal(precision: 18, scale: 2),
                    vDescCond = c.Decimal(precision: 18, scale: 2),
                    vISSRet = c.Decimal(precision: 18, scale: 2),
                    indISS = c.Int(nullable: false),
                    cServico = c.String(),
                    cMun = c.Long(),
                    cPais = c.Int(),
                    nProcesso = c.String(),
                    indIncentivo = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPIS",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    TipoPIS_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalPISBasico", t => t.TipoPIS_Id)
                .Index(t => t.TipoPIS_Id);

            CreateTable(
                "dbo.NotaFiscalPISBasico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalImpostoDevolvido",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDetalheId = c.Long(nullable: false),
                    pDevol = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IPI_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalIPIDevolvido", t => t.IPI_Id)
                .Index(t => t.IPI_Id);

            CreateTable(
                "dbo.NotaFiscalIPIDevolvido",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalImpostoDevolvidoId = c.Long(nullable: false),
                    vIPIDevol = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalProduto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalDetalheId = c.Long(nullable: false),
                    cProd = c.String(),
                    cEAN = c.String(),
                    xProd = c.String(),
                    NCM = c.String(),
                    CEST = c.String(),
                    EXTIPI = c.String(),
                    CFOP = c.Int(nullable: false),
                    uCom = c.String(),
                    qCom = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vUnCom = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    cEANTrib = c.String(),
                    uTrib = c.String(),
                    qTrib = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vUnTrib = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFrete = c.Decimal(precision: 18, scale: 2),
                    vSeg = c.Decimal(precision: 18, scale: 2),
                    vDesc = c.Decimal(precision: 18, scale: 2),
                    vOutro = c.Decimal(precision: 18, scale: 2),
                    indTot = c.Int(nullable: false),
                    xPed = c.String(),
                    nItemPed = c.Int(),
                    nFCI = c.String(),
                    NotaFiscalProdutoEspecificoId = c.Long(),
                    nRECOPI = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalProdutoEspecifico", t => t.NotaFiscalProdutoEspecificoId)
                .Index(t => t.NotaFiscalProdutoEspecificoId);

            CreateTable(
                "dbo.NotaFiscaldetExport",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    nDraw = c.String(),
                    NotaFiscalexportIndId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalexportInd", t => t.NotaFiscalexportIndId)
                .ForeignKey("dbo.NotaFiscalProduto", t => t.NotaFiscalProdutoId, cascadeDelete: false)
                .Index(t => t.NotaFiscalProdutoId)
                .Index(t => t.NotaFiscalexportIndId);

            CreateTable(
                "dbo.NotaFiscalexportInd",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    nRE = c.String(),
                    chNFe = c.String(),
                    qExport = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalDI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    nDI = c.String(),
                    dDI = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydDI = c.String(),
                    xLocDesemb = c.String(),
                    UFDesemb = c.String(),
                    dDesemb = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydDesemb = c.String(),
                    tpViaTransp = c.Int(nullable: false),
                    vAFRMM = c.Decimal(precision: 18, scale: 2),
                    tpIntermedio = c.Int(nullable: false),
                    CNPJ = c.String(),
                    UFTerceiro = c.String(),
                    cExportador = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalProduto", t => t.NotaFiscalProdutoId, cascadeDelete: false)
                .Index(t => t.NotaFiscalProdutoId);

            CreateTable(
                "dbo.NotaFiscaladi",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    nAdicao = c.Int(nullable: false),
                    nSeqAdic = c.Int(nullable: false),
                    cFabricante = c.String(),
                    vDescDI = c.Decimal(precision: 18, scale: 2),
                    nDraw = c.String(),
                    NotaFiscalDI_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalDI", t => t.NotaFiscalDI_Id)
                .Index(t => t.NotaFiscalDI_Id);

            CreateTable(
                "dbo.NotaFiscalProdutoEspecifico",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCIDE",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcombId = c.Long(nullable: false),
                    qBCProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliqProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCIDE = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEncerrante",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalcombId = c.Long(nullable: false),
                    nBico = c.Int(nullable: false),
                    nBomba = c.Int(),
                    nTanque = c.Int(nullable: false),
                    vEncIni = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vEncFin = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEmitente",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    xNome = c.String(),
                    xFant = c.String(),
                    NotaFiscalEnderecoEmitenteId = c.Long(),
                    IE = c.String(),
                    IEST = c.String(),
                    IM = c.String(),
                    CNAE = c.String(),
                    CRT = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalEnderecoEmitente", t => t.NotaFiscalEnderecoEmitenteId)
                .Index(t => t.NotaFiscalEnderecoEmitenteId);

            CreateTable(
                "dbo.NotaFiscalEnderecoEmitente",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalEmitenteId = c.Long(nullable: false),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                    CEP = c.String(),
                    cPais = c.Int(),
                    xPais = c.String(),
                    fone = c.Long(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalEntrega",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalexporta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    UFSaidaPais = c.String(),
                    xLocExporta = c.String(),
                    xLocDespacho = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIdentificacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    cUF = c.Int(nullable: false),
                    cNF = c.String(),
                    natOp = c.String(),
                    indPag = c.Int(nullable: false),
                    mod = c.Int(nullable: false),
                    serie = c.Int(nullable: false),
                    nNF = c.Long(nullable: false),
                    dEmi = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydEmi = c.String(),
                    dSaiEnt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydSaiEnt = c.String(),
                    dhEmi = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxyDhEmi = c.String(),
                    dhSaiEnt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydhSaiEnt = c.String(),
                    tpNF = c.Int(nullable: false),
                    idDest = c.Int(),
                    cMunFG = c.Long(nullable: false),
                    tpImp = c.Int(nullable: false),
                    tpEmis = c.Int(nullable: false),
                    cDV = c.Int(nullable: false),
                    tpAmb = c.Int(nullable: false),
                    finNFe = c.Int(nullable: false),
                    indFinal = c.Int(),
                    indPres = c.Int(),
                    procEmi = c.Int(nullable: false),
                    verProc = c.String(),
                    dhCont = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydhCont = c.String(),
                    xJust = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalNFref",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalIdentificacaoId = c.Long(nullable: false),
                    refNFe = c.String(),
                    refECF_Id = c.Long(),
                    refNF_Id = c.Long(),
                    refNFP_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalrefECF", t => t.refECF_Id)
                .ForeignKey("dbo.NotaFiscalrefNF", t => t.refNF_Id)
                .ForeignKey("dbo.NotaFiscalrefNFP", t => t.refNFP_Id)
                .ForeignKey("dbo.NotaFiscalIdentificacao", t => t.NotaFiscalIdentificacaoId, cascadeDelete: false)
                .Index(t => t.NotaFiscalIdentificacaoId)
                .Index(t => t.refECF_Id)
                .Index(t => t.refNF_Id)
                .Index(t => t.refNFP_Id);

            CreateTable(
                "dbo.NotaFiscalrefECF",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalNFrefId = c.Long(nullable: false),
                    mod = c.String(),
                    nECF = c.Int(nullable: false),
                    nCOO = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalrefNF",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalNFrefId = c.Long(nullable: false),
                    cUF = c.Int(nullable: false),
                    AAMM = c.String(),
                    CNPJ = c.String(),
                    mod = c.String(),
                    serie = c.Int(nullable: false),
                    nNF = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalrefNFP",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalNFrefId = c.Long(nullable: false),
                    cUF = c.Int(nullable: false),
                    AAMM = c.String(),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    IE = c.String(),
                    mod = c.String(),
                    serie = c.Int(nullable: false),
                    nNF = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicional",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    infAdFisco = c.String(),
                    infCpl = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicionalCont",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalInformacaoAdicionalId = c.Long(nullable: false),
                    xCampo = c.String(),
                    xTexto = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalInformacaoAdicional", t => t.NotaFiscalInformacaoAdicionalId, cascadeDelete: false)
                .Index(t => t.NotaFiscalInformacaoAdicionalId);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicionalobsFisco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalInformacaoAdicionalId = c.Long(nullable: false),
                    xCampo = c.String(),
                    xTexto = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalInformacaoAdicional", t => t.NotaFiscalInformacaoAdicionalId, cascadeDelete: false)
                .Index(t => t.NotaFiscalInformacaoAdicionalId);

            CreateTable(
                "dbo.NotaFiscalInformacaoAdicionalprocRef",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalInformacaoAdicionalId = c.Long(nullable: false),
                    nProc = c.String(),
                    indProc = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalInformacaoAdicional", t => t.NotaFiscalInformacaoAdicionalId, cascadeDelete: false)
                .Index(t => t.NotaFiscalInformacaoAdicionalId);

            CreateTable(
                "dbo.NotaFiscalPagamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    tPag = c.Int(nullable: false),
                    vPag = c.Decimal(nullable: false, precision: 18, scale: 2),
                    card_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalPagamentoCartao", t => t.card_Id)
                .ForeignKey("dbo.NotaFiscal", t => t.NotaFiscalId, cascadeDelete: false)
                .Index(t => t.NotaFiscalId)
                .Index(t => t.card_Id);

            CreateTable(
                "dbo.NotaFiscalPagamentoCartao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalPagamentoId = c.Long(nullable: false),
                    tpIntegra = c.Int(),
                    CNPJ = c.String(),
                    tBand = c.Int(),
                    cAut = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalRetirada",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    CNPJ = c.String(),
                    CPF = c.String(),
                    xLgr = c.String(),
                    nro = c.String(),
                    xCpl = c.String(),
                    xBairro = c.String(),
                    cMun = c.Long(nullable: false),
                    xMun = c.String(),
                    UF = c.String(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalTotal",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalId = c.Long(nullable: false),
                    ICMSTot_Id = c.Long(),
                    ISSQNtot_Id = c.Long(),
                    retTrib_Id = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSTot", t => t.ICMSTot_Id)
                .ForeignKey("dbo.NotaFiscalISSQNTot", t => t.ISSQNtot_Id)
                .ForeignKey("dbo.NotaFiscalretTrib", t => t.retTrib_Id)
                .Index(t => t.ICMSTot_Id)
                .Index(t => t.ISSQNtot_Id)
                .Index(t => t.retTrib_Id);

            CreateTable(
                "dbo.NotaFiscalICMSTot",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalTotalId = c.Long(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    vFCPUFDest = c.Decimal(precision: 18, scale: 2),
                    vICMSUFDest = c.Decimal(precision: 18, scale: 2),
                    vICMSUFRemet = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vFrete = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vSeg = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vDesc = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vII = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vIPI = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vOutro = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vNF = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vTotTrib = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalISSQNTot",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalTotalId = c.Long(nullable: false),
                    vServ = c.Decimal(precision: 18, scale: 2),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    vISS = c.Decimal(precision: 18, scale: 2),
                    vPIS = c.Decimal(precision: 18, scale: 2),
                    vCOFINS = c.Decimal(precision: 18, scale: 2),
                    dCompet = c.String(),
                    vDeducao = c.Decimal(precision: 18, scale: 2),
                    vOutro = c.Decimal(precision: 18, scale: 2),
                    vDescIncond = c.Decimal(precision: 18, scale: 2),
                    vDescCond = c.Decimal(precision: 18, scale: 2),
                    vISSRet = c.Decimal(precision: 18, scale: 2),
                    cRegTrib = c.Int(),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalretTrib",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NotaFiscalTotalId = c.Long(nullable: false),
                    vRetPIS = c.Decimal(precision: 18, scale: 2),
                    vRetCOFINS = c.Decimal(precision: 18, scale: 2),
                    vRetCSLL = c.Decimal(precision: 18, scale: 2),
                    vBCIRRF = c.Decimal(precision: 18, scale: 2),
                    vIRRF = c.Decimal(precision: 18, scale: 2),
                    vBCRetPrev = c.Decimal(precision: 18, scale: 2),
                    vRetPrev = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Orcamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    Data = c.DateTime(nullable: false),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(nullable: false),
                    PrestadorId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    CentroCustoId = c.Long(nullable: false),
                    UnidadeOrganizacionalId = c.Long(nullable: false),
                    PreAtendimentoId = c.Long(nullable: false),
                    PacienteId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Orcamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId, cascadeDelete: false)
                .ForeignKey("dbo.Paciente", t => t.PacienteId, cascadeDelete: false)
                .ForeignKey("dbo.Plano", t => t.PlanoId, cascadeDelete: false)
                .ForeignKey("dbo.PreAtendimento", t => t.PreAtendimentoId, cascadeDelete: false)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId)
                .Index(t => t.EmpresaId)
                .Index(t => t.PreAtendimentoId)
                .Index(t => t.PacienteId);

            CreateTable(
                "dbo.PacienteConvenioBloqueado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConvenioId = c.Long(),
                    Matricula = c.String(),
                    DataImportacao = c.DateTime(nullable: false),
                    IsReativaCarteira = c.Boolean(nullable: false),
                    Justificativa = c.String(),
                    UsuarioReativado = c.String(),
                    DataUsuarioReativado = c.DateTime(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PacienteConvenioBloqueado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .Index(t => t.ConvenioId);

            CreateTable(
                "dbo.Parentesco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Parentesco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Prestador",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    CapturaFoto = c.Binary(),
                    TipoVinculoEmpregaticioId = c.Long(),
                    TipoParticipacaoId = c.Long(),
                    IsCorpoClinico = c.Boolean(nullable: false),
                    NomeGuerra = c.String(),
                    DataNascimento = c.DateTime(nullable: false),
                    Identidade = c.Int(nullable: false),
                    Cnpj = c.Int(nullable: false),
                    CartaoNacionalSus = c.Int(nullable: false),
                    CepComercialId = c.Long(),
                    TipoLogradouroComercialId = c.Long(),
                    EnderecoComercial = c.String(maxLength: 255),
                    NumeroComercial = c.String(),
                    ComplementoComercial = c.String(),
                    BairroComercial = c.String(),
                    CidadeComercial = c.String(),
                    EstadoUfComercial = c.String(),
                    TipoPrestadorId = c.Long(),
                    ConselhoId = c.Long(),
                    NumeroConselho = c.Int(nullable: false),
                    Faculdade = c.String(),
                    IsAtivo = c.Boolean(nullable: false),
                    DataCadastro = c.DateTime(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    UsuarioAlteracao = c.String(),
                    NomeCompleto = c.String(nullable: false, maxLength: 100),
                    Nascimento = c.DateTime(nullable: false),
                    Sexo = c.Int(),
                    CorPele = c.Int(),
                    ProfissaoId = c.Long(),
                    Escolaridade = c.Int(),
                    Rg = c.String(maxLength: 20),
                    Emissor = c.String(maxLength: 20),
                    Emissao = c.DateTime(),
                    Cpf = c.String(nullable: false, maxLength: 14),
                    NaturalidadeId = c.Long(),
                    NacionalidadeId = c.Long(),
                    EstadoCivil = c.Int(),
                    NomeMae = c.String(maxLength: 100),
                    NomePai = c.String(maxLength: 100),
                    Religiao = c.Int(),
                    Foto = c.Binary(),
                    FotoMimeType = c.String(),
                    Cep = c.String(maxLength: 9),
                    TipoLogradouroId = c.Long(),
                    Logradouro = c.String(maxLength: 80),
                    Complemento = c.String(maxLength: 30),
                    Numero = c.String(maxLength: 20),
                    Bairro = c.String(maxLength: 40),
                    CidadeId = c.Long(),
                    EstadoId = c.Long(),
                    PaisId = c.Long(),
                    Telefone1 = c.String(maxLength: 20),
                    TipoTelefone1 = c.Int(),
                    DddTelefone1 = c.Int(),
                    Telefone2 = c.String(maxLength: 20),
                    TipoTelefone2 = c.Int(),
                    DddTelefone2 = c.Int(),
                    Telefone3 = c.String(maxLength: 20),
                    TipoTelefone3 = c.Int(),
                    DddTelefone3 = c.Int(),
                    Telefone4 = c.String(maxLength: 20),
                    TipoTelefone4 = c.Int(),
                    DddTelefone4 = c.Int(),
                    Email = c.String(),
                    Email2 = c.String(),
                    Email3 = c.String(),
                    Email4 = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Prestador_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cep", t => t.CepComercialId)
                .ForeignKey("dbo.Cidade", t => t.CidadeId)
                .ForeignKey("dbo.Conselho", t => t.ConselhoId)
                .ForeignKey("dbo.Estado", t => t.EstadoId)
                .ForeignKey("dbo.Nacionalidade", t => t.NacionalidadeId)
                .ForeignKey("dbo.Naturalidade", t => t.NaturalidadeId)
                .ForeignKey("dbo.Pais", t => t.PaisId)
                .ForeignKey("dbo.Profissao", t => t.ProfissaoId)
                .ForeignKey("dbo.TipoLogradouro", t => t.TipoLogradouroId)
                .ForeignKey("dbo.TipoParticipacao", t => t.TipoParticipacaoId)
                .ForeignKey("dbo.TipoPrestador", t => t.TipoPrestadorId)
                .ForeignKey("dbo.TipoVinculoEmpregaticio", t => t.TipoVinculoEmpregaticioId)
                .Index(t => t.TipoVinculoEmpregaticioId)
                .Index(t => t.TipoParticipacaoId)
                .Index(t => t.CepComercialId)
                .Index(t => t.TipoPrestadorId)
                .Index(t => t.ConselhoId)
                .Index(t => t.ProfissaoId)
                .Index(t => t.NaturalidadeId)
                .Index(t => t.NacionalidadeId)
                .Index(t => t.TipoLogradouroId)
                .Index(t => t.CidadeId)
                .Index(t => t.EstadoId)
                .Index(t => t.PaisId);

            CreateTable(
                "dbo.TipoParticipacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoParticipacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoPrestador",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsDescricao = c.Boolean(nullable: false),
                    IsEvolucaoEnfermagem = c.Boolean(nullable: false),
                    IsSolicitaIntervencao = c.Boolean(nullable: false),
                    IsTecnicoExame = c.Boolean(nullable: false),
                    IsAssinaLaudo = c.Boolean(nullable: false),
                    IsNumeroConselhoObrigatorio = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoPrestador_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoVinculoEmpregaticio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoVinculoEmpregaticio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.PrestadorCredenciamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PrestadorId = c.Long(),
                    ConvenioId = c.Long(),
                    DataInicio = c.DateTime(nullable: false),
                    DataFim = c.DateTime(nullable: false),
                    CodCredenciamento = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrestadorCredenciamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Prestador", t => t.PrestadorId)
                .Index(t => t.PrestadorId)
                .Index(t => t.ConvenioId);

            CreateTable(
                "dbo.PrestadorGrupoProcedimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PrestadorId = c.Long(),
                    GrupoProcedimentoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrestadorGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GrupoProcedimento", t => t.GrupoProcedimentoId)
                .ForeignKey("dbo.Prestador", t => t.PrestadorId)
                .Index(t => t.PrestadorId)
                .Index(t => t.GrupoProcedimentoId);

            CreateTable(
                "dbo.ProdutoAcaoTerapeutica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoAcaoTerapeutica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoCodigoMedicamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(nullable: false, maxLength: 10),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoCodigoMedicamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoGrupoTratamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoGrupoTratamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoLocalizacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    Sigla = c.String(maxLength: 10),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoLocalizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoPalavraChave",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Palavra = c.String(maxLength: 15),
                    Observacao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoPalavraChave_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoPortaria",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoPortaria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoRelacaoAcaoTerapeutica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoAcaoTerapeuticaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoAcaoTerapeutica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoAcaoTerapeutica", t => t.ProdutoAcaoTerapeuticaId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoAcaoTerapeuticaId);

            CreateTable(
                "dbo.ProdutoRelacaoEstoque",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoEstoqueId = c.Long(nullable: false),
                    ProdutoLocalizacaoId = c.Long(nullable: false),
                    EstoqueMinimo = c.Long(nullable: false),
                    EstoqueMaximo = c.Long(nullable: false),
                    PontoPedido = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoEstoque", t => t.ProdutoEstoqueId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoLocalizacao", t => t.ProdutoLocalizacaoId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoEstoqueId)
                .Index(t => t.ProdutoLocalizacaoId);

            CreateTable(
                "dbo.ProdutoRelacaoLaboratorio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoLaboratorioId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoLaboratorio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoLaboratorio", t => t.ProdutoLaboratorioId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoLaboratorioId);

            CreateTable(
                "dbo.ProdutoRelacaoPalavraChave",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoPalavraChaveId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoPalavraChave_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoPalavraChave", t => t.ProdutoPalavraChaveId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoPalavraChaveId);

            CreateTable(
                "dbo.ProdutoRelacaoPortaria",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoPortariaId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoPortaria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoPortaria", t => t.ProdutoPortariaId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.ProdutoPortariaId);

            CreateTable(
                "dbo.ProdutoRelacaoUnidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    isAtivo = c.Boolean(nullable: false),
                    Prescricao = c.Boolean(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    UnidadeId = c.Long(nullable: false),
                    TipoUnidadeId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.Est_ProdutoUnidade", t => t.UnidadeId, cascadeDelete: false)
                .ForeignKey("dbo.ProdutoTipoUnidade", t => t.TipoUnidadeId, cascadeDelete: false)
                .Index(t => t.ProdutoId)
                .Index(t => t.UnidadeId)
                .Index(t => t.TipoUnidadeId);

            CreateTable(
                "dbo.ProdutoTipoUnidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 30),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoTipoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoClasse",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    GrupoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProdutoGrupo", t => t.GrupoId)
                .Index(t => t.GrupoId);

            CreateTable(
                "dbo.ProdutoGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.ProdutoSubClasse",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    GrupoClasseId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoSubClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProdutoClasse", t => t.GrupoClasseId)
                .Index(t => t.GrupoClasseId);

            CreateTable(
                "dbo.ProdutoListaSubstituicao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    ProdutoSubstituicaoId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoListaSubstituicao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: false)
                .Index(t => t.ProdutoId);

            CreateTable(
                "dbo.ProdutoSubstancia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoSubstancia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Regiao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Regiao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Religiao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(nullable: false, maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Religiao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Sexo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Sexo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoLeito",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CodigoTiss = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoTelefone",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoTelefone_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TipoUnidade",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Nome = c.String(maxLength: 30),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.UnidadeInternacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    Localizacao = c.String(maxLength: 255),
                    IsHospitalDia = c.Boolean(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
                    UnidadeInternacaoTipoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeInternacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UnidadeInternacaoTipo", t => t.UnidadeInternacaoTipoId)
                .Index(t => t.UnidadeInternacaoTipoId);

            //CreateTable(
            //    "dbo.VWConsultaFaturamentoAberto",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            AnoMesVenc = c.String(),
            //            ValorDifEntregaGlosa = c.String(),
            //            ValorEntrega = c.String(),
            //            ValorQuitacaoAmbulatorio = c.String(),
            //            ValorQuitacaoInternacao = c.String(),
            //            ValorLancamentoAberto = c.String(),
            //            ValorGlosa = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.VWConsultaFaturamentoEntrega",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            AnoMes = c.String(),
            //            Ano = c.String(),
            //            Mes = c.String(),
            //            ValorTotalEntregue = c.String(),
            //            ValorTotalRecebido = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.VWConsultaFaturamentoRecebimento",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            AnoMesPG = c.String(),
            //            ValorQuitacaoAmbulatorio = c.String(),
            //            ValorQuitacaoInternacao = c.String(),
            //            ValorQuitacaoSemIdentificacao = c.String(),
            //            ValorQuitacaoLancamento = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.VWFaturamentoAberto",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            AnoMesVenc = c.String(),
            //            ValorDifEntregaGlosa = c.String(),
            //            ValorEntrega = c.String(),
            //            ValorQuitacaoAmbulatorio = c.String(),
            //            ValorQuitacaoInternacao = c.String(),
            //            AnoVenc = c.String(),
            //            MesVenc = c.String(),
            //            Empresa = c.String(),
            //            convenio = c.String(),
            //            QuantCredito = c.String(),
            //            ValorQuitacaoSemIdentificacao = c.String(),
            //            ValorQuitacaoLancamento = c.String(),
            //            ValorGlosaExterna = c.String(),
            //            ValorGlosaRecuperavel = c.String(),
            //            ValorGlosaIrrecuperavel = c.String(),
            //            ValorLancamentoAbertoSemGlosa = c.String(),
            //            ValorLancamentoAberto = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.VWFaturamentoAberto6Meses",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Convenio = c.String(),
            //            ValorLancamentoAberto = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Mes01 = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Mes02 = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Mes03 = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Mes04 = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Mes05 = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Mes06 = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Total = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.VWTeste",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            PacienteId = c.Long(nullable: false),
            //            NomePaciente = c.String(),
            //            CidadeId = c.Long(nullable: false),
            //            NomeCidade = c.String(),
            //            EstadoId = c.Long(nullable: false),
            //            NomeEstado = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSAliq",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCOFINSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSNT",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCOFINSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSOutr",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pCOFINS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vCOFINS = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCOFINSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSQtde",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalCOFINSId = c.Long(nullable: false),
                    qBCProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliqProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCOFINS = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCOFINSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalCOFINSST",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pCOFINS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vCOFINS = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalCOFINSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS00",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS10",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS20",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    pRedBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS30",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS40",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS51",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(precision: 18, scale: 2),
                    vICMSOp = c.Decimal(precision: 18, scale: 2),
                    pDif = c.Decimal(precision: 18, scale: 2),
                    vICMSDif = c.Decimal(precision: 18, scale: 2),
                    vICMS = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS60",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    vBCSTRet = c.Decimal(precision: 18, scale: 2),
                    vICMSSTRet = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS70",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    pRedBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMS90",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(precision: 18, scale: 2),
                    vICMS = c.Decimal(precision: 18, scale: 2),
                    modBCST = c.Int(),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(precision: 18, scale: 2),
                    pICMSST = c.Decimal(precision: 18, scale: 2),
                    vICMSST = c.Decimal(precision: 18, scale: 2),
                    vICMSDeson = c.Decimal(precision: 18, scale: 2),
                    motDesICMS = c.Int(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSPart",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    modBC = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pBCOp = c.Decimal(nullable: false, precision: 18, scale: 2),
                    UFST = c.String(),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN101",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    pCredSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCredICMSSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN102",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN201",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pCredSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vCredICMSSN = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN202",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    modBCST = c.Int(nullable: false),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSST = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN500",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    vBCSTRet = c.Decimal(precision: 18, scale: 2),
                    vICMSSTRet = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSSN900",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CSOSN = c.Int(nullable: false),
                    modBC = c.Int(),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pRedBC = c.Decimal(precision: 18, scale: 2),
                    pICMS = c.Decimal(precision: 18, scale: 2),
                    vICMS = c.Decimal(precision: 18, scale: 2),
                    modBCST = c.Int(),
                    pMVAST = c.Decimal(precision: 18, scale: 2),
                    pRedBCST = c.Decimal(precision: 18, scale: 2),
                    vBCST = c.Decimal(precision: 18, scale: 2),
                    pICMSST = c.Decimal(precision: 18, scale: 2),
                    vICMSST = c.Decimal(precision: 18, scale: 2),
                    pCredSN = c.Decimal(precision: 18, scale: 2),
                    vCredICMSSN = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalICMSST",
                c => new
                {
                    Id = c.Long(nullable: false),
                    orig = c.Int(nullable: false),
                    CST = c.Int(nullable: false),
                    vBCSTRet = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSSTRet = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vBCSTDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vICMSSTDest = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalICMSId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalICMSBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPINT",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalIPIId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalIPIBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalIPITrib",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pIPI = c.Decimal(precision: 18, scale: 2),
                    qUnid = c.Decimal(precision: 18, scale: 2),
                    vUnid = c.Decimal(precision: 18, scale: 2),
                    vIPI = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalIPIId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalIPIBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISAliq",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                    NotaFiscalPISId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalPISBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISNT",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalPISId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalPISBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISOutr",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pPIS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vPIS = c.Decimal(precision: 18, scale: 2),
                    NotaFiscalPISId = c.Long(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalPISBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISQtde",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CST = c.Int(nullable: false),
                    NotaFiscalPISId = c.Long(nullable: false),
                    qBCProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vAliqProd = c.Decimal(nullable: false, precision: 18, scale: 2),
                    vPIS = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalPISBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalPISST",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalImpostoId = c.Long(nullable: false),
                    vBC = c.Decimal(precision: 18, scale: 2),
                    pPIS = c.Decimal(precision: 18, scale: 2),
                    qBCProd = c.Decimal(precision: 18, scale: 2),
                    vAliqProd = c.Decimal(precision: 18, scale: 2),
                    vPIS = c.Decimal(precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalPISBasico", t => t.Id)
                .Index(t => t.Id);

            CreateTable(
                "dbo.NotaFiscalarma",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    tpArma = c.Int(nullable: false),
                    nSerie = c.String(),
                    nCano = c.String(),
                    descr = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalProdutoEspecifico", t => t.Id)
                .ForeignKey("dbo.NotaFiscalProduto", t => t.NotaFiscalProdutoId, cascadeDelete: false)
                .Index(t => t.Id)
                .Index(t => t.NotaFiscalProdutoId);

            CreateTable(
                "dbo.NotaFiscalcomb",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    cProdANP = c.String(),
                    pMixGN = c.Decimal(precision: 18, scale: 2),
                    CODIF = c.String(),
                    qTemp = c.Decimal(precision: 18, scale: 2),
                    UFCons = c.String(),
                    NotaFiscalCIDEId = c.Long(),
                    NotaFiscalEncerranteId = c.Long(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalProdutoEspecifico", t => t.Id)
                .ForeignKey("dbo.NotaFiscalProduto", t => t.NotaFiscalProdutoId, cascadeDelete: false)
                .ForeignKey("dbo.NotaFiscalCIDE", t => t.NotaFiscalCIDEId)
                .ForeignKey("dbo.NotaFiscalEncerrante", t => t.NotaFiscalEncerranteId)
                .Index(t => t.Id)
                .Index(t => t.NotaFiscalProdutoId)
                .Index(t => t.NotaFiscalCIDEId)
                .Index(t => t.NotaFiscalEncerranteId);

            CreateTable(
                "dbo.NotaFiscalmed",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    nLote = c.String(),
                    qLote = c.Decimal(nullable: false, precision: 18, scale: 2),
                    dFab = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydFab = c.String(),
                    dVal = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    ProxydVal = c.String(),
                    vPMC = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalProdutoEspecifico", t => t.Id)
                .ForeignKey("dbo.NotaFiscalProduto", t => t.NotaFiscalProdutoId, cascadeDelete: false)
                .Index(t => t.Id)
                .Index(t => t.NotaFiscalProdutoId);

            CreateTable(
                "dbo.NotaFiscalveicProd",
                c => new
                {
                    Id = c.Long(nullable: false),
                    NotaFiscalProdutoId = c.Long(nullable: false),
                    tpOp = c.Int(nullable: false),
                    chassi = c.String(),
                    cCor = c.String(),
                    xCor = c.String(),
                    pot = c.String(),
                    cilin = c.String(),
                    pesoL = c.Decimal(nullable: false, precision: 18, scale: 2),
                    pesoB = c.Decimal(nullable: false, precision: 18, scale: 2),
                    nSerie = c.String(),
                    tpComb = c.String(),
                    nMotor = c.String(),
                    CMT = c.Decimal(nullable: false, precision: 18, scale: 2),
                    dist = c.String(),
                    anoMod = c.Int(nullable: false),
                    anoFab = c.Int(nullable: false),
                    tpPint = c.String(),
                    tpVeic = c.String(),
                    espVeic = c.Int(nullable: false),
                    VIN = c.Int(nullable: false),
                    condVeic = c.Int(nullable: false),
                    cMod = c.String(),
                    cCorDENATRAN = c.String(),
                    lota = c.Int(nullable: false),
                    tpRest = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.NotaFiscalProdutoEspecifico", t => t.Id)
                .ForeignKey("dbo.NotaFiscalProduto", t => t.NotaFiscalProdutoId, cascadeDelete: false)
                .Index(t => t.Id)
                .Index(t => t.NotaFiscalProdutoId);

            AddColumn("dbo.AbpUsers", "EmpresaId", c => c.Long());
            CreateIndex("dbo.AbpUsers", "EmpresaId");
            AddForeignKey("dbo.AbpUsers", "EmpresaId", "dbo.Empresa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.NotaFiscalveicProd", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalveicProd", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalmed", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalmed", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalcomb", "NotaFiscalEncerranteId", "dbo.NotaFiscalEncerrante");
            DropForeignKey("dbo.NotaFiscalcomb", "NotaFiscalCIDEId", "dbo.NotaFiscalCIDE");
            DropForeignKey("dbo.NotaFiscalcomb", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalcomb", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalarma", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalarma", "Id", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalPISST", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISQtde", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISOutr", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISNT", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalPISAliq", "Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalIPITrib", "Id", "dbo.NotaFiscalIPIBasico");
            DropForeignKey("dbo.NotaFiscalIPINT", "Id", "dbo.NotaFiscalIPIBasico");
            DropForeignKey("dbo.NotaFiscalICMSST", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN900", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN500", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN202", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN201", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN102", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSSN101", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMSPart", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS90", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS70", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS60", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS51", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS40", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS30", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS20", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS10", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalICMS00", "Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSST", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSQtde", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSOutr", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSNT", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscalCOFINSAliq", "Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.UnidadeInternacao", "UnidadeInternacaoTipoId", "dbo.UnidadeInternacaoTipo");
            DropForeignKey("dbo.ProdutoListaSubstituicao", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoSubClasse", "GrupoClasseId", "dbo.ProdutoClasse");
            DropForeignKey("dbo.ProdutoClasse", "GrupoId", "dbo.ProdutoGrupo");
            DropForeignKey("dbo.ProdutoRelacaoUnidade", "TipoUnidadeId", "dbo.ProdutoTipoUnidade");
            DropForeignKey("dbo.ProdutoRelacaoUnidade", "UnidadeId", "dbo.Est_ProdutoUnidade");
            DropForeignKey("dbo.ProdutoRelacaoUnidade", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoRelacaoPortaria", "ProdutoPortariaId", "dbo.ProdutoPortaria");
            DropForeignKey("dbo.ProdutoRelacaoPortaria", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoRelacaoPalavraChave", "ProdutoPalavraChaveId", "dbo.ProdutoPalavraChave");
            DropForeignKey("dbo.ProdutoRelacaoPalavraChave", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoRelacaoLaboratorio", "ProdutoLaboratorioId", "dbo.ProdutoLaboratorio");
            DropForeignKey("dbo.ProdutoRelacaoLaboratorio", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoLocalizacaoId", "dbo.ProdutoLocalizacao");
            DropForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoEstoqueId", "dbo.ProdutoEstoque");
            DropForeignKey("dbo.ProdutoRelacaoEstoque", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.ProdutoRelacaoAcaoTerapeutica", "ProdutoAcaoTerapeuticaId", "dbo.ProdutoAcaoTerapeutica");
            DropForeignKey("dbo.ProdutoRelacaoAcaoTerapeutica", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.PrestadorGrupoProcedimento", "PrestadorId", "dbo.Prestador");
            DropForeignKey("dbo.PrestadorGrupoProcedimento", "GrupoProcedimentoId", "dbo.GrupoProcedimento");
            DropForeignKey("dbo.PrestadorCredenciamento", "PrestadorId", "dbo.Prestador");
            DropForeignKey("dbo.PrestadorCredenciamento", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.Prestador", "TipoVinculoEmpregaticioId", "dbo.TipoVinculoEmpregaticio");
            DropForeignKey("dbo.Prestador", "TipoPrestadorId", "dbo.TipoPrestador");
            DropForeignKey("dbo.Prestador", "TipoParticipacaoId", "dbo.TipoParticipacao");
            DropForeignKey("dbo.Prestador", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.Prestador", "ProfissaoId", "dbo.Profissao");
            DropForeignKey("dbo.Prestador", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Prestador", "NaturalidadeId", "dbo.Naturalidade");
            DropForeignKey("dbo.Prestador", "NacionalidadeId", "dbo.Nacionalidade");
            DropForeignKey("dbo.Prestador", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Prestador", "ConselhoId", "dbo.Conselho");
            DropForeignKey("dbo.Prestador", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.Prestador", "CepComercialId", "dbo.Cep");
            DropForeignKey("dbo.PacienteConvenioBloqueado", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.Orcamento", "PreAtendimentoId", "dbo.PreAtendimento");
            DropForeignKey("dbo.Orcamento", "PlanoId", "dbo.Plano");
            DropForeignKey("dbo.Orcamento", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Orcamento", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Orcamento", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.NotaFiscalManifestacaoDestinatario", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscal", "TotalNota_Id", "dbo.NotaFiscalTotal");
            DropForeignKey("dbo.NotaFiscalTotal", "retTrib_Id", "dbo.NotaFiscalretTrib");
            DropForeignKey("dbo.NotaFiscalTotal", "ISSQNtot_Id", "dbo.NotaFiscalISSQNTot");
            DropForeignKey("dbo.NotaFiscalTotal", "ICMSTot_Id", "dbo.NotaFiscalICMSTot");
            DropForeignKey("dbo.NotaFiscal", "Retirada_Id", "dbo.NotaFiscalRetirada");
            DropForeignKey("dbo.NotaFiscalPagamento", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalPagamento", "card_Id", "dbo.NotaFiscalPagamentoCartao");
            DropForeignKey("dbo.NotaFiscal", "InformacaoAdicional_Id", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscalInformacaoAdicionalprocRef", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscalInformacaoAdicionalobsFisco", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscalInformacaoAdicionalCont", "NotaFiscalInformacaoAdicionalId", "dbo.NotaFiscalInformacaoAdicional");
            DropForeignKey("dbo.NotaFiscal", "Ide_Id", "dbo.NotaFiscalIdentificacao");
            DropForeignKey("dbo.NotaFiscalNFref", "NotaFiscalIdentificacaoId", "dbo.NotaFiscalIdentificacao");
            DropForeignKey("dbo.NotaFiscalNFref", "refNFP_Id", "dbo.NotaFiscalrefNFP");
            DropForeignKey("dbo.NotaFiscalNFref", "refNF_Id", "dbo.NotaFiscalrefNF");
            DropForeignKey("dbo.NotaFiscalNFref", "refECF_Id", "dbo.NotaFiscalrefECF");
            DropForeignKey("dbo.NotaFiscal", "Exporta_Id", "dbo.NotaFiscalexporta");
            DropForeignKey("dbo.NotaFiscal", "Entrega_Id", "dbo.NotaFiscalEntrega");
            DropForeignKey("dbo.NotaFiscal", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.NotaFiscal", "Emitente_Id", "dbo.NotaFiscalEmitente");
            DropForeignKey("dbo.NotaFiscalEmitente", "NotaFiscalEnderecoEmitenteId", "dbo.NotaFiscalEnderecoEmitente");
            DropForeignKey("dbo.NotaFiscalDetalhe", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalDetalhe", "prod_Id", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscalProduto", "NotaFiscalProdutoEspecificoId", "dbo.NotaFiscalProdutoEspecifico");
            DropForeignKey("dbo.NotaFiscalDI", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscaladi", "NotaFiscalDI_Id", "dbo.NotaFiscalDI");
            DropForeignKey("dbo.NotaFiscaldetExport", "NotaFiscalProdutoId", "dbo.NotaFiscalProduto");
            DropForeignKey("dbo.NotaFiscaldetExport", "NotaFiscalexportIndId", "dbo.NotaFiscalexportInd");
            DropForeignKey("dbo.NotaFiscalDetalhe", "impostoDevol_Id", "dbo.NotaFiscalImpostoDevolvido");
            DropForeignKey("dbo.NotaFiscalImpostoDevolvido", "IPI_Id", "dbo.NotaFiscalIPIDevolvido");
            DropForeignKey("dbo.NotaFiscalDetalhe", "imposto_Id", "dbo.NotaFiscalImposto");
            DropForeignKey("dbo.NotaFiscalImposto", "PISST_Id", "dbo.NotaFiscalPISST");
            DropForeignKey("dbo.NotaFiscalImposto", "PIS_Id", "dbo.NotaFiscalPIS");
            DropForeignKey("dbo.NotaFiscalPIS", "TipoPIS_Id", "dbo.NotaFiscalPISBasico");
            DropForeignKey("dbo.NotaFiscalImposto", "ISSQN_Id", "dbo.NotaFiscalISSQN");
            DropForeignKey("dbo.NotaFiscalImposto", "IPI_Id", "dbo.NotaFiscalIPI");
            DropForeignKey("dbo.NotaFiscalIPI", "TipoIPI_Id", "dbo.NotaFiscalIPIBasico");
            DropForeignKey("dbo.NotaFiscalImposto", "II_Id", "dbo.NotaFiscalICMSImpostoImportacao");
            DropForeignKey("dbo.NotaFiscalImposto", "ICMSUFDest_Id", "dbo.NotaFiscalICMSUFDest");
            DropForeignKey("dbo.NotaFiscalImposto", "ICMS_Id", "dbo.NotaFiscalICMS");
            DropForeignKey("dbo.NotaFiscalICMS", "TipoICMS_Id", "dbo.NotaFiscalICMSBasico");
            DropForeignKey("dbo.NotaFiscalImposto", "COFINSST_Id", "dbo.NotaFiscalCOFINSST");
            DropForeignKey("dbo.NotaFiscalImposto", "COFINS_Id", "dbo.NotaFiscalCOFINS");
            DropForeignKey("dbo.NotaFiscalCOFINS", "TipoCOFINS_Id", "dbo.NotaFiscalCOFINSBasico");
            DropForeignKey("dbo.NotaFiscal", "Destinatario_Id", "dbo.NotaFiscalDestinatario");
            DropForeignKey("dbo.NotaFiscalDestinatario", "NotaFiscalEnderecoDestinatarioId", "dbo.NotaFiscalEnderecoDestinatario");
            DropForeignKey("dbo.NotaFiscal", "Compra_Id", "dbo.NotaFiscalcompra");
            DropForeignKey("dbo.NotaFiscalCobranca", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.NotaFiscalCobranca", "NotaFiscalCobrancaFaturaId", "dbo.NotaFiscalCobrancaFatura");
            DropForeignKey("dbo.NotaFiscalCobrancaDuplicata", "NotaFiscalCobrancaId", "dbo.NotaFiscalCobranca");
            DropForeignKey("dbo.NotaFiscal", "Cana_Id", "dbo.NotaFiscalcana");
            DropForeignKey("dbo.NotaFiscalcanaforDia", "NotaFiscalcanaId", "dbo.NotaFiscalcana");
            DropForeignKey("dbo.NotaFiscalcanadeduc", "NotaFiscalcanaId", "dbo.NotaFiscalcana");
            DropForeignKey("dbo.NotaFiscal", "Avulsa_Id", "dbo.NotaFiscalAvulsa");
            DropForeignKey("dbo.NotaFiscalautXML", "NotaFiscalId", "dbo.NotaFiscal");
            DropForeignKey("dbo.EstoquePreMovimentoLoteValidade", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.EstoquePreMovimentoLoteValidade", "EstoquePreMovimentoItemId", "dbo.EstoquePreMovimentoItem");
            DropForeignKey("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId", "dbo.Est_ProdutoUnidade");
            DropForeignKey("dbo.EstoquePreMovimentoItem", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstoquePreMovimentoItem", "PreMovimentoId", "dbo.EstoquePreMovimento");
            DropForeignKey("dbo.EstoqueMovimentoLoteValidade", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.LoteValidade", "ProdutoLaboratorioId", "dbo.ProdutoLaboratorio");
            DropForeignKey("dbo.ProdutoLaboratorio", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.LoteValidade", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstoqueMovimentoLoteValidade", "EstoqueMovimentoItemId", "dbo.EstoqueMovimentoItem");
            DropForeignKey("dbo.EstoqueMovimentoItem", "ProdutoUnidadeId", "dbo.Est_ProdutoUnidade");
            DropForeignKey("dbo.EstoqueMovimentoItem", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstoqueMovimentoItem", "MovimentoId", "dbo.EstoqueMovimento");
            DropForeignKey("dbo.EstoqueMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento");
            DropForeignKey("dbo.EstoqueMovimento", "TipoFreteId", "dbo.TipoFrete");
            DropForeignKey("dbo.EstoqueMovimento", "TipoDocumentoId", "dbo.TipoDocumento");
            DropForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.ProdutoEstoque");
            DropForeignKey("dbo.EstoqueMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoqueMovimento", "OrdemId", "dbo.OrdemCompra");
            DropForeignKey("dbo.EstoqueMovimento", "Frete_FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.EstoqueMovimento", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.EstoqueMovimento", "EstoquePreMovimentoId", "dbo.EstoquePreMovimento");
            DropForeignKey("dbo.EstoquePreMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento");
            DropForeignKey("dbo.EstoquePreMovimento", "TipoFreteId", "dbo.TipoFrete");
            DropForeignKey("dbo.EstoquePreMovimento", "TipoDocumentoId", "dbo.TipoDocumento");
            DropForeignKey("dbo.TipoDocumento", "TipoEntradaId", "dbo.TipoEntrada");
            DropForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.ProdutoEstoque");
            DropForeignKey("dbo.EstoquePreMovimento", "PreMovimentoEstadoId", "dbo.EstoquePreMovimentoEstado");
            DropForeignKey("dbo.EstoquePreMovimento", "OrdemId", "dbo.OrdemCompra");
            DropForeignKey("dbo.EstoquePreMovimento", "Frete_FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.EstoquePreMovimento", "FornecedorId", "dbo.Fornecedor");
            DropForeignKey("dbo.Fornecedor", "TipoPessoaId", "dbo.TipoPessoa");
            DropForeignKey("dbo.Fornecedor", "TipoCadastroExistenteId", "dbo.TipoCadastroExistente");
            DropForeignKey("dbo.Fornecedor", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Fornecedor", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.Fornecedor", "FornecedorPessoaJuridicaId", "dbo.FornecedorPessoaJuridica");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.FornecedorPessoaJuridica", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.Fornecedor", "FornecedorPessoaFisicaId", "dbo.FornecedorPessoaFisica");
            DropForeignKey("dbo.FornecedorPessoaFisica", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.FornecedorPessoaFisica", "ProfissaoId", "dbo.Profissao");
            DropForeignKey("dbo.FornecedorPessoaFisica", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.FornecedorPessoaFisica", "NaturalidadeId", "dbo.Naturalidade");
            DropForeignKey("dbo.FornecedorPessoaFisica", "NacionalidadeId", "dbo.Nacionalidade");
            DropForeignKey("dbo.FornecedorPessoaFisica", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.FornecedorPessoaFisica", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.Fornecedor", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Fornecedor", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.EstoquePreMovimento", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.EstoquePreMovimento", "CFOPId", "dbo.Cfop");
            DropForeignKey("dbo.EstoquePreMovimento", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.EstoqueMovimento", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.EstoqueMovimento", "CFOPId", "dbo.Cfop");
            DropForeignKey("dbo.EstoqueMovimento", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.Entrada", "EstoqueId", "dbo.ProdutoEstoque");
            DropForeignKey("dbo.EntradaItem", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.Est_Produto", "GrupoSubClasseId", "dbo.Est_GrupoSubClasse");
            DropForeignKey("dbo.Est_ProdutoUnidade", "UnidadeId", "dbo.Est_Unidade");
            DropForeignKey("dbo.Est_Unidade", "UnidadeReferenciaId", "dbo.Est_Unidade");
            DropForeignKey("dbo.Est_ProdutoUnidade", "UnidadeTipoId", "dbo.Est_UnidadeTipo");
            DropForeignKey("dbo.Est_ProdutoUnidade", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.Est_Produto", "ProdutoPrincipalId", "dbo.Est_Produto");
            DropForeignKey("dbo.Est_Produto", "GrupoId", "dbo.Est_Grupo");
            DropForeignKey("dbo.Est_Produto", "GeneroId", "dbo.Est_Genero");
            DropForeignKey("dbo.Est_Produto", "EstoqueLocalizacaoId", "dbo.Est_EstoqueLocalizacao");
            DropForeignKey("dbo.Est_EstoqueLocalizacao", "EstoqueId", "dbo.Est_Estoque");
            DropForeignKey("dbo.Est_Produto", "DCBId", "dbo.Est_DCB");
            DropForeignKey("dbo.Est_Produto", "GrupoClasseId", "dbo.Est_GrupoClasse");
            DropForeignKey("dbo.Est_GrupoSubClasse", "GrupoClasseId", "dbo.Est_GrupoClasse");
            DropForeignKey("dbo.Est_GrupoClasse", "GrupoId", "dbo.Est_Grupo");
            DropForeignKey("dbo.EntradaItem", "EntradaId", "dbo.Entrada");
            DropForeignKey("dbo.Entrada", "CfopId", "dbo.Cfop");
            DropForeignKey("dbo.Entrada", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.ControleProducao", "UsuarioAprovacaoId", "dbo.AbpUsers");
            DropForeignKey("dbo.ControleProducao", "TabelaSistemaId", "dbo.ConsultorTabela");
            DropForeignKey("dbo.ControleProducao", "DesenvolvedorId", "dbo.AbpUsers");
            DropForeignKey("dbo.UserEmpresas", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.UserEmpresas", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.AbpUsers", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.ConsultorTabelaCampoRelacao", "ConsultorTabelaCampoId", "dbo.ConsultorTabelaCampo");
            DropForeignKey("dbo.ConsultorTabelaCampoRelacao", "ConsultorTabelaId", "dbo.ConsultorTabela");
            DropForeignKey("dbo.ConsultorTabelaCampo", "ConsultorTipoDadoNFId", "dbo.ConsultorTipoDadoNF");
            DropForeignKey("dbo.ConsultorTabelaCampo", "ConsultorTabelaId", "dbo.ConsultorTabela");
            DropForeignKey("dbo.ConsultorTabelaCampo", "ConsultorOcorrenciaId", "dbo.ConsultorOcorrencia");
            DropForeignKey("dbo.FormResposta", "FormConfigId", "dbo.FormConfig");
            DropForeignKey("dbo.RowConfig", "FormConfig_Id", "dbo.FormConfig");
            DropForeignKey("dbo.ColConfig", "RowConfig_Id", "dbo.RowConfig");
            DropForeignKey("dbo.FormData", "FormRespostaId", "dbo.FormResposta");
            DropForeignKey("dbo.FormData", "ColConfigId", "dbo.ColConfig");
            DropForeignKey("dbo.ColMultiOption", "ColConfig_Id", "dbo.ColConfig");
            DropForeignKey("dbo.ClassificacaoRisco", "PreAtendimentoId", "dbo.PreAtendimento");
            DropForeignKey("dbo.ClassificacaoRisco", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.ClassificacaoRisco", "EspecialidadeId", "dbo.Especialidade");
            DropForeignKey("dbo.UnidadeOrganizacional", "CentroCusto_Id", "dbo.CentroCusto");
            DropForeignKey("dbo.CentroCusto", "GrupoCentroCustoId", "dbo.GrupoCentroCusto");
            DropForeignKey("dbo.GrupoCentroCusto", "TipoGrupoCentroCustosId", "dbo.TipoGrupoCentroCusto");
            DropForeignKey("dbo.Atestado", "TipoAtestadoId", "dbo.TipoAtestado");
            DropForeignKey("dbo.Atestado", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Atestado", "ModeloAtestadoId", "dbo.TipoAtestado");
            DropForeignKey("dbo.Atestado", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.Atendimento", "UnidadeOrganizacionalId", "dbo.AbpOrganizationUnits");
            DropForeignKey("dbo.Atendimento", "ServicoMedicoPrestadoId", "dbo.ServicoMedicoPrestado");
            DropForeignKey("dbo.ServicoMedicoPrestado", "EspecialidadeId", "dbo.Especialidade");
            DropForeignKey("dbo.Atendimento", "PlanoId", "dbo.Plano");
            DropForeignKey("dbo.Atendimento", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Atendimento", "OrigemId", "dbo.Origem");
            DropForeignKey("dbo.Origem", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.Atendimento", "MotivoAltaId", "dbo.MotivoAlta");
            DropForeignKey("dbo.MotivoAlta", "MotivoAltaTipoAltaId", "dbo.MotivoAltaTipoAlta");
            DropForeignKey("dbo.Atendimento", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.Atendimento", "LeitoId", "dbo.Leito");
            DropForeignKey("dbo.Leito", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.Leito", "TipoAcomodacaoId", "dbo.TipoAcomodacao");
            DropForeignKey("dbo.Leito", "TabelaItemTissId", "dbo.TabelaDominio");
            DropForeignKey("dbo.TabelaDominio", "TipoTabelaDominioId", "dbo.TipoTabelaDominio");
            DropForeignKey("dbo.TabelaDominioVersaoTiss", "VersaoTissId", "dbo.VersaoTiss");
            DropForeignKey("dbo.TabelaDominioVersaoTiss", "TabelaDominioId", "dbo.TabelaDominio");
            DropForeignKey("dbo.TabelaDominio", "GrupoTipoTabelaDominioId", "dbo.GrupoTipoTabelaDominio");
            DropForeignKey("dbo.GrupoTipoTabelaDominio", "TipoTabelaDominioId", "dbo.TipoTabelaDominio");
            DropForeignKey("dbo.Leito", "LeitoStatusId", "dbo.LeitoStatus");
            DropForeignKey("dbo.Atendimento", "GuiaId", "dbo.Guia");
            DropForeignKey("dbo.Guia", "OriginariaId", "dbo.Guia");
            DropForeignKey("dbo.RelacaoGuiaCampo", "GuiaCampoId", "dbo.GuiaCampo");
            DropForeignKey("dbo.RelacaoGuiaCampo", "GuiaId", "dbo.Guia");
            DropForeignKey("dbo.Atendimento", "EspecialidadeId", "dbo.Especialidade");
            DropForeignKey("dbo.Atendimento", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Atendimento", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.Atendimento", "AtendimentoTipoId", "dbo.TipoAtendimento");
            DropForeignKey("dbo.AssistencialAtendimento", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.Empresa", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.Empresa", "PlanoId", "dbo.Plano");
            DropForeignKey("dbo.Empresa", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Empresa", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Empresa", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.Empresa", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.AssistencialAtendimento", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.UnidadeOrganizacional", "UnidadeInternacaoTipoId", "dbo.UnidadeInternacaoTipo");
            DropForeignKey("dbo.UnidadeOrganizacional", "OrganizationUnitId", "dbo.AbpOrganizationUnits");
            DropForeignKey("dbo.AssistencialAtendimento", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.AssistencialAtendimento", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.AssistencialAtendimento", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.AgendamentoConsulta", "PlanoId", "dbo.Plano");
            DropForeignKey("dbo.Plano", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.AgendamentoConsulta", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Paciente", "TipoSanguineoId", "dbo.TipoSanguineo");
            DropForeignKey("dbo.Paciente", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.Paciente", "ProfissaoId", "dbo.Profissao");
            DropForeignKey("dbo.Paciente", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.PacientePeso", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.Paciente", "NaturalidadeId", "dbo.Naturalidade");
            DropForeignKey("dbo.Paciente", "NacionalidadeId", "dbo.Nacionalidade");
            DropForeignKey("dbo.Paciente", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Paciente", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.AgendamentoConsulta", "MedicoEspecialidadeId", "dbo.MedicoEspecialidade");
            DropForeignKey("dbo.AgendamentoConsulta", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.AgendamentoConsulta", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.Convenio", "TipoLogradouroCobrancaId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.Convenio", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.Convenio", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Convenio", "EstadoCobrancaId", "dbo.Estado");
            DropForeignKey("dbo.Convenio", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Convenio", "CidadeCobrancaId", "dbo.Cidade");
            DropForeignKey("dbo.Convenio", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.Convenio", "CepCobrancaId", "dbo.Cep");
            DropForeignKey("dbo.Cep", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.Cep", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Cep", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Cep", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.AgendamentoConsulta", "AgendamentoConsultaMedicoDisponibilidadeId", "dbo.AgendamentoConsultaMedicoDisponibilidade");
            DropForeignKey("dbo.AgendamentoConsultaMedicoDisponibilidade", "MedicoEspecialidadeId", "dbo.MedicoEspecialidade");
            DropForeignKey("dbo.AgendamentoConsultaMedicoDisponibilidade", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.Medico", "TipoLogradouroId", "dbo.TipoLogradouro");
            DropForeignKey("dbo.Medico", "ProfissaoId", "dbo.Profissao");
            DropForeignKey("dbo.Medico", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.Medico", "NaturalidadeId", "dbo.Naturalidade");
            DropForeignKey("dbo.Medico", "NacionalidadeId", "dbo.Nacionalidade");
            DropForeignKey("dbo.MedicoEspecialidade", "MedicoId", "dbo.Medico");
            DropForeignKey("dbo.MedicoEspecialidade", "EspecialidadeId", "dbo.Especialidade");
            DropForeignKey("dbo.Medico", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Medico", "CidadeId", "dbo.Cidade");
            DropForeignKey("dbo.Cidade", "EstadoId", "dbo.Estado");
            DropForeignKey("dbo.Estado", "PaisId", "dbo.Pais");
            DropForeignKey("dbo.AgendamentoConsultaMedicoDisponibilidade", "IntervaloId", "dbo.Intervalo");
            DropIndex("dbo.NotaFiscalveicProd", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalveicProd", new[] { "Id" });
            DropIndex("dbo.NotaFiscalmed", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalmed", new[] { "Id" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "NotaFiscalEncerranteId" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "NotaFiscalCIDEId" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalcomb", new[] { "Id" });
            DropIndex("dbo.NotaFiscalarma", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalarma", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISST", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISQtde", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISOutr", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISNT", new[] { "Id" });
            DropIndex("dbo.NotaFiscalPISAliq", new[] { "Id" });
            DropIndex("dbo.NotaFiscalIPITrib", new[] { "Id" });
            DropIndex("dbo.NotaFiscalIPINT", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSST", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN900", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN500", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN202", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN201", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN102", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSSN101", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMSPart", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS90", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS70", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS60", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS51", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS40", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS30", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS20", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS10", new[] { "Id" });
            DropIndex("dbo.NotaFiscalICMS00", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSST", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSQtde", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSOutr", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSNT", new[] { "Id" });
            DropIndex("dbo.NotaFiscalCOFINSAliq", new[] { "Id" });
            DropIndex("dbo.UnidadeInternacao", new[] { "UnidadeInternacaoTipoId" });
            DropIndex("dbo.ProdutoListaSubstituicao", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoSubClasse", new[] { "GrupoClasseId" });
            DropIndex("dbo.ProdutoClasse", new[] { "GrupoId" });
            DropIndex("dbo.ProdutoRelacaoUnidade", new[] { "TipoUnidadeId" });
            DropIndex("dbo.ProdutoRelacaoUnidade", new[] { "UnidadeId" });
            DropIndex("dbo.ProdutoRelacaoUnidade", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoRelacaoPortaria", new[] { "ProdutoPortariaId" });
            DropIndex("dbo.ProdutoRelacaoPortaria", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoRelacaoPalavraChave", new[] { "ProdutoPalavraChaveId" });
            DropIndex("dbo.ProdutoRelacaoPalavraChave", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoRelacaoLaboratorio", new[] { "ProdutoLaboratorioId" });
            DropIndex("dbo.ProdutoRelacaoLaboratorio", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoRelacaoEstoque", new[] { "ProdutoLocalizacaoId" });
            DropIndex("dbo.ProdutoRelacaoEstoque", new[] { "ProdutoEstoqueId" });
            DropIndex("dbo.ProdutoRelacaoEstoque", new[] { "ProdutoId" });
            DropIndex("dbo.ProdutoRelacaoAcaoTerapeutica", new[] { "ProdutoAcaoTerapeuticaId" });
            DropIndex("dbo.ProdutoRelacaoAcaoTerapeutica", new[] { "ProdutoId" });
            DropIndex("dbo.PrestadorGrupoProcedimento", new[] { "GrupoProcedimentoId" });
            DropIndex("dbo.PrestadorGrupoProcedimento", new[] { "PrestadorId" });
            DropIndex("dbo.PrestadorCredenciamento", new[] { "ConvenioId" });
            DropIndex("dbo.PrestadorCredenciamento", new[] { "PrestadorId" });
            DropIndex("dbo.Prestador", new[] { "PaisId" });
            DropIndex("dbo.Prestador", new[] { "EstadoId" });
            DropIndex("dbo.Prestador", new[] { "CidadeId" });
            DropIndex("dbo.Prestador", new[] { "TipoLogradouroId" });
            DropIndex("dbo.Prestador", new[] { "NacionalidadeId" });
            DropIndex("dbo.Prestador", new[] { "NaturalidadeId" });
            DropIndex("dbo.Prestador", new[] { "ProfissaoId" });
            DropIndex("dbo.Prestador", new[] { "ConselhoId" });
            DropIndex("dbo.Prestador", new[] { "TipoPrestadorId" });
            DropIndex("dbo.Prestador", new[] { "CepComercialId" });
            DropIndex("dbo.Prestador", new[] { "TipoParticipacaoId" });
            DropIndex("dbo.Prestador", new[] { "TipoVinculoEmpregaticioId" });
            DropIndex("dbo.PacienteConvenioBloqueado", new[] { "ConvenioId" });
            DropIndex("dbo.Orcamento", new[] { "PacienteId" });
            DropIndex("dbo.Orcamento", new[] { "PreAtendimentoId" });
            DropIndex("dbo.Orcamento", new[] { "EmpresaId" });
            DropIndex("dbo.Orcamento", new[] { "PlanoId" });
            DropIndex("dbo.Orcamento", new[] { "ConvenioId" });
            DropIndex("dbo.NotaFiscalTotal", new[] { "retTrib_Id" });
            DropIndex("dbo.NotaFiscalTotal", new[] { "ISSQNtot_Id" });
            DropIndex("dbo.NotaFiscalTotal", new[] { "ICMSTot_Id" });
            DropIndex("dbo.NotaFiscalPagamento", new[] { "card_Id" });
            DropIndex("dbo.NotaFiscalPagamento", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalInformacaoAdicionalprocRef", new[] { "NotaFiscalInformacaoAdicionalId" });
            DropIndex("dbo.NotaFiscalInformacaoAdicionalobsFisco", new[] { "NotaFiscalInformacaoAdicionalId" });
            DropIndex("dbo.NotaFiscalInformacaoAdicionalCont", new[] { "NotaFiscalInformacaoAdicionalId" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "refNFP_Id" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "refNF_Id" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "refECF_Id" });
            DropIndex("dbo.NotaFiscalNFref", new[] { "NotaFiscalIdentificacaoId" });
            DropIndex("dbo.NotaFiscalEmitente", new[] { "NotaFiscalEnderecoEmitenteId" });
            DropIndex("dbo.NotaFiscaladi", new[] { "NotaFiscalDI_Id" });
            DropIndex("dbo.NotaFiscalDI", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscaldetExport", new[] { "NotaFiscalexportIndId" });
            DropIndex("dbo.NotaFiscaldetExport", new[] { "NotaFiscalProdutoId" });
            DropIndex("dbo.NotaFiscalProduto", new[] { "NotaFiscalProdutoEspecificoId" });
            DropIndex("dbo.NotaFiscalImpostoDevolvido", new[] { "IPI_Id" });
            DropIndex("dbo.NotaFiscalPIS", new[] { "TipoPIS_Id" });
            DropIndex("dbo.NotaFiscalIPI", new[] { "TipoIPI_Id" });
            DropIndex("dbo.NotaFiscalICMS", new[] { "TipoICMS_Id" });
            DropIndex("dbo.NotaFiscalCOFINS", new[] { "TipoCOFINS_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "PISST_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "PIS_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "ISSQN_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "IPI_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "II_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "ICMSUFDest_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "ICMS_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "COFINSST_Id" });
            DropIndex("dbo.NotaFiscalImposto", new[] { "COFINS_Id" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "prod_Id" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "impostoDevol_Id" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "imposto_Id" });
            DropIndex("dbo.NotaFiscalDetalhe", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalDestinatario", new[] { "NotaFiscalEnderecoDestinatarioId" });
            DropIndex("dbo.NotaFiscalCobrancaDuplicata", new[] { "NotaFiscalCobrancaId" });
            DropIndex("dbo.NotaFiscalCobranca", new[] { "NotaFiscalCobrancaFaturaId" });
            DropIndex("dbo.NotaFiscalCobranca", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscalcanaforDia", new[] { "NotaFiscalcanaId" });
            DropIndex("dbo.NotaFiscalcanadeduc", new[] { "NotaFiscalcanaId" });
            DropIndex("dbo.NotaFiscalautXML", new[] { "NotaFiscalId" });
            DropIndex("dbo.NotaFiscal", new[] { "TotalNota_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Retirada_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "InformacaoAdicional_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Ide_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Exporta_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Entrega_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Emitente_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Destinatario_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Compra_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Cana_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "Avulsa_Id" });
            DropIndex("dbo.NotaFiscal", new[] { "EmpresaId" });
            DropIndex("dbo.NotaFiscalManifestacaoDestinatario", new[] { "NotaFiscalId" });
            DropIndex("dbo.EstoquePreMovimentoLoteValidade", new[] { "LoteValidadeId" });
            DropIndex("dbo.EstoquePreMovimentoLoteValidade", new[] { "EstoquePreMovimentoItemId" });
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "ProdutoUnidadeId" });
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "ProdutoId" });
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "PreMovimentoId" });
            DropIndex("dbo.ProdutoLaboratorio", new[] { "FornecedorId" });
            DropIndex("dbo.LoteValidade", new[] { "ProdutoLaboratorioId" });
            DropIndex("dbo.LoteValidade", new[] { "ProdutoId" });
            DropIndex("dbo.EstoqueMovimentoLoteValidade", new[] { "LoteValidadeId" });
            DropIndex("dbo.EstoqueMovimentoLoteValidade", new[] { "EstoqueMovimentoItemId" });
            DropIndex("dbo.TipoDocumento", new[] { "TipoEntradaId" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "PaisId" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "EstadoId" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "CidadeId" });
            DropIndex("dbo.FornecedorPessoaJuridica", new[] { "TipoLogradouroId" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "PaisId" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "EstadoId" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "CidadeId" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "TipoLogradouroId" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "NacionalidadeId" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "NaturalidadeId" });
            DropIndex("dbo.FornecedorPessoaFisica", new[] { "ProfissaoId" });
            DropIndex("dbo.Fornecedor", new[] { "EmpresaId" });
            DropIndex("dbo.Fornecedor", new[] { "ConvenioId" });
            DropIndex("dbo.Fornecedor", new[] { "MedicoId" });
            DropIndex("dbo.Fornecedor", new[] { "PacienteId" });
            DropIndex("dbo.Fornecedor", new[] { "FornecedorPessoaJuridicaId" });
            DropIndex("dbo.Fornecedor", new[] { "FornecedorPessoaFisicaId" });
            DropIndex("dbo.Fornecedor", new[] { "TipoCadastroExistenteId" });
            DropIndex("dbo.Fornecedor", new[] { "TipoPessoaId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "Frete_FornecedorId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "TipoFreteId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "CFOPId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "OrdemId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "TipoDocumentoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "CentroCustoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "PreMovimentoEstadoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "EmpresaId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "FornecedorId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "TipoMovimentoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstoqueId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstoquePreMovimentoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "Frete_FornecedorId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "TipoFreteId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "CFOPId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "OrdemId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "TipoDocumentoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "CentroCustoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "PreMovimentoEstadoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EmpresaId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "FornecedorId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "TipoMovimentoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstoqueId" });
            DropIndex("dbo.EstoqueMovimentoItem", new[] { "ProdutoUnidadeId" });
            DropIndex("dbo.EstoqueMovimentoItem", new[] { "ProdutoId" });
            DropIndex("dbo.EstoqueMovimentoItem", new[] { "MovimentoId" });
            DropIndex("dbo.Est_Unidade", new[] { "UnidadeReferenciaId" });
            DropIndex("dbo.Est_ProdutoUnidade", new[] { "UnidadeTipoId" });
            DropIndex("dbo.Est_ProdutoUnidade", new[] { "UnidadeId" });
            DropIndex("dbo.Est_ProdutoUnidade", new[] { "ProdutoId" });
            DropIndex("dbo.Est_EstoqueLocalizacao", new[] { "EstoqueId" });
            DropIndex("dbo.Est_GrupoSubClasse", new[] { "GrupoClasseId" });
            DropIndex("dbo.Est_GrupoClasse", new[] { "GrupoId" });
            DropIndex("dbo.Est_Produto", new[] { "EstoqueLocalizacaoId" });
            DropIndex("dbo.Est_Produto", new[] { "DCBId" });
            DropIndex("dbo.Est_Produto", new[] { "GrupoSubClasseId" });
            DropIndex("dbo.Est_Produto", new[] { "GrupoClasseId" });
            DropIndex("dbo.Est_Produto", new[] { "GrupoId" });
            DropIndex("dbo.Est_Produto", new[] { "GeneroId" });
            DropIndex("dbo.Est_Produto", new[] { "ProdutoPrincipalId" });
            DropIndex("dbo.EntradaItem", new[] { "ProdutoId" });
            DropIndex("dbo.EntradaItem", new[] { "EntradaId" });
            DropIndex("dbo.Entrada", new[] { "EstoqueId" });
            DropIndex("dbo.Entrada", new[] { "CfopId" });
            DropIndex("dbo.Entrada", new[] { "CentroCustoId" });
            DropIndex("dbo.UserEmpresas", new[] { "EmpresaId" });
            DropIndex("dbo.UserEmpresas", new[] { "UserId" });
            DropIndex("dbo.AbpUsers", new[] { "EmpresaId" });
            DropIndex("dbo.ControleProducao", new[] { "TabelaSistemaId" });
            DropIndex("dbo.ControleProducao", new[] { "UsuarioAprovacaoId" });
            DropIndex("dbo.ControleProducao", new[] { "DesenvolvedorId" });
            DropIndex("dbo.ConsultorTabelaCampo", new[] { "ConsultorOcorrenciaId" });
            DropIndex("dbo.ConsultorTabelaCampo", new[] { "ConsultorTipoDadoNFId" });
            DropIndex("dbo.ConsultorTabelaCampo", new[] { "ConsultorTabelaId" });
            DropIndex("dbo.ConsultorTabelaCampoRelacao", new[] { "ConsultorTabelaCampoId" });
            DropIndex("dbo.ConsultorTabelaCampoRelacao", new[] { "ConsultorTabelaId" });
            DropIndex("dbo.RowConfig", new[] { "FormConfig_Id" });
            DropIndex("dbo.FormResposta", new[] { "FormConfigId" });
            DropIndex("dbo.FormData", new[] { "FormRespostaId" });
            DropIndex("dbo.FormData", new[] { "ColConfigId" });
            DropIndex("dbo.ColMultiOption", new[] { "ColConfig_Id" });
            DropIndex("dbo.ColConfig", new[] { "RowConfig_Id" });
            DropIndex("dbo.ClassificacaoRisco", new[] { "EspecialidadeId" });
            DropIndex("dbo.ClassificacaoRisco", new[] { "PacienteId" });
            DropIndex("dbo.ClassificacaoRisco", new[] { "PreAtendimentoId" });
            DropIndex("dbo.GrupoCentroCusto", new[] { "TipoGrupoCentroCustosId" });
            DropIndex("dbo.CentroCusto", new[] { "GrupoCentroCustoId" });
            DropIndex("dbo.Atestado", new[] { "ModeloAtestadoId" });
            DropIndex("dbo.Atestado", new[] { "TipoAtestadoId" });
            DropIndex("dbo.Atestado", new[] { "PacienteId" });
            DropIndex("dbo.Atestado", new[] { "MedicoId" });
            DropIndex("dbo.ServicoMedicoPrestado", new[] { "EspecialidadeId" });
            DropIndex("dbo.Origem", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.MotivoAlta", new[] { "MotivoAltaTipoAltaId" });
            DropIndex("dbo.TabelaDominioVersaoTiss", new[] { "VersaoTissId" });
            DropIndex("dbo.TabelaDominioVersaoTiss", new[] { "TabelaDominioId" });
            DropIndex("dbo.GrupoTipoTabelaDominio", new[] { "TipoTabelaDominioId" });
            DropIndex("dbo.TabelaDominio", new[] { "GrupoTipoTabelaDominioId" });
            DropIndex("dbo.TabelaDominio", new[] { "TipoTabelaDominioId" });
            DropIndex("dbo.Leito", new[] { "LeitoStatusId" });
            DropIndex("dbo.Leito", new[] { "TabelaItemTissId" });
            DropIndex("dbo.Leito", new[] { "TipoAcomodacaoId" });
            DropIndex("dbo.Leito", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.RelacaoGuiaCampo", new[] { "GuiaCampoId" });
            DropIndex("dbo.RelacaoGuiaCampo", new[] { "GuiaId" });
            DropIndex("dbo.Guia", new[] { "OriginariaId" });
            DropIndex("dbo.Atendimento", new[] { "ServicoMedicoPrestadoId" });
            DropIndex("dbo.Atendimento", new[] { "MotivoAltaId" });
            DropIndex("dbo.Atendimento", new[] { "LeitoId" });
            DropIndex("dbo.Atendimento", new[] { "GuiaId" });
            DropIndex("dbo.Atendimento", new[] { "AtendimentoTipoId" });
            DropIndex("dbo.Atendimento", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.Atendimento", new[] { "PlanoId" });
            DropIndex("dbo.Atendimento", new[] { "ConvenioId" });
            DropIndex("dbo.Atendimento", new[] { "EmpresaId" });
            DropIndex("dbo.Atendimento", new[] { "EspecialidadeId" });
            DropIndex("dbo.Atendimento", new[] { "MedicoId" });
            DropIndex("dbo.Atendimento", new[] { "OrigemId" });
            DropIndex("dbo.Atendimento", new[] { "PacienteId" });
            DropIndex("dbo.Empresa", new[] { "PaisId" });
            DropIndex("dbo.Empresa", new[] { "EstadoId" });
            DropIndex("dbo.Empresa", new[] { "CidadeId" });
            DropIndex("dbo.Empresa", new[] { "TipoLogradouroId" });
            DropIndex("dbo.Empresa", new[] { "PlanoId" });
            DropIndex("dbo.Empresa", new[] { "ConvenioId" });
            DropIndex("dbo.UnidadeOrganizacional", new[] { "CentroCusto_Id" });
            DropIndex("dbo.UnidadeOrganizacional", new[] { "OrganizationUnitId" });
            DropIndex("dbo.UnidadeOrganizacional", new[] { "UnidadeInternacaoTipoId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "EmpresaId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "ConvenioId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "MedicoId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "PacienteId" });
            DropIndex("dbo.Plano", new[] { "ConvenioId" });
            DropIndex("dbo.PacientePeso", new[] { "PacienteId" });
            DropIndex("dbo.Paciente", new[] { "PaisId" });
            DropIndex("dbo.Paciente", new[] { "EstadoId" });
            DropIndex("dbo.Paciente", new[] { "CidadeId" });
            DropIndex("dbo.Paciente", new[] { "TipoLogradouroId" });
            DropIndex("dbo.Paciente", new[] { "NacionalidadeId" });
            DropIndex("dbo.Paciente", new[] { "NaturalidadeId" });
            DropIndex("dbo.Paciente", new[] { "ProfissaoId" });
            DropIndex("dbo.Paciente", new[] { "TipoSanguineoId" });
            DropIndex("dbo.Cep", new[] { "TipoLogradouroId" });
            DropIndex("dbo.Cep", new[] { "PaisId" });
            DropIndex("dbo.Cep", new[] { "EstadoId" });
            DropIndex("dbo.Cep", new[] { "CidadeId" });
            DropIndex("dbo.Convenio", new[] { "PaisId" });
            DropIndex("dbo.Convenio", new[] { "EstadoId" });
            DropIndex("dbo.Convenio", new[] { "CidadeId" });
            DropIndex("dbo.Convenio", new[] { "TipoLogradouroId" });
            DropIndex("dbo.Convenio", new[] { "EstadoCobrancaId" });
            DropIndex("dbo.Convenio", new[] { "CidadeCobrancaId" });
            DropIndex("dbo.Convenio", new[] { "TipoLogradouroCobrancaId" });
            DropIndex("dbo.Convenio", new[] { "CepCobrancaId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "PlanoId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "ConvenioId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "PacienteId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "MedicoEspecialidadeId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "MedicoId" });
            DropIndex("dbo.AgendamentoConsulta", new[] { "AgendamentoConsultaMedicoDisponibilidadeId" });
            DropIndex("dbo.MedicoEspecialidade", new[] { "EspecialidadeId" });
            DropIndex("dbo.MedicoEspecialidade", new[] { "MedicoId" });
            DropIndex("dbo.Estado", new[] { "PaisId" });
            DropIndex("dbo.Cidade", new[] { "EstadoId" });
            DropIndex("dbo.Medico", new[] { "PaisId" });
            DropIndex("dbo.Medico", new[] { "EstadoId" });
            DropIndex("dbo.Medico", new[] { "CidadeId" });
            DropIndex("dbo.Medico", new[] { "TipoLogradouroId" });
            DropIndex("dbo.Medico", new[] { "NacionalidadeId" });
            DropIndex("dbo.Medico", new[] { "NaturalidadeId" });
            DropIndex("dbo.Medico", new[] { "ProfissaoId" });
            DropIndex("dbo.AgendamentoConsultaMedicoDisponibilidade", new[] { "IntervaloId" });
            DropIndex("dbo.AgendamentoConsultaMedicoDisponibilidade", new[] { "MedicoEspecialidadeId" });
            DropIndex("dbo.AgendamentoConsultaMedicoDisponibilidade", new[] { "MedicoId" });
            DropColumn("dbo.AbpUsers", "EmpresaId");
            DropTable("dbo.NotaFiscalveicProd");
            DropTable("dbo.NotaFiscalmed");
            DropTable("dbo.NotaFiscalcomb");
            DropTable("dbo.NotaFiscalarma");
            DropTable("dbo.NotaFiscalPISST");
            DropTable("dbo.NotaFiscalPISQtde");
            DropTable("dbo.NotaFiscalPISOutr");
            DropTable("dbo.NotaFiscalPISNT");
            DropTable("dbo.NotaFiscalPISAliq");
            DropTable("dbo.NotaFiscalIPITrib");
            DropTable("dbo.NotaFiscalIPINT");
            DropTable("dbo.NotaFiscalICMSST");
            DropTable("dbo.NotaFiscalICMSSN900");
            DropTable("dbo.NotaFiscalICMSSN500");
            DropTable("dbo.NotaFiscalICMSSN202");
            DropTable("dbo.NotaFiscalICMSSN201");
            DropTable("dbo.NotaFiscalICMSSN102");
            DropTable("dbo.NotaFiscalICMSSN101");
            DropTable("dbo.NotaFiscalICMSPart");
            DropTable("dbo.NotaFiscalICMS90");
            DropTable("dbo.NotaFiscalICMS70");
            DropTable("dbo.NotaFiscalICMS60");
            DropTable("dbo.NotaFiscalICMS51");
            DropTable("dbo.NotaFiscalICMS40");
            DropTable("dbo.NotaFiscalICMS30");
            DropTable("dbo.NotaFiscalICMS20");
            DropTable("dbo.NotaFiscalICMS10");
            DropTable("dbo.NotaFiscalICMS00");
            DropTable("dbo.NotaFiscalCOFINSST");
            DropTable("dbo.NotaFiscalCOFINSQtde");
            DropTable("dbo.NotaFiscalCOFINSOutr");
            DropTable("dbo.NotaFiscalCOFINSNT");
            DropTable("dbo.NotaFiscalCOFINSAliq");
            //DropTable("dbo.VWTeste");
            //DropTable("dbo.VWFaturamentoAberto6Meses");
            //DropTable("dbo.VWFaturamentoAberto");
            //DropTable("dbo.VWConsultaFaturamentoRecebimento");
            //DropTable("dbo.VWConsultaFaturamentoEntrega");
            //DropTable("dbo.VWConsultaFaturamentoAberto");
            DropTable("dbo.UnidadeInternacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeInternacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoUnidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoTelefone",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoTelefone_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoLeito",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Sexo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Sexo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Religiao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Religiao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Regiao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Regiao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoSubstancia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoSubstancia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoListaSubstituicao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoListaSubstituicao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoSubClasse",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoSubClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoClasse",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoTipoUnidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoTipoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoRelacaoUnidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoRelacaoPortaria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoPortaria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoRelacaoPalavraChave",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoPalavraChave_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoRelacaoLaboratorio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoLaboratorio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoRelacaoEstoque",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoRelacaoAcaoTerapeutica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoRelacaoAcaoTerapeutica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoPortaria",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoPortaria_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoPalavraChave",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoPalavraChave_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoLocalizacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoLocalizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoGrupoTratamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoGrupoTratamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoCodigoMedicamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoCodigoMedicamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoAcaoTerapeutica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoAcaoTerapeutica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PrestadorGrupoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrestadorGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PrestadorCredenciamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrestadorCredenciamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoVinculoEmpregaticio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoVinculoEmpregaticio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoPrestador",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoPrestador_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoParticipacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoParticipacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Prestador",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Prestador_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Parentesco",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Parentesco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PacienteConvenioBloqueado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PacienteConvenioBloqueado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Orcamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Orcamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.NotaFiscalretTrib");
            DropTable("dbo.NotaFiscalISSQNTot");
            DropTable("dbo.NotaFiscalICMSTot");
            DropTable("dbo.NotaFiscalTotal");
            DropTable("dbo.NotaFiscalRetirada");
            DropTable("dbo.NotaFiscalPagamentoCartao");
            DropTable("dbo.NotaFiscalPagamento");
            DropTable("dbo.NotaFiscalInformacaoAdicionalprocRef");
            DropTable("dbo.NotaFiscalInformacaoAdicionalobsFisco");
            DropTable("dbo.NotaFiscalInformacaoAdicionalCont");
            DropTable("dbo.NotaFiscalInformacaoAdicional");
            DropTable("dbo.NotaFiscalrefNFP");
            DropTable("dbo.NotaFiscalrefNF");
            DropTable("dbo.NotaFiscalrefECF");
            DropTable("dbo.NotaFiscalNFref");
            DropTable("dbo.NotaFiscalIdentificacao");
            DropTable("dbo.NotaFiscalexporta");
            DropTable("dbo.NotaFiscalEntrega");
            DropTable("dbo.NotaFiscalEnderecoEmitente");
            DropTable("dbo.NotaFiscalEmitente");
            DropTable("dbo.NotaFiscalEncerrante");
            DropTable("dbo.NotaFiscalCIDE");
            DropTable("dbo.NotaFiscalProdutoEspecifico");
            DropTable("dbo.NotaFiscaladi");
            DropTable("dbo.NotaFiscalDI");
            DropTable("dbo.NotaFiscalexportInd");
            DropTable("dbo.NotaFiscaldetExport");
            DropTable("dbo.NotaFiscalProduto");
            DropTable("dbo.NotaFiscalIPIDevolvido");
            DropTable("dbo.NotaFiscalImpostoDevolvido");
            DropTable("dbo.NotaFiscalPISBasico");
            DropTable("dbo.NotaFiscalPIS");
            DropTable("dbo.NotaFiscalISSQN");
            DropTable("dbo.NotaFiscalIPIBasico");
            DropTable("dbo.NotaFiscalIPI");
            DropTable("dbo.NotaFiscalICMSImpostoImportacao");
            DropTable("dbo.NotaFiscalICMSUFDest");
            DropTable("dbo.NotaFiscalICMSBasico");
            DropTable("dbo.NotaFiscalICMS");
            DropTable("dbo.NotaFiscalCOFINSBasico");
            DropTable("dbo.NotaFiscalCOFINS");
            DropTable("dbo.NotaFiscalImposto");
            DropTable("dbo.NotaFiscalDetalhe");
            DropTable("dbo.NotaFiscalEnderecoDestinatario");
            DropTable("dbo.NotaFiscalDestinatario");
            DropTable("dbo.NotaFiscalcompra");
            DropTable("dbo.NotaFiscalCobrancaFatura");
            DropTable("dbo.NotaFiscalCobrancaDuplicata");
            DropTable("dbo.NotaFiscalCobranca");
            DropTable("dbo.NotaFiscalcanaforDia");
            DropTable("dbo.NotaFiscalcanadeduc");
            DropTable("dbo.NotaFiscalcana");
            DropTable("dbo.NotaFiscalAvulsa");
            DropTable("dbo.NotaFiscalautXML");
            DropTable("dbo.NotaFiscal",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NotaFiscal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.NotaFiscalManifestacaoDestinatario",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_NotaFiscalManifestacaoDestinatario_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MotivoTransferenciaLeito",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoTransferenciaLeito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MotivoCaucao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoCaucao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MotivoCancelamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoCancelamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ModeloAtestado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ModeloAtestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MailingTemplate",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MailingTemplate_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LeitoServico",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LeitoServico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LeitoCaracteristica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LeitoCaracteristica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.InstituicaoTransferencia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_InstituicaoTransferencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Indicacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Indicacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GuiaTipo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GuiaTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GrupoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GrupoCID",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoCID_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GrauInstrucao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrauInstrucao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Feriado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Feriado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Favorito",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Favorito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoquePreMovimentoLoteValidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoLoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoquePreMovimentoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoLaboratorio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoLaboratorio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LoteValidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoqueMovimentoLoteValidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueMovimentoLoteValidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoqueTipoMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueTipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoFrete",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoFrete_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoEntrada",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoEntrada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoDocumento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoDocumento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoquePreMovimentoEstado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimentoEstado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.OrdemCompra",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_OrdemCompra_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoPessoa");
            DropTable("dbo.TipoCadastroExistente");
            DropTable("dbo.FornecedorPessoaJuridica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FornecedorPessoaJuridica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FornecedorPessoaFisica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FornecedorPessoaFisica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Fornecedor",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Fornecedor_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoquePreMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoquePreMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoqueMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstoqueMovimentoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueMovimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstadoCivil",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstadoCivil_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Escolaridade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Escolaridade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ProdutoEstoque",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_Unidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Unidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_UnidadeTipo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_ProdutoUnidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ProdutoUnidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_Genero",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Genero_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_Estoque",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Estoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_EstoqueLocalizacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueLocalizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_GrupoSubClasse",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoSubClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_Grupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Grupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_GrupoClasse",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoClasse_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_Produto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Produto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EntradaItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EntradaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Entrada",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Entrada_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Est_DCB",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DCB_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CorPele",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CorPele_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.UserEmpresas",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UserEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ControleProducao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ControleProducao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ConsultorTipoDadoNF",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorTipoDadoNF_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ConsultorTabelaCampo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorTabelaCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ConsultorTabela",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorTabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ConsultorTabelaCampoRelacao");
            DropTable("dbo.ConsultorOcorrencia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ConsultorOcorrencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Conselho",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Conselho_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RowConfig",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RowConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FormConfig",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FormResposta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FormData",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormData_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ColMultiOption",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ColMultiOption_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ColConfig",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ColConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PreAtendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PreAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ClassificacaoRisco",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ClassificacaoRisco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Cfop",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cfop_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoGrupoCentroCusto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoGrupoCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GrupoCentroCusto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CentroCusto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CapituloCID",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CapituloCID_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Banco",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Banco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoAtestado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoAtestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Atestado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Atestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ServicoMedicoPrestado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ServicoMedicoPrestado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Origem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Origem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MotivoAltaTipoAlta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoAltaTipoAlta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MotivoAlta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_MotivoAlta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoAcomodacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoAcomodacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.VersaoTiss",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_VersaoTiss_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TabelaDominioVersaoTiss");
            DropTable("dbo.TipoTabelaDominio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoTabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GrupoTipoTabelaDominio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoTipoTabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TabelaDominio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TabelaDominio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LeitoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_LeitoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Leito",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Leito_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.GuiaCampo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GuiaCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.RelacaoGuiaCampo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RelacaoGuiaCampo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Guia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Guia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoAtendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Atendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Atendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Empresa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Empresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.UnidadeInternacaoTipo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeInternacaoTipo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.UnidadeOrganizacional",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_UnidadeOrganizacional_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssistencialAtendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AssistencialAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Plano",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Plano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoSanguineo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoSanguineo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PacientePeso",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PacientePeso_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Paciente",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Paciente_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Cep",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cep_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Convenio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Convenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AgendamentoConsulta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoConsulta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.TipoLogradouro",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoLogradouro_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Profissao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Profissao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Naturalidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Naturalidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Nacionalidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Nacionalidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Especialidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Especialidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.MedicoEspecialidade");
            DropTable("dbo.Pais",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Pais_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Estado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Estado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Cidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Cidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Medico",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Medico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Intervalo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Intervalo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AgendamentoConsultaMedicoDisponibilidade",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AgendamentoConsultaMedicoDisponibilidade_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
