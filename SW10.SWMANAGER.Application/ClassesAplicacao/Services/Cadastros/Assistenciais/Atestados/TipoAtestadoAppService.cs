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
    public class TipoAtestadoAppService : SWMANAGERAppServiceBase, ITipoAtestadoAppService
    {
        public async Task CriarOuEditar(TipoAtestadoDto input)
        {
            try
            {
                using (var _tipoAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtestado, long>>())
                {
                    var tipoAtestado = TipoAtestadoDto.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _tipoAtestadoRepository.Object.InsertAsync(tipoAtestado);
                    }
                    else
                    {
                        await _tipoAtestadoRepository.Object.UpdateAsync(tipoAtestado);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(TipoAtestadoDto input)
        {
            try
            {
                using (var _tipoAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtestado, long>>())
                {
                    await _tipoAtestadoRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<TipoAtestadoDto>> Listar(ListarInput input)
        {
            try
            {
                using (var _tipoAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtestado, long>>())
                {
                    var contarAtestados = 0;
                    List<TipoAtestado> tipoAtestados = new List<TipoAtestado>();
                    List<TipoAtestadoDto> tipoAtestadosDto = new List<TipoAtestadoDto>();

                    var query = _tipoAtestadoRepository.Object
                        .GetAll()
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(input.Filtro)
                        );

                    contarAtestados = await query
                        .CountAsync();

                    tipoAtestados = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    tipoAtestadosDto = TipoAtestadoDto.Mapear(tipoAtestados);

                    return new PagedResultDto<TipoAtestadoDto>(contarAtestados, tipoAtestadosDto);
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
                using (var _tipoAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtestado, long>>())
                {
                    var query = await _tipoAtestadoRepository.Object
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                       m.Descricao.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
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
                using (var _listarAtestadosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarTiposAtestadosExcelExporter>())
                {
                    var result = await Listar(input);
                    var tiposAtestados = result.Items;
                    return _listarAtestadosExcelExporter.Object.ExportToFile(tiposAtestados.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<ListResultDto<TipoAtestadoDto>> ListarTodos()
        {
            try
            {
                using (var _tipoAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtestado, long>>())
                {
                    var query = await _tipoAtestadoRepository.Object.GetAllListAsync();

                    var tipoAtestadosDto = TipoAtestadoDto.Mapear(query);

                    return new ListResultDto<TipoAtestadoDto> { Items = tipoAtestadosDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<TipoAtestadoDto> Obter(long id)
        {
            using (var _tipoAtestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAtestado, long>>())
            {
                var query = await _tipoAtestadoRepository.Object.GetAsync(id);

                var tipoAtestado = TipoAtestadoDto.Mapear(query);

                return tipoAtestado;
            }
        }

    }
}