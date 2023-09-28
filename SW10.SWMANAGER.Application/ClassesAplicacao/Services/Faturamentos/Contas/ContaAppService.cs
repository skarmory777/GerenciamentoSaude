#region Usings
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ServicosValidacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.VisualASA;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Uow;
using Dapper;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using SW10.SWMANAGER.Helpers;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Dtos;
using Castle.Core.Internal;
using MoreLinq;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Pacote;
using SW10.SWMANAGER.CorreiosService;
using Exception = System.Exception;
using System.Text;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;

#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas
{
    public class ContaAppService : SWMANAGERAppServiceBase, IContaAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoConta, long> _contaRepository;
        private readonly IRepository<FaturamentoContaItem, long> _contaItemRepository;
        private readonly IListarContasExcelExporter _listarContasExcelExporter;
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IFaturamentoContaItemAppService _itemAppService;
        private readonly IFaturamentoConfigConvenioAppService _configConvenioAppService;
        private readonly IFaturamentoTabelaAppService _tabelaAppService;
        private readonly IFaturamentoItemTabelaAppService _tabelaItemAppService;
        private readonly ISisMoedaAppService _moedaAppService;
        private readonly ISisMoedaCotacaoAppService _cotacaoAppService;
        private readonly IFaturamentoItemAppService _fatItemAppService;
        private readonly IEmpresaAppService _empresaAppService;
        private readonly IConvenioAppService _convenioAppService;
        private readonly IMedicoAppService _medicoAppService;
        private readonly IPlanoAppService _planoAppService;
        private readonly IAtendimentoAppService _atendimentoAppService;
        private readonly IGuiaAppService _guiaAppService;
        private readonly IVisualAppService _visualAppService;
        private readonly IRepository<FaturamentoItemTabela, long> _faturamentoItemTabelaRepository;
        private readonly IRepository<Atendimento, long> _atendimentoRepository;
        private readonly IRepository<FaturamentoPacote, long> _faturamentoPacoteRepository;

        public ContaAppService(
            IRepository<FaturamentoConta, long> contaRepository,
            IRepository<FaturamentoContaItem, long> contaItemRepository,
            IListarContasExcelExporter listarContasExcelExporter,
            IPacienteAppService pacienteAppService,
            IFaturamentoContaItemAppService itemAppService,
            IFaturamentoConfigConvenioAppService configConvenioAppService,
            IFaturamentoTabelaAppService tabelaAppService,
            IFaturamentoItemTabelaAppService tabelaItemAppService,
            ISisMoedaAppService moedaAppService,
            ISisMoedaCotacaoAppService cotacaoAppService,
            IFaturamentoItemAppService fatItemAppService,
            IEmpresaAppService empresaAppService,
            IConvenioAppService convenioAppService,
            IMedicoAppService medicoAppService,
            IPlanoAppService planoAppService,
            IAtendimentoAppService atendimentoAppService,
            IGuiaAppService guiaAppService,
            IVisualAppService visualAppService,
            IRepository<FaturamentoItemTabela, long> faturamentoItemTabela,
            IRepository<Atendimento, long> atendimentoRepository,
            IRepository<FaturamentoPacote, long> faturamentoPacoteRepository
            )
        {
            _contaRepository = contaRepository;
            _contaItemRepository = contaItemRepository;
            _listarContasExcelExporter = listarContasExcelExporter;
            _pacienteAppService = pacienteAppService;
            _itemAppService = itemAppService;
            _configConvenioAppService = configConvenioAppService;
            _tabelaAppService = tabelaAppService;
            _tabelaItemAppService = tabelaItemAppService;
            _moedaAppService = moedaAppService;
            _cotacaoAppService = cotacaoAppService;
            _fatItemAppService = fatItemAppService;
            _empresaAppService = empresaAppService;
            _convenioAppService = convenioAppService;
            _medicoAppService = medicoAppService;
            _planoAppService = planoAppService;
            _atendimentoAppService = atendimentoAppService;
            _guiaAppService = guiaAppService;
            _visualAppService = visualAppService;
            _faturamentoItemTabelaRepository = faturamentoItemTabela;
            _atendimentoRepository = atendimentoRepository;
            _faturamentoPacoteRepository = faturamentoPacoteRepository;
        }
        #endregion cabecalho.


        public async Task<PagedResultDto<FaturamentoContaLoteDto>> ListarContasParaLotes(FaturamentoContaLoteFilterDto input)
        {
            const string DefaultField = "FatConta.Id";

            const string SelectClause = @"
                FatConta.Id,
                AteAtendimento.Id as AtendimentoId,
                FatConta.Codigo,
                FatConta.DataInicio AS DataInicial,
                FatConta.DataFim AS DataFinal,
                AteAtendimento.DataRegistro,
                AteAtendimento.DataAlta,
                AteAtendimento.Matricula,
                AteAtendimento.NumeroGuia,
                SisPessoa.NomeCompleto AS PacienteNomeCompleto,
                SisPessoaMedico.NomeCompleto AS MedicoNomeCompleto,
                SisConvenio.Id AS ConvenioId,
                SisConvenio.NomeFantasia AS ConvenioNomeFantasia,
                SisPlano.Id AS PlanoId,
                SisPlano.Descricao AS PlanoDescricao,
                AteAtendimento.IsAmbulatorioEmergencia,
                FatContaStatus.Descricao AS FatContaStatusDescricao,
                ValorConta.valor AS ValorConta";

            const string FromClause = @"FatConta
                INNER JOIN AteAtendimento (NOLOCK) ON AteAtendimento.Id = FatConta.SisAtendimentoId AND AteAtendimento.IsDeleted = @isDeleted
                INNER JOIN SisPaciente (NOLOCK) ON SisPaciente.Id =FatConta.SisPacienteId AND SisPaciente.IsDeleted = @isDeleted
                INNER JOIN SisPessoa (NOLOCK) ON SisPessoa.Id = SisPaciente.SisPessoaId AND SisPessoa.IsDeleted = @isDeleted
                INNER JOIN FatContaStatus (NOLOCK) ON FatContaStatus.Id = FatConta.FatContaStatusId AND FatContaStatus.IsDeleted = @isDeleted
                LEFT JOIN (SELECT FatContaId, sum(valorAprovado) AS valor FROM FatContaItem (NOLOCK) WHERE IsDeleted = @isDeleted GROUP BY FatContaId) AS ValorConta ON FatConta.Id =  ValorConta.FatContaId
                LEFT JOIN SisConvenio (NOLOCK) ON SisConvenio.Id = AteAtendimento.SisConveniolId AND SisConvenio.IsDeleted = @isDeleted
                LEFT JOIN SisPlano (NOLOCK) ON SisPlano.Id = AteAtendimento.SisPlanoId AND SisPlano.IsDeleted = @isDeleted
                LEFT JOIN SisProfissionalSaude (NOLOCK) ON SisProfissionalSaude.Id = AteAtendimento.SisMedicoId AND SisProfissionalSaude.IsDeleted = @isDeleted
                LEFT JOIN SisPessoa AS SisPessoaMedico (NOLOCK) ON SisPessoaMedico.Id = SisProfissionalSaude.SisPessoaId AND SisPessoaMedico.IsDeleted = @isDeleted
                LEFT JOIN SisGuia (NOLOCK) ON SisGuia.Id = AteAtendimento.SisGuiaId AND SisGuia.IsDeleted = @isDeleted";

            const string WhereClause = @"FatConta.IsAtivo = @isAtivo AND FatConta.IsDeleted = @isDeleted AND FatContaStatus.Id = @FatContaStatusId";

            return await this.CreateDataTable<FaturamentoContaLoteDto, FaturamentoContaLoteFilterDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .EnablePagination(true)
                .AddWhereMethod((inputWhere, dapperParameters) =>
                {
                    var whereBuilder = new StringBuilder();

                    whereBuilder.WhereIf(input.ConvenioId.HasValue, " AND SisConvenio.Id = @ConvenioId ");
                    whereBuilder.WhereIf(input.EmpresaId.HasValue, " AND AteAtendimento.SisEmpresaId = @EmpresaId ");
                    whereBuilder.WhereIf(input.TipoInternacao.HasValue, " AND AteAtendimento.IsAmbulatorioEmergencia = @TipoInternacao ");

                    whereBuilder.WhereIf(input.StartDate.HasValue && input.EndDate.HasValue, " AND FatConta.DataInicio BETWEEN @StartDate AND @EndDate ");


                    dapperParameters.Add("isDeleted", false);
                    dapperParameters.Add("isAtivo", true);
                    return whereBuilder.ToString();
                })
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }


        public async Task<FaturamentoContaDto> CriarOuEditar(FaturamentoContaDto input)
        {
            try
            {
                using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var contaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                using (var unitOfWork = UnitOfWorkManager.Begin())
                {
                    var conta = input.MapearParaBanco();

                    if (!conta.AtendimentoId.HasValue)
                    {
                        throw new UserFriendlyException("É preciso ter um atendiento vinculado.");
                    }

                    if (input.Id.Equals(0))
                    {
                        conta.StatusId ??= FaturamentoContaStatus.Inicial;
                        conta.IsAtivo = true;
                        conta.Versao = 0;

                        var atendimento = await atendimentoAppService.Object.Obter((long)conta.AtendimentoId);

                        if (atendimento != null)
                        {
                            conta.EmpresaId = atendimento.EmpresaId;
                            conta.PacienteId = atendimento.PacienteId;
                            conta.NumeroGuia = atendimento.NumeroGuia;
                        }

                        input.Id = await contaRepository.Object.InsertAndGetIdAsync(conta);
                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            OcorrenciaTexto.ContaMedicaCriada(input.Codigo, atendimento?.Codigo, (await this.GetCurrentUserAsync()).FullName),
                            TipoOcorrencia.ContaMedica, null, typeof(FaturamentoConta).FullName, conta.Id,
                            typeof(Atendimento).FullName, atendimento.Id));
                    }
                    else
                    {
                        var atendimento = await atendimentoAppService.Object.Obter((long)conta.AtendimentoId);
                        var ori = await contaRepository.Object.GetAsync(conta.Id);
                        ori.CodDependente = conta.CodDependente;
                        ori.Codigo = conta.Codigo;
                        //ori.ConvenioId = conta.ConvenioId;
                        ori.DataAutorizacao = conta.DataAutorizacao;
                        ori.DataConferencia = conta.DataConferencia;
                        ori.DataEntrBolAnest = conta.DataEntrBolAnest;
                        ori.DataEntrCDFilme = conta.DataEntrCDFilme;
                        ori.DataEntrDescCir = conta.DataEntrDescCir;
                        ori.DataEntrFolhaSala = conta.DataEntrFolhaSala;
                        ori.DataFim = conta.DataFim;
                        ori.DataInicio = conta.DataInicio;
                        ori.DataPagamento = conta.DataPagamento;
                        ori.DataValidadeSenha = conta.DataValidadeSenha;
                        ori.Descricao = conta.Descricao;
                        ori.DiaSerie1 = conta.DiaSerie1;
                        ori.DiaSerie10 = conta.DiaSerie10;
                        ori.DiaSerie2 = conta.DiaSerie2;
                        ori.DiaSerie3 = conta.DiaSerie3;
                        ori.DiaSerie4 = conta.DiaSerie4;
                        ori.DiaSerie5 = conta.DiaSerie5;
                        ori.DiaSerie6 = conta.DiaSerie6;
                        ori.DiaSerie7 = conta.DiaSerie7;
                        ori.DiaSerie8 = conta.DiaSerie8;
                        ori.DiaSerie9 = conta.DiaSerie9;
                        ori.EmpresaId = conta.EmpresaId;
                        ori.FatGuiaId = conta.FatGuiaId;
                        ori.GuiaId = conta.GuiaId;
                        ori.GuiaOperadora = conta.GuiaOperadora;
                        ori.GuiaPrincipal = conta.GuiaPrincipal;
                        ori.IdentAcompanhante = conta.IdentAcompanhante;
                        ori.IsAutorizador = conta.IsAutorizador;
                        ori.IsSistema = conta.IsSistema;
                        ori.Matricula = conta.Matricula;
                        ori.MedicoId = conta.MedicoId;
                        ori.MotivoPendencia = conta.MotivoPendencia;
                        ori.NumeroGuia = conta.NumeroGuia;
                        ori.Observacao = conta.Observacao;
                        //ori.PacienteId = conta.PacienteId;
                        ori.PlanoId = conta.PlanoId;
                        ori.SenhaAutorizacao = conta.SenhaAutorizacao;
                        //ori.StatusId = conta.StatusId;
                        ori.TipoAcomodacaoId = conta.TipoAcomodacaoId;
                        ori.UnidadeOrganizacionalId = conta.UnidadeOrganizacionalId;
                        ori.UsuarioConferenciaId = conta.UsuarioConferenciaId;
                        ori.ValidadeCarteira = conta.ValidadeCarteira;

                        await contaRepository.Object.UpdateAsync(ori);

                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            OcorrenciaTexto.ContaMedicaCriada(input.Codigo, atendimento?.Codigo, (await this.GetCurrentUserAsync()).FullName),
                            TipoOcorrencia.ContaMedica, null, typeof(FaturamentoConta).FullName, conta.Id,
                            typeof(Atendimento).FullName, ori.AtendimentoId));
                    }

                    UnitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();

                    return input;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoContaItemTableDto>> ListarItems(FaturamentoContaItemTableFilterDto input)
        {
            const string DefaultField = "FatContaItem.Id";

            const string SelectClause = @"
                FatContaItem.Id,
                FatContaItem.FatContaId,
                FatContaStatus.Descricao AS ItemStatus,
                FatContaStatus.Cor AS ItemStatusCor,
                FatContaItem.Data,
                FatItem.Descricao AS ItemDescricao,
                FatGrupo.Id         AS GrupoId,
                FatGrupo.Descricao AS grupoDescricao,
                FatGrupo.Codigo AS grupoCodigo,
                FatTipoGrupo.Id AS TipoGrupoId,
                FatTipoGrupo.Descricao AS tipoGrupoDescricao,
                FatKit.Id AS FaturamentokitId,
                FatContaKit.Id AS FaturamentoContakitId,
                CONCAT(FatContaKit.Id,' - ',FatKit.Descricao) AS kitDescricao,
                SisTurno.Descricao AS turnoDescricao,
                SisTurno.Codigo AS turnoCodigo,
                CentroCusto.Descricao AS centroCustoDescricao,
                CentroCusto.CodigoCentroCusto AS centroCustoCodigo,
                SisUnidadeOrganizacional.Id AS UnidadeOrganizacionalId,
                SisUnidadeOrganizacional.Descricao AS unidadeOrganizacionalDesricao,
                TerceirizadoPessoa.NomeCompleto AS terceirizadoDescricao,
                SisTerceirizado.Codigo AS terceirizadoCodigo,
                SisTipoAcomodacao.Id AS TipoAcomodacaoId,
                SisTipoAcomodacao.Descricao AS tipoAcomodacaoDescricao,
                FatContaItem.Qtde AS Qtde,
                FatPacote.Id AS FaturamentoPacoteId,
                FatItemPacote.Id AS FaturamentoPacoteItemId,
                FatItemPacote.Descricao AS FaturamentoPacoteItemDescricao,
                FatContaItem.ValorItem";

            const string FromClause = @"FatContaItem
                LEFT JOIN FatItem ON FatContaItem.FatItemId = FatItem.Id AND FatItem.IsDeleted = @isDeleted
                LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id AND FatGrupo.IsDeleted = @isDeleted
                LEFT JOIN FatTipoGrupo ON FatGrupo.TipoGrupoId = FatTipoGrupo.Id AND FatTipoGrupo.IsDeleted = @isDeleted
                LEFT JOIN FatContaStatus ON FatContaItem.FatContaStatusId = FatContaStatus.Id AND FatContaStatus.IsDeleted = @isDeleted
                LEFT JOIN FatContaKit ON FatContaKit.FatContaId = FatContaItem.FatContaId AND FatContaKit.Id = FatContaItem.FaturamentoContaKitId AND FatContaKit.IsDeleted = 0
                LEFT JOIN FatKit ON  FatContaKit.FatKitId = FatKit.Id AND FatKit.IsDeleted = @isDeleted
                LEFT JOIN SisTurno ON FatContaItem.TurnoId = SisTurno.Id AND SisTurno.IsDeleted = @isDeleted
                LEFT JOIN CentroCusto ON FatContaItem.CentroCustoId = CentroCusto.Id AND CentroCusto.IsDeleted = @isDeleted
                LEFT JOIN SisUnidadeOrganizacional ON FatContaItem.UnidadeOrganizacionalId = SisUnidadeOrganizacional.Id AND SisUnidadeOrganizacional.IsDeleted = @isDeleted
                LEFT JOIN SisTerceirizado ON FatContaItem.TerceirizadoId = SisTerceirizado.Id AND SisTerceirizado.IsDeleted = @isDeleted
                LEFT JOIN SisPessoa TerceirizadoPessoa ON SisTerceirizado.SisPessoaId = TerceirizadoPessoa.Id AND TerceirizadoPessoa.IsDeleted = @isDeleted
                LEFT JOIN SisTipoAcomodacao ON FatContaItem.TipoAcomodacaoId = SisTipoAcomodacao.Id AND SisTipoAcomodacao.IsDeleted = @isDeleted
                LEFT JOIN FatPacote  ON FatPacote.FaturamentoContaId = FatContaItem.FatContaId AND FatPacote.Id = FatContaItem.FatPacoteId AND FatPacote.IsDeleted = @isDeleted
                LEFT JOIN FatItem AS FatItemPacote ON FatPacote.FaturamentoItemId = FatItemPacote.Id AND FatItem.IsDeleted = @isDeleted";

            const string WhereClause = @"FatContaItem.FatContaId = @ContaMedicaId AND FatContaItem.IsDeleted = @isDeleted";

            return await this.CreateDataTable<FaturamentoContaItemTableDto, FaturamentoContaItemTableFilterDto>()
                .AddDefaultField(DefaultField)
                .AddSelectClause(SelectClause)
                .AddFromClause(FromClause)
                .AddWhereClause(WhereClause)
                .EnablePagination(input.EnablePaginate)
                .AddWhereMethod((inputWhere, daperParamters) =>
                {
                    var stringBuilder = new StringBuilder();

                    stringBuilder = stringBuilder.WhereIf(inputWhere.DataInicial.HasValue, " AND FatContaItem.DATA >= @dataInicial ");
                    stringBuilder = stringBuilder.WhereIf(inputWhere.DataFinal.HasValue, " AND FatContaItem.DATA <= @dataFinal ");

                    stringBuilder = stringBuilder.WhereIf(inputWhere.FatContaKitId.HasValue, "AND FatContaKit.Id =@fatContaKitId ");
                    stringBuilder = stringBuilder.WhereIf(inputWhere.FatKitId.HasValue, "AND FatKit.Id = @fatKitId ");


                    stringBuilder = stringBuilder.WhereIf(inputWhere.FatContaPacoteId.HasValue, "AND FatPacote.Id = @fatContaPacoteId ");
                    stringBuilder = stringBuilder.WhereIf(inputWhere.FatPacoteId.HasValue, "AND FatItemPacote.Id =@fatPacoteId ");


                    daperParamters.Add("isDeleted", false);
                    return stringBuilder.ToString();
                })
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }


        private static List<ResumoContaItemDto> AgruparItems(List<ResumoContaItemDto> items, Func<ResumoContaItemDto,Object> groupItems)
        {
            var aggroupedContaItem = items.GroupBy(groupItems);
            var contaItems = new List<ResumoContaItemDto>();

            foreach (var item in aggroupedContaItem.ToList())
            {
                var count = item.Count();
                if (count == 1)
                {
                    var firstItem = item.First();
                    if (firstItem.ResumoDetalhamento != null)
                    {
                        contaItems.Add(firstItem);
                        continue;
                    }
                    if(firstItem.ValorMoeda == 0)
                    {
                        firstItem.ValorMoeda = 1;
                    }
                    var resumoDetalhamento = new ResumoDetalhamento
                    {
                        ValorTaxas = (float)Math.Round(firstItem.ValorTaxas,2),
                        TaxasValor = (float)Math.Round(firstItem.ValorTaxas,2),
                        ValorPorte = (float)Math.Round(firstItem.ValorPorte,2),
                        COCH = firstItem.COCH,
                        HMCH = firstItem.HMCH,
                        ValorHMCH = (float) Math.Round(firstItem.ValorHMCH * firstItem.HMCH,2),
                        ValorCOCH = (float) Math.Round(firstItem.ValorCOCH * firstItem.COCH,2),
                        ValorFilme = (float)Math.Round(firstItem.ValorFilme * firstItem.MetragemFilme,2),
                        MetragemFilme = firstItem.MetragemFilme,
                        Percentual = firstItem.Percentual != 0 ? firstItem.Percentual / 100 : 0,
                        Valor = (float)Math.Round(firstItem.ValorItem * firstItem.ValorMoeda,2),
                        Preco = (float)Math.Round(firstItem.ValorItem,2),
                        Qtde = firstItem.Qtde,
                        Moeda = new ResumoDetalhamentoMoeda
                        {
                            Codigo = firstItem.CodigoMoeda
                        }
                    };
                    resumoDetalhamento.ValorTotal = (
                        resumoDetalhamento.Valor
                        + resumoDetalhamento.ValorTaxas
                        + resumoDetalhamento.ValorPorte
                        + resumoDetalhamento.ValorFilme
                        + resumoDetalhamento.ValorHMCH
                        + resumoDetalhamento.ValorCOCH
                        ) * firstItem.Qtde;
                    firstItem.ResumoDetalhamentoJSON = JsonConvert.SerializeObject(resumoDetalhamento);

                    contaItems.Add(firstItem);
                }
                else
                {
                    var qtde = item.Sum(x => x.Qtde);
                    var firstItem = item.First();
                    firstItem.Qtde = qtde;
                    if (firstItem.ValorMoeda == 0)
                    {
                        firstItem.ValorMoeda = 1;
                    }
                    var resumoDetalhamento = new ResumoDetalhamento
                    {
                        ValorTaxas = (float)Math.Round(item.Sum(x => x.ValorTaxas),2),
                        TaxasValor = (float)Math.Round(item.Sum(x => x.ValorTaxas),2),
                        ValorPorte = (float)Math.Round(item.Sum(x => x.ValorPorte),2),
                        ValorHMCH = (float)Math.Round(item.Sum(x => x.ValorHMCH * x.HMCH),2),
                        HMCH = item.Sum(x => x.HMCH),
                        ValorCOCH = (float)Math.Round(item.Sum(x => x.ValorCOCH * x.COCH),2),
                        COCH = item.Sum(x => x.COCH),
                        ValorFilme = (float)Math.Round(item.Sum(x => x.ValorFilme * x.MetragemFilme),2),
                        MetragemFilme = item.Sum(x => x.MetragemFilme),
                        Percentual = firstItem.Percentual != 0 ? firstItem.Percentual / 100 : 0,
                        Valor = (float)Math.Round(firstItem.ValorItem * firstItem.ValorMoeda, 2),
                        Preco = (float)Math.Round(firstItem.ValorItem,2),
                        Qtde = qtde,
                        Moeda = new ResumoDetalhamentoMoeda
                        {
                            Codigo = firstItem.CodigoMoeda
                        }
                    };
                    resumoDetalhamento.ValorTotal =
                        ((resumoDetalhamento.Preco * firstItem.Qtde)
                        + resumoDetalhamento.ValorTaxas
                        + resumoDetalhamento.ValorPorte
                        + resumoDetalhamento.ValorFilme
                        + resumoDetalhamento.ValorHMCH
                        + resumoDetalhamento.ValorCOCH)
                        * (float)(firstItem.Percentual != 0 ? firstItem.Percentual / 100 : 1);
                    firstItem.ResumoDetalhamentoJSON = JsonConvert.SerializeObject(resumoDetalhamento);
                    contaItems.Add(firstItem);
                }
            }

            return contaItems;
        }
        private async Task<List<ResumoContaItemDto>> BaseResumoConta(FaturamentoResumoContaFilterDto input)
        {
            
            const string queryContaItem = @"
                SELECT 
                    FatContaItem.Id AS Id,
                    FatContaItem.Data AS DataInicial,
                    FatItem.Id AS FatItemId,
                    FatItem.GrupoId AS GrupoId,
                    CASE WHEN AteAtendimento.IsAmbulatorioEmergencia = 1 THEN 
	                    FatGrupo.OrdemAmbulatorio 
                    ELSE FatGrupo.OrdemInternacao END AS GrupoOrdem,
                    FatGrupo.Descricao AS GrupoDescricao,
                    FatSubGrupo.Id AS SubGrupoId,
                    
                    FatSubGrupo.Descricao AS SubGrupoDescricao,
                    FatItem.Descricao AS FatItemDescricao,
                    FatItem.DescricaoTuss AS FatItemDescricaoTuss,
                    FatItem.Codigo AS Codigo,
                    FatItem.CodAmb AS CodAmb,
                    FatItem.CodCbhpm AS CodCbhpm,
                    FatItem.CodTuss AS CodTuss,
                    FatContaItem.Descricao AS FatContaItemDescricao,
                    FatContaItem.Codigo AS FatContaItemCodigo,
                    FatContaItem.Qtde,
                    FatContaItem.ValorItem,
                    FatCOntaItem.ValorAprovado,
                    FatContaItem.ValorMoedaAprovado AS ValorMoeda,
                    FatCOntaItem.ValorTaxas,
                    FatCOntaItem.IsValorItemManual,
                    FatCOntaItem.HMCH,
                    FatCOntaItem.ValorFilme,
                    FatCOntaItem.MetragemFilme,
                    FatCOntaItem.MetragemFilmeAprovada,
                    FatCOntaItem.ValorCOCH,
                    FatCOntaItem.COCH,
                    FatCOntaItem.COCHAprovado,
                    FatCOntaItem.Percentual,
                    FatCOntaItem.FaturamentoConfigConvenioId,
                    FatContaItem.ResumoDetalhamento AS ResumoDetalhamentoJSON,
                    FatContaItem.FaturamentoContaKitId,
                    FatCOntaItem.FatPacoteId,
                    FatCOntaItem.Observacao,
                    SisUnidadeOrganizacional.Id AS UnidadeOrganizacionalId,
					SisUnidadeOrganizacional.Descricao AS UnidadeOrganizacionalDescricao,
                    SisMoeda.Id,
                    SisMoeda.Codigo AS CodigoMoeda,
                    SisMoeda.Descricao
                FROM 
                    FatContaItem
                    INNER JOIN FatConta ON FatConta.Id = FatContaItem.FatContaId AND FatConta.IsDeleted = @IsDeleted
                    INNER JOIN AteAtendimento ON AteAtendimento.Id = FatConta.SisAtendimentoId AND AteAtendimento.IsDeleted = @IsDeleted
                    INNER JOIN FatItem ON FatContaItem.FatItemId = FatItem.Id AND FatItem.IsDeleted = @IsDeleted
                    INNER JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId
                    LEFT JOIN FatSubGrupo ON  FatSubGrupo.Id = FatItem.SubGrupoId
                    LEFT JOIN SisUnidadeOrganizacional ON SisUnidadeOrganizacional.Id = FatContaItem.UnidadeOrganizacionalId
                    LEFT JOIN SisMoeda On SisMoeda.id = FatCOntaItem.SisMoedaId
                WHERE 
                    FatContaId = @ContaId AND FatContaItem.IsDeleted = @IsDeleted";
         //   var queryPacotes = @"
         //       SELECT 
		       //     FatPacote.Id,
         //           FatPacote.Inicio AS DataInicial,
			      //  FatPacote.Final AS DataFinal,
			      //  FatItem.GrupoId AS GrupoId,
         //           CASE WHEN AteAtendimento.IsAmbulatorioEmergencia = 1 THEN 
	        //            FatGrupo.OrdemAmbulatorio 
         //           ELSE FatGrupo.OrdemInternacao END AS GrupoOrdem,
         //           FatGrupo.Descricao AS GrupoDescricao,
         //           FatSubGrupo.Id AS SubGrupoId,
         //           FatSubGrupo.Descricao AS SubGrupoDescricao,
         //           FatItem.Descricao AS FatItemDescricao,
         //           FatItem.Codigo AS Codigo,
         //           FatItem.CodAmb AS CodAmb,
         //           FatItem.CodCbhpm AS CodCbhpm,
         //           FatItem.CodTuss AS CodTuss,
         //           FatPacote.Qtde
		       //FROM 
         //           FatPacote INNER JOIN FatItem ON FatPacote.FaturamentoItemId = FatItem.Id AND FatItem.IsDeleted = @IsDeleted
		       //    INNER JOIN FatConta ON FatConta.Id = FatPacote.FaturamentoContaId AND FatConta.IsDeleted = @IsDeleted
		       //    INNER JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId
         //           LEFT JOIN FatSubGrupo ON  FatSubGrupo.Id = FatItem.SubGrupoId
		       //    INNER JOIN AteAtendimento ON AteAtendimento.Id = FatConta.SisAtendimentoId AND AteAtendimento.IsDeleted = @IsDeleted
         //      WHERE 
         //          FatPacote.FaturamentoContaId = @ContaId AND FatPacote.IsDeleted = @IsDeleted";
            using (var conn = new SqlConnection(this.GetConnection()))
            {
                var whereFilterCondition = new StringBuilder();

                whereFilterCondition.WhereIf(input.DataInicial.HasValue, " AND FatContaItem.Data >= @DataInicial ");
                whereFilterCondition.WhereIf(input.DataFinal.HasValue, " AND FatContaItem.Data <= @DataFinal ");
                whereFilterCondition.WhereIf(!input.GrupoIds.IsNullOrEmpty(), " AND FatGrupo.Id IN(@GrupoIds) ");
                whereFilterCondition.WhereIf(!input.CentroDeCustoIds.IsNullOrEmpty(), " AND FatContaItem.CentroCustoId IN(@CentroCustoIds) ");
                whereFilterCondition.WhereIf(!input.TerceirizadoIds.IsNullOrEmpty(), " AND FatContaItem.TerceirizadoId IN(@TerceirizadoIds) ");
                whereFilterCondition.WhereIf(!input.TurnoIds.IsNullOrEmpty(), " AND FatContaItem.TurnoId IN(@TurnoIds) ");
                whereFilterCondition.WhereIf(!input.LocalUtilizacaoIds.IsNullOrEmpty(), " AND FatContaItem.UnidadeOrganizacionalId IN(@LocalUtilizacaoIds) ");

                var contaItems = (await conn.QueryAsync<ResumoContaItemDto>(queryContaItem + whereFilterCondition.ToString(),
                    new
                    {
                        ContaId = input.Id,
                        DataInicial = input.DataInicial,
                        DataFinal = input.DataFinal,
                        GrupoIds = input.GrupoIds,
                        CentroDeCustoIds = input.CentroDeCustoIds,
                        TerceirizadoIds = input.TerceirizadoIds,
                        TurnoIds = input.TurnoIds,
                        LocalUtilizacaoIds = input.LocalUtilizacaoIds,
                        IsDeleted = false
                    })).ToList();

                //var resultPacotes = await conn.QueryAsync<ResumoContaPacoteDto>(queryPacotes, new { ContaId = input.Id, IsDeleted = false });
                //var resultPacotesIds = resultPacotes.Select(x => x.Id).ToList();

                //// retira os items dos pacotes e colocar o pacote
                //var resultContaItemSemPacotes = contaItems.Where(x => x.FatPacoteId == null || !resultPacotesIds.Contains(x.FatPacoteId.Value)).ToList();

                //resultContaItemSemPacotes.AddRange(resultPacotes.Select(x => ResumoContaItemDto.Mapear(x, contaItems.Where(x => x.FatPacoteId.HasValue && resultPacotesIds.Contains(x.FatPacoteId.Value)).ToList())));

                var resultContaItemSemPacotes = contaItems.Where(x => x.FatPacoteId == null || x.GrupoId == FaturamentoGrupoDto.Pacote).ToList();

                return resultContaItemSemPacotes;
            }
        }

        public async Task<DefaultReturn<ResumoContaDto>> ResumoContaFechada(FaturamentoResumoContaFilterDto input)
        {

            var result = new DefaultReturn<ResumoContaDto>()
            {
                ReturnObject = new ResumoContaDto()
            };

            long contaMedicaId = 0;
            if(string.IsNullOrEmpty(input.Id) || !long.TryParse(input.Id, out contaMedicaId))
            {
                result.Errors.Add(ErroDto.Criar("", "não foi possível encontrar a conta médica"));
                return result;
            }



            var contaMedica = await Obter(contaMedicaId).ConfigureAwait(false);


            var config = contaMedica.Atendimento.IsAmbulatorioEmergencia ? contaMedica.Convenio.ConfiguracaoResumoContaEmergencia : contaMedica.Convenio.ConfiguracaoResumoContaInternacao;

            if(config == null)
            {
                config = new Cadastros.Convenios.Dto.ConvenioConfiguracaoResumoContaDto();
            }


            var items = await BaseResumoConta(input);
            var resumoContaGrupo = new List<ResumoContaGrupoDto>();

            if (config.IsAgrupaItens)
            {
                if (config.IsAgrupaUnidadeOrganizacional)
                {
                    var aggroupedContaItems = AgruparItems(items, x => new
                    {
                        x.UnidadeOrganizacionalId,
                        x.FatItemId,
                        x.ValorItem,
                        x.GrupoId,
                        x.Observacao
                    }).GroupBy(x => new { x.GrupoId, x.GrupoDescricao, x.GrupoOrdem });

                    resumoContaGrupo = aggroupedContaItems.Select(aggroupedContaItem =>
                            new ResumoContaGrupoDto(aggroupedContaItem.Key.GrupoDescricao,
                                aggroupedContaItem.Key.GrupoId, aggroupedContaItem.Key.GrupoOrdem,
                                aggroupedContaItem.ToList())).ToList();
                } else
                {
                    var aggroupedContaItems = AgruparItems(items, x => new
                    {
                        x.DataInicial,
                        x.FatItemId,
                        x.ValorItem,
                        x.GrupoId,
                        x.Observacao
                    }).GroupBy(x => new { x.DataInicial });

                    resumoContaGrupo = aggroupedContaItems.Select(aggroupedContaItem =>
                            new ResumoContaGrupoDto(aggroupedContaItem.Key.DataInicial?.Date.ToString("dd/MM/YYYY"),
                                aggroupedContaItem.Key.DataInicial?.Date.Ticks, aggroupedContaItem.Key.DataInicial?.Date.Ticks, aggroupedContaItem.ToList())).ToList();
                }
            } else
            {
                resumoContaGrupo.Add(new ResumoContaGrupoDto("",null, null, items.ToList()));
            }

            result.ReturnObject.ResumoContaGrupo = resumoContaGrupo
                .OrderByDescending(x => x.GrupoOrdem.HasValue)
                .ThenBy(x => x.GrupoOrdem)
                .ThenBy(x => x.GrupoDescricao);

            return result;

        }

        public async Task<DefaultReturn<ResumoContaDto>> ResumoContaAberta(FaturamentoResumoContaFilterDto input)
        {
            var result = new DefaultReturn<ResumoContaDto>()
            {
                ReturnObject = new ResumoContaDto()
            };

            var items = await BaseResumoConta(input);

            var contaItems = AgruparItems(items, x => new
            {
                x.DataInicial,
                x.DataFinal,
                x.UnidadeOrganizacionalId,
                x.FatItemId,
                x.ValorItem,
                x.GrupoId,
                x.Observacao
            });


            var aggroupedContaItems =
                    contaItems.GroupBy(x => new { x.GrupoId, x.GrupoDescricao, x.GrupoOrdem });

            var resumoContaGrupo = aggroupedContaItems.Select(
                aggroupedContaItem =>
                    new ResumoContaGrupoDto(aggroupedContaItem.Key.GrupoDescricao,
                        aggroupedContaItem.Key.GrupoId, aggroupedContaItem.Key.GrupoOrdem,
                        aggroupedContaItem.ToList())).ToList();

            result.ReturnObject.ResumoContaGrupo = resumoContaGrupo
                .OrderByDescending(x => x.GrupoOrdem.HasValue)
                .ThenBy(x => x.GrupoOrdem)
                .ThenBy(x => x.GrupoDescricao);

            return result;
        }


        public async Task<DefaultReturn<DefaultReturnBool>> VerificaPacote(CriarOuEditarPacoteModalInputDto input)
        {
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            using (var fatItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAppService>())
            using (var fatPacoteAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoPacoteAppService>())
            using (var fatContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            {
                var result = new DefaultReturn<DefaultReturnBool>
                {
                    Errors = new List<ErroDto>(),
                    ReturnObject = new DefaultReturnBool()
                };

                if (!input.PacoteId.HasValue)
                {

                    result.Errors.Add(ErroDto.Criar("", "Não é possível cadastrar pacote sem pacote selecionado."));
                    result.ReturnObject.Sucesso = result.Errors.Any();
                    return result;
                }

                var viewModel = new CriarOuEditarPacoteModalViewModel()
                {
                    PacoteId = input.PacoteId,
                    DataInicio = input.DataInicio,
                    DataFim = input.DataFim,
                    ContaMedicaId = input.ContaMedicaId,
                    CentroCustoId = input.CentroCustoId,
                    TerceirizadoId = input.TerceirizadoId,
                    TurnoId = input.TurnoId,
                    TipoLeitoId = input.TipoLeitoId,
                    UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                    Qtde = input.Qtde,
                    Input = input
                };

                if (input.Id != 0)
                {
                    //var kit = await fatPacoteAppService.Object.Obter(input.KitId.Value).ConfigureAwait(false);
                    //viewModel.Kit = kit;
                    //viewModel.KitId = input.KitId;
                    //viewModel.Items = (await fatContaItemAppService.Object.ObterPorContaKit(input.Id, input.ContaMedicaId).ConfigureAwait(false)).ToList();


                }
                else
                {
                    var pacote = await fatItemAppService.Object.Obter(input.PacoteId ?? 0).ConfigureAwait(false);
                    var items = (await contaMedicaAppService.Object.ListarItems(new FaturamentoContaItemTableFilterDto()
                    {
                        ContaMedicaId = input.ContaMedicaId,
                        EnablePaginate = false
                    }).ConfigureAwait(false)).Items;

                    if (!items.IsNullOrEmpty())
                    {
                        viewModel.Pacote = pacote;
                        viewModel.Items = items.Where(x => x.Data >= input.DataInicio
                                                                                   && x.Data <= input.DataFim
                                                                                   && x.FaturamentoPacoteId == null
                                                                                   && x.TipoGrupoId != 4).Select(x => CriarOuEditarPacoteModalViewModel.MapearPacoteItemParaFatContaItem(x, input)).ToList();

                        if (viewModel.Items.IsNullOrEmpty())
                        {
                            result.Errors.Add(ErroDto.Criar("", "Não há nenhum item para o filtro selecionado."));
                        }

                        foreach (var item in viewModel.Items.Where(x => x.FaturamentoItemId.HasValue))
                        {
                            var honorarios = ResumoDetalhamentoExtensions.MapearHonorarios(item);
                            var dto = new ValorTotalItemFaturamentoDto(
                                input.ContaMedicaId, 
                                item.Data.Value.DateTime, 
                                item.FaturamentoItemId.Value, 
                                (input.Qtde ?? 0) * item.Qtde, 
                                item.Percentual == 0f ? 1f : item.Percentual,
                                item.UnidadeOrganizacionalId,
                                item.TerceirizadoId,
                                item.CentroCustoId,
                                item.TurnoId,
                                item.TipoLeitoId,
                                honorarios);
                            var resultItem = await fatContaItemAppService.Object
                                .CalcularValorTotalItemFaturamento(dto).ConfigureAwait(false);
                            item.ResumoDetalhamento = resultItem.ReturnObject.ResumoDetalhamento;
                        }
                    }
                    else
                    {
                        result.Errors.Add(ErroDto.Criar("", "Não é possível cadastrar pacote sem nenhum item possível."));
                    }
                }

                result.ReturnObject.Sucesso = result.Errors.Any();

                return result;
            }
        }

        public async Task<DefaultReturn<DefaultReturnBool>> VerificaRemoverItensKit(long contaMedicaId, List<long> itemIds)
        {
            var result = new DefaultReturn<DefaultReturnBool>
            {
                Errors = new List<ErroDto>(),
                ReturnObject = new DefaultReturnBool()
            };
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            {
                if (contaMedicaId == 0)
                {

                    result.Errors.Add(ErroDto.Criar("", "Não é possível remover itens sem conta medica. "));
                    result.ReturnObject.Sucesso = result.Errors.Any();
                    return result;
                }

                if (itemIds.IsNullOrEmpty() || itemIds.Where(x=> x != 0).IsNullOrEmpty())
                {
                    result.Errors.Add(ErroDto.Criar("", "Não há itens para remover do kit."));
                    result.ReturnObject.Sucesso = result.Errors.Any();
                    return result;
                }

                var items = (await contaMedicaAppService.Object.ListarItems(new FaturamentoContaItemTableFilterDto()
                {
                    ContaMedicaId = contaMedicaId,
                    EnablePaginate = false
                }).ConfigureAwait(false)).Items;

                items = items.Where(x => itemIds.Contains(x.Id)).ToList();

                if(items.Any(x=> x.FaturamentokitId == null)) {

                    var itens = string.Join("", items.Where(x => x.FaturamentokitId == null).Select(x => "<li><b>"+ (x.Data.HasValue ? x.Data.Value.ToString("dd/MM/yyyy HH:mm:ss") : "") +" - " +x.ItemDescricao+"</b></li>"));
                    result.Errors.Add(ErroDto.Criar("", "Existem itens que não possuem kit atrelado." + 
                        "<ul style='display:block'>"
                            + itens +
                         "</ul>"));
                    result.ReturnObject.Sucesso = result.Errors.Any();
                    return result;
                }
                var kits = items.Select(x => new { x.KitDescricao, x.FaturamentokitId }).DistinctBy(x=> x.FaturamentokitId);
                if(kits.IsNullOrEmpty())
                {
                    result.Errors.Add(ErroDto.Criar("", "Não há kits atrelado aos itens selecionados."));
                    result.ReturnObject.Sucesso = result.Errors.Any();
                    return result;
                }
                else if (kits.Count() > 1)
                {
                    result.Warnings.Add(ErroDto.Criar("", "Foi identificado mais de um kit nos itens selecionados, deseja prosseguir a remoção dos itens mesmo assim?"));
                }

                return result;
            }

        }

        public async Task<DefaultReturn<DefaultReturnBool>> RemoverItensKit(long contaMedicaId, List<long> itemIds)
        {
            var result = new DefaultReturn<DefaultReturnBool>
            {
                Errors = new List<ErroDto>(),
                ReturnObject = new DefaultReturnBool()
            };

            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            using (var contaMedicaItemsRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
            using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
            {
                if (contaMedicaId == 0)
                {

                    result.Errors.Add(ErroDto.Criar("", "Não é possível remover itens sem conta medica. "));
                    result.ReturnObject.Sucesso = result.Errors.Any();
                    return result;
                }

                if (itemIds.IsNullOrEmpty() || itemIds.Where(x => x != 0).IsNullOrEmpty())
                {
                    result.Errors.Add(ErroDto.Criar("", "Não há itens para remover do kit."));
                    result.ReturnObject.Sucesso = result.Errors.Any();
                    return result;
                }

                var items = (await contaMedicaAppService.Object.ListarItems(new FaturamentoContaItemTableFilterDto()
                {
                    ContaMedicaId = contaMedicaId,
                    EnablePaginate = false
                }).ConfigureAwait(false)).Items;

                items = items.Where(x => itemIds.Contains(x.Id)).ToList();

                var entityItens = await contaMedicaItemsRepository.Object.GetAll()
                    .Where(x => x.FaturamentoContaId == contaMedicaId && itemIds.Contains(x.Id))
                    .ToListAsync();
                var userName = (await this.GetCurrentUserAsync()).FullName;
                foreach (var entity in entityItens)
                {
                    entity.FaturamentoContaKitId = null;
                    await contaMedicaItemsRepository.Object.InsertOrUpdateAndGetIdAsync(entity);

                    var currentItem = items.FirstOrDefault(x => x.Id == entity.Id);
                    await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            OcorrenciaTexto.ContaMedicaItemKitRemovido(currentItem?.ItemDescricao, currentItem?.KitDescricao, userName),
                            TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaKit, typeof(FaturamentoConta).FullName, contaMedicaId));
                }

            }

            return result;
        }



        #region comentado

        public Task<User> ObterUsuarioLogadoAsync()
        {
            return GetCurrentUserAsync();
        }

        // public async Task<FaturamentoContaDto> CriarOuEditar(FaturamentoContaDto input)
        // {
        //     try
        //     {
        //         using (var unitOfWork = UnitOfWorkManager.Begin())
        //         {
        //             var conta = input.MapearParaBanco();
        //
        //             if (input.Id.Equals(0))
        //             {
        //                 conta.StatusId = conta.StatusId ?? 1;
        //
        //                 if (conta.AtendimentoId != null)
        //                 {
        //                     var atendimento = await _atendimentoAppService.Obter((long)conta.AtendimentoId);
        //
        //                     if (atendimento != null)
        //                     {
        //                         conta.EmpresaId = atendimento.EmpresaId;
        //                         conta.PacienteId = atendimento.PacienteId;
        //                         conta.NumeroGuia = atendimento.NumeroGuia;
        //                     }
        //                 }
        //
        //                 input.Id = await _contaRepository.InsertAndGetIdAsync(conta);
        //             }
        //             else
        //             {
        //                 var ori = await _contaRepository.GetAsync(conta.Id);
        //                 ori.CodDependente = conta.CodDependente;
        //                 ori.Codigo = conta.Codigo;
        //                 ori.ConvenioId = conta.ConvenioId;
        //                 ori.DataAutorizacao = conta.DataAutorizacao;
        //                 ori.DataConferencia = conta.DataConferencia;
        //                 ori.DataEntrBolAnest = conta.DataEntrBolAnest;
        //                 ori.DataEntrCDFilme = conta.DataEntrCDFilme;
        //                 ori.DataEntrDescCir = conta.DataEntrDescCir;
        //                 ori.DataEntrFolhaSala = conta.DataEntrFolhaSala;
        //                 ori.DataFim = conta.DataFim;
        //                 ori.DataIncio = conta.DataIncio;
        //                 ori.DataPagamento = conta.DataPagamento;
        //                 ori.DataValidadeSenha = conta.DataValidadeSenha;
        //                 ori.Descricao = conta.Descricao;
        //                 ori.DiaSerie1 = conta.DiaSerie1;
        //                 ori.DiaSerie10 = conta.DiaSerie10;
        //                 ori.DiaSerie2 = conta.DiaSerie2;
        //                 ori.DiaSerie3 = conta.DiaSerie3;
        //                 ori.DiaSerie4 = conta.DiaSerie4;
        //                 ori.DiaSerie5 = conta.DiaSerie5;
        //                 ori.DiaSerie6 = conta.DiaSerie6;
        //                 ori.DiaSerie7 = conta.DiaSerie7;
        //                 ori.DiaSerie8 = conta.DiaSerie8;
        //                 ori.DiaSerie9 = conta.DiaSerie9;
        //                 ori.EmpresaId = conta.EmpresaId;
        //                 ori.FatGuiaId = conta.FatGuiaId;
        //                 ori.GuiaId = conta.GuiaId;
        //                 ori.GuiaOperadora = conta.GuiaOperadora;
        //                 ori.GuiaPrincipal = conta.GuiaPrincipal;
        //                 ori.IdentAcompanhante = conta.IdentAcompanhante;
        //                 ori.IsAutorizador = conta.IsAutorizador;
        //                 ori.IsSistema = conta.IsSistema;
        //                 ori.Matricula = conta.Matricula;
        //                 ori.MedicoId = conta.MedicoId;
        //                 ori.MotivoPendencia = conta.MotivoPendencia;
        //                 ori.NumeroGuia = conta.NumeroGuia;
        //                 ori.Observacao = conta.Observacao;
        //                 ori.PacienteId = conta.PacienteId;
        //                 ori.PlanoId = conta.PlanoId;
        //                 ori.SenhaAutorizacao = conta.SenhaAutorizacao;
        //                 ori.StatusId = conta.StatusId;
        //                 ori.TipoAcomodacaoId = conta.TipoAcomodacaoId;
        //                 ori.UnidadeOrganizacionalId = conta.UnidadeOrganizacionalId;
        //                 ori.UsuarioConferenciaId = conta.UsuarioConferenciaId;
        //                 ori.ValidadeCarteira = conta.ValidadeCarteira;
        //
        //                 await _contaRepository.UpdateAsync(ori);
        //
        //                 await FecharConta(input);
        //             }
        //
        //             UnitOfWorkManager.Current.SaveChanges();
        //             unitOfWork.Complete();
        //             unitOfWork.Dispose();
        //
        //             return input;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         throw new UserFriendlyException(L("ErroSalvar"), ex);
        //     }
        // }
        private async Task FecharConta(FaturamentoContaDto input)
        {
            if (input.DataFim != null)
            {
                ValidacaoFechamentoContaAppService validacaoService = new ValidacaoFechamentoContaAppService(_atendimentoRepository);

                var retorno = validacaoService.Validar(input);

                if (retorno.Errors.Count == 0)
                {
                    var atendimento = await _atendimentoAppService.Obter((long)input.AtendimentoId);

                    if (atendimento != null && (atendimento.DataAlta == null || (atendimento.DataAlta > input.DataFim)))
                    {
                        FaturamentoConta faturamentoConta = new FaturamentoConta();

                        faturamentoConta = input.MapearParaBanco();

                        faturamentoConta.Id = 0;
                        faturamentoConta.DataFim = null;
                        faturamentoConta.DataInicio = ((DateTime)input.DataFim).Date.AddDays(1);

                        var novacontaId = _contaRepository.InsertAndGetId(faturamentoConta);

                        var itensNoPeriodo = _contaItemRepository.GetAll()
                                                                    .Include(i => i.FaturamentoPacote)
                                                                    .Include(i => i.FaturamentoContaKit)
                                                                 .Where(w => w.FaturamentoContaId == input.Id
                                                                         && w.Data > input.DataFim)
                                                                 .ToList();

                        foreach (var item in itensNoPeriodo)
                        {
                            item.FaturamentoContaId = novacontaId;
                            if (item.FaturamentoContaKit != null)
                            {
                                item.FaturamentoContaKit.FaturamentoContaId = novacontaId;
                            }

                            if (item.FaturamentoPacote != null)
                            {
                                item.FaturamentoPacote.FaturamentoContaId = novacontaId;


                                var itensPacote = _contaItemRepository.GetAll()
                                                                      .Where(w => w.FaturamentoPacoteId == item.FaturamentoPacoteId)
                                                                      .ToList();

                                foreach (var itemPacote in itensPacote)
                                {
                                    itemPacote.FaturamentoContaId = novacontaId;
                                }
                            }
                        }
                    }
                }

            }
        }

        public async Task ConferirContas(ConferirContasInput input)
        {
            try
            {
                foreach (var contaId in input.ContasIds)
                {
                    var conta = _contaRepository.Get(contaId);
                    conta.StatusId = 2; // 2 - Conta Conferida

                    var usuario = GetCurrentUser();
                    conta.UsuarioConferenciaId = usuario.Id;

                    // Atualizando Status de entrega dos itens da conta (PRECISA IMPLEMENTAR 'POR PERIODO')
                    var itens = _contaItemRepository.GetAll().Where(x => x.FaturamentoContaId == contaId);

                    foreach (var i in itens)
                    {
                        i.StatusId = 2;
                        await _contaItemRepository.UpdateAsync(i);
                    }

                    await _contaRepository.UpdateAsync(conta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task EditarComUsuarioConferencia(FaturamentoContaDto input)
        {
            try
            {
                var Conta = input.MapearParaBanco();
                var usuarioLogado = await ObterUsuarioLogadoAsync();
                Conta.UsuarioConferenciaId = usuarioLogado.Id;

                if (input.Id.Equals(0))
                {
                    await _contaRepository.InsertAsync(Conta);
                }
                else
                {
                    await _contaRepository.UpdateAsync(Conta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<PagedResultDto<ContaMedicaViewModel>> Listar(ListarContasInput input)
        {
            var contarContas = 0;
            List<FaturamentoConta> contas;
            List<ContaMedicaViewModel> contasDtos = new List<ContaMedicaViewModel>();

            var dataInicial = input.StartDate != null ? ((DateTime)input.StartDate).Date : (DateTime?)null;
            var dataFinal = input.EndDate != null ? ((DateTime)input.EndDate).Date : (DateTime?)null;


            try
            {
                var query = _contaRepository
                    .GetAll()
                    .Include(i => i.Empresa)
                    .Include(i => i.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(i => i.Medico)
                    .Include(i => i.Medico.SisPessoa)
                    .Include(i => i.Plano)
                    .Include(i => i.Atendimento)
                    .Include(i => i.Atendimento.Paciente)
                    .Include(i => i.Atendimento.Paciente.SisPessoa)
                    .Include(i => i.Atendimento.Guia)
                    .Include(i => i.Status)

                    .WhereIf(input.IsEmergencia, x => x.Atendimento.IsAmbulatorioEmergencia)
                    .WhereIf(input.IsInternacao, x => x.Atendimento.IsInternacao)
                    .WhereIf(!string.IsNullOrEmpty(input.EmpresaId.ToString()), e => e.EmpresaId.ToString() == input.EmpresaId.ToString())
                    .WhereIf(!string.IsNullOrEmpty(input.ConvenioId), e => e.ConvenioId.ToString() == input.ConvenioId)
                    .WhereIf(!string.IsNullOrEmpty(input.PacienteId), e => e.Atendimento.PacienteId.ToString() == input.PacienteId)
                    .WhereIf(!string.IsNullOrEmpty(input.MedicoId), e => e.MedicoId.ToString() == input.MedicoId)

                    .Where(_ => _.DataInicio >= dataInicial && _.DataFim <= dataFinal
                       || (_.DataInicio == null && _.DataFim == null)
                       || (_.DataInicio >= dataInicial && _.DataFim == null)
                       || (_.DataFim <= dataFinal && _.DataInicio == null)
                       );

                contarContas = await query.CountAsync();

                contas = await query
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var c in contas)
                {
                    var conta = new ContaMedicaViewModel();

                    conta.Id = c.Id;
                    conta.Matricula = c.Matricula;
                    conta.CodDependente = c.CodDependente;
                    conta.NumeroGuia = c.NumeroGuia;
                    conta.Titular = c.Titular;
                    conta.GuiaOperadora = c.GuiaOperadora;
                    conta.GuiaPrincipal = c.GuiaPrincipal;
                    conta.Observacao = c.Observacao;
                    conta.SenhaAutorizacao = c.SenhaAutorizacao;
                    conta.IdentAcompanhante = c.IdentAcompanhante;

                    if (c.Atendimento != null)
                    {
                        conta.AtendimentoCodigo = c.Atendimento.Codigo;
                        conta.PlanoNome = c.Atendimento.Plano != null ? c.Atendimento.Plano.Descricao : string.Empty;
                        conta.GuiaNumero = c.Atendimento.GuiaNumero;

                        if (c.Atendimento.Paciente != null)
                        {
                            conta.PacienteNome = c.Atendimento.Paciente.NomeCompleto;
                        }
                    }

                    conta.PacienteId = c.PacienteId;
                    conta.MedicoId = c.MedicoId;
                    conta.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                    conta.ConvenioId = c.ConvenioId;
                    conta.ConvenioNome = c.Convenio != null ? c.Convenio.NomeFantasia : string.Empty;
                    conta.PlanoId = c.PlanoId;
                    conta.GuiaId = c.GuiaId;
                    conta.EmpresaId = c.EmpresaId;
                    conta.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                    conta.AtendimentoId = c.AtendimentoId;
                    conta.UnidadeOrganizacionalId = c.UnidadeOrganizacionalId;
                    conta.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                    //conta.TipoLeitoId = c.TipoLeitoId;
                    //conta.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;


                    conta.TipoLeitoId = c.TipoAcomodacaoId;
                    conta.TipoLeitoDescricao = c.TipoAcomodacao != null ? c.TipoAcomodacao.Descricao : string.Empty;

                    conta.DataInicio = c.DataInicio;
                    conta.DataFim = c.DataFim;
                    conta.DataPagamento = c.DataPagamento;
                    conta.ValidadeCarteira = c.ValidadeCarteira;
                    conta.DataAutorizacao = c.DataAutorizacao;
                    conta.DiaSerie1 = c.DiaSerie1;
                    conta.DiaSerie2 = c.DiaSerie2;
                    conta.DiaSerie3 = c.DiaSerie3;
                    conta.DiaSerie4 = c.DiaSerie4;
                    conta.DiaSerie5 = c.DiaSerie5;
                    conta.DiaSerie6 = c.DiaSerie6;
                    conta.DiaSerie7 = c.DiaSerie7;
                    conta.DiaSerie8 = c.DiaSerie8;
                    conta.DiaSerie9 = c.DiaSerie9;
                    conta.DiaSerie10 = c.DiaSerie10;
                    conta.DataEntrFolhaSala = c.DataEntrFolhaSala;
                    conta.DataEntrDescCir = c.DataEntrDescCir;
                    conta.DataEntrBolAnest = c.DataEntrBolAnest;
                    conta.DataEntrCDFilme = c.DataEntrCDFilme;
                    conta.DataValidadeSenha = c.DataValidadeSenha;
                    conta.IsAutorizador = c.IsAutorizador;
                    conta.TipoAtendimento = c.TipoAtendimento;
                    conta.StatusEntregaCodigo = c.Status?.Codigo;
                    conta.StatusEntregaDescricao = c.Status?.Descricao;
                    conta.StatusEntregaCor = c.Status?.Cor;

                    contasDtos.Add(conta);
                }

                return new PagedResultDto<ContaMedicaViewModel>(contarContas, contasDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ContaMedicaViewModel>> ListarParaAtendimento(ListarContasInput input)
        {
            var contarContas = 0;
            List<FaturamentoConta> contas;
            List<ContaMedicaViewModel> contasDtos = new List<ContaMedicaViewModel>();

            try
            {
                var query = _contaRepository.GetAll()
                    .Where(i => i.AtendimentoId.ToString() == input.AtendimentoId)
                    .Include(i => i.Empresa)
                    .Include(i => i.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(i => i.Medico)
                    .Include(i => i.Medico.SisPessoa)
                    .Include(i => i.Plano)
                    .Include(i => i.Atendimento)
                    .Include(i => i.Atendimento.Paciente)
                    .Include(i => i.Atendimento.Plano)
                    .Include(i => i.Atendimento.Paciente.SisPessoa)
                    .Include(i => i.Atendimento.Guia)
                    .Include(i => i.Atendimento.FatGuia)
                    .Include(i => i.Paciente)
                    .Include(i => i.FatGuia)
                    .Include(i => i.Status)
                    .Include(i => i.Atendimento.TipoAcomodacao)
                    .Include(i => i.TipoAcomodacao)
                    .WhereIf(!string.IsNullOrEmpty(input.EmpresaId.ToString()), e => e.EmpresaId.ToString() == input.EmpresaId.ToString())
                    .WhereIf(!string.IsNullOrEmpty(input.ConvenioId), e => e.ConvenioId.ToString() == input.ConvenioId)
                    .WhereIf(!string.IsNullOrEmpty(input.PacienteId), e => e.Atendimento.PacienteId.ToString() == input.PacienteId)
                    .WhereIf(!string.IsNullOrEmpty(input.MedicoId), e => e.MedicoId.ToString() == input.MedicoId)

                    .WhereIf(!input.IgnoraData,
                           _ =>
                           _.DataInicio >= input.StartDate && _.DataFim <= input.EndDate
                       || (_.DataInicio == null && _.DataFim == null)
                       || (_.DataInicio >= input.StartDate && _.DataFim == null)
                       || (_.DataFim <= input.EndDate && _.DataInicio == null)
                        )
                    ;

                contarContas = await query.CountAsync();
                contas = await query.AsNoTracking().ToListAsync();

                foreach (var c in contas)
                {
                    var conta = new ContaMedicaViewModel();

                    conta.Id = c.Id;
                    conta.Matricula = c.Matricula;
                    conta.CodDependente = c.CodDependente;
                    conta.NumeroGuia = c.NumeroGuia;
                    conta.Titular = c.Titular;
                    conta.GuiaOperadora = c.GuiaOperadora;
                    conta.GuiaPrincipal = c.GuiaPrincipal;
                    conta.Observacao = c.Observacao;
                    conta.SenhaAutorizacao = c.SenhaAutorizacao;
                    conta.IdentAcompanhante = c.IdentAcompanhante;

                    if (c.Atendimento != null)
                    {
                        conta.AtendimentoCodigo = c.Atendimento.Codigo;
                        conta.PlanoNome = c.Atendimento.Plano != null ? c.Atendimento.Plano.Descricao : string.Empty;
                        conta.PlanoId = c.Atendimento.Plano != null ? c.Atendimento.Plano.Id : 0;
                        conta.GuiaNumero = c.Atendimento.GuiaNumero;
                        if (c.Atendimento.Paciente != null)
                        {
                            conta.PacienteNome = c.Atendimento.Paciente.NomeCompleto;
                        }
                    }

                    conta.PacienteId = c.PacienteId;
                    conta.MedicoId = c.MedicoId;
                    conta.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                    conta.ConvenioId = c.ConvenioId;
                    conta.ConvenioNome = c.Convenio != null ? c.Convenio.NomeFantasia : string.Empty;
                    conta.PlanoId = c.PlanoId;
                    conta.GuiaId = c.FatGuia?.Id;
                    conta.FatGuia = c.FatGuia != null ? c.FatGuia : null;
                    conta.FatGuiaId = c.FatGuiaId?.ToString();
                    conta.EmpresaId = c.EmpresaId;
                    conta.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                    conta.AtendimentoId = c.AtendimentoId;
                    conta.UnidadeOrganizacionalId = c.UnidadeOrganizacionalId;
                    conta.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                    //conta.TipoLeitoId = c.TipoLeitoId;
                    //conta.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;

                    conta.TipoLeitoId = c.TipoAcomodacaoId;
                    conta.TipoLeitoDescricao = c.TipoAcomodacao != null ? c.TipoAcomodacao.Descricao : string.Empty;

                    conta.DataInicio = c.DataInicio;
                    conta.DataFim = c.DataFim;
                    conta.DataPagamento = c.DataPagamento;
                    conta.ValidadeCarteira = c.ValidadeCarteira;
                    conta.DataAutorizacao = c.DataAutorizacao;
                    conta.DiaSerie1 = c.DiaSerie1;
                    conta.DiaSerie2 = c.DiaSerie2;
                    conta.DiaSerie3 = c.DiaSerie3;
                    conta.DiaSerie4 = c.DiaSerie4;
                    conta.DiaSerie5 = c.DiaSerie5;
                    conta.DiaSerie6 = c.DiaSerie6;
                    conta.DiaSerie7 = c.DiaSerie7;
                    conta.DiaSerie8 = c.DiaSerie8;
                    conta.DiaSerie9 = c.DiaSerie9;
                    conta.DiaSerie10 = c.DiaSerie10;
                    conta.DataEntrFolhaSala = c.DataEntrFolhaSala;
                    conta.DataEntrDescCir = c.DataEntrDescCir;
                    conta.DataEntrBolAnest = c.DataEntrBolAnest;
                    conta.DataEntrCDFilme = c.DataEntrCDFilme;
                    conta.DataValidadeSenha = c.DataValidadeSenha;
                    conta.IsAutorizador = c.IsAutorizador;
                    conta.TipoAtendimento = c.TipoAtendimento;
                    conta.StatusEntregaCodigo = c.Status?.Codigo;
                    conta.StatusEntregaDescricao = c.Status?.Descricao;

                    contasDtos.Add(conta);
                }

                return new PagedResultDto<ContaMedicaViewModel>(contarContas, contasDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ContaMedicaViewModel>> ListarParaExame(ListarContasInput input)
        {
            var contarContas = 0;
            List<FaturamentoConta> contas;
            List<ContaMedicaViewModel> contasDtos = new List<ContaMedicaViewModel>();
            try
            {

                var query = _contaItemRepository
                    .GetAll()
                    .Include(m => m.FaturamentoConta)
                    .Include(m => m.FaturamentoItem)
                    .Where(m => (m.FaturamentoItem.IsRequisicaoExame))
                    .WhereIf(input.EmpresaId.HasValue, e => e.FaturamentoConta.EmpresaId == input.EmpresaId)
                    .WhereIf(!string.IsNullOrEmpty(input.ConvenioId), e => e.FaturamentoConta.ConvenioId.ToString() == input.ConvenioId)
                    .WhereIf(!string.IsNullOrEmpty(input.PacienteId), e => e.FaturamentoConta.Atendimento.PacienteId.ToString() == input.PacienteId)
                    .WhereIf(!string.IsNullOrEmpty(input.MedicoId), e => e.MedicoId.ToString() == input.MedicoId)

                    .WhereIf(!input.IgnoraData,
                           _ =>
                           _.FaturamentoConta.DataInicio >= input.StartDate && _.FaturamentoConta.DataFim <= input.EndDate
                       || (_.FaturamentoConta.DataInicio == null && _.FaturamentoConta.DataFim == null)
                       || (_.FaturamentoConta.DataInicio >= input.StartDate && _.FaturamentoConta.DataFim == null)
                       || (_.FaturamentoConta.DataFim <= input.EndDate && _.FaturamentoConta.DataInicio == null)
                        )

                    .Select(m => m.FaturamentoConta)
                    .Distinct();

                contarContas = await query.CountAsync();
                contas = await query.AsNoTracking().ToListAsync();

                foreach (var c in contas)
                {
                    if (c != null)
                    {
                        if (c.EmpresaId.HasValue)
                        {
                            var empresa = await _empresaAppService.Obter(c.EmpresaId.Value);
                            c.Empresa = empresa.MapTo<Empresa>();
                        }
                        if (c.ConvenioId.HasValue)
                        {
                            var convenio = await _convenioAppService.Obter(c.ConvenioId.Value);
                            c.Convenio = convenio.MapTo<Convenio>();
                        }
                        if (c.MedicoId.HasValue)
                        {
                            var medico = await _medicoAppService.Obter(c.MedicoId.Value);
                            c.Medico = medico.MapTo<Medico>();
                        }
                        if (c.PlanoId.HasValue)
                        {
                            var plano = await _planoAppService.Obter(c.PlanoId.Value);
                            c.Plano = plano.MapTo<Plano>();
                        }
                        if (c.AtendimentoId.HasValue)
                        {
                            var atendimento = await _atendimentoAppService.Obter(c.AtendimentoId.Value);
                            c.Atendimento = atendimento.MapTo<Atendimento>();
                        }
                        if (c.PacienteId.HasValue)
                        {
                            var paciente = await _pacienteAppService.Obter(c.PacienteId.Value);
                            c.Paciente = paciente.MapTo<Paciente>();
                        }
                        if (c.GuiaId.HasValue)
                        {
                            var guia = await _guiaAppService.Obter(c.GuiaId.Value);
                            c.Guia = guia.MapTo<Guia>();
                        }

                        var conta = new ContaMedicaViewModel();

                        conta.Id = c.Id;
                        conta.Matricula = c.Matricula;
                        conta.CodDependente = c.CodDependente;
                        conta.NumeroGuia = c.NumeroGuia;
                        conta.Titular = c.Titular;
                        conta.GuiaOperadora = c.GuiaOperadora;
                        conta.GuiaPrincipal = c.GuiaPrincipal;
                        conta.Observacao = c.Observacao;
                        conta.SenhaAutorizacao = c.SenhaAutorizacao;
                        conta.IdentAcompanhante = c.IdentAcompanhante;

                        if (c.Atendimento != null)
                        {
                            conta.AtendimentoCodigo = c.Atendimento.Codigo;
                            conta.PlanoNome = c.Atendimento.Plano != null ? c.Atendimento.Plano.Descricao : string.Empty;
                            conta.PlanoId = c.Atendimento.Plano != null ? c.Atendimento.Plano.Id : 0;
                            conta.GuiaNumero = c.Atendimento.GuiaNumero;
                            if (c.Atendimento.Paciente != null)
                            {
                                conta.PacienteNome = c.Atendimento.Paciente.NomeCompleto;
                            }
                        }

                        conta.PacienteId = c.PacienteId;
                        conta.MedicoId = c.MedicoId;
                        conta.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                        conta.ConvenioId = c.ConvenioId;
                        conta.ConvenioNome = c.Convenio != null ? c.Convenio.NomeFantasia : string.Empty;
                        conta.GuiaId = c.FatGuia?.Id;
                        conta.FatGuia = c.FatGuia != null ? c.FatGuia : null;
                        conta.FatGuiaId = c.FatGuiaId.ToString();
                        conta.EmpresaId = c.EmpresaId;
                        conta.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                        conta.AtendimentoId = c.AtendimentoId;
                        conta.UnidadeOrganizacionalId = c.UnidadeOrganizacionalId;
                        conta.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                        //conta.TipoLeitoId = c.TipoLeitoId;
                        //conta.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;

                        conta.TipoLeitoId = c.TipoAcomodacaoId;
                        conta.TipoLeitoDescricao = c.TipoAcomodacao != null ? c.TipoAcomodacao.Descricao : string.Empty;
                        conta.DataInicio = c.DataInicio;
                        conta.DataFim = c.DataFim;
                        conta.DataPagamento = c.DataPagamento;
                        conta.ValidadeCarteira = c.ValidadeCarteira;
                        conta.DataAutorizacao = c.DataAutorizacao;
                        conta.DiaSerie1 = c.DiaSerie1;
                        conta.DiaSerie2 = c.DiaSerie2;
                        conta.DiaSerie3 = c.DiaSerie3;
                        conta.DiaSerie4 = c.DiaSerie4;
                        conta.DiaSerie5 = c.DiaSerie5;
                        conta.DiaSerie6 = c.DiaSerie6;
                        conta.DiaSerie7 = c.DiaSerie7;
                        conta.DiaSerie8 = c.DiaSerie8;
                        conta.DiaSerie9 = c.DiaSerie9;
                        conta.DiaSerie10 = c.DiaSerie10;
                        conta.DataEntrFolhaSala = c.DataEntrFolhaSala;
                        conta.DataEntrDescCir = c.DataEntrDescCir;
                        conta.DataEntrBolAnest = c.DataEntrBolAnest;
                        conta.DataEntrCDFilme = c.DataEntrCDFilme;
                        conta.DataValidadeSenha = c.DataValidadeSenha;
                        conta.IsAutorizador = c.IsAutorizador;
                        conta.TipoAtendimento = c.TipoAtendimento;
                        conta.StatusEntregaCodigo = c.Status?.Codigo;
                        conta.StatusEntregaDescricao = c.Status?.Descricao;

                        contasDtos.Add(conta);
                    }
                }

                return new PagedResultDto<ContaMedicaViewModel>(contarContas, contasDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task Excluir(FaturamentoContaDto input)
        {
            try
            {
                await _contaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<int> VerificaFluxo(long id)
        {
            var conta = await Obter(id);
            if (conta == null)
            {
                return -1;
            }

            switch (conta.StatusId)
            {
                case FaturamentoContaStatusDto.Inicial:
                {
                    if (conta.Convenio != null && conta.Convenio.HabilitaAuditoriaInterna)
                    {
                        return FaturamentoContaStatusDto.AuditoriaInterna;
                    }
                    else if(conta.Convenio != null && !conta.Convenio.HabilitaAuditoriaInterna)
                    {
                        return FaturamentoContaStatusDto.Conferido;
                    }
                    return FaturamentoContaStatusDto.AuditoriaInterna;
                }
                case FaturamentoContaStatusDto.AuditoriaInterna:
                {
                    if (conta.Convenio != null && conta.Convenio.HabilitaAuditoriaExterna)
                    {
                        return FaturamentoContaStatusDto.AuditoriaExterna;
                    }
                    else if (conta.Convenio != null && !conta.Convenio.HabilitaAuditoriaExterna)
                    {
                        return FaturamentoContaStatusDto.Conferido;
                    }
                    return FaturamentoContaStatusDto.AuditoriaExterna;
                }
                case FaturamentoContaStatusDto.AuditoriaExterna:
                {
                    return FaturamentoContaStatusDto.Conferido;
                }
            }

            return -1;
        }

        public async Task<int> VerificaFluxoVolta(long id)
        {
            var conta = await Obter(id);
            if (conta == null)
            {
                return -1;
            }

            switch (conta.StatusId)
            {
                case FaturamentoContaStatusDto.Conferido:
                {
                    if (conta.Convenio != null && conta.Convenio.HabilitaAuditoriaExterna)
                    {
                        return FaturamentoContaStatusDto.AuditoriaExterna;
                    }
                    else if (conta.Convenio != null && conta.Convenio.HabilitaAuditoriaInterna)
                    {
                        return FaturamentoContaStatusDto.AuditoriaInterna;
                    }

                    return FaturamentoContaStatusDto.Inicial;
                }
                case FaturamentoContaStatusDto.AuditoriaExterna:
                {
                    if (conta.Convenio != null && conta.Convenio.HabilitaAuditoriaInterna)
                    {
                        return FaturamentoContaStatusDto.AuditoriaInterna;
                    }
                    else if (conta.Convenio != null && !conta.Convenio.HabilitaAuditoriaInterna)
                    {
                        return FaturamentoContaStatusDto.Inicial;
                    }
                    return FaturamentoContaStatusDto.Inicial;
                }
                case FaturamentoContaStatusDto.AuditoriaInterna:
                {
                    return FaturamentoContaStatusDto.Inicial;
                }
                case FaturamentoContaStatusDto.Inicial:
                {
                    if (conta.Convenio != null && conta.Convenio.HabilitaAuditoriaInterna)
                    {
                        return FaturamentoContaStatusDto.AuditoriaInterna;
                    }
                    else if (conta.Convenio != null && !conta.Convenio.HabilitaAuditoriaInterna)
                    {
                        return FaturamentoContaStatusDto.Conferido;
                    }
                    return FaturamentoContaStatusDto.AuditoriaInterna;
                }
            }

            return -1;
        }

        public async Task<long> AlteraStatusConta(long contaMedicaId, int statusId)
        {
            using (var faturamentoContaMedica = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
            {
                var contaMedica = await Obter(contaMedicaId);
                return await GerarVersao(contaMedica, statusId);
            }
        }


        

        public async Task<long> GerarVersao(long contaMedicaId, int? novoStatusId = null)
        {
            var contaMedica = await Obter(contaMedicaId);
            return await GerarVersao(contaMedica, novoStatusId);
        }

        public async Task<IEnumerable<FaturamentoContaVersaoDto>> ObterVersoes(long contaMedicaId)
        {
            const string query = "SELECT Id, IsAtivo, Versao, FatContaMedicaId AS ContaMedicaId FROM FatConta WHERE (Id = @contaMedicaId OR FatContaMedicaId = @contaMedicaId) AND IsDeleted = @IsDeleted";
            var result = new List<FaturamentoContaVersaoDto>();
            using (var conn = new SqlConnection(this.GetConnection()))
            {
                var temp = await conn.QueryAsync<FaturamentoContaVersaoDto>(query,
                    new { contaMedicaId, IsDeleted = false });
                result.AddRange(temp);

                foreach (var item in temp)
                {
                    result.AddRange(await ObterVersoesTree(item));
                }

                async Task<IEnumerable<FaturamentoContaVersaoDto>> ObterVersoesTree(FaturamentoContaVersaoDto item)
                {
                    var treeData = new List<FaturamentoContaVersaoDto>();
                    treeData.AddRange(await conn.QueryAsync<FaturamentoContaVersaoDto>(query, new { contaMedicaId = item.ContaMedicaId, IsDeleted = false }));
                    if (!treeData.Any(x => x.ContaMedicaId.HasValue))
                    {
                        return treeData;
                    }

                    var treeDataContaMedica = treeData
                        .Where(x =>
                            x.ContaMedicaId.HasValue
                            && treeData.All(z => z.Id != x.ContaMedicaId.Value)).ToList();
                    foreach (var treeDataContaMedicaItem in treeDataContaMedica)
                    {
                        treeData.AddRange(await ObterVersoesTree(treeDataContaMedicaItem));
                    }
                    return treeData;
                }
            }


            return result.DistinctBy(x => x.Id);
        }

        public async Task<long> GerarVersao(FaturamentoContaDto contaMedica, int? novoStatusId = null)
        {
            using (var contaMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
            using (var contaMedicaItensRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
            using (var contaKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaKit, long>>())
            using (var contaPacoteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoPacote, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                if (contaMedica == null)
                {
                    throw new UserFriendlyException(
                        "Não foi possivel gerar uma versão pois a conta Medica não foi encontrada");
                }

                try
                {
                    var versoes = await ObterVersoes(contaMedica.Id);
                    var numVersao = versoes.IsNullOrEmpty() ? 0 : versoes.Max(x => x.Versao);
                    var contaMedicaPrincipal = versoes.IsNullOrEmpty() ? contaMedica.Id : versoes.FirstOrDefault(v => v.ContaMedicaId == null).Id;

                    // Conta Medica

                    var novaContaMedica = contaMedica.MapearParaBanco();
                    novaContaMedica.Id = 0;
                    novaContaMedica.Versao = numVersao + 1;
                    novaContaMedica.ContaMedicaId = contaMedicaPrincipal;
                    novaContaMedica.ContaMedica = null;
                    novaContaMedica.IsAtivo = true;
                    if (novoStatusId.HasValue)
                    {
                        novaContaMedica.StatusId = novoStatusId.Value;
                    }

                    novaContaMedica.Id = contaMedicaRepository.Object.InsertOrUpdateAndGetId(novaContaMedica);

                    //Kit

                    var kits = await contaKitRepository.Object.GetAll().AsNoTracking()
                        .Where(x => x.FaturamentoContaId == contaMedica.Id).ToListAsync();

                    var kitDictionary = new Dictionary<long, long>();

                    foreach (var kit in kits)
                    {
                        var kitId = kit.Id;
                        kit.Id = 0;
                        kit.FaturamentoContaId = novaContaMedica.Id;
                        var novoKitId = await contaKitRepository.Object.InsertAndGetIdAsync(kit);

                        kitDictionary.Add(kitId, novoKitId);
                    }



                    //Pacotes

                    var pacotes = await contaPacoteRepository.Object.GetAll().AsNoTracking()
                        .Where(x => x.FaturamentoContaId == contaMedica.Id).ToListAsync();

                    var pacoteDictionary = new Dictionary<long, long>();

                    foreach (var pacote in pacotes)
                    {
                        var pacoteId = pacote.Id;
                        pacote.Id = 0;
                        pacote.FaturamentoContaId = novaContaMedica.Id;
                        var novoPacoteId = await contaPacoteRepository.Object.InsertAndGetIdAsync(pacote);

                        pacoteDictionary.Add(pacoteId, novoPacoteId);
                    }

                    //Items
                    var items = await contaMedicaItensRepository.Object.GetAll().AsNoTracking().Where(x => x.FaturamentoContaId == contaMedica.Id).ToListAsync();
                    items.ForEach(x =>
                    {
                        x.Id = 0;
                        x.FaturamentoContaId = novaContaMedica.Id;

                        if (x.FaturamentoPacoteId.HasValue && pacoteDictionary.ContainsKey(x.FaturamentoPacoteId.Value))
                        {
                            x.FaturamentoPacoteId = pacoteDictionary.FirstOrDefault(pd=> pd.Key == x.FaturamentoPacoteId.Value).Value;
                        }

                        if (x.FaturamentoContaKitId.HasValue && kitDictionary.ContainsKey(x.FaturamentoContaKitId.Value))
                        {
                            x.FaturamentoContaKitId = kitDictionary.FirstOrDefault(pd => pd.Key == x.FaturamentoContaKitId.Value).Value;
                        }

                        x.Id = contaMedicaItensRepository.Object.InsertOrUpdateAndGetId(x);
                    });


                    contaMedicaRepository.Object.Update(contaMedica.Id, x => x.IsAtivo = false);

                    // Recalcular valores na hora de gerar uma versão?

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    return novaContaMedica.Id;

                }
                catch (Exception e)
                {
                    throw new UserFriendlyException(
                        "Não foi possivel gerar uma versão. Entre em contato com o suporte.", e);
                }
            }
        }

        public async Task<long> ObterVersaoAtual(long contaMedicaId)
        {
            var versoes = (await ObterVersoes(contaMedicaId)).Where(x => x.IsAtivo);

            var contaMedicaAtual = versoes.FirstOrDefault(x => x.Versao == versoes.Max(z => z.Versao))?.Id ?? 0;

            if (contaMedicaAtual == 0)
            {
                contaMedicaAtual = contaMedicaId;
            }

            return contaMedicaAtual;
        }

        public async Task<FaturamentoContaDto> Obter(long id)
        {
            try
            {
                var contaMedicaAtual = await ObterVersaoAtual(id);

                var query = await _contaRepository
                    .GetAll()
                    .Include(p => p.Medico)
                    .Include(i => i.Medico.SisPessoa)
                    .Include(c => c.Convenio)
                    .Include(c => c.Convenio.ConfiguracaoResumoContaEmergencia)
                    .Include(c => c.Convenio.ConfiguracaoResumoContaInternacao)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(c => c.Plano)
                    .Include(a => a.Atendimento)
                    .Include(a => a.Atendimento.Paciente)
                    .Include(a => a.Atendimento.Paciente.SisPessoa)
                    .Include(a => a.Atendimento.Guia)
                    .Include(e => e.Empresa)
                    .Include(e => e.Status)
                    .Where(m => m.Id == contaMedicaAtual)
                    .FirstOrDefaultAsync();

                var conta = FaturamentoContaDto.Mapear(query); //query.MapTo<FaturamentoContaDto>();

                return conta;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long> ObterUltimaContaAtendimentoId(long atendimentoId)
        {
            try
            {
                var query = await _contaRepository
                    .GetAll()
                    .Where(m => m.AtendimentoId == atendimentoId)
                    .FirstOrDefaultAsync();

                var id = query != null ? query.Id : 0;
                return id;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ContaMedicaViewModel> ObterViewModel(long id)
        {
            try
            {
                var c = await _contaRepository
                    .GetAll()
                    .Include(p => p.Medico)
                    .Include(p => p.Status)
                    .Include(i => i.Medico.SisPessoa)
                    .Include(d => d.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(d => d.Plano)
                    .Include(a => a.Atendimento)
                    .Include(a => a.Atendimento.Paciente)
                    .Include(a => a.Atendimento.Paciente.SisPessoa)
                    .Include(a => a.Atendimento.FatGuia)
                    .Include(a => a.Atendimento.Guia)
                    .Include(e => e.Empresa)
                    //.Include(t => t.TipoLeito)
                    .Include(t => t.TipoAcomodacao)
                    .Include(s => s.Status)
                    .Include(s => s.FatGuia)
                    .Include(s => s.ContaItens)
                    .Include(s => s.ContaItens.Select(s2 => s2.FaturamentoItem.Grupo))
                    .Include(s => s.ContaItens.Select(s2 => s2.FaturamentoConfigConvenio))
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var conta = new ContaMedicaViewModel();

                conta.Id = c.Id;
                conta.Matricula = c.Matricula;
                conta.CodDependente = c.CodDependente;
                conta.NumeroGuia = c.NumeroGuia;
                conta.Titular = c.Titular;
                conta.GuiaOperadora = c.GuiaOperadora;
                conta.GuiaPrincipal = c.GuiaPrincipal;
                conta.Observacao = c.Observacao;
                conta.SenhaAutorizacao = c.SenhaAutorizacao;
                conta.IdentAcompanhante = c.IdentAcompanhante;
                conta.PacienteId = c.Atendimento.Paciente.Id;
                conta.PacienteNome = c.Atendimento.Paciente.NomeCompleto;
                conta.AtendimentoId = c.Atendimento.Id;
                conta.MedicoId = c.Medico != null ? c.Medico.Id : 0;
                conta.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                conta.ConvenioId = c.Convenio != null ? c.Convenio.Id : 0;
                conta.ConvenioNome = c.Convenio != null ? c.Convenio.NomeFantasia : string.Empty;
                conta.PlanoId = c.Atendimento.Plano != null ? c.Atendimento.Plano.Id : 0;
                conta.PlanoNome = c.Atendimento.Plano != null ? c.Atendimento.Plano.Descricao : string.Empty;
                conta.GuiaId = c.Atendimento.FatGuia != null ? c.Atendimento.FatGuia.Id : 0;
                conta.GuiaNumero = c.Atendimento.GuiaNumero;
                conta.FatGuia = c.Atendimento.FatGuia;
                conta.EmpresaId = c.Empresa != null ? c.Empresa.Id : 0;
                conta.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                conta.AtendimentoCodigo = c.Atendimento.Codigo;
                conta.UnidadeOrganizacionalId = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Id : 0;
                conta.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                //conta.TipoLeitoId = c.TipoLeito != null ? c.TipoLeito.Id : 0;
                //conta.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;

                conta.TipoLeitoId = c.TipoAcomodacao != null ? c.TipoAcomodacao.Id : 0;
                conta.TipoLeitoDescricao = c.TipoAcomodacao != null ? c.TipoAcomodacao.Descricao : string.Empty;

                conta.DataInicio = c.DataInicio;
                conta.DataFim = c.DataFim;
                conta.DataPagamento = c.DataPagamento;
                conta.ValidadeCarteira = c.ValidadeCarteira;
                conta.DataAutorizacao = c.DataAutorizacao;
                conta.DiaSerie1 = c.DiaSerie1;
                conta.DiaSerie2 = c.DiaSerie2;
                conta.DiaSerie3 = c.DiaSerie3;
                conta.DiaSerie4 = c.DiaSerie4;
                conta.DiaSerie5 = c.DiaSerie5;
                conta.DiaSerie6 = c.DiaSerie6;
                conta.DiaSerie7 = c.DiaSerie7;
                conta.DiaSerie8 = c.DiaSerie8;
                conta.DiaSerie9 = c.DiaSerie9;
                conta.DiaSerie10 = c.DiaSerie10;
                conta.DataEntrFolhaSala = c.DataEntrFolhaSala;
                conta.DataEntrDescCir = c.DataEntrDescCir;
                conta.DataEntrBolAnest = c.DataEntrBolAnest;
                conta.DataEntrCDFilme = c.DataEntrCDFilme;
                conta.DataValidadeSenha = c.DataValidadeSenha;
                conta.IsAutorizador = c.IsAutorizador;
                conta.TipoAtendimento = c.TipoAtendimento;
                conta.StatusEntregaCodigo = c.Status?.Codigo;
                conta.StatusEntregaDescricao = c.Status?.Descricao;
                conta.StatusEntregaId = c.Status?.Id;
                conta.Id = c.Id;


                if (c.ContaItens != null)
                {
                    conta.ContaItensDto = new List<FaturamentoContaItemDto>();

                    foreach (var item in c.ContaItens)
                    {
                        conta.ContaItensDto.Add(FaturamentoContaItemDto.MapearFromCore(item));
                    }
                }

                return conta;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ContaMedicaReportModel> ObterReportModel(long id, long atendimentoId = 0)
        {
            try
            {
                var c = await _contaRepository
                    .GetAll()
                    .Include(p => p.Medico)
                    .Include(p => p.Medico.Conselho)
                    .Include(i => i.Medico.SisPessoa)
                    .Include(d => d.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(p => p.Plano)
                    .Include(a => a.Atendimento.Guia)
                    .Include(a => a.Atendimento.FatGuia)
                    .Include(a => a.Atendimento)
                    .Include(a => a.Atendimento.Paciente)
                    .Include(a => a.Atendimento.Paciente.SisPessoa)
                    .Include(a => a.Atendimento.Guia)
                    .Include(e => e.Empresa)
                    //  .Include(t => t.TipoLeito)
                    .Include(t => t.TipoAcomodacao)
                    .Include(t => t.FatGuia)
                    .Include(p => p.Paciente.SisPessoa)
                    .WhereIf(id != 0, m => m.Id == id)
                    .WhereIf(atendimentoId != 0, m => m.Atendimento.Id == atendimentoId)
                    .FirstOrDefaultAsync();

                var conta = new ContaMedicaReportModel();
                if (c != null)
                {
                    conta.Id = c.Id;
                    conta.Matricula = c.Matricula;
                    conta.CodDependente = c.CodDependente;
                    conta.NumeroGuia = c.NumeroGuia;
                    conta.Titular = c.Titular;
                    conta.GuiaOperadora = c.GuiaOperadora;
                    conta.GuiaPrincipal = c.GuiaPrincipal;
                    conta.Observacao = c.Observacao;
                    conta.SenhaAutorizacao = c.SenhaAutorizacao;
                    conta.IdentAcompanhante = c.IdentAcompanhante;
                    conta.PacienteId = c.Atendimento.Paciente.Id;
                    conta.PacienteNome = c.Atendimento.Paciente.SisPessoa.NomeCompleto;
                    conta.PacienteNascimento = string.Format("{0:dd/MM/yyyy}", c.Atendimento.Paciente.Nascimento);
                    conta.AtendimentoId = c.Atendimento.Id;
                    conta.TipoAlta = c.Atendimento?.MotivoAlta?.MotivoAltaTipoAlta?.Descricao;

                    conta.MedicoId = c.Medico != null ? c.Medico.Id : 0;
                    conta.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                    conta.CRM = c.Medico?.NumeroConselho.ToString();
                    conta.Conselho = c.Medico?.Conselho?.Descricao;

                    conta.ConvenioId = c.Convenio != null ? c.Convenio.Id : 0;
                    conta.ConvenioNome = c.Convenio != null ? c.Convenio.NomeFantasia : string.Empty;
                    conta.PlanoId = c.Plano != null ? c.Plano.Id : 0;
                    conta.EmpresaId = c.Empresa != null ? c.Empresa.Id : 0;
                    conta.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                    conta.PlanoNome = c.Plano?.Descricao;
                    conta.GuiaId = c.Atendimento.FatGuia != null ? c.Atendimento.FatGuia.Id : 0;
                    conta.GuiaNumero = c.Atendimento.GuiaNumero;
                    conta.AtendimentoCodigo = c.Atendimento.Codigo;
                    conta.UnidadeOrganizacionalId = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Id : 0;
                    conta.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                    //conta.TipoLeitoId = c.TipoLeito != null ? c.TipoLeito.Id : 0;
                    //conta.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;
                    conta.TipoLeitoId = c.TipoAcomodacao != null ? c.TipoAcomodacao.Id : 0;
                    conta.TipoLeitoDescricao = c.TipoAcomodacao != null ? c.TipoAcomodacao.Descricao : string.Empty;
                    conta.DataIncio = c.DataInicio;
                    conta.DataFim = c.DataFim;
                    conta.DataPagamento = c.DataPagamento;
                    conta.ValidadeCarteira = c.ValidadeCarteira;
                    conta.DataAutorizacao = c.DataAutorizacao;
                    conta.DiaSerie1 = c.DiaSerie1;
                    conta.DiaSerie2 = c.DiaSerie2;
                    conta.DiaSerie3 = c.DiaSerie3;
                    conta.DiaSerie4 = c.DiaSerie4;
                    conta.DiaSerie5 = c.DiaSerie5;
                    conta.DiaSerie6 = c.DiaSerie6;
                    conta.DiaSerie7 = c.DiaSerie7;
                    conta.DiaSerie8 = c.DiaSerie8;
                    conta.DiaSerie9 = c.DiaSerie9;
                    conta.DiaSerie10 = c.DiaSerie10;
                    conta.DataEntrFolhaSala = c.DataEntrFolhaSala;
                    conta.DataEntrDescCir = c.DataEntrDescCir;
                    conta.DataEntrBolAnest = c.DataEntrBolAnest;
                    conta.DataEntrCDFilme = c.DataEntrCDFilme;
                    conta.DataValidadeSenha = c.DataValidadeSenha;
                    conta.IsAutorizador = c.IsAutorizador;
                    conta.TipoAtendimento = c.TipoAtendimento;

                    if (c.Status != null)
                    {
                        conta.StatusEntregaCodigo = c.Status.Codigo;
                        conta.StatusEntregaDescricao = c.Status.Descricao;
                    }

                    conta.Id = c.Id;
                }

                return conta;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarContasInput input)
        {
            try
            {
                var result = await Listar(input);
                var contas = result.Items;
                return _listarContasExcelExporter.ExportToFile(null/*contas.ToList()*/);
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<bool> VerificarCadastroPrecoItem(VerificarCadastroPrecoInput input)
        {
            bool possuiCadastro = false;
            long? tabelaId;

            try
            {
                if (input.configsConvenio == null)
                {
                    ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();
                    configConvenioInput.Filtro = input.conta.ConvenioId.ToString();
                    var configsConvenio = await _configConvenioAppService.ListarPorConvenio(configConvenioInput);
                    input.configsConvenio = configsConvenio.Items.ToList();
                }

                // Achando tabela adequada via analise combinatoria das 'configConvenio'

                // 1 - caso mais especifico
                var caso1 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId != null && x.ItemId != 0
                    );

                // 2
                var caso2 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId != null && x.ItemId != 0
                    );


                // 5
                var caso5 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );


                // 3
                var caso3 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId != null && x.ItemId != 0
                    );

                // 4
                var caso4 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId != null && x.ItemId != 0
                    );



                // 6
                var caso6 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 7
                var caso7 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 8
                var caso8 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId != null && x.SubGrupoId != 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 9
                var caso9 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 10
                var caso10 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 11
                var caso11 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 12
                var caso12 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId != null && x.GrupoId != 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 13
                var caso13 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId == null || x.GrupoId == 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 14
                var caso14 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId != null && x.EmpresaId != 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId == null || x.GrupoId == 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 15
                var caso15 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId != null && x.PlanoId != 0 &&
                         x.GrupoId == null || x.GrupoId == 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // 16 - caso mais generico
                var caso16 = input.configsConvenio.FirstOrDefault(
                    x => x.EmpresaId == null || x.EmpresaId == 0 &&
                         x.PlanoId == null || x.PlanoId == 0 &&
                         x.GrupoId == null || x.GrupoId == 0 &&
                         x.SubGrupoId == null || x.SubGrupoId == 0 &&
                         x.ItemId == null || x.ItemId == 0
                    );

                // Quanto menor o numero, maior a prioridade (ex: 1 eh mais prioritario que 20)
                CasoConfig[] casosConfig =
                {
                    new CasoConfig(1,  caso1),
                    new CasoConfig(2,  caso2),
                    new CasoConfig(3,  caso3),
                    new CasoConfig(4,  caso4),
                    new CasoConfig(5,  caso5),
                    new CasoConfig(6,  caso6),
                    new CasoConfig(7,  caso7),
                    new CasoConfig(8,  caso8),
                    new CasoConfig(9,  caso9),
                    new CasoConfig(10, caso10),
                    new CasoConfig(11, caso11),
                    new CasoConfig(12, caso12),
                    new CasoConfig(13, caso13),
                    new CasoConfig(14, caso14),
                    new CasoConfig(15, caso15),
                    new CasoConfig(16, caso16),
                };

                List<CasoConfig> casosConfigList = new List<CasoConfig>();

                foreach (var cc in casosConfig)
                {
                    if (cc.Config != null)
                        casosConfigList.Add(cc);
                }

                // Obtendo config com maior prioridade
                var config = casosConfigList.OrderBy(p => p.Prioridade).FirstOrDefault().Config;
                tabelaId = config?.TabelaId;

                // fim - via analise combinatoria

                // ======================= PRECO =========================

                // Obter preco vigente
                var listarParaTabelaInput = new ListarFaturamentoItensTabelaInput();
                listarParaTabelaInput.TabelaId = tabelaId.ToString();
                var precosPorTabela = AsyncHelper.RunSync(() => _tabelaItemAppService.ListarParaFatTabela(listarParaTabelaInput)).Items;
                var precosPorFatItem = precosPorTabela.Where(_ => _.ItemId == input.FatContaItemDto.FaturamentoItem.Id);
                var preco = precosPorFatItem
                    .Where(_ => _.VigenciaDataInicio <= DateTime.Now)
                    .OrderByDescending(_ => _.VigenciaDataInicio).FirstOrDefault();

                if (preco != null)
                    possuiCadastro = true;

                return possuiCadastro;
            }
            catch (Exception e)
            {
                e.ToString();
                return false;
            }
        }

        public async Task<PagedResultDto<FaturamentoContaItemViewModel>> ListarItensVM(ListarFaturamentoContaItensInput input)
        {
            var contarContaItens = 0;
            List<FaturamentoContaItem> contaItens;
            List<FaturamentoContaItemViewModel> contaItensDtos = new List<FaturamentoContaItemViewModel>();
            try
            {
                var query = _contaItemRepository
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.FaturamentoContaId.ToString() == input.Filtro)
                    .Include(i => i.FaturamentoItem)
                    .Include(i => i.FaturamentoConta)
                    .Include(i => i.FaturamentoItem.Grupo)
                    .Include(i => i.FaturamentoItem.Grupo.TipoGrupo)
                    .Include(i => i.Turno)
                    //.Include(t => t.TipoLeito)
                    .Include(t => t.TipoAcomodacao)
                    .Include(u => u.UnidadeOrganizacional)
                    .Include(u => u.FaturamentoConfigConvenio)
                    .Include(u => u.FaturamentoPacote.FaturamentoItem)
                    ;

                contarContaItens = await query.CountAsync();

                contaItens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();


                ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();

                if (contaItens.Count > 0)
                {
                    configConvenioInput.Filtro = contaItens[0].FaturamentoConta.ConvenioId.ToString();
                    configConvenioInput.ConvenioId = contaItens[0].FaturamentoConta.ConvenioId;
                    configConvenioInput.PlanoId = contaItens[0].FaturamentoConta.PlanoId;
                    configConvenioInput.EmpresaId = contaItens[0].FaturamentoConta.EmpresaId;

                    //configConvenioInput.GrupoId = contaItens[0].FaturamentoItem.GrupoId;
                    //configConvenioInput.SubGrupoId = contaItens[0].FaturamentoItem.SubGrupoId;
                    //configConvenioInput.ItemId = contaItens[0].FaturamentoItemId;

                }

                var configsConvenio = await _configConvenioAppService.ListarPorConvenio(configConvenioInput);
                // Fim - obtencao de config.convenio



                var configsPorEmpresa = configsConvenio.Items
                       .Where(c => c.EmpresaId == contaItens[0].FaturamentoConta.EmpresaId);

                // Filtrar por plano
                var configsPorPlano = configsPorEmpresa
                    .Where(x => x.PlanoId != null)
                    .Where(c => c.PlanoId == contaItens[0].FaturamentoConta.PlanoId);



                foreach (var item in contaItens)
                {
                    if (item.FaturamentoItem == null)
                    {
                        continue;
                    }

                    // Obtendo configuracoes do convenio para calculo de valor dos itens
                    //ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();
                    //configConvenioInput.Filtro = item.FaturamentoConta.ConvenioId.ToString();
                    //configConvenioInput.ConvenioId = item.FaturamentoConta.ConvenioId;
                    //configConvenioInput.PlanoId = item.FaturamentoConta.PlanoId;
                    //configConvenioInput.EmpresaId = item.FaturamentoConta.EmpresaId;


                    //var configsConvenio = await _configConvenioAppService.ListarPorConvenio(configConvenioInput);
                    // Fim - obtencao de config.convenio

                    var i = new FaturamentoContaItemViewModel();
                    input.CalculoContaItemInput.FatContaItemDto = new FaturamentoContaItemDto();
                    input.CalculoContaItemInput.FatContaItemDto.Id = item.Id;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem = new Itens.Dto.FaturamentoItemDto();
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.SubGrupoId = item.FaturamentoItem.SubGrupoId;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.GrupoId = item.FaturamentoItem.GrupoId;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.Id = item.FaturamentoItem.Id;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItemId = item.FaturamentoItem.Id;
                    input.CalculoContaItemInput.FatContaItemDto.MetragemFilme = item.MetragemFilme;

                    ///////////////////////////////////////////////////////////
                    // ============== CALCULO DE VALOR UNITARIO ===============
                    // Filtrar por empresa
                    //var configsPorEmpresa = configsConvenio.Items
                    //    .Where(c => c.EmpresaId == item.FaturamentoConta.EmpresaId);

                    //// Filtrar por plano
                    //var configsPorPlano = configsPorEmpresa
                    //    .Where(x => x.PlanoId != null)
                    //    .Where(c => c.PlanoId == item.FaturamentoConta.PlanoId);

                    input.CalculoContaItemInput.configsPorEmpresa = configsPorEmpresa.ToArray();
                    input.CalculoContaItemInput.configsPorPlano = configsPorPlano.ToArray();

                    // Valor manual ou calculado em tempo de execucao
                    i.IsValorItemManual = item.IsValorItemManual;
                    //if (i.IsValorItemManual)
                    //{
                    i.ValorItem = item.ValorItem;

                    //}
                    //else
                    //{
                    //    i.ValorUnitario = await _itemAppService.CalcularValorUnitarioContaItem(input.CalculoContaItemInput);
                    //    i.ValorItem = i.ValorUnitario;
                    //    i.ValorTotal = i.ValorUnitario * item.Qtde;
                    //}




                    //long? tabelaId = null;

                    //if (item.FaturamentoConfigConvenio != null)
                    //{
                    //    tabelaId = item.FaturamentoConfigConvenio.TabelaId;
                    //}
                    //if (!item.IsValorItemManual)
                    //{
                    //    var fatItemTabela = _faturamentoItemTabelaRepository.GetAll()
                    //                            .Where(w => w.ItemId == item.FaturamentoItemId
                    //                                    && w.TabelaId == tabelaId)
                    //                            .FirstOrDefault();
                    //    if (fatItemTabela != null)
                    //    {
                    //        i.ValorItem = fatItemTabela.Preco;
                    //    }
                    //    else
                    //    {
                    //        i.ValorItem = 0;
                    //    }
                    //}


                    i.ValorTotal = i.ValorItem * item.Qtde;


                    i.Id = item.Id;
                    i.Grupo = item.FaturamentoItem.Grupo != null ? item.FaturamentoItem.Grupo.Descricao : string.Empty;
                    i.Tipo = item.FaturamentoItem.Grupo != null ? (item.FaturamentoItem.Grupo.TipoGrupo != null ? item.FaturamentoItem.Grupo.TipoGrupo.Descricao : string.Empty) : string.Empty;
                    i.Descricao = item.FaturamentoItem.Descricao;
                    i.FaturamentoItemId = item.FaturamentoItemId;
                    i.FaturamentoItemDescricao = item.FaturamentoItem != null ? item.FaturamentoItem.Descricao : string.Empty;
                    i.FaturamentoContaId = item.FaturamentoContaId;
                    i.Data = item.Data;
                    i.Qtde = item.Qtde;
                    i.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                    i.UnidadeOrganizacionalDescricao = item.UnidadeOrganizacional != null ? item.UnidadeOrganizacional.Descricao : string.Empty;
                    i.TerceirizadoId = item.TerceirizadoId;
                    i.TerceirizadoDescricao = item.Terceirizado != null ? item.Terceirizado.Codigo : string.Empty;
                    i.CentroCustoId = item.CentroCustoId;
                    i.CentroCustoDescricao = item.CentroCusto != null ? item.CentroCusto.Descricao : string.Empty;
                    i.TurnoId = item.TurnoId;
                    i.TurnoDescricao = item.Turno != null ? item.Turno.Descricao : string.Empty;
                    //i.TipoLeitoId = item.TipoLeitoId;
                    //i.TipoLeitoDescricao = item.TipoLeito != null ? item.TipoLeito.Descricao : string.Empty;
                    i.TipoLeitoId = item.TipoAcomodacaoId;
                    i.TipoLeitoDescricao = item.TipoAcomodacao != null ? item.TipoAcomodacao.Descricao : string.Empty;
                    i.ValorTemp = item.ValorTemp;
                    i.MedicoId = item.MedicoId;
                    i.MedicoNome = item.Medico != null ? item.Medico.NomeCompleto : string.Empty;
                    i.IsMedCredenciado = item.IsMedCredenciado;
                    i.IsGlosaMedico = item.IsGlosaMedico;
                    i.MedicoEspecialidadeId = item.MedicoEspecialidadeId;
                    i.MedicoEspecialidadeNome = item.MedicoEspecialidade != null ? item.MedicoEspecialidade.Especialidade.Nome : string.Empty;
                    i.FaturamentoContaKitId = item.FaturamentoContaKitId;
                    i.IsCirurgia = item.IsCirurgia;
                    i.ValorAprovado = item.ValorAprovado;
                    i.ValorTaxas = item.ValorTaxas;
                    i.HMCH = item.HMCH;
                    i.ValorFilme = item.ValorFilme;
                    i.ValorFilmeAprovado = item.ValorFilmeAprovado;
                    i.ValorCOCH = item.ValorCOCH;
                    i.ValorCOCHAprovado = item.ValorCOCHAprovado;
                    i.Percentual = item.Percentual;
                    i.IsInstrCredenciado = item.IsInstrCredenciado;
                    i.ValorTotalRecuperado = item.ValorTotalRecuperado;
                    i.ValorTotalRecebido = item.ValorTotalRecebido;
                    i.MetragemFilme = item.MetragemFilme;
                    i.MetragemFilmeAprovada = item.MetragemFilmeAprovada;
                    i.COCH = item.COCH;
                    i.COCHAprovado = item.COCHAprovado;
                    // STATUSNOVO ALTERAR          i.StatusEntrega = item.StatusEntrega;
                    i.IsRecuperaMedico = item.IsRecuperaMedico;
                    i.IsAux1Credenciado = item.IsAux1Credenciado;
                    i.IsRecebeAuxiliar1 = item.IsRecebeAuxiliar1;
                    i.IsGlosaAuxiliar1 = item.IsGlosaAuxiliar1;
                    i.IsRecuperaAuxiliar1 = item.IsRecuperaAuxiliar1;
                    i.IsAux2Credenciado = item.IsAux2Credenciado;
                    i.IsRecebeAuxiliar2 = item.IsRecebeAuxiliar2;
                    i.IsGlosaAuxiliar2 = item.IsGlosaAuxiliar2;
                    i.IsRecuperaAuxiliar2 = item.IsRecuperaAuxiliar2;
                    i.IsAux3Credenciado = item.IsAux3Credenciado;
                    i.IsRecebeAuxiliar3 = item.IsRecebeAuxiliar3;
                    i.IsGlosaAuxiliar3 = item.IsGlosaAuxiliar3;
                    i.IsRecuperaAuxiliar3 = item.IsRecuperaAuxiliar3;
                    i.IsRecebeInstrumentador = item.IsRecebeInstrumentador;
                    i.IsGlosaInstrumentador = item.IsGlosaInstrumentador;
                    i.IsRecuperaInstrumentador = item.IsRecuperaInstrumentador;
                    i.Observacao = item.Observacao;
                    i.QtdeRecuperada = item.QtdeRecuperada;
                    i.QtdeAprovada = item.QtdeAprovada;
                    i.QtdeRecebida = item.QtdeRecebida;
                    i.ValorMoedaAprovado = item.ValorMoedaAprovado;
                    i.SisMoedaId = item.SisMoedaId;
                    i.SisMoedaNome = item.SisMoeda != null ? item.SisMoeda.Descricao : string.Empty;
                    i.DataAutorizacao = item.DataAutorizacao;
                    i.SenhaAutorizacao = item.SenhaAutorizacao;
                    i.NomeAutorizacao = item.NomeAutorizacao;
                    i.ObsAutorizacao = item.ObsAutorizacao;
                    i.HoraIncio = item.HoraIncio;
                    i.HoraFim = item.HoraFim;
                    i.ViaAcesso = item.ViaAcesso;
                    i.Tecnica = item.Tecnica;
                    i.ClinicaId = item.ClinicaId;
                    i.FornecedorId = item.FornecedorId;
                    i.FornecedorNome = item.Fornecedor != null ? item.Fornecedor.Descricao : string.Empty;
                    i.NumeroNF = item.NumeroNF;
                    i.IsImportaEstoque = item.IsImportaEstoque;

                    i.IsPacote = item.FaturamentoItem.Grupo.TipoGrupoId == 4;

                    if (item.FaturamentoItem.Grupo.TipoGrupoId != 4)
                    {
                        i.Pacote = item.FaturamentoPacote?.FaturamentoItem?.Descricao;
                    }
                    contaItensDtos.Add(i);
                }

                return new PagedResultDto<FaturamentoContaItemViewModel>(contarContaItens, contaItensDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<float> CalcularTotalConta(CalcularTotalContaInput input)
        {
            float total = 0f;

            foreach (var i in input.Itens)
            {
                total += i.ValorTotal;
            }

            return total;
        }

        // Entrega de contas
        public async Task<PagedResultDto<ContaMedicaViewModel>> ListarNaoConferidas(ListarContasInput input)
        {
            var contarContas = 0;
            List<FaturamentoConta> contas;
            List<ContaMedicaViewModel> contasDtos = new List<ContaMedicaViewModel>();
            try
            {
                string[] per = null;
                DateTime periodoInicio = DateTime.Now;
                DateTime periodoFim = DateTime.Now;

                if (!input.IgnoraData)
                {
                    input.Periodo = input.Periodo.Replace(" ", string.Empty);
                    per = input.Periodo.Split('-');
                    periodoInicio = Convert.ToDateTime(per[0]);
                    periodoFim = Convert.ToDateTime(per[1]);
                    periodoFim = periodoFim.AddHours(23);
                    periodoFim = periodoFim.AddMinutes(59);
                    periodoFim = periodoFim.AddSeconds(59);
                }


                if (input.IsEmergencia)
                {
                    var query = _contaRepository
                   .GetAll()

                   .WhereIf(!input.IgnoraData && input.IsEmergencia, x => x.Atendimento.DataRegistro >= periodoInicio && x.Atendimento.DataRegistro <= periodoFim)

                   .Include(i => i.Empresa)
                   .Include(i => i.Convenio)
                   .Include(i => i.Convenio.SisPessoa)
                   .Include(i => i.Medico)
                   .Include(i => i.Medico.SisPessoa)
                   .Include(i => i.Plano)
                   .Include(i => i.Atendimento)
                   .Include(i => i.Atendimento.Paciente)
                   .Include(a => a.Atendimento.Paciente.SisPessoa)
                   .Include(i => i.Atendimento.Guia)
                   .Include(i => i.Status)
                   .Where(i => i.Status != null && (i.Status.Codigo == 1.ToString() || i.Status.Codigo == 5.ToString())) // codigo 1 == "Status inicial (nao conferida) - atribuido no Seed"; Status 5 = Pendente
                   .WhereIf(!string.IsNullOrEmpty(input.EmpresaId.ToString()), e => e.EmpresaId.ToString() == input.EmpresaId.ToString())
                   .WhereIf(!string.IsNullOrEmpty(input.ConvenioId), e => e.ConvenioId.ToString() == input.ConvenioId)
                   .WhereIf(!string.IsNullOrEmpty(input.PacienteId), e => e.Atendimento.PacienteId.ToString() == input.PacienteId)
                   .WhereIf(!string.IsNullOrEmpty(input.MedicoId), e => e.MedicoId.ToString() == input.MedicoId)
                   .WhereIf(input.IsEmergencia == true, e => e.Atendimento.IsAmbulatorioEmergencia == true)
                   .WhereIf(input.IsInternacao == true, e => e.Atendimento.IsInternacao == true)
                   ;

                    contarContas = await query.CountAsync();

                    contas = await query
                        .AsNoTracking()
                        .ToListAsync();
                }
                else // IsINternacao
                {
                    List<FaturamentoContaItem> itens = new List<FaturamentoContaItem>();
                    if (!input.IgnoraData)
                    {
                        itens = _contaItemRepository.GetAll()
                           .Where(x => x.Data >= periodoInicio && x.Data <= periodoFim)
                           .ToList();
                    }

                    var query = _contaRepository
                   .GetAll()

                   //.WhereIf(!input.IgnoraData && input.IsEmergencia,
                   //    x => x.Atendimento.DataRegistro >= periodoInicio &&
                   //         x.Atendimento.DataRegistro <= periodoFim
                   //)

                   //.WhereIf(!input.IgnoraData && input.IsInternacao,
                   //    x => FaturamentoConta.PossuiItensNoPeriodo(x, itens, periodoInicio, periodoFim)
                   //)

                   .Include(i => i.Empresa)
                   .Include(i => i.Convenio)
                   .Include(i => i.Convenio.SisPessoa)
                   .Include(i => i.Medico)
                   .Include(i => i.Medico.SisPessoa)
                   .Include(i => i.Plano)
                   .Include(i => i.Atendimento)
                   .Include(i => i.Atendimento.Paciente)
                   .Include(a => a.Atendimento.Paciente.SisPessoa)
                   .Include(i => i.Atendimento.Guia)
                   .Include(i => i.Status)
                   .Where(i => i.Status != null && (i.Status.Codigo == 1.ToString() || i.Status.Codigo == 5.ToString())) // codigo 1 == "Status inicial (nao conferida) - atribuido no Seed"; Status 5 = Pendente
                   .WhereIf(!string.IsNullOrEmpty(input.EmpresaId.ToString()), e => e.EmpresaId.ToString() == input.EmpresaId.ToString())
                   .WhereIf(!string.IsNullOrEmpty(input.ConvenioId), e => e.ConvenioId.ToString() == input.ConvenioId)
                   .WhereIf(!string.IsNullOrEmpty(input.PacienteId), e => e.Atendimento.PacienteId.ToString() == input.PacienteId)
                   .WhereIf(!string.IsNullOrEmpty(input.MedicoId), e => e.MedicoId.ToString() == input.MedicoId)
                   .WhereIf(input.IsEmergencia == true, e => e.Atendimento.IsAmbulatorioEmergencia == true)
                   .WhereIf(input.IsInternacao == true, e => e.Atendimento.IsInternacao == true)
                   ;

                    contarContas = await query.CountAsync();

                    var contasPre = await query
                        .AsNoTracking()
                        .ToListAsync();

                    contas = new List<FaturamentoConta>();// await query.AsNoTracking().ToListAsync();

                    foreach (var c in contasPre)
                    {
                        if (!input.IgnoraData)
                        {
                            if (FaturamentoConta.PossuiItensNoPeriodo(c, itens, periodoInicio, periodoFim))
                            {
                                contas.Add(c);
                            }
                        }
                        else
                        {
                            contas.Add(c);
                        }
                    }

                }

                foreach (var c in contas)
                {
                    var conta = new ContaMedicaViewModel();

                    conta.Id = c.Id;
                    conta.PacienteNome = c.Atendimento.Paciente.SisPessoa?.NomeCompleto;
                    conta.Matricula = c.Matricula;
                    conta.CodDependente = c.CodDependente;
                    conta.NumeroGuia = c.NumeroGuia;
                    conta.Titular = c.Titular;
                    conta.GuiaOperadora = c.GuiaOperadora;
                    conta.GuiaPrincipal = c.GuiaPrincipal;
                    conta.Observacao = c.Observacao;
                    conta.SenhaAutorizacao = c.SenhaAutorizacao;
                    conta.IdentAcompanhante = c.IdentAcompanhante;

                    if (c.Atendimento != null)
                    {
                        conta.AtendimentoCodigo = c.Atendimento.Codigo;
                        conta.PlanoNome = c.Atendimento.Plano != null ? c.Atendimento.Plano.Descricao : string.Empty;
                        conta.GuiaNumero = c.Atendimento.GuiaNumero;
                        if (c.Atendimento.Paciente != null)
                        {
                            conta.PacienteNome = c.Atendimento.Paciente.NomeCompleto;
                        }
                    }

                    conta.PacienteId = c.PacienteId;
                    conta.MedicoId = c.MedicoId;
                    conta.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                    conta.ConvenioId = c.ConvenioId;
                    conta.ConvenioNome = c.Convenio != null ? c.Convenio.NomeFantasia : string.Empty;
                    conta.PlanoId = c.PlanoId;
                    conta.GuiaId = c.GuiaId;
                    conta.EmpresaId = c.EmpresaId;
                    conta.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                    conta.AtendimentoId = c.AtendimentoId;
                    conta.UnidadeOrganizacionalId = c.UnidadeOrganizacionalId;
                    conta.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                    //conta.TipoLeitoId = c.TipoLeitoId;
                    //conta.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;

                    conta.TipoLeitoId = c.TipoAcomodacaoId;
                    conta.TipoLeitoDescricao = c.TipoAcomodacao != null ? c.TipoAcomodacao.Descricao : string.Empty;
                    conta.DataInicio = c.DataInicio;
                    conta.DataFim = c.DataFim;
                    conta.DataPagamento = c.DataPagamento;
                    conta.ValidadeCarteira = c.ValidadeCarteira;
                    conta.DataAutorizacao = c.DataAutorizacao;
                    conta.DiaSerie1 = c.DiaSerie1;
                    conta.DiaSerie2 = c.DiaSerie2;
                    conta.DiaSerie3 = c.DiaSerie3;
                    conta.DiaSerie4 = c.DiaSerie4;
                    conta.DiaSerie5 = c.DiaSerie5;
                    conta.DiaSerie6 = c.DiaSerie6;
                    conta.DiaSerie7 = c.DiaSerie7;
                    conta.DiaSerie8 = c.DiaSerie8;
                    conta.DiaSerie9 = c.DiaSerie9;
                    conta.DiaSerie10 = c.DiaSerie10;
                    conta.DataEntrFolhaSala = c.DataEntrFolhaSala;
                    conta.DataEntrDescCir = c.DataEntrDescCir;
                    conta.DataEntrBolAnest = c.DataEntrBolAnest;
                    conta.DataEntrCDFilme = c.DataEntrCDFilme;
                    conta.DataValidadeSenha = c.DataValidadeSenha;
                    conta.IsAutorizador = c.IsAutorizador;
                    conta.TipoAtendimento = c.TipoAtendimento;
                    conta.StatusEntregaCodigo = c.Status?.Codigo;
                    conta.StatusEntregaDescricao = c.Status?.Descricao;
                    conta.StatusEntregaCor = c.Status?.Cor;

                    contasDtos.Add(conta);
                }

                return new PagedResultDto<ContaMedicaViewModel>(contarContas, contasDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<PagedResultDto<ContaMedicaViewModel>> ListarParaEntrega(ListarContasInput input)
        {
            var contarContas = 0;
            List<FaturamentoConta> contas;
            List<ContaMedicaViewModel> contasDtos = new List<ContaMedicaViewModel>();
            try
            {
                var query = _contaRepository
                    .GetAll()
                    .Include(i => i.Empresa)
                    .Include(i => i.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .Include(i => i.Medico)
                    .Include(i => i.Medico.SisPessoa)
                    .Include(i => i.Plano)
                    .Include(i => i.Atendimento)
                    .Include(i => i.Atendimento.Paciente)
                    .Include(a => a.Atendimento.Paciente.SisPessoa)
                    .Include(i => i.Atendimento.Guia)
                    .Include(s => s.Status)
                    .Include(s => s.UsuarioConferencia)

                    .WhereIf(input.ApenasConferidas, e => e.StatusId == 2)
                    .Where(i => i.StatusId != 3 && i.StatusId != 4)
                    //.WhereIf(input.IsEmergencia, e => e.Atendimento.IsAmbulatorioEmergencia)
                    //.WhereIf(input.IsInternacao, e => e.Atendimento.IsInternacao)
                    .WhereIf(!string.IsNullOrEmpty(input.UsuarioId), e => e.UsuarioConferenciaId.ToString() == input.UsuarioId)
                    .WhereIf(!string.IsNullOrEmpty(input.EmpresaId.ToString()), e => e.EmpresaId.ToString() == input.EmpresaId.ToString())
                    .WhereIf(!string.IsNullOrEmpty(input.ConvenioId), e => e.ConvenioId.ToString() == input.ConvenioId)
                    .WhereIf(!string.IsNullOrEmpty(input.PacienteId), e => e.Atendimento.PacienteId.ToString() == input.PacienteId)
                    .WhereIf(!string.IsNullOrEmpty(input.MedicoId), e => e.MedicoId.ToString() == input.MedicoId)

                    .WhereIf(input.IsEmergencia == true, e => e.Atendimento.IsAmbulatorioEmergencia == true && e.Atendimento.IsInternacao == false)
                    .WhereIf(input.IsInternacao == true, e => e.Atendimento.IsInternacao == true && e.Atendimento.IsAmbulatorioEmergencia == false)
                    ;

                contarContas = await query.CountAsync();

                contas = await query
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var c in contas)
                {
                    var conta = new ContaMedicaViewModel();
                    conta.Id = c.Id;
                    conta.UsuarioConferenciaNome = c.UsuarioConferencia?.Name + " " + c.UsuarioConferencia?.Surname;
                    conta.Matricula = c.Atendimento.Matricula;
                    conta.CodDependente = c.CodDependente;
                    conta.NumeroGuia = c.NumeroGuia;
                    conta.Titular = c.Titular;
                    conta.GuiaOperadora = c.GuiaOperadora;
                    conta.GuiaPrincipal = c.GuiaPrincipal;
                    conta.Observacao = c.Observacao;
                    conta.SenhaAutorizacao = c.SenhaAutorizacao;
                    conta.IdentAcompanhante = c.IdentAcompanhante;

                    if (c.Atendimento != null)
                    {
                        conta.AtendimentoCodigo = c.Atendimento.Codigo;
                        conta.PlanoNome = c.Atendimento.Plano != null ? c.Atendimento.Plano.Descricao : string.Empty;
                        conta.GuiaNumero = c.Atendimento.GuiaNumero;
                        if (c.Atendimento.Paciente != null)
                        {
                            conta.PacienteNome = c.Atendimento.Paciente.NomeCompleto;
                        }
                    }

                    conta.PacienteId = c.PacienteId;
                    conta.MedicoId = c.MedicoId;
                    conta.MedicoNome = c.Medico != null ? c.Medico.NomeCompleto : string.Empty;
                    conta.ConvenioId = c.ConvenioId;
                    conta.ConvenioNome = c.Convenio != null && c.Convenio.SisPessoa != null ? c.Convenio.SisPessoa.NomeFantasia : string.Empty;
                    conta.PlanoId = c.PlanoId;
                    conta.GuiaId = c.GuiaId;
                    conta.EmpresaId = c.EmpresaId;
                    conta.EmpresaNome = c.Empresa != null ? c.Empresa.NomeFantasia : string.Empty;
                    conta.AtendimentoId = c.AtendimentoId;
                    conta.UnidadeOrganizacionalId = c.UnidadeOrganizacionalId;
                    conta.UnidadeOrganizacionalNome = c.UnidadeOrganizacional != null ? c.UnidadeOrganizacional.Descricao : string.Empty;
                    //conta.TipoLeitoId = c.TipoLeitoId;
                    //conta.TipoLeitoDescricao = c.TipoLeito != null ? c.TipoLeito.Descricao : string.Empty;
                    conta.TipoLeitoId = c.TipoAcomodacaoId;
                    conta.TipoLeitoDescricao = c.TipoAcomodacao != null ? c.TipoAcomodacao.Descricao : string.Empty;
                    conta.DataInicio = c.DataInicio;
                    conta.DataFim = c.DataFim;
                    conta.DataPagamento = c.DataPagamento;
                    conta.ValidadeCarteira = c.ValidadeCarteira;
                    conta.DataAutorizacao = c.DataAutorizacao;
                    conta.DiaSerie1 = c.DiaSerie1;
                    conta.DiaSerie2 = c.DiaSerie2;
                    conta.DiaSerie3 = c.DiaSerie3;
                    conta.DiaSerie4 = c.DiaSerie4;
                    conta.DiaSerie5 = c.DiaSerie5;
                    conta.DiaSerie6 = c.DiaSerie6;
                    conta.DiaSerie7 = c.DiaSerie7;
                    conta.DiaSerie8 = c.DiaSerie8;
                    conta.DiaSerie9 = c.DiaSerie9;
                    conta.DiaSerie10 = c.DiaSerie10;
                    conta.DataEntrFolhaSala = c.DataEntrFolhaSala;
                    conta.DataEntrDescCir = c.DataEntrDescCir;
                    conta.DataEntrBolAnest = c.DataEntrBolAnest;
                    conta.DataEntrCDFilme = c.DataEntrCDFilme;
                    conta.DataValidadeSenha = c.DataValidadeSenha;
                    conta.IsAutorizador = c.IsAutorizador;
                    conta.TipoAtendimento = c.TipoAtendimento;
                    conta.StatusEntregaCodigo = c.Status?.Codigo;
                    conta.StatusEntregaDescricao = c.Status?.Descricao;
                    conta.StatusEntregaCor = c.Status?.Cor;
                    contasDtos.Add(conta);
                }

                return new PagedResultDto<ContaMedicaViewModel>(contarContas, contasDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<float> ObterValorTotalConta(long contaId)
        {
            ListarFaturamentoContaItensInput input = new ListarFaturamentoContaItensInput();
            var contarContaItens = 0;
            List<FaturamentoContaItem> contaItens;
            List<FaturamentoContaItemViewModel> contaItensDtos = new List<FaturamentoContaItemViewModel>();
            try
            {
                var query = _contaItemRepository
                    .GetAll()
                    .Where(e => e.FaturamentoContaId == contaId)
                    .Include(i => i.FaturamentoItem)
                    .Include(i => i.FaturamentoConta)
                    ;

                contaItens = query.ToList();

                ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();

                if (contaItens.Count > 0)
                {
                    configConvenioInput.Filtro = contaItens[0].FaturamentoConta.ConvenioId.ToString();
                    configConvenioInput.ConvenioId = contaItens[0].FaturamentoConta.ConvenioId;
                    configConvenioInput.PlanoId = contaItens[0].FaturamentoConta.PlanoId;
                    configConvenioInput.EmpresaId = contaItens[0].FaturamentoConta.EmpresaId;

                }

                var configsConvenio = await _configConvenioAppService.ListarPorConvenio(configConvenioInput);
                // Fim - obtencao de config.convenio



                var configsPorEmpresa = configsConvenio.Items
                       .Where(c => c.EmpresaId == contaItens[0].FaturamentoConta.EmpresaId);

                // Filtrar por plano
                var configsPorPlano = configsPorEmpresa
                    .Where(x => x.PlanoId != null)
                    .Where(c => c.PlanoId == contaItens[0].FaturamentoConta.PlanoId);



                foreach (var item in contaItens)
                {
                    if (item.FaturamentoItem == null)
                    {
                        continue;
                    }

                    // Obtendo configuracoes do convenio para calculo de valor dos itens
                    //ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();
                    //configConvenioInput.Filtro = item.FaturamentoConta.ConvenioId.ToString();
                    //configConvenioInput.ConvenioId = item.FaturamentoConta.ConvenioId;
                    //configConvenioInput.PlanoId = item.FaturamentoConta.PlanoId;
                    //configConvenioInput.EmpresaId = item.FaturamentoConta.EmpresaId;


                    //var configsConvenio = await _configConvenioAppService.ListarPorConvenio(configConvenioInput);
                    // Fim - obtencao de config.convenio

                    var i = new FaturamentoContaItemViewModel();

                    input.CalculoContaItemInput = new CalculoContaItemInput();

                    input.CalculoContaItemInput.FatContaItemDto = new FaturamentoContaItemDto();
                    input.CalculoContaItemInput.FatContaItemDto.Id = item.Id;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem = new Itens.Dto.FaturamentoItemDto();
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.SubGrupoId = item.FaturamentoItem.SubGrupoId;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.GrupoId = item.FaturamentoItem.GrupoId;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.Id = item.FaturamentoItem.Id;
                    input.CalculoContaItemInput.FatContaItemDto.FaturamentoItemId = item.FaturamentoItem.Id;
                    input.CalculoContaItemInput.FatContaItemDto.MetragemFilme = item.MetragemFilme;

                    ///////////////////////////////////////////////////////////
                    // ============== CALCULO DE VALOR UNITARIO ===============
                    // Filtrar por empresa
                    //var configsPorEmpresa = configsConvenio.Items
                    //    .Where(c => c.EmpresaId == item.FaturamentoConta.EmpresaId);

                    //// Filtrar por plano
                    //var configsPorPlano = configsPorEmpresa
                    //    .Where(x => x.PlanoId != null)
                    //    .Where(c => c.PlanoId == item.FaturamentoConta.PlanoId);

                    input.CalculoContaItemInput.configsPorEmpresa = configsPorEmpresa.ToArray();
                    input.CalculoContaItemInput.configsPorPlano = configsPorPlano.ToArray();

                    // Valor manual ou calculado em tempo de execucao
                    i.IsValorItemManual = item.IsValorItemManual;
                    if (i.IsValorItemManual)
                    {
                        i.ValorItem = item.ValorItem;
                        i.ValorTotal = i.ValorItem * item.Qtde;
                    }
                    else
                    {
                        input.CalculoContaItemInput.conta = new ContaCalculoItem();

                        input.CalculoContaItemInput.conta.EmpresaId = (long)contaItens[0].FaturamentoConta.EmpresaId;
                        input.CalculoContaItemInput.conta.ConvenioId = (long)contaItens[0].FaturamentoConta.ConvenioId;
                        input.CalculoContaItemInput.conta.PlanoId = (long)contaItens[0].FaturamentoConta.PlanoId;


                        i.ValorUnitario = await _itemAppService.CalcularValorUnitarioContaItem(input.CalculoContaItemInput);
                        i.ValorItem = i.ValorUnitario;
                        i.ValorTotal = i.ValorUnitario * item.Qtde;
                    }





                    contaItensDtos.Add(i);
                }

                return 0;// contaItensDtos.Sum(s => s.ValorTotal);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }





            return 0f;
        }

        public async Task<float> ObterValorContaRegistrado(long contaId)
        {

            float valorTotal = 0;
            try
            {

                var itensConta = _contaItemRepository.GetAll()
                                                     .Include(i => i.FaturamentoConfigConvenio)
                                                     .Where(w => w.FaturamentoContaId == contaId
                                                              && (w.FaturamentoPacoteId == null || w.FaturamentoItem.Grupo.TipoGrupoId == 4))
                                                     .ToList();


                foreach (var item in itensConta)
                {
                    //var tabelaId = item.FaturamentoConfigConvenio?.TabelaId;


                    //var tabelaItem = _faturamentoItemTabelaRepository.GetAll()
                    //                                                 .Where(w => w.TabelaId == tabelaId
                    //                                                            && w.ItemId == item.FaturamentoItemId)
                    //                                                 .FirstOrDefault();

                    //if (tabelaItem != null)
                    //{
                    //    valorTotal += tabelaItem.Preco;
                    //}

                    valorTotal += (item.ValorItem * item.Qtde);

                }
            }
            catch (Exception)
            {

            }

            return valorTotal;
        }

        public async Task RecalcularValores(long contaId)
        {
            var itensConta = _contaItemRepository.GetAll()
                                                 .Where(w => w.FaturamentoContaId == contaId)
                                                 .ToList();

            foreach (var item in itensConta)
            {
                var valorTabela = await _itemAppService.CalcularValorItemFaturamento(contaId, (long)item.FaturamentoItemId);
                item.ValorItem = valorTabela.Valor;
                item.FaturamentoItemCobradoId = valorTabela.FaturamentoItemCobradoId;
            }
        }
        #endregion

    }

    public class ResumoContaDto
    {
        public IEnumerable<ResumoContaGrupoDto> ResumoContaGrupo { get; set; }
    }

    public class ResumoContaGrupoDto
    {
        public ResumoContaGrupoDto()
        {

        }

        public ResumoContaGrupoDto(string grupoDescricao, long? grupoId, long? grupoOrdem, IEnumerable<ResumoContaItemDto> resumoContaItem)
        {
            GrupoDescricao = grupoDescricao;
            GrupoId = grupoId;
            GrupoOrdem = grupoOrdem;
            ResumoContaSubGrupo = resumoContaItem.GroupBy(x => new { x.UnidadeOrganizacionalId, x.UnidadeOrganizacionalDescricao })
                .Select(x => new ResumoContaSubGrupoDto(x.Key.UnidadeOrganizacionalDescricao, x.Key.UnidadeOrganizacionalId, x.ToList()));
        }

        public string GrupoDescricao { get; set; }
        public long? GrupoId { get; set; }
        public long? GrupoOrdem { get; set; }
        public IEnumerable<ResumoContaSubGrupoDto> ResumoContaSubGrupo { get; set; }

    }

    public class ResumoContaSubGrupoDto
    {
        public ResumoContaSubGrupoDto()
        {

        }

        public ResumoContaSubGrupoDto(string subGrupoDescricao, long? subGrupoId, IEnumerable<ResumoContaItemDto> resumoContaItem)
        {
            SubGrupoDescricao = subGrupoDescricao;
            SubGrupoId = subGrupoId;
            ResumoContaItem = resumoContaItem.OrderBy(x => x.FatItemDescricao);
        }

        public string SubGrupoDescricao { get; set; }
        public long? SubGrupoId { get; set; }
        public IEnumerable<ResumoContaItemDto> ResumoContaItem { get; set; }

    }


    public class ResumoContaPacoteDto
    {
        public long Id { get; set; }

        public DateTimeOffset? DataInicial { get; set; }
        public DateTimeOffset? DataFinal { get; set; }

        public string FatItemDescricao { get; set; }

        public long? GrupoId { get; set; }
        public string GrupoDescricao { get; set; }

        public long? SubGrupoId { get; set; }
        public string SubGrupoDescricao { get; set; }

        public long? GrupoOrdem { get; set; }

        public string Codigo { get; set; }
        public string CodAmb { get; set; }
        public string CodCbhpm { get; set; }
        public string CodTuss { get; set; }

        public float Qtde { get; set; }

        public ResumoContaPacoteDto()
        {

        }

        public ResumoContaPacoteDto(long id, DateTimeOffset? dataInicial, DateTimeOffset? dataFinal, float qtde, string fatItemDescricao, long? grupoId, string grupoDescricao, long? subGrupoId, string subGrupoDescricao, long? grupoOrdem, string codigo, string codAmb, string codCbhpm, string codTuss)
        {
            Id = id;
            Qtde = qtde;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            FatItemDescricao = fatItemDescricao;
            GrupoId = grupoId;
            GrupoDescricao = grupoDescricao;
            SubGrupoId = subGrupoId;
            SubGrupoDescricao = subGrupoDescricao;
            GrupoOrdem = grupoOrdem;
            Codigo = codigo;
            CodAmb = codAmb;
            CodCbhpm = codCbhpm;
            CodTuss = codTuss;
        }

    }

    public class ResumoContaItemDto
    {
        public DateTimeOffset? DataInicial { get; set; }
        public DateTimeOffset? DataFinal { get; set; }
        public long? Id { get; set; }

        public long? FatItemId { get; set; }
        public long? GrupoId { get; set; }
        public string GrupoDescricao { get; set; }

        public long? GrupoOrdem { get; set; }

        public long? SubGrupoId { get; set; }
        public string SubGrupoDescricao { get; set; }

        public string FatContaItemDescricao { get; set; }

        public string FatContaItemCodigo { get; set; }

        public string FatItemDescricao { get; set; }
        public string FatItemDescricaoTuss { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public string UnidadeOrganizacionalDescricao { get; set; }
        public string Codigo { get; set; }
        public string CodAmb { get; set; }
        public string CodCbhpm { get; set; }
        public string CodTuss { get; set; }
        public float Qtde { get; set; }

        [JsonIgnore]
        public string ResumoDetalhamentoJSON { get; set; }

        public ResumoDetalhamento ResumoDetalhamento => !string.IsNullOrEmpty(ResumoDetalhamentoJSON) ? JsonConvert.DeserializeObject<ResumoDetalhamento>(ResumoDetalhamentoJSON) : null;

        public long? FaturamentoContaKitId { get; set; }
        public long? FatPacoteId { get; set; }

        public float ValorMoeda { get; internal set; }
        public float ValorTaxas { get; internal set; }
        public float ValorPorte { get; internal set; }
        public float ValorHMCH { get; internal set; }
        public float ValorCOCH { get; internal set; }
        public float ValorFilme { get; internal set; }
        public double Percentual { get; internal set; }
        public float ValorItem { get; internal set; }

        public string Observacao { get; set; }
        public float MetragemFilme { get; internal set; }
        public float COCH { get; internal set; }
        public float HMCH { get; internal set; }
        public string CodigoMoeda { get; internal set; }

        public ResumoContaItemDto()
        {

        }

        public ResumoContaItemDto(DateTimeOffset? dataInicial, DateTimeOffset? dataFinal,
            long? id, long? grupoId, string grupoDescricao, long? grupoOrdem, long? subGrupoId, string subGrupoDescricao,
            string fatItemDescricao, string codigo, string codAmb, string codCbhpm, string codTuss, float qtde, string resumoDetalhamentoJSON, long? faturamentoContaKitId, long? fatPacoteId)
        {
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            Id = id;
            FatItemDescricao = fatItemDescricao;
            GrupoId = grupoId;
            GrupoDescricao = grupoDescricao;
            GrupoOrdem = grupoOrdem;
            SubGrupoId = subGrupoId;
            SubGrupoDescricao = subGrupoDescricao;
            FatItemDescricao = fatItemDescricao;
            Codigo = codigo;
            CodAmb = codAmb;
            CodCbhpm = codCbhpm;
            CodTuss = codTuss;
            Qtde = qtde;
            ResumoDetalhamentoJSON = resumoDetalhamentoJSON;
            FaturamentoContaKitId = faturamentoContaKitId;
            FatPacoteId = fatPacoteId;
        }

        public static ResumoContaItemDto Mapear(ResumoContaPacoteDto pacoteDto, IEnumerable<ResumoContaItemDto> itemsPacote)
        {

            var resumoDetalhamento = new ResumoDetalhamento();
            var resumos = itemsPacote.Where(x => !x.ResumoDetalhamentoJSON.IsNullOrEmpty()).Select(x =>{ return JsonConvert.DeserializeObject<ResumoDetalhamento>(x.ResumoDetalhamentoJSON); });
            resumoDetalhamento.ValorTotal = resumos.Sum(x=> x.ValorTotal);
            resumoDetalhamento.Valor = resumos.Sum(x => x.Valor);
            resumoDetalhamento.ValorTaxas = resumos.Sum(x => x.ValorTaxas);

            return new ResumoContaItemDto(pacoteDto.DataInicial, pacoteDto.DataInicial,
                pacoteDto.Id, pacoteDto.GrupoId,
                pacoteDto.GrupoDescricao, pacoteDto.GrupoOrdem,
                pacoteDto.SubGrupoId, pacoteDto.SubGrupoDescricao,
                pacoteDto.FatItemDescricao, pacoteDto.Codigo, pacoteDto.CodAmb, pacoteDto.CodCbhpm, pacoteDto.CodTuss, pacoteDto.Qtde, JsonConvert.SerializeObject(resumoDetalhamento), null, pacoteDto.Id);
        } 
    }

    public class CalcularTotalContaInput
    {
        public FaturamentoContaItemViewModel[] Itens { get; set; }
    }

    public class ContaCalculoItem
    {
        public long ConvenioId { get; set; }
        public long EmpresaId { get; set; }
        public long PlanoId { get; set; }
    }

    public class CalculoContaItemInput
    {
        public ContaCalculoItem conta { get; set; }
        public FaturamentoContaItemDto FatContaItemDto { get; set; }
        public List<FaturamentoConfigConvenioDto> configsConvenio { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorEmpresa { get; set; }
        public FaturamentoConfigConvenioDto[] configsTodasEmpresas { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorPlano { get; set; }
        public FaturamentoConfigConvenioDto[] configsTodosPlanos { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorGrupo { get; set; }
        public FaturamentoConfigConvenioDto[] configsTodosGrupos { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorSubGrupo { get; set; }
        public FaturamentoConfigConvenioDto[] configsTodosSubGrupos { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorItem { get; set; }
        public FaturamentoConfigConvenioDto[] configsTodosItens { get; set; }

        public string TabelaUilizada { get; set; }
        public long? FaturamentoConfigConvenioId { get; set; }
        public long? FaturamentoItemCobradoId { get; set; }
    }

    public class VerificarCadastroPrecoInput
    {
        public ContaCalculoItem conta { get; set; }
        public FaturamentoContaItemDto FatContaItemDto { get; set; }
        public List<FaturamentoConfigConvenioDto> configsConvenio { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorPlano { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorEmpresa { get; set; }
    }

    public class ContaMedicaViewModel
    {
        public long Id { get; set; }
        public string UsuarioConferenciaNome { get; set; }
        public string Matricula { get; set; }
        public string CodDependente { get; set; }
        public string NumeroGuia { get; set; }
        public string Titular { get; set; }
        public string GuiaOperadora { get; set; }
        public string GuiaPrincipal { get; set; }
        public string Observacao { get; set; }
        public string SenhaAutorizacao { get; set; }
        public string IdentAcompanhante { get; set; }
        public string PacienteNome { get; set; }
        public string MedicoNome { get; set; }
        public string ConvenioNome { get; set; }
        public string PlanoNome { get; set; }
        public string GuiaNumero { get; set; }
        public string EmpresaNome { get; set; }
        public string AtendimentoCodigo { get; set; }
        public string UnidadeOrganizacionalNome { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public string StatusEntregaCodigo { get; set; }
        public string StatusEntregaDescricao { get; set; }
        public string StatusEntregaCor { get; set; }
        public string FatGuiaId { get; set; }
        public long? PacienteId { get; set; }
        public long? MedicoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? PlanoId { get; set; }
        // public long? TipoLeitoId { get; set; }
        public long? TipoLeitoId { get; set; }
        public long? GuiaId { get; set; }
        public long? EmpresaId { get; set; }
        public long? AtendimentoId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public long? StatusEntregaId { get; set; }
        public bool IsAutorizador { get; set; }
        public int TipoAtendimento { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? ValidadeCarteira { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public DateTime? DiaSerie1 { get; set; }
        public DateTime? DiaSerie2 { get; set; }
        public DateTime? DiaSerie3 { get; set; }
        public DateTime? DiaSerie4 { get; set; }
        public DateTime? DiaSerie5 { get; set; }
        public DateTime? DiaSerie6 { get; set; }
        public DateTime? DiaSerie7 { get; set; }
        public DateTime? DiaSerie8 { get; set; }
        public DateTime? DiaSerie9 { get; set; }
        public DateTime? DiaSerie10 { get; set; }
        public DateTime? DataEntrFolhaSala { get; set; }
        public DateTime? DataEntrDescCir { get; set; }
        public DateTime? DataEntrBolAnest { get; set; }
        public DateTime? DataEntrCDFilme { get; set; }
        public DateTime? DataValidadeSenha { get; set; }
        public FaturamentoGuia FatGuia { get; set; }

        public List<FaturamentoContaItemDto> ContaItensDto { get; set; }




    }

    public class ContaMedicaReportModel
    {
        public long Id { get; set; }
        public string Matricula { get; set; }
        public string CodDependente { get; set; }
        public string NumeroGuia { get; set; }
        public string Titular { get; set; }
        public string GuiaOperadora { get; set; }
        public string GuiaPrincipal { get; set; }
        public string Observacao { get; set; }
        public string SenhaAutorizacao { get; set; }
        public string IdentAcompanhante { get; set; }
        public string PacienteNome { get; set; }
        public string PacienteNascimento { get; set; }
        public string MedicoNome { get; set; }
        public string ConvenioNome { get; set; }
        public string GuiaNumero { get; set; }
        public string PlanoNome { get; set; }
        public string EmpresaNome { get; set; }
        public string AtendimentoCodigo { get; set; }
        public string UnidadeOrganizacionalNome { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public string StatusEntregaCodigo { get; set; }
        public string StatusEntregaDescricao { get; set; }
        public long? MedicoId { get; set; }
        public long? PacienteId { get; set; }
        public long? ConvenioId { get; set; }
        public long? PlanoId { get; set; }
        public long? GuiaId { get; set; }
        public long? EmpresaId { get; set; }
        public long? AtendimentoId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public long? TipoLeitoId { get; set; }
        public bool IsAutorizador { get; set; }
        public int TipoAtendimento { get; set; }
        public DateTime? DataIncio { get; set; }
        public DateTime? DataFim { get; set; }
        public DateTime? DataPagamento { get; set; }
        public DateTime? ValidadeCarteira { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public DateTime? DiaSerie1 { get; set; }
        public DateTime? DiaSerie2 { get; set; }
        public DateTime? DiaSerie3 { get; set; }
        public DateTime? DiaSerie4 { get; set; }
        public DateTime? DiaSerie5 { get; set; }
        public DateTime? DiaSerie6 { get; set; }
        public DateTime? DiaSerie7 { get; set; }
        public DateTime? DiaSerie8 { get; set; }
        public DateTime? DiaSerie9 { get; set; }
        public DateTime? DiaSerie10 { get; set; }
        public DateTime? DataEntrFolhaSala { get; set; }
        public DateTime? DataEntrDescCir { get; set; }
        public DateTime? DataEntrBolAnest { get; set; }
        public DateTime? DataEntrCDFilme { get; set; }
        public DateTime? DataValidadeSenha { get; set; }
        public string CRM { get; set; }
        public string TipoAlta { get; set; }
        public string Conselho { get; set; }
    }

    public class CasoConfig
    {
        public int Prioridade { get; set; }
        public FaturamentoConfigConvenioDto Config { get; set; }

        public CasoConfig(int prioridade, FaturamentoConfigConvenioDto config)
        {
            Prioridade = prioridade;
            Config = config;
        }
    }

    public class ConferirContasInput
    {
        public long[] ContasIds { get; set; }
    }
}
