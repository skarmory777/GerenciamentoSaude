using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public class ProjetoAppService : SWMANAGERAppServiceBase, IProjetoAppService
    {
        private readonly IRepository<Projeto, long> _projetoRepository;

        public ProjetoAppService(
            IRepository<Projeto, long> projetoRepository
            )
        {
            _projetoRepository = projetoRepository;
        }

        public async Task CriarOuEditar(ProjetoDto input)
        {
            try
            {
                var projeto = input.MapTo<Projeto>();

                if (input.Id.Equals(0))
                {
                    await _projetoRepository.InsertAsync(projeto);
                }
                else
                {
                    await _projetoRepository.UpdateAsync(projeto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(ProjetoDto input)
        {
            try
            {
                await _projetoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ProjetoDto>> ListarTodos()
        {
            var contarProjetos = 0;
            List<Projeto> projetos;
            List<ProjetoDto> projetosDtos = new List<ProjetoDto>();

            try
            {
                var query = _projetoRepository
                   .GetAll();

                contarProjetos = await query
                    .CountAsync();

                projetos = await query
                  .AsNoTracking()
                    .ToListAsync();

                projetosDtos = projetos
                   .MapTo<List<ProjetoDto>>();

                return new PagedResultDto<ProjetoDto>(
               contarProjetos,
               projetosDtos
               );
                //return new PagedResultDto<ProjetoDto> { Items = projetosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ProjetoDto> Obter(long id)
        {
            try
            {
                var query = await _projetoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var projeto = query.MapTo<ProjetoDto>();

                return projeto;
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
            List<DocRotuloDto> atendimentosDto = new List<DocRotuloDto>();
            try
            {
                //get com filtro
                var query = from p in _projetoRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.DataCriacao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = 10;// await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public UsuarioIdNome UsuarioLogado(long id)
        {
            //var usersQ = from p in UserManager.Users
            //             where p.Id == id
            //             select new UsuarioIdNome(p.Id, p.UserName);
            //;

            var x = UserManager.Users.FirstOrDefault(u => u.Id == id);
            var y = new UsuarioIdNome(x.Id, x.Name + " " + x.Surname, x.EmailAddress);
            return y;
        }
    }

    public class UsuarioIdNome
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public UsuarioIdNome(long id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }
    }
}
