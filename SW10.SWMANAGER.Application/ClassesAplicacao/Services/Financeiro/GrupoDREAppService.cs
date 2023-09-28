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

    public class GrupoDREAppService : SWMANAGERAppServiceBase, IGrupoDREAppService
    {
        private readonly IRepository<GrupoDRE, long> _grupoDRERepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public GrupoDREAppService(IRepository<GrupoDRE, long> grupoDRERepository
                                , IUnitOfWorkManager unitOfWorkManager)
        {
            _grupoDRERepository = grupoDRERepository;
            _unitOfWorkManager = unitOfWorkManager;
        }


        public async Task<ListResultDto<GrupoDREDto>> Listar(ListarGrupoDREInput input)
        {
            try
            {
                List<GrupoDREDto> EstoquePreMovimentoDtos = new List<GrupoDREDto>();


                EstoquePreMovimentoDtos = _grupoDRERepository.GetAll()
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                      .Select(s => new GrupoDREDto
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao
                                                      }).ToList();

                return new PagedResultDto<GrupoDREDto>(
                  EstoquePreMovimentoDtos.Count,
                  EstoquePreMovimentoDtos
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<GrupoDREDto> Obter(long id)
        {
            try
            {
                var query = _grupoDRERepository
                    .GetAll()
                    .FirstOrDefault(m => m.Id == id);

                return query.MapTo<GrupoDREDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<GrupoDREDto> CriarOuEditar(GrupoDREDto input)
        {
            var _retornoPadrao = new DefaultReturn<GrupoDREDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.Id == 0)
                    {
                        GrupoDRE grupoDRE = input.MapTo<GrupoDRE>();
                        AsyncHelper.RunSync(() => _grupoDRERepository.InsertAsync(grupoDRE));

                        _retornoPadrao.ReturnObject = grupoDRE.MapTo<GrupoDREDto>();


                    }
                    else
                    {
                        var grupoDRE = _grupoDRERepository
                                                                     .GetAll()
                                                                     .FirstOrDefault(w => w.Id == input.Id);

                        if (grupoDRE != null)
                        {
                            grupoDRE.Codigo = input.Codigo;
                            grupoDRE.Descricao = input.Descricao;

                            AsyncHelper.RunSync(() => _grupoDRERepository.UpdateAsync(grupoDRE));

                            _retornoPadrao.ReturnObject = grupoDRE.MapTo<GrupoDREDto>();
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

        public async Task Excluir(GrupoDREDto input)
        {
            try
            {
                await _grupoDRERepository.DeleteAsync(input.Id);
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
            return await this.CreateSelect2(this._grupoDRERepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
