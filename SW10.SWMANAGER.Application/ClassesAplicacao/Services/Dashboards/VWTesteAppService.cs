using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;

using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Dashboards
{
    public class VWTesteAppService : SWMANAGERAppServiceBase, IVWTesteAppService
    {
        private readonly IRepository<VWTeste, long> _vwTesteRepository;
        //private readonly IListarControleProducaosExcelExporter _listarControleProducaosExcelExporter;

        public VWTesteAppService(
            IRepository<VWTeste, long> vwTesteRepository
        //    IListarControleProducaosExcelExporter listarControleProducaosExcelExporter
            )
        {
            _vwTesteRepository = vwTesteRepository;
            //    _listarControleProducaosExcelExporter = listarControleProducaosExcelExporter;
        }



        public async Task<PagedResultDto<VWTesteDto>> Listar(ListarVWTesteInput input)
        {
            var contarControleProducaos = 0;
            List<VWTeste> pacientes;
            List<VWTesteDto> pacientesDtos = new List<VWTesteDto>();
            try
            {
                var query = _vwTesteRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                    m.NomePaciente.ToUpper().Contains(input.Filtro.ToUpper()) ||
                    m.NomeCidade.ToUpper().Contains(input.Filtro.ToUpper()) ||
                    m.NomeEstado.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarControleProducaos = await query
                    .CountAsync();

                pacientes = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                pacientesDtos = pacientes
                    .MapTo<List<VWTesteDto>>();

                return new PagedResultDto<VWTesteDto>(
                contarControleProducaos,
                pacientesDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<VWTesteDto> Obter(long id)
        {
            try
            {
                var query = await _vwTesteRepository
                    .GetAsync(id);

                var paciente = query.MapTo<VWTesteDto>();

                return paciente;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
