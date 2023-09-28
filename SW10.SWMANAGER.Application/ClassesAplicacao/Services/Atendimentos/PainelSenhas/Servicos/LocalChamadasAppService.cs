namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Servicos
{
    using Abp.Application.Services.Dto;
    using Abp.Auditing;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.Linq.Extensions;
    using Abp.Threading;
    using Abp.UI;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Dynamic;
    using System.Text;
    using System.Threading.Tasks;

    public class LocalChamadasAppService : SWMANAGERAppServiceBase, ILocalChamadasAppService
    {
        private readonly IRepository<LocalChamada, long> _localChamadaRepository;
        private readonly IRepository<SenhaMovimentacao, long> _senhaMovimentacaoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        /// <inheritdoc />
        public LocalChamadasAppService(
            IRepository<LocalChamada, long> localChamadaRepository,
            IRepository<SenhaMovimentacao, long> senhaMovimentacaoRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _localChamadaRepository = localChamadaRepository;
            _senhaMovimentacaoRepository = senhaMovimentacaoRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarLocalChamadaDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._localChamadaRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarLocalChamadaPorTipoDropdown(DropdownInput dropdownInput)
        {
            long tipoLocalChamadaId;

            long.TryParse(dropdownInput.filtro, out tipoLocalChamadaId);


            //var localChamada = _localChamadaRepository.GetAll()
            //                                   .Include(i => i.TipoLocalChamada)
            //                                   .Where(w => w.Id == localChamadaId)
            //                                   .FirstOrDefault();

            if (tipoLocalChamadaId <= 0)
            {
                return new ResultDropdownList<long>() { Items = new List<DropdownItems<long>>(), TotalCount = 0 };
            }

            return await this.CreateSelect2(this._localChamadaRepository).AddWhereMethod(
                       (input, dapperParameters) =>
                           {
                               var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(input, dapperParameters));

                               whereBuilder.WhereIf(
                                   tipoLocalChamadaId > 0,
                                   " AND TipoLocalChamadaId = @TipoLocalChamadaId ");

                               dapperParameters.Add("TipoLocalChamadaId", tipoLocalChamadaId);

                               return whereBuilder.ToString();
                           }).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<SenhaIndex>> ListarSenhasNaoChamadasIndex(ListarPainelSenhaInput input)
        {
            try
            {
                var query = this._senhaMovimentacaoRepository
                    .GetAll()
                    .AsNoTracking()
                    .Include(i => i.TipoLocalChamada)
                    .Include(i => i.Senha)
                    .Include(i => i.Senha.Atendimento.Paciente.SisPessoa)
                    .Where(w => (input.TipoLocalChamadaId == null || w.TipoLocalChamadaId == input.TipoLocalChamadaId) && w.LocalChamadaId == null);

                var contarSenhas = await query
                                   .CountAsync().ConfigureAwait(false);

                var senhasIndex = await query
                                      .OrderBy(input.Sorting)
                                           .PageBy(input)
                                           .AsNoTracking()
                                           .Select(item => new SenhaIndex()
                                           {
                                               SenhaMovimentoId = item.Id,
                                               NumeroSenha = item.Senha.Numero,
                                               NomePaciente = item.Senha.Atendimento != null && item.Senha.Atendimento.Paciente != null ? item.Senha.Atendimento.Paciente.NomeCompleto : string.Empty,
                                               TipoLocalChamada = item.TipoLocalChamada != null ? item.TipoLocalChamada.Descricao : string.Empty,
                                               TipoLocalChamadaId = item.TipoLocalChamadaId
                                           })
                                           .ToListAsync().ConfigureAwait(false);

                return new PagedResultDto<SenhaIndex>(contarSenhas, senhasIndex);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <inheritdoc />
        public async Task AlterarTipoLocalChamadaSenha(long senhaMovAtualId, long tipoLocalChamadaNovoId)
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var senhaMovimentacao = await this._senhaMovimentacaoRepository.GetAll().FirstOrDefaultAsync(w => w.Id == senhaMovAtualId).ConfigureAwait(false);

                if (senhaMovimentacao != null)
                {
                    senhaMovimentacao.DataHoraFinal = DateTime.Now;

                    //Qual deve ser o local d chamada correto????
                    senhaMovimentacao.LocalChamadaId = 1;

                    var senhaMovimentacaoNova = new SenhaMovimentacao
                    {
                        DataHora = DateTime.Now,
                        SenhaId = senhaMovimentacao.SenhaId,
                        TipoLocalChamadaId = tipoLocalChamadaNovoId
                    };


                    AsyncHelper.RunSync(() => _senhaMovimentacaoRepository.InsertAsync(senhaMovimentacaoNova));

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();


                }
            }
        }

        /// <inheritdoc />
        public async Task<LocalChamadaDto> Obter(long id)
        {
            var localChamada = await this._localChamadaRepository
                                   .GetAll()
                                   .AsNoTracking()
                                   .Include(i => i.TipoLocalChamada)
                                   .FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

            if (localChamada != null)
            {
                return LocalChamadaDto.Mapear(localChamada);
            }
            else
            {
                return null;
            }
        }

    }
}
