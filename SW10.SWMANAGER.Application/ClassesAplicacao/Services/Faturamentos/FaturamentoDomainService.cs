using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.MicroKernel.Registration;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Honorarios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Taxas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Calculos
{
    public class FaturamentoDomainService : SWMANAGERDomainServiceBase, IFaturamentoDomainService
    {
        public async Task<DefaultReturn<ValorCodigoTabela>> ValorFaturamentoItem(ValorTotalItemFaturamentoDto input)
        {
            var result = new DefaultReturn<ValorCodigoTabela>
            {
                ReturnObject = new ValorCodigoTabela()
            };

            if (input.Data == DateTime.MinValue)
            {
                result.Errors.Add(ErroDto.Criar("", "Campo Data é obrigatório"));
                return result;
            }

            if (input.Percentual == 0)
            {
                input.Percentual = 100;
            }

            using (var fatItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
            using (var faturamentoContaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
            {
                var resumoDetalhamentoDto = new ResumoDetalhamento();
                var faturamentoItem = fatItemRepository.Object.GetAll().AsNoTracking()
                    .Select(x => new { x.Id, x.GrupoId, x.SubGrupoId })
                    .FirstOrDefault(w => w.Id == input.FaturamentoItemId);

                var conta = faturamentoContaRepository.Object
                    .GetAll()
                    .Include(x => x.Atendimento)
                    .AsNoTracking().Select(x => new
                    {
                        x.Id,
                        x.EmpresaId,
                        x.ConvenioId,
                        x.PlanoId,
                        IsAmbulatorioEmergencia = x.Atendimento != null ? x.Atendimento.IsAmbulatorioEmergencia : false
                    }).FirstOrDefault(w => w.Id == input.ContaMedicaId);

                if (faturamentoItem == null || conta == null)
                {
                    if (faturamentoItem == null)
                    {
                        result.Errors.Add(ErroDto.Criar("", "Não foi possível calcular o valor por que não existe o item de faturamento"));
                    }

                    if (conta == null)
                    {
                        result.Errors.Add(ErroDto.Criar("", "Não foi possível calcular o valor por que não existe a conta"));
                    }
                }


                var valorUnitarioContaItemDto = new ValorUnitarioContaItemInputDto(
                    input, conta.IsAmbulatorioEmergencia, conta.ConvenioId, conta.EmpresaId,
                    conta.PlanoId, faturamentoItem.Id, faturamentoItem.GrupoId, faturamentoItem.SubGrupoId);

                resumoDetalhamentoDto.Taxas = (await RetornaTaxas(valorUnitarioContaItemDto)).ToList();

                var resultValorUnitarioContaItem = await ValorUnitarioContaItem(valorUnitarioContaItemDto, resumoDetalhamentoDto);

                result.Warnings.AddRange(resultValorUnitarioContaItem.Warnings);
                result.Errors.AddRange(resultValorUnitarioContaItem.Errors);
                if (result.Errors.Any())
                {
                    return result;
                }


                if (input.Honorarios != null)
                {
                    using (var profissionalDeSaudeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                    using (var valoresHonorariosRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoValoresHonorario, long>>())
                    {
                        var profissionaisDeSaudeIds = ResumoDetalhamentoHonorarios.RetornaProfissionaisDaSaude(input.Honorarios);

                        var profissionaisDeSaude = await profissionalDeSaudeRepository.Object
                            .GetAll()
                            .AsNoTracking()
                            .Include(x => x.SisPessoa)
                            .Include(x => x.Conselho)
                        .Where(x => profissionaisDeSaudeIds.Contains(x.Id)).ToListAsync();


                        var valoresHonorarios = await valoresHonorariosRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync();
                        if (input.Honorarios.HasMedico && !input.Honorarios.MedicoIsCredenciado)
                        {
                            input.Honorarios.DadosMedico = ResumoDetalhamentoHonorarios.MapearDadosProfissional(profissionaisDeSaude.FirstOrDefault(x => x.Id == input.Honorarios.MedicoId));
                            input.Honorarios.MedicoValor = (float)Math.Round((float)(resumoDetalhamentoDto.ValorTotal * valoresHonorarios.PercentualMedico), 2);
                        }

                        if (input.Honorarios.HasAuxiliar1 && !input.Honorarios.Auxiliar1IsCredenciado)
                        {
                            input.Honorarios.DadosAuxiliar1 = ResumoDetalhamentoHonorarios.MapearDadosProfissional(profissionaisDeSaude.FirstOrDefault(x => x.Id == input.Honorarios.Auxiliar1Id));
                            input.Honorarios.Auxiliar1Valor = (float)Math.Round((float)(resumoDetalhamentoDto.ValorTotal * valoresHonorarios.PercentualAuxiliar1), 2);
                        }

                        if (input.Honorarios.HasAuxiliar2 && !input.Honorarios.Auxiliar2IsCredenciado)
                        {
                            input.Honorarios.DadosAuxiliar2 = ResumoDetalhamentoHonorarios.MapearDadosProfissional(profissionaisDeSaude.FirstOrDefault(x => x.Id == input.Honorarios.Auxiliar2Id));
                            input.Honorarios.Auxiliar2Valor = (float)Math.Round((float)(resumoDetalhamentoDto.ValorTotal * valoresHonorarios.PercentualAuxiliar2), 2);
                        }

                        if (input.Honorarios.HasAuxiliar3 && !input.Honorarios.Auxiliar3IsCredenciado)
                        {
                            input.Honorarios.DadosAuxiliar3 = ResumoDetalhamentoHonorarios.MapearDadosProfissional(profissionaisDeSaude.FirstOrDefault(x => x.Id == input.Honorarios.Auxiliar3Id));
                            input.Honorarios.Auxiliar3Valor = (float)Math.Round((float)(resumoDetalhamentoDto.ValorTotal * valoresHonorarios.PercentualAuxiliar3), 2);
                        }

                        if (input.Honorarios.HasInstrumentador && !input.Honorarios.InstrumentadorIsCredenciado)
                        {
                            input.Honorarios.DadosInstrumentador = ResumoDetalhamentoHonorarios.MapearDadosProfissional(profissionaisDeSaude.FirstOrDefault(x => x.Id == input.Honorarios.InstrumentadorId));
                            input.Honorarios.InstrumentadorValor = (float)Math.Round((float)(resumoDetalhamentoDto.ValorTotal * valoresHonorarios.PercentualInstrumentador), 2);
                        }

                        if (input.Honorarios.HasAnestesista && !input.Honorarios.AnestesistaIsCredenciado)
                        {
                            input.Honorarios.DadosAnestesista = ResumoDetalhamentoHonorarios.MapearDadosProfissional(profissionaisDeSaude.FirstOrDefault(x => x.Id == input.Honorarios.AnestesistaId));
                            input.Honorarios.AnestesistaValor = (float)Math.Round((float)(resumoDetalhamentoDto.ValorTotal * valoresHonorarios.PercentualAnestesista), 2);
                        }

                        resumoDetalhamentoDto.Honorarios = input.Honorarios;
                    }
                }

                result.ReturnObject = new ValorCodigoTabela
                {
                    Valor = resultValorUnitarioContaItem.ReturnObject.Valor,
                    TabelaId = resultValorUnitarioContaItem.ReturnObject.TabelaId,
                    FaturamentoItemCobradoId = resultValorUnitarioContaItem.ReturnObject.FaturamentoItemCobradoId,
                    ResumoDetalhamento = resumoDetalhamentoDto,
                    ValorTotal = resumoDetalhamentoDto.ValorTotal
                };
            }

            return result;
        }

        private async Task<DefaultReturn<ValorUnitarioContaItemOutputDto>> ValorUnitarioContaItem(ValorUnitarioContaItemInputDto input, ResumoDetalhamento resumoDetalhamentoDto)
        {
            var result = new DefaultReturn<ValorUnitarioContaItemOutputDto>();
            var configAtual = await RetornaConfiguracaoAtiva(input);
            resumoDetalhamentoDto.ConfigAtual = ResumoDetalhamentoExtensions.Mapear(configAtual);
            resumoDetalhamentoDto.Tabela = ResumoDetalhamentoExtensions.Mapear(configAtual?.Tabela);
            var precoAtualPorTabela = await RetornaPrecoAtualPorTabela(configAtual, input, resumoDetalhamentoDto, result);



            result.ReturnObject = new ValorUnitarioContaItemOutputDto
            {
                Valor = precoAtualPorTabela.Valor,
                TabelaId = configAtual?.TabelaId,
                FaturamentoItemCobradoId = precoAtualPorTabela.FaturamentoItemCobradoId
            };

            return result;

        }


        private async Task<IEnumerable<ResumoDetalhamentoTaxa>> RetornaTaxas(ValorUnitarioContaItemInputDto input)
        {
            var itemCobranca = await ObterItemASerCobrado(input.ConvenioId, input.PlanoId, input.FaturamentoItemId);

            using (var conn = new SqlConnection(this.GetConnection()))
            {
                //TOOD: Não deveria ter um vinculo com o plano tbm?
                var query = $@"SELECT DISTINCT
                                {QueryHelper.CreateQueryFields<ResumoDetalhamentoTaxa>("FatTaxa").AllowAllFields().IgnoreField(x => x.Valor).GetFields()}
                                FROM FatTaxa
                                LEFT JOIN FatTaxaGrupo ON FatTaxaGrupo.FatTaxaId = FatTaxa.Id AND FatTaxaGrupo.IsDeleted = @IsDeleted
                                LEFT JOIN FatTaxaLocal ON FatTaxaLocal.FatTaxaId = FatTaxa.Id AND FatTaxaLocal.IsDeleted = @IsDeleted
                                LEFT JOIN FatTaxaTurno ON FatTaxaTurno.FatTaxaId = FatTaxa.Id AND FatTaxaTurno.IsDeleted = @IsDeleted
                                LEFT JOIN FatTaxaEmpresa ON FatTaxaEmpresa.FatTaxaId = FatTaxa.Id AND FatTaxaEmpresa.IsDeleted = @IsDeleted
                                WHERE 
                                FatTaxa.DataInicio <= @Data AND FatTaxa.IsDeleted = @IsDeleted
                                AND (FatTaxa.DataFim > @Data OR FatTaxa.DataFim IS NULL)                                
                                AND (FatTaxa.IsAmbulatorio = @IsAmbulatorioEmergencia  OR FatTaxa.IsInternacao = @IsInternacao)
                                AND (FatTaxa.IsTodosConvenio = @IsTrue OR FatTaxa.ConvenioId = @ConvenioId)
                                AND (FatTaxa.IsTodosGrupo = @IsTrue OR FatTaxaGrupo.SisGrupoId = @FaturamentoItemGrupoId)
                                AND (FatTaxa.IsTodosLocal = @IsTrue OR FatTaxaLocal.SisUnidadeOrganizacionalId = @UnidadeOrganizacionalId)
                                AND (FatTaxa.IsTodosTurno = @IsTrue OR FatTaxaTurno.SisTurnoId = @TurnoId)";
                try
                {
                    return await conn.QueryAsync<ResumoDetalhamentoTaxa>(query,
                        new
                        {
                            input.Data,
                            input.ConvenioId,
                            FaturamentoItemGrupoId = input.FaturamentoItemGrupoId ?? 0,
                            UnidadeOrganizacionalId = input.UnidadeOrganizacionalId ?? null,
                            TurnoId = input.TurnoId ?? null,
                            input.IsAmbulatorioEmergencia,
                            IsInternacao = !input.IsAmbulatorioEmergencia,
                            IsDeleted = false,
                            IsTrue = true
                        });
                }
                catch (Exception ex)
                {

                }
                return null;
            }
        }

        private static async Task<ValorUnitarioContaItemOutputDto> RetornaPrecoAtualPorTabela(FaturamentoConfigAtualDto configAtual, ValorUnitarioContaItemInputDto input, ResumoDetalhamento resumoDetalhamentoDto, DefaultReturn<ValorUnitarioContaItemOutputDto> valorUnitarioResult)
        {
            using (var faturamentoTabelaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoTabela, long>>())
            using (var faturamentoItemTabelaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItemTabela, long>>())

            {
                var result = new ValorUnitarioContaItemOutputDto();

                if (configAtual == null)
                {
                    valorUnitarioResult.Errors.Add(ErroDto.Criar("", "Não há preço cadastrado!"));
                    result.Valor = 0f;
                    return result;
                }
                result.FaturamentoItemCobradoId = configAtual.ItemId;

                resumoDetalhamentoDto.Qtde = (float)input.Qtd;
                resumoDetalhamentoDto.Percentual = input.Percentual;
                resumoDetalhamentoDto.FaturamentoItemCobradoId = configAtual.ItemId;

                if (configAtual == null)
                {
                    valorUnitarioResult.Errors.Add(ErroDto.Criar("", "Não há preço cadastrado!"));
                    result.Valor = 0f;
                    return result;
                }

                var tabela = await faturamentoTabelaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == configAtual.TabelaId);

                if (tabela != null)
                {
                    input.TabelaUilizada = tabela.Descricao;
                }

                var moeda = configAtual.Moeda;
                resumoDetalhamentoDto.Moeda = ResumoDetalhamentoExtensions.Mapear(moeda);

                var cotacao = await RetornaCotacaoMoeda(input, configAtual.SisMoedaId);

                if (cotacao == null && configAtual.SisMoedaId == SisMoeda.Real)
                {
                    cotacao = new FaturamentoCotacaoMoedaDto()
                    {
                        Codigo = "R$",
                        Valor = 1
                    };
                }
                else if (cotacao == null)
                {
                    valorUnitarioResult.Errors.Add(ErroDto.Criar("", "Não há cotação cadastrada para a moeda!"));
                }

                var cotacaoPorte = await RetornaCotacaoPorte(input.ConvenioId);
                var cotacaoPorteFilme = await RetornaCotacaoFilme(input.ConvenioId);

                resumoDetalhamentoDto.CotacaoPorte = ResumoDetalhamentoExtensions.Mapear(cotacaoPorte);
                resumoDetalhamentoDto.CotacaoPorteFilme = ResumoDetalhamentoExtensions.Mapear(cotacaoPorteFilme);


                if (!configAtual.COCH.HasValue)
                {
                    configAtual.COCH = 0;
                }

                resumoDetalhamentoDto.ValorCOCH = configAtual.COCH.Value;
                resumoDetalhamentoDto.ValorHMCH = configAtual.HMCH ?? 0;
                resumoDetalhamentoDto.MetragemFilme = input.MetragemFilme;

                var totalFilme = resumoDetalhamentoDto.ValorFilme = (cotacaoPorteFilme?.Valor ?? 0) * input.MetragemFilme;

                resumoDetalhamentoDto.TaxasFilme = CalculaTaxas(resumoDetalhamentoDto, CalculaTaxas_Filme, resumoDetalhamentoDto.ValorFilme);

                var totalPorte = resumoDetalhamentoDto.ValorPorte = (cotacaoPorte?.Valor ?? 0) * (long)(configAtual.Porte ?? 0);

                resumoDetalhamentoDto.TaxasPorte = CalculaTaxas(resumoDetalhamentoDto, CalculaTaxas_Porte, resumoDetalhamentoDto.ValorPorte);

                if (input.MetragemFilme != 0 && (cotacaoPorteFilme?.Valor ?? 0) == 0)
                {
                    valorUnitarioResult.Errors.Add(ErroDto.Criar("", "Não há cotação cadastrada para o filme!"));
                }

                if ((configAtual?.Porte ?? 0) != 0 && (cotacaoPorte?.Valor ?? 0) == 0)
                {
                    valorUnitarioResult.Errors.Add(ErroDto.Criar("", "Não há cotação cadastrada para o Porte!"));
                }

                var valorItem = resumoDetalhamentoDto.Valor = resumoDetalhamentoDto.Preco = configAtual.Preco * (cotacao?.Valor ?? 0);
                resumoDetalhamentoDto.TaxasValor = CalculaTaxas(resumoDetalhamentoDto, CalculaTaxas_PrecoItem, resumoDetalhamentoDto.Valor);

                result.Valor = 0;
                if ((cotacao?.Valor ?? 0) != 0)
                {
                    result.Valor = (float)Math.Round((decimal)
                        (valorItem + resumoDetalhamentoDto.TaxasValor
                        + totalFilme + resumoDetalhamentoDto.TaxasFilme
                        + totalPorte + resumoDetalhamentoDto.TaxasPorte), 2);
                }


                resumoDetalhamentoDto.ValorTotal = (float)Math.Round((float)(result.Valor * resumoDetalhamentoDto.Qtde) * (float)(resumoDetalhamentoDto.Percentual / 100), 2);


                return result;
            }
        }

        private static int CalculaTaxas_PrecoItem = 1;
        private static int CalculaTaxas_Porte = 2;
        private static int CalculaTaxas_Filme = 3;
        public static float CalculaTaxas(ResumoDetalhamento resumoDetalhamento, int tipo, float preco)
        {
            var valor = preco;
            var totalTaxas = 0f;
            IEnumerable<ResumoDetalhamentoTaxa> taxasEnumerable = new List<ResumoDetalhamentoTaxa>();

            if (tipo == 1)
            {
                taxasEnumerable = resumoDetalhamento.Taxas.Where(x => x.IsIncidePrecoItem);
            }
            else if (tipo == 2)
            {
                taxasEnumerable = resumoDetalhamento.Taxas.Where(x => x.IsIncidePrecoItem);
            }
            else if (tipo == 3)
            {
                taxasEnumerable = resumoDetalhamento.Taxas.Where(x => x.IsIncidePrecoItem);
            }

            if (!(taxasEnumerable?.Any() ?? false))
            {
                return totalTaxas;
            }

            var taxasNivel1 = 0f;
            foreach (var taxa in taxasEnumerable.Where(x => x.Nivel == 1 && x.Percentual != 0).OrderBy(x => x.DataInicio))
            {
                taxasNivel1 += taxa.Valor = valor * (float)(taxa.Percentual / 100);
            }

            valor += taxasNivel1;

            var taxasNivel2 = 0f;
            foreach (var taxa in taxasEnumerable.Where(x => x.Nivel == 2 && x.Percentual != 0).OrderBy(x => x.DataInicio))
            {
                taxasNivel2 += taxa.Valor = valor * (float)(taxa.Percentual / 100);
            }

            valor += taxasNivel2;

            var taxasNivel3 = 0f;
            foreach (var taxa in taxasEnumerable.Where(x => x.Nivel == 3 && x.Percentual != 0).OrderBy(x => x.DataInicio))
            {
                taxasNivel3 += taxa.Valor = valor * (float)(taxa.Percentual / 100);
            }

            valor += taxasNivel3;

            totalTaxas = taxasNivel1 + taxasNivel2 + taxasNivel3;

            return totalTaxas;
        }

        private static async Task<FaturamentoCotacaoMoedaDto> RetornaCotacaoFilme(long? convenioId)
        {
            using (var cotacaoMoedaRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SisMoedaCotacao, long>>())
            {
                return FaturamentoCotacaoMoedaDto.Mapear(await cotacaoMoedaRepository.Object.GetAll().AsNoTracking()
                    .Where(x => x.ConvenioId == convenioId && x.SisMoedaId == FaturamentoCotacaoMoedaDto.UCO)
                    .Where(_ => _.DataInicio <= DateTime.Now)
                    .OrderByDescending(_ => _.DataInicio)
                    .FirstOrDefaultAsync());
            }
        }

        private static async Task<FaturamentoCotacaoMoedaDto> RetornaCotacaoPorte(long? convenioId)
        {
            using (var cotacaoMoedaRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SisMoedaCotacao, long>>())
            {
                return FaturamentoCotacaoMoedaDto.Mapear(await cotacaoMoedaRepository.Object.GetAll().AsNoTracking()
                    .Where(x => x.ConvenioId == convenioId && x.SisMoedaId == FaturamentoCotacaoMoedaDto.UCO)
                    .Where(_ => _.DataInicio <= DateTime.Now)
                    .OrderByDescending(_ => _.DataInicio)
                    .FirstOrDefaultAsync());
            }
        }

        private static async Task<FaturamentoCotacaoMoedaDto> RetornaCotacaoMoeda(ValorUnitarioContaItemInputDto input, long? sisMoedaId)
        {
            using (var cotacaoMoedaRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SisMoedaCotacao, long>>())
            {
                var cotacoesPorConvenio = await cotacaoMoedaRepository.Object.GetAll().AsNoTracking()
                    .Where(x =>
                    (x.ConvenioId == input.ConvenioId || !input.ConvenioId.HasValue)
                    && x.SisMoedaId == sisMoedaId).ToListAsync();
                var cotacoesPorEmpresa = cotacoesPorConvenio.Where(x => x.EmpresaId == input.EmpresaId);

                // A cotacao padrao inicial eh por Convenio, se houver configuracao especifica, sera sobrescrita no fluxo abaixo
                var cotacao = cotacoesPorConvenio
                    .Where(_ => _.DataInicio <= DateTime.Now)
                    .OrderByDescending(_ => _.DataInicio)
                    .FirstOrDefault();

                var cotacoesPorPlano = cotacoesPorEmpresa
                    .Where(x => x.PlanoId != null)
                    .Where(c => c.PlanoId == input.PlanoId);

                if (cotacoesPorPlano.Any())
                {
                    // Por subGrupo especifico
                    var cotacaoPorSubGrupo = cotacoesPorPlano.Where(_ => _.SubGrupoId == input.FaturamentoItemSubGrupoId && _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now)
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
                            .Where(_ => _.DataInicio <= DateTime.Now && _.DataFinal >= DateTime.Now && _.GrupoId == input.FaturamentoItemGrupoId)
                            .OrderByDescending(_ => _.DataInicio)
                            .FirstOrDefault();

                        if (cotacaoPorGrupo != null)
                        {
                            cotacao = cotacaoPorGrupo;
                        }
                    }
                }
                else
                {
                    // Por subGrupo
                    var cotacaoPorSubGrupo = cotacoesPorConvenio
                        .Where(_ => _.DataInicio <= DateTime.Now /*&& _.DataFinal >= DateTime.Now*/)
                        .OrderByDescending(_ => _.DataInicio)
                        .FirstOrDefault(_ => _.SubGrupoId == input.FaturamentoItemSubGrupoId);

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
                            .FirstOrDefault(_ => _.GrupoId == input.FaturamentoItemGrupoId);

                        if (cotacaoPorGrupo != null)
                        {
                            cotacao = cotacaoPorGrupo;
                        }
                    }

                }

                return FaturamentoCotacaoMoedaDto.Mapear(cotacao);

            }
        }


        private static async Task<long?> ObterItemASerCobrado(long? convenioId, long? planoId, long? faturamentoItemId)
        {
            using (var itemConfigRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItemConfig, long>>())
            {
                var config = await itemConfigRepository.Object
                    .GetAll().AsNoTracking()
                    .FirstOrDefaultAsync(
                        w => (w.ConvenioId == convenioId || w.ConvenioId == null)
                             && (w.PlanoId == planoId || w.PlanoId == null)
                             && w.ItemId == faturamentoItemId);
                return config != null ? config.ItemCobrarId : faturamentoItemId;
            }
        }

        private async Task<IEnumerable<FaturamentoConfigConvenioDto>> RetornaConfiguracoes(ValorUnitarioContaItemInputDto input)
        {
            using (var configConvenioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConfigConvenio, long>>())
            {
                var query = $@"
                    SELECT
                        {QueryHelper.CreateQueryFields<FaturamentoConfigConvenio>("FatConfigConvenio").AllowAllFields().GetFields()},
                        {QueryHelper.CreateQueryFields<Convenio>("SisConvenio").AllowAllFields().GetFields()},
                        {QueryHelper.CreateQueryFields<FaturamentoGrupo>("FatGrupo").AllowAllFields().GetFields()},
                        {QueryHelper.CreateQueryFields<FaturamentoSubGrupo>("FatSubGrupo").AllowAllFields().GetFields()},
                        {QueryHelper.CreateQueryFields<FaturamentoTabela>("FatTabela").AllowAllFields().GetFields()},
                        {QueryHelper.CreateQueryFields<FaturamentoItem>("FatItem").AllowAllFields().GetFields()}
                    FROM FatConfigConvenio
                        LEFT JOIN SisConvenio ON FatConfigConvenio.ConvenioId = SisConvenio.Id AND SisConvenio.IsDeleted = @IsDeleted
                        LEFT JOIN FatGrupo ON FatConfigConvenio.FatGrupoId = FatGrupo.Id AND FatGrupo.IsDeleted = @IsDeleted
                        LEFT JOIN FatSubGrupo ON FatConfigConvenio.FatSubGrupoId = FatSubGrupo.Id AND FatSubGrupo.IsDeleted = @IsDeleted
                        LEFT JOIN FatTabela ON FatConfigConvenio.FatTabelaId = FatTabela.Id AND FatTabela.IsDeleted = @IsDeleted
                        LEFT JOIN FatItem ON FatConfigConvenio.FatItemId = FatItem.Id AND FatItem.IsDeleted = @IsDeleted
                    WHERE FatConfigConvenio.ConvenioId = @ConvenioId AND FatConfigConvenio.IsDeleted = @IsDeleted 
                    AND FatConfigConvenio.DataIncio <= @Data AND ((FatConfigConvenio.DataFim > @Data) OR (FatConfigConvenio.DataFim IS NULL))
                ";
                using (var conn = new SqlConnection(this.GetConnection()))
                {
                    return await conn.QueryAsync<FaturamentoConfigConvenioDto, ConvenioDto, FaturamentoGrupoDto, FaturamentoSubGrupoDto, FaturamentoTabelaDto, FaturamentoItemDto, FaturamentoConfigConvenioDto>(
                        query, MapearRetornaConfiguracoes, new { IsDeleted = false, input.ConvenioId, input.Data });
                }
            }
        }

        private static FaturamentoConfigConvenioDto MapearRetornaConfiguracoes(
                        FaturamentoConfigConvenioDto faturamentoConfigConvenio,
                        ConvenioDto convenio,
                        FaturamentoGrupoDto faturamentoGrupo,
                        FaturamentoSubGrupoDto faturamentoSubGrupo,
                        FaturamentoTabelaDto faturamentoTabela,
                        FaturamentoItemDto faturamentoItem)
        {
            if (faturamentoConfigConvenio == null)
            {
                return null;
            }


            if (convenio != null)
            {
                faturamentoConfigConvenio.Convenio = convenio;
            }

            if (faturamentoGrupo != null)
            {
                faturamentoConfigConvenio.Grupo = faturamentoGrupo;
            }


            if (faturamentoSubGrupo != null)
            {
                faturamentoConfigConvenio.SubGrupo = faturamentoSubGrupo;
            }

            if (faturamentoTabela != null)
            {
                faturamentoConfigConvenio.Tabela = faturamentoTabela;
            }

            if (faturamentoItem != null)
            {
                faturamentoConfigConvenio.Item = faturamentoItem;
            }

            return faturamentoConfigConvenio;
        }

        private async Task<FaturamentoConfigAtualDto> RetornaConfiguracaoAtiva(ValorUnitarioContaItemInputDto input)
        {
            var itemCobranca = await ObterItemASerCobrado(input.ConvenioId, input.PlanoId, input.FaturamentoItemId);
            var itemId = input.FaturamentoItemId;
            var grupoId = input.FaturamentoItemGrupoId;
            var subGrupoId = input.FaturamentoItemSubGrupoId;

            var query = @"
                SELECT 
                    ConfigAtual.*,
                    FatTabela.*, 
                    SisMoeda.*, 
                    SisEmpresa.*, 
                    SisConvenio.*,
                    SisPlano.*, 
                    FatItem.*, 
                    FatGrupo.*,
                    FatSubGrupo.* 
                FROM
                    [FatRetornaConfigAtual](@empresaId,@convenioId,@planoId,@itemId,@grupoId,@subGrupoId,@data) AS ConfigAtual
                    LEFT JOIN  FatTabela (NOLOCK) ON FatTabela.Id = ConfigAtual.FatTabelaId AND FatTabela.IsDeleted = @isDeleted
                    LEFT JOIN  SisMoeda (NOLOCK) ON SisMoeda.Id = ConfigAtual.SisMoedaId AND SisMoeda.IsDeleted = @isDeleted
                    LEFT JOIN  SisConvenio (NOLOCK) ON SisConvenio.Id = ConfigAtual.ConvenioId AND SisConvenio.IsDeleted = @isDeleted
                    LEFT JOIN  SisPlano (NOLOCK) ON SisPlano.Id = ConfigAtual.PlanoId AND SisPlano.IsDeleted = @isDeleted
                    LEFT JOIN  SisEmpresa (NOLOCK) ON SisEmpresa.Id = ConfigAtual.EmpresaId AND SisEmpresa.IsDeleted = @isDeleted
                    LEFT JOIN  FatGrupo (NOLOCK) ON FatGrupo.Id = ConfigAtual.GrupoId AND FatGrupo.IsDeleted = @isDeleted
                    LEFT JOIN  FatSubGrupo (NOLOCK) ON FatSubGrupo.Id = ConfigAtual.SubGrupoId AND FatSubGrupo.IsDeleted = @isDeleted
                    LEFT JOIN  FatItem (NOLOCK) ON FatItem.Id = ConfigAtual.ItemId AND FatItem.IsDeleted = @isDeleted
            ";

            var queryTypes = new Type[] {
                typeof(FaturamentoConfigAtualDto),
                typeof(FaturamentoTabelaDto),
                typeof(SisMoedaDto),
                typeof(EmpresaDto),
                typeof(ConvenioDto),
                typeof(PlanoDto),
                typeof(FaturamentoItemDto),
                typeof(FaturamentoGrupoDto),
                typeof(FaturamentoSubGrupoDto),
            };

            using (var FaturamentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
            {
                if (itemCobranca.HasValue)
                {
                    var itemACobrar = await FaturamentoItemRepository.Object.FirstOrDefaultAsync(x => itemCobranca.Value != 0 && x.Id == itemCobranca.Value);
                    if (itemACobrar != null)
                    {
                        itemId = itemACobrar.Id;
                        grupoId = itemACobrar.GrupoId;
                        subGrupoId = itemACobrar.SubGrupoId;
                    }
                }

                using (var conn = new SqlConnection(this.GetConnection()))
                {
                    return (await conn.
                        QueryAsync(
                        query,
                        queryTypes,
                        MapearRetornaConfiguracaoAtiva,
                    new
                    {
                        input.EmpresaId,
                        input.ConvenioId,
                        input.PlanoId,
                        input.Data,
                        itemId,
                        grupoId,
                        subGrupoId,
                        isDeleted = false
                    })).FirstOrDefault();
                }
            }
        }


        private static FaturamentoConfigAtualDto MapearRetornaConfiguracaoAtiva(object[] entities)
        {
            var faturamentoConfigAtual = entities[0] as FaturamentoConfigAtualDto;
            if (faturamentoConfigAtual == null)
            {
                return null;
            }

            var faturamentoTabela = entities[1] as FaturamentoTabelaDto;
            if (faturamentoTabela != null)
            {
                faturamentoConfigAtual.Tabela = faturamentoTabela;
            }

            var moeda = entities[2] as SisMoedaDto;
            if (moeda != null)
            {
                faturamentoConfigAtual.Moeda = moeda;
            }

            var empresa = entities[3] as EmpresaDto;
            if (empresa != null)
            {
                faturamentoConfigAtual.Empresa = empresa;
            }

            var convenio = entities[4] as ConvenioDto;
            if (convenio != null)
            {
                faturamentoConfigAtual.Convenio = convenio;
            }

            var plano = entities[5] as PlanoDto;
            if (plano != null)
            {
                faturamentoConfigAtual.Plano = plano;
            }

            var item = entities[6] as FaturamentoItemDto;
            if (item != null)
            {
                faturamentoConfigAtual.Convenio = convenio;
            }
            var faturamentoGrupo = entities[7] as FaturamentoGrupoDto;
            if (faturamentoGrupo != null)
            {
                faturamentoConfigAtual.Grupo = faturamentoGrupo;
            }

            var faturamentoSubGrupo = entities[8] as FaturamentoSubGrupoDto;
            if (faturamentoSubGrupo != null)
            {
                faturamentoConfigAtual.SubGrupo = faturamentoSubGrupo;
            }

            return faturamentoConfigAtual;

        }

        public class ValorUnitarioContaItemInputDto : ValorTotalItemFaturamentoDto
        {
            public ValorUnitarioContaItemInputDto(
                ValorTotalItemFaturamentoDto input,
                bool isAmbulatorioEmergencia,
                long? convenioId, long? empresaId, long? planoId,
                long faturamentoItemId, long? faturamentoItemGrupoId, long? faturamentoItemSubGrupoId)
            {
                Data = input.Data;
                Qtd = input.Qtd;

                ContaMedicaId = input.ContaMedicaId;

                UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                TurnoId = input.TurnoId;
                CentroCustoId = input.CentroCustoId;
                TerceirizadoId = input.TerceirizadoId;
                TipoLeitoId = input.TipoLeitoId;

                Percentual = input.Percentual;
                Honorarios = input.Honorarios;

                ConvenioId = convenioId;
                EmpresaId = empresaId;
                PlanoId = planoId;
                FaturamentoItemId = faturamentoItemId;
                FaturamentoItemGrupoId = faturamentoItemGrupoId;
                FaturamentoItemSubGrupoId = faturamentoItemSubGrupoId;

                IsAmbulatorioEmergencia = IsAmbulatorioEmergencia;
            }
            public bool IsAmbulatorioEmergencia { get; set; }
            public long? ConvenioId { get; set; }
            public long? EmpresaId { get; set; }
            public long? PlanoId { get; set; }

            public long? FaturamentoItemGrupoId { get; set; }
            public long? FaturamentoItemSubGrupoId { get; set; }

            public float MetragemFilme { get; set; }
            public string TabelaUilizada { get; set; }
        }

        public class ValorUnitarioContaItemOutputDto
        {
            public float Valor { get; set; }

            public long? TabelaId { get; set; }

            public List<ValorUnitarioContaItemTaxas> Taxas { get; set; } = new List<ValorUnitarioContaItemTaxas>();

            public long? FaturamentoItemCobradoId { get; set; }
        }

        public class ValorUnitarioContaItemTaxas : CamposPadraoCRUDDto
        {
        }
    }

    public static class ConfiguracaoConvenioQueries
    {
        public static IEnumerable<FaturamentoConfigConvenioDto> PossuiEmpresa(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? empresaId)
            => configsConvenio.Where(x => x.EmpresaId == (empresaId ?? 0));

        public static IEnumerable<FaturamentoConfigConvenioDto> NaoPossuiEmpresa(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? empresaId)
            => configsConvenio.Where(x => x.EmpresaId == null || x.EmpresaId == 0 || x.EmpresaId == empresaId);

        public static IEnumerable<FaturamentoConfigConvenioDto> PossuiPlano(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? planoId)
           => configsConvenio.Where(x => x.PlanoId == (planoId ?? 0));

        public static IEnumerable<FaturamentoConfigConvenioDto> NaoPossuiPlano(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? planoId)
           => configsConvenio.Where(x => x.PlanoId == null || x.PlanoId == 0 || x.PlanoId == planoId);

        public static IEnumerable<FaturamentoConfigConvenioDto> PossuiConvenio(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? convenioId)
           => configsConvenio.Where(x => x.ConvenioId == (convenioId ?? 0));

        public static IEnumerable<FaturamentoConfigConvenioDto> NaoPossuiConvenio(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? convenioId)
           => configsConvenio.Where(x => x.ConvenioId == null || x.ConvenioId == 0 || x.ConvenioId == convenioId);

        public static IEnumerable<FaturamentoConfigConvenioDto> PossuiGrupo(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? grupoId)
           => configsConvenio.Where(x => x.GrupoId == (grupoId ?? 0));

        public static IEnumerable<FaturamentoConfigConvenioDto> NaoPossuiGrupo(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? grupoId)
          => configsConvenio.Where(x => x.GrupoId == null || x.GrupoId == 0 || x.GrupoId == grupoId);

        public static IEnumerable<FaturamentoConfigConvenioDto> PossuiSubGrupo(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? SubGrupoId)
           => configsConvenio.Where(x => x.SubGrupoId == (SubGrupoId ?? 0));

        public static IEnumerable<FaturamentoConfigConvenioDto> NaoPossuiSubGrupo(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? SubGrupoId)
           => configsConvenio.Where(x => x.SubGrupoId == null || x.SubGrupoId == 0 || x.SubGrupoId == SubGrupoId);

        public static IEnumerable<FaturamentoConfigConvenioDto> PossuiItem(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? itemId)
           => configsConvenio.Where(x => x.ItemId == (itemId ?? 0));

        public static IEnumerable<FaturamentoConfigConvenioDto> NaoPossuiItem(this IEnumerable<FaturamentoConfigConvenioDto> configsConvenio, long? itemId)
           => configsConvenio.Where(x => x.ItemId == null || x.ItemId == 0 || x.ItemId == itemId);
    }
}