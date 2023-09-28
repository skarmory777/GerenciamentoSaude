using System;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes
{
	public class PacientePlanoAppService : SWMANAGERAppServiceBase, IPacientePlanoAppService
	{
		private readonly IRepository<PacientePlano,long> _pacientePlanoRepository;
		public PacientePlanoAppService(IRepository<PacientePlano,long> pacientePlanoRepository)
		{
			_pacientePlanoRepository = pacientePlanoRepository;
		}

		public async Task CriarOuEditar(PacientePlanoDto input)
		{
			try
			{
				var pacientePlano = new PacientePlano();
				pacientePlano = input.MapTo<PacientePlano>();
				if(input.Id.Equals(0))
				{
					await _pacientePlanoRepository.InsertAsync(pacientePlano);
				}
				else
				{
					await _pacientePlanoRepository.UpdateAsync(pacientePlano);
				}
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroSalvar"));
			}
		}

		public async Task Excluir(PacientePlanoDto input)
		{
			try
			{
				await _pacientePlanoRepository.DeleteAsync(input.Id);
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroExcluir"));
			}

		}

		public async Task<PacientePlanoDto> Obter(long id)
		{
			try
			{
				var result = await _pacientePlanoRepository
					.GetAsync(id);

				var pacientePlano = result
					//.FirstOrDefault()
					.MapTo<PacientePlanoDto>();

				return pacientePlano;
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroPesquisar"));
			}
		}
	}
}
