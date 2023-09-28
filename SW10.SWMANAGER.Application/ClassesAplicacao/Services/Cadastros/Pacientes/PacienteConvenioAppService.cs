using System;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using Abp.AutoMapper;
using Abp.UI;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
	public class PacienteConvenioAppService : SWMANAGERAppServiceBase, IPacienteConvenioAppService
	{
		private readonly IRepository<PacienteConvenio,long> _pacienteConvenioRepository;
		public PacienteConvenioAppService(IRepository<PacienteConvenio,long> pacienteConvenioRepository)
		{
			_pacienteConvenioRepository = pacienteConvenioRepository;
		}

		public async Task CriarOuEditar(PacienteConvenioDto input)
		{
			try
			{
				var pacienteConvenio = new PacienteConvenio();
				pacienteConvenio = input.MapTo<PacienteConvenio>();
				if(input.Id.Equals(0))
				{
					await _pacienteConvenioRepository.InsertAsync(pacienteConvenio);
				}
				else
				{
					await _pacienteConvenioRepository.UpdateAsync(pacienteConvenio);
				}
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroSalvar"));
			}
		}

		public async Task Excluir(PacienteConvenioDto input)
		{
			try
			{
				await _pacienteConvenioRepository.DeleteAsync(input.Id);
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroExcluir"));
			}

		}

		public async Task<PacienteConvenioDto> Obter(long id)
		{
			try
			{
				var result = await _pacienteConvenioRepository
					.GetAsync(id);

				var pacienteConvenio = result
					//.FirstOrDefault()
					.MapTo<PacienteConvenioDto>();

				return pacienteConvenio;
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroPesquisar"));
			}
		}
	}
}
