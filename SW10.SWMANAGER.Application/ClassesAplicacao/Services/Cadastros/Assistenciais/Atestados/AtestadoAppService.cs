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
    public class AtestadoAppService : SWMANAGERAppServiceBase, IAtestadoAppService
    {
        public async Task CriarOuEditar(AtestadoDto input)
        {
            try
            {
                using (var _atestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atestado, long>>())
                {
                    var estado = AtestadoDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        await _atestadoRepository.Object.InsertAsync(estado);
                    }
                    else
                    {
                        await _atestadoRepository.Object.UpdateAsync(estado);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(AtestadoDto input)
        {
            try
            {
                using (var _atestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atestado, long>>())
                {
                    await _atestadoRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<AtestadoDto>> Listar(ListarAtestadosInput input)
        {
            try
            {
                using (var _atestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atestado, long>>())
                {

                    var contarAtestados = 0;
                    var atestados = new List<Atestado>();
                    var atestadosDto = new List<AtestadoDto>();

                    var query = _atestadoRepository.Object
                        .GetAll()
                        .Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa)
                        .Include(m => m.ModeloAtestado)
                        .Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa)
                        .Include(m => m.TipoAtestado)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Conteudo.Contains(input.Filtro)
                        );

                    contarAtestados = await query.CountAsync();

                    atestados = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    atestadosDto = AtestadoDto.Mapear(atestados);

                    return new PagedResultDto<AtestadoDto>(contarAtestados, atestadosDto);
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
                using (var _atestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atestado, long>>())
                {

                    var query = await _atestadoRepository.Object
                    .GetAll()
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.ModeloAtestado)
                    .Include(m => m.Paciente)
                    .Include(m => m.Paciente.SisPessoa)
                    .Include(m => m.TipoAtestado)
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

        public async Task<FileDto> ListarParaExcel(ListarAtestadosInput input)
        {
            try
            {
                using (var listarAtestadosExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarAtestadosExcelExporter>())
                {
                    var result = await Listar(input);
                    var atestados = result.Items;
                    return listarAtestadosExcelExporter.Object.ExportToFile(atestados.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<AtestadoDto> Obter(long id)
        {
            using (var _atestadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atestado, long>>())
            {
                var query = await _atestadoRepository.Object
                .GetAll()
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.ModeloAtestado)
                    .Include(m => m.Paciente)
                    .Include(m => m.Paciente.SisPessoa)
                    .Include(m => m.TipoAtestado)
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

                var estado = AtestadoDto.Mapear(query);

                return estado;
            }
        }

    }
}
