using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    public class PrescricaoItemAppService : SWMANAGERAppServiceBase, IPrescricaoItemAppService
    {
        [UnitOfWork]
        public async Task<PrescricaoItemDto> CriarOuEditar(PrescricaoItemDto input)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var formulaEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueAppService>())
                using (var formulaFaturamentoAppService = IocManager.Instance.ResolveAsDisposable<IFormulaFaturamentoAppService>())
                using (var formulaEstoqueKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormulaEstoqueKit, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var prescricaoItem = input.MapTo<PrescricaoItem>();

                    var formulasEstoque = new List<FormulaEstoqueDto>();
                    if (!input.FormulaEstoqueList.IsNullOrWhiteSpace())
                    {
                        formulasEstoque = JsonConvert.DeserializeObject<List<FormulaEstoqueDto>>(input.FormulaEstoqueList, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }
                    var formulasFaturamento = new List<FormulaFaturamentoDto>();
                    if (!input.FormulaFaturamentoList.IsNullOrWhiteSpace())
                    {
                        formulasFaturamento = JsonConvert.DeserializeObject<List<FormulaFaturamentoDto>>(input.FormulaFaturamentoList, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }

                    var formulasExameImagem = new List<FormulaFaturamentoDto>();
                    if (!input.FormulaExameImagemList.IsNullOrWhiteSpace())
                    {
                        formulasExameImagem = JsonConvert.DeserializeObject<List<FormulaFaturamentoDto>>(input.FormulaExameImagemList, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }

                    var formulasExameLaboratorial = new List<FormulaFaturamentoDto>();
                    if (!input.FormulaExameLaboratorialList.IsNullOrWhiteSpace())
                    {
                        formulasExameLaboratorial = JsonConvert.DeserializeObject<List<FormulaFaturamentoDto>>(input.FormulaExameLaboratorialList, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    }

                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await prescricaoItemRepositorio.Object.InsertAndGetIdAsync(prescricaoItem).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    else
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await prescricaoItemRepositorio.Object.UpdateAsync(prescricaoItem).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }

                    foreach (var formulaEstoque in formulasEstoque)
                    {
                        formulaEstoque.PrescricaoItemId = input.Id;
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            if (formulaEstoque.IsDeleted)
                            {
                                await formulaEstoqueAppService.Object.Excluir(formulaEstoque).ConfigureAwait(false);
                            }
                            else
                            {
                                await formulaEstoqueAppService.Object.CriarOuEditar(formulaEstoque).ConfigureAwait(false);
                            }
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    foreach (var formulaFaturamento in formulasFaturamento)
                    {
                        formulaFaturamento.PrescricaoItemId = input.Id;
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            if (formulaFaturamento.IsDeleted)
                            {
                                await formulaFaturamentoAppService.Object.Excluir(formulaFaturamento).ConfigureAwait(false);
                            }
                            else
                            {
                                await formulaFaturamentoAppService.Object.CriarOuEditar(formulaFaturamento).ConfigureAwait(false);
                            }
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    foreach (var formulaExameImagem in formulasExameImagem)
                    {
                        formulaExameImagem.PrescricaoItemId = input.Id;
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            if (formulaExameImagem.IsDeleted)
                            {
                                await formulaFaturamentoAppService.Object.Excluir(formulaExameImagem).ConfigureAwait(false);
                            }
                            else
                            {
                                await formulaFaturamentoAppService.Object.CriarOuEditar(formulaExameImagem).ConfigureAwait(false);
                            }
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    foreach (var formulaExameLaboratorial in formulasExameLaboratorial)
                    {
                        formulaExameLaboratorial.PrescricaoItemId = input.Id;
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            if (formulaExameLaboratorial.IsDeleted)
                            {
                                await formulaFaturamentoAppService.Object.Excluir(formulaExameLaboratorial).ConfigureAwait(false);
                            }
                            else
                            {
                                await formulaFaturamentoAppService.Object.CriarOuEditar(formulaExameLaboratorial).ConfigureAwait(false);
                            }
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }



                    if (!string.IsNullOrEmpty(input.FormulaEstoqueKitJson))
                    {
                        var formulaEstoqueKitsInput = JsonConvert.DeserializeObject<List<FormulaEstoqueKitIndex>>(input.FormulaEstoqueKitJson);



                        var formulaEstoqueKits = formulaEstoqueKitRepository.Object.GetAll()
                                                                             .Where(w => w.PrescricaoItemId == input.Id)
                                                                             .ToList();

                        var kitsExcluidos = formulaEstoqueKits.Where(w => !formulaEstoqueKitsInput.Any(a => a.Id == w.Id));

                        foreach (var item in kitsExcluidos)
                        {
                            formulaEstoqueKitRepository.Object.Delete(item);
                        }


                        foreach (var item in formulaEstoqueKitsInput.Where(w => w.Id == 0))
                        {
                            var formulaEstoqueKit = new FormulaEstoqueKit();

                            formulaEstoqueKit.EstoqueKitId = item.KitId;
                            formulaEstoqueKit.PrescricaoItemId = input.Id;

                            formulaEstoqueKitRepository.Object.Insert(formulaEstoqueKit);
                        }
                    }
                }

                return input;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task<PrescricaoItemDto> CriarOuEditarSubItem(CriarSubItemPrescricaoViewModel input)
        {
            using (var prescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
            {
                var prescricaoItem = await prescricaoItemRepository.Object.GetAll().AsNoTracking().Where(x=> x.Id == input.PrescricaoItemId).FirstOrDefaultAsync().ConfigureAwait(false);
                if (input.Id == 0)
                {
                    prescricaoItem.Id = 0;
                    prescricaoItem.Codigo = input.Codigo;
                    prescricaoItem.Descricao = input.Descricao;
                    prescricaoItem.PrescricaoItemId = input.PrescricaoItemId;
                    prescricaoItem.Id = await prescricaoItemRepository.Object.InsertAndGetIdAsync(prescricaoItem).ConfigureAwait(false);
                    
                    return PrescricaoItemDto.Mapear(prescricaoItem);
                }
                
                var subPrescricaoItem = await prescricaoItemRepository.Object.GetAsync(input.Id).ConfigureAwait(false);

                subPrescricaoItem.Codigo = input.Codigo;
                subPrescricaoItem.Descricao = input.Descricao;
                    
                subPrescricaoItem.Id = await prescricaoItemRepository.Object.InsertOrUpdateAndGetIdAsync(subPrescricaoItem).ConfigureAwait(false);
                    
                return PrescricaoItemDto.Mapear(subPrescricaoItem);
            }
        }
        
        [UnitOfWork]
        public async Task InserirPorProduto(CadastroAgilizadoDto input)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var produtoId in input.Ids)
                    {
                        var produto = await produtoRepositorio.Object.GetAsync(produtoId).ConfigureAwait(false);
                        var prescricaoItem = new PrescricaoItem();
                        prescricaoItem.Codigo = produto.Codigo;
                        prescricaoItem.Descricao = produto.Descricao;
                        prescricaoItem.CreationTime = DateTime.Now;
                        prescricaoItem.CreatorUserId = AbpSession.UserId;
                        prescricaoItem.IsDeleted = false;
                        prescricaoItem.IsSistema = false;
                        prescricaoItem.ProdutoId = produtoId;
                        prescricaoItem.EstoqueId = input.EstoqueId;
                        prescricaoItem.FaturamentoItemId = produto.FaturamentoItemId;
                        prescricaoItem.DivisaoId = input.DivisaoId;
                        await prescricaoItemRepositorio.Object.InsertAsync(prescricaoItem).ConfigureAwait(false);
                    }
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task InserirPorFatItem(CadastroAgilizadoDto input)
        {
            try
            {
                using (var fatItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var id in input.Ids)
                    {
                        var item = await fatItemRepositorio.Object.GetAsync(id).ConfigureAwait(false);
                        var prescricaoItem = new PrescricaoItem
                        {
                            Codigo = item.Codigo,
                            Descricao = item.Descricao,
                            CreationTime = DateTime.Now,
                            CreatorUserId = this.AbpSession.UserId,
                            IsDeleted = false,
                            IsSistema = false,
                            FaturamentoItemId = id,
                            DivisaoId = input.DivisaoId
                        };
                        await prescricaoItemRepositorio.Object.InsertAsync(prescricaoItem).ConfigureAwait(false);
                    }
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await prescricaoItemRepositorio.Object.DeleteAsync(id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task ExcluirPorProduto(List<string> ids)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var item in ids)
                    {
                        long id;
                        var isId = long.TryParse(item, out id);
                        if (isId)
                        {
                            var prescricao = await prescricaoItemRepositorio.Object.GetAll()
                                                 .Where(m => m.ProdutoId == id)
                                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                            if (prescricao != null)
                            {
                                await prescricaoItemRepositorio.Object.DeleteAsync(prescricao.Id).ConfigureAwait(false);
                            }
                        }
                    }
                    unitOfWork.Complete();
                    unitOfWork.Dispose();

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task ExcluirPorFatItem(List<string> ids)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var item in ids)
                    {
                        long id;
                        var isId = long.TryParse(item, out id);
                        if (isId)
                        {
                            var prescricao = await prescricaoItemRepositorio.Object.GetAll()
                                                 .Where(m => m.FaturamentoItemId == id)
                                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                            if (prescricao != null)
                            {
                                await prescricaoItemRepositorio.Object.DeleteAsync(prescricao.Id).ConfigureAwait(false);
                            }
                        }
                    }
                    unitOfWork.Complete();
                    unitOfWork.Dispose();

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoItemDto>> Listar(ListarInput input)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var query = prescricaoItemRepositorio.Object.GetAll()
                        .Where(x=> !x.PrescricaoItemId.HasValue)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    var contarPrescricaoItem = await query.CountAsync().ConfigureAwait(false);

                    var prescricaoItem = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var PrescricaoItemDtos = prescricaoItem.MapTo<List<PrescricaoItemDto>>();

                    return new PagedResultDto<PrescricaoItemDto>(contarPrescricaoItem, PrescricaoItemDtos);
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<SubPrescricaoItemDto>> ListarSelecionarPorPrescricaoItemId(ListarInput input)
        {
            using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using(var configuracaoPrescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IConfiguracaoPrescricaoItemAppService>())
                {
                    long prescricaoItemId;
                    if (input.Id.IsNullOrEmpty() || !long.TryParse(input.Id, out prescricaoItemId))
                    {
                        return new PagedResultDto<SubPrescricaoItemDto>();
                    }

                    var query = prescricaoItemRepositorio.Object.GetAll()
                        .Where(x=> (x.PrescricaoItemId.HasValue && x.PrescricaoItemId.Value == prescricaoItemId) || x.Id == prescricaoItemId )
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    var contarPrescricaoItem = await query.CountAsync().ConfigureAwait(false);

                    var prescricaoItem = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    prescricaoItem = prescricaoItem.OrderBy(x => x.Descricao).ThenBy(x => x.Id == prescricaoItemId).ToList();

                    var PrescricaoItemDtos = SubPrescricaoItemDto.Mapear(prescricaoItem.ToList()).ToList();

                    var configuracaoPrescricaoItemPorItem = await configuracaoPrescricaoItemAppService
                        .Object.ObterPorPrescricaoItemAgrupado(PrescricaoItemDtos.Select(x=> x.Id)).ConfigureAwait(false);
                    foreach (var item in PrescricaoItemDtos)
                    {
                        if (!configuracaoPrescricaoItemPorItem.ContainsKey(item.Id))
                        {
                            continue;
                        }

                        var configuracaoPrescricaoItems = configuracaoPrescricaoItemPorItem[item.Id];

                        foreach (var configuracaoPrescricaoItem in configuracaoPrescricaoItems)
                        {
                            switch (configuracaoPrescricaoItem.ConfiguracaoPrescricaoItemCampoId)
                            {
                                case ConfiguracaoPrescricaoItemCampoDto.QtdPorHorario:
                                {
                                    item.Quantidade = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Unidade:
                                {
                                    item.Unidade = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }

                                case ConfiguracaoPrescricaoItemCampoDto.ViaDeAplicacao:
                                {
                                    item.ViaDeAplicacao = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.FormaDeAplicacao:
                                {
                                    item.FormaDeAplicacao = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Diluente:
                                {
                                    item.Diluente = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Volume:
                                {
                                    item.Volume = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Frequencia:
                                {
                                    item.Frequencia = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }

                                case ConfiguracaoPrescricaoItemCampoDto.Observacao:
                                {
                                    item.Observacao = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                            }
                        }
                    }

                    var unidades = PrescricaoItemDtos.Select(x => x.Unidade).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var viaDeAplicacao = PrescricaoItemDtos.Select(x => x.ViaDeAplicacao).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var formaDeAplicacao = PrescricaoItemDtos.Select(x => x.FormaDeAplicacao).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var diluente = PrescricaoItemDtos.Select(x => x.Diluente).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var volume = PrescricaoItemDtos.Select(x => x.Volume).Where(x => !x.IsNullOrEmpty()).Distinct();
                    var frequencia = PrescricaoItemDtos.Select(x => x.Frequencia).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));

                    using (var unidadesRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                    using (var viaDeAplicacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                    using (var formaDeAplicacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                    using (var diluenteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                    using (var frequenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                    {
                        var unidadesEf = unidadesRepository.Object.GetAll().AsNoTracking().Where(x => unidades.Contains(x.Id)).ToList();
                        var viaDeAplicacaoEf = viaDeAplicacaoRepository.Object.GetAll().AsNoTracking().Where(x => viaDeAplicacao.Contains(x.Id)).ToList();
                        var formaDeAplicacaoEf = formaDeAplicacaoRepository.Object.GetAll().AsNoTracking().Where(x => formaDeAplicacao.Contains(x.Id)).ToList();
                        var diluenteEf = diluenteRepository.Object.GetAll().AsNoTracking().Where(x => diluente.Contains(x.Id)).ToList();
                        var frequenciaEf = frequenciaRepository.Object.GetAll().AsNoTracking().Where(x => frequencia.Contains(x.Id)).ToList();

                        foreach (var item in PrescricaoItemDtos)
                        {
                            item.Unidade = unidadesEf.FirstOrDefault(x => item.Unidade == x.Id.ToString())?.Descricao;
                            item.ViaDeAplicacao = viaDeAplicacaoEf.FirstOrDefault(x => item.ViaDeAplicacao == x.Id.ToString())?.Descricao;
                            item.FormaDeAplicacao = formaDeAplicacaoEf.FirstOrDefault(x => item.FormaDeAplicacao == x.Id.ToString())?.Descricao;
                            item.Diluente = diluenteEf.FirstOrDefault(x => item.Diluente == x.Id.ToString())?.Descricao;
                            item.Frequencia = frequenciaEf.FirstOrDefault(x => item.Frequencia == x.Id.ToString())?.Descricao;
                        }
                    }
                    return new PagedResultDto<SubPrescricaoItemDto>(contarPrescricaoItem, PrescricaoItemDtos);
                }
        }
        
        public async Task<PagedResultDto<SubPrescricaoItemDto>> ListarPorPrescricaoItemId(ListarInput input)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using(var configuracaoPrescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IConfiguracaoPrescricaoItemAppService>())
                {
                    long prescricaoItemId;
                    if (input.Id.IsNullOrEmpty() || !long.TryParse(input.Id, out prescricaoItemId))
                    {
                        return new PagedResultDto<SubPrescricaoItemDto>();
                    }

                    var query = prescricaoItemRepositorio.Object.GetAll()
                        .Where(x=> x.PrescricaoItemId.HasValue && x.PrescricaoItemId.Value == prescricaoItemId )
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    var contarPrescricaoItem = await query.CountAsync().ConfigureAwait(false);

                    var prescricaoItem = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var PrescricaoItemDtos = SubPrescricaoItemDto.Mapear(prescricaoItem.ToList()).ToList();

                    var configuracaoPrescricaoItemPorItem = await configuracaoPrescricaoItemAppService
                        .Object.ObterPorPrescricaoItemAgrupado(PrescricaoItemDtos.Select(x=> x.Id)).ConfigureAwait(false);
                    foreach (var item in PrescricaoItemDtos)
                    {
                        if (!configuracaoPrescricaoItemPorItem.ContainsKey(item.Id))
                        {
                            continue;
                        }

                        var configuracaoPrescricaoItems = configuracaoPrescricaoItemPorItem[item.Id];

                        foreach (var configuracaoPrescricaoItem in configuracaoPrescricaoItems)
                        {
                            switch (configuracaoPrescricaoItem.ConfiguracaoPrescricaoItemCampoId)
                            {
                                case ConfiguracaoPrescricaoItemCampoDto.QtdPorHorario:
                                {
                                    item.Quantidade = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Unidade:
                                {
                                    item.Unidade = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }

                                case ConfiguracaoPrescricaoItemCampoDto.ViaDeAplicacao:
                                {
                                    item.ViaDeAplicacao = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.FormaDeAplicacao:
                                {
                                    item.FormaDeAplicacao = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Diluente:
                                {
                                    item.Diluente = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Volume:
                                {
                                    item.Volume = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                case ConfiguracaoPrescricaoItemCampoDto.Frequencia:
                                {
                                    item.Frequencia = configuracaoPrescricaoItem.DefaultValue;
                                    break;
                                }
                                
                                default:
                                {
                                    break;
                                }
                            }
                        }
                    }

                    var unidades = PrescricaoItemDtos.Select(x => x.Unidade).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var viaDeAplicacao = PrescricaoItemDtos.Select(x => x.ViaDeAplicacao).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var formaDeAplicacao = PrescricaoItemDtos.Select(x => x.FormaDeAplicacao).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var diluente = PrescricaoItemDtos.Select(x => x.Diluente).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));
                    var volume = PrescricaoItemDtos.Select(x => x.Volume).Where(x => !x.IsNullOrEmpty()).Distinct();
                    var frequencia = PrescricaoItemDtos.Select(x => x.Frequencia).Where(x => !x.IsNullOrEmpty()).Distinct().Select(x=> double.Parse(x));

                    using (var unidadesRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                    using (var viaDeAplicacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<VelocidadeInfusao, long>>())
                    using (var formaDeAplicacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                    using (var diluenteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                    using (var frequenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                    {
                        var unidadesEf = unidadesRepository.Object.GetAll().AsNoTracking().Where(x => unidades.Contains(x.Id)).ToList();
                        var viaDeAplicacaoEf = viaDeAplicacaoRepository.Object.GetAll().AsNoTracking().Where(x => viaDeAplicacao.Contains(x.Id)).ToList();
                        var formaDeAplicacaoEf = formaDeAplicacaoRepository.Object.GetAll().AsNoTracking().Where(x => formaDeAplicacao.Contains(x.Id)).ToList();
                        var diluenteEf = diluenteRepository.Object.GetAll().AsNoTracking().Where(x => diluente.Contains(x.Id)).ToList();
                        var frequenciaEf = frequenciaRepository.Object.GetAll().AsNoTracking().Where(x => frequencia.Contains(x.Id)).ToList();

                        foreach (var item in PrescricaoItemDtos)
                        {
                            item.Unidade = unidadesEf.FirstOrDefault(x => item.Unidade == x.Id.ToString())?.Descricao;
                            item.ViaDeAplicacao = viaDeAplicacaoEf.FirstOrDefault(x => item.ViaDeAplicacao == x.Id.ToString())?.Descricao;
                            item.FormaDeAplicacao = formaDeAplicacaoEf.FirstOrDefault(x => item.FormaDeAplicacao == x.Id.ToString())?.Descricao;
                            item.Diluente = diluenteEf.FirstOrDefault(x => item.Diluente == x.Id.ToString())?.Descricao;
                            item.Frequencia = frequenciaEf.FirstOrDefault(x => item.Frequencia == x.Id.ToString())?.Descricao;
                        }
                    }
                    return new PagedResultDto<SubPrescricaoItemDto>(contarPrescricaoItem, PrescricaoItemDtos);
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PrescricaoItemDto> Obter(long id)
        {
            try
            {
                using(var configuracaoPrescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IConfiguracaoPrescricaoItemAppService>() )
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var result = PrescricaoItemDto.Mapear(await prescricaoItemRepositorio.Object
                                 .GetAll()
                                 .AsNoTracking()
                                 .Include(m => m.TipoPrescricao)
                                 .Include(m => m.Divisao)
                                 .Include(m => m.FormaAplicacao)
                                 .Include(m => m.Frequencia)
                                 .Include(m => m.TipoControle)
                                 .Include(m => m.Unidade)
                                 .Include(m => m.UnidadeRequisicao)
                                 .Include(m => m.VelocidadeInfusao)
                                 .Include(m => m.Produto)
                                 .Include(m => m.FaturamentoItem)
                                 .Include(m => m.Estoque)
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false));
                    if (result != null)
                    {
                        result.ConfiguracaoPrescricaoItems = await configuracaoPrescricaoItemAppService.Object.ObterPorPrescricaoItem(result.Id);

                        result.HasParent = await prescricaoItemRepositorio.Object.GetAll().AnyAsync(x => x.PrescricaoItemId == result.Id);
                    }
                    return result;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public PrescricaoItemDto ObterSync(long id)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var result = prescricaoItemRepositorio.Object
                                 .GetAll()
                                 .AsNoTracking()
                                 .Include(m => m.TipoPrescricao)
                                 .Include(m => m.Divisao)
                                 .Include(m => m.FormaAplicacao)
                                 .Include(m => m.Frequencia)
                                 .Include(m => m.TipoControle)
                                 .Include(m => m.Unidade)
                                 .Include(m => m.UnidadeRequisicao)
                                 .Include(m => m.VelocidadeInfusao)
                                 .Include(m => m.Produto)
                                 .Include(m => m.FaturamentoItem)
                                 .Include(m => m.ConfiguracaoPrescricaoItems)
                                 .Include(m => m.Estoque)
                                 .Where(m => m.Id == id)
                                 .FirstOrDefault();
                    return PrescricaoItemDto.Mapear(result);
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IEnumerable<PrescricaoItemDto>> ObterDapper(long id)
        {
            try
            {

                var query = $@"
                                SELECT
                                    {QueryHelper.CreateQueryFields<PrescricaoItem>(tableAlias: "PrescricaoItem").GetFields()},
                                    {QueryHelper.CreateQueryFields<TipoPrescricao>(tableAlias: "TipoPrescricao").GetFields()},
                                    {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "Divisao").GetFields()},
                                    {QueryHelper.CreateQueryFields<FormaAplicacao>(tableAlias: "FormaAplicacao").GetFields()},
                                    {QueryHelper.CreateQueryFields<Frequencia>(tableAlias: "Frequencia").GetFields()},
                                    {QueryHelper.CreateQueryFields<TipoControle>(tableAlias: "TipoControle").GetFields()},
                                    {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "Unidade").GetFields()},
                                    {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "UnidadeRequisicao").GetFields()},
                                    {QueryHelper.CreateQueryFields<VelocidadeInfusao>(tableAlias: "VelocidadeInfusao").GetFields()},
                                    {QueryHelper.CreateQueryFields<Produto>(tableAlias: "Produto").GetFields()},
                                    {QueryHelper.CreateQueryFields<FaturamentoItem>(tableAlias: "FaturamentoItem").GetFields()},
                                    {QueryHelper.CreateQueryFields<Estoque>(tableAlias: "Estoque").GetFields()}
                                FROM AssPrescricaoItem AS PrescricaoItem 
                                    LEFT JOIN AssTipoPrescricao AS TipoPrescricao ON  PrescricaoItem.AssTipoPrescricaoId = TipoPrescricao.Id
                                    LEFT JOIN AssDivisao AS Divisao ON  PrescricaoItem.AssDivisaoId = Divisao.Id
                                    LEFT JOIN AssFormaAplicacao AS FormaAplicacao ON  PrescricaoItem.AssFormaAplicacaoId = FormaAplicacao.Id
                                    LEFT JOIN AssFrequencia AS Frequencia ON  PrescricaoItem.AssFrequenciaId = Frequencia.Id
                                    LEFT JOIN AssTipoControle AS TipoControle ON  PrescricaoItem.AssTipoControleId = TipoControle.Id
                                    LEFT JOIN Est_Unidade AS Unidade ON  PrescricaoItem.EstUnidadeId = Unidade.Id
                                    LEFT JOIN Est_Unidade AS UnidadeRequisicao ON  PrescricaoItem.EstUnidadeRequisicaoId = UnidadeRequisicao.Id
                                    LEFT JOIN AssVelocidadeInfusao AS VelocidadeInfusao ON  PrescricaoItem.AssVelocidadeInfusaoId = VelocidadeInfusao.Id
                                    LEFT JOIN Est_Produto AS Produto ON  PrescricaoItem.EstProdutoId = Produto.Id
                                    LEFT JOIN FatItem AS FaturamentoItem ON  PrescricaoItem.FatItemId = FaturamentoItem.Id
                                    LEFT JOIN Est_Estoque AS Estoque ON  PrescricaoItem.EstEstoqueId = Estoque.Id
                                WHERE  
                                    PrescricaoItem.AssPrescricaoMedicaId = @id
                                    AND PrescricaoItem.IsDeleted = 0";

                var types = new Type[] {
                    typeof(PrescricaoItemDto), // 0
                    typeof(TipoPrescricaoDto), // 1
                    typeof(DivisaoDto), // 2
                    typeof(FormaAplicacaoDto), // 3
                    typeof(FrequenciaDto), // 4
                    typeof(TipoControleDto), // 5
                    typeof(UnidadeDto), // 6
                    typeof(UnidadeDto), // 7
                    typeof(VelocidadeInfusaoDto), // 8
                    typeof(ProdutoDto), // 9
                    typeof(FaturamentoItemDto), // 10
                    typeof(EstoqueDto), // 11
                };

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync(query, types, DapperMapper, new { id });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PrescricaoItemDto>> ObterIds(List<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return null;
                }

                var query = $@"
                                SELECT
                                    {QueryHelper.CreateQueryFields<PrescricaoItem>(tableAlias: "PrescricaoItem").GetFields()},
                                    {QueryHelper.CreateQueryFields<TipoPrescricao>(tableAlias: "TipoPrescricao").GetFields()},
                                    {QueryHelper.CreateQueryFields<Divisao>(tableAlias: "Divisao").GetFields()},
                                    {QueryHelper.CreateQueryFields<FormaAplicacao>(tableAlias: "FormaAplicacao").GetFields()},
                                    {QueryHelper.CreateQueryFields<Frequencia>(tableAlias: "Frequencia").GetFields()},
                                    {QueryHelper.CreateQueryFields<TipoControle>(tableAlias: "TipoControle").GetFields()},
                                    {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "Unidade").GetFields()},
                                    {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "UnidadeRequisicao").GetFields()},
                                    {QueryHelper.CreateQueryFields<VelocidadeInfusao>(tableAlias: "VelocidadeInfusao").GetFields()},
                                    {QueryHelper.CreateQueryFields<Produto>(tableAlias: "Produto").GetFields()},
                                    {QueryHelper.CreateQueryFields<FaturamentoItem>(tableAlias: "FaturamentoItem").GetFields()},
                                    {QueryHelper.CreateQueryFields<Estoque>(tableAlias: "Estoque").GetFields()}
                                FROM AssPrescricaoItem AS PrescricaoItem 
                                    LEFT JOIN AssTipoPrescricao AS TipoPrescricao ON  PrescricaoItem.AssTipoPrescricaoId = TipoPrescricao.Id
                                    LEFT JOIN AssDivisao AS Divisao ON  PrescricaoItem.AssDivisaoId = Divisao.Id
                                    LEFT JOIN AssFormaAplicacao AS FormaAplicacao ON  PrescricaoItem.AssFormaAplicacaoId = FormaAplicacao.Id
                                    LEFT JOIN AssFrequencia AS Frequencia ON  PrescricaoItem.AssFrequenciaId = Frequencia.Id
                                    LEFT JOIN AssTipoControle AS TipoControle ON  PrescricaoItem.AssTipoControleId = TipoControle.Id
                                    LEFT JOIN Est_Unidade AS Unidade ON  PrescricaoItem.EstUnidadeId = Unidade.Id
                                    LEFT JOIN Est_Unidade AS UnidadeRequisicao ON  PrescricaoItem.EstUnidadeRequisicaoId = UnidadeRequisicao.Id
                                    LEFT JOIN AssVelocidadeInfusao AS VelocidadeInfusao ON  PrescricaoItem.AssVelocidadeInfusaoId = VelocidadeInfusao.Id
                                    LEFT JOIN Est_Produto AS Produto ON  PrescricaoItem.EstProdutoId = Produto.Id
                                    LEFT JOIN FatItem AS FaturamentoItem ON  PrescricaoItem.FatItemId = FaturamentoItem.Id
                                    LEFT JOIN Est_Estoque AS Estoque ON  PrescricaoItem.EstEstoqueId = Estoque.Id
                                WHERE  
                                    PrescricaoItem.id IN @ids
                                    AND PrescricaoItem.IsDeleted = 0";

                var types = new Type[] {
                    typeof(PrescricaoItemDto), // 0
                    typeof(TipoPrescricaoDto), // 1
                    typeof(DivisaoDto), // 2
                    typeof(FormaAplicacaoDto), // 3
                    typeof(FrequenciaDto), // 4
                    typeof(TipoControleDto), // 5
                    typeof(UnidadeDto), // 6
                    typeof(UnidadeDto), // 7
                    typeof(VelocidadeInfusaoDto), // 8
                    typeof(ProdutoDto), // 9
                    typeof(FaturamentoItemDto), // 10
                    typeof(EstoqueDto), // 11
                };

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync(query, types, DapperMapper, new { ids = ids.Distinct().ToList() });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static PrescricaoItemDto DapperMapper(object[] result)
        {
            if (result.IsNullOrEmpty() || result[0] == null)
            {
                return null;
            }

            var item = result[0] as PrescricaoItemDto;

            if (result[1] != null)
            {
                item.TipoPrescricao = result[1] as TipoPrescricaoDto;
            }

            if (result[2] != null)
            {
                item.Divisao = result[2] as DivisaoDto;
            }

            if (result[3] != null)
            {
                item.FormaAplicacao = result[3] as FormaAplicacaoDto;
            }

            if (result[4] != null)
            {
                item.Frequencia = result[4] as FrequenciaDto;
            }

            if (result[5] != null)
            {
                item.TipoControle = result[5] as TipoControleDto;
            }

            if (result[6] != null)
            {
                item.Unidade = result[6] as UnidadeDto;
            }

            if (result[7] != null)
            {
                item.UnidadeRequisicao = result[7] as UnidadeDto;
            }

            if (result[8] != null)
            {
                item.VelocidadeInfusao = result[8] as VelocidadeInfusaoDto;
            }

            if (result[9] != null)
            {
                item.Produto = result[9] as ProdutoDto;
            }

            if (result[10] != null)
            {
                item.FaturamentoItem = result[10] as FaturamentoItemDto;
            }

            if (result[11] != null)
            {
                item.Estoque = result[11] as EstoqueDto;
            }

            return item;
        }

        [UnitOfWork(false)]
        public async Task<PrescricaoItemDto> ObterPorProduto(long produtoId)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var result = await prescricaoItemRepositorio.Object
                                 .GetAll()
                                 .AsNoTracking()
                                 .Include(m => m.TipoPrescricao)
                                 .Include(m => m.Divisao)
                                 .Include(m => m.FormaAplicacao)
                                 .Include(m => m.Frequencia)
                                 .Include(m => m.TipoControle)
                                 .Include(m => m.Unidade)
                                 .Include(m => m.UnidadeRequisicao)
                                 .Include(m => m.VelocidadeInfusao)
                                 .Include(m => m.Produto)
                                 .Include(m => m.FaturamentoItem)
                                 .Include(m => m.Estoque)
                                 .Where(m => m.ProdutoId == produtoId)
                                 .FirstOrDefaultAsync().ConfigureAwait(false);
                    var prescricaoItem = PrescricaoItemDto.Mapear(result);

                    return prescricaoItem;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemDto>> ListarTodos()
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var query = prescricaoItemRepositorio.Object
                    .GetAll()
                    .AsNoTracking()
                    .OrderBy(m => m.Codigo);

                    var prescricaoItem = await query
                                             .ToListAsync().ConfigureAwait(false);

                    var tiposControlesDto = PrescricaoItemDto.Mapear(prescricaoItem);

                    return new ListResultDto<PrescricaoItemDto>
                    {
                        Items = tiposControlesDto.ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var query = prescricaoItemRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(filtro) ||
                        m.Descricao.Contains(filtro)
                        );

                    var prescricaoItem = await query
                                             .AsNoTracking()
                                             .ToListAsync().ConfigureAwait(false);

                    var tiposControlesDto = PrescricaoItemDto.Mapear(prescricaoItem);

                    return new ListResultDto<PrescricaoItemDto>
                    {
                        Items = tiposControlesDto.ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            //return await ListarCodigoDescricaoDropdown(dropdownInput, _prescricaoItemRepositorio);
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            long id = 0;
            var result = long.TryParse(dropdownInput.id, out id);
            try
            {
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var query = from p in prescricaoItemRepositorio.Object.GetAll().AsNoTracking()
                            //.Include(m => m.Divisao)
                            //.Include(m => m.Divisao.Subdivisoes)
                            .Where(m => !m.IsDiluente)
                            .Where(m => m.IsAtivo)
                            .Where(m=> !m.PrescricaoItemId.HasValue)
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.ToString().Contains(dropdownInput.search)
                            )
                            .WhereIf(id > 0, m =>
                               m.DivisaoId == id)
                                orderby p.Descricao ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<PagedResultDto<GenericoIdNome>> ListarProdutosDisponiveis(ListarInput input)
        {
            try
            {
                long grupoId = 0;
                var isGrupo = long.TryParse(input.Id, out grupoId);
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var produtoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var query = produtoRepositorio.Object.GetAll().AsNoTracking()
                    .WhereIf(isGrupo, m => m.GrupoId.Equals(grupoId))
                    .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), m =>
                        m.Descricao.Contains(input.Filtro) ||
                        m.DescricaoResumida.Contains(input.Filtro)
                        )
                    .Except(prescricaoItemRepositorio.Object.GetAll()
                    .Include(m => m.Produto)
                    .Where(m => m.ProdutoId.HasValue)
                    .Select(s => s.Produto));

                    var result = await query.ToListAsync().ConfigureAwait(false);

                    var produtos = new List<GenericoIdNome>();

                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            produtos.Add(new GenericoIdNome
                            {
                                Id = item.Id,
                                Nome = item.Descricao ?? string.Empty
                            });
                        }
                    }

                    if (input.Sorting.ToUpper().Contains("CREATIONTIME"))
                    {
                        input.Sorting = "Nome";
                    }

                    produtos = produtos
                        .AsQueryable()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToList();

                    return new PagedResultDto<GenericoIdNome>()
                    {
                        Items = produtos,
                        TotalCount = result.Count()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<GenericoIdNome>> ListarProdutosIncluidos(ListarInput input)
        {
            try
            {
                long grupoId = 0;
                var isGrupo = long.TryParse(input.Id, out grupoId);
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var query = prescricaoItemRepositorio.Object.GetAll().AsNoTracking()
                    .Include(m => m.Produto)
                    .Where(m => m.ProdutoId.HasValue)
                    .WhereIf(isGrupo, m => m.Produto.GrupoId.Equals(grupoId))
                    .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), m =>
                        m.Produto.Descricao.Contains(input.Filtro) ||
                        m.Produto.DescricaoResumida.Contains(input.Filtro) ||
                        m.Produto.Codigo.Contains(input.Filtro))
                    .Select(s => s.Produto)
                    .Distinct();

                    var result = await query.ToListAsync().ConfigureAwait(false);

                    var produtos = new List<GenericoIdNome>();

                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            produtos.Add(new GenericoIdNome
                            {
                                Id = item.Id,
                                Nome = item.Descricao ?? string.Empty
                            });
                        }

                    }

                    if (input.Sorting.ToUpper().Contains("CREATIONTIME"))
                    {
                        input.Sorting = "Nome";
                    }

                    produtos = produtos
                        .AsQueryable()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToList();

                    return new PagedResultDto<GenericoIdNome>()
                    {
                        Items = produtos,
                        TotalCount = result.Count()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<GenericoIdNome>> ListarExamesLaboratoriaisDisponiveis(ListarInput input)
        {
            try
            {
                long grupoId = 0;
                var isGrupo = long.TryParse(input.Id, out grupoId);
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var fatItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                {
                    var query = fatItemRepositorio.Object.GetAll().AsNoTracking()
                    .Include(m => m.Grupo)
                    .Include(m => m.SubGrupo)
                    .Where(f => f.Grupo.IsLaboratorio || f.IsLaboratorio || f.SubGrupo.IsLaboratorio)
                    .WhereIf(isGrupo, m => m.GrupoId == grupoId)
                    .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), m =>
                        m.Descricao.Contains(input.Filtro) ||
                        m.Codigo.Contains(input.Filtro)
                        )
                    .Except(prescricaoItemRepositorio.Object.GetAll()
                    .Include(m => m.FaturamentoItem)
                    .Where(m => m.FaturamentoItemId.HasValue)
                    .Select(s => s.FaturamentoItem));
                    //.Distinct();

                    var result = await query.ToListAsync().ConfigureAwait(false);

                    var itens = new List<GenericoIdNome>();

                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            itens.Add(new GenericoIdNome
                            {
                                Id = item.Id,
                                Nome = item.Descricao ?? string.Empty
                            });
                        }
                    }

                    if (input.Sorting.ToUpper().Contains("CREATIONTIME"))
                    {
                        input.Sorting = "Nome";
                    }

                    itens = itens
                        .AsQueryable()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToList();

                    return new PagedResultDto<GenericoIdNome>()
                    {
                        Items = itens,
                        TotalCount = result.Count()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<GenericoIdNome>> ListarExamesLaboratoriaisIncluidos(ListarInput input)
        {
            try
            {
                long grupoId = 0;
                var isGrupo = long.TryParse(input.Id, out grupoId);
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var query = prescricaoItemRepositorio.Object.GetAll().AsNoTracking()
                    .Include(m => m.FaturamentoItem)
                    .Where(m => m.FaturamentoItemId.HasValue)
                    .Where(f => f.FaturamentoItem.Grupo.IsLaboratorio || f.FaturamentoItem.IsLaboratorio || f.FaturamentoItem.SubGrupo.IsLaboratorio)
                    .WhereIf(isGrupo, m => m.FaturamentoItem.GrupoId == grupoId)
                    .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), m =>
                        m.FaturamentoItem.Descricao.Contains(input.Filtro) ||
                        m.FaturamentoItem.Codigo.Contains(input.Filtro)
                        )
                    .Select(s => s.FaturamentoItem)
                    .Distinct();

                    var result = await query.ToListAsync().ConfigureAwait(false);

                    var itens = new List<GenericoIdNome>();

                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            itens.Add(new GenericoIdNome
                            {
                                Id = item.Id,
                                Nome = item.Descricao ?? string.Empty
                            });
                        }

                    }

                    if (input.Sorting.ToUpper().Contains("CREATIONTIME"))
                    {
                        input.Sorting = "Nome";
                    }

                    itens = itens
                        .AsQueryable()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToList();

                    return new PagedResultDto<GenericoIdNome>()
                    {
                        Items = itens,
                        TotalCount = result.Count()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<GenericoIdNome>> ListarExamesImagemDisponiveis(ListarInput input)
        {
            try
            {
                long grupoId = 0;
                var isGrupo = long.TryParse(input.Id, out grupoId);
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var fatItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
                {
                    var query = fatItemRepositorio.Object.GetAll().AsNoTracking()
                    .Where(f => (f.Grupo.IsLaudo && !f.Grupo.IsLaboratorio) || (f.IsLaudo && !f.IsLaboratorio) || (f.SubGrupo.IsLaudo && !f.SubGrupo.IsLaboratorio))
                    .WhereIf(isGrupo, m => m.GrupoId == grupoId)
                    .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), m =>
                        m.Descricao.Contains(input.Filtro) ||
                        m.Codigo.Contains(input.Filtro)
                        )
                    .Except(prescricaoItemRepositorio.Object.GetAll()
                    .Include(m => m.FaturamentoItem)
                    .Where(m => m.FaturamentoItemId.HasValue)
                    .Select(s => s.FaturamentoItem))
                    .Distinct();

                    var result = await query.ToListAsync().ConfigureAwait(false);

                    var itens = new List<GenericoIdNome>();

                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            itens.Add(new GenericoIdNome
                            {
                                Id = item.Id,
                                Nome = item.Descricao ?? string.Empty
                            });
                        }
                    }

                    if (input.Sorting.ToUpper().Contains("CREATIONTIME"))
                    {
                        input.Sorting = "Nome";
                    }

                    itens = itens
                        .AsQueryable()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToList();

                    return new PagedResultDto<GenericoIdNome>()
                    {
                        Items = itens,
                        TotalCount = result.Count()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<GenericoIdNome>> ListarExamesImagemIncluidos(ListarInput input)
        {
            try
            {
                long grupoId = 0;
                var isGrupo = long.TryParse(input.Id, out grupoId);
                using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                {
                    var query = prescricaoItemRepositorio.Object.GetAll().AsNoTracking()
                    .Include(m => m.FaturamentoItem)
                    .Where(m => m.FaturamentoItemId.HasValue)
                    .Where(f => (f.FaturamentoItem.Grupo.IsLaudo && !f.FaturamentoItem.Grupo.IsLaboratorio) || (f.FaturamentoItem.IsLaudo && !f.FaturamentoItem.IsLaboratorio) || (f.FaturamentoItem.SubGrupo.IsLaudo && !f.FaturamentoItem.SubGrupo.IsLaboratorio))
                    .WhereIf(isGrupo, m => m.FaturamentoItem.GrupoId == grupoId)
                    .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), m =>
                        m.FaturamentoItem.Descricao.Contains(input.Filtro) ||
                        m.FaturamentoItem.Codigo.Contains(input.Filtro))
                    .Select(s => s.FaturamentoItem)
                    .Distinct();

                    var result = await query.ToListAsync().ConfigureAwait(false);

                    var itens = new List<GenericoIdNome>();

                    foreach (var item in result)
                    {
                        if (item != null)
                        {
                            itens.Add(new GenericoIdNome
                            {
                                Id = item.Id,
                                Nome = item.Descricao ?? string.Empty
                            });
                        }

                    }

                    if (input.Sorting.ToUpper().Contains("CREATIONTIME"))
                    {
                        input.Sorting = "Nome";
                    }

                    itens = itens
                        .AsQueryable()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToList();

                    return new PagedResultDto<GenericoIdNome>()
                    {
                        Items = itens,
                        TotalCount = result.Count()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarUnidadePorProdutoDropdown(ConfiguracaoPrescricaoItemDropDownInput input)
        {

            using (var produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
            {
                return await this.CreateSelect2<ConfiguracaoPrescricaoItemDropDownInput>()
                    .EnableDistinct(true)
                    .AddIdField("Unidade.Id").AddTextField(@"[Unidade].[Descricao]").AddOrderByClause("[Unidade].[Descricao], Unidade.Id")
                    .AddFromClause(" Est_ProdutoUnidade AS ProdutoUnidade INNER JOIN Est_Unidade AS Unidade ON ProdutoUnidade.UnidadeId = Unidade.Id ")
                    
                    .AddWhereMethod((select2Input, dapperParameters) =>
                    {
                        var whereBuilder = new StringBuilder();
                        dapperParameters.Add("deleted", false);

                        whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND ([Unidade].[Sigla] LIKE '%' + @search + '%' OR [Unidade].[Descricao] LIKE '%' + @search + '%')");

                        if (!select2Input.filtros.IsNullOrEmpty())
                        {
                            var produtoId = 0l;
                            long.TryParse(select2Input.filtros[0], out produtoId);

                            dapperParameters.Add("produtoId", produtoId);
                            whereBuilder.Append(" AND ProdutoUnidade.ProdutoId = @produtoId ");
                        }

                        whereBuilder.Append(" AND ProdutoUnidade.IsDeleted = @deleted AND Unidade.IsDeleted = @deleted");

                        if (!select2Input.JsonFilter.IsNullOrEmpty())
                        {
                            var jsonFilter = JsonConvert.DeserializeObject<ConfiguracaoPrescricaoItemJsonFilter>(select2Input.JsonFilter);
                            dapperParameters.Add("options", jsonFilter.Options);
                            dapperParameters["id"] = jsonFilter.Id;
                            if (!select2Input.filtros.IsNullOrEmpty())
                            {
                                dapperParameters["filtros"] = select2Input.filtros.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                            }

                            if (!jsonFilter.Options.IsNullOrEmpty())
                            {
                                if (!jsonFilter.Id.IsNullOrEmpty())
                                {
                                    whereBuilder.Append(" AND (Unidade.Id IN @options OR Unidade.Id IN @id) ");
                                }
                                else
                                {
                                    whereBuilder.Append(" AND Unidade.Id IN @options ");
                                }
                            }
                            else if(!jsonFilter.Id.IsNullOrEmpty())
                            {
                                whereBuilder.Append(" AND Unidade.Id IN @id ");
                            }
                        }
                        return whereBuilder.ToString();
                    }).ExecuteAsync(input);
            }
        }


        public async Task<IResultDropdownList<long>> ListarDiluenteDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput)
        {
            using (var prescricaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
            {
                return await this.CreateSelect2(prescricaoItemRepositorio.Object)
                    .EnableDistinct(true)
                    .AddIdField("AssPrescricaoItem.Id")
                    .AddTextField(@"[AssPrescricaoItem].[Descricao]")
                    .AddOrderByClause("[AssPrescricaoItem].[Descricao], AssPrescricaoItem.Id")
                    .AddWhereMethod((select2Input, dapperParameters) =>
                    {
                        var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(select2Input, dapperParameters));

                        dapperParameters.Add("isDiluente", true);

                        whereBuilder.Append(" AND AssPrescricaoItem.IsDeleted = @deleted AND IsDiluente = @isDiluente ");

                        if (!select2Input.filtros.IsNullOrEmpty())
                        {
                            var divisaoId = 0l;
                            long.TryParse(select2Input.filtros[0], out divisaoId);

                            dapperParameters.Add("divisaoId", divisaoId);
                            whereBuilder.Append(" AND AssPrescricaoItem.AssDivisaoId = @divisaoId ");
                        }


                        if (!dropdownInput.JsonFilter.IsNullOrEmpty())
                        {
                            try
                            {
                                var jsonFilter = JsonConvert.DeserializeObject<ConfiguracaoPrescricaoItemJsonFilter>(dropdownInput.JsonFilter);
                                dapperParameters.Add("options", jsonFilter.Options);
                                dapperParameters["id"] = jsonFilter.Id;
                                if (!dropdownInput.filtros.IsNullOrEmpty())
                                {
                                    dapperParameters["filtros"] = select2Input.filtros.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                                }

                                if (!jsonFilter.Options.IsNullOrEmpty())
                                {
                                    if (!jsonFilter.Id.IsNullOrEmpty())
                                    {
                                        whereBuilder.Append(" AND (AssPrescricaoItem.Id IN @options OR AssPrescricaoItem.Id IN @id) ");
                                    }
                                    else
                                    {
                                        whereBuilder.Append(" AND AssPrescricaoItem.Id IN @options ");
                                    }
                                }
                                else if (!jsonFilter.Id.IsNullOrEmpty())
                                {
                                    whereBuilder.Append(" AND AssPrescricaoItem.Id IN @id ");
                                }
                            }
                            catch(Exception e)
                            {

                            }
                        }
                        return whereBuilder.ToString();
                    }).ExecuteAsync(dropdownInput);
            }
        }

    }

    public class CriarSubItemPrescricaoViewModel
    {
        public long Id { get; set; }
        
        public long PrescricaoItemId { get; set; }
        
        public string Codigo { get; set; }
        
        public string Descricao { get; set; }
    }
    
    public class ConfiguracaoPrescricaoItemDropDownInput : DropdownInput
    {
        public string JsonFilter { get; set; }
    }

    public class ConfiguracaoPrescricaoItemJsonFilter
    {
        public List<long> Options { get; set; }

        public long[] Id { get; set; }
    }
}
