using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposPessoa
{
    public class TipoPessoaAppService : SWMANAGERAppServiceBase, ITipoPessoaAppService
    {
        private readonly IRepository<TipoPessoa, long> _tipoPessoaRepository;

        public TipoPessoaAppService(IRepository<TipoPessoa, long> tipoPessoaRepository)
        {
            _tipoPessoaRepository = tipoPessoaRepository;
        }

        public async Task CriarOuEditar(TipoPessoaDto input)
        {
            try
            {
                var tipoPessoa = input.MapTo<TipoPessoa>();
                if (input.Id.Equals(0))
                {
                    await _tipoPessoaRepository.InsertAsync(tipoPessoa);
                }
                else
                {
                    await _tipoPessoaRepository.UpdateAsync(tipoPessoa);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TipoPessoaDto input)
        {
            try
            {
                await _tipoPessoaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<TipoPessoaDto>> ListarTodos()
        {
            List<TipoPessoa> tiposPessoa;
            List<TipoPessoaDto> tiposPessoaDtos = new List<TipoPessoaDto>();
            try
            {
                tiposPessoa = await _tipoPessoaRepository
                    .GetAll()
                    .ToListAsync();

                tiposPessoaDtos = tiposPessoa
                    .MapTo<List<TipoPessoaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<TipoPessoaDto> { Items = tiposPessoaDtos };
        }

        public async Task<TipoPessoaDto> Obter(long id)
        {
            try
            {
                var result = await _tipoPessoaRepository.GetAsync(id);
                var tipoPessoa = result.MapTo<TipoPessoaDto>();
                return tipoPessoa;
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
            //List<TipoPessoaDto> pacientesDtos = new List<TipoPessoaDto>();
            try
            {
                //get com filtro
                var query = from p in _tipoPessoaRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        //m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Id, " - ", p.Descricao) };
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
