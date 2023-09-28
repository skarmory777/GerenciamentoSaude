using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
    using Abp.Application.Services.Dto;
    using System.Collections.Generic;

    public class PacientePesoAppService : SWMANAGERAppServiceBase, IPacientePesoAppService
    {
        private readonly IRepository<PacientePeso, long> _pacientePesoRepository;
        public PacientePesoAppService(IRepository<PacientePeso, long> pacientePesoRepository)
        {
            _pacientePesoRepository = pacientePesoRepository;
        }

        public async Task CriarOuEditar(PacientePesoDto input)
        {
            try
            {
                var pacientePeso = new PacientePeso();
                pacientePeso = input.MapTo<PacientePeso>();
                if (input.Id.Equals(0))
                {
                    await _pacientePesoRepository.InsertAsync(pacientePeso);
                }
                else
                {
                    await _pacientePesoRepository.UpdateAsync(pacientePeso);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(PacientePesoDto input)
        {
            try
            {
                await _pacientePesoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PacientePesoDto> Obter(long id)
        {
            try
            {
                var result = await _pacientePesoRepository
                    .GetAll()
                    .Include(m => m.Paciente)
                    .Include(m => m.Paciente.SisPessoa)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();


                var pacientePeso = result
                    //.FirstOrDefault()
                    .MapTo<PacientePesoDto>();

                return pacientePeso;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// The listar index async.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<PagedResultDto<PacientePesoDto>> ListarIndexAsync(long id)
        {
            var result =
                (await this._pacientePesoRepository.GetAll().AsNoTracking().Where(c => c.PacienteId == id)
                     .OrderByDescending(c => c.DataPesagem).ToListAsync().ConfigureAwait(false))
                .MapTo<List<PacientePesoDto>>();

            return new PagedResultDto<PacientePesoDto>(result.Count, result);
        }
    }
}