using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.EstadosCivis
{
    public class EstadoCivilAppService : SWMANAGERAppServiceBase, IEstadoCivilAppService
    {
        private readonly IRepository<EstadoCivil, long> _estadoCivilRepository;

        public EstadoCivilAppService(IRepository<EstadoCivil, long> estadoCivilRepository)
        {
            _estadoCivilRepository = estadoCivilRepository;
        }

        public async Task CriarOuEditar(EstadoCivilDto input)
        {
            try
            {
                var estadoCivil = input.MapTo<EstadoCivil>();
                if (input.Id.Equals(0))
                {
                    await _estadoCivilRepository.InsertAsync(estadoCivil);
                }
                else
                {
                    await _estadoCivilRepository.UpdateAsync(estadoCivil);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(EstadoCivilDto input)
        {
            try
            {
                await _estadoCivilRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<EstadoCivilDto>> ListarTodos()
        {
            List<EstadoCivil> estadosCivis;
            List<EstadoCivilDto> estadosCivisDtos = new List<EstadoCivilDto>();
            try
            {
                estadosCivis = await _estadoCivilRepository
                    .GetAll()
                    .ToListAsync();

                estadosCivisDtos = estadosCivis
                    .MapTo<List<EstadoCivilDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<EstadoCivilDto> { Items = estadosCivisDtos };
        }

        public async Task<EstadoCivilDto> Obter(long id)
        {
            try
            {
                var result = await _estadoCivilRepository.GetAsync(id);
                var estadoCivil = result.MapTo<EstadoCivilDto>();
                return estadoCivil;
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
            //List<CorPeleDto> pacientesDtos = new List<CorPeleDto>();
            try
            {
                //get com filtro
                var query = from p in _estadoCivilRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                          m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
