using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes
{
    public class FaturamentoItemAutorizacaoAppService : SWMANAGERAppServiceBase, IFaturamentoItemAutorizacaoAppService
    {
        private readonly IRepository<FaturamentoItemAutorizacao, long> _faturamentoItemAutorizacaoRepository;
        private readonly IRepository<FaturamentoItem, long> _faturamentoItemRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;


        public FaturamentoItemAutorizacaoAppService(IRepository<FaturamentoItemAutorizacao, long> faturamentoItemAutorizacaoRepository
                                                   , IRepository<FaturamentoItem, long> faturamentoItemRepository
                                                   , IUnitOfWorkManager unitOfWorkManager)
        {
            _faturamentoItemAutorizacaoRepository = faturamentoItemAutorizacaoRepository;
            _faturamentoItemRepository = faturamentoItemRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        //public async Task<ResultDropdownList> ListarItemFaturamentoAutorizacaoPorConvenioDropdown(DropdownInput dropdownInput)
        //{
        //    try
        //    {
        //        int pageInt = int.Parse(dropdownInput.page) - 1;
        //        int numberOfObjectsPerPage = 1;
        //        long convenioId;

        //        long.TryParse(dropdownInput.filtros[0], out convenioId);

        //        var queryfaturamentoItemAutorizacao = _faturamentoItemAutorizacaoRepository.GetAll()
        //                                                                                .Where(w => w.ConvenioId == convenioId);

        //        var query = _faturamentoItemRepository.GetAll()
        //                                              .Where(w => queryfaturamentoItemAutorizacao.Any(a => (a.FaturamentoItemId == null || a.FaturamentoItemId == w.Id)
        //                                                                                                && (a.FaturamentoGrupoId == null || a.FaturamentoGrupoId == w.GrupoId)
        //                                                                                                && (a.FaturamentoSubGrupoId == null || a.FaturamentoSubGrupoId == w.SubGrupoId)
        //                                                                                                ))
        //                                             .OrderBy(o => o.Descricao)
        //                                             .Select( s=> new DropdownItems
        //                                             {
        //                                                 id = s.Id,
        //                                                 text = string.Concat(s.Codigo, " - ", s.Descricao)
        //                                             });


        //        var queryResultPage = query
        //                       .Skip(numberOfObjectsPerPage * pageInt)
        //                       .Take(numberOfObjectsPerPage);

        //        var result = await queryResultPage.ToListAsync();

        //        int total = await query.CountAsync();

        //        return new ResultDropdownList() { Items = result, TotalCount = total };

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return null;
        //}

        public async Task<IResultDropdownList<long>> ListarItemFaturamentoAutorizacaoPorConvenioDropdown(DropdownInput dropdownInput)
        {

            long convenioId;
            long.TryParse(dropdownInput.filtros[0], out convenioId);
            var queryfaturamentoItemAutorizacao = _faturamentoItemAutorizacaoRepository.GetAll()
                                                                                    .Where(w => w.ConvenioId == convenioId);


            return await ListarDropdownLambda(dropdownInput
                                                     , _faturamentoItemRepository
                                                     , w => queryfaturamentoItemAutorizacao.Any(a => (a.FaturamentoItemId == null || a.FaturamentoItemId == w.Id)
                                                                                                  && (a.FaturamentoGrupoId == null || a.FaturamentoGrupoId == w.GrupoId)
                                                                                                  && (a.FaturamentoSubGrupoId == null || a.FaturamentoSubGrupoId == w.SubGrupoId)
                                                                                              )

                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );
        }

        public async Task<PagedResultDto<FaturamentoItemAutorizacaoDto>> Listar(ListarFaturamentoItemAutorizacaoInput input)
        {
            var query = _faturamentoItemAutorizacaoRepository.GetAll()
                                                             .Where(w => ((input.ConvenioId == null || w.ConvenioId == input.ConvenioId)
                                                                        && (input.GrupoId == null || w.FaturamentoGrupoId == input.GrupoId)
                                                                        && (input.SubGrupoId == null || w.FaturamentoSubGrupoId == input.SubGrupoId)
                                                                        ))
                                                             .Include(i => i.Convenio)
                                                             .Include(i => i.Convenio.SisPessoa)
                                                             .Include(i => i.FaturamentoSubGrupo)
                                                             .Include(i => i.FaturamentoGrupo)
                                                             .Include(i => i.FaturamentoItem);

            var totalItens = await query
                 .CountAsync();

            var itens = await query
                 .AsNoTracking()
                 //.OrderBy(input.Sorting)
                 //.PageBy(input)
                 .ToListAsync();

            var itensDtos = itens
                .MapTo<List<FaturamentoItemAutorizacaoDto>>();

            return new PagedResultDto<FaturamentoItemAutorizacaoDto>(
              totalItens,
              itensDtos
              );
        }

        public async Task<FaturamentoItemAutorizacaoDto> Obter(long id)
        {
            try
            {
                var query = await _faturamentoItemAutorizacaoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(m => m.FaturamentoGrupo)
                    .Include(m => m.FaturamentoSubGrupo)
                    .Include(m => m.FaturamentoItem)
                    .Include(m => m.Convenio)
                    .Include(i => i.Convenio.SisPessoa)
                    .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoItemAutorizacaoDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public DefaultReturn<FaturamentoItemAutorizacaoDto> CriarOuEditar(FaturamentoItemAutorizacaoDto input)
        {
            DefaultReturn<FaturamentoItemAutorizacaoDto> _retornoPadrao;

            _retornoPadrao = new DefaultReturn<FaturamentoItemAutorizacaoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            var faturamentoItemAutorizacaoDto = input.MapTo<FaturamentoItemAutorizacao>();

            try
            {
                if (_retornoPadrao.Errors.Count() == 0)
                {

                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Begin())
                        {
                            input.Id = AsyncHelper.RunSync(() => _faturamentoItemAutorizacaoRepository.InsertAndGetIdAsync(faturamentoItemAutorizacaoDto));

                            unitOfWork.Complete();
                            _unitOfWorkManager.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }

                    }
                    else
                    {
                        using (var unitOfWork = _unitOfWorkManager.Begin())
                        {
                            var edit = _faturamentoItemAutorizacaoRepository.Get(input.Id);
                            if (edit != null)
                            {
                                edit.ConvenioId = input.ConvenioId;
                                edit.FaturamentoGrupoId = input.FaturamentoGrupoId;
                                edit.FaturamentoSubGrupoId = input.FaturamentoSubGrupoId;
                                edit.FaturamentoItemId = input.FaturamentoItemId;

                                AsyncHelper.RunSync(() => _faturamentoItemAutorizacaoRepository.UpdateAsync(edit));//.MapTo<EstoquePreMovimentoDto>();

                                unitOfWork.Complete();
                                _unitOfWorkManager.Current.SaveChanges();
                                unitOfWork.Dispose();

                            }
                        }
                    }

                    _retornoPadrao.ReturnObject = input;
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
    }
}
