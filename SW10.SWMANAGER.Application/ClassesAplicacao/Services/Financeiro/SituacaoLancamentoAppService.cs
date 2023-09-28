using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class SituacaoLancamentoAppService : SWMANAGERAppServiceBase, ISituacaoLancamentoAppService
    {
        private readonly IRepository<SituacaoLancamento, long> _situacaoLancamentoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;



        public SituacaoLancamentoAppService(IRepository<SituacaoLancamento, long> situacaoLancamentoRepository
                                            , IUnitOfWorkManager unitOfWorkManager)
        {
            _situacaoLancamentoRepository = situacaoLancamentoRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ListResultDto<SituacaoLancamentoDto>> Listar(ListarSituacaoLancamentoInput input)
        {
            try
            {
                List<SituacaoLancamentoDto> situacaoLancamentoDtos = new List<SituacaoLancamentoDto>();


                situacaoLancamentoDtos = _situacaoLancamentoRepository.GetAll()
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                      .Select(s => new SituacaoLancamentoDto
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao
                                                      }).ToList();

                return new PagedResultDto<SituacaoLancamentoDto>(
                  situacaoLancamentoDtos.Count,
                  situacaoLancamentoDtos
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<SituacaoLancamentoDto> Obter(long id)
        {
            try
            {
                var query = _situacaoLancamentoRepository
                    .GetAll()
                    .FirstOrDefault(m => m.Id == id);

                return query.MapTo<SituacaoLancamentoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<SituacaoLancamentoDto> CriarOuEditar(SituacaoLancamentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<SituacaoLancamentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.Id == 0)
                    {
                        SituacaoLancamento situacaoLancamento = input.MapTo<SituacaoLancamento>();
                        AsyncHelper.RunSync(() => _situacaoLancamentoRepository.InsertAsync(situacaoLancamento));

                        _retornoPadrao.ReturnObject = situacaoLancamento.MapTo<SituacaoLancamentoDto>();


                    }
                    else
                    {
                        var situacaoLancamento = _situacaoLancamentoRepository
                                                                     .GetAll()
                                                                     .FirstOrDefault(w => w.Id == input.Id);

                        if (situacaoLancamento != null)
                        {
                            situacaoLancamento.Codigo = input.Codigo;
                            situacaoLancamento.Descricao = input.Descricao;
                            situacaoLancamento.IsPermiteAlteracao = input.IsPermiteAlteracao;
                            situacaoLancamento.CorLancamentoFundo = input.CorLancamentoFundo;
                            situacaoLancamento.CorLancamentoLetra = input.CorLancamentoLetra;

                            AsyncHelper.RunSync(() => _situacaoLancamentoRepository.UpdateAsync(situacaoLancamento));

                            _retornoPadrao.ReturnObject = situacaoLancamento.MapTo<SituacaoLancamentoDto>();
                        }
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

        public async Task Excluir(SituacaoLancamentoDto input)
        {
            try
            {
                await _situacaoLancamentoRepository.DeleteAsync(input.Id);
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
            return await this.CreateSelect2(this._situacaoLancamentoRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
