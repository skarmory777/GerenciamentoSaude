using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.InstituicoesTransferencia;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.InstituicoesTransferencia
{
    public class InstituicaoTransferenciaAppService : SWMANAGERAppServiceBase, IInstituicaoTransferenciaAppService
    {
        private readonly IRepository<InstituicaoTransferencia, long> _InstituicaoTransferenciaRepositorio;
        //     private readonly IListarInstituicaoTransferenciaExcelExporter _listarInstituicaoTransferenciaExcelExporter;


        public InstituicaoTransferenciaAppService(
            IRepository<InstituicaoTransferencia, long> InstituicaoTransferenciaRepositorio
            //,
            //  IListarInstituicaoTransferenciaExcelExporter listarInstituicaoTransferenciaExcelExporter
            )
        {
            _InstituicaoTransferenciaRepositorio = InstituicaoTransferenciaRepositorio;
            //  _listarInstituicaoTransferenciaExcelExporter = listarInstituicaoTransferenciaExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_InstituicoesTransferencia_Edit)]
        public async Task CriarOuEditar(CriarOuEditarInstituicaoTransferencia input)
        {
            try
            {
                var InstituicaoTransferencia = input.MapTo<InstituicaoTransferencia>();
                if (input.Id.Equals(0))
                {
                    await _InstituicaoTransferenciaRepositorio.InsertOrUpdateAsync(InstituicaoTransferencia);
                }
                else
                {
                    await _InstituicaoTransferenciaRepositorio.UpdateAsync(InstituicaoTransferencia);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarInstituicaoTransferencia input)
        {
            try
            {
                await _InstituicaoTransferenciaRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<InstituicaoTransferenciaDto>> Listar(ListarInstituicoesTransferenciaInput input)
        {
            var contarInstituicoesTransferencia = 0;
            List<InstituicaoTransferencia> InstituicoesTransferencia;
            List<InstituicaoTransferenciaDto> InstituicoesTransferenciaDtos = new List<InstituicaoTransferenciaDto>();
            try
            {
                var query = _InstituicaoTransferenciaRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.CodigoSUS.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarInstituicoesTransferencia = await query
                    .CountAsync();

                InstituicoesTransferencia = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                InstituicoesTransferenciaDtos = InstituicoesTransferencia
                    .MapTo<List<InstituicaoTransferenciaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<InstituicaoTransferenciaDto>(
                contarInstituicoesTransferencia,
                InstituicoesTransferenciaDtos
                );
        }

        //public async Task<FileDto> ListarParaExcel(ListarInstituicoesTransferenciaInput input)
        //{
        //    try
        //    {
        //        var query = await Listar(input);

        //        var InstituicoesTransferenciaDtos = query.Items;

        //        return _listarInstituicaoTransferenciaExcelExporter.ExportToFile(InstituicoesTransferenciaDtos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }

        //}

        public async Task<CriarOuEditarInstituicaoTransferencia> Obter(long id)
        {
            try
            {
                var result = await _InstituicaoTransferenciaRepositorio.GetAsync(id);
                var InstituicaoTransferencia = result.MapTo<CriarOuEditarInstituicaoTransferencia>();
                return InstituicaoTransferencia;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
