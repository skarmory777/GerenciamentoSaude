
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Exporting;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos
{
    public class LaudoModeloLaudoAppService : SWMANAGERAppServiceBase, IModeloLaudoAppService
    {
        private readonly IRepository<LaudoModeloLaudo, long> _modeloLaudoRepository;
        private readonly IListarModelosLaudosExcelExporter _listarModelosLaudosExcelExporter;

        public LaudoModeloLaudoAppService(IRepository<LaudoModeloLaudo, long> modeloLaudoRepository, IListarModelosLaudosExcelExporter listarModelosLaudosExcelExporter)
        {
            _modeloLaudoRepository = modeloLaudoRepository;
            _listarModelosLaudosExcelExporter = listarModelosLaudosExcelExporter;
        }

        public async Task CriarOuEditar(ModeloLaudoDto input)
        {
            try
            {
                var modeloLaudo = input.MapTo<LaudoModeloLaudo>();

                if (modeloLaudo.LaudoGrupoId == 0)
                    modeloLaudo.LaudoGrupoId = null;

                if (input.Id.Equals(0))
                {
                    await _modeloLaudoRepository.InsertAsync(modeloLaudo);
                }
                else
                {
                    await _modeloLaudoRepository.UpdateAsync(modeloLaudo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(ModeloLaudoDto input)
        {
            try
            {
                await _modeloLaudoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ModeloLaudoDto>> Listar(ListarInput input)
        {
            try
            {
                var contarLaudos = 0;
                List<LaudoModeloLaudo> modelosLaudos = new List<LaudoModeloLaudo>();
                List<ModeloLaudoDto> modelosatestadosDto = new List<ModeloLaudoDto>();

                var query = _modeloLaudoRepository
                    .GetAll()
                    .Include(lg => lg.LaudoGrupo)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Modelo.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarLaudos = await query
                    .CountAsync();

                modelosLaudos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                modelosatestadosDto = modelosLaudos.MapTo<List<ModeloLaudoDto>>();

                return new PagedResultDto<ModeloLaudoDto>(
                    contarLaudos,
                    modelosatestadosDto
                    );
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
        //        var modelosLaudos = result.Items;
        //        return _listarModelosLaudosExcelExporter.ExportToFile(modelosLaudos.ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroExportar"));
        //    }

        //}

        public async Task<ModeloLaudoDto> Obter(long id)
        {
            var query = _modeloLaudoRepository
                .GetAllIncluding(
                g => g.LaudoGrupo
                )
                .FirstOrDefault(f => f.Id == id)
                ;

            var modeloLaudo = query.MapTo<ModeloLaudoDto>();

            return modeloLaudo;
        }

        public async Task<ListResultDto<ModeloLaudoDto>> ListarTodos()
        {
            try
            {
                var query = await _modeloLaudoRepository
                    .GetAllListAsync();

                var modeloLaudosDto = query.MapTo<List<ModeloLaudoDto>>();

                return new ListResultDto<ModeloLaudoDto> { Items = modeloLaudosDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<ModeloLaudoDto> faturamentoItensDto = new List<ModeloLaudoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _modeloLaudoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )

                            orderby p.Descricao ascending
                            select new DropdownItems<long>
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList<long>() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<IResultDropdownList<long>> ListarDropdownPorExame(DropdownInput dropdownInput)
        {
            long faturamentoItemId;
            long.TryParse(dropdownInput.filtros[0], out faturamentoItemId);

            return await base.ListarDropdownLambda(dropdownInput
                                          , _modeloLaudoRepository
                                          , w => ((LaudoModeloLaudo)w).LaudoGrupo.Exames.Any(a => a.Id == faturamentoItemId)
                                           , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                           , o => o.Descricao);
        }
    }
}
