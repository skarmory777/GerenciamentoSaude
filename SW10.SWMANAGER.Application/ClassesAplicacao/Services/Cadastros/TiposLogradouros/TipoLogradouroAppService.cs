using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
//using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouro.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros
{
    using Abp.Auditing;

    using SW10.SWMANAGER.Helpers;

    public class TipoLogradouroAppService : SWMANAGERAppServiceBase, ITipoLogradouroAppService
    {
        private readonly IRepository<TipoLogradouro, long> _tipoLogradouroRepositorio;
        private readonly IListarTipoLogradouroExcelExporter _listarTiposLogradouroExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TipoLogradouroAppService(
            IRepository<TipoLogradouro, long> tipoLogradouroRepositorio,
            IUnitOfWorkManager unitOfWorkManager,
            IListarTipoLogradouroExcelExporter listarTipoLogradouroExcelExporter
            )
        {
            _tipoLogradouroRepositorio = tipoLogradouroRepositorio;
            _listarTiposLogradouroExcelExporter = listarTipoLogradouroExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task<CriarOuEditarTipoLogradouroDto> CriarOuEditar(CriarOuEditarTipoLogradouroDto input)
        {
            try
            {
                var tipoLogradouro = input.MapTo<TipoLogradouro>();
                if (input.Id.Equals(0))
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var result = await this._tipoLogradouroRepositorio.InsertAndGetIdAsync(tipoLogradouro).ConfigureAwait(false);
                        //var tipoLogradouroDto = result.MapTo<CriarOuEditarTipoLogradouroDto>();
                        input.Id = result;
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        return input;
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var result = await this._tipoLogradouroRepositorio.UpdateAsync(tipoLogradouro).ConfigureAwait(false);
                        var tipoLogradouroDto = result.MapTo<CriarOuEditarTipoLogradouroDto>();
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                        return tipoLogradouroDto;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }
        [UnitOfWork]
        public async Task Excluir(CriarOuEditarTipoLogradouroDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await this._tipoLogradouroRepositorio.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<TipoLogradouroDto>> Listar(ListarTiposLogradouroInput input)
        {
            var contarTiposLogradouro = 0;
            List<TipoLogradouro> tiposLogradouro;
            List<TipoLogradouroDto> tiposLogradouroDtos = new List<TipoLogradouroDto>();
            try
            {
                var query = _tipoLogradouroRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Abreviacao.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposLogradouro = await query
                                            .CountAsync().ConfigureAwait(false);

                tiposLogradouro = await query
                                      .AsNoTracking()
                                      .OrderBy(input.Sorting)
                                      .PageBy(input)
                                      .ToListAsync().ConfigureAwait(false);

                tiposLogradouroDtos = tiposLogradouro
                    .MapTo<List<TipoLogradouroDto>>();

                return new PagedResultDto<TipoLogradouroDto>(
                    contarTiposLogradouro,
                    tiposLogradouroDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<TipoLogradouroDto>> ListarTodos()
        {
            try
            {
                var tiposLogradouro = await this._tipoLogradouroRepositorio
                                          .GetAll()
                                          .AsNoTracking()
                                          .ToListAsync().ConfigureAwait(false);

                var tiposLograouroDto = tiposLogradouro
                    .MapTo<List<TipoLogradouroDto>>();

                return new ListResultDto<TipoLogradouroDto> { Items = tiposLograouroDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarTiposLogradouroInput input)
        {
            try
            {
                var result = await this.Listar(input).ConfigureAwait(false);
                var TiposLogradouro = result.Items;
                return _listarTiposLogradouroExcelExporter.ExportToFile(TiposLogradouro.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<CriarOuEditarTipoLogradouroDto> Obter(long id)
        {
            try
            {
                var result = await this._tipoLogradouroRepositorio.GetAsync(id).ConfigureAwait(false);
                var tipoAtendimento = result.MapTo<CriarOuEditarTipoLogradouroDto>();

                return tipoAtendimento;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<CriarOuEditarTipoLogradouroDto> Obter(string logradouro)
        {
            try
            {
                var query = await this._tipoLogradouroRepositorio.FirstOrDefaultAsync(m => m.Descricao.ToUpper().Equals(logradouro.ToUpper())).ConfigureAwait(false);
                var result = query.MapTo<CriarOuEditarTipoLogradouroDto>();

                return result;
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
            return await this.CreateSelect2(this._tipoLogradouroRepositorio).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

    }
}
