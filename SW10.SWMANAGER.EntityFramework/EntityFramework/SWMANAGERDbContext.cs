#region Usings
using Abp.Zero.EntityFramework;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.Chat;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.BalancoHidrico;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.InternacoesTev;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Diagnosticos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.TiposAcompanhantes;
using SW10.SWMANAGER.ClassesAplicacao.AtendimentosLeitosMov;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Atestados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.ModelosPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.TiposAtendimento;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Bairros;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CapitulosCID;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CEP;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ElementosHtml;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Enderecos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Feriados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GrausInstrucoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCentroCusto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposProcedimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposTipoTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Indicacoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.InstituicoesTransferencia;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Intervalos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCancelamento;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCaucao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosPerdaProdutos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosTransferenciaLeito;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Nacionalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.PacientesConveniosBloqueados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Parentescos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Prestadores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.PrestadoresCredenciamentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.PrestadoresGruposProcedimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosAcoesTerapeutica;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosClasse;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosCodigosMedicamento;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosGrupo;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosGruposTratamento;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLocalizacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPalavrasChave;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPortaria;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosSubClasse;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosSubstancia;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosTiposUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Regioes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposEntrada;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposParticipacoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposPrestadores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposSanguineos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposVinculosEmpregaticios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using SW10.SWMANAGER.ClassesAplicacao.ClassificacoesRisco;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Modulos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Controladorias.NotasFiscais;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem;
using SW10.SWMANAGER.ClassesAplicacao.Eventos.Eventos;
using SW10.SWMANAGER.ClassesAplicacao.Eventos.EventosGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Eventos.EventosMov;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Autorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasImports;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasIndices;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasPrecos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.CodigoCredenciado;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Relacionamento;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaConvenioCodigo;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaPrecoItens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Taxas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TiposGrupo;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Impressoras;
using SW10.SWMANAGER.ClassesAplicacao.Manutencoes.BIs;
using SW10.SWMANAGER.ClassesAplicacao.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
using SW10.SWMANAGER.ClassesAplicacao.Orcamentos;
using SW10.SWMANAGER.ClassesAplicacao.Pessoas;
using SW10.SWMANAGER.ClassesAplicacao.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Religioes;
using SW10.SWMANAGER.ClassesAplicacao.Sefaz;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Entradas;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using SW10.SWMANAGER.ClassesAplicacao.Visitantes;
using SW10.SWMANAGER.ClassesAplicacao.VisualASA;
using SW10.SWMANAGER.ClassesAplicacao.VisualAsaImportExportLogs;
using SW10.SWMANAGER.Friendships;
using SW10.SWMANAGER.MultiTenancy;
using SW10.SWMANAGER.Storage;
using System.Data.Common;
using System.Data.Entity;
using SW10.SWMANAGER.ClassesAplicacao.Avisos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using SW10.SWMANAGER.Notifications;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Honorarios;
using SW10.SWMANAGER.ClassesAplicacao.Anexos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.AwsS3;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.EntregaContas;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Receituarios;

#endregion usings.

namespace SW10.SWMANAGER.EntityFramework
{


    public class SWMANAGERDbContext : AbpZeroDbContext<Tenant, Role, User>
    {

        /* Define an IDbSet for each entity of the application */

        public virtual IDbSet<Anexo> Anexo { get; set; }

        public virtual IDbSet<AwsS3Configuracao> AwsS3Configuracao { get; set; }

        public virtual IDbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual IDbSet<Friendship> Friendships { get; set; }

        public virtual IDbSet<ChatMessage> ChatMessages { get; set; }

        public virtual IDbSet<TipoAtendimento> TiposAtendimento { get; set; }

        public virtual IDbSet<MotivoCancelamento> MotivosCancelamento { get; set; }

        public virtual IDbSet<InstituicaoTransferencia> InstituicoesTransferencia { get; set; }

        public virtual IDbSet<MotivoCaucao> MotivosCaucao { get; set; }

        public virtual IDbSet<MotivoTransferenciaLeito> MotivosTransferenciaLeito { get; set; }

        public virtual IDbSet<Paciente> Pacientes { get; set; }

        public virtual IDbSet<Convenio> Convenios { get; set; }

        public virtual IDbSet<ConvenioConfiguracaoResumoConta> ConvenioConfiguracaoResumoContas { get;set;}

        public virtual IDbSet<PacientePeso> PacientePesos { get; set; }

        public virtual IDbSet<PacienteAlergias> PacienteAlergias { get; set; }

        public virtual IDbSet<Profissao> Profissoes { get; set; }

        public virtual IDbSet<Naturalidade> Naturalidades { get; set; }

        public virtual IDbSet<Empresa> Empresas { get; set; }

        public virtual IDbSet<Origem> Origens { get; set; }

        public virtual IDbSet<Plano> Planos { get; set; }

        public virtual IDbSet<Pais> Paises { get; set; }

        public virtual IDbSet<Estado> Estados { get; set; }

        public virtual IDbSet<Cidade> Cidades { get; set; }

        public virtual IDbSet<Bairro> Bairros { get; set; }

        public virtual IDbSet<Cep> Ceps { get; set; }

        public virtual IDbSet<Medico> Medicos { get; set; }

        public virtual IDbSet<MedicoEspecialidade> MedicoEspecialidades { get; set; }

        public virtual IDbSet<Especialidade> Especialidades { get; set; }

        public virtual IDbSet<Intervalo> Intervalos { get; set; }

        public virtual IDbSet<Sexo> Sexos { get; set; }

        public virtual IDbSet<CorPele> CoresPele { get; set; }

        public virtual IDbSet<Turno> Turnos { get; set; }

        public virtual IDbSet<Escolaridade> Escolaridades { get; set; }

        public virtual IDbSet<Religiao> Religioes { get; set; }

        public virtual IDbSet<EstadoCivil> EstadosCivis { get; set; }

        public virtual IDbSet<TipoTelefone> TiposTelefone { get; set; }

        public virtual IDbSet<AgendamentoConsulta> AgendamentoConsultas { get; set; }

        public virtual IDbSet<AgendamentoConsultaMedicoDisponibilidade> AgendamentoConsultaMedicoDisponibilidades { get; set; }

        public virtual IDbSet<NotaFiscal> NotasFiscais { get; set; }

        public virtual IDbSet<TipoAcomodacao> TiposAcomodacoes { get; set; }

        public virtual IDbSet<TipoTabelaDominio> TiposTabelaDominio { get; set; }

        public virtual IDbSet<GrupoTipoTabelaDominio> GruposTipoTabelaDominio { get; set; }

        public virtual IDbSet<TabelaDominio> TabelasDominio { get; set; }

        public virtual IDbSet<TabelaDominioVersaoTiss> TabelaDominioVersoesTiss { get; set; }

        public virtual IDbSet<VersaoTiss> VersoesTiss { get; set; }

        public virtual IDbSet<Fornecedor> Fornecedores { get; set; }

        public virtual IDbSet<SisPessoa> SisPessoas { get; set; }

        public virtual IDbSet<TipoPessoa> TiposPessoa { get; set; }

        public virtual IDbSet<TipoCadastroExistente> TiposCadastroExistente { get; set; }

        public virtual IDbSet<FornecedorPessoaFisica> FornecedoresPessoaFisica { get; set; }

        public virtual IDbSet<FornecedorPessoaJuridica> FornecedoresPessoaJuridica { get; set; }

        public virtual IDbSet<BI> BIs { get; set; }

        public virtual IDbSet<SisTipoPessoa> SisTiposPessoa { get; set; }

        public virtual IDbSet<TipoEndereco> TiposEnderecos { get; set; }

        public virtual IDbSet<Endereco> Enderecos { get; set; }

        public virtual IDbSet<SisFornecedor> SisFornecedores { get; set; }

        public virtual IDbSet<Cbo> Cbos { get; set; }

        public virtual IDbSet<CboFamilia> CboFamilias { get; set; }

        public virtual IDbSet<CboTipo> CboTipos { get; set; }

        public virtual IDbSet<HoraDia> HorasDia { get; set; }

        public virtual IDbSet<VisualAsaImportExportLog> VisualAsaImportExportLogs { get; set; }

        public virtual IDbSet<AtendimentoGrupoCID> AtendimentosGruposCID { get; set; }

        public virtual IDbSet<ClassificacaoAtendimento> ClassificacoesAtendimentos { get; set; }

        public virtual IDbSet<ProtocoloAtendimento> ProtocolosAtendimentos { get; set; }

        public virtual IDbSet<IntervaloGuiasConvenio> IntervalosGuiasConvenios { get; set; }

        public virtual IDbSet<Ocorrencia> Ocorrencias { get; set; }
        public virtual IDbSet<TipoOcorrencia> TipoOcorrencias { get; set; }
        
        public virtual IDbSet<SubTipoOcorrencia> SubTipoOcorrencias { get; set; }


        // Consultor
        public virtual IDbSet<ConsultorOcorrencia> ConsultorOcorrencias { get; set; }

        public virtual IDbSet<ConsultorTipoDadoNF> ConsultorTipoDadosNF { get; set; }

        public virtual IDbSet<ConsultorTabela> ConsultorTabelas { get; set; }

        public virtual IDbSet<ConsultorTabelaCampo> ConsultorTabelaCampos { get; set; }

        public virtual IDbSet<ConsultorTabelaCampoRelacao> ConsultorTabelaCampoRelacoes { get; set; }

        public virtual IDbSet<ControleProducao> ControleProducoes { get; set; }

        #region Atendimento

        public virtual IDbSet<MotivoAlta> MotivosAlta { get; set; }

        public virtual IDbSet<MotivoAltaTipoAlta> MotivoAltaTiposAlta { get; set; }

        public virtual IDbSet<LeitoStatus> LeitosStatus { get; set; }

        public virtual IDbSet<LeitoCaracteristica> LeitoCaracteristicas { get; set; }

        public virtual IDbSet<LeitoServico> LeitoServicos { get; set; }

        public virtual IDbSet<Leito> Leitos { get; set; }

        public virtual IDbSet<UnidadeInternacao> UnidadesInternacao { get; set; }

        public virtual IDbSet<UnidadeInternacaoTipo> UnidadeInternacaoTipos { get; set; }

        public virtual IDbSet<Atendimento> Atendimentos { get; set; }

        public virtual IDbSet<UltimoId> UltimosIds { get; set; }

        public virtual IDbSet<PreAtendimento> PreAtendimentos { get; set; }

        public virtual IDbSet<ClassificacaoRisco> ClassificacoesRisco { get; set; }

        public virtual IDbSet<Orcamento> Orcamento { get; set; }

        public virtual IDbSet<Favorito> Favoritos { get; set; }

        public virtual IDbSet<GuiaTipo> GuiaTipos { get; set; }

        public virtual IDbSet<ServicoMedicoPrestado> ServicosMedicosPrestados { get; set; }

        public virtual IDbSet<Guia> Guia { get; set; }

        public virtual IDbSet<GuiaCampo> GuiaCampo { get; set; }

        public virtual IDbSet<RelacaoGuiaCampo> RelacaoGuiaCampo { get; set; }

        public virtual IDbSet<Visitante> AteVisitante { get; set; }

        public virtual IDbSet<TipoAcompanhante> TipoAcompanhante { get; set; }

        public virtual IDbSet<AtendimentoLeitoMov> AteAtendimentoLeitoMov { get; set; }

        public virtual IDbSet<AtendimentoStatus> AtendimentosStatus { get; set; }

        public virtual IDbSet<AtendimentoMotivoCancelamento> AtendimentoMotivosCancelamentos { get; set; }

        public virtual IDbSet<SalaCirurgica> SalasCirurgicas { get; set; }

        public virtual IDbSet<AgendamentoCirurgico> AgendamentosCirurgicos { get; set; }

        public virtual IDbSet<AgendamentoItemFaturamento> AgendamentosItensFaturamentos { get; set; }

        public virtual IDbSet<AgendamentoMaterial> AgendamentosMateriais { get; set; }

        public virtual IDbSet<TipoCirurgia> TiposCirurgias { get; set; }

        public virtual IDbSet<AgendamentoSalaCirurgicaDisponibilidade> AgendamentoSalasCirurgicasDisponibilidades { get; set; }

        public virtual IDbSet<AgendamentoStatus> AgendamentosStatus { get; set; }

        public virtual IDbSet<PacienteDiagnosticos> PacienteDiagnosticos { get; set; }


        #endregion

        public virtual IDbSet<TipoLogradouro> TiposLogradouro { get; set; }

        //  public virtual IDbSet<TipoLeito> TiposLeito { get; set; }

        public virtual IDbSet<GrupoCentroCusto> GruposCentroCusto { get; set; }

        public virtual IDbSet<ProdutoPalavraChave> ProdutoPalavraChave { get; set; }

        public virtual IDbSet<ProdutoAcaoTerapeutica> ProdutoAcaoTerapeutica { get; set; }

        public virtual IDbSet<ProdutoTipoUnidade> ProdutoTipoUnidade { get; set; }

        public virtual IDbSet<TipoUnidade> TiposUnidade { get; set; }

        public virtual IDbSet<ProdutoRelacaoUnidade> ProdutoRelacaoUnidade { get; set; }

        public virtual IDbSet<Regiao> Regioes { get; set; }

        public virtual IDbSet<ProdutoGrupo> ProdutosGrupos { get; set; }

        public virtual IDbSet<EstoqueLaboratorio> EstoqueLaboratorio { get; set; }

        public virtual IDbSet<ProdutoPortaria> ProdutoPortaria { get; set; }

        public virtual IDbSet<ProdutoGrupoTratamento> ProdutoGrupoTratamento { get; set; }

        public virtual IDbSet<ProdutoLocalizacao> ProdutoLocalizacao { get; set; }

        public virtual IDbSet<ProdutoCodigoMedicamento> ProdutoCodigoMedicamento { get; set; }

        public virtual IDbSet<ProdutoEstoque> ProdutoEstoque { get; set; }

        public virtual IDbSet<ProdutoEmpresa> ProdutoEmpresa { get; set; }

        public virtual IDbSet<ProdutoClasse> ProdutosClasses { get; set; }

        public virtual IDbSet<ProdutoSubClasse> ProdutosSubClasses { get; set; }

        public virtual IDbSet<ProdutoSubstancia> ProdutoSubstancia { get; set; }

        public virtual IDbSet<TipoInventario> TiposInventarios { get; set; }
        public virtual IDbSet<StatusInventario> StatusInventarios { get; set; }

        public virtual IDbSet<StatusInventarioItem> StatusInventarioItems { get; set; }
        public virtual IDbSet<Inventario> Inventarios { get; set; }
        public virtual IDbSet<InventarioItem> InventariosItens { get; set; }

        public virtual IDbSet<UserEmpresa> UserEmpresas { get; set; }

        //Dashboard
        public virtual IDbSet<VWTeste> VWTestes { get; set; }

        //pablo - em 03/04/17
        public virtual IDbSet<CentroCusto> CentrosCustos { get; set; }

        public virtual IDbSet<UnidadeOrganizacional> UnidadesOrganizacionais { get; set; }

        public virtual IDbSet<Nacionalidade> Nacionalidades { get; set; }

        public virtual IDbSet<GrauInstrucao> GrauInstrucoes { get; set; }

        public virtual IDbSet<Indicacao> Indicacoes { get; set; }

        public virtual IDbSet<Feriado> Feriados { get; set; }

        public virtual IDbSet<Parentesco> Parentescos { get; set; }

        public virtual IDbSet<TipoPrestador> TipoPrestadores { get; set; }

        public virtual IDbSet<CapituloCID> CapitulosCid { get; set; }

        public virtual IDbSet<GrupoCID> GruposCid { get; set; }

        public virtual IDbSet<TipoSanguineo> TiposSanguineos { get; set; }

        public virtual IDbSet<PacienteConvenioBloqueado> PacientesConveniosBloqueados { get; set; }

        public virtual IDbSet<GrupoProcedimento> GruposProcedimentos { get; set; }

        public virtual IDbSet<TipoVinculoEmpregaticio> TiposVinculosEmpregaticios { get; set; }

        public virtual IDbSet<TipoParticipacao> TiposParticipacoes { get; set; }

        public virtual IDbSet<Prestador> Prestadores { get; set; }

        public virtual IDbSet<Conselho> Conselho { get; set; }

        public virtual IDbSet<PrestadorCredenciamento> PrestadoresCredenciamentos { get; set; }

        public virtual IDbSet<PrestadorGrupoProcedimento> PrestadoresGruposProcedimentos { get; set; }

        public virtual IDbSet<Evento> Eventos { get; set; }

        public virtual IDbSet<EventoGrupo> EventosGrupos { get; set; }

        public virtual IDbSet<EventoMov> EventosMovs { get; set; }

        //====pablo==fim====

        public virtual IDbSet<MailingTemplate> MailingTemplates { get; set; }

        public virtual IDbSet<VWFaturamentoAberto> VWFaturamentoAberto { get; set; }

        public virtual IDbSet<VWConsultaFaturamentoAberto> VWConsultaFaturamentoAberto { get; set; }

        public virtual IDbSet<VWConsultaFaturamentoEntrega> VWConsultaFaturamentoEntrega { get; set; }

        public virtual IDbSet<VWConsultaFaturamentoRecebimento> VWConsultaFaturamentoRecebimento { get; set; }

        public virtual IDbSet<VWEmpresa> VWEmpresas { get; set; }

        public virtual IDbSet<TipoAtestado> TiposAtestados { get; set; }

        public virtual IDbSet<ModeloAtestado> ModelosAtestados { get; set; }

        public virtual IDbSet<Atestado> Atestados { get; set; }

        public virtual IDbSet<VelocidadeInfusao> VelocidadesInfusao { get; set; }

        public virtual IDbSet<VelocidadeInfusaoFormaAplicacao> VelocidadeInfusaoFormaAplicacao { get; set; }

        //PRODUTO X RELACIONAMENTOS
        public virtual IDbSet<ProdutoListaSubstituicao> ProdutosListaSubstituicao { get; set; }
        public virtual IDbSet<ProdutoRelacaoPortaria> ProdutoRelacaoPortaria { get; set; }
        public virtual IDbSet<ProdutoRelacaoLaboratorio> ProdutoRelacaoLaboratorio { get; set; }
        public virtual IDbSet<ProdutoRelacaoPalavraChave> ProdutoRelacaoPalavraChave { get; set; }
        public virtual IDbSet<ProdutoRelacaoAcaoTerapeutica> ProdutoRelacaoAcaoTerapeutica { get; set; }

        //ENTRADA/ROGERIO
        public virtual IDbSet<Entrada> Entradas { get; set; }
        public virtual IDbSet<EntradaItem> EntradasItens { get; set; }
        public virtual IDbSet<Cfop> Cfops { get; set; }
        public virtual IDbSet<TipoEntrada> TiposEntrada { get; set; }
        public virtual IDbSet<FormConfig> FormsConfig { get; set; }
        public virtual IDbSet<FormConfigOperacao> FormsConfigOperacoes { get; set; }
        public virtual IDbSet<FormConfigUnidadeOrganizacional> FormsConfigUnidadesOrganizacionais { get; set; }
        public virtual IDbSet<FormConfigEspecialidade> FormsConfigEspecialidades { get; set; }
        public virtual IDbSet<RowConfig> RowsConfig { get; set; }
        public virtual IDbSet<ColConfig> ColsConfig { get; set; }
        public virtual IDbSet<ColMultiOption> ColsMultiOption { get; set; }
        public virtual IDbSet<FormData> FormsData { get; set; }
        public virtual IDbSet<FormResposta> FormsRespostas { get; set; }


        public virtual IDbSet<EstoquePreMovimento> EstoquePreMovimento { get; set; }
        public virtual IDbSet<EstoquePreMovimentoItem> EstoquePreMovimentoItem { get; set; }
        public virtual IDbSet<EstoquePreMovimentoEstado> EstoquePreMovimentoEstado { get; set; }
        public virtual IDbSet<EstoquePreMovimentoLoteValidade> EstoquePreMovimentoLoteValidade { get; set; }
        public virtual IDbSet<EstoqueEmprestimo> EstoqueEmprestimo { get; set; }
        public virtual IDbSet<ClassesAplicacao.Cadastros.OrdemCompra> OrdemCompra { get; set; }
        public virtual IDbSet<TipoFrete> TipoFrete { get; set; }
        public virtual IDbSet<LoteValidade> LoteValidade { get; set; }
        public virtual IDbSet<TipoMovimento> EstTipoMovimentos { get; set; }
        public virtual IDbSet<TipoOperacao> EstTipoOperacoes { get; set; }

        public virtual IDbSet<EstoqueMovimento> EstoqueMovimentos { get; set; }
        public virtual IDbSet<EstoqueMovimentoItem> EstoqueMovimentoItens { get; set; }
        public virtual IDbSet<EstoqueMovimentoLoteValidade> EstoqueMovimentoLoteValidades { get; set; }
        public virtual IDbSet<EstoqueTransferenciaProduto> EstoqueTransferenciaProdutos { get; set; }
        public virtual IDbSet<EstoqueTransferenciaProdutoItem> EstoqueTransferenciaProdutosItens { get; set; }
        public virtual IDbSet<ProdutoSaldo> ProdutoSaldos { get; set; }
        public virtual IDbSet<ProdutoSaldoEmprestimo> ProdutoSaldoEmprestimos { get; set; }
        public virtual IDbSet<MotivoPerdaProduto> MotivosPerdaProdutos { get; set; }
        public virtual IDbSet<EstMovimentoBaixa> EstMovimentoBaixas { get; set; }
        public virtual IDbSet<EstMovimentoBaixaItem> EstMovimentoBaixasItens { get; set; }
        public virtual IDbSet<EstoqueBaixaEmprestimo> EstoqueBaixaEmprestimos { get; set; }
        public virtual IDbSet<EstoqueBaixaEmprestimoItem> EstoqueBaixaEmprestimoItens { get; set; }
        public virtual IDbSet<EstoqueGrupoOperacao> EstoqueGrupoOperacoes { get; set; }
        public virtual IDbSet<EstoqueSolicitacaoItem> EstoqueSolicitacaoItens { get; set; }
        public virtual IDbSet<EstoqueItemSolicitacaoAtendida> EstoqueItensSolicitacoesAtendidas { get; set; }
        public virtual IDbSet<EstoqueImportacaoProduto> EstoqueImportacaoProdutos { get; set; }
        public virtual IDbSet<EstoqueKit> EstoqueKits { get; set; }
        public virtual IDbSet<EstoqueKitItem> EstoqueKitItens{ get; set; }
        public virtual IDbSet<EstoquePreMovimentoItemKitLoteValidade> EstoquesPreMovimentosItensKitLotesValidades { get; set; }
        public virtual IDbSet<EstoquePreMovimentoItemKit> EstoquesPreMovimentosItensKits { get; set; }
        public virtual IDbSet<EstoqueEtiqueta> EstoquesEtiquetas { get; set; }
        


        //NOVO PRODUTO       
        public virtual IDbSet<Produto> Produtos { get; set; }
        public virtual IDbSet<ProdutoUnidade> ProdutoUnidades { get; set; }
        public virtual IDbSet<Estoque> Estoques { get; set; }
        public virtual IDbSet<EstoqueGrupo> EstoquesGrupo { get; set; }
        public virtual IDbSet<EstoqueEmpresa> EstoquesEmpresa { get; set; }
        public virtual IDbSet<EstoqueLocalizacao> EstoqueLocalizacao { get; set; }
        public virtual IDbSet<GrupoClasse> GrupoClasses { get; set; }
        public virtual IDbSet<Genero> Generos { get; set; }
        public virtual IDbSet<Grupo> Grupos { get; set; }
        public virtual IDbSet<DCB> DCBs { get; set; }
        public virtual IDbSet<GrupoSubClasse> GrupoSubClasses { get; set; }
        public virtual IDbSet<Unidade> Unidades { get; set; }
        public virtual IDbSet<UnidadeTipo> UnidadeTipos { get; set; }


        //Dashboard
        public virtual IDbSet<VWFaturamentoAbertoSeisMeses> VWFaturamentosAbertosSeisMeses { get; set; }


        // SisMoeda
        public virtual IDbSet<SisMoeda> SisMoedas { get; set; }
        public virtual IDbSet<SisMoedaCotacao> SisMoedaCotacoes { get; set; }
        public virtual IDbSet<SisMoedaCotacaoItem> SisMoedaCotacaoItens { get; set; }
        public virtual IDbSet<SisMoedaCotacaoPlano> SisMoedaCotacaoPlanos { get; set; }

        // Faturamento
        public virtual IDbSet<FaturamentoGrupo> FaturamentoGrupos { get; set; }
        public virtual IDbSet<FaturamentoItem> FaturamentoItens { get; set; }

        public virtual IDbSet<FaturamentoValoresHonorario> FaturamentoValoresHonorarios { get; set; }


        public virtual IDbSet<FaturamentoKit> FaturamentoKits { get; set; }
        public virtual IDbSet<FaturamentoItemTabela> FaturamentoItensTabela { get; set; }
        public virtual IDbSet<FaturamentoSubGrupo> FaturamentoSubGrupos { get; set; }
        public virtual IDbSet<FaturamentoTabela> FaturamentoTabelas { get; set; }
        public virtual IDbSet<FaturamentoTabelaPrecoItem> FaturamentoTabelaPrecoItens { get; set; }
        public virtual IDbSet<FaturamentoTipoGrupo> FaturamentoTiposGrupo { get; set; }
        public virtual IDbSet<FaturamentoConta> FaturamentoConta { get; set; }
        public virtual IDbSet<FaturamentoItemAtendimento> FaturamentoItemAtendimento { get; set; }
        public virtual IDbSet<FaturamentoAtendimentoStatus> FaturamentoAtendimentoStatus { get; set; }
        public virtual IDbSet<FaturamentoBrasIndice> FaturamentoBrasindices { get; set; }
        public virtual IDbSet<FaturamentoBrasItem> FaturamentoBrasItens { get; set; }
        public virtual IDbSet<FaturamentoBrasImport> FaturamentoBrasImports { get; set; }
        public virtual IDbSet<FaturamentoBrasLaboratorio> FaturamentoBrasLaboratorios { get; set; }
        public virtual IDbSet<FaturamentoBrasPreco> FaturamentoBrasPrecos { get; set; }
        public virtual IDbSet<FaturamentoBrasApresentacao> FaturamentoBrasApresentacoes { get; set; }
        public virtual IDbSet<FaturamentoContaItem> FaturamentoContaItens { get; set; }
        public virtual IDbSet<FaturamentoContaKit> FaturamentoContaKits { get; set; }
        public virtual IDbSet<FaturamentoConfigConvenio> FaturamentoConfigConvenios { get; set; }
        public virtual IDbSet<FaturamentoTaxa> FaturamentoTaxas { get; set; }
        public virtual IDbSet<FaturamentoTaxaEmpresa> FaturamentoTaxaEmpresas { get; set; }
        public virtual IDbSet<FaturamentoTaxaLocal> FaturamentoTaxaLocais { get; set; }
        public virtual IDbSet<FaturamentoTaxaTurno> FaturamentoTaxaTurnos { get; set; }
        public virtual IDbSet<FaturamentoTaxaTipoLeito> FaturamentoTaxaTipoLeitos { get; set; }
        public virtual IDbSet<FaturamentoTaxaItem> FaturamentoTaxaItem { get; set; }
        public virtual IDbSet<FaturamentoTaxaGrupo> FaturamentoTaxaGrupos { get; set; }
        public virtual IDbSet<FaturamentoItemAutorizacao> FaturamentoItensAutorizacoes { get; set; }
        public virtual IDbSet<FaturamentoItemConfig> FaturamentoItensConfigs { get; set; }
        public virtual IDbSet<FaturamentoGlobal> FaturamentoGlobais { get; set; }
        public virtual IDbSet<FaturamentoItemConfigGlobal> FaturamentoItemConfigGlobais { get; set; }
        public virtual IDbSet<FaturamentoConfigConvenioGlobal> FaturamentoConfigConvenioGlobais { get; set; }
        public virtual IDbSet<FaturamentoGuia> FaturamentoGuias { get; set; }
        public virtual IDbSet<FaturamentoGuiaConvenio> FaturamentoGuiasConvenios { get; set; }
        public virtual IDbSet<FaturamentoGrupoConvenio> FaturamentoGruposConvenios { get; set; }
        public virtual IDbSet<FaturamentoEntregaLote> FaturamentoEntregasLotes { get; set; }
        public virtual IDbSet<FaturamentoEntregaConta> FaturamentoEntregasContas { get; set; }
        public virtual IDbSet<FaturamentoEntregaContaRecebida> FaturamentoEntregasContasRecebidas { get; set; }
        public virtual IDbSet<FaturamentoContaStatus> FaturamentoContaStatus { get; set; }
        public virtual IDbSet<FaturamentoKitItem> FaturamentoKitItens { get; set; }

        public virtual IDbSet<FaturamentoTabelaRelacionamento> FaturamentoTabelaRelacionamentos { get; set; }
        public virtual IDbSet<FaturamentoItemRelacionamento> FaturamentoItemRelacionamentos { get; set; }
        public virtual IDbSet<FaturamentoConvenioRelacionamento> FaturamentoConvenioRelacionamentos { get; set; }
        public virtual IDbSet<FaturamentoAutorizacao> FaturamentoAutorizacoes { get; set; }
        public virtual IDbSet<FaturamentoAutorizacaoDetalhe> FaturamentoAutorizacaoDetalhes { get; set; }
        public virtual IDbSet<FaturamentoPacote> FaturamentosPacotes { get; set; }
        public virtual IDbSet<FaturamentoCodigoDespesa> FaturamentosCodigosDespesas { get; set; }
        public virtual IDbSet<FaturamentoSequenciaLote> FaturamentoSequenciasLotes { get; set; }
        public virtual IDbSet<IdentificacaoPrestadorNaOperadora> IdentificacoesPrestadoresNasOperadoras { get; set; }

        public virtual IDbSet<MovimentoAutomatico> MovimentosAutomaticos { get; set; }
        public virtual IDbSet<MovimentoAutomaticoFaturamentoItem> MovimentosAutomaticosFaturamentoItens { get; set; }
        public virtual IDbSet<MovimentoAutomaticoConvenioPlano> MovimentosAutomaticosConveniosPlanos { get; set; }
        public virtual IDbSet<MovimentoAutomaticoEspecialidade> MovimentosAutomaticosEspecialidades { get; set; }
        public virtual IDbSet<MovimentoAutomaticoTipoGuia> MovimentosAutomaticosTiposGuias { get; set; }

        public virtual IDbSet<TextoModelo> TextoModelos { get; set; }
        public virtual IDbSet<TextoModeloEmpresa> TextoModeloEmpresas { get; set; }
        public virtual IDbSet<TextoModeloGuia> TextoModeloGuias { get; set; }

        public virtual IDbSet<Terceirizado> Terceirizados { get; set; }

        public virtual IDbSet<CodigoCredenciado> CodigosCredenciados { get; set; }
        public virtual IDbSet<TabelaConvenioCodigo> TabelaConvenioCodigos { get; set; }
        public virtual IDbSet<ConvenioURLServico> ConveniosURLServicos { get; set; }

        public virtual IDbSet<VWRptSaldoProduto> VWRptSaldosProdutos { get; set; }
        public virtual IDbSet<VWRptEstoqueMovimentoDetalhado> VWRptEstoqueMovimentosDetalhados { get; set; }
        public virtual IDbSet<VWRptEstoqueMovimentoResumido> VWRptEstoqueMovimentosResumidos { get; set; }
        public virtual IDbSet<VWRptAtendimentoDetalhado> VWRptAtendimentosDetalhados { get; set; }
        public virtual IDbSet<VWRptAtendimentoResumido> VWRptAtendimentosResumidos { get; set; }
        public virtual IDbSet<VWRptContaPagarDetalhado> VWRptContasPagarDetalhados { get; set; }



        #region Desenvolvimento e Documentacao

        public virtual IDbSet<DocItem> DocItens { get; set; }
        public virtual IDbSet<DocRotulo> DocRotulos { get; set; }
        public virtual IDbSet<Projeto> Projetos { get; set; }
        public virtual IDbSet<Tarefa> Tarefas { get; set; }
        public virtual IDbSet<TarefaIntervalo> TarefaIntervalos { get; set; }
        public virtual IDbSet<Comentario> Comentarios { get; set; }

        #endregion desenvolvimento e documentacao.

        #region Assistencial
        public virtual IDbSet<SolicitacaoExame> SolicitacoesExames { get; set; }
        public virtual IDbSet<SolicitacaoExameItem> SolicitacoesExamesItens { get; set; }
        public virtual IDbSet<PrescricaoMedica> PrescricoesMedicas { get; set; }
        public virtual IDbSet<SolicitacaoExamePrioridade> SolicitacaoExamePrioridades { get; set; }
        public virtual IDbSet<TipoResposta> TiposRespostas { get; set; }
        public virtual IDbSet<TipoRespostaConfiguracao> TipoRespostaConfiguracoes { get; set; }
        public virtual IDbSet<TipoRespostaTipoRespostaConfiguracao> TiposRespostasTipoRespostaConfiguracoes { get; set; }
        public virtual IDbSet<ElementoHtml> ElementosHtml { get; set; }
        public virtual IDbSet<ElementoHtmlTipo> ElementosHtmlTipos { get; set; }
        public virtual IDbSet<Divisao> Divisoes { get; set; }
        public virtual IDbSet<DivisaoTipoResposta> DivisaoTiposRespostas { get; set; }
        public virtual IDbSet<Frequencia> Frequencias { get; set; }
        public virtual IDbSet<TipoRespostaConfiguracaoElementoHtml> TipoRespostaConfiguracoesElementosHtml { get; set; }
        public virtual IDbSet<PrescricaoItemResposta> PrescricoesItensRespostas { get; set; }
        public virtual IDbSet<FormaAplicacao> FormasAplicacao { get; set; }
        public virtual IDbSet<TipoPrescricao> TiposPrescricoes { get; set; }
        public virtual IDbSet<PrescricaoItem> ItensPrescricoes { get; set; }
        public virtual IDbSet<TipoControle> TiposControles { get; set; }
        public virtual IDbSet<FormulaEstoque> FormulasEstoques { get; set; }
        public virtual IDbSet<FormulaFaturamento> FormulasFaturamentos { get; set; }
        public virtual IDbSet<Modulo> Modulos { get; set; }
        public virtual IDbSet<Operacao> Operacoes { get; set; }
        public virtual IDbSet<Prontuario> Prontuarios { get; set; }
        public virtual IDbSet<PrescricaoItemHora> PrescricoesItensHoras { get; set; }
        public virtual IDbSet<PrescricaoStatus> PrescricoesStatus { get; set; }
        public virtual IDbSet<PrescricaoItemStatus> PrescricoesItensStatus { get; set; }
        public virtual IDbSet<StatusSolicitacaoExameItem> StatusSolicitacoesExameItens { get; set; }
        public virtual IDbSet<TevMovimento> TevMovimentos { get; set; }
        public virtual IDbSet<TevRisco> TevRiscos { get; set; }
        public virtual IDbSet<FormulaEstoqueKit> FormulasEstoquesKits { get; set; }

        public virtual IDbSet<AtendimentoMovimento> AtendimentoMovimento { get; set; }

        public virtual IDbSet<BalancoHidrico> BalancoHidrico { get; set; }

        public virtual IDbSet<BalancoHidricoItem> BalancoHidricoItem { get; set; }

        public virtual IDbSet<BalancoHidricoSolucoes> BalancoHidricoSolucoes { get; set; }

        public virtual IDbSet<BalancoHidricoEndovenoso> BalancoHidricoEndovenosos { get; set; }

        public virtual IDbSet<BalancoHidricoSinaisVitais> BalancoHidricoSinaisVitais { get; set; }
        public virtual IDbSet<ModeloPrescricao> ModelosPrescricoes { get; set; }


        public virtual IDbSet<SolicitacaoAntimicrobiano> SolicitacaoAntimicrobianos { get; set; }
        public virtual IDbSet<SolicitacaoAntimicrobianosIndicacao> SolicitacaoAntimicrobianosIndicacoes { get; set; }

        public virtual IDbSet<SolicitacaoAntimicrobianosResultados> SolicitacaoAntimicrobianosResultados { get; set; }
        public virtual IDbSet<TipoSolicitacaoAntimicrobianosResultado> TipoSolicitacaoAntimicrobianosResultados { get; set; }

        public virtual IDbSet<TipoSolicitacaoAntimicrobianosIndicacao> TipoSolicitacaoAntimicrobianosIndicacoes { get; set; }

        public virtual IDbSet<SolicitacaoAntimicrobianosCulturas> SolicitacaoAntimicrobianosCulturas { get; set; }

        public virtual IDbSet<TipoSolicitacaoAntimicrobianosCultura> TipoSolicitacaoAntimicrobianosCulturas { get; set; }

        public virtual IDbSet<ConfiguracaoPrescricaoItemCampo> ConfiguracaoPrescricaoItemCampos { get; set; }

        public virtual IDbSet<ConfiguracaoPrescricaoItem> ConfiguracaoPrescricaoItems { get; set; }

        public virtual IDbSet<ReceituarioMedico> ReceituarioMedico { get; set; }



        #endregion

        #region Laboratório 
        //Gustavo 28.03.2018
        //public virtual IDbSet<ColetaExame> ColetaExames { get; set; }
        //public virtual IDbSet<ColetaExameInformacao> ColetaExameInformacoes { get; set; }
        //public virtual IDbSet<Formatacao> Formatacoes { get; set; }
        //public virtual IDbSet<FormatacaoInformacao> FormatacaoInformacaos { get; set; }
        //public virtual IDbSet<Informacao> Informacaos { get; set; }
        public virtual IDbSet<Resultado> Resultados { get; set; }
        public virtual IDbSet<ResultadoExame> ResultadosExames { get; set; }
        public virtual IDbSet<ResultadoLaudo> ResultadosLaudos { get; set; }
        public virtual IDbSet<ResultadoStatus> ResultadosStatus { get; set; }
        public virtual IDbSet<FormataItem> FormataItems { get; set; }
        public virtual IDbSet<ItemResultado> ItemResultados { get; set; }
        public virtual IDbSet<TenantLogo> TenantsLogos { get; set; }
        public virtual IDbSet<TenantImportConfig> TenantImportConfigs { get; set; }
        public virtual IDbSet<TabelaResultado> TabelasResultados { get; set; }
        //public virtual IDbSet<TabelaResultadoItem> TabelaResultadoItens { get; set; }
        public virtual IDbSet<TipoResultado> TiposResultados { get; set; }
        public virtual IDbSet<Material> Materiais { get; set; }
        public virtual IDbSet<KitExame> KitsExames { get; set; }
        public virtual IDbSet<KitExameItem> KitExameItens { get; set; }
        public virtual IDbSet<Equipamento> Equipamentos { get; set; }
        public virtual IDbSet<Formata> Formatas { get; set; }
        public virtual IDbSet<Metodo> Metodos { get; set; }
        public virtual IDbSet<Tecnico> Tecnicos { get; set; }
        public virtual IDbSet<LaboratorioUnidade> LaboratorioUnidades { get; set; }
        public virtual IDbSet<Setor> Setores { get; set; }
        public virtual IDbSet<Mapa> Mapas { get; set; }
        public virtual IDbSet<Exame> Exames { get; set; }
        public virtual IDbSet<SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios.Tabela> LabTabelas { get; set; }
        public virtual IDbSet<ExameStatus> ExamesStatus { get; set; }
        #endregion

        #region Imagens/Laudos
        public virtual IDbSet<LaudoMovimento> LaudoMovimento { get; set; }
        public virtual IDbSet<LaudoMovimentoItem> LaudoMovimentoItem { get; set; }
        public virtual IDbSet<Modalidade> Modalidade { get; set; }
        public virtual IDbSet<LaudoMovimentoStatus> LaudoMovimentoItemStatus { get; set; }
        public virtual IDbSet<LaudoGrupo> LaudoGrupos { get; set; }
        public virtual IDbSet<LaudoModeloLaudo> LaudoModeloLaudos { get; set; }
        #endregion

        #region Central de Autorizações

        public virtual IDbSet<AutorizacaoProcedimento> AutorizacaoProcedimentos { get; set; }
        public virtual IDbSet<AutorizacaoProcedimentoItem> AutorizacaoProcedimentosItens { get; set; }
        public virtual IDbSet<StatusSolicitacaoProcedimento> StatusSolicitacoesProcedimentos { get; set; }
        public virtual IDbSet<ComentarioAutorizacaoProcedimento> ComentariosAutorizacoesProcedimentos { get; set; }
        public virtual IDbSet<FormaAutorizacao> FormasAutorizacoes { get; set; }

        #endregion

        #region Financeiro

        public virtual IDbSet<FormaPagamento> FormasPagamentos { get; set; }
        public virtual IDbSet<GrupoDRE> GruposDREs { get; set; }
        public virtual IDbSet<GrupoContaAdministrativa> GruposContasAdministrativas { get; set; }
        public virtual IDbSet<SubGrupoContaAdministrativa> SubGruposContasAdministrativas { get; set; }
        public virtual IDbSet<SituacaoLancamento> SituacoesLancamentos { get; set; }
        public virtual IDbSet<RateioCentroCusto> RateiosCentroCustos { get; set; }
        public virtual IDbSet<RateioCentroCustoItem> RateiosCentroCustoItens { get; set; }
        public virtual IDbSet<ContaAdministrativa> ContasAdministrativas { get; set; }
        public virtual IDbSet<ContaAdministrativaCentroCusto> ContasAdministrativasCentroCustos { get; set; }
        public virtual IDbSet<ContaAdministrativaEmpresa> ContasAdministrativasEmpresas { get; set; }
        public virtual IDbSet<MeioPagamento> MeiosPagamentos { get; set; }
        public virtual IDbSet<TipoMeioPagamento> TiposMeioPagamentos { get; set; }
        public virtual IDbSet<TipoDocumento> TiposDocumentos { get; set; }
        public virtual IDbSet<Documento> Documentos { get; set; }
        public virtual IDbSet<Lancamento> Lancamentos { get; set; }
        public virtual IDbSet<DocumentoRateio> DocumentosRateios { get; set; }
        public virtual IDbSet<Banco> Bancos { get; set; }
        public virtual IDbSet<Agencia> Agencias { get; set; }
        public virtual IDbSet<ContaCorrente> ContasCorrentes { get; set; }
        public virtual IDbSet<TipoContaCorrente> TiposContaCorrente { get; set; }
        public virtual IDbSet<Quitacao> Quitacoes { get; set; }
        public virtual IDbSet<LancamentoQuitacao> LancamentosQuitacoes { get; set; }
        public virtual IDbSet<TalaoCheque> TaloesCheques { get; set; }
        public virtual IDbSet<Cheque> Cheques { get; set; }






        #endregion

        #region Compras
        public virtual IDbSet<SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras.CmpOrdemCompra> CmpOrdemCompra { get; set; }
        public virtual IDbSet<OrdemCompraItem> OrdemCompraItem { get; set; }
        public virtual IDbSet<OrdemCompraStatus> OrdemCompraStatus { get; set; }
        public virtual IDbSet<CompraRequisicao> CompraRequisicao { get; set; }
        public virtual IDbSet<CompraRequisicaoItem> CompraRequisicaoItem { get; set; }
        public virtual IDbSet<CompraRequisicaoTipo> CompraTipoRequisicao { get; set; }
        public virtual IDbSet<CompraRequisicaoModo> CompraRequisicaoModo { get; set; }
        public virtual IDbSet<CompraMotivoPedido> CompraMotivoPedido { get; set; }
        public virtual IDbSet<CompraCotacao> CompraCotacao { get; set; }
        public virtual IDbSet<CompraCotacaoItem> CompraCotacaoItem { get; set; }
        public virtual IDbSet<CompraCotacaoStatus> CompraCotacaoStatus { get; set; }
        public virtual IDbSet<CompraAprovacaoStatus> CompraAprovacaoStatus { get; set; }

        #endregion

        #region Extras

        public virtual IDbSet<Erro> SisErros { get; set; }

        #endregion extras.

        #region VISUALASA
        public virtual IDbSet<Sis_Atendimento> Sis_AtendimentosVisualASA { get; set; }
        public virtual IDbSet<Sis_Ambulatorio> Sis_AmbulatoriosVisualASA { get; set; }
        public virtual IDbSet<Sis_ContaMedica> Sis_ContaMedicasVisualASA { get; set; }
        public virtual IDbSet<Sis_Internacao> Sis_InternacoesVisualASA { get; set; }
        public virtual IDbSet<Sis_Pessoa> Sis_PessoasVisualASA { get; set; }
        public virtual IDbSet<Sis_Paciente> Sis_PacientesVisualASA { get; set; }
        public virtual IDbSet<Pro_ReqExameMov> Pro_ReqExamesMovVisualAsa { get; set; }
        public virtual IDbSet<Pro_ReqExameMovItem> Pro_ReqExamesMovItensVisualASA { get; set; }


        #endregion

        #region Painel de Senha

        public virtual IDbSet<Painel> Paineis { get; set; }
        public virtual IDbSet<TipoLocalChamada> TiposLocaisChamadas { get; set; }
        public virtual IDbSet<PainelTipoLocalChamada> PaineisTipoLocaisChamadas { get; set; }
        public virtual IDbSet<Fila> Filas { get; set; }
        public virtual IDbSet<LocalChamada> LocaisChamadas { get; set; }
        public virtual IDbSet<LocalChamadaFila> LocaisChamadasFilas { get; set; }
        public virtual IDbSet<Senha> Senhas { get; set; }
        public virtual IDbSet<SenhaMovimentacao> SenhasMovimentacoes { get; set; }
        public virtual IDbSet<SenhaMovimentacaoPainel> SenhasMovimentacoesPaineis { get; set; }

        #endregion

        #region Registro de arquivo

        public virtual IDbSet<RegistroArquivo> RegistrosArquivos { get; set; }
        public virtual IDbSet<RegistroTabela> RegistrosTabelas { get; set; }

        #endregion

        #region Sistemas

        public virtual IDbSet<Parametro> Parametros { get; set; }
        
        public virtual IDbSet<AgGridUserPreference> AgGridUserPreferences { get; set; }

        /// <summary>
        /// Gets or sets the impressora arquivos.
        /// </summary>
        public virtual IDbSet<ImpressoraArquivo> ImpressoraArquivos { get; set; }
        
        public virtual IDbSet<DisparoDeMensagem> DisparoDeMensagem { get; set; }

        public virtual IDbSet<DisparoDeMensagemItem> DisparoDeMensagemItem { get; set; }

        public virtual IDbSet<DisparoDeMensagemItemTipo> DisparoDeMensagemItemTipo { get; set; }
        
        public virtual IDbSet<ParametrizacaoIp> ParametrizacaoIp { get; set; }

        public virtual IDbSet<Parametrizacao> Parametrizacao { get; set; }
        
        public virtual IDbSet<Aviso> Avisos { get; set; }
        public virtual IDbSet<AvisoGrupo> AvisoGrupos { get; set; }
        public  virtual IDbSet<AppUserNotificationMessage> AppUserNotificationMessage { get; set; }

        #endregion
        
        #region Modelo de Texto

        /// <summary>
        /// Gets or sets the tipo modelo.
        /// </summary>
        public IDbSet<TipoModelo> TipoModelo { get; set; }

        /// <summary>
        /// Gets or sets the tipo modelo variaveis.
        /// </summary>
        public IDbSet<TipoModeloVariaveis> TipoModeloVariaveis { get; set; }

        /// <summary>
        /// Gets or sets the tamanho modelo.
        /// </summary>
        public IDbSet<TamanhoModelo> TamanhoModelo { get; set; }
        #endregion
        
        #region Sefaz

        public virtual IDbSet<AbpSefazTecnoSpeedNotas> SefazTecnoSpeedNotas { get; set; }
        public virtual IDbSet<AbpSefazTecnoSpeedConfiguracoes> SefazTecnoSpeedConfiguracoes { get; set; }
        #endregion

        /* Setting "Default" to base class helps us when working migration commands on Package Manager Console.
         * But it may cause problems when working Migrate.exe of EF. ABP works either way.         * 
         */
        public SWMANAGERDbContext() : base("Default")
        {
            // 26/09/2017 - reativando o lazy loading e removendo o virtual das classes de navegação
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        /* This constructor is used by ABP to pass connection string defined in SWMANAGERDataModule.PreInitialize.
         * Notice that, actually you will not directly create an instance of SWMANAGERDbContext since ABP automatically handles it.
         */
        public SWMANAGERDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        /* This constructor is used in tests to pass a fake/mock connection.
         */
        public SWMANAGERDbContext(DbConnection dbConnection)
            : base(dbConnection, true)
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }


        //public override int SaveChanges()
        //{
        //    try
        //    {
        //        return base.SaveChanges();
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw new Exception("Erro SaveChange", e);
        //    }
        //}


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstoquePreMovimentoItem>()
             .Property(p => p.CustoUnitario)
             .HasPrecision(18, 10);

            modelBuilder.Entity<EstoqueMovimentoItem>()
              .Property(p => p.CustoUnitario)
              .HasPrecision(18, 10);

            modelBuilder.Entity<EstoquePreMovimentoItem>()
             .Property(p => p.ValorIPI)
             .HasPrecision(18, 10);

            modelBuilder.Entity<EstoqueMovimentoItem>()
              .Property(p => p.ValorIPI)
              .HasPrecision(18, 10);

            modelBuilder.Entity<DisparoDeMensagem>()
                .Property(p => p.DataProgramada)
                .HasColumnType("datetime2");

            modelBuilder.Entity<DisparoDeMensagem>()
                .Property(p => p.DataFinalDisparo)
                .HasColumnType("datetime2")
                .IsOptional();

            modelBuilder.Entity<DisparoDeMensagem>()
                .Property(p => p.DataInicioDisparo)
                .HasColumnType("datetime2")
                .IsOptional();

            modelBuilder.Entity<DisparoDeMensagemItem>()
                .Property(p => p.DataProgramada)
                .HasColumnType("datetime2");

            modelBuilder.Entity<DisparoDeMensagemItem>()
                .Property(p => p.DataFinalDisparo)
                .HasColumnType("datetime2")
                .IsOptional();

            modelBuilder.Entity<DisparoDeMensagemItem>()
               .Property(p => p.DataInicioDisparo)
               .HasColumnType("datetime2")
               .IsOptional();

            modelBuilder.Entity<DisparoDeMensagemItem>()
               .Property(p => p.DataRecebimento)
               .HasColumnType("datetime2")
               .IsOptional();


            modelBuilder.Entity<FaturamentoContaItem>()
                .Property(c => c.ResumoDetalhamentoJSON)
                .HasColumnName("ResumoDetalhamento");

            modelBuilder.Entity<OrdemCompraItem>()
             .Property(p => p.ValorUnitario)
             .HasPrecision(18, 5);

            modelBuilder.Entity<OrdemCompraItem>()
             .Property(p => p.ValorTotal)
             .HasPrecision(18, 5);

            base.OnModelCreating(modelBuilder);

            ////ignora a criação de tabelas com nomes de views
            //modelBuilder.Ignore<VWConsultaFaturamentoAberto>();
            //modelBuilder.Ignore<VWConsultaFaturamentoEntrega>();
            //modelBuilder.Ignore<VWConsultaFaturamentoRecebimento>();
            //modelBuilder.Ignore<VWFaturamentoAberto>();
            //modelBuilder.Ignore<VWFaturamentoAbertoSeisMeses>();
            //modelBuilder.Ignore<VWTeste>();

            //modelBuilder.Entity<Produto>()
            //       .HasOptional(i => i.ProdutoPrincipal)
            //       .WithMany()
            //       .HasForeignKey(i => i.ProdutoPrincipalId);
        }
    }

    //    public class ReadOnlyContext : DbContext
    //    {
    //        public ReadOnlyContext()
    //            : base("name=Default")
    //        {
    //            Database.SetInitializer<ReadOnlyContext>(null);
    //        }

    //        public ReadOnlyContext(string nameOrConnectionString)
    //            : base(nameOrConnectionString)
    //        {
    //            //this.Configuration.LazyLoadingEnabled = false;
    //        }

    //        /* This constructor is used in tests to pass a fake/mock connection.
    //         */
    //        public ReadOnlyContext(DbConnection dbConnection)
    //            : base(dbConnection, true)
    //        {
    //            //this.Configuration.LazyLoadingEnabled = false;
    //        }


    //        //Dashboard
    //        public virtual IDbSet<VWTeste> VWTAn error occurred while updating the entries. See the inner exception for details.stes { get; set; }

    //        public virtual IDbSet<VWConsultaFaturamentoAberto> VWConsultaFaturamentoAberto { get; set; }

    //        public virtual IDbSet<VWConsultaFaturamentoEntrega> VWConsultaFaturamentoEntrega { get; set; }

    //        public virtual IDbSet<VWConsultaFaturamentoRecebimento> VWConsultaFaturamentoRecebimento { get; set; }
    ////        public DbSet<UserView> UsersView { get; set; }
    //    }
}
