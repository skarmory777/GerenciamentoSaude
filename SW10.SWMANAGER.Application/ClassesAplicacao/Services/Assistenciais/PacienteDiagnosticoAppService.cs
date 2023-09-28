using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.EntityFramework.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Diagnosticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    public class PacienteDiagnosticoAppService : SWMANAGERAppServiceBase, IPacienteDiagnosticoAppService
    {
        public PacienteDiagnosticoAppService(IRepository<PacienteDiagnosticos, long> pacienteDiagnosticoRepository)
        {
            this._pacienteDiagnosticoRepository = pacienteDiagnosticoRepository;
        }

        private readonly IRepository<PacienteDiagnosticos, long> _pacienteDiagnosticoRepository;

        public async Task<List<PacienteDiagnosticosDto>> DiagnosticosPorPaciente(long pacienteId, long? atendimentoId)
        {
            var query = _pacienteDiagnosticoRepository.GetAll().AsNoTracking().Include(c => c.GrupoCID).Where(c => c.PacienteId == pacienteId);

            if (atendimentoId.HasValue)
            {
                query = _pacienteDiagnosticoRepository.GetAll().AsNoTracking().Where(c => c.AtendimentoId == atendimentoId);
            }

            return (await query.ToListAsync().ConfigureAwait(false)).MapTo<List<PacienteDiagnosticosDto>>();
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PacienteDiagnosticosDto>> ListarIndexDiagnosticosPorPaciente(long pacienteId, long? atendimentoId)
        {
            var data = await this.DiagnosticosPorPaciente(pacienteId, atendimentoId).ConfigureAwait(false);
            return new PagedResultDto<PacienteDiagnosticosDto>(data.Count(), data);
        }

        public async Task Excluir(PacienteDiagnosticosDto model)
        {
            if (model != null && model.Id != 0)
            {
                await _pacienteDiagnosticoRepository.DeleteAsync(model.Id).ConfigureAwait(false); ;
            }
        }

        public async Task<PacienteDiagnosticosDto> ObterAsync(long id)
        {
            return (await _pacienteDiagnosticoRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false)).MapTo<PacienteDiagnosticosDto>();
        }

        public async Task UpsertPacienteDiagnostico(PacienteDiagnosticosDto modelDto)
        {
            var model = modelDto.MapTo<PacienteDiagnosticos>();

            if (!model.IsTransient())
            {
                this._pacienteDiagnosticoRepository.GetDbContext().Entry(model).State = EntityState.Modified;
            }

            await this._pacienteDiagnosticoRepository.InsertOrUpdateAsync(model).ConfigureAwait(false);
        }
    }
}
