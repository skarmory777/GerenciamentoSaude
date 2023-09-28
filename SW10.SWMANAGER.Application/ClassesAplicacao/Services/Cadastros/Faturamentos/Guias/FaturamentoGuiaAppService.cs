using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias
{
    using Abp.Auditing;
    using Abp.Domain.Uow;

    using SW10.SWMANAGER.Helpers;

    public class FaturamentoGuiaAppService : SWMANAGERAppServiceBase, IFaturamentoGuiaAppService
    {
        private readonly IRepository<FaturamentoGuia, long> _guiaRepository;
        private readonly IListarGuiasExcelExporter _listarFaturamentoGuiaesExcelExporter;

        public FaturamentoGuiaAppService(
            IRepository<FaturamentoGuia, long> guiaRepository,
            IListarGuiasExcelExporter listarFaturamentoGuiaesExcelExporter
            )
        {
            _guiaRepository = guiaRepository;
            _listarFaturamentoGuiaesExcelExporter = listarFaturamentoGuiaesExcelExporter;
        }

        public async Task CriarOuEditar(FaturamentoGuiaDto input)
        {
            try
            {
                var guia = input.MapTo<FaturamentoGuia>();
                if (input.Id.Equals(0))
                {
                    await _guiaRepository.InsertAsync(guia);
                }
                else
                {
                    var ori = await _guiaRepository.GetAsync(guia.Id);
                    ori.Codigo = guia.Codigo;
                    ori.Descricao = guia.Descricao;
                    ori.IsAmbulatorio = guia.IsAmbulatorio;
                    ori.IsInternacao = guia.IsInternacao;

                    await _guiaRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(FaturamentoGuiaDto input)
        {
            try
            {
                await _guiaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<FaturamentoGuiaDto>> Listar(ListarGuiasInput input)
        {
            var contarFaturamentoGuiaes = 0;
            List<FaturamentoGuia> guiaes;
            List<FaturamentoGuiaDto> guiaesDtos = new List<FaturamentoGuiaDto>();
            try
            {
                var query = _guiaRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarFaturamentoGuiaes = await query
                    .CountAsync();

                guiaes = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                guiaesDtos = guiaes
                    .MapTo<List<FaturamentoGuiaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoGuiaDto>(
                contarFaturamentoGuiaes,
                guiaesDtos
                );
        }

        public async Task<ListResultDto<FaturamentoGuiaDto>> ListarTodos()
        {
            var contarFaturamentoGuiaes = 0;
            List<FaturamentoGuia> guiaes;
            List<FaturamentoGuiaDto> guiaesDtos = new List<FaturamentoGuiaDto>();
            try
            {
                var query = _guiaRepository
                    .GetAll();

                contarFaturamentoGuiaes = await query
                    .CountAsync();

                guiaes = await query
                    .AsNoTracking()
                    .ToListAsync();

                guiaesDtos = guiaes
                    .MapTo<List<FaturamentoGuiaDto>>();

                return new ListResultDto<FaturamentoGuiaDto> { Items = guiaesDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _guiaRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Codigo.ToUpper().Contains(input.ToUpper()) ||
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                        )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                return new ListResultDto<GenericoIdNome> { Items = query };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarGuiasInput input)
        {
            try
            {
                var result = await Listar(input);
                var guiaes = result.Items;
                return _listarFaturamentoGuiaesExcelExporter.ExportToFile(guiaes.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<FaturamentoGuiaDto> Obter(long id)
        {
            try
            {
                var query = await _guiaRepository.GetAsync(id);
                var guia = query.MapTo<FaturamentoGuiaDto>();
                return guia;
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
            return await this.CreateSelect2(this._guiaRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        //public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        //{
        //    int pageInt = int.Parse(dropdownInput.page) - 1;
        //    var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
        //    var isAmb = bool.Parse(dropdownInput.filtros[0]);
        //    var isInt = bool.Parse(dropdownInput.filtros[1]);
        //    List<FaturamentoGuiaDto> pacientesDtos = new List<FaturamentoGuiaDto>();
        //    try
        //    {
        //        //get com filtro
        //        var query = from p in _guiaRepository.GetAll()
        //                .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
        //                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
        //                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
        //                )
        //                .WhereIf(isAmb, m => m.IsAmbulatorio)
        //                .WhereIf(isInt, m => m.IsInternacao)
        //                    orderby p.Descricao ascending
        //                    select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
        //        //paginação 
        //        var queryResultPage = query
        //          .Skip(numberOfObjectsPerPage * pageInt)
        //          .Take(numberOfObjectsPerPage);

        //        int total = await query.CountAsync();

        //        return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

    }
}
