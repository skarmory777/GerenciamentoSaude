using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Dashboard.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Dashboards
{

    public class DashboardAppService : SWMANAGERAppServiceBase, IDashboardAppService
    {
        private readonly IRepository<VWFaturamentoAberto, long> _vwFaturamentoAbertoRepository;
        private readonly IRepository<VWConsultaFaturamentoAberto, long> _vwConsultaFaturamentoAbertoRepository;
        private readonly IRepository<VWConsultaFaturamentoEntrega, long> _vwConsultaFaturamentoEntregaRepository;
        private readonly IRepository<VWConsultaFaturamentoRecebimento, long> _vwConsultaFaturamentoRecebimentoRepository;
        private readonly IRepository<VWFaturamentoAbertoSeisMeses, long> _vwFaturamentoAbertoSeisMesesRepository;
        private readonly IRepository<VWEmpresa, long> _vwEmpresaRepository;
        //private readonly ReadOnlyContext _dbContext;
        //private readonly IListarControleProducaosExcelExporter _listarControleProducaosExcelExporter;

        public DashboardAppService(
            IRepository<VWFaturamentoAberto, long> vwFaturamentoAbertoRepository,
            IRepository<VWConsultaFaturamentoAberto, long> vwConsultaFaturamentoAbertoRepository,
            IRepository<VWConsultaFaturamentoEntrega, long> vwConsultaFaturamentoEntregaRepository,
            IRepository<VWConsultaFaturamentoRecebimento, long> vwConsultaFaturamentoRecebimentoRepository,
            IRepository<VWFaturamentoAbertoSeisMeses, long> vwFaturamentoAbertoSeisMesesRepository,
            IRepository<VWEmpresa, long> vwEmpresaRepository
        //    ReadOnlyContext dbContext
        //    IListarControleProducaosExcelExporter listarControleProducaosExcelExporter
            )
        {
            _vwFaturamentoAbertoRepository = vwFaturamentoAbertoRepository;
            _vwConsultaFaturamentoAbertoRepository = vwConsultaFaturamentoAbertoRepository;
            _vwConsultaFaturamentoEntregaRepository = vwConsultaFaturamentoEntregaRepository;
            _vwConsultaFaturamentoRecebimentoRepository = vwConsultaFaturamentoRecebimentoRepository;
            _vwFaturamentoAbertoSeisMesesRepository = vwFaturamentoAbertoSeisMesesRepository;
            _vwEmpresaRepository = vwEmpresaRepository;
            //  _dbContext = dbContext;
        }

        public async Task<ListResultDto<VWConsultaFaturamentoAbertoDto>> ListarFaturamentoAberto()
        {
            List<VWConsultaFaturamentoAberto> faturamentosAberto;
            List<VWConsultaFaturamentoAbertoDto> faturamentosAbertosDtos = new List<VWConsultaFaturamentoAbertoDto>();
            try
            {
                var query = _vwConsultaFaturamentoAbertoRepository
                    .GetAll();

                // var query = _dbContext.VWConsultaFaturamentoAberto;
                faturamentosAberto = await query
                    .AsNoTracking()
                    .ToListAsync();

                faturamentosAbertosDtos = faturamentosAberto
                    .MapTo<List<VWConsultaFaturamentoAbertoDto>>();

                return new ListResultDto<VWConsultaFaturamentoAbertoDto> { Items = faturamentosAbertosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<VWFaturamentoAbertoDto>> ListarFaturamentoAbertoGrid(ListarVWFaturamentoAbertoInput input)
        {
            var contarRegistros = 0;
            List<VWFaturamentoAberto> faturamentosAberto;
            List<VWFaturamentoAbertoDto> faturamentosAbertosDtos = new List<VWFaturamentoAbertoDto>();
            try
            {
                var query = _vwFaturamentoAbertoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.AnoMesVenc.Contains(input.Filtro) ||
                                                                 m.convenio.Contains(input.Filtro) ||
                                                                 m.Empresa.Contains(input.Filtro)
                    );

                contarRegistros = await query
                    .CountAsync();

                // var query = _dbContext.VWConsultaFaturamentoAberto;
                faturamentosAberto = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                faturamentosAbertosDtos = faturamentosAberto
                    .MapTo<List<VWFaturamentoAbertoDto>>();

                return new PagedResultDto<VWFaturamentoAbertoDto>(
                    contarRegistros,
                    faturamentosAbertosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWConsultaFaturamentoEntregaDto>> ListarFaturamentoEntrega()
        {
            List<VWConsultaFaturamentoEntrega> faturamentosEntrega;
            List<VWConsultaFaturamentoEntregaDto> faturamentosEntregasDtos = new List<VWConsultaFaturamentoEntregaDto>();
            try
            {
                //var query = _dbContext.VWConsultaFaturamentoEntrega;
                var query = _vwConsultaFaturamentoEntregaRepository
                .GetAll();

                faturamentosEntrega = await query
                    .AsNoTracking()
                    .ToListAsync();

                faturamentosEntregasDtos = faturamentosEntrega
                    .MapTo<List<VWConsultaFaturamentoEntregaDto>>();

                return new ListResultDto<VWConsultaFaturamentoEntregaDto> { Items = faturamentosEntregasDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWConsultaFaturamentoRecebimentoDto>> ListarFaturamentoRecebimento()
        {
            List<VWConsultaFaturamentoRecebimento> faturamentosRecebimento;
            List<VWConsultaFaturamentoRecebimentoDto> faturamentosRecebimentosDtos = new List<VWConsultaFaturamentoRecebimentoDto>();
            try
            {
                //var query = _dbContext.VWConsultaFaturamentoRecebimento; // 
                var query = _vwConsultaFaturamentoRecebimentoRepository
                    .GetAll();

                faturamentosRecebimento = await query
                    .AsNoTracking()
                    .ToListAsync();

                faturamentosRecebimentosDtos = faturamentosRecebimento
                    .MapTo<List<VWConsultaFaturamentoRecebimentoDto>>();

                return new ListResultDto<VWConsultaFaturamentoRecebimentoDto> { Items = faturamentosRecebimentosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<VWFaturamentoAbertoSeisMesesDto>> ListarFaturamentoAbertoSeisMeses(ListarFaturamentoAbertoSeisMesesInput input)
        {
            var contarRegistros = 0;
            List<VWFaturamentoAbertoSeisMeses> faturamentosAbertosSesisMeses;
            List<VWFaturamentoAbertoSeisMesesDto> faturamentosAbertosSeisMesesDtos = new List<VWFaturamentoAbertoSeisMesesDto>();
            try
            {
                var query = _vwFaturamentoAbertoSeisMesesRepository
                    .GetAll()
                    .WhereIf(input.EmpresaId.HasValue, m =>
                         m.EmpresaId == input.EmpresaId.Value
                    )
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Convenio.Contains(input.Filtro) ||
                        m.LancamentoAberto.ToString().Contains(input.Filtro) ||
                        m.PrimeiroMes.ToString().Contains(input.Filtro) ||
                        m.SegundoMes.ToString().Contains(input.Filtro) ||
                        m.TerceiroMes.ToString().Contains(input.Filtro) ||
                        m.QuartoMes.ToString().Contains(input.Filtro) ||
                        m.QuintoMes.ToString().Contains(input.Filtro) ||
                        m.SextoMes.ToString().Contains(input.Filtro) ||
                        m.ValorTotal.ToString().Contains(input.Filtro)
                    );

                contarRegistros = await query
                    .CountAsync();

                // var query = _dbContext.VWConsultaFaturamentoAberto;
                faturamentosAbertosSesisMeses = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                faturamentosAbertosSeisMesesDtos = faturamentosAbertosSesisMeses
                    .MapTo<List<VWFaturamentoAbertoSeisMesesDto>>();

                return new PagedResultDto<VWFaturamentoAbertoSeisMesesDto>(
                    contarRegistros,
                    faturamentosAbertosSeisMesesDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<VWEmpresaDto>> ListarEmpresas(ListarInput input)
        {
            var contar = 0;
            List<VWEmpresa> list;
            List<VWEmpresaDto> listDto = new List<VWEmpresaDto>();
            try
            {
                var query = _vwEmpresaRepository
                    .GetAll()
                    .WhereIf(input.EmpresaId.HasValue, m =>
                        m.Id == input.EmpresaId.Value
                    )
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Nome.Contains(input.Filtro)
                    );

                contar = await query
                    .CountAsync();

                list = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                listDto = list
                    .MapTo<List<VWEmpresaDto>>();

                return new PagedResultDto<VWEmpresaDto>(
                    contar,
                    listDto
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWEmpresaDto>> ListarTodasEmpresas()
        {
            List<VWEmpresa> list;
            List<VWEmpresaDto> listDto = new List<VWEmpresaDto>();
            try
            {
                var query = _vwEmpresaRepository
                    .GetAll();

                list = await query
                    .AsNoTracking()
                    .ToListAsync();

                listDto = list
                    .MapTo<List<VWEmpresaDto>>();

                return new ListResultDto<VWEmpresaDto> { Items = listDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
