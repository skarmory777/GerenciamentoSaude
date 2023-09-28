using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class PrescricaoItemHoraAppService : SWMANAGERAppServiceBase, IPrescricaoItemHoraAppService
    {
        private readonly IRepository<PrescricaoItemHora, long> _prescricaoItemHoraRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PrescricaoItemHoraAppService(IRepository<PrescricaoItemHora, long> prescricaoItemHoraRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _prescricaoItemHoraRepository = prescricaoItemHoraRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task CriarOuEditar(PrescricaoItemHoraDto input)
        {
            try
            {
                var prescricaoItemHora = PrescricaoItemHoraDto.Mapear(input); //input.MapTo<PrescricaoItemHora>();

                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await _prescricaoItemHoraRepository.InsertAndGetIdAsync(prescricaoItemHora);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                else
                {
                    var entidade = await _prescricaoItemHoraRepository.GetAsync(prescricaoItemHora.Id);
                    entidade.Codigo = prescricaoItemHora.Codigo;
                    entidade.CreationTime = prescricaoItemHora.CreationTime;
                    entidade.CreatorUserId = prescricaoItemHora.CreatorUserId;
                    entidade.DeleterUserId = prescricaoItemHora.DeleterUserId;
                    entidade.DeletionTime = prescricaoItemHora.DeletionTime;
                    entidade.Descricao = prescricaoItemHora.Descricao;
                    entidade.DataMedicamento = prescricaoItemHora.DataMedicamento;
                    entidade.DiaMedicamento = prescricaoItemHora.DiaMedicamento;
                    entidade.Hora = prescricaoItemHora.Hora;
                    entidade.PrescricaoItemRespostaId = prescricaoItemHora.PrescricaoItemRespostaId;
                    entidade.HoraDiaId = prescricaoItemHora.HoraDiaId;

                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _prescricaoItemHoraRepository.UpdateAsync(entidade);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
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
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _prescricaoItemHoraRepository.DeleteAsync(id);
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

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoItemHoraDto>> Listar(ListarInput input)
        {
            var contarPrescricoesMedicas = 0;
            var prescricoesItensHorasDto = new List<PrescricaoItemHoraDto>();
            try
            {
                var principalId = string.IsNullOrWhiteSpace(input.PrincipalId) ? 0 : Convert.ToInt64(input.PrincipalId);
                var query = _prescricaoItemHoraRepository
                    .GetAll()
                    .AsNoTracking()
                    .Include(m => m.PrescricaoItemResposta)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.Hora.Contains(input.Filtro));

                var prescricoesItensHoras = await query
                                                .OrderBy(input.Sorting)
                                                .PageBy(input)
                                                .ToListAsync().ConfigureAwait(false);

                contarPrescricoesMedicas = await query.CountAsync().ConfigureAwait(false);

                prescricoesItensHorasDto = PrescricaoItemHoraDto.Mapear(prescricoesItensHoras).ToList();

                return new PagedResultDto<PrescricaoItemHoraDto>(contarPrescricoesMedicas, prescricoesItensHorasDto);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemHoraDto>> ListarTodos()
        {
            try
            {
                var query = _prescricaoItemHoraRepository.GetAll().AsNoTracking();

                var prescricoesItensHorasDto = await query.ToListAsync().ConfigureAwait(false);

                var result = PrescricaoItemHoraDto.Mapear(prescricoesItensHorasDto).ToList();

                return new ListResultDto<PrescricaoItemHoraDto> { Items = result };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemHoraDto>> ListarPorItem(long respostaId)
        {
            try
            {
                var query = _prescricaoItemHoraRepository.GetAll().AsNoTracking().Where(m => m.PrescricaoItemRespostaId == respostaId);

                var prescricoesItensHorasDto = await query.ToListAsync().ConfigureAwait(false);

                var result = PrescricaoItemHoraDto.Mapear(prescricoesItensHorasDto).ToList();

                return new ListResultDto<PrescricaoItemHoraDto> { Items = result };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemHoraDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = _prescricaoItemHoraRepository.GetAll().AsNoTracking();

                var prescricoesItensHoras = await query.ToListAsync().ConfigureAwait(false);

                var prescricoesItensHorasDto = PrescricaoItemHoraDto.Mapear(prescricoesItensHoras).ToList();

                return new ListResultDto<PrescricaoItemHoraDto> { Items = prescricoesItensHorasDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._prescricaoItemHoraRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            //return await this.ListarCodigoDescricaoDropdown(dropdownInput, this._prescricaoItemHoraRepository).ConfigureAwait(false);
        }

        [UnitOfWork(false)]
        public async Task<PrescricaoItemHoraDto> Obter(long id)
        {
            try
            {
                var m = await _prescricaoItemHoraRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                var result = PrescricaoItemHoraDto.Mapear(m);
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<List<PrescricaoItemHoraDto>> ObterListaPorItem(long respostaId)
        {
            try
            {
                var query = _prescricaoItemHoraRepository
                    .GetAll()
                    .AsNoTracking()
                    .Where(m => m.PrescricaoItemRespostaId == respostaId);

                var prescricoesItensHorasDto = await query.ToListAsync().ConfigureAwait(false);

                return PrescricaoItemHoraDto.Mapear(prescricoesItensHorasDto).ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
