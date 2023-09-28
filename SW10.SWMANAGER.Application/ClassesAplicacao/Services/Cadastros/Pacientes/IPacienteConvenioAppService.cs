using System.Threading.Tasks;
using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
	public interface IPacienteConvenioAppService : IApplicationService
	{
		Task CriarOuEditar (PacienteConvenioDto input);

		Task Excluir (PacienteConvenioDto input);

		Task<PacienteConvenioDto> Obter (long id);
	}
}
