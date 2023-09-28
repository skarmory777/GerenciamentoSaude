using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.EntityFramework.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    public class PacienteAlergiasAppService : SWMANAGERAppServiceBase, IPacienteAlergiasAppService
    {
        private readonly IRepository<PacienteAlergias, long> _pacienteAlergiasAppService;
        public PacienteAlergiasAppService(IRepository<PacienteAlergias, long> pacienteAlergiasAppService)
        {
            this._pacienteAlergiasAppService = pacienteAlergiasAppService;
        }


        public async Task<List<PacienteAlergiasDto>> AlergiasPorPaciente(long pacienteId, long? atendimentoId)
        {
            var query = _pacienteAlergiasAppService.GetAll().AsNoTracking().Where(c => c.PacienteId == pacienteId);

            if (atendimentoId.HasValue)
            {
                query = _pacienteAlergiasAppService.GetAll().AsNoTracking().Where(c => c.AtendimentoId == atendimentoId);
            }

            return (await query.ToListAsync().ConfigureAwait(false)).MapTo<List<PacienteAlergiasDto>>();
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PacienteAlergiasDto>> ListarIndexAlergiasPorPaciente(long pacienteId, long? atendimentoId)
        {
            var data = await this.AlergiasPorPaciente(pacienteId, atendimentoId).ConfigureAwait(false);
            return new PagedResultDto<PacienteAlergiasDto>(data.Count(), data);
        }

        public async Task Excluir(PacienteAlergiasDto model)
        {
            if (model != null && model.Id != 0)
            {
                await _pacienteAlergiasAppService.DeleteAsync(model.Id).ConfigureAwait(false); ;
            }
        }

        public async Task<PacienteAlergiasDto> ObterAsync(long id)
        {
            return (await _pacienteAlergiasAppService.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false)).MapTo<PacienteAlergiasDto>();
        }

        public async Task UpsertPacienteAlergia(PacienteAlergiasDto modelDto)
        {
            var model = modelDto.MapTo<PacienteAlergias>();

            if (!model.IsTransient())
            {
                this._pacienteAlergiasAppService.GetDbContext().Entry(model).State = EntityState.Modified;
            }

            await this._pacienteAlergiasAppService.InsertOrUpdateAsync(model).ConfigureAwait(false);
        }
    }
}
