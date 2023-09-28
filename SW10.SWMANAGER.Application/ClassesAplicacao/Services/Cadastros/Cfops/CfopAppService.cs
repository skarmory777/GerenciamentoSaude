using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops
{
    public class CfopAppService : SWMANAGERAppServiceBase, ICfopAppService
    {
        private readonly IRepository<Cfop, long> _cfopRepositorio;
        private readonly IListarCfopExcelExporter _listarCfopExcelExporter;


        public CfopAppService(IRepository<Cfop, long> cfopRepositorio,
            IListarCfopExcelExporter listarCfopExcelExporter)
        {
            _cfopRepositorio = cfopRepositorio;
            _listarCfopExcelExporter = listarCfopExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cfop_Edit)]
        public async Task CriarOuEditar(CriarOuEditarCfop input)
        {
            try
            {
                var cfop = input.MapTo<Cfop>();
                if (input.Id.Equals(0))
                {
                    await _cfopRepositorio.InsertOrUpdateAsync(cfop);
                }
                else
                {
                    await _cfopRepositorio.UpdateAsync(cfop);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarCfop input)
        {
            try
            {
                await _cfopRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<CfopDto>> Listar(ListarCfopsInput input)
        {
            var contarCfops = 0;
            List<Cfop> cfops;
            List<CfopDto> cfopsDtos = new List<CfopDto>();
            try
            {
                var query = _cfopRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarCfops = await query
                    .CountAsync();

                cfops = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                cfopsDtos = cfops
                    .MapTo<List<CfopDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CfopDto>(
                contarCfops,
                cfopsDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarCfopsInput input)
        {
            try
            {
                var query = await Listar(input);

                var cfopsDtos = query.Items;

                return _listarCfopExcelExporter.ExportToFile(cfopsDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarCfop> Obter(long id)
        {
            try
            {
                var result = await _cfopRepositorio.GetAsync(id);
                var cfop = result.MapTo<CriarOuEditarCfop>();
                return cfop;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<CfopDto>> ListarTodos()
        {
            List<CfopDto> cfopsDtos = new List<CfopDto>();
            try
            {
                var query = await _cfopRepositorio
                    .GetAllListAsync();

                var cfopsDto = query.MapTo<List<CfopDto>>();

                return new ListResultDto<CfopDto> { Items = cfopsDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            //return await new DropdownAppService().ListarCodigoDescricaoDropdown<Cfop>(dropdownInput, _cfopRepositorio);

            return await ListarDropdownLambda(dropdownInput
                                                                          , _cfopRepositorio
                                                                          , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                                          || m.Numero.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                                          , p => new DropdownItems { id = p.Id, text = string.Concat(p.Numero.ToString(), " - ", p.Descricao) }
                                                                          , o => o.Descricao
                                                                              );
        }

        public async Task<CriarOuEditarCfop> ObterPorNumero(long numero)
        {
            try
            {
                var result = Task.Run(() => _cfopRepositorio.GetAll()
                                    .Where(w => w.Numero == numero)
                                    .FirstOrDefault()).Result;

                var cfop = result.MapTo<CriarOuEditarCfop>();
                return cfop;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }



    }
}
