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

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos
{
    public class SexoAppService : SWMANAGERAppServiceBase, ISexoAppService
    {
        private readonly IRepository<Sexo, long> _sexoRepository;

        public SexoAppService(IRepository<Sexo, long> sexoRepository)
        {
            _sexoRepository = sexoRepository;
        }

        public async Task CriarOuEditar(SexoDto input)
        {
            try
            {
                var sexo = input.MapTo<Sexo>();
                if (input.Id.Equals(0))
                {
                    await _sexoRepository.InsertAsync(sexo);
                }
                else
                {
                    await _sexoRepository.UpdateAsync(sexo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(SexoDto input)
        {
            try
            {
                await _sexoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<ListResultDto<SexoDto>> ListarTodos()
        {
            List<Sexo> sexos;
            List<SexoDto> sexosDtos = new List<SexoDto>();
            try
            {
                sexos = await _sexoRepository
                    .GetAll()
                    .ToListAsync();

                sexosDtos = sexos
                    .MapTo<List<SexoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<SexoDto> { Items = sexosDtos };
        }

        public async Task<SexoDto> Obter(long id)
        {
            try
            {
                var result = await _sexoRepository.GetAsync(id);
                var sexo = result.MapTo<SexoDto>();
                return sexo;
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
            //List<SexoDto> pacientesDtos = new List<SexoDto>();
            try
            {
                //get com filtro
                var query = from p in _sexoRepository.GetAll()
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
