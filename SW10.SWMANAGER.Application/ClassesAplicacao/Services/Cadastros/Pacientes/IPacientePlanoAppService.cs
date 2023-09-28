using System.Threading.Tasks;
using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
	public interface IPacientePlanoAppService : IApplicationService
	{
		Task CriarOuEditar(PacientePlanoDto input);

		Task Excluir(PacientePlanoDto input);

		Task<PacientePlanoDto> Obter(long id);
	}
}