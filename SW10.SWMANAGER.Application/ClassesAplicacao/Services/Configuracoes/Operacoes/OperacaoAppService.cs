using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization.Permissions;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes
{
    public class OperacaoAppService : SWMANAGERAppServiceBase, IOperacaoAppService
    {
        private readonly IRepository<Operacao, long> _operacaoRepository;
        //private readonly IListarOperacoesExcelExporter _listarOperacoesExcelExporter;
        private readonly IPermissionAppService _permissionsAppService;
        public OperacaoAppService(
            IRepository<Operacao, long> operacaoRepository,
            IPermissionAppService permissionsAppService
            //IListarOperacoesExcelExporter listarOperacoesExcelExporter
            )
        {
            _operacaoRepository = operacaoRepository;
            _permissionsAppService = permissionsAppService;
            //_listarOperacoesExcelExporter = listarOperacoesExcelExporter;
        }

        public async Task CriarOuEditar(OperacaoDto input)
        {
            try
            {
                var operacao = input.MapTo<Operacao>();
                if (input.Id.Equals(0))
                {
                    await this._operacaoRepository.InsertAsync(operacao).ConfigureAwait(false);
                }
                else
                {
                    await this._operacaoRepository.UpdateAsync(operacao).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(OperacaoDto input)
        {
            try
            {
                await this._operacaoRepository.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<OperacaoDto>> Listar(ListarInput input)
        {
            try
            {
                var query = _operacaoRepository.GetAll().WhereIf(
                    !input.Filtro.IsNullOrEmpty(),
                    m => m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) || m.Descricao.ToUpper().Contains(input.Filtro.ToUpper()));

                var contarOperacoes = await query
                                          .CountAsync().ConfigureAwait(false);

                var operacoes = await query
                                               .AsNoTracking()
                                               .OrderBy(input.Sorting)
                                               .PageBy(input)
                                               .ToListAsync().ConfigureAwait(false);

                var operacoesDtos = operacoes
                    .MapTo<List<OperacaoDto>>();

                return new PagedResultDto<OperacaoDto>(contarOperacoes, operacoesDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<OperacaoDto>> ListarPorModulo(ListarOperacaoInput input)
        {
            try
            {
                var query = _operacaoRepository
                    .GetAll().Where(m => m.ModuloId == input.ModuloId).WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) || m.Descricao.ToUpper().Contains(input.Filtro.ToUpper()));

                var contarOperacoes = await query
                                          .CountAsync().ConfigureAwait(false);

                var operacoes = await query
                                               .AsNoTracking()
                                               .OrderBy(input.Sorting)
                                               .PageBy(input)
                                               .ToListAsync().ConfigureAwait(false);

                var operacoesDtos = operacoes
                    .MapTo<List<OperacaoDto>>();

                return new PagedResultDto<OperacaoDto>(contarOperacoes, operacoesDtos);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<OperacaoDto>> ListarTodos()
        {
            try
            {
                var query = await this._operacaoRepository
                                .GetAllListAsync().ConfigureAwait(false);

                var operacoes = query.MapTo<List<OperacaoDto>>();
                return new ListResultDto<OperacaoDto> { Items = operacoes };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<FileDto> ListarParaExcel(ListarInput input)
        //{
        //    try
        //    {
        //        var result = await Listar(input);
        //        var operacoes = result.Items;
        //        return _listarOperacoesExcelExporter.ExportToFile(operacoes.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }
        //}

        public async Task<OperacaoDto> Obter(long id)
        {
            try
            {
                var result = await this._operacaoRepository
                                 .GetAllListAsync(m => m.Id == id).ConfigureAwait(false);

                var operacao = result
                    .FirstOrDefault()
                    .MapTo<OperacaoDto>();

                return operacao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<OperacaoDto> ObterPorNome(string name)
        {
            try
            {
                var result = await this._operacaoRepository
                                 .GetAllListAsync(m => m.Name.Contains(name)).ConfigureAwait(false);

                var operacao = result
                    .FirstOrDefault()
                    .MapTo<OperacaoDto>();

                return operacao;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _operacaoRepository.GetAll().WhereIf(
                                !dropdownInput.search.IsNullOrEmpty(),
                                m => m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                     || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()))
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownList() { Items = queryResultPage.Distinct().ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarPorModuloDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                long moduloId = 0;
                var isModulo = long.TryParse(dropdownInput.filtro, out moduloId);
                if (moduloId == 0)
                {
                    throw new Exception("InformarModulo");
                }
                //get com filtro
                var query = from p in _operacaoRepository.GetAll()
                            .Where(m => m.ModuloId == moduloId)
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

                int total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownList() { Items = queryResultPage.Distinct().ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public ResultDropdownList ListarPermissoesDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                //var query = from p in _operacaoRepository.GetAll()
                //        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                //        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower()) ||
                //        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                //        )
                //            orderby p.Descricao ascending
                //            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };

                var allPermissions = _permissionsAppService.GetAllPermissions();
                var permissions = allPermissions.Items.ToList().AsQueryable();
                var query = from p in permissions
                            .WhereIf(!dropdownInput.search.IsNullOrWhiteSpace(), m =>
                                 m.Name.ToLower().Contains(dropdownInput.search.ToLower()) ||
                                 m.DisplayName.ToLower().Contains(dropdownInput.search.ToLower())
                                )
                                .Where(m =>
                                    !m.Name.ToLower().Contains("create") &&
                                    !m.Name.ToLower().Contains("edit") &&
                                    !m.Name.ToLower().Contains("delete")
                                )


                            orderby p.Name ascending
                            group p by new { p.Name }
                            into mygroup
                            select new DropdownItems { id = mygroup.FirstOrDefault().Level, text = mygroup.FirstOrDefault().Name };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = query.Count();

                return new ResultDropdownList() { Items = queryResultPage.Distinct().ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
