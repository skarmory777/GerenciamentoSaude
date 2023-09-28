using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Intervalos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos
{
    public class IntervaloAppService : SWMANAGERAppServiceBase, IIntervaloAppService
    {
        private readonly IRepository<Intervalo, long> _intervaloRepository;
        private readonly IListarIntervalosExcelExporter _listarIntervalosExcelExporter;

        public IntervaloAppService(IRepository<Intervalo, long> intervaloRepository, IListarIntervalosExcelExporter listarIntervalosExcelExporter)
        {
            _intervaloRepository = intervaloRepository;
            _listarIntervalosExcelExporter = listarIntervalosExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarIntervalo input)
        {
            try
            {
                var intervalo = input.MapTo<Intervalo>();
                if (input.Id.Equals(0))
                {
                    await _intervaloRepository.InsertAsync(intervalo);
                }
                else
                {
                    await _intervaloRepository.UpdateAsync(intervalo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarIntervalo input)
        {
            try
            {
                await _intervaloRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<IntervaloDto>> Listar(ListarIntervalosInput input)
        {
            var contarIntervalos = 0;
            List<Intervalo> intervalos;
            List<IntervaloDto> intervalosDtos = new List<IntervaloDto>();
            try
            {
                var query = _intervaloRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.IntervaloMinutos.ToString().ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarIntervalos = await query
                    .CountAsync();

                intervalos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                intervalosDtos = intervalos
                    .MapTo<List<IntervaloDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<IntervaloDto>(
                contarIntervalos,
                intervalosDtos
                );
        }

        public async Task<ListResultDto<IntervaloDto>> ListarTodos()
        {
            try
            {
                var intervalos = await _intervaloRepository
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();

                var intervalosDtos = intervalos
                    .MapTo<List<IntervaloDto>>();

                return new ListResultDto<IntervaloDto> { Items = intervalosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarIntervalosInput input)
        {
            try
            {
                var result = await Listar(input);
                var intervalos = result.Items;
                return _listarIntervalosExcelExporter.ExportToFile(intervalos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<CriarOuEditarIntervalo> Obter(long id)
        {
            try
            {
                var result = await _intervaloRepository
                    .GetAsync(id);

                var intervalo = result
                    //.FirstOrDefault()
                    .MapTo<CriarOuEditarIntervalo>();

                return intervalo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
