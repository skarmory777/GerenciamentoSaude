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

    public class ElementoHtmlTipoAppService : SWMANAGERAppServiceBase, IElementoHtmlTipoAppService
    {
        private readonly IRepository<ElementoHtmlTipo, long> _elementoHtmlRepositorio;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ElementoHtmlTipoAppService(
            IRepository<ElementoHtmlTipo, long> elementoHtmlRepositorio,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _elementoHtmlRepositorio = elementoHtmlRepositorio;
            _unitOfWorkManager = unitOfWorkManager;
        }
        [UnitOfWork]
        public async Task<ElementoHtmlTipoDto> CriarOuEditar(ElementoHtmlTipoDto input)
        {
            try
            {
                var elementoHtml = input.MapTo<ElementoHtmlTipo>();
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
        public async Task Excluir(ElementoHtmlTipoDto input)
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

        public async Task<PagedResultDto<ElementoHtmlTipoDto>> Listar(ListarInput input)
        {
            var contarElementoHtml = 0;
            List<ElementoHtmlTipo> elementoHtml;
            List<ElementoHtmlTipoDto> ElementoHtmlDtos = new List<ElementoHtmlTipoDto>();
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
                    .MapTo<List<ElementoHtmlTipoDto>>();

                return new PagedResultDto<ElementoHtmlTipoDto>(
                    contarElementoHtml,
                    ElementoHtmlDtos
                    );
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ElementoHtmlTipoDto> Obter(long id)
        {
            try
            {
                var elementoHtmlPrincipalDto = new ElementoHtmlTipoDto();
                var query = _elementoHtmlRepositorio
                    .GetAll()
                    .Where(m => m.Id == id);

                var dtos = await query.FirstOrDefaultAsync().ConfigureAwait(false);

                var result = dtos.MapTo<ElementoHtmlTipoDto>();

                return result;
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ElementoHtmlTipoDto>> ListarTodos()
        {
            try
            {
                var query = _elementoHtmlRepositorio
                    .GetAll();

                var elementoHtml = await query
                                       .AsNoTracking()
                                       .ToListAsync().ConfigureAwait(false);

                var elementosHtmlDto = elementoHtml
                    .MapTo<List<ElementoHtmlTipoDto>>();

                return new ListResultDto<ElementoHtmlTipoDto>
                {
                    Items = elementosHtmlDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ElementoHtmlTipoDto>> ListarFiltro(string filtro)
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
                    .MapTo<List<ElementoHtmlTipoDto>>();

                return new ListResultDto<ElementoHtmlTipoDto>
                {
                    Items = elementosHtmlDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
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
