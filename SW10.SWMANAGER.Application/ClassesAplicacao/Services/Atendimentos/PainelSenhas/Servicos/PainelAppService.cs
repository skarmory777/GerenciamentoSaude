using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Servicos
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class PainelAppService : SWMANAGERAppServiceBase, IPainelAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Painel, long> _painelRepository;

        public PainelAppService(IRepository<Painel, long> painelRepository
                               , IUnitOfWorkManager unitOfWorkManager)
        {
            _painelRepository = painelRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }


        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _painelRepository.DeleteAsync(id);

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PainelDto> Obter(long id)
        {
            try
            {
                var result = await _painelRepository.GetAll()
                    .AsNoTracking()
                    .Include(i => i.PaineisTipoLocaisChamadas)
                    .Include(i => i.PaineisTipoLocaisChamadas.Select(s => s.TipoLocalChamada))
                    .FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

                return PainelDto.Mapear(result);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PainelDto>> Listar(ListarPainelSenhaInput input)
        {
            var contarPaineis = 0;
            List<Painel> paineis;
            List<PainelDto> paineisDtos = new List<PainelDto>();
            try
            {
                var query = _painelRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Descricao.Contains(input.Filtro));

                contarPaineis = await query
                                    .CountAsync().ConfigureAwait(false);

                paineis = await query
                              .OrderBy(input.Sorting)
                              .PageBy(input)
                              .ToListAsync().ConfigureAwait(false);
                paineisDtos = paineis.Select(PainelDto.Mapear).ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<PainelDto>(
                contarPaineis,
                paineisDtos);
        }

        public DefaultReturn<PainelDto> CriarOuEditar(PainelDto input)
        {
            var _retornoPadrao =
                new DefaultReturn<PainelDto>
                {
                    Warnings = new List<ErroDto>(),
                    Errors = new List<ErroDto>()
                };
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var tiposLoaisChamadas = JsonConvert.DeserializeObject<List<TipoLocalChamadaIndex>>(input.TipoLocalChamadas);



                    //   ContaAdministrativaValidacaoService contaAdministrativaValidacaoService = new ContaAdministrativaValidacaoService();

                    // _retornoPadrao = contaAdministrativaValidacaoService.Validar(input);

                    if (_retornoPadrao.Errors.Count == 0)
                    {
                        if (input.Id == 0)
                        {
                            Painel painel = PainelDto.Mapear(input);

                            painel.PaineisTipoLocaisChamadas = new List<PainelTipoLocalChamada>();

                            foreach (var item in tiposLoaisChamadas)
                            {
                                var painelTipoLocalChamada = new PainelTipoLocalChamada();
                                painelTipoLocalChamada.TipoLocalChamadaId = item.TipoLocalChamdaId;

                                painel.PaineisTipoLocaisChamadas.Add(painelTipoLocalChamada);
                            }

                            AsyncHelper.RunSync(() => _painelRepository.InsertAsync(painel));
                            _retornoPadrao.ReturnObject = PainelDto.Mapear(painel);
                        }
                        else
                        {
                            var painel = _painelRepository
                                .GetAll()
                                .Include(i => i.PaineisTipoLocaisChamadas)
                                .FirstOrDefault(w => w.Id == input.Id);

                            if (painel != null)
                            {
                                painel.Codigo = input.Codigo;
                                painel.Descricao = input.Descricao;

                                #region Tipo local chamada
                                //Exclui 
                                painel.PaineisTipoLocaisChamadas.RemoveAll(r => !tiposLoaisChamadas.Any(a => a.Id == r.Id));


                                //inclui
                                foreach (var tipoLocalChamada in tiposLoaisChamadas.Where(w => w.Id == 0 || w.Id == null))
                                {
                                    painel.PaineisTipoLocaisChamadas.Add(new PainelTipoLocalChamada
                                    {
                                        TipoLocalChamadaId = tipoLocalChamada.TipoLocalChamdaId
                                    });
                                }


                                #endregion


                                AsyncHelper.RunSync(() => _painelRepository.UpdateAsync(painel));
                                _retornoPadrao.ReturnObject = PainelDto.Mapear(painel);
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

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._painelRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }

}
