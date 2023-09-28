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

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone
{
    public class TipoTelefoneAppService : SWMANAGERAppServiceBase, ITipoTelefoneAppService
    {
        private readonly IRepository<TipoTelefone, long> _tipoTelefoneRepository;

        public TipoTelefoneAppService(IRepository<TipoTelefone, long> tipoTelefoneRepository)
        {
            _tipoTelefoneRepository = tipoTelefoneRepository;
        }

        public async Task CriarOuEditar(TipoTelefoneDto input)
        {
            try
            {
                var tipoTelefone = input.MapTo<TipoTelefone>();
                if (input.Id.Equals(0))
                {
                    await _tipoTelefoneRepository.InsertAsync(tipoTelefone);
                }
                else
                {
                    await _tipoTelefoneRepository.UpdateAsync(tipoTelefone);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TipoTelefoneDto input)
        {
            try
            {
                await _tipoTelefoneRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<TipoTelefoneDto>> ListarTodos()
        {
            List<TipoTelefone> tiposTelefone;
            List<TipoTelefoneDto> tiposTelefoneDtos = new List<TipoTelefoneDto>();
            try
            {
                tiposTelefone = await _tipoTelefoneRepository
                    .GetAll()
                    .ToListAsync();

                tiposTelefoneDtos = tiposTelefone
                    .MapTo<List<TipoTelefoneDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<TipoTelefoneDto> { Items = tiposTelefoneDtos };
        }

        public async Task<TipoTelefoneDto> Obter(long id)
        {
            try
            {
                var result = await _tipoTelefoneRepository.GetAsync(id);
                var tipoTelefone = result.MapTo<TipoTelefoneDto>();
                return tipoTelefone;
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
            //List<TipoTelefoneDto> pacientesDtos = new List<TipoTelefoneDto>();
            try
            {
                //get com filtro
                var query = from p in _tipoTelefoneRepository.GetAll()
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
