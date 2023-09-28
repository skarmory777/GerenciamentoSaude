using Abp.Application.Services.Dto;
using Abp.Dependency;
//using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Calculos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Dtos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Pacote;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens
{
    public class FaturamentoContaItemAppService : SWMANAGERAppServiceBase, IFaturamentoContaItemAppService
    {
        public async Task<PagedResultDto<FaturamentoContaItemDto>> Listar(ListarFaturamentoContaItensInput input)
        {
            var contarContaItens = 0;
            List<FaturamentoContaItemDto> contaItensDtos = new List<FaturamentoContaItemDto>();
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {

                    var query = contaItemRepository.Object
                        .GetAll()
                        .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.FaturamentoContaId.ToString() == input.Filtro)
                        .Include(i => i.FaturamentoItem)
                        .Include(i => i.FaturamentoItem.Grupo)
                        .Include(i => i.Turno);

                    contarContaItens = await query
                        .CountAsync();

                    contaItensDtos = (await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync())
                        .Select(s => FaturamentoContaItemDto.MapearFromCore(s))
                        .ToList();

                    // contaItensDtos = contaItens.MapTo<List<FaturamentoContaItemDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoContaItemDto>(
                contarContaItens,
                contaItensDtos
                );
        }

        public async Task<PagedResultDto<FaturamentoContaItemDto>> ListarPorConta(ListarFaturamentoContaItensInput input)
        {
            var contarContaItens = 0;
            List<FaturamentoContaItem> contaItens;
            List<FaturamentoContaItemDto> contaItensDtos = new List<FaturamentoContaItemDto>();
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                using (var faturamentoItemTabela = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItemTabela, long>>())
                {

                    var query = contaItemRepository.Object
                        .GetAll()
                        .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.FaturamentoContaId.ToString() == input.Filtro && (e.FaturamentoPacoteId == null || e.FaturamentoItem.Grupo.TipoGrupoId == 4))
                        .Include(i => i.FaturamentoItem)
                        .Include(i => i.FaturamentoItem.Grupo)
                        .Include(i => i.Turno)
                        .Include(i => i.FaturamentoConfigConvenio)
                        ;

                    contarContaItens = await query
                        .CountAsync();

                    contaItens = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();


                    foreach (var contaItem in contaItens)
                    {
                        var novo = FaturamentoContaItemDto.MapearFromCore(contaItem);

                        long? tabelaId = null;

                        if (contaItem.FaturamentoConfigConvenio != null)
                        {
                            tabelaId = contaItem.FaturamentoConfigConvenio.TabelaId;
                        }
                        if (!novo.IsValorItemManual)
                        {
                            var fatItemTabela = faturamentoItemTabela.Object.GetAll()
                                                    .Where(w => w.ItemId == contaItem.FaturamentoItemId
                                                            && w.TabelaId == tabelaId)
                                                    .FirstOrDefault();
                            if (fatItemTabela != null)
                            {
                                novo.ValorItem = fatItemTabela.Preco;
                            }
                            else
                            {
                                novo.ValorItem = 0;
                            }
                        }


                        contaItensDtos.Add(novo);
                    }
                    //         contaItensDtos = contaItens.MapTo<List<FaturamentoContaItemDto>>();

                    return new PagedResultDto<FaturamentoContaItemDto>(contarContaItens, contaItensDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<float> CalcularValorUnitarioContaItem (CalculoContaItemInput input)
        //{
        //    try
        //    {
        //        input.i.FaturamentoItem = await _fatItemAppService.Obter((long)input.i.FaturamentoItemId);

        //        long? tabelaId;

        //        // Caso haja para um plano especifico
        //        if (input.configsPorPlano.Count() > 0)
        //        {
        //            tabelaId = input.configsPorPlano/*.Where(x=>x.GrupoId == i.FaturamentoItem.GrupoId)*/.FirstOrDefault().TabelaId;

        //            // Por item especifico
        //            var configPorItem = input.configsPorPlano.FirstOrDefault(_ => _.ItemId == input.i.Id);

        //            if (configPorItem != null)
        //            {
        //                tabelaId = configPorItem.TabelaId;
        //            }
        //            else
        //            {
        //                // Por subGrupo especifico
        //                var configPorSubGrupo = input.configsPorPlano.FirstOrDefault(_ => _.SubGrupoId == input.i.FaturamentoItem.SubGrupoId);

        //                if (configPorSubGrupo != null)
        //                {
        //                    tabelaId = configPorSubGrupo.TabelaId;
        //                }
        //                else
        //                {
        //                    // Por grupo especifico
        //                    var configPorGrupo = input.configsPorPlano.FirstOrDefault(_ => _.GrupoId == input.i.FaturamentoItem.GrupoId);

        //                    if (configPorGrupo != null)
        //                    {
        //                        tabelaId = configPorGrupo.TabelaId;
        //                    }
        //                }
        //            }
        //        }
        //        // Caso seja para todos os planos
        //        else
        //        {
        //            tabelaId = input.configsPorEmpresa.First().TabelaId;

        //            // Por item
        //            var configPorItem = input.configsPorEmpresa.FirstOrDefault(_ => _.ItemId == input.i.Id);

        //            if (configPorItem != null)
        //            {
        //                tabelaId = configPorItem.TabelaId;
        //            }
        //            else
        //            {
        //                // Por subGrupo
        //                var configPorSubGrupo = input.configsPorEmpresa.FirstOrDefault(_ => _.SubGrupoId == input.i.FaturamentoItem.SubGrupoId);

        //                if (configPorSubGrupo != null)
        //                {
        //                    tabelaId = configPorSubGrupo.TabelaId;
        //                }
        //                else
        //                {
        //                    // Por grupo
        //                    var configPorGrupo = input.configsPorEmpresa.FirstOrDefault(_ => _.GrupoId == input.i.FaturamentoItem.GrupoId);

        //                    if (configPorGrupo != null)
        //                    {
        //                        tabelaId = configPorGrupo.TabelaId;
        //                    }
        //                }
        //            }
        //        }

        //        // ======================= PRECO =========================

        //        // Obter preco vigente
        //        var precosPorTabela = AsyncHelper.RunSync(() => _tabelaItemAppService.ListarParaFatTabela((long)tabelaId)).Items;
        //        var precosPorFatItem = precosPorTabela.Where(_ => _.ItemId == input.i.FaturamentoItemId);
        //        var preco = precosPorFatItem
        //            .Where(_ => _.VigenciaDataInicio <= DateTime.Now)
        //            .OrderByDescending(_ => _.VigenciaDataInicio).First();

        //        var moeda = AsyncHelper.RunSync(() => _moedaAppService.Obter((long)preco.SisMoedaId));
        //        ListarSisMoedaCotacoesInput cotacaoInput = new ListarSisMoedaCotacoesInput();
        //        cotacaoInput.Filtro = moeda.Id.ToString();

        //        // ======================= COTACAO =========================

        //        // Buscar cotacoes por convenio
        //        var cotacoesPorConvenio = AsyncHelper.RunSync(() => _cotacaoAppService.ListarPorMoeda(cotacaoInput))
        //            .Items
        //            .Where(_ => _.ConvenioId == input.conta.ConvenioId)
        //            ;

        //        // Cotacoes por Empresa
        //        var cotacoesPorEmpresa = cotacoesPorConvenio
        //            .Where(_ => _.EmpresaId == input.conta.EmpresaId)
        //            ;

        //        var cotacao = cotacoesPorConvenio
        //                    .Where(_ => _.DataInicio <= DateTime.Now /*&& _.DataFinal >= DateTime.Now*/)
        //                    .OrderByDescending(_ => _.DataInicio)
        //                    .FirstOrDefault();

        //        // Filtrar cotacoes por plano
        //        var cotacoesPorPlano = cotacoesPorEmpresa
        //            .Where(x => x.PlanoId != null)
        //            .Where(c => c.PlanoId == input.conta.PlanoId); // se planoId for null vai pegar todos com planoId null

        //        // Caso haja para um plano especifico
        //        if (cotacoesPorPlano.Count() > 0)
        //        {
        //            // Por subGrupo especifico
        //            var cotacaoPorSubGrupo = cotacoesPorPlano
        //                .Where(_ => _.SubGrupoId == input.i.FaturamentoItem.SubGrupoId && _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now)
        //                .OrderByDescending(_ => _.DataInicio)
        //                .FirstOrDefault();

        //            if (cotacaoPorSubGrupo != null)
        //            {
        //                cotacao = cotacaoPorSubGrupo;
        //            }
        //            else
        //            {
        //                // Por grupo especifico
        //                var cotacaoPorGrupo = cotacoesPorPlano
        //                    .Where(_ => _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now && _.GrupoId == input.i.FaturamentoItem.GrupoId)
        //                    .OrderByDescending(_ => _.DataInicio)
        //                    .FirstOrDefault();

        //                if (cotacaoPorGrupo != null)
        //                {
        //                    cotacao = cotacaoPorGrupo;
        //                }
        //            }
        //        }
        //        // Caso seja para todos os planos
        //        else
        //        {
        //            // Por subGrupo
        //            var cotacaoPorSubGrupo = cotacoesPorConvenio
        //                .Where(_ => _.DataInicio <= DateTime.Now /*&& _.DataFinal >= DateTime.Now*/)
        //                .OrderByDescending(_ => _.DataInicio)
        //                .FirstOrDefault(_ => _.SubGrupoId == input.i.FaturamentoItem.SubGrupoId);

        //            if (cotacaoPorSubGrupo != null)
        //            {
        //                cotacao = cotacaoPorSubGrupo;
        //            }
        //            else
        //            {
        //                // Por grupo
        //                var cotacaoPorGrupo = cotacoesPorConvenio
        //                    .Where(_ => _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now)
        //                    .OrderByDescending(_ => _.DataInicio)
        //                    .FirstOrDefault(_ => _.GrupoId == input.i.FaturamentoItem.GrupoId);

        //                if (cotacaoPorGrupo != null)
        //                {
        //                    cotacao = cotacaoPorGrupo;
        //                }
        //            }

        //        }

        //        // Filme
        //        cotacaoInput.Filtro = "1";//AQUI DEVE SER A ID FIXA DA MOEDA 'FILME' - CRIAR SEED NO EF PARA MOEDAS 'FIXAS' DO SISTEMA
        //        var cotacaoFilme = AsyncHelper.RunSync(() => _cotacaoAppService.ListarPorMoeda(cotacaoInput))
        //                                .Items
        //                                .Where(_ => _.DataInicio <= DateTime.Now)
        //                                .OrderByDescending(_ => _.DataInicio)
        //                                .FirstOrDefault();

        //        var totalFilme = cotacaoFilme.Valor * input.i.MetragemFilme;

        //        // Porte
        //        var tabela = await _tabelaAppService.Obter((long)tabelaId);
        //        float totalPorte = 0f;
        //        if (tabela.IsCBHPM)
        //        {
        //            cotacaoInput.Filtro = moeda.Id.ToString();
        //            var cotacaoPorte = AsyncHelper.RunSync(() => _cotacaoAppService.ListarPorMoeda(cotacaoInput))
        //                                    .Items
        //                                    .Where(_ => _.DataInicio <= DateTime.Now)
        //                                    .OrderByDescending(_ => _.DataInicio)
        //                                    .FirstOrDefault();

        //            totalPorte = cotacaoPorte.Valor * preco.Porte;
        //        }

        //        // Valor unitario
        //        var valorUnitario = ((preco.Preco * cotacao.Valor) + totalFilme + totalPorte);
        //        return valorUnitario;
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //        return 0f;
        //    }


        //}

        public async Task<PagedResultDto<FaturamentoContaItemViewModel>> ListarVM(ListarFaturamentoContaItensInput input)
        {
            var contarContaItens = 0;
            List<FaturamentoContaItemDto> contaItens;
            List<FaturamentoContaItemViewModel> contaItensDtos = new List<FaturamentoContaItemViewModel>();
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    var query = contaItemRepository.Object
                        .GetAll()
                        .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.FaturamentoContaId.ToString() == input.Filtro)
                        .Include(i => i.FaturamentoItem)
                        .Include(i => i.FaturamentoItem.Grupo)
                        .Include(i => i.FaturamentoItem.Grupo.TipoGrupo)
                        .Include(i => i.Turno)
                        // .Include(t => t.TipoLeito)
                        .Include(t => t.TipoAcomodacao)
                        .Include(u => u.UnidadeOrganizacional);

                    contarContaItens = await query
                        .CountAsync();

                    contaItens = (await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync())
                        .Select(s => FaturamentoContaItemDto.MapearFromCore(s))
                        .ToList();

                    //     var conta = _contaAppService.Obter((long)int.Parse(input.Filtro));

                    foreach (var item in contaItens)
                    {
                        var i = new FaturamentoContaItemViewModel();
                        input.CalculoContaItemInput.FatContaItemDto = item;//.MapTo<FaturamentoContaItemDto>();// DEMORANDO PRA CARALHO TOMAR NO CU

                        ///////////////////////////////////////////////
                        //     i.ValorUnitario =  await CalcularValorUnitarioContaItem(input.CalculoContaItemInput);
                        i.ValorTotal = i.ValorUnitario * i.Qtde;
                        /////////////////////////////////////////////

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
                        //i.TipoLeitoId                    = item.TipoLeitoId;
                        //i.TipoLeitoDescricao             = item.TipoLeito != null ? item.TipoLeito.Descricao : string.Empty;

                        i.TipoLeitoId = item.TipoLeitoId;
                        i.TipoLeitoDescricao = item.TipoLeito != null ? item.TipoLeito.Descricao : string.Empty;


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
                        i.IsValorItemManual = item.IsValorItemManual;
                        i.ValorItem = item.ValorItem;
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
                        // STATUSNOVO ALTERAR             i.StatusEntrega                  = item.StatusEntrega;
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

                        contaItensDtos.Add(i);
                    }

                    return new PagedResultDto<FaturamentoContaItemViewModel>(contarContaItens, contaItensDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoContaItemReportModel>> ListarReportModel(ListarFaturamentoContaItensInput input)
        {
            var contarContaItens = 0;
            List<FaturamentoContaItem> contaItens;
            List<FaturamentoContaItemReportModel> contaItensDtos = new List<FaturamentoContaItemReportModel>();
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    var query = contaItemRepository.Object
                        .GetAll()
                        .WhereIf(!string.IsNullOrEmpty(input.Filtro), e => e.FaturamentoContaId.ToString() == input.Filtro)
                        .Include(i => i.FaturamentoItem)
                        .Include(i => i.FaturamentoItem.Grupo)
                        .Include(i => i.FaturamentoItem.Grupo.TipoGrupo)
                        .Include(i => i.Turno)
                        // .Include(t => t.TipoLeito)
                        .Include(t => t.TipoAcomodacao)
                        .Include(u => u.UnidadeOrganizacional)
                        .Include(u => u.Medico)
                        .Include(m => m.Medico.SisPessoa)
                        .Include(u => u.FaturamentoConta)
                        .Include(u => u.FaturamentoConfigConvenio)
                        ;

                    contarContaItens = await query
                        .CountAsync();

                    contaItens = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();




                    //ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();

                    //if (contaItens.Count > 0)
                    //{
                    //    configConvenioInput.Filtro = contaItens[0].FaturamentoConta.ConvenioId.ToString();
                    //    configConvenioInput.ConvenioId = contaItens[0].FaturamentoConta.ConvenioId;
                    //    configConvenioInput.PlanoId = contaItens[0].FaturamentoConta.PlanoId;
                    //    configConvenioInput.EmpresaId = contaItens[0].FaturamentoConta.EmpresaId;

                    //    configConvenioInput.GrupoId = contaItens[0].FaturamentoItem.GrupoId;
                    //    configConvenioInput.SubGrupoId = contaItens[0].FaturamentoItem.SubGrupoId;
                    //    configConvenioInput.ItemId = contaItens[0].FaturamentoItemId;

                    //    input.CalculoContaItemInput = new CalculoContaItemInput();
                    //    input.CalculoContaItemInput.conta = new ContaCalculoItem();
                    //    input.CalculoContaItemInput.conta.ConvenioId = configConvenioInput.ConvenioId ?? 0;
                    //    input.CalculoContaItemInput.conta.PlanoId = configConvenioInput.PlanoId ?? 0;
                    //    input.CalculoContaItemInput.conta.EmpresaId = configConvenioInput.EmpresaId ?? 0;
                    //}

                    //var configsConvenio = await _configConvenioAppService.ListarPorConvenio(configConvenioInput);
                    // Fim - obtencao de config.convenio



                    //var configsPorEmpresa = configsConvenio.Items
                    //       .Where(c => c.EmpresaId == contaItens[0].FaturamentoConta.EmpresaId);

                    //// Filtrar por plano
                    //var configsPorPlano = configsPorEmpresa
                    //    .Where(x => x.PlanoId != null)
                    //    .Where(c => c.PlanoId == contaItens[0].FaturamentoConta.PlanoId);




                    foreach (var item in contaItens)
                    {
                        var i = new FaturamentoContaItemReportModel();



                        //input.CalculoContaItemInput.FatContaItemDto = new FaturamentoContaItemDto();
                        //input.CalculoContaItemInput.FatContaItemDto.Id = item.Id;
                        //input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem = new Itens.Dto.FaturamentoItemDto();
                        //input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.SubGrupoId = item.FaturamentoItem?.SubGrupoId;
                        //input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.GrupoId = item.FaturamentoItem?.GrupoId;
                        //input.CalculoContaItemInput.FatContaItemDto.FaturamentoItem.Id = item.FaturamentoItemId ?? 0;
                        //input.CalculoContaItemInput.FatContaItemDto.FaturamentoItemId = item.FaturamentoItemId ?? 0;
                        //input.CalculoContaItemInput.FatContaItemDto.MetragemFilme = item.MetragemFilme;

                        ///////////////////////////////////////////////////////////
                        // ============== CALCULO DE VALOR UNITARIO ===============
                        // Filtrar por empresa
                        //var configsPorEmpresa = configsConvenio.Items
                        //    .Where(c => c.EmpresaId == item.FaturamentoConta.EmpresaId);

                        //// Filtrar por plano
                        //var configsPorPlano = configsPorEmpresa
                        //    .Where(x => x.PlanoId != null)
                        //    .Where(c => c.PlanoId == item.FaturamentoConta.PlanoId);

                        //input.CalculoContaItemInput.configsPorEmpresa = configsPorEmpresa.ToArray();
                        //input.CalculoContaItemInput.configsPorPlano = configsPorPlano.ToArray();

                        // Valor manual ou calculado em tempo de execucao
                        //i.IsValorItemManual = item.IsValorItemManual;
                        //if (i.IsValorItemManual)
                        //{
                        //    i.ValorItem = item.ValorItem;
                        //    // i.ValorTotal = i.ValorItem * item.Qtde;
                        //}
                        //else
                        //{
                        //    i.ValorItem = await CalcularValorUnitarioContaItem(input.CalculoContaItemInput);
                        //    // i.ValorItem = i.ValorUnitario;
                        //    // i. .ValorTotal = i.ValorUnitario * item.Qtde;
                        //}


                        //long? tabelaId = null;

                        //if (item.FaturamentoConfigConvenio != null)
                        //{
                        //    tabelaId = item.FaturamentoConfigConvenio.TabelaId;
                        //}
                        //if (!item.IsValorItemManual)
                        //{
                        //    var fatItemTabela = _faturamentoItemTabela.GetAll()
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


                        //i.ValorTotal = i.ValorItem * item.Qtde;






                        i.Id = item.Id;
                        i.Grupo = item.FaturamentoItem.Grupo != null ? item.FaturamentoItem.Grupo.Descricao : string.Empty;
                        i.Tipo = item.FaturamentoItem.Grupo != null ? (item.FaturamentoItem.Grupo.TipoGrupo != null ? item.FaturamentoItem.Grupo.TipoGrupo.Descricao : string.Empty) : string.Empty;
                        i.Descricao = item.FaturamentoItem.Descricao;
                        i.FaturamentoItemId = item.FaturamentoItemId;
                        i.FaturamentoItemCodigo = item.FaturamentoItem != null ? item.FaturamentoItem.Codigo : string.Empty;
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
                        //i.TipoLeitoId                    = item.TipoLeitoId;
                        //i.TipoLeitoDescricao             = item.TipoLeito != null ? item.TipoLeito.Descricao : string.Empty;

                        i.TipoLeitoId = item.TipoAcomodacaoId;
                        i.TipoLeitoDescricao = item.TipoAcomodacao != null ? item.TipoAcomodacao.Descricao : string.Empty;

                        i.ValorTemp = item.ValorTemp;
                        i.MedicoId = item.MedicoId;
                        i.MedicoNome = item.Medico != null ? item.Medico.NomeCompleto : string.Empty;
                        i.IsMedCrendenciado = item.IsMedCredenciado;
                        i.IsGlosaMedico = item.IsGlosaMedico;
                        i.MedicoEspecialidadeId = item.MedicoEspecialidadeId;
                        i.MedicoEspecialidadeNome = item.MedicoEspecialidade != null ? item.MedicoEspecialidade.Especialidade.Nome : string.Empty;
                        i.FaturamentoContaKitId = item.FaturamentoContaKitId;
                        i.IsCirurgia = item.IsCirurgia;
                        i.ValorAprovado = item.ValorAprovado;
                        i.ValorTaxas = item.ValorTaxas;
                        i.IsValorItemManual = item.IsValorItemManual;
                        // i.ValorItem = item.ValorItem;
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
                        // STATUSNOVO ALTERAR         i.StatusEntrega                  = item.StatusEntrega;
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
                        i.ValorItem = item.ValorItem;


                        contaItensDtos.Add(i);
                    }

                    return new PagedResultDto<FaturamentoContaItemReportModel>(contarContaItens, contaItensDtos);
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        public async Task CriarOuEditar(FaturamentoContaItemDto input)
        {
            try
            {
                using(var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia,long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var i = new FaturamentoContaItem
                    {
                        Id = input.Id,
                        Descricao = input.FaturamentoItem != null ? input.FaturamentoItem.Descricao : string.Empty,
                        FaturamentoItemId = input.FaturamentoItemId,
                        FaturamentoContaId = input.FaturamentoContaId,
                        Data = input.Data,
                        Qtde = input.Qtde,
                        UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                        TerceirizadoId = input.TerceirizadoId,
                        CentroCustoId = input.CentroCustoId,
                        TurnoId = input.TurnoId,
                        // i.TipoLeitoId                    = input.TipoLeitoId;
                        TipoAcomodacaoId = input.TipoLeitoId,
                        ValorTemp = input.ValorTemp,
                        MedicoId = input.MedicoId,
                        AnestesistaId = input.AnestesistaId,
                        Auxiliar1Id = input.Auxiliar1Id,
                        Auxiliar2Id = input.Auxiliar2Id,
                        Auxiliar3Id = input.Auxiliar3Id,
                        InstrumentadorId = input.InstrumentadorId,
                        IsMedCredenciado = input.IsMedCredenciado,
                        IsGlosaMedico = input.IsGlosaMedico,
                        MedicoEspecialidadeId = input.MedicoEspecialidadeId,
                        Auxiliar1EspecialidadeId = input.Auxiliar1EspecialidadeId,
                        Auxiliar2EspecialidadeId = input.Auxiliar2EspecialidadeId,
                        Auxiliar3EspecialidadeId = input.Auxiliar3EspecialidadeId,
                        AnestesistaEspecialidadeId = input.EspecialidadeAnestesistaId,
                        InstrumentadorEspecialidadeId = input.InstrumentadorId,
                        FaturamentoContaKitId = input.FaturamentoContaKitId,
                        IsCirurgia = input.IsCirurgia,
                        ValorAprovado = input.ValorAprovado,
                        ValorTaxas = input.ValorTaxas,
                        IsValorItemManual = input.IsValorItemManual,
                        ValorItem = input.ValorItem,
                        HMCH = input.HMCH,
                        ValorFilme = input.ValorFilme,
                        ValorFilmeAprovado = input.ValorFilmeAprovado,
                        ValorCOCH = input.ValorCOCH,
                        ValorCOCHAprovado = input.ValorCOCHAprovado,
                        Percentual = input.Percentual,
                        IsInstrCredenciado = input.IsInstrCredenciado,
                        ValorTotalRecuperado = input.ValorTotalRecuperado,
                        ValorTotalRecebido = input.ValorTotalRecebido,
                        MetragemFilme = input.MetragemFilme,
                        MetragemFilmeAprovada = input.MetragemFilmeAprovada,
                        COCH = input.COCH,
                        COCHAprovado = input.COCHAprovado,
                        // STATUSNOVO ALTERAR        i.StatusEntrega                  = input.StatusEntrega;
                        IsRecuperaMedico = input.IsRecuperaMedico,
                        IsAux1Credenciado = input.IsAux1Credenciado,
                        IsRecebeAuxiliar1 = input.IsRecebeAuxiliar1,
                        IsGlosaAuxiliar1 = input.IsGlosaAuxiliar1,
                        IsRecuperaAuxiliar1 = input.IsRecuperaAuxiliar1,
                        IsAux2Credenciado = input.IsAux2Credenciado,
                        IsRecebeAuxiliar2 = input.IsRecebeAuxiliar2,
                        IsGlosaAuxiliar2 = input.IsGlosaAuxiliar2,
                        IsRecuperaAuxiliar2 = input.IsRecuperaAuxiliar2,
                        IsAux3Credenciado = input.IsAux3Credenciado,
                        IsRecebeAuxiliar3 = input.IsRecebeAuxiliar3,
                        IsGlosaAuxiliar3 = input.IsGlosaAuxiliar3,
                        IsRecuperaAuxiliar3 = input.IsRecuperaAuxiliar3,
                        IsRecebeInstrumentador = input.IsRecebeInstrumentador,
                        IsGlosaInstrumentador = input.IsGlosaInstrumentador,
                        IsRecuperaInstrumentador = input.IsRecuperaInstrumentador,
                        Observacao = input.Observacao,
                        QtdeRecuperada = input.QtdeRecuperada,
                        QtdeAprovada = input.QtdeAprovada,
                        QtdeRecebida = input.QtdeRecebida,
                        ValorMoedaAprovado = input.ValorMoedaAprovado,
                        SisMoedaId = input.SisMoedaId,
                        DataAutorizacao = input.DataAutorizacao,
                        SenhaAutorizacao = input.SenhaAutorizacao,
                        NomeAutorizacao = input.NomeAutorizacao,
                        ObsAutorizacao = input.ObsAutorizacao,
                        HoraIncio = input.HoraIncio,
                        HoraFim = input.HoraFim,
                        ViaAcesso = input.ViaAcesso,
                        Tecnica = input.Tecnica,
                        ClinicaId = input.ClinicaId,
                        FornecedorId = input.FornecedorId,
                        NumeroNF = input.NumeroNF,
                        IsImportaEstoque = input.IsImportaEstoque,
                        FaturamentoConfigConvenioId = input.FaturamentoConfigConvenioId,
                        FaturamentoPacoteId = input.FaturamentoPacoteId,
                        ResumoDetalhamento = input.ResumoDetalhamento
                    };


                    if (input.Id.Equals(0))
                    {
                        input.Id = await contaItemRepository.Object.InsertAndGetIdAsync(i);
                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now, 
                            OcorrenciaTexto.ContaMedicaItemCriada(i.Codigo,i.FaturamentoItem?.Descricao, (await this.GetCurrentUserAsync()).FullName),
                            TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaItem,
                            typeof(FaturamentoContaItem).FullName, input.Id, typeof(FaturamentoConta).FullName,
                            i.FaturamentoContaId));
                    }
                    else
                    {
                        var item = contaItemRepository.Object.GetAll().FirstOrDefault(w => w.Id == input.Id);

                        item.Id = input.Id;
                        item.Descricao = input.FaturamentoItem != null ? input.FaturamentoItem.Descricao : string.Empty;
                        item.FaturamentoItemId = input.FaturamentoItemId;
                        //  item.FaturamentoContaId = input.FaturamentoContaId;
                        item.Data = input.Data;
                        item.Qtde = input.Qtde;
                        item.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                        item.TerceirizadoId = input.TerceirizadoId;
                        item.CentroCustoId = input.CentroCustoId;
                        item.TurnoId = input.TurnoId;
                        // item.TipoLeitoId                    = input.TipoLeitoId;
                        item.TipoAcomodacaoId = input.TipoLeitoId;
                        item.ValorTemp = input.ValorTemp;
                        item.MedicoId = input.MedicoId;
                        item.AnestesistaId = input.AnestesistaId;
                        item.Auxiliar1Id = input.Auxiliar1Id;
                        item.Auxiliar2Id = input.Auxiliar2Id;
                        item.Auxiliar3Id = input.Auxiliar3Id;
                        item.InstrumentadorId = input.InstrumentadorId;
                        item.IsMedCredenciado = input.IsMedCredenciado;
                        item.IsGlosaMedico = input.IsGlosaMedico;
                        item.MedicoEspecialidadeId = input.MedicoEspecialidadeId;
                        item.Auxiliar1EspecialidadeId = input.Auxiliar1EspecialidadeId;
                        item.Auxiliar2EspecialidadeId = input.Auxiliar2EspecialidadeId;
                        item.Auxiliar3EspecialidadeId = input.Auxiliar3EspecialidadeId;
                        item.AnestesistaEspecialidadeId = input.EspecialidadeAnestesistaId;
                        item.InstrumentadorEspecialidadeId = input.InstrumentadorEspecialidadeId;
                        item.FaturamentoContaKitId = input.FaturamentoContaKitId;
                        item.IsCirurgia = input.IsCirurgia;
                        item.ValorAprovado = input.ValorAprovado;
                        item.ValorTaxas = input.ValorTaxas;
                        item.IsValorItemManual = input.IsValorItemManual;
                        item.ValorItem = input.ValorItem;
                        item.HMCH = input.HMCH;
                        item.ValorFilme = input.ValorFilme;
                        item.ValorFilmeAprovado = input.ValorFilmeAprovado;
                        item.ValorCOCH = input.ValorCOCH;
                        item.ValorCOCHAprovado = input.ValorCOCHAprovado;
                        item.Percentual = input.Percentual;
                        item.IsInstrCredenciado = input.IsInstrCredenciado;
                        item.ValorTotalRecuperado = input.ValorTotalRecuperado;
                        item.ValorTotalRecebido = input.ValorTotalRecebido;
                        item.MetragemFilme = input.MetragemFilme;
                        item.MetragemFilmeAprovada = input.MetragemFilmeAprovada;
                        item.COCH = input.COCH;
                        item.COCHAprovado = input.COCHAprovado;
                        // STATUSNOVO ALTERAR        item.StatusEntrega                  = input.StatusEntrega;
                        item.IsRecuperaMedico = input.IsRecuperaMedico;
                        item.IsAux1Credenciado = input.IsAux1Credenciado;
                        item.IsRecebeAuxiliar1 = input.IsRecebeAuxiliar1;
                        item.IsGlosaAuxiliar1 = input.IsGlosaAuxiliar1;
                        item.IsRecuperaAuxiliar1 = input.IsRecuperaAuxiliar1;
                        item.IsAux2Credenciado = input.IsAux2Credenciado;
                        item.IsRecebeAuxiliar2 = input.IsRecebeAuxiliar2;
                        item.IsGlosaAuxiliar2 = input.IsGlosaAuxiliar2;
                        item.IsRecuperaAuxiliar2 = input.IsRecuperaAuxiliar2;
                        item.IsAux3Credenciado = input.IsAux3Credenciado;
                        item.IsRecebeAuxiliar3 = input.IsRecebeAuxiliar3;
                        item.IsGlosaAuxiliar3 = input.IsGlosaAuxiliar3;
                        item.IsRecuperaAuxiliar3 = input.IsRecuperaAuxiliar3;
                        item.IsRecebeInstrumentador = input.IsRecebeInstrumentador;
                        item.IsGlosaInstrumentador = input.IsGlosaInstrumentador;
                        item.IsRecuperaInstrumentador = input.IsRecuperaInstrumentador;
                        item.Observacao = input.Observacao;
                        item.QtdeRecuperada = input.QtdeRecuperada;
                        item.QtdeAprovada = input.QtdeAprovada;
                        item.QtdeRecebida = input.QtdeRecebida;
                        item.ValorMoedaAprovado = input.ValorMoedaAprovado;
                        item.SisMoedaId = input.SisMoedaId;
                        item.DataAutorizacao = input.DataAutorizacao;
                        item.SenhaAutorizacao = input.SenhaAutorizacao;
                        item.NomeAutorizacao = input.NomeAutorizacao;
                        item.ObsAutorizacao = input.ObsAutorizacao;
                        item.HoraIncio = input.HoraIncio;
                        item.HoraFim = input.HoraFim;
                        item.ViaAcesso = input.ViaAcesso;
                        item.Tecnica = input.Tecnica;
                        item.ClinicaId = input.ClinicaId;
                        item.FornecedorId = input.FornecedorId;
                        item.NumeroNF = input.NumeroNF;
                        item.IsImportaEstoque = input.IsImportaEstoque;
                        item.FaturamentoConfigConvenioId = input.FaturamentoConfigConvenioId;
                        item.FaturamentoPacoteId = input.FaturamentoPacoteId;
                        item.ResumoDetalhamento = input.ResumoDetalhamento;

                        await contaItemRepository.Object.UpdateAsync(item);
                        
                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now, 
                            OcorrenciaTexto.ContaMedicaItemAlterado(item.Codigo, item.FaturamentoItem?.Descricao, (await this.GetCurrentUserAsync()).FullName),
                            TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaItem,
                            typeof(FaturamentoContaItem).FullName, item.Id, typeof(FaturamentoConta).FullName,
                            item.FaturamentoContaId));
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task IncluirItemsDoKit(IncluirItemsDoKitInputDto input)
        {
            if (input.Items.IsNullOrEmpty())
            {
                return;
            }
            try
            {
                using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia,long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>()) 
                using( var fatItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem,long>>())
                using(var fatKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoKit,long>>())
                using(var fatContaKitAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaKitAppService>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var fatKit = await fatKitRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == input.FaturamentoKitId.Value);
                    var fatItemIds = input.Items.Where(x => x.FaturamentoItemId.HasValue && x.FaturamentoItemId != 0)
                        .Select(x => x.FaturamentoItemId.Value);
                    var fatItems = await fatItemRepository.Object.GetAll().AsNoTracking()
                        .Where(x => fatItemIds.Contains(x.Id)).ToListAsync();
                    var contaKidId = await fatContaKitAppService.Object.CriarOuEditar(input);
                    foreach (var inputContaItem in input.Items)
                    {
                        inputContaItem.FaturamentoContaKitId = contaKidId;
                        inputContaItem.FaturamentoContaId = input.FaturamentoContaId;
                        var contaItemEntity = new FaturamentoContaItem
                        {
                            Id = inputContaItem.Id,
                            Descricao = inputContaItem.FaturamentoItem != null ? inputContaItem.FaturamentoItem.Descricao : string.Empty,
                            FaturamentoItemId = inputContaItem.FaturamentoItemId,
                            FaturamentoContaId = inputContaItem.FaturamentoContaId,
                            Data = inputContaItem.Data,
                            Qtde = inputContaItem.Qtde,
                            UnidadeOrganizacionalId = inputContaItem.UnidadeOrganizacionalId,
                            TerceirizadoId = inputContaItem.TerceirizadoId,
                            CentroCustoId = inputContaItem.CentroCustoId,
                            TurnoId = inputContaItem.TurnoId,
                            // i.TipoLeitoId                    = input.TipoLeitoId;
                            TipoAcomodacaoId = inputContaItem.TipoLeitoId,
                            ValorTemp = inputContaItem.ValorTemp,
                            MedicoId = inputContaItem.MedicoId,
                            AnestesistaId = inputContaItem.AnestesistaId,
                            Auxiliar1Id = inputContaItem.Auxiliar1Id,
                            Auxiliar2Id = inputContaItem.Auxiliar2Id,
                            Auxiliar3Id = inputContaItem.Auxiliar3Id,
                            InstrumentadorId = inputContaItem.InstrumentadorId,
                            IsMedCredenciado = inputContaItem.IsMedCredenciado,
                            IsGlosaMedico = inputContaItem.IsGlosaMedico,
                            MedicoEspecialidadeId = inputContaItem.MedicoEspecialidadeId,
                            Auxiliar1EspecialidadeId = inputContaItem.Auxiliar1EspecialidadeId,
                            Auxiliar2EspecialidadeId = inputContaItem.Auxiliar2EspecialidadeId,
                            Auxiliar3EspecialidadeId = inputContaItem.Auxiliar3EspecialidadeId,
                            AnestesistaEspecialidadeId = inputContaItem.EspecialidadeAnestesistaId,
                            InstrumentadorEspecialidadeId = inputContaItem.InstrumentadorId,
                            FaturamentoContaKitId = inputContaItem.FaturamentoContaKitId,
                            IsCirurgia = inputContaItem.IsCirurgia,
                            ValorAprovado = inputContaItem.ValorAprovado,
                            ValorTaxas = inputContaItem.ValorTaxas,
                            IsValorItemManual = inputContaItem.IsValorItemManual,
                            ValorItem = inputContaItem.ValorItem,
                            HMCH = inputContaItem.HMCH,
                            ValorFilme = inputContaItem.ValorFilme,
                            ValorFilmeAprovado = inputContaItem.ValorFilmeAprovado,
                            ValorCOCH = inputContaItem.ValorCOCH,
                            ValorCOCHAprovado = inputContaItem.ValorCOCHAprovado,
                            Percentual = inputContaItem.Percentual,
                            IsInstrCredenciado = inputContaItem.IsInstrCredenciado,
                            ValorTotalRecuperado = inputContaItem.ValorTotalRecuperado,
                            ValorTotalRecebido = inputContaItem.ValorTotalRecebido,
                            MetragemFilme = inputContaItem.MetragemFilme,
                            MetragemFilmeAprovada = inputContaItem.MetragemFilmeAprovada,
                            COCH = inputContaItem.COCH,
                            COCHAprovado = inputContaItem.COCHAprovado,
                            // STATUSNOVO ALTERAR        i.StatusEntrega                  = input.StatusEntrega;
                            IsRecuperaMedico = inputContaItem.IsRecuperaMedico,
                            IsAux1Credenciado = inputContaItem.IsAux1Credenciado,
                            IsRecebeAuxiliar1 = inputContaItem.IsRecebeAuxiliar1,
                            IsGlosaAuxiliar1 = inputContaItem.IsGlosaAuxiliar1,
                            IsRecuperaAuxiliar1 = inputContaItem.IsRecuperaAuxiliar1,
                            IsAux2Credenciado = inputContaItem.IsAux2Credenciado,
                            IsRecebeAuxiliar2 = inputContaItem.IsRecebeAuxiliar2,
                            IsGlosaAuxiliar2 = inputContaItem.IsGlosaAuxiliar2,
                            IsRecuperaAuxiliar2 = inputContaItem.IsRecuperaAuxiliar2,
                            IsAux3Credenciado = inputContaItem.IsAux3Credenciado,
                            IsRecebeAuxiliar3 = inputContaItem.IsRecebeAuxiliar3,
                            IsGlosaAuxiliar3 = inputContaItem.IsGlosaAuxiliar3,
                            IsRecuperaAuxiliar3 = inputContaItem.IsRecuperaAuxiliar3,
                            IsRecebeInstrumentador = inputContaItem.IsRecebeInstrumentador,
                            IsGlosaInstrumentador = inputContaItem.IsGlosaInstrumentador,
                            IsRecuperaInstrumentador = inputContaItem.IsRecuperaInstrumentador,
                            Observacao = inputContaItem.Observacao,
                            QtdeRecuperada = inputContaItem.QtdeRecuperada,
                            QtdeAprovada = inputContaItem.QtdeAprovada,
                            QtdeRecebida = inputContaItem.QtdeRecebida,
                            ValorMoedaAprovado = inputContaItem.ValorMoedaAprovado,
                            SisMoedaId = inputContaItem.SisMoedaId,
                            DataAutorizacao = inputContaItem.DataAutorizacao,
                            SenhaAutorizacao = inputContaItem.SenhaAutorizacao,
                            NomeAutorizacao = inputContaItem.NomeAutorizacao,
                            ObsAutorizacao = inputContaItem.ObsAutorizacao,
                            HoraIncio = inputContaItem.HoraIncio,
                            HoraFim = inputContaItem.HoraFim,
                            ViaAcesso = inputContaItem.ViaAcesso,
                            Tecnica = inputContaItem.Tecnica,
                            ClinicaId = inputContaItem.ClinicaId,
                            FornecedorId = inputContaItem.FornecedorId,
                            NumeroNF = inputContaItem.NumeroNF,
                            IsImportaEstoque = inputContaItem.IsImportaEstoque,
                            FaturamentoConfigConvenioId = inputContaItem.FaturamentoConfigConvenioId,
                            FaturamentoPacoteId = inputContaItem.FaturamentoPacoteId
                        };


                        if (contaItemEntity.Id.Equals(0))
                        {
                            contaItemEntity.Id = await contaItemRepository.Object.InsertAndGetIdAsync(contaItemEntity);
                            await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                                OcorrenciaTexto.ContaMedicaItemCriadaPorKit(contaItemEntity.Codigo,fatItems.FirstOrDefault(x=> x.Id == contaItemEntity.FaturamentoItemId)?.Descricao,fatKit?.Descricao, (await this.GetCurrentUserAsync()).FullName),
                                TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaItem,
                                typeof(FaturamentoContaItem).FullName, contaItemEntity.Id, typeof(FaturamentoConta).FullName,
                                contaItemEntity.FaturamentoContaId));
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<DefaultReturn<DefaultReturnBool>> IncluirPacoteAvulso(CriarOuEditarPacoteModalInputDto input)
        {
            var result = new DefaultReturn<DefaultReturnBool>();
            try
            {
                using (var faturamentoContaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                using (var faturamentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var faturamentoPacoteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoPacote, long>>())
                using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var fatContaItem = new FaturamentoContaItemDto
                    {
                        FaturamentoContaId = input.ContaMedicaId,
                        FaturamentoItemId = input.PacoteId,
                        Data = input.DataInicio,
                        UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                        TerceirizadoId = input.TerceirizadoId,
                        Qtde = input.Qtde ?? 0,
                        HoraIncio = input.HoraIncio,
                        HoraFim = input.HoraFim,
                    };

                    var calculaItem = await this.CalcularValorTotalItemFaturamento(
                        new ValorTotalItemFaturamentoDto(
                            fatContaItem.FaturamentoContaId.Value, fatContaItem.Data.Value.DateTime, fatContaItem.FaturamentoItemId.Value,
                            fatContaItem.Qtde, 0, fatContaItem.UnidadeOrganizacionalId,
                            fatContaItem.TerceirizadoId, fatContaItem.CentroCustoId, fatContaItem.TurnoId, fatContaItem.TipoLeitoId)
                    );

                    if (!calculaItem.Errors.IsNullOrEmpty())
                    {
                        result.ReturnObject = new DefaultReturnBool() { Sucesso = false };
                        result.Errors = calculaItem.Errors;
                        result.Warnings = calculaItem.Warnings;
                        return result;
                    }


                    var pacote = new FaturamentoPacote();
                    var user = (await GetCurrentUserAsync()).FullName;
                    var faturamentoItem = await faturamentoItemRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.PacoteId).ConfigureAwait(false);
                    var faturamentoConta = await faturamentoContaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.ContaMedicaId).ConfigureAwait(false);

                    pacote = new FaturamentoPacote
                    {
                        Id = input.Id,
                        Inicio = input.DataInicio?.Date ?? DateTime.Now,
                        Final = input.DataFim?.Date ?? DateTime.Now,
                        FaturamentoItemId = input.PacoteId,
                        FaturamentoContaId = input.ContaMedicaId,
                        Descricao = input.Descricao,
                        Qtde = input.Qtde
                    };


                    pacote.Id = await faturamentoPacoteRepository.Object.InsertAndGetIdAsync(pacote).ConfigureAwait(false);

                    fatContaItem.FaturamentoPacoteId = pacote.Id;

                    await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                                   OcorrenciaTexto.ContaMedicaPacoteCriado(faturamentoItem.Codigo, faturamentoItem.Descricao, pacote.Descricao, faturamentoConta.Codigo, user),
                                   TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaPacote,
                                   typeof(FaturamentoPacote).FullName, pacote.Id, typeof(FaturamentoConta).FullName,
                                   pacote.FaturamentoContaId));

                    fatContaItem.COCH = calculaItem.ReturnObject.ResumoDetalhamento?.COCH ?? 0;
                    fatContaItem.ValorCOCH = calculaItem.ReturnObject.ResumoDetalhamento?.ValorCOCH ?? 0;

                    fatContaItem.HMCH = calculaItem.ReturnObject.ResumoDetalhamento?.HMCH.ToString();

                    fatContaItem.SisMoedaId = calculaItem.ReturnObject.ResumoDetalhamento?.Moeda?.Id;

                    fatContaItem.ValorFilme = calculaItem.ReturnObject.ResumoDetalhamento?.ValorFilme ?? 0;
                    fatContaItem.MetragemFilme = calculaItem.ReturnObject.ResumoDetalhamento?.MetragemFilme ?? 0;
                    fatContaItem.ValorTaxas = calculaItem.ReturnObject.ResumoDetalhamento?.ValorTaxas ?? 0;
                    fatContaItem.ValorItem = calculaItem.ReturnObject.ResumoDetalhamento?.Valor ?? 0;
                    fatContaItem.ResumoDetalhamento = calculaItem.ReturnObject.ResumoDetalhamento;

                    await this.CriarOuEditar(fatContaItem);



                    result.ReturnObject = new DefaultReturnBool() { Sucesso = true };
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    return result;
                }
            }
            catch
            {
                result.ReturnObject = new DefaultReturnBool() { Sucesso = false };
                result.Errors.Add(ErroDto.Criar("", "Erro ao incluir o Pacote"));
            }
            return result;


        }

        public async Task<DefaultReturn<FaturamentoPacoteDto>> IncluirPacote(FaturamentoIncluirPacoteDto input)
        {
            var retornoPadrao = new DefaultReturn<FaturamentoPacoteDto>
            {
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>(),
                ReturnObject = new FaturamentoPacoteDto()
            };


            if(!input.FaturamentoContaId.HasValue || input.FaturamentoContaId == 0 )
            {
                retornoPadrao.Errors.Add(ErroDto.Criar("", "Não é possivel criar pacote sem conta."));
                return retornoPadrao;
            }

            if (!input.FaturamentoItemId.HasValue || input.FaturamentoItemId == 0)
            {
                retornoPadrao.Errors.Add(ErroDto.Criar("", "Não é possivel criar pacote sem pacote."));
                return retornoPadrao;
            }
            try
            {
                using (var faturamentoContaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                using (var faturamentoContaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                using (var faturamentoPacoteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoPacote, long>>())
                using (var faturamentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var itemIds = input.Items.Select(x => x.Id);
                    var items = await faturamentoContaItemRepository.Object.GetAll().Where(x => x.FaturamentoContaId == input.FaturamentoContaId && itemIds.Contains(x.Id)).ToListAsync();

                    if (items.IsNullOrEmpty())
                    {
                        retornoPadrao.Errors.Add(ErroDto.Criar("", "Não é possivel criar pacote sem item da conta atrelado."));
                        return retornoPadrao;
                    }
                    var pacote = new FaturamentoPacote();
                    var user = (await GetCurrentUserAsync()).FullName;
                    var faturamentoItem = await faturamentoItemRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.FaturamentoItemId).ConfigureAwait(false);
                    var faturamentoConta = await faturamentoContaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.FaturamentoContaId).ConfigureAwait(false);
                    if (input.Id == 0)
                    {
                        var fatContaItem = new FaturamentoContaItemDto
                        {
                            FaturamentoContaId = input.FaturamentoContaId,
                            FaturamentoItemId = input.FaturamentoItemId,
                            Data = input.Inicio,
                            UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                            TerceirizadoId = input.TerceirizadoId,
                            Qtde = input.Quantidade ?? 0,
                            HoraIncio = input.HoraInicio,
                            HoraFim = input.HoraFim,
                        };

                        var calculaItem = await this.CalcularValorTotalItemFaturamento(
                            new ValorTotalItemFaturamentoDto(
                                fatContaItem.FaturamentoContaId.Value, fatContaItem.Data.Value.DateTime, fatContaItem.FaturamentoItemId.Value,
                                fatContaItem.Qtde, 0, fatContaItem.UnidadeOrganizacionalId,
                                fatContaItem.TerceirizadoId, fatContaItem.CentroCustoId, fatContaItem.TurnoId, fatContaItem.TipoLeitoId)
                        );

                        if (!calculaItem.Errors.IsNullOrEmpty())
                        {
                            retornoPadrao.Errors = calculaItem.Errors;
                            retornoPadrao.Warnings = calculaItem.Warnings;
                            return retornoPadrao;
                        }

                        fatContaItem.COCH = calculaItem.ReturnObject.ResumoDetalhamento?.COCH ?? 0;
                        fatContaItem.ValorCOCH = calculaItem.ReturnObject.ResumoDetalhamento?.ValorCOCH ?? 0;

                        fatContaItem.HMCH = calculaItem.ReturnObject.ResumoDetalhamento?.HMCH.ToString();

                        fatContaItem.SisMoedaId = calculaItem.ReturnObject.ResumoDetalhamento?.Moeda?.Id;

                        fatContaItem.ValorFilme = calculaItem.ReturnObject.ResumoDetalhamento?.ValorFilme ?? 0;
                        fatContaItem.MetragemFilme = calculaItem.ReturnObject.ResumoDetalhamento?.MetragemFilme ?? 0;
                        fatContaItem.ValorTaxas = calculaItem.ReturnObject.ResumoDetalhamento?.ValorTaxas ?? 0;
                        fatContaItem.ValorItem = calculaItem.ReturnObject.ResumoDetalhamento?.Valor ?? 0;
                        fatContaItem.ResumoDetalhamento = calculaItem.ReturnObject.ResumoDetalhamento;


                        pacote = new FaturamentoPacote
                        {
                            Id = input.Id,
                            Inicio = input.Inicio,
                            Final = input.Final,
                            FaturamentoItemId = input.FaturamentoItemId,
                            FaturamentoContaId = input.FaturamentoContaId,
                            Descricao = input.Descricao,
                            Qtde = input.Quantidade
                        };
                        
                        pacote.Id = await faturamentoPacoteRepository.Object.InsertAndGetIdAsync(pacote).ConfigureAwait(false);

                        fatContaItem.FaturamentoPacoteId = pacote.Id;
                        await this.CriarOuEditar(fatContaItem);

                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                                       OcorrenciaTexto.ContaMedicaPacoteCriado(faturamentoItem.Codigo, faturamentoItem.Descricao, pacote.Descricao, faturamentoConta.Codigo, user),
                                       TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaPacote,
                                       typeof(FaturamentoPacote).FullName, pacote.Id, typeof(FaturamentoConta).FullName,
                                       pacote.FaturamentoContaId));
                    }

                    foreach (var item in items)
                    {
                        item.FaturamentoPacoteId = pacote.Id;
                        await faturamentoContaItemRepository.Object.UpdateAsync(item).ConfigureAwait(false);

                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                                   OcorrenciaTexto.ContaMedicaItemAssociadaPacote(item.Codigo, item.Descricao, faturamentoItem.Descricao, pacote.Descricao, user),
                                   TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaItem,
                                   typeof(FaturamentoContaItem).FullName, item.Id, typeof(FaturamentoConta).FullName,
                                   item.FaturamentoContaId));
                    }

                    retornoPadrao.ReturnObject = FaturamentoPacoteDto.Mapear(pacote);

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    return retornoPadrao;
                }
            }
            catch (Exception e)
            {
                return retornoPadrao;
            }
        }

        public async Task ExcluirItens(long contaMedicaId, List<long> itemIds)
        {
            try
            {
                using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                using (var ocorrenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Ocorrencia, long>>())
                {
                    var items = (await contaMedicaAppService.Object.ListarItems(new FaturamentoContaItemTableFilterDto()
                    {
                        ContaMedicaId = contaMedicaId,
                        EnablePaginate = false
                    }).ConfigureAwait(false)).Items;

                    items = items.Where(x => itemIds.Contains(x.Id)).ToList();
                    var userName = (await this.GetCurrentUserAsync()).FullName;
                    foreach (var itemId in itemIds)
                    {
                        var currentItem = items.FirstOrDefault(x => x.Id == itemId);
                        await contaItemRepository.Object.DeleteAsync(itemId);
                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            OcorrenciaTexto.ContaMedicaItemRemovido(currentItem?.Codigo, currentItem?.ItemDescricao, userName),
                            TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaItem, typeof(FaturamentoConta).FullName, contaMedicaId));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task Excluir(FaturamentoContaItemDto input)
        {
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    await contaItemRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task ExcluirVM(long id)
        {
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    await contaItemRepository.Object.DeleteAsync(id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoContaItemDto> Obter(long id)
        {
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    var query = await contaItemRepository.Object
                        .GetAll()
                        .Include(m => m.FaturamentoItem)
                        .Include(m => m.Turno)
                        // .Include(m => m.TipoLeito)
                        .Include(m => m.TipoAcomodacao)
                        .Include(m => m.Terceirizado)
                        .Include(m => m.UnidadeOrganizacional)
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    var conta = FaturamentoContaItemDto.MapearFromCore(query);

                    return conta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoContaItemDto> ObterPorCodigo(string codigo)
        {
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    var query = await contaItemRepository.Object
                        .GetAll()
                        .Include(m => m.FaturamentoItem)
                        .Include(m => m.Turno)
                        // .Include(m => m.TipoLeito)
                        .Include(m => m.TipoAcomodacao)
                        .Include(m => m.Terceirizado)
                        .Include(m => m.UnidadeOrganizacional)
                        .Where(m => m.Codigo == codigo)
                        .FirstOrDefaultAsync();

                    var contaItem = FaturamentoContaItemDto.MapearFromCore(query);
                    return contaItem;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FaturamentoContaItemViewModel> ObterViewModel(long id)
        {
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                using (var fatItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                {
                    var item = await contaItemRepository.Object
                        .GetAll()
                        .Include(m => m.Turno)
                        //  .Include(m => m.TipoLeito)
                        .Include(m => m.TipoAcomodacao)
                        .Include(m => m.Terceirizado)
                        .Include(m => m.CentroCusto)
                        .Include(m => m.UnidadeOrganizacional)
                        .Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa)
                        .Include(m => m.Auxiliar1)
                        .Include(m => m.Auxiliar1.SisPessoa)
                        .Include(m => m.Auxiliar2)
                        .Include(m => m.Auxiliar2.SisPessoa)
                        .Include(m => m.Auxiliar3)
                        .Include(m => m.Auxiliar3.SisPessoa)
                        .Include(m => m.Anestesista)
                        .Include(m => m.Anestesista.SisPessoa)
                        .Include(m => m.Instrumentador)
                        .Include(m => m.Instrumentador.SisPessoa)
                        .Include(m => m.MedicoEspecialidade)
                        .Include(m => m.MedicoEspecialidade.Especialidade)
                        .Include(m => m.Auxiliar1Especialidade.Especialidade)
                        .Include(m => m.Auxiliar2Especialidade.Especialidade)
                        .Include(m => m.Auxiliar3Especialidade.Especialidade)
                        .Include(m => m.AnestesistaEspecialidade)
                        .Include(m => m.InstrumentadorEspecialidade.Especialidade)
                        .Include(m => m.FaturamentoPacote)
                        .Include(m => m.FaturamentoPacote.FaturamentoItem)
                        .Include(m => m.FaturamentoItemCobrado)
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    var i = new FaturamentoContaItemViewModel();

                    var fatItem = FaturamentoItemDto.Mapear(fatItemRepository.Object.GetAll().Include(o => o.Grupo.TipoGrupo).FirstOrDefault(y => y.Id == item.FaturamentoItemId));

                    i.Id = item.Id;
                    i.Descricao = item.Descricao;
                    i.FaturamentoItemId = item.FaturamentoItemId;
                    i.FaturamentoItemDescricao = item.FaturamentoItem != null ? item.FaturamentoItem.Descricao : string.Empty;
                    i.FaturamentoContaId = item.FaturamentoContaId;
                    i.FatItem = fatItem;
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
                    //i.TipoLeitoId                     = item.TipoLeitoId;
                    //i.TipoLeitoDescricao              = item.TipoLeito != null ? item.TipoLeito.Descricao : string.Empty;
                    i.TipoLeitoId = item.TipoAcomodacaoId;
                    i.TipoLeitoDescricao = item.TipoAcomodacao != null ? item.TipoAcomodacao.Descricao : string.Empty;
                    i.ValorTemp = item.ValorTemp;
                    i.MedicoId = item.MedicoId;
                    i.MedicoNome = item.Medico != null ? item.Medico.NomeCompleto : string.Empty;
                    i.IsMedCredenciado = item.IsMedCredenciado;
                    i.IsGlosaMedico = item.IsGlosaMedico;
                    i.MedicoId = item.MedicoId;
                    i.MedicoNome = item.Medico != null ? item.Medico.NomeCompleto : string.Empty;
                    i.IsMedCredenciado = item.IsMedCredenciado;
                    i.MedicoEspecialidadeId = item.MedicoEspecialidadeId;
                    i.MedicoEspecialidadeNome = item.MedicoEspecialidade != null ? item.MedicoEspecialidade.Especialidade.Nome : string.Empty;
                    i.Auxiliar1Id = item.Auxiliar1Id;
                    i.Auxiliar1Nome = item.Auxiliar1 != null ? item.Auxiliar1.NomeCompleto : string.Empty;
                    i.IsAux1Credenciado = item.IsAux1Credenciado;
                    i.Auxiliar1EspecialidadeId = item.Auxiliar1EspecialidadeId;
                    i.Auxiliar1EspecialidadeNome = item.Auxiliar1Especialidade != null ? item.Auxiliar1Especialidade.Especialidade.Nome : string.Empty;
                    i.Auxiliar2Id = item.Auxiliar2Id;
                    i.Auxiliar2Nome = item.Auxiliar2 != null ? item.Auxiliar2.NomeCompleto : string.Empty;
                    i.IsAux2Credenciado = item.IsAux2Credenciado;
                    i.Auxiliar2EspecialidadeId = item.Auxiliar2EspecialidadeId;
                    i.Auxiliar2EspecialidadeNome = item.Auxiliar2Especialidade != null ? item.Auxiliar2Especialidade.Especialidade.Nome : string.Empty;
                    i.Auxiliar3Id = item.Auxiliar3Id;
                    i.Auxiliar3Nome = item.Auxiliar3 != null ? item.Auxiliar3.NomeCompleto : string.Empty;
                    i.IsAux3Credenciado = item.IsAux3Credenciado;
                    i.Auxiliar3EspecialidadeId = item.Auxiliar3EspecialidadeId;
                    i.Auxiliar3EspecialidadeNome = item.Auxiliar3Especialidade != null ? item.Auxiliar3Especialidade.Especialidade.Nome : string.Empty;
                    i.AnestesistaId = item.AnestesistaId;
                    i.AnestesistaNome = item.Anestesista != null ? item.Anestesista.NomeCompleto : string.Empty;
                    i.AnestesistaEspecialidadeId = item.AnestesistaEspecialidadeId;
                    i.InstrumentadorId = item.InstrumentadorId;
                    i.InstrumentadorNome = item.Instrumentador != null ? item.Instrumentador.NomeCompleto : string.Empty;
                    i.IsInstCredenciado = item.IsInstrCredenciado;
                    i.InstrumentadorEspecialidadeId = item.InstrumentadorEspecialidadeId;
                    i.InstrumentadorEspecialidadeNome = item.InstrumentadorEspecialidade != null ? item.InstrumentadorEspecialidade.Especialidade.Nome : string.Empty;
                    i.FaturamentoContaKitId = item.FaturamentoContaKitId;
                    i.IsCirurgia = item.IsCirurgia;
                    i.ValorAprovado = item.ValorAprovado;
                    i.ValorTaxas = item.ValorTaxas;
                    i.IsValorItemManual = item.IsValorItemManual;
                    i.ValorItem = item.ValorItem;
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
                    // STATUSNOVO ALTERAR          i.StatusEntrega                   = item.StatusEntrega;
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
                    i.FaturamentoConfigConvenioId = item.FaturamentoConfigConvenioId;
                    i.FaturamentoPacoteId = item.FaturamentoPacoteId;

                    if (item.FaturamentoPacote != null)
                    {
                        i.FaturamentoPacote = new FaturamentoPacoteDto { Id = item.FaturamentoPacote.Id, Descricao = item.FaturamentoPacote.FaturamentoItem.Descricao };
                    }

                    if (item.FaturamentoItemCobrado != null)
                    {
                        i.FaturamentoItemCobrado = FaturamentoItemDto.Mapear(item.FaturamentoItemCobrado);
                    }

                    return i;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoContaItemReportModel> ObterReportModel(long id)
        {
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    var item = await contaItemRepository.Object
                        .GetAll()
                        .Include(m => m.FaturamentoItem)
                        .Include(m => m.Turno)
                        //.Include(m => m.TipoLeito)
                        .Include(m => m.TipoAcomodacao)
                        .Include(m => m.Terceirizado)
                        .Include(m => m.UnidadeOrganizacional)
                        .Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa)
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    var i = new FaturamentoContaItemReportModel();

                    i.Id = item.Id;
                    i.Descricao = item.Descricao;
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
                    //i.TipoLeitoId                    = item.TipoLeitoId;
                    //i.TipoLeitoDescricao             = item.TipoLeito != null ? item.TipoLeito.Descricao : string.Empty;

                    i.TipoLeitoId = item.TipoAcomodacaoId;
                    i.TipoLeitoDescricao = item.TipoAcomodacao != null ? item.TipoAcomodacao.Descricao : string.Empty;

                    i.IsGlosaMedico = item.IsGlosaMedico;
                    i.ValorTemp = item.ValorTemp;
                    i.MedicoId = item.MedicoId;
                    i.MedicoNome = item.Medico != null ? item.Medico.NomeCompleto : string.Empty;
                    i.IsMedCrendenciado = item.IsMedCredenciado;
                    i.IsGlosaMedico = item.IsGlosaMedico;
                    i.MedicoEspecialidadeId = item.MedicoEspecialidadeId;
                    i.MedicoEspecialidadeNome = item.MedicoEspecialidade != null ? item.MedicoEspecialidade.Especialidade.Nome : string.Empty;
                    i.FaturamentoContaKitId = item.FaturamentoContaKitId;
                    i.IsCirurgia = item.IsCirurgia;
                    i.ValorAprovado = item.ValorAprovado;
                    i.ValorTaxas = item.ValorTaxas;
                    i.IsValorItemManual = item.IsValorItemManual;
                    i.ValorItem = item.ValorItem;
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
                    // STATUSNOVO ALTERAR         i.StatusEntrega                  = item.StatusEntrega;
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

                    return i;
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
        }

        //public async Task<float> CalcularValorUntario ()
        //{
        //    float valor = 0f;


        //    return valor;
        //}

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    var query = from p in contaItemRepository.Object.GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Codigo.Contains(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search))
                                orderby p.Descricao ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                    //paginação 
                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync();

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<float> CalcularValorUnitarioContaItemViaFront(long contaItemId)
        {
            if (contaItemId == 0)
                return 0f;

            try
            {
                using (var configConvenioAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoConfigConvenioAppService>())
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                {
                    var contaItem = contaItemRepository.Object.GetAll()
                        .Include(x => x.FaturamentoConta)
                        .Include(x => x.FaturamentoItem)
                        .Include(x => x.FaturamentoItem.Grupo)
                        .Include(x => x.FaturamentoItem.SubGrupo)
                        .FirstOrDefault(x => x.Id == contaItemId);

                    var input = new CalculoContaItemInput
                    {
                        FatContaItemDto = FaturamentoContaItemDto.MapearFromCore(contaItem)
                    };
                    var contaCalculoItem = new ContaCalculoItem
                    {
                        EmpresaId = (long)input.FatContaItemDto.FaturamentoConta?.EmpresaId,
                        ConvenioId = (long)input.FatContaItemDto.FaturamentoConta?.ConvenioId
                    };

                    if (input.FatContaItemDto.FaturamentoConta?.PlanoId != null)
                    {
                        contaCalculoItem.PlanoId = (long)input.FatContaItemDto.FaturamentoConta?.PlanoId;
                    }

                    input.conta = contaCalculoItem;
                    var configConvenioInput = new ListarFaturamentoConfigConveniosInput
                    {
                        ConvenioId = contaCalculoItem.ConvenioId,
                        EmpresaId = contaCalculoItem.EmpresaId,
                        PlanoId = contaCalculoItem.PlanoId,

                        GrupoId = input.FatContaItemDto.FaturamentoItem.GrupoId,
                        SubGrupoId = input.FatContaItemDto.FaturamentoItem.SubGrupoId,
                        ItemId = input.FatContaItemDto.FaturamentoItemId
                    };

                    var configsConvenio = await configConvenioAppService.Object.ListarPorConvenio(configConvenioInput);

                    // por empresa
                    var configsPorEmpresa = configsConvenio.Items
                        .Where(x => x.EmpresaId != null)
                        .Where(c => c.EmpresaId == contaCalculoItem.EmpresaId);

                    // todas emprsas
                    var configsTodasEmpresas = configsConvenio.Items
                        .Where(x => x.EmpresaId == null || x.EmpresaId == 0);

                    // por plano
                    var configsPorPlano = configsConvenio.Items
                        .Where(x => x.PlanoId != null)
                        .Where(c => c.PlanoId == contaCalculoItem.PlanoId);

                    // Todos planos
                    var configsTodosPlanos = configsConvenio.Items
                        .Where(x => x.PlanoId == null || x.PlanoId == 0);

                    // por grupo
                    var configsPorGrupo = configsConvenio.Items
                        .Where(x => x.GrupoId != null)
                        .Where(c => c.GrupoId == input.FatContaItemDto.FaturamentoItem.GrupoId);

                    // todos grupos
                    var configsTodosGrupos = configsConvenio.Items
                        .Where(x => x.GrupoId == null || x.GrupoId == 0);

                    // por subGrupo
                    var configsPorSubGrupo = configsConvenio.Items
                        .Where(x => x.GrupoId != null)
                        .Where(c => c.SubGrupoId == input.FatContaItemDto.FaturamentoItem.SubGrupoId);

                    // todos subGrupos
                    var configsTodosSubGrupos = configsConvenio.Items
                        .Where(x => x.SubGrupoId == null || x.SubGrupoId == 0);

                    // por item
                    var configsPorItem = configsConvenio.Items
                        .Where(x => x.ItemId != null)
                        .Where(c => c.ItemId == input.FatContaItemDto.FaturamentoItem.Id);

                    // todos itens
                    var configsTodosItens = configsConvenio.Items
                        .Where(x => x.ItemId == null || x.ItemId == 0);

                    input.configsPorEmpresa = configsPorEmpresa.ToArray();
                    input.configsTodasEmpresas = configsTodasEmpresas.ToArray();
                    input.configsPorPlano = configsPorPlano.ToArray();
                    input.configsTodosPlanos = configsTodosPlanos.ToArray();
                    input.configsPorGrupo = configsPorGrupo.ToArray();
                    input.configsTodosGrupos = configsTodosGrupos.ToArray();
                    input.configsPorSubGrupo = configsPorSubGrupo.ToArray();
                    input.configsTodosSubGrupos = configsTodosSubGrupos.ToArray();
                    input.configsPorItem = configsPorItem.ToArray();
                    input.configsTodosItens = configsTodosItens.ToArray();
                    input.configsConvenio = configsConvenio.Items.ToList();

                    return await this.CalcularValorUnitarioContaItem(input);
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(L("ErroCalculoValorUnitario"), e);
            }
        }

        public async Task<float> CalcularValorUnitarioContaItem(CalculoContaItemInput input)
        {
            try
            {
                using (var tabelaAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoTabelaAppService>())
                using (var cotacaoAppService = IocManager.Instance.ResolveAsDisposable<ISisMoedaCotacaoAppService>())
                using (var tabelaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemTabelaAppService>())
                using (var moedaAppService = IocManager.Instance.ResolveAsDisposable<ISisMoedaAppService>())
                using (var configConvenioAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoConfigConvenioAppService>())
                {
                    if (input.configsConvenio == null)
                    {
                        var configConvenioInput = new ListarFaturamentoConfigConveniosInput
                        {
                            ConvenioId = input.conta.ConvenioId,
                            EmpresaId = input.conta.EmpresaId,
                            PlanoId = input.conta.PlanoId,

                            GrupoId = input.FatContaItemDto.FaturamentoItem.GrupoId,
                            SubGrupoId = input.FatContaItemDto.FaturamentoItem.SubGrupoId,
                            ItemId = input.FatContaItemDto.FaturamentoItemId
                        };

                        var configsConvenio = await configConvenioAppService.Object.ListarPorConvenio(configConvenioInput);
                        input.configsConvenio = configsConvenio.Items.ToList();
                    }

                    long? tabelaId;

                    // Achando tabela adequada via analise combinatoria das 'configConvenio'

                    // 1 - caso mais especifico
                    var caso1 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId != null && x.ItemId != 0));

                    // 5
                    var caso5 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 9
                    var caso9 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));


                    // 13
                    var caso13 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId == null || x.GrupoId == 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));


                    // 14
                    var caso14 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId == null || x.GrupoId == 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 2
                    var caso2 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId != null && x.ItemId != 0));
                    // 3
                    var caso3 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId != null && x.ItemId != 0));
                    // 4
                    var caso4 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId != null && x.ItemId != 0));

                    // 6
                    var caso6 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 7
                    var caso7 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 8
                    var caso8 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId != null && x.SubGrupoId != 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 10
                    var caso10 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId != null && x.EmpresaId != 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 11
                    var caso11 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 12
                    var caso12 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId != null && x.GrupoId != 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 15
                    var caso15 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId != null && x.PlanoId != 0) &&
                             (x.GrupoId == null || x.GrupoId == 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));

                    // 16 - caso mais generico
                    var caso16 = input.configsConvenio.FirstOrDefault(
                        x => (x.EmpresaId == null || x.EmpresaId == 0) &&
                             (x.PlanoId == null || x.PlanoId == 0) &&
                             (x.GrupoId == null || x.GrupoId == 0) &&
                             (x.SubGrupoId == null || x.SubGrupoId == 0) &&
                             (x.ItemId == null || x.ItemId == 0));

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

                    var casosConfigList = new List<CasoConfig>();

                    foreach (var cc in casosConfig)
                    {
                        if (cc.Config != null)
                        {
                            casosConfigList.Add(cc);
                        }
                    }


                    // Obtendo config com maior prioridade
                    var config = casosConfigList.IsNullOrEmpty() ? null : casosConfigList.OrderByDescending(p => p.Prioridade).FirstOrDefault().Config;
                    tabelaId = config?.TabelaId;

                    input.FaturamentoConfigConvenioId = config?.Id;



                    // fim - via analise combinatoria

                    // ======================= PRECO =========================

                    // Obter preco vigente
                    var listarParaTabelaInput = new ListarFaturamentoItensTabelaInput
                    {
                        TabelaId = tabelaId.ToString()
                    };

                    var precosPorTabela = (await tabelaItemAppService.Object.ListarParaFatTabela(listarParaTabelaInput)).Items;

                    var itemCobranca = this.ObterItemASerCobrado(input.conta.ConvenioId, input.conta.PlanoId, input.FatContaItemDto.FaturamentoItemId ?? 0);


                    // var precosPorFatItem = precosPorTabela.Where(_ => _.ItemId == input.FatContaItemDto.FaturamentoItemId);
                    var precosPorFatItem = precosPorTabela.Where(_ => _.ItemId == itemCobranca);
                    var preco = precosPorFatItem
                        .Where(_ => _.VigenciaDataInicio <= DateTime.Now)
                        .OrderByDescending(_ => _.VigenciaDataInicio).FirstOrDefault();

                    if (itemCobranca != input.FatContaItemDto.FaturamentoItemId)
                    {
                        input.FaturamentoItemCobradoId = itemCobranca;
                    }

                    // ======================= MOEDA E COTACAO =========================

                    if (preco == null)
                        return 0f;

                    var moeda = await moedaAppService.Object.Obter((long)(preco.SisMoedaId ?? 0));
                    var cotacaoInput = new ListarSisMoedaCotacoesInput
                    {
                        Filtro = moeda.Id.ToString()
                    };

                    // Buscar cotacoes por convenio



                    var cotacoesPorConvenio = (await cotacaoAppService.Object.ListarPorMoeda(cotacaoInput)).Items.Where(_ => _.ConvenioId == input.conta.ConvenioId);

                    // Cotacoes por Empresa
                    var cotacoesPorEmpresa = cotacoesPorConvenio.Where(_ => _.EmpresaId == input.conta.EmpresaId);

                    // A cotacao padrao inicial eh por Convenio, se houver configuracao especifica, sera sobrescrita no fluxo abaixo
                    var cotacao = cotacoesPorConvenio
                                .Where(_ => _.DataInicio <= DateTime.Now /*&& _.DataFinal >= DateTime.Now*/)
                                .OrderByDescending(_ => _.DataInicio)
                                .FirstOrDefault();

                    // Filtrar cotacoes por Plano
                    var cotacoesPorPlano = cotacoesPorEmpresa //por que aqui nao eh por convenio?
                        .Where(x => x.PlanoId != null)
                        .Where(c => c.PlanoId == input.conta.PlanoId); // se planoId da conta for null (o que nao deveria existir) vai pegar todos com planoId null

                    // Caso haja cotacao para um plano especifico
                    if (cotacoesPorPlano.Any())
                    {
                        // Por subGrupo especifico
                        var cotacaoPorSubGrupo = cotacoesPorPlano
                            .Where(_ => _.SubGrupoId == input.FatContaItemDto.FaturamentoItem.SubGrupoId && _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now)
                            //?
                            .OrderByDescending(_ => _.DataInicio)
                            .FirstOrDefault();

                        if (cotacaoPorSubGrupo != null)
                        {
                            cotacao = cotacaoPorSubGrupo;
                        }
                        else
                        {
                            // Por grupo especifico
                            var cotacaoPorGrupo = cotacoesPorPlano
                                .Where(_ => _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now && _.GrupoId == input.FatContaItemDto.FaturamentoItem.GrupoId)
                                .OrderByDescending(_ => _.DataInicio)
                                .FirstOrDefault();

                            if (cotacaoPorGrupo != null)
                            {
                                cotacao = cotacaoPorGrupo;
                            }
                        }
                    }
                    // Caso seja para todos os planos
                    else
                    {
                        // Por subGrupo
                        var cotacaoPorSubGrupo = cotacoesPorConvenio
                            .Where(_ => _.DataInicio <= DateTime.Now /*&& _.DataFinal >= DateTime.Now*/)
                            .OrderByDescending(_ => _.DataInicio)
                            .FirstOrDefault(_ => _.SubGrupoId == input.FatContaItemDto.FaturamentoItem.SubGrupoId);

                        if (cotacaoPorSubGrupo != null)
                        {
                            cotacao = cotacaoPorSubGrupo;
                        }
                        else
                        {
                            // Por grupo
                            var cotacaoPorGrupo = cotacoesPorConvenio
                                .Where(_ => _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now)
                                .OrderByDescending(_ => _.DataInicio)
                                .FirstOrDefault(_ => _.GrupoId == input.FatContaItemDto.FaturamentoItem.GrupoId);

                            if (cotacaoPorGrupo != null)
                            {
                                cotacao = cotacaoPorGrupo;
                            }
                        }

                    }

                    // Filme
                    cotacaoInput.Filtro = "1";//AQUI DEVE SER A ID FIXA DA MOEDA 'FILME' - CRIAR SEED NO EF PARA MOEDAS 'FIXAS' DO SISTEMA
                    var cotacaoFilme = (await cotacaoAppService.Object.ListarPorMoeda(cotacaoInput))
                                            .Items
                                            .Where(_ => _.DataInicio <= DateTime.Now)
                                            .OrderByDescending(_ => _.DataInicio)
                                            .FirstOrDefault();

                    var totalFilme = (cotacaoFilme?.Valor ?? 0) * input.FatContaItemDto.MetragemFilme;

                    // Porte
                    var tabela = await tabelaAppService.Object.Obter((long)tabelaId);

                    if (tabela != null)
                    {
                        input.TabelaUilizada = tabela.Descricao;
                    }

                    var totalPorte = 0f;
                    tabela.IsCBHPM = true;
                    if (tabela.IsCBHPM)
                    {
                        cotacaoInput.Filtro = "5"; // Valor Id fixo setado pelo Seed para UCO
                        cotacaoInput.ConvenioId = input.conta.ConvenioId.ToString();
                        cotacaoInput.IsUco = true;
                        var cotacaoPorte = (await cotacaoAppService.Object.ListarPorMoeda(cotacaoInput))
                                                .Items
                                                .Where(_ => _.DataInicio <= DateTime.Now)
                                                .OrderByDescending(_ => _.DataInicio)
                                                .FirstOrDefault();

                        if (!preco.COCH.HasValue)
                            preco.COCH = 0;

                        totalPorte = (cotacaoPorte?.Valor ?? 0) * (long)(preco.COCH ?? 0);
                    }

                    // Valor unitario
                    var valorUnitario = ((preco.Preco * (cotacao?.Valor ?? 1)) + totalFilme + totalPorte);
                    return valorUnitario;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                return 0f;
            }
        }

        public async Task<float> CalcularValorUnitarioItem(long empresaId, long convenioId, long planoId, FaturamentoContaItemDto fatContaItemDto)
        {
            CalculoContaItemInput calculoContaItemInput = new CalculoContaItemInput
            {
                conta = new ContaCalculoItem
                {
                    EmpresaId = empresaId,
                    ConvenioId = convenioId,
                    PlanoId = planoId
                },

                FatContaItemDto = fatContaItemDto
            };

            var result = await CalcularValorUnitarioContaItem(calculoContaItemInput);

            fatContaItemDto.TabelaUtilizada = calculoContaItemInput.TabelaUilizada;
            fatContaItemDto.FaturamentoConfigConvenioId = calculoContaItemInput.FaturamentoConfigConvenioId;

            return result;
        }

        public async Task<DefaultReturn<ValorCodigoTabela>> CalcularValorTotalItemFaturamento(ValorTotalItemFaturamentoDto input)
        {
            using (var faturamentoDomainService = IocManager.Instance.ResolveAsDisposable<IFaturamentoDomainService>())
            {
                var result = await faturamentoDomainService.Object.ValorFaturamentoItem(input);

                if (input.Percentual == 0)
                {
                    input.Percentual = 1;
                    result.ReturnObject.ResumoDetalhamento.Percentual = 1;
                }
                result.ReturnObject.ValorTotal = (float)Math.Round((float)(result.ReturnObject.Valor * input.Qtd) * (float)(input.Percentual/100),2);


                return result;
            }
        }

        public async Task<ValorCodigoTabela> CalcularValorItemFaturamento(long contaId, long faturamentoItemId)
        {
            using (var fatItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
            using (var faturamentoContaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
            {
                var faturamentoItem = fatItemRepository.Object.GetAll().FirstOrDefault(w => w.Id == faturamentoItemId);

                FaturamentoItemDto faturamentoItemDto = null;

                if (faturamentoItem != null)
                {
                    faturamentoItemDto = FaturamentoItemDto.Mapear(faturamentoItem);
                }


                var conta = faturamentoContaRepository.Object.GetAll().FirstOrDefault(w => w.Id == contaId);


                var calculoContaItemInput = new CalculoContaItemInput
                {
                    conta = new ContaCalculoItem()
                };

                if (conta != null)
                {
                    calculoContaItemInput.conta.EmpresaId = conta.EmpresaId ?? 0;
                    calculoContaItemInput.conta.ConvenioId = conta.ConvenioId ?? 0;
                    calculoContaItemInput.conta.PlanoId = conta.PlanoId ?? 0;
                }
                calculoContaItemInput.FatContaItemDto = new FaturamentoContaItemDto { FaturamentoItem = faturamentoItemDto, FaturamentoItemId = faturamentoItemId };

                var result = await this.CalcularValorUnitarioContaItem(calculoContaItemInput);


                var valorCodigoTabela = new ValorCodigoTabela
                {
                    Valor = result,
                    TabelaId = calculoContaItemInput.FaturamentoConfigConvenioId,
                    FaturamentoItemCobradoId = calculoContaItemInput.FaturamentoItemCobradoId
                };

                return valorCodigoTabela;
            }
        }


        long? ObterItemASerCobrado(long convenioId, long planoId, long itemOriginalId)
        {
            using (var itemConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItemConfig, long>>())
            {
                var config = itemConfigRepository.Object
                                                 .GetAll()
                                                 .FirstOrDefault(
                                                     w => (w.ConvenioId == convenioId || w.ConvenioId == null)
                                                          && (w.PlanoId == planoId || w.PlanoId == null)
                                                          && w.ItemId == itemOriginalId);
                if (config != null)
                {
                    return config.ItemCobrarId;
                }

                return itemOriginalId;
            }
        }

        public async Task<DefaultReturn<FaturamentoContaItemInsertDto>> InserirItensContaFaturamento(FaturamentoContaItemInsertDto itensConta)
        {
            var _retornoPadrao = new DefaultReturn<FaturamentoContaItemInsertDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();


            FaturamentoConta faturamentoConta = null;



            try
            {
                using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
                using (var fatItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var faturamentoContaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var atendimento = atendimentoRepository.Object
                                                          .GetAll()
                                                          .FirstOrDefault(w => w.Id == itensConta.AtendimentoId);

                    if (itensConta.ContaId == null)
                    {

                        var faturamentoContas = faturamentoContaRepository.Object.GetAll()
                                                                          .Where(w => w.AtendimentoId == itensConta.AtendimentoId)
                                                                          .ToList();

                        faturamentoConta = atendimento.IsAmbulatorioEmergencia ? faturamentoContas.FirstOrDefault() : faturamentoContas.LastOrDefault();
                    }
                    else
                    {
                        faturamentoConta = faturamentoContaRepository.Object
                                                                      .GetAll()
                                                                      .FirstOrDefault(w => w.Id == itensConta.ContaId);
                    }

                    if (faturamentoConta != null)
                    {


                        foreach (var itemFaturamento in itensConta.ItensFaturamento)
                        {
                            var faturamentoContaItem = new FaturamentoContaItem
                            {
                                FaturamentoItemId = itemFaturamento.Id,
                                FaturamentoItem = fatItemRepository.Object
                                                                    .GetAll()
                                                                    .FirstOrDefault(w => w.Id == itemFaturamento.Id),

                                // long.Parse("");

                                CentroCustoId = itensConta.CentroCustoId,
                                Data = (itensConta.Data == null || itensConta.Data == DateTime.MinValue) ? DateTime.Now : itensConta.Data,
                                FaturamentoContaId = faturamentoConta.Id,
                                MedicoId = itensConta.MedicoId,
                                Observacao = itensConta.Obs,
                                Qtde = itemFaturamento.Qtde,
                                TipoAcomodacaoId = itemFaturamento.TipoLeitoId,
                                TurnoId = itensConta.TurnoId,
                                UnidadeOrganizacionalId = itensConta.UnidadeOrganizacionalId,
                                HoraIncio = itemFaturamento.HoraIncio,
                                HoraFim = itemFaturamento.HoraFim,
                                FaturamentoPacoteId = itemFaturamento.FaturamentoPacoteId,
                                FaturamentoContaKitId = itemFaturamento.FaturamentoContaKitId,
                                TerceirizadoId = itemFaturamento.TerceirizadoId,
                                MedicoEspecialidadeId = itemFaturamento.MedicoEspecialidadeId
                            };

                            var contaCalculoItem = new ContaCalculoItem
                            {
                                EmpresaId = (long)atendimento.EmpresaId,
                                ConvenioId = (long)atendimento.ConvenioId,
                                PlanoId = (long)atendimento.PlanoId
                            };

                            CalculoContaItemInput calculoContaItemInput = new CalculoContaItemInput
                            {
                                conta = contaCalculoItem,
                                FatContaItemDto = FaturamentoContaItemDto.MapearFromCore(faturamentoContaItem)
                            };

                            faturamentoContaItem.ValorItem = await this.CalcularValorUnitarioContaItem(calculoContaItemInput);
                            faturamentoContaItem.FaturamentoConfigConvenioId = calculoContaItemInput.FaturamentoConfigConvenioId;
                            faturamentoContaItem.FaturamentoItem = null;
                            faturamentoContaItem.FaturamentoConta = null;
                            await contaItemRepository.Object.InsertAsync(faturamentoContaItem);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return _retornoPadrao;
        }



        public void ExcluirPacote(long contaItemId)
        {

            using (var contaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var contaItem = contaItemRepository.Object.GetAll()
                                                .Where(w => w.Id == contaItemId)
                                                .FirstOrDefault();


                contaItem.FaturamentoPacoteId = null;


                unitOfWork.Complete();
                unitOfWorkManager.Object.Current.SaveChanges();
                unitOfWork.Dispose();
            }
        }

        public async Task<IEnumerable<FaturamentoContaItemDto>> ObterPorContaKit(long contaKitId, long contaId)
        {
            using (var contaItemRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoContaItem, long>>())
            {
                var contaItems = await contaItemRepository.Object.GetAll().AsNoTracking()
                    .Include(x=> x.FaturamentoContaKit)
                    .Include(x=> x.FaturamentoContaKit.FaturamentoKit)
                    .Where(x =>
                    x.FaturamentoContaKitId == contaKitId && x.FaturamentoContaId == contaId).ToListAsync();

                return contaItems.Select(x => FaturamentoContaItemDto.MapearFromCore(x)).ToList();

            }
        }
    }

    public class FaturamentoContaItemViewModel
    {
        public float ValorUnitario { get; set; }
        public float ValorTotal { get; set; }
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long? FaturamentoItemId { get; set; }
        public string FaturamentoItemDescricao { get; set; }
        public long? FaturamentoContaId { get; set; }
        public FaturamentoItemDto FatItem { get; set; }
        public DateTimeOffset? Data { get; set; }
        public float Qtde { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public string UnidadeOrganizacionalDescricao { get; set; }
        public long? TerceirizadoId { get; set; }
        public string TerceirizadoDescricao { get; set; }
        public long? CentroCustoId { get; set; }
        public string CentroCustoDescricao { get; set; }
        public long? TurnoId { get; set; }
        public string TurnoDescricao { get; set; }
        public long? TipoLeitoId { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public float ValorTemp { get; set; }
        public long? MedicoId { get; set; }
        public string MedicoNome { get; set; }
        public bool IsMedCredenciado { get; set; }
        public long? MedicoEspecialidadeId { get; set; }
        public string MedicoEspecialidadeNome { get; set; }
        public long? Auxiliar1Id { get; set; }
        public string Auxiliar1Nome { get; set; }
        public bool IsAux1Credenciado { get; set; }
        public long? Auxiliar1EspecialidadeId { get; set; }
        public string Auxiliar1EspecialidadeNome { get; set; }
        public long? Auxiliar2Id { get; set; }
        public string Auxiliar2Nome { get; set; }
        public bool IsAux2Credenciado { get; set; }
        public long? Auxiliar2EspecialidadeId { get; set; }
        public string Auxiliar2EspecialidadeNome { get; set; }
        public long? Auxiliar3Id { get; set; }
        public string Auxiliar3Nome { get; set; }
        public bool IsAux3Credenciado { get; set; }
        public long? Auxiliar3EspecialidadeId { get; set; }
        public string Auxiliar3EspecialidadeNome { get; set; }
        public long? AnestesistaId { get; set; }
        public string AnestesistaNome { get; set; }
        public bool IsAnestCredenciado { get; set; }
        public long? AnestesistaEspecialidadeId { get; set; }
        public string AnestesistaEspecialidadeNome { get; set; }
        public long? InstrumentadorId { get; set; }
        public string InstrumentadorNome { get; set; }
        public bool IsInstCredenciado { get; set; }
        public long? InstrumentadorEspecialidadeId { get; set; }
        public string InstrumentadorEspecialidadeNome { get; set; }
        public bool IsGlosaMedico { get; set; }
        public long? FaturamentoContaKitId { get; set; }
        public bool IsCirurgia { get; set; }
        public float ValorAprovado { get; set; }
        public float ValorTaxas { get; set; }
        public bool IsValorItemManual { get; set; }
        public float ValorItem { get; set; }
        public string HMCH { get; set; }
        public float ValorFilme { get; set; }
        public float ValorFilmeAprovado { get; set; }
        public float ValorCOCH { get; set; }
        public float ValorCOCHAprovado { get; set; }
        public float Percentual { get; set; }
        public bool IsInstrCredenciado { get; set; }
        public float ValorTotalRecuperado { get; set; }
        public float ValorTotalRecebido { get; set; }
        public float MetragemFilme { get; set; }
        public float MetragemFilmeAprovada { get; set; }
        public float COCH { get; set; }
        public float COCHAprovado { get; set; }
        public string StatusEntrega { get; set; }
        public bool IsRecuperaMedico { get; set; }
        public bool IsRecebeAuxiliar1 { get; set; }
        public bool IsGlosaAuxiliar1 { get; set; }
        public bool IsRecuperaAuxiliar1 { get; set; }
        public bool IsRecebeAuxiliar2 { get; set; }
        public bool IsGlosaAuxiliar2 { get; set; }
        public bool IsRecuperaAuxiliar2 { get; set; }
        public bool IsRecebeAuxiliar3 { get; set; }
        public bool IsGlosaAuxiliar3 { get; set; }
        public bool IsRecuperaAuxiliar3 { get; set; }
        public bool IsRecebeInstrumentador { get; set; }
        public bool IsGlosaInstrumentador { get; set; }
        public bool IsRecuperaInstrumentador { get; set; }
        public string Observacao { get; set; }
        public int QtdeRecuperada { get; set; }
        public int QtdeAprovada { get; set; }
        public int QtdeRecebida { get; set; }
        public float ValorMoedaAprovado { get; set; }
        public long? SisMoedaId { get; set; }
        public string SisMoedaNome { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public string SenhaAutorizacao { get; set; }
        public string NomeAutorizacao { get; set; }
        public string ObsAutorizacao { get; set; }
        public DateTime? HoraIncio { get; set; }
        public DateTime? HoraFim { get; set; }
        public string ViaAcesso { get; set; }
        public string Tecnica { get; set; }
        public string ClinicaId { get; set; }
        public long? FornecedorId { get; set; }
        public string FornecedorNome { get; set; }
        public string NumeroNF { get; set; }
        public bool IsImportaEstoque { get; set; }
        public string Tipo { get; set; }
        public string Grupo { get; set; }
        public long? FaturamentoConfigConvenioId { get; set; }
        public string Pacote { get; set; }
        public long? FaturamentoPacoteId { get; set; }
        public FaturamentoPacoteDto FaturamentoPacote { get; set; }
        public bool IsPacote { get; set; }

        public long? FaturamentoItemCobradoId { get; set; }
        public FaturamentoItemDto FaturamentoItemCobrado { get; set; }

    }

    public class FaturamentoContaItemReportModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long? FaturamentoItemId { get; set; }
        public string FaturamentoItemCodigo { get; set; }
        public string FaturamentoItemDescricao { get; set; }
        public long? FaturamentoContaId { get; set; }
        public DateTimeOffset? Data { get; set; }
        public double Qtde { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public string UnidadeOrganizacionalDescricao { get; set; }
        public long? TerceirizadoId { get; set; }
        public string TerceirizadoDescricao { get; set; }
        public long? CentroCustoId { get; set; }
        public string CentroCustoDescricao { get; set; }
        public long? TurnoId { get; set; }
        public string TurnoDescricao { get; set; }
        public long? TipoLeitoId { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public float ValorTemp { get; set; }
        public long? MedicoId { get; set; }
        public string MedicoNome { get; set; }
        public bool IsMedCrendenciado { get; set; }
        public bool IsGlosaMedico { get; set; }
        public long? MedicoEspecialidadeId { get; set; }
        public string MedicoEspecialidadeNome { get; set; }
        public long? FaturamentoContaKitId { get; set; }
        public bool IsCirurgia { get; set; }
        public float ValorAprovado { get; set; }
        public float ValorTaxas { get; set; }
        public bool IsValorItemManual { get; set; }
        public double ValorItem { get; set; }
        public string HMCH { get; set; }
        public float ValorFilme { get; set; }
        public float ValorFilmeAprovado { get; set; }
        public float ValorCOCH { get; set; }
        public float ValorCOCHAprovado { get; set; }
        public float Percentual { get; set; }
        public bool IsInstrCredenciado { get; set; }
        public float ValorTotalRecuperado { get; set; }
        public float ValorTotalRecebido { get; set; }
        public float MetragemFilme { get; set; }
        public float MetragemFilmeAprovada { get; set; }
        public float COCH { get; set; }
        public float COCHAprovado { get; set; }
        public string StatusEntrega { get; set; }
        public bool IsRecuperaMedico { get; set; }
        public bool IsAux1Credenciado { get; set; }
        public bool IsRecebeAuxiliar1 { get; set; }
        public bool IsGlosaAuxiliar1 { get; set; }
        public bool IsRecuperaAuxiliar1 { get; set; }
        public bool IsAux2Credenciado { get; set; }
        public bool IsRecebeAuxiliar2 { get; set; }
        public bool IsGlosaAuxiliar2 { get; set; }
        public bool IsRecuperaAuxiliar2 { get; set; }
        public bool IsAux3Credenciado { get; set; }
        public bool IsRecebeAuxiliar3 { get; set; }
        public bool IsGlosaAuxiliar3 { get; set; }
        public bool IsRecuperaAuxiliar3 { get; set; }
        public bool IsRecebeInstrumentador { get; set; }
        public bool IsGlosaInstrumentador { get; set; }
        public bool IsRecuperaInstrumentador { get; set; }
        public string Observacao { get; set; }
        public int QtdeRecuperada { get; set; }
        public int QtdeAprovada { get; set; }
        public int QtdeRecebida { get; set; }
        public float ValorMoedaAprovado { get; set; }
        public long? SisMoedaId { get; set; }
        public string SisMoedaNome { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public string SenhaAutorizacao { get; set; }
        public string NomeAutorizacao { get; set; }
        public string ObsAutorizacao { get; set; }
        public DateTime? HoraIncio { get; set; }
        public DateTime? HoraFim { get; set; }
        public string ViaAcesso { get; set; }
        public string Tecnica { get; set; }
        public string ClinicaId { get; set; }
        public long? FornecedorId { get; set; }
        public string FornecedorNome { get; set; }
        public string NumeroNF { get; set; }
        public bool IsImportaEstoque { get; set; }
        public string Tipo { get; set; }
        public string Grupo { get; set; }

    }
}
