using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposVinculosEmpregaticios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios
{
    public class TipoVinculoEmpregaticioAppService : SWMANAGERAppServiceBase, ITipoVinculoEmpregaticioAppService
    {
        private readonly IRepository<TipoVinculoEmpregaticio, long> _TipoVinculoEmpregaticioRepository;
        private readonly IListarTiposVinculosEmpregaticiosExcelExporter _listarTiposVinculosEmpregaticiosExcelExporter;

        public TipoVinculoEmpregaticioAppService(IRepository<TipoVinculoEmpregaticio, long> TipoVinculoEmpregaticioRepository, IListarTiposVinculosEmpregaticiosExcelExporter listarTiposVinculosEmpregaticiosExcelExporter)
        {
            _TipoVinculoEmpregaticioRepository = TipoVinculoEmpregaticioRepository;
            _listarTiposVinculosEmpregaticiosExcelExporter = listarTiposVinculosEmpregaticiosExcelExporter;
        }

        public async Task CriarOuEditar(TipoVinculoEmpregaticioDto input)
        {
            try
            {
                var TipoVinculoEmpregaticio = input.MapTo<TipoVinculoEmpregaticio>();
                if (input.Id.Equals(0))
                {
                    await _TipoVinculoEmpregaticioRepository.InsertAsync(TipoVinculoEmpregaticio);
                }
                else
                {
                    await _TipoVinculoEmpregaticioRepository.UpdateAsync(TipoVinculoEmpregaticio);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TipoVinculoEmpregaticioDto input)
        {
            try
            {
                await _TipoVinculoEmpregaticioRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoVinculoEmpregaticioDto>> Listar(ListarTiposVinculosEmpregaticiosInput input)
        {
            var contarTiposVinculosEmpregaticios = 0;
            List<TipoVinculoEmpregaticio> TiposVinculosEmpregaticios;
            List<TipoVinculoEmpregaticioDto> TiposVinculosEmpregaticiosDtos = new List<TipoVinculoEmpregaticioDto>();
            try
            {
                var query = _TipoVinculoEmpregaticioRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposVinculosEmpregaticios = await query
                    .CountAsync();

                TiposVinculosEmpregaticios = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                TiposVinculosEmpregaticiosDtos = TiposVinculosEmpregaticios
                    .MapTo<List<TipoVinculoEmpregaticioDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoVinculoEmpregaticioDto>(
                contarTiposVinculosEmpregaticios,
                TiposVinculosEmpregaticiosDtos
                );
        }

        public async Task<ListResultDto<TipoVinculoEmpregaticioDto>> ListarTodos()
        {
            try
            {
                var TiposVinculosEmpregaticios = await _TipoVinculoEmpregaticioRepository
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();

                var TiposVinculosEmpregaticiosDtos = TiposVinculosEmpregaticios
                    .MapTo<List<TipoVinculoEmpregaticioDto>>();

                return new ListResultDto<TipoVinculoEmpregaticioDto> { Items = TiposVinculosEmpregaticiosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposVinculosEmpregaticiosInput input)
        {
            try
            {
                var result = await Listar(input);
                var TiposVinculosEmpregaticios = result.Items;
                return _listarTiposVinculosEmpregaticiosExcelExporter.ExportToFile(TiposVinculosEmpregaticios.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<TipoVinculoEmpregaticioDto> Obter(long id)
        {
            try
            {
                var result = await _TipoVinculoEmpregaticioRepository
                    .GetAsync(id);

                var TipoVinculoEmpregaticio = result
                    //.FirstOrDefault()
                    .MapTo<TipoVinculoEmpregaticioDto>();

                return TipoVinculoEmpregaticio;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
