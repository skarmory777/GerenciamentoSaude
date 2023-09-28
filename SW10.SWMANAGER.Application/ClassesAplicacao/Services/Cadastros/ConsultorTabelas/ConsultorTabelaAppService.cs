using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas
{
    public class ConsultorTabelaAppService : SWMANAGERAppServiceBase, IConsultorTabelaAppService
    {
        private readonly IRepository<ConsultorTabela, long> _consultorTabelaRepository;
        private readonly IListarConsultorTabelaExcelExporter _listarConsultorTabelaExcelExporter;

        public ConsultorTabelaAppService(IRepository<ConsultorTabela, long> consultorTabelaRepository,
            IListarConsultorTabelaExcelExporter listarConsultorTabelaExcelExporter)
        {
            _consultorTabelaRepository = consultorTabelaRepository;
            _listarConsultorTabelaExcelExporter = listarConsultorTabelaExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Create, AppPermissions.Pages_Tenant_Manutencao_Consultor_Tabela_Edit)]
        public async Task CriarOuEditar(CriarOuEditarConsultorTabela input)
        {
            try
            {
                var consultorTabela = input.MapTo<ConsultorTabela>();
                if (input.Id.Equals(0))
                {
                    await _consultorTabelaRepository.InsertOrUpdateAsync(consultorTabela);
                }
                else
                {
                    await _consultorTabelaRepository.UpdateAsync(consultorTabela);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarConsultorTabela input)
        {
            try
            {
                await _consultorTabelaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ConsultorTabelaDto>> Listar(ListarConsultorTabelasInput input)
        {
            var contarConsultorTabelas = 0;
            List<ConsultorTabela> consultorTabelas;
            List<ConsultorTabelaDto> consultorTabelasDtos = new List<ConsultorTabelaDto>();

            try
            {
                var query = _consultorTabelaRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    )
                    .WhereIf(!input.CampoId.Equals(0), m =>
                        m.ConsultorTabelaCampos.ToList().Any(v => v.Id == input.CampoId)
                    );

                contarConsultorTabelas = await query
                    .CountAsync();

                consultorTabelas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                //    .PageBy(input)
                    .ToListAsync();

                consultorTabelasDtos = consultorTabelas
                    .MapTo<List<ConsultorTabelaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ConsultorTabelaDto>(
                contarConsultorTabelas,
                consultorTabelasDtos
                );
        }

        public async Task<PagedResultDto<ConsultorTabelaDto>> ListarTodos()
        {
            var contarConsultorTabelas = 0;
            List<ConsultorTabela> consultorTabelas;
            List<ConsultorTabelaDto> consultorTabelasDtos = new List<ConsultorTabelaDto>();

            try
            {
                var query = _consultorTabelaRepository
                    .GetAll();

                contarConsultorTabelas = await query
                    .CountAsync();

                consultorTabelas = await query
                    .ToListAsync();

                consultorTabelasDtos = consultorTabelas
                    .MapTo<List<ConsultorTabelaDto>>();

            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }
            return new PagedResultDto<ConsultorTabelaDto>(
                contarConsultorTabelas,
                consultorTabelasDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarConsultorTabelasInput input)
        {
            try
            {
                var query = await Listar(input);

                var tiposConsultorTabelaDtos = query.Items;

                return _listarConsultorTabelaExcelExporter.ExportToFile(tiposConsultorTabelaDtos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<CriarOuEditarConsultorTabela> Obter(long id)
        {
            try
            {
                var result = await _consultorTabelaRepository.GetAsync(id);
                var consultorTabela = result.MapTo<CriarOuEditarConsultorTabela>();
                return consultorTabela;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
