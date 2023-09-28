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

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.CoresPele
{
    public class CorPeleAppService : SWMANAGERAppServiceBase, ICorPeleAppService
    {
        private readonly IRepository<CorPele, long> _corPeleRepository;

        public CorPeleAppService(IRepository<CorPele, long> corPeleRepository)
        {
            _corPeleRepository = corPeleRepository;
        }

        public async Task CriarOuEditar(CorPeleDto input)
        {
            try
            {
                var corPele = input.MapTo<CorPele>();
                if (input.Id.Equals(0))
                {
                    await _corPeleRepository.InsertAsync(corPele);
                }
                else
                {
                    await _corPeleRepository.UpdateAsync(corPele);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CorPeleDto input)
        {
            try
            {
                await _corPeleRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<CorPeleDto>> ListarTodos()
        {
            List<CorPele> coresPele;
            List<CorPeleDto> coresPeleDtos = new List<CorPeleDto>();
            try
            {
                coresPele = await _corPeleRepository
                    .GetAll()
                    .ToListAsync();

                coresPeleDtos = coresPele
                    .MapTo<List<CorPeleDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<CorPeleDto> { Items = coresPeleDtos };
        }

        public async Task<CorPeleDto> Obter(long id)
        {
            try
            {
                var result = await _corPeleRepository.GetAsync(id);
                var corPele = result.MapTo<CorPeleDto>();
                return corPele;
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
                var query = from p in _corPeleRepository.GetAll()
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
