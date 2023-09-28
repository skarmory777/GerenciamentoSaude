using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos
{
    public class ConselhoAppService : SWMANAGERAppServiceBase, IConselhoAppService
    {
        #region Cabecalho
        private readonly IRepository<Conselho, long> _conselhoRepository;

        public ConselhoAppService(IRepository<Conselho, long> conselhoRepository)
        {
            _conselhoRepository = conselhoRepository;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<ConselhoDto>> Listar(ListarConselhosInput input)
        {
            var contarConselhos = 0;
            List<Conselho> conselhos;
            List<ConselhoDto> conselhosDtos = new List<ConselhoDto>();
            try
            {
                var query = _conselhoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                contarConselhos = await query
                    .CountAsync();

                conselhos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                conselhosDtos = conselhos
                    .MapTo<List<ConselhoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ConselhoDto>(
                contarConselhos,
                conselhosDtos
                );
        }

        public async Task CriarOuEditar(CriarOuEditarConselho input)
        {
            try
            {
                var Conselho = input.MapTo<Conselho>();
                if (input.Id.Equals(0))
                {
                    await _conselhoRepository.InsertAsync(Conselho);
                }
                else
                {
                    await _conselhoRepository.UpdateAsync(Conselho);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarConselho input)
        {
            try
            {
                await _conselhoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<CriarOuEditarConselho> Obter(long id)
        {
            try
            {
                var query = await _conselhoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var conselho = query
                    .MapTo<CriarOuEditarConselho>();

                return conselho;
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
            List<ConselhoDto> pacientesDtos = new List<ConselhoDto>();
            try
            {
                //get com filtro
                var query = from p in _conselhoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) || m.Sigla.ToLower().Contains(dropdownInput.search.ToLower())

                        )
                            orderby p.Descricao ascending
                            //select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Nome) };
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
