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

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Escolaridades
{
    public class EscolaridadeAppService : SWMANAGERAppServiceBase, IEscolaridadeAppService
    {
        private readonly IRepository<Escolaridade, long> _escolaridadeRepository;

        public EscolaridadeAppService(IRepository<Escolaridade, long> escolaridadeRepository)
        {
            _escolaridadeRepository = escolaridadeRepository;
        }

        public async Task CriarOuEditar(EscolaridadeDto input)
        {
            try
            {
                var escolaridade = input.MapTo<Escolaridade>();
                if (input.Id.Equals(0))
                {
                    await _escolaridadeRepository.InsertAsync(escolaridade);
                }
                else
                {
                    await _escolaridadeRepository.UpdateAsync(escolaridade);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(EscolaridadeDto input)
        {
            try
            {
                await _escolaridadeRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<EscolaridadeDto>> ListarTodos()
        {
            List<Escolaridade> escolaridades;
            List<EscolaridadeDto> escolaridadesDtos = new List<EscolaridadeDto>();
            try
            {
                escolaridades = await _escolaridadeRepository
                    .GetAll()
                    .ToListAsync();

                escolaridadesDtos = escolaridades
                    .MapTo<List<EscolaridadeDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<EscolaridadeDto> { Items = escolaridadesDtos };
        }

        public async Task<EscolaridadeDto> Obter(long id)
        {
            try
            {
                var result = await _escolaridadeRepository.GetAsync(id);
                var escolaridade = result.MapTo<EscolaridadeDto>();
                return escolaridade;
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
                var query = from p in _escolaridadeRepository.GetAll()
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
