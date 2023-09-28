using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos
{
    public class FaturamentoSubGrupoAppService : SWMANAGERAppServiceBase, IFaturamentoSubGrupoAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoSubGrupo, long> _subGrupoRepository;
        //     private readonly IListarFaturamentoSubGruposExcelExporter _listarSubGruposExcelExporter;

        public FaturamentoSubGrupoAppService(
            IRepository<FaturamentoSubGrupo, long> subGrupoRepository
            //     , 
            //     IListarFaturamentoSubGruposExcelExporter listarSubGruposExcelExporter
            )
        {
            _subGrupoRepository = subGrupoRepository;
            //   _subGrupoRepository = listarSubGruposExcelExporter;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoSubGrupoDto>> Listar(ListarFaturamentoSubGruposInput input)
        {
            var itemrSubGrupos = 0;
            List<FaturamentoSubGrupo> itens;
            List<FaturamentoSubGrupoDto> itensDtos = new List<FaturamentoSubGrupoDto>();
            try
            {
                var query = _subGrupoRepository
                    .GetAll()
                    //.Include(m => m.Estado)
                    //.WhereIf(!input.EstadoId.Equals(0), m =>
                    //    m.EstadoId == input.EstadoId
                    //)
                    ;

                itemrSubGrupos = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoSubGrupoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoSubGrupoDto>(
                itemrSubGrupos,
                itensDtos
                );
        }

        public async Task<ResultDropdownList> ListarParaGrupo(DropdownInput dropdownInput)
        {
            if (string.IsNullOrEmpty(dropdownInput.filtro))
            {
                return new ResultDropdownList() { Items = new List<DropdownItems>(), TotalCount = 0 };
            }

            //      int pageInt = int.Parse(dropdownInput.page) - 1;
            //    int numberOfObjectsPerPage = 1;

            List<FaturamentoSubGrupoDto> faturamentoItensDto = new List<FaturamentoSubGrupoDto>();
            try
            {
                //if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                //{
                //    throw new Exception("NotANumber");
                //}


                // Filtro usado como id de grupo
                var query = from p in _subGrupoRepository.GetAll()
                            .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(),
                                m => m.Grupo.Id.ToString() == dropdownInput.filtro)

                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )




                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query;
                //    .Skip(numberOfObjectsPerPage * pageInt)
                //      .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };



                //    var itemrSubGrupos = 0;
                //List<FaturamentoSubGrupo> itens;
                //List<FaturamentoSubGrupoDto> itensDtos = new List<FaturamentoSubGrupoDto>();
                //try
                //{
                //    var query = _subGrupoRepository
                //        .GetAll()
                //        .Where(s => s.GrupoId.ToString() == input.Filtro)
                //        ;

                //    itemrSubGrupos = await query
                //        .CountAsync();

                //    itens = await query
                //        .AsNoTracking()
                //        .OrderBy(input.Sorting)
                //        .PageBy(input)
                //        .ToListAsync();

                //    itensDtos = itens
                //        .MapTo<List<FaturamentoSubGrupoDto>>();

                //    return new ResultDropdownList() { Items = itensDtos, TotalCount = itemrSubGrupos };

                //    //return new PagedResultDto<FaturamentoSubGrupoDto>(
                //    //itemrSubGrupos,
                //    //itensDtos
                //    //);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ResultDropdownList> ListarParaGrupoObrigatorio(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoSubGrupoDto> faturamentoItensDto = new List<FaturamentoSubGrupoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }


                // Filtro usado como id de grupo
                var query = from p in _subGrupoRepository.GetAll()
                            .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(),
                                m => m.Grupo.Id.ToString() == dropdownInput.filtro)

                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                                m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )

                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                if (dropdownInput.filtro == null)
                {
                    result.RemoveAll(f => true);
                    total = 0;
                }

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }


        public async Task CriarOuEditar(FaturamentoSubGrupoDto input)
        {
            try
            {
                var SubGrupo = input.MapTo<FaturamentoSubGrupo>();
                if (input.Id.Equals(0))
                {
                    await _subGrupoRepository.InsertAsync(SubGrupo);
                }
                else
                {
                    await _subGrupoRepository.UpdateAsync(SubGrupo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoSubGrupoDto input)
        {
            try
            {
                await _subGrupoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoSubGrupoDto> Obter(long id)
        {
            try
            {
                var query = await _subGrupoRepository
                    .GetAll()
                    .Include(m => m.Grupo)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoSubGrupoDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoSubGrupoDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _subGrupoRepository
                    .GetAll()
                    //.Include(m => m.Estado)
                    //.Where(m =>
                    //    m.Nome.ToUpper().Equals(nome.ToUpper()) &&
                    //    m.EstadoId.Equals(estadoId)
                    //)
                    ;

                var result = await query.FirstOrDefaultAsync();

                var item = result
                    .MapTo<FaturamentoSubGrupoDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoSubGruposInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarSubGruposExcelExporter.ExportToFile(itens.ToList());
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroExportar"));
            //}
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoSubGrupoDto> faturamentoItensDto = new List<FaturamentoSubGrupoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }


                // Filtro usado como id de grupo
                var query = from p in _subGrupoRepository.GetAll()
                             .WhereIf(!dropdownInput.filtro.IsNullOrEmpty(),
                            m => m.Grupo.Id.ToString() == dropdownInput.filtro)
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
