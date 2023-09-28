using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.DocRotulos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public class DocRotuloAppService : SWMANAGERAppServiceBase, IDocRotuloAppService
    {
        private readonly IRepository<DocRotulo, long> _docRotuloRepository;

        public DocRotuloAppService(
            IRepository<DocRotulo, long> docRotuloRepository
            )
        {
            _docRotuloRepository = docRotuloRepository;
        }

        public async Task CriarOuEditar(DocRotuloDto input)
        {
            try
            {
                var docRotulo = input.MapTo<DocRotulo>();

                if (input.Id.Equals(0))
                {
                    await _docRotuloRepository.InsertAsync(docRotulo);
                }
                else
                {
                    await _docRotuloRepository.UpdateAsync(docRotulo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(DocRotuloDto input)
        {
            try
            {
                await _docRotuloRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<DocRotuloDto>> ListarCapitulos()
        {
            try
            {
                var docRotulos = await _docRotuloRepository
                    .GetAll()
                    .Where(i => i.IsCapitulo)
                    .AsNoTracking()
                    .ToListAsync();

                var docRotulosDtos = docRotulos
                    .MapTo<List<DocRotuloDto>>();

                return new PagedResultDto<DocRotuloDto> { Items = docRotulosDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<DocRotuloDto>> ListarModulos(ListardocRotulosInput input)
        {
            var contarRotulos = 0;
            List<DocRotulo> docRotulos;
            List<DocRotuloDto> docRotulosDtos = new List<DocRotuloDto>();

            try
            {
                var query = _docRotuloRepository
                    .GetAll()
                    .Where(i => i.IsModulo);

                contarRotulos = await query
                    .CountAsync();

                docRotulos = await query
                  .AsNoTracking()
                  .OrderBy(input.Sorting)
                  .PageBy(input)
                  .ToListAsync();

                docRotulosDtos = docRotulos
                  .MapTo<List<DocRotuloDto>>();

                return new PagedResultDto<DocRotuloDto>(
               contarRotulos,
               docRotulosDtos
               );

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<DocRotuloDto>> ListarStatus()
        {
            var contarRotulos = 0;
            List<DocRotulo> docRotulos;
            List<DocRotuloDto> docRotulosDtos = new List<DocRotuloDto>();

            try
            {
                var query = _docRotuloRepository
                    .GetAll()
                    .Where(i => i.IsStatus);

                contarRotulos = await query
                    .CountAsync();

                docRotulos = await query
                  .AsNoTracking()
                  //.OrderBy(input.Sorting)
                  //.PageBy(input)
                  .ToListAsync();

                docRotulosDtos = docRotulos
                  .MapTo<List<DocRotuloDto>>();

                return new PagedResultDto<DocRotuloDto>(
               contarRotulos,
               docRotulosDtos
               );

                //    /////////////////////
                //    var contarRotulos = 0;
                //List<DocRotulo> docRotulos;
                //List<DocRotuloDto> docRotulosDtos = new List<DocRotuloDto>();

                //try
                //{
                //    var docRotulos = await _docRotuloRepository
                //        .GetAll()
                //        .Where(i => i.IsStatus);

                //    var docRotulosDtos = docRotulos
                //        .MapTo<List<DocRotuloDto>>();

                //    return new PagedResultDto<DocRotuloDto> { Items = docRotulosDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<DocRotuloDto>> ListarPrioridades()
        {


            var contarRotulos = 0;
            List<DocRotulo> docRotulos;
            List<DocRotuloDto> docRotulosDtos = new List<DocRotuloDto>();

            try
            {
                var query = _docRotuloRepository
                    .GetAll()
                    .Where(i => i.IsPrioridade);

                contarRotulos = await query
                    .CountAsync();

                docRotulos = await query
                  .AsNoTracking()
                  //.OrderBy(input.Sorting)
                  //.PageBy(input)
                  .ToListAsync();

                docRotulosDtos = docRotulos
                  .MapTo<List<DocRotuloDto>>();

                return new PagedResultDto<DocRotuloDto>(
               contarRotulos,
               docRotulosDtos
               );


                //    /////////
                //    try
                //{
                //    var docRotulos = await _docRotuloRepository
                //        .GetAll()
                //        .Where(i => i.IsPrioridade)
                //        .AsNoTracking()
                //        .ToListAsync();

                //    var docRotulosDtos = docRotulos
                //        .MapTo<List<DocRotuloDto>>();

                //    return new PagedResultDto<DocRotuloDto> { Items = docRotulosDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<DocRotuloDto> Obter(long id)
        {
            try
            {
                var query = await _docRotuloRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var docRotulo = query.MapTo<DocRotuloDto>();

                return docRotulo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarCapitulosDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<DocRotuloDto> atendimentosDto = new List<DocRotuloDto>();
            try
            {
                //get com filtro
                var query = from p in _docRotuloRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            .Where(c => c.IsCapitulo)
                            orderby p.Ordem ascending
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

        public async Task<ResultDropdownList> ListarAssuntosDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<DocRotuloDto> atendimentosDto = new List<DocRotuloDto>();
            try
            {
                //get com filtro
                var query = from p in _docRotuloRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            .Where(c => c.IsAssunto)
                            orderby p.Ordem ascending
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

        public async Task<ResultDropdownList> ListarSessoesDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<DocRotuloDto> atendimentosDto = new List<DocRotuloDto>();
            try
            {
                //get com filtro
                var query = from p in _docRotuloRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            .Where(c => c.IsSessao)
                            orderby p.Ordem ascending
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

        public async Task<ResultDropdownList> ListarStatusDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<DocRotuloDto> atendimentosDto = new List<DocRotuloDto>();
            try
            {
                //get com filtro
                var query = from p in _docRotuloRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            .Where(c => c.IsStatus)
                            orderby p.Ordem ascending
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

        [AcceptVerbs("GET", "POST", "PUT")]
        public async Task<ResultDropdownList> ListarPrioridadesDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<DocRotuloDto> atendimentosDto = new List<DocRotuloDto>();
            try
            {
                //get com filtro
                var query = from p in _docRotuloRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            .Where(c => c.IsPrioridade)
                            orderby p.Ordem ascending
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

        public async Task<ResultDropdownList> ListarModulosDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<DocRotuloDto> atendimentosDto = new List<DocRotuloDto>();
            try
            {
                //get com filtro
                var query = from p in _docRotuloRepository
                            .GetAll()
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                    .Where(c => c.IsModulo)
                            orderby p.Ordem ascending
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

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _docRotuloRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
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
