using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Atestados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados
{
    public class ModeloAtestadoAppService : SWMANAGERAppServiceBase, IModeloAtestadoAppService
    {
        public async Task CriarOuEditar(ModeloAtestadoDto input)
        {
            try
            {
                using (var _modeloAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloAtestado, long>>())
                {
                    var modeloAtestado = ModeloAtestadoDto.Mapear(input);
                    if (input.Id.Equals(0))
                        await _modeloAtestadoRepository.Object.InsertAsync(modeloAtestado);
                    else
                        await _modeloAtestadoRepository.Object.UpdateAsync(modeloAtestado);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(ModeloAtestadoDto input)
        {
            try
            {
                using (var _modeloAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloAtestado, long>>())
                {
                    await _modeloAtestadoRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ModeloAtestadoDto>> Listar(ListarInput input)
        {
            try
            {
                using (var _modeloAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloAtestado, long>>())
                {
                    var contarAtestados = 0;
                    List<ModeloAtestado> modelosAtestados = new List<ModeloAtestado>();
                    List<ModeloAtestadoDto> modelosatestadosDto = new List<ModeloAtestadoDto>();

                    var query = _modeloAtestadoRepository.Object
                        .GetAll()
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Conteudo.Contains(input.Filtro)
                        );

                    contarAtestados = await query
                        .CountAsync();

                    modelosAtestados = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    modelosatestadosDto = ModeloAtestadoDto.Mapear(modelosAtestados);

                    return new PagedResultDto<ModeloAtestadoDto>(contarAtestados, modelosatestadosDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                using (var _modeloAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloAtestado, long>>())
                {
                    var query = await _modeloAtestadoRepository.Object
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                       m.Conteudo.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Conteudo.Substring(0, 40) + "..." })
                    .ToListAsync();

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarInput input)
        {
            try
            {
                using (var _listarModelosAtestadosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarModelosAtestadosExcelExporter>())
                {
                    var result = await Listar(input);
                    var modelosAtestados = result.Items;
                    return _listarModelosAtestadosExcelExporter.Object.ExportToFile(modelosAtestados.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ModeloAtestadoDto> Obter(long id)
        {
            using (var _modeloAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloAtestado, long>>())
            {
                var query = await _modeloAtestadoRepository.Object.GetAsync(id);

                var modeloAtestado = ModeloAtestadoDto.Mapear(query);

                return modeloAtestado;
            }
        }

        public async Task<ListResultDto<ModeloAtestadoDto>> ListarTodos()
        {
            try
            {
                using (var _modeloAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ModeloAtestado, long>>())
                {
                    var query = await _modeloAtestadoRepository.Object.GetAllListAsync();

                    var modeloAtestadosDto = ModeloAtestadoDto.Mapear(query);

                    return new ListResultDto<ModeloAtestadoDto> { Items = modeloAtestadosDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
