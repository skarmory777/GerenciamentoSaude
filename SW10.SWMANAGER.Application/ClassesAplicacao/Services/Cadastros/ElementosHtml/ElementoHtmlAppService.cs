using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ElementosHtml;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class ElementoHtmlAppService : SWMANAGERAppServiceBase, IElementoHtmlAppService
    {
        private readonly IRepository<ElementoHtml, long> _elementoHtmlRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ElementoHtmlAppService(
            IRepository<ElementoHtml, long> elementoHtmlRepositorio,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _elementoHtmlRepositorio = elementoHtmlRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
        }
        [UnitOfWork]
        public async Task<ElementoHtmlDto> CriarOuEditar(ElementoHtmlDto input)
        {
            try
            {
                var elementoHtml = input.MapTo<ElementoHtml>();
                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        input.Id = await this._elementoHtmlRepositorio.InsertAndGetIdAsync(elementoHtml).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        return input;
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        elementoHtml = await this._elementoHtmlRepositorio.UpdateAsync(elementoHtml).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        return input;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }
        [UnitOfWork]
        public async Task Excluir(ElementoHtmlDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await this._elementoHtmlRepositorio.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<ElementoHtmlDto>> Listar(ListarInput input)
        {
            var contarElementoHtml = 0;
            List<ElementoHtml> elementoHtml;
            List<ElementoHtmlDto> ElementoHtmlDtos = new List<ElementoHtmlDto>();
            try
            {
                var query = _elementoHtmlRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                        );

                contarElementoHtml = await query
                                         .CountAsync().ConfigureAwait(false);

                elementoHtml = await query
                                   .AsNoTracking()
                                   .OrderBy(input.Sorting)
                                   .PageBy(input)
                                   .ToListAsync().ConfigureAwait(false);

                ElementoHtmlDtos = elementoHtml
                    .MapTo<List<ElementoHtmlDto>>();

                return new PagedResultDto<ElementoHtmlDto>(
                    contarElementoHtml,
                    ElementoHtmlDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ElementoHtmlDto> Obter(long id)
        {
            try
            {
                var elementoHtmlPrincipalDto = new ElementoHtmlDto();
                var query = _elementoHtmlRepositorio
                    .GetAll()
                    .Include(m => m.ElementoHtmlTipo)
                    .Where(m => m.Id == id);

                var dtos = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                var result = dtos.MapTo<ElementoHtmlDto>();

                return result;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ElementoHtmlDto>> ListarTodos()
        {
            try
            {
                var query = _elementoHtmlRepositorio
                    .GetAll();

                var elementoHtml = await query
                                       .AsNoTracking()
                                       .ToListAsync().ConfigureAwait(false);

                var elementosHtmlDto = elementoHtml
                    .MapTo<List<ElementoHtmlDto>>();

                return new ListResultDto<ElementoHtmlDto>
                {
                    Items = elementosHtmlDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ElementoHtmlDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = _elementoHtmlRepositorio
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(filtro.ToUpper()) ||
                        m.Descricao.ToUpper().Contains(filtro.ToUpper())
                        );

                var elementoHtml = await query
                                       .AsNoTracking()
                                       .ToListAsync().ConfigureAwait(false);

                var elementosHtmlDto = elementoHtml
                    .MapTo<List<ElementoHtmlDto>>();

                return new ListResultDto<ElementoHtmlDto>
                {
                    Items = elementosHtmlDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._elementoHtmlRepositorio).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public Task<FileDto> ListarParaExcel(ListarInput input)
        {
            throw new NotImplementedException();
        }
    }
}
