using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Religioes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Religioes
{
    public class ReligiaoAppService : SWMANAGERAppServiceBase, IReligiaoAppService
    {
        private readonly IRepository<Religiao, long> _religiaoRepository;

        public ReligiaoAppService(IRepository<Religiao, long> religiaoRepository)
        {
            _religiaoRepository = religiaoRepository;
        }

        public async Task CriarOuEditar(ReligiaoDto input)
        {
            try
            {
                var religiao = input.MapTo<Religiao>();
                if (input.Id.Equals(0))
                {
                    await _religiaoRepository.InsertAsync(religiao);
                }
                else
                {
                    await _religiaoRepository.UpdateAsync(religiao);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(ReligiaoDto input)
        {
            try
            {
                await _religiaoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<ReligiaoDto>> ListarTodos()
        {
            List<Religiao> religioes;
            List<ReligiaoDto> religioesDtos = new List<ReligiaoDto>();
            try
            {
                religioes = await _religiaoRepository
                    .GetAll()
                    .ToListAsync();

                religioesDtos = religioes
                    .MapTo<List<ReligiaoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<ReligiaoDto> { Items = religioesDtos };
        }

        public async Task<ReligiaoDto> Obter(long id)
        {
            try
            {
                var result = await _religiaoRepository.GetAsync(id);
                var religiao = result.MapTo<ReligiaoDto>();
                return religiao;
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
            //List<ReligiaoDto> pacientesDtos = new List<ReligiaoDto>();
            try
            {
                //get com filtro
                var query = from p in _religiaoRepository.GetAll()
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
