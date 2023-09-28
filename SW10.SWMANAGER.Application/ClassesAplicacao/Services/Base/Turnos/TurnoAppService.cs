using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Turnos
{
    public class TurnoAppService : SWMANAGERAppServiceBase, ITurnoAppService
    {
        private readonly IRepository<Turno, long> _turnoRepository;

        public TurnoAppService(IRepository<Turno, long> turnoRepository)
        {
            _turnoRepository = turnoRepository;
        }

        public async Task CriarOuEditar(TurnoDto input)
        {
            try
            {
                var turno = input.MapTo<Turno>();
                if (input.Id.Equals(0))
                {
                    await _turnoRepository.InsertAsync(turno);
                }
                else
                {
                    await _turnoRepository.UpdateAsync(turno);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TurnoDto input)
        {
            try
            {
                await _turnoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
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
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
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
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            //List<TurnoDto> pacientesDtos = new List<TurnoDto>();
            try
            {
                //get com filtro
                var query = from p in _turnoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        m.Descricao.Contains(dropdownInput.search)
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = query.Count();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
