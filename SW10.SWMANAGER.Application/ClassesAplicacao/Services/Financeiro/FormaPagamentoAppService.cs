using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class FormaPagamentoAppService : SWMANAGERAppServiceBase, IFormaPagamentoAppService
    {
        private readonly IRepository<FormaPagamento, long> _formaPagamentoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FormaPagamentoAppService(IRepository<FormaPagamento, long> formaPagamentoRepository
                                       , IUnitOfWorkManager unitOfWorkManager)
        {
            _formaPagamentoRepository = formaPagamentoRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ListResultDto<FormaPagamentoDto>> Listar(ListarFormaPagamentoInput input)
        {
            try
            {
                List<FormaPagamentoDto> EstoquePreMovimentoDtos = new List<FormaPagamentoDto>();


                EstoquePreMovimentoDtos = _formaPagamentoRepository.GetAll()
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                      .Select(s => new FormaPagamentoDto
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao
                                                      }).ToList();

                return new PagedResultDto<FormaPagamentoDto>(
                  EstoquePreMovimentoDtos.Count,
                  EstoquePreMovimentoDtos
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(dropdownInput
                                                     , _formaPagamentoRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );
        }

        public async Task<FormaPagamentoDto> Obter(long id)
        {
            try
            {
                var query = _formaPagamentoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefault();

                return query.MapTo<FormaPagamentoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<FormaPagamentoDto> CriarOuEditar(FormaPagamentoDto input)
        {
            var _retornoPadrao = new DefaultReturn<FormaPagamentoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    if (input.Id == 0)
                    {
                        FormaPagamento formaPagamento = input.MapTo<FormaPagamento>();
                        AsyncHelper.RunSync(() => _formaPagamentoRepository.InsertAsync(formaPagamento));

                        _retornoPadrao.ReturnObject = formaPagamento.MapTo<FormaPagamentoDto>();


                    }
                    else
                    {
                        var formaPagamento = _formaPagamentoRepository.GetAll()
                                                                     .Where(w => w.Id == input.Id)
                                                                     .FirstOrDefault();

                        if (formaPagamento != null)
                        {
                            formaPagamento.Codigo = input.Codigo;
                            formaPagamento.Descricao = input.Descricao;

                            formaPagamento.NumeroParcelas = input.NumeroParcelas;
                            formaPagamento.PercentualDesconto = input.PercentualDesconto;
                            formaPagamento.DiasParcela1 = input.DiasParcela1;
                            formaPagamento.DiasParcela2 = input.DiasParcela2;
                            formaPagamento.DiasParcela3 = input.DiasParcela3;
                            formaPagamento.DiasParcela4 = input.DiasParcela4;
                            formaPagamento.DiasParcela5 = input.DiasParcela5;
                            formaPagamento.DiasParcela6 = input.DiasParcela6;
                            formaPagamento.DiasParcela7 = input.DiasParcela7;
                            formaPagamento.DiasParcela8 = input.DiasParcela8;

                            AsyncHelper.RunSync(() => _formaPagamentoRepository.UpdateAsync(formaPagamento));

                            _retornoPadrao.ReturnObject = formaPagamento.MapTo<FormaPagamentoDto>();
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

        public async Task Excluir(FormaPagamentoDto input)
        {
            try
            {
                await _formaPagamentoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }
    }
}