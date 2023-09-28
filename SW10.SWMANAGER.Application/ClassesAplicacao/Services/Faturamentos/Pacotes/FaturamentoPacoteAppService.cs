using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Threading;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.Helpers;
using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;
using Abp.UI;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes
{
    public class FaturamentoPacoteAppService : SWMANAGERAppServiceBase, IFaturamentoPacoteAppService
    {
        private readonly IRepository<FaturamentoPacote, long> _faturamentoPacoteRepository;
        private readonly IRepository<FaturamentoContaItem, long> _faturamentoContaItemRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<FaturamentoConta, long> _faturamentoContaRepository;
        private readonly IFaturamentoContaItemAppService _faturamentoContaItemAppService;

        public FaturamentoPacoteAppService(IRepository<FaturamentoPacote, long> faturamentoPacoteRepository
            , IUnitOfWorkManager unitOfWorkManager
            , IRepository<FaturamentoContaItem, long> faturamentoContaItemRepository
            , IRepository<FaturamentoConta, long> faturamentoContaRepository
            , IFaturamentoContaItemAppService faturamentoContaItemAppService)
        {
            _faturamentoPacoteRepository = faturamentoPacoteRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _faturamentoContaItemRepository = faturamentoContaItemRepository;
            _faturamentoContaRepository = faturamentoContaRepository;
            _faturamentoContaItemAppService = faturamentoContaItemAppService;
        }

        public FaturamentoPacoteDto Obter(long id)
        {
            var pacote = _faturamentoPacoteRepository.GetAll()
                                                     .Where(w => w.Id == id)
                                                     .FirstOrDefault();

            if (pacote != null)
            {
                return FaturamentoPacoteDto.Mapear(pacote);
            }

            return null;
        }

        public async Task<PagedResultDto<PacoteDto>> ListarPacotesPorConta(ListarFaturamentoPacoteInput input)
        {

            var query = _faturamentoPacoteRepository.GetAll()
                                                    .Include(i => i.FaturamentoItem)
                                                    .Where(w => w.FaturamentoContaId == input.ContaId);

            var pacotesCount = await query.CountAsync();

            var pacotes = await query
                .AsNoTracking()
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();


            var pacotesDto = pacotes.Select(item => new PacoteDto
            {
                Id = item.Id, Descricao = item.FaturamentoItem.Descricao, Inicio = item.Inicio, Final = item.Final
            }).ToList();


            return new PagedResultDto<PacoteDto>(
              pacotesCount,
              pacotesDto
              );
        }

        public async Task<PagedResultDto<FaturamentoContaMedicaPacoteDto>> ListarPacotesPorContaMedica(
            FaturamentoContaPacoteFilterDto input)
        {
            const string selectStatement = @"FatPacote.Id,
                FatPacote.Inicio AS DataInicio,
                FatPacote.Final AS DataFinal,
                FatPacote.FaturamentoItemId,
                FatPacote.Qtde,
                FatItem.Descricao AS FaturamentoItemDescricao,
                FatItem.CodAmb AS FaturamentoItemCodAmb,
                FatItem.CodCbhpm AS FaturamentoItemCodCbhpm,
                FatItem.Codigo AS FaturamentoItemCodigo,
                FatItem.CodTuss AS FaturamentoItemCodTuss,
                FatItem.DescricaoTuss AS FaturamentoItemDescricaoTuss,
				countPacoteItems.TotalItensNoPacote AS TotalItensNoPacote";

            const string fromStatement = @"
                FatPacote INNER JOIN FatItem ON FatPacote.FaturamentoItemId = FatItem.Id AND FatItem.IsDeleted = @isDeleted
                LEFT JOIN (
                    SELECT COUNT(FatPacoteId) AS TotalItensNoPacote,FatContaId,FatPacoteId 
                    FROM FatContaItem WHERE IsDeleted = @isDeleted AND FatPacoteId is not null AND FatContaItem.FatContaId = @ContaMedicaId
                    GROUP BY FatContaId, FatPacoteId
                ) AS countPacoteItems ON  countPacoteItems.FatContaId = @ContaMedicaId and countPacoteItems.FatPacoteId = FatPacote.Id";

            const string whereStatement = @"FaturamentoContaId = @ContaMedicaId AND FatPacote.IsDeleted = @isDeleted";
                
                
            return await this.CreateDataTable<FaturamentoContaMedicaPacoteDto, FaturamentoContaPacoteFilterDto>()
                .AddDefaultField("FatPacote.Id")
                .AddSelectClause(selectStatement)
                .AddFromClause(fromStatement)
                .AddWhereClause(whereStatement)
                .EnablePagination(input.EnablePaginate)
                .AddWhereMethod((inputWhere, daperParamters) =>
                {
                    daperParamters.Add("isDeleted", false);
                    return "";
                })
                .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                .ExecuteAsync(input).ConfigureAwait(false);
        }

        public async Task<IResultDropdownList<long>> ListarDropdownPacoteContaMedica(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<DropdownInput>()
                .EnableDistinct()
                .AddIdField("FatPacote.FaturamentoItemId")
                .AddTextField("CONCAT(FatItem.Codigo, ' - ', FatItem.Descricao)")
                .AddFromClause("FatPacote INNER JOIN FatItem ON FatPacote.FaturamentoItemId = FatItem.Id AND FatItem.IsDeleted = 0")
                .AddWhereClause(@"FaturamentoContaId = @filtro AND FatPacote.IsDeleted = 0
                    AND (
                        @search IS NULL
	                    OR FatItem.Descricao like '%' + @search + '%'
	                    OR  FatItem.Codigo like '%' + @search + '%'
	                    OR  FatItem.CodAmb like '%' + @search + '%'
	                    OR  FatItem.CodCbhpm like '%' + @search + '%'
	                    OR  FatItem.CodTuss like '%' + @search + '%'
	                    OR  FatItem.DescricaoTuss like '%' + @search + '%'
                    )")
                .AddOrderByClause("CONCAT(FatItem.Codigo, ' - ', FatItem.Descricao) ASC")
                .ExecuteAsync(dropdownInput);
        }
        
        public async Task<IResultDropdownList<long>> ListarDropdownPacoteContaMedicaPorPacote(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<DropdownInput>()
                .EnableDistinct()
                .AddIdField("FatPacote.Id")
                .AddTextField(@"CONCAT('Codigo: ',FatPacote.Id,
                    CONCAT(' - Inicio:',format(FatPacote.Inicio,'dd/MM/yyyy HH:mm:ss')), 
                    CASE WHEN(FatPacote.Final) IS NOT NULL THEN CONCAT(' - Final:',format(FatPacote.Final,'dd/MM/yyyy HH:mm:ss')) ELSE '' END)")
                .AddFromClause("FatPacote INNER JOIN FatItem ON FatPacote.FaturamentoItemId = FatItem.Id AND FatItem.IsDeleted = 0")
                .AddWhereClause(@"FaturamentoContaId like @contaId AND FatPacote.FaturamentoItemId like @faturamentoItemId 
                    AND (
                        @search IS NULL 
	                    OR FatItem.Descricao like '%' + @search + '%'
	                    OR  FatItem.Codigo like '%' + @search + '%'
	                    OR  FatItem.CodAmb like '%' + @search + '%'
	                    OR  FatItem.CodCbhpm like '%' + @search + '%'
	                    OR  FatItem.CodTuss like '%' + @search + '%'
	                    OR  FatItem.DescricaoTuss like '%' + @search + '%'
                    )")
                .AddWhereMethod((input, dapperParameters) =>
                {
                    var contaId = "0";
                    var faturamentoItemId = "0";
                    if (!input.filtros[0].IsNullOrEmpty())
                    {
                        contaId = input.filtros[0];
                    }
                    
                    if (!input.filtros[1].IsNullOrEmpty())
                    {
                        faturamentoItemId = input.filtros[1];
                    }
                    
                    dapperParameters.Add("contaId", contaId);
                    dapperParameters.Add("faturamentoItemId", faturamentoItemId);

                    return "";
                })
                .AddOrderByClause(@"CONCAT('Codigo: ',FatPacote.Id,
                    CONCAT(' - Inicio:',format(FatPacote.Inicio,'dd/MM/yyyy HH:mm:ss')), 
                    CASE WHEN(FatPacote.Final) IS NOT NULL THEN CONCAT(' - Final:',format(FatPacote.Final,'dd/MM/yyyy HH:mm:ss')) ELSE '' END) ASC")
                .ExecuteAsync(dropdownInput);
        }
        
        

        public DefaultReturn<FaturamentoPacoteDto> InserirPacote(FaturamentoPacoteDto pacoteDto)
        {
            var _retornoPadrao = new DefaultReturn<FaturamentoPacoteDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    var pacote = new FaturamentoPacote();

                    pacote.Inicio = pacoteDto.Inicio;
                    pacote.Final = pacoteDto.Final;
                    pacote.FaturamentoItemId = pacoteDto.FaturamentoItemId;
                    pacote.FaturamentoContaId = pacoteDto.FaturamentoContaId;

                    //  long.Parse("");

                    var pacoteId = AsyncHelper.RunSync(() => _faturamentoPacoteRepository.InsertAndGetIdAsync(pacote));



                    var faturamentoContaItens = _faturamentoContaItemRepository.GetAll()
                                                                          .Where(w => w.FaturamentoContaId == pacoteDto.FaturamentoContaId
                                                                                   && w.Data >= pacoteDto.Inicio
                                                                                   && w.Data <= pacoteDto.Final
                                                                                   && w.FaturamentoPacoteId == null
                                                                                   && w.FaturamentoItem.Grupo.TipoGrupoId != 4
                                                                                   )
                                                                          .ToList();

                    foreach (var item in faturamentoContaItens)
                    {
                        item.FaturamentoPacoteId = pacoteId;
                    }


                    var conta = _faturamentoContaRepository.GetAll()
                                                           .Where(w => w.Id == pacoteDto.FaturamentoContaId)
                                                           .FirstOrDefault();

                    if (conta != null)
                    {
                        FaturamentoContaItemInsertDto faturamentoContaItemInsertDto = new FaturamentoContaItemInsertDto();

                        faturamentoContaItemInsertDto.AtendimentoId = (long)conta.AtendimentoId;
                        //faturamentoContaItemInsertDto.CentroCustoId =

                        faturamentoContaItemInsertDto.Data = DateTime.Now.Date;
                        faturamentoContaItemInsertDto.MedicoId = conta.MedicoId;
                        faturamentoContaItemInsertDto.Qtd = pacoteDto.Quantidade;

                        List<FaturamentoContaItemDto> itensFaturamento = new List<FaturamentoContaItemDto>();

                        var faturamentoContaItemDto = new FaturamentoContaItemDto { Id = (long)pacote.FaturamentoItemId, Qtde = (float)faturamentoContaItemInsertDto.Qtd };
                        faturamentoContaItemInsertDto.UnidadeOrganizacionalId = pacoteDto.UnidadeOrganizacionalId;
                        faturamentoContaItemInsertDto.TurnoId = pacoteDto.TurnoId;
                        faturamentoContaItemDto.HoraIncio = pacoteDto.HoraInicio;
                        faturamentoContaItemDto.HoraFim = pacoteDto.HoraFim;
                        faturamentoContaItemDto.FaturamentoPacoteId = pacoteId;
                        faturamentoContaItemDto.FaturamentoContaId = pacoteDto.FaturamentoContaId;

                        itensFaturamento.Add(faturamentoContaItemDto);

                        faturamentoContaItemInsertDto.ItensFaturamento = itensFaturamento;


                        _faturamentoContaItemAppService.InserirItensContaFaturamento(faturamentoContaItemInsertDto);



                    }


                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();



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

        public async Task<IResultDropdownList<long>> ListarDropdownPacoteConta(DropdownInput dropdownInput)
        {
            long contaId;

            long.TryParse(dropdownInput.filtro, out contaId);


            return await ListarDropdownLambda(dropdownInput
                                              , _faturamentoPacoteRepository
                                              , m => (m.FaturamentoContaId == contaId
                                                  && (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                                       m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()))
                                                       )
                                              , p => new DropdownItems { id = p.Id, text = string.Concat(p.FaturamentoItem.Codigo, " - ", p.FaturamentoItem.Descricao) }
                                              , o => o.Descricao
                                              );
        }



        public async Task ExcluirPacote(long id)
        {
            using (var unitOfWork = UnitOfWorkManager.Begin())
            {
                //var itemContaFaturamentoPacote = _faturamentoContaItemRepository.GetAll()
                //                                                            .Where(w => w.FaturamentoPacoteId == id
                //                                                                      && w.FaturamentoItem.Grupo.TipoGrupoId == 4)
                //                                                            .FirstOrDefault();


                //await _faturamentoContaItemRepository.DeleteAsync(itemContaFaturamentoPacote);



                var itensContaFaturamento = _faturamentoContaItemRepository.GetAll()
                                                                            .Where(w => w.FaturamentoPacoteId == id)
                                                                            .ToList();

                foreach (var item in itensContaFaturamento)
                {
                    item.FaturamentoPacoteId = null;
                }

                await _faturamentoPacoteRepository.DeleteAsync(id);

                // await _faturamentoContaItemRepository.DeleteAsync(id);

                unitOfWork.Complete();
                unitOfWork.Dispose();

            }
        }

        public async Task<DefaultReturn<DefaultReturnBool>> ExcluirItemsPacote(long contaMedicaId, List<long> itemIds, bool excluirDiversosPacotes = false)
        {
            try
            {
                var result = new DefaultReturn<DefaultReturnBool>
                {
                    ReturnObject = new DefaultReturnBool()
                };

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
                    var pacotes = items.Select(x => new { x.FaturamentoPacoteId, x.FaturamentoPacoteItemDescricao }).Distinct();
                    if (pacotes.Count() > 1 && !excluirDiversosPacotes)
                    {
                        result.Errors.AddRange(pacotes.Select(x => ErroDto.Criar(x.FaturamentoPacoteId.ToString(), x.FaturamentoPacoteItemDescricao)));
                        return result;
                    }

                    var userName = (await this.GetCurrentUserAsync()).FullName;
                    foreach (var itemId in itemIds)
                    {
                        var currentItem = items.FirstOrDefault(x => x.Id == itemId);
                        contaItemRepository.Object.Update(itemId, x => x.FaturamentoPacoteId = null);
                        await ocorrenciaRepository.Object.InsertAsync(Ocorrencia.Criar(DateTime.Now,
                            OcorrenciaTexto.ContaMedicaItemPacoteRemovido(currentItem?.ItemDescricao, currentItem?.FaturamentoPacoteItemDescricao, userName),
                            TipoOcorrencia.ContaMedica, SubTipoOcorrencia.ContaMedicaItem, typeof(FaturamentoConta).FullName, contaMedicaId));
                    }
                    result.ReturnObject.Sucesso = true;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }
    }
}
