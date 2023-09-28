using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class RateioCentroCustoAppService : SWMANAGERAppServiceBase, IRateioCentroCustoAppService
    {
        private readonly IRepository<RateioCentroCusto, long> _rateioCentroCustoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RateioCentroCustoAppService(IRepository<RateioCentroCusto, long> rateioCentroCustoRepository
                                         , IUnitOfWorkManager unitOfWorkManager)
        {
            _rateioCentroCustoRepository = rateioCentroCustoRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }



        public async Task<ListResultDto<RateioCentroCustoDto>> Listar(ListarRateioCentroCustoInput input)
        {
            try
            {
                List<RateioCentroCustoDto> rateioCentroCustoDtos = new List<RateioCentroCustoDto>();


                rateioCentroCustoDtos = _rateioCentroCustoRepository.GetAll()
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                      .Select(s => new RateioCentroCustoDto
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao
                                                      }).ToList();

                return new PagedResultDto<RateioCentroCustoDto>(
                  rateioCentroCustoDtos.Count,
                  rateioCentroCustoDtos
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<RateioCentroCustoDto> Obter(long id)
        {
            try
            {
                var query = await _rateioCentroCustoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(i => i.RateioCentroCustoItens)
                    .Include(i => i.RateioCentroCustoItens.Select(s => s.CentroCusto))
                    .FirstOrDefaultAsync();

                var rateioCentroCustoDto = query.MapTo<RateioCentroCustoDto>();

                rateioCentroCustoDto.RateioCentroCustoItensDto = new List<RateioCentroCustoItemDto>();

                long idGrid = 0;

                foreach (var item in query.RateioCentroCustoItens)
                {
                    RateioCentroCustoItemDto rateioCentroCustoItemDto = item.MapTo<RateioCentroCustoItemDto>();
                    rateioCentroCustoItemDto.CentroCustoDescricao = string.Concat(item.CentroCusto.CodigoCentroCusto, " - ", item.CentroCusto.Descricao);
                    rateioCentroCustoItemDto.IdGrid = idGrid++;
                    rateioCentroCustoDto.RateioCentroCustoItensDto.Add(rateioCentroCustoItemDto);
                }

                return rateioCentroCustoDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }



        public string Obter2(long id)
        {
            try
            {
                var query = _rateioCentroCustoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(i => i.RateioCentroCustoItens)
                    .Include(i => i.RateioCentroCustoItens.Select(s => s.CentroCusto))
                    .FirstOrDefault();

                var rateioCentroCustoItens = new List<RateioCentroCustoItemIndex>();

                long idGrid = 0;

                foreach (var item in query.RateioCentroCustoItens)
                {
                    RateioCentroCustoItemIndex rateioCentroCustoItem = new RateioCentroCustoItemIndex();
                    rateioCentroCustoItem.CentroCustoId = item.CentroCustoId;
                    rateioCentroCustoItem.PercentualRateio = item.PercentualRateio;
                    rateioCentroCustoItem.CentroCustoDescricao = string.Concat(item.CentroCusto.CodigoCentroCusto, " - ", item.CentroCusto.Descricao);
                    rateioCentroCustoItem.IdGrid = idGrid++;
                    rateioCentroCustoItens.Add(rateioCentroCustoItem);
                }

                return JsonConvert.SerializeObject(rateioCentroCustoItens);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public DefaultReturn<RateioCentroCustoDto> CriarOuEditar(RateioCentroCustoDto input)
        {
            var _retornoPadrao = new DefaultReturn<RateioCentroCustoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {

                var rateioItensDto = JsonConvert.DeserializeObject<List<RateioCentroCustoItemDto>>(input.CentrosCustos);

                var somaPercentual = rateioItensDto.Sum(s => s.PercentualRateio);

                if (somaPercentual != 100)
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "RAT0001", Descricao = "Soma dos percentuais deve ser igual a 100." });
                }

                if (_retornoPadrao.Errors.Count == 0)
                {

                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {

                        if (input.Id == 0)
                        {
                            RateioCentroCusto rateioCentroCusto = input.MapTo<RateioCentroCusto>();

                            rateioCentroCusto.RateioCentroCustoItens = new List<RateioCentroCustoItem>();

                            foreach (var item in rateioItensDto)
                            {
                                rateioCentroCusto.RateioCentroCustoItens.Add(item.MapTo<RateioCentroCustoItem>());
                            }

                            AsyncHelper.RunSync(() => _rateioCentroCustoRepository.InsertAsync(rateioCentroCusto));
                            _retornoPadrao.ReturnObject = rateioCentroCusto.MapTo<RateioCentroCustoDto>();

                        }
                        else
                        {
                            var rateioCentroCusto = _rateioCentroCustoRepository.GetAll()
                                                                         .Where(w => w.Id == input.Id)
                                                                         .Include(i => i.RateioCentroCustoItens)
                                                                         .FirstOrDefault();

                            if (rateioCentroCusto != null)
                            {
                                rateioCentroCusto.Codigo = input.Codigo;
                                rateioCentroCusto.Descricao = input.Descricao;




                                //Exclui 
                                rateioCentroCusto.RateioCentroCustoItens.RemoveAll(r => !rateioItensDto.Any(a => a.Id == r.Id));

                                //atuliza 
                                foreach (var rateioItem in rateioCentroCusto.RateioCentroCustoItens)
                                {
                                    var novoRateioItem = rateioItensDto.Where(w => w.Id == rateioItem.Id)
                                                                   .First();

                                    rateioItem.CentroCustoId = novoRateioItem.CentroCustoId;
                                    rateioItem.PercentualRateio = novoRateioItem.PercentualRateio;

                                }

                                //inclui
                                foreach (var rateioItem in rateioItensDto.Where(w => w.Id == 0))
                                {
                                    rateioCentroCusto.RateioCentroCustoItens.Add(rateioItem.MapTo<RateioCentroCustoItem>());

                                }



                                AsyncHelper.RunSync(() => _rateioCentroCustoRepository.UpdateAsync(rateioCentroCusto));

                                _retornoPadrao.ReturnObject = rateioCentroCusto.MapTo<RateioCentroCustoDto>();
                            }
                        }

                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
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

        public async Task Excluir(RateioCentroCustoDto input)
        {
            try
            {
                await _rateioCentroCustoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._rateioCentroCustoRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
