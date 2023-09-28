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
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class MeioPagamentoAppService : SWMANAGERAppServiceBase, IMeioPagamentoAppService
    {
        private readonly IRepository<MeioPagamento, long> _meioPagamentoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public MeioPagamentoAppService(IRepository<MeioPagamento, long> meioPagamentoRepository
                                     , IUnitOfWorkManager unitOfWorkManager)
        {
            _meioPagamentoRepository = meioPagamentoRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ListResultDto<MeioPagamentoDto>> Listar(ListarMeioPagamentoInput input)
        {
            try
            {
                List<MeioPagamentoDto> meiosPagamentosDtos = new List<MeioPagamentoDto>();


                meiosPagamentosDtos = _meioPagamentoRepository.GetAll()
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                      .Select(s => new MeioPagamentoDto
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao
                                                      }).ToList();

                return new PagedResultDto<MeioPagamentoDto>(
                  meiosPagamentosDtos.Count,
                  meiosPagamentosDtos
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<MeioPagamentoDto> Obter(long id)
        {
            try
            {
                var query = _meioPagamentoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(i => i.TipoMeioPagamento)
                    .FirstOrDefault();

                return query.MapTo<MeioPagamentoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<MeioPagamentoDto> CriarOuEditar(MeioPagamentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<MeioPagamentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.Id == 0)
                    {
                        MeioPagamento meioPagamento = input.MapTo<MeioPagamento>();
                        AsyncHelper.RunSync(() => _meioPagamentoRepository.InsertAsync(meioPagamento));

                        _retornoPadrao.ReturnObject = meioPagamento.MapTo<MeioPagamentoDto>();


                    }
                    else
                    {
                        var meioPagamento = _meioPagamentoRepository
                                                                     .GetAll()
                                                                     .FirstOrDefault(w => w.Id == input.Id);

                        if (meioPagamento != null)
                        {
                            meioPagamento.Codigo = input.Codigo;
                            meioPagamento.Descricao = input.Descricao;
                            meioPagamento.TipoMeioPagamentoId = input.TipoMeioPagamentoId;
                            meioPagamento.IsNumeroDocumentoObrigatorio = input.IsNumeroDocumentoObrigatorio;
                            meioPagamento.IsPagamentoEletronico = input.IsPagamentoEletronico;
                            meioPagamento.DiasRetencaoDebito = input.DiasRetencaoDebito;
                            meioPagamento.DiasRetencaoCredito = input.DiasRetencaoCredito;
                            meioPagamento.TaxaAdministracao = input.TaxaAdministracao;

                            AsyncHelper.RunSync(() => _meioPagamentoRepository.UpdateAsync(meioPagamento));

                            _retornoPadrao.ReturnObject = meioPagamento.MapTo<MeioPagamentoDto>();
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

        public async Task Excluir(MeioPagamentoDto input)
        {
            try
            {
                await _meioPagamentoRepository.DeleteAsync(input.Id);
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
            return await this.CreateSelect2(this._meioPagamentoRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
