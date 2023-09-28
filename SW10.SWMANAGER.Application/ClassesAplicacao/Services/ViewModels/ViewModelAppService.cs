using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public class ViewModelAppService : SWMANAGERAppServiceBase, IViewModelAppService
    {
        private readonly IRepository<VWFaturamentoAbertoSeisMeses, long> _vwFaturamentoAbertoSeisMesesRepositorio;
        private readonly IRepository<VWEmpresa, long> _vwEmpresaRepositorio;


        public ViewModelAppService(
            IRepository<VWFaturamentoAbertoSeisMeses, long> vwFaturamentoAbertoSeisMesesRepositorio,
            IRepository<VWEmpresa, long> vwEmpresaRepositorio
            )
        {
            _vwFaturamentoAbertoSeisMesesRepositorio = vwFaturamentoAbertoSeisMesesRepositorio;
            _vwEmpresaRepositorio = vwEmpresaRepositorio;
        }

        public async Task<PagedResultDto<VWEmpresaDto>> ListarEmpresas(ListarInput input)
        {
            var contar = 0;
            List<VWEmpresa> list;
            List<VWEmpresaDto> listDto = new List<VWEmpresaDto>();
            try
            {
                var query = _vwEmpresaRepositorio
                    .GetAll()
                    .WhereIf(input.EmpresaId.HasValue, m =>
                        m.Id == input.EmpresaId.Value
                    )
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Nome.ToUpper().Contains(input.Filtro.ToUpper())
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

        public async Task<PagedResultDto<VWFaturamentoAbertoSeisMesesDto>> ListarFaturamentoAbertoSeisMeses(ListarInput input)
        {
            var contar = 0;
            List<VWFaturamentoAbertoSeisMeses> list;
            List<VWFaturamentoAbertoSeisMesesDto> listDto = new List<VWFaturamentoAbertoSeisMesesDto>();
            try
            {
                var query = _vwFaturamentoAbertoSeisMesesRepositorio
                    .GetAll()
                    .WhereIf(input.EmpresaId.HasValue, m =>
                        m.EmpresaId == input.EmpresaId.Value
                    )
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Convenio.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contar = await query
                    .CountAsync();

                list = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                listDto = list
                    .MapTo<List<VWFaturamentoAbertoSeisMesesDto>>();

                return new PagedResultDto<VWFaturamentoAbertoSeisMesesDto>(
                    contar,
                    listDto
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<VWFaturamentoAbertoSeisMesesDto>> ListarTodosFaturamentoAbertoSeisMeses()
        {
            var query = _vwFaturamentoAbertoSeisMesesRepositorio.GetAll();

            var faturamentos = await query.ToListAsync();

            var faturamentosDto = faturamentos.MapTo<List<VWFaturamentoAbertoSeisMesesDto>>();

            return new ListResultDto<VWFaturamentoAbertoSeisMesesDto> { Items = faturamentosDto };
        }
    }
}
