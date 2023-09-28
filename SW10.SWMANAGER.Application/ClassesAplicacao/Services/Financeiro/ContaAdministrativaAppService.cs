using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;

using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.ServicoValidacao;
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

    public class ContaAdministrativaAppService : SWMANAGERAppServiceBase, IContaAdministrativaAppService
    {
        public readonly IRepository<ContaAdministrativa, long> _contaAdministrativaRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly UserManager _userManager;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<UserEmpresa, long> _userEmpresas;

        public ContaAdministrativaAppService(IRepository<ContaAdministrativa, long> contaAdministrativaRepository
                                           , IUnitOfWorkManager unitOfWorkManager
                                           , UserManager userManager
                                             , IUserAppService userAppService,
                                             IRepository<UserEmpresa, long> userEmpresas)
        {
            _contaAdministrativaRepository = contaAdministrativaRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userManager = userManager;
            _userAppService = userAppService;
            _userEmpresas = userEmpresas;
        }

        public async Task<ListResultDto<ContaAdministrativaDto>> Listar(ListarContaAdministrativaInput input)
        {
            try
            {
                List<ContaAdministrativaDto> contaAdministrativaDtos = new List<ContaAdministrativaDto>();


                contaAdministrativaDtos = _contaAdministrativaRepository.GetAll()
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.ToString().ToUpper().Contains(input.Filtro.ToUpper()))
                                                      .Select(s => new ContaAdministrativaDto
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao
                                                      }).ToList();

                return new PagedResultDto<ContaAdministrativaDto>(
                  contaAdministrativaDtos.Count,
                  contaAdministrativaDtos
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ContaAdministrativaDto> Obter(long id)
        {
            try
            {
                var query = _contaAdministrativaRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .Include(i => i.SubGrupoContaAdministrativa)
                    .Include(i => i.RateioCentroCusto)
                    .Include(i => i.ContaAdministrativaCentrosCustos.Select(s => s.CentroCusto))
                    .Include(i => i.ContaAdministrativaEmpresas.Select(s => s.Empresa))
                    .FirstOrDefault();



                var contaAdministravaDto = query.MapTo<ContaAdministrativaDto>();
                contaAdministravaDto.ContaAdministrativaCustos = new List<ContaAdministrativaCentroCustoDto>();

                foreach (var item in query.ContaAdministrativaCentrosCustos)
                {
                    ContaAdministrativaCentroCustoDto contaAdministrativaCentroCusto = new ContaAdministrativaCentroCustoDto();

                    contaAdministrativaCentroCusto.Id = item.Id;
                    contaAdministrativaCentroCusto.Percentual = item.Percentual;
                    contaAdministrativaCentroCusto.CentroCustoId = item.CentroCustoId;
                    contaAdministrativaCentroCusto.CentroCusto = new CentroCustoDto { Id = item.CentroCusto.Id, CodigoCentroCusto = item.CentroCusto.CodigoCentroCusto, Descricao = item.CentroCusto.Descricao };
                    contaAdministravaDto.ContaAdministrativaCustos.Add(contaAdministrativaCentroCusto);
                }

                contaAdministravaDto.ContasAdministrativaEmpresas = new List<ContaAdministrativaEmpresaDto>();
                foreach (var item in query.ContaAdministrativaEmpresas)
                {
                    ContaAdministrativaEmpresaDto contaAdministrativaEmpresaDto = new ContaAdministrativaEmpresaDto();
                    contaAdministrativaEmpresaDto.Id = item.Id;
                    contaAdministrativaEmpresaDto.EmpresaId = item.EmpresaId != null ? (long)item.EmpresaId : 0;
                    contaAdministrativaEmpresaDto.Empresa = new EmpresaDto { Id = item.Empresa.Id, Descricao = item.Empresa.NomeFantasia };
                    contaAdministravaDto.ContasAdministrativaEmpresas.Add(contaAdministrativaEmpresaDto);
                }


                return contaAdministravaDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<ContaAdministrativaDto> CriarOuEditar(ContaAdministrativaDto input)
        {
            var _retornoPadrao = new DefaultReturn<ContaAdministrativaDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var rateios = JsonConvert.DeserializeObject<List<RateioCentroCustoItemIndex>>(input.CentrosCustos);
                    var empresas = JsonConvert.DeserializeObject<List<EmpresaIndex>>(input.Empresas);


                    ContaAdministrativaValidacaoService contaAdministrativaValidacaoService = new ContaAdministrativaValidacaoService();

                    _retornoPadrao = contaAdministrativaValidacaoService.Validar(input);

                    if (_retornoPadrao.Errors.Count == 0)
                    {
                        if (input.Id == 0)
                        {
                            ContaAdministrativa contaAdministrativa = input.MapTo<ContaAdministrativa>();

                            contaAdministrativa.ContaAdministrativaCentrosCustos = new List<ContaAdministrativaCentroCusto>();

                            foreach (var item in rateios)
                            {
                                var contaAdministrativaCentroCusto = new ContaAdministrativaCentroCusto();
                                contaAdministrativaCentroCusto.CentroCustoId = item.CentroCustoId;
                                contaAdministrativaCentroCusto.Percentual = item.PercentualRateio;

                                contaAdministrativa.ContaAdministrativaCentrosCustos.Add(contaAdministrativaCentroCusto);
                            }


                            contaAdministrativa.ContaAdministrativaEmpresas = new List<ContaAdministrativaEmpresa>();
                            foreach (var item in empresas)
                            {
                                var contaAdministrativaEmpresa = new ContaAdministrativaEmpresa();

                                contaAdministrativaEmpresa.EmpresaId = item.EmpresaId;
                                contaAdministrativa.ContaAdministrativaEmpresas.Add(contaAdministrativaEmpresa);

                            }


                            AsyncHelper.RunSync(() => _contaAdministrativaRepository.InsertAsync(contaAdministrativa));
                            _retornoPadrao.ReturnObject = contaAdministrativa.MapTo<ContaAdministrativaDto>();
                        }
                        else
                        {
                            var contaAdministrativa = _contaAdministrativaRepository.GetAll()
                                                                         .Where(w => w.Id == input.Id)
                                                                         .Include(i => i.ContaAdministrativaCentrosCustos.Select(s => s.CentroCusto))
                                                                         .Include(i => i.ContaAdministrativaEmpresas.Select(s => s.Empresa))
                                                                         .FirstOrDefault();

                            if (contaAdministrativa != null)
                            {
                                contaAdministrativa.Codigo = input.Codigo;
                                contaAdministrativa.Descricao = input.Descricao;
                                contaAdministrativa.IsDespesa = input.IsDespesa;
                                contaAdministrativa.IsReceita = input.IsReceita;
                                contaAdministrativa.IsNaoContabilizarPagarGerencial = input.IsNaoContabilizarPagarGerencial;
                                contaAdministrativa.IsNaoContabilizarReceberGerencial = input.IsNaoContabilizarReceberGerencial;
                                contaAdministrativa.RateioCentroCustoId = input.RateioCentroCustoId;
                                contaAdministrativa.SubGrupoContaAdministrativaId = input.SubGrupoContaAdministrativaId;


                                #region CentroCusto
                                //Exclui 
                                contaAdministrativa.ContaAdministrativaCentrosCustos.RemoveAll(r => !rateios.Any(a => a.Id == r.Id));

                                //atuliza 
                                foreach (var centroCusto in contaAdministrativa.ContaAdministrativaCentrosCustos)
                                {
                                    var novoCentroCusto = rateios.Where(w => w.Id == centroCusto.Id)
                                                                   .First();

                                    centroCusto.CentroCustoId = novoCentroCusto.CentroCustoId;
                                    centroCusto.Percentual = novoCentroCusto.PercentualRateio;

                                }

                                //inclui
                                foreach (var centroCusto in rateios.Where(w => w.Id == 0 || w.Id == null))
                                {
                                    contaAdministrativa.ContaAdministrativaCentrosCustos.Add(new ContaAdministrativaCentroCusto
                                    {
                                        CentroCustoId = centroCusto.CentroCustoId,
                                        Percentual = centroCusto.PercentualRateio
                                    });
                                }


                                #endregion

                                #region Empresas
                                //Exclui 
                                contaAdministrativa.ContaAdministrativaEmpresas.RemoveAll(r => !empresas.Any(a => a.Id == r.Id));

                                //atuliza 
                                foreach (var empresa in contaAdministrativa.ContaAdministrativaEmpresas)
                                {
                                    var novaEmpresa = empresas.Where(w => w.Id == empresa.Id)
                                                                   .First();

                                    empresa.EmpresaId = novaEmpresa.EmpresaId;

                                }

                                //inclui
                                foreach (var empresa in empresas.Where(w => w.Id == 0 || w.Id == null))
                                {
                                    contaAdministrativa.ContaAdministrativaEmpresas.Add(new ContaAdministrativaEmpresa
                                    {
                                        EmpresaId = empresa.EmpresaId
                                    });
                                }


                                #endregion



                                AsyncHelper.RunSync(() => _contaAdministrativaRepository.UpdateAsync(contaAdministrativa));
                                _retornoPadrao.ReturnObject = contaAdministrativa.MapTo<ContaAdministrativaDto>();
                            }
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

        public async Task Excluir(ContaAdministrativaDto input)
        {
            try
            {
                await this._contaAdministrativaRepository.DeleteAsync(input.Id).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarContaAdministrivaPorEmpresaDropdown(DropdownInput dropdownInput)
        {
            long empresaId;

            long.TryParse(dropdownInput.filtro, out empresaId);




            return await this.ListarDropdownLambda(dropdownInput
                       , this._contaAdministrativaRepository
                       , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                          || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                              && (((ContaAdministrativa)m).ContaAdministrativaEmpresas.Any(a => a.EmpresaId == empresaId)
                                  && ((ContaAdministrativa)m).IsReceita

                                 )

                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                       , o => o.Descricao
                   ).ConfigureAwait(true);


        }

        public async Task<IResultDropdownList<long>> ListarContaAdministrivaDespesaDropdown(DropdownInput dropdownInput)
        {
            var user = await this._userManager.GetUserByIdAsync((long)this.AbpSession.UserId).ConfigureAwait(true);
            ListResultDto<EmpresaDto> empresas = await this._userAppService.GetUserEmpresas(this.AbpSession.UserId.Value).ConfigureAwait(true);

            long empresaFiltro;

            long.TryParse(dropdownInput.filtro, out empresaFiltro);


            var empresasId = empresas.Items.Where(w => w.Id == empresaFiltro).Select(s => s.Id);

            try
            {

                return await this.ListarDropdownLambda(dropdownInput
                           , this._contaAdministrativaRepository
                           , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                              || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                  && (((ContaAdministrativa)m).ContaAdministrativaEmpresas.Any(a => empresasId.Any(e => e == a.EmpresaId))
                                      && ((ContaAdministrativa)m).IsDespesa

                                     )

                           , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                           , o => o.Descricao
                       ).ConfigureAwait(true);
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<IResultDropdownList<long>> ListarContaAdministrivaDespesaTodasEmpresasDropdown(DropdownInput dropdownInput)
        {
            var user = await this._userManager.GetUserByIdAsync((long)this.AbpSession.UserId).ConfigureAwait(true);
            ListResultDto<EmpresaDto> empresas = await this._userAppService.GetUserEmpresas(this.AbpSession.UserId.Value).ConfigureAwait(true);



            try
            {

                return await this.ListarDropdownLambda(dropdownInput
                           , this._contaAdministrativaRepository
                           , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                              || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                  && (((ContaAdministrativa)m).IsDespesa


                                     //((ContaAdministrativa)m).ContaAdministrativaEmpresas.Any(a => empresasId.Any(e => e == a.EmpresaId))
                                     //&& ((ContaAdministrativa)m).IsDespesa

                                     )

                           , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                           , o => o.Descricao
                       ).ConfigureAwait(true);
            }
            catch (Exception)
            {
                return null;
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._contaAdministrativaRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }
    }
}
