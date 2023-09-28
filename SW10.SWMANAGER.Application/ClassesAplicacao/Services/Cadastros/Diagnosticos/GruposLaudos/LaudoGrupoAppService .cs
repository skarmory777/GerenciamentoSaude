
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
    public class LaudoGrupoAppService : SWMANAGERAppServiceBase, ILaudoGrupoAppService
    {
        private readonly IRepository<LaudoGrupo, long> _laudoGrupoRepository;
        private readonly IListarModelosLaudosExcelExporter _listarModelosLaudosExcelExporter;

        public LaudoGrupoAppService(IRepository<LaudoGrupo, long> laudoGrupoRepository, IListarModelosLaudosExcelExporter listarModelosLaudosExcelExporter)
        {
            _laudoGrupoRepository = laudoGrupoRepository;
            _listarModelosLaudosExcelExporter = listarModelosLaudosExcelExporter;
        }

        public async Task CriarOuEditar(LaudoGrupoDto input)
        {
            try
            {
                var modeloLaudo = input.MapTo<LaudoGrupo>();

                //if (modeloLaudo.LaudoGrupoId == 0)
                //    modeloLaudo.LaudoGrupoId = null;

                if (input.Id.Equals(0))
                {
                    await _laudoGrupoRepository.InsertAsync(modeloLaudo);
                }
                else
                {
                    await _laudoGrupoRepository.UpdateAsync(modeloLaudo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(LaudoGrupoDto input)
        {
            try
            {
                await _laudoGrupoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<LaudoGrupoDto>> Listar(ListarInput input)
        {
            try
            {
                var contarLaudos = 0;
                List<LaudoGrupo> modelosLaudos = new List<LaudoGrupo>();
                List<LaudoGrupoDto> modelosatestadosDto = new List<LaudoGrupoDto>();

                var query = _laudoGrupoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarLaudos = await query
                    .CountAsync();

                modelosLaudos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                modelosatestadosDto = modelosLaudos.MapTo<List<LaudoGrupoDto>>();

                return new PagedResultDto<LaudoGrupoDto>(
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

        public async Task<LaudoGrupoDto> Obter(long id)
        {
            var query = _laudoGrupoRepository.GetAll()
                                             .Include(i => i.Modalidade)
                                             .Where(w => w.Id == id)
                                             .FirstOrDefault();


            var modeloLaudo = query.MapTo<LaudoGrupoDto>();

            return modeloLaudo;
        }

        public async Task<ListResultDto<LaudoGrupoDto>> ListarTodos()
        {
            try
            {
                var query = await _laudoGrupoRepository
                    .GetAllListAsync();

                var modeloLaudosDto = query.MapTo<List<LaudoGrupoDto>>();

                return new ListResultDto<LaudoGrupoDto> { Items = modeloLaudosDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<LaudoGrupoDto> faturamentoItensDto = new List<LaudoGrupoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                //   bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _laudoGrupoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                //.Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
