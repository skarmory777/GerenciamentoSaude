using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Turnos
{
	class TurnoAppService : SWMANAGERAppServiceBase, ITurnoAppService
	{
		private readonly IRepository<Turno,long> _turnoRepository;

		public TurnoAppService(IRepository<Turno,long> turnoRepository)
		{
			_turnoRepository = turnoRepository;
		}

		public async Task CriarOuEditar(TurnoDto input)
		{
			try
			{
				var turno = input.MapTo<Turno>();
				if(input.Id.Equals(0))
				{
					await _turnoRepository.InsertAsync(turno);
				}
				else
				{
					await _turnoRepository.UpdateAsync(turno);
				}
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroSalvar"));
			}

		}

		public async Task Excluir(TurnoDto input)
		{
			try
			{
				await _turnoRepository.DeleteAsync(input.Id);
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroExcluir"));
			}
		}

		public async Task<ListResultDto<TurnoDto>> ListarTodos()
		{
			List<Turno> turnos;
			List<TurnoDto> turnosDtos = new List<TurnoDto>();
			try
			{
				turnos = await _turnoRepository
                    .GetAll()
					.ToListAsync();

                turnosDtos = turnos
					.MapTo<List<TurnoDto>>();

			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroPesquisar"));
			}
			return new ListResultDto<TurnoDto> { Items = turnosDtos };
		}

		public async Task<TurnoDto> Obter(long id)
		{
			try
			{
				var result = await _turnoRepository.GetAsync(id);
				var turno = result.MapTo<TurnoDto>();
				return turno;
			}
			catch(Exception ex)
			{
				throw new UserFriendlyException(L("ErroPesquisar"));
			}

		}
	}
}
