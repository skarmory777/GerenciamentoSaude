using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Parentescos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos
{
    public class ParentescoAppService : SWMANAGERAppServiceBase, IParentescoAppService
    {
        private readonly IRepository<Parentesco, long> _parentescoRepository;
        private readonly IListarParentescosExcelExporter _listarParentescosExcelExporter;

        public ParentescoAppService(IRepository<Parentesco, long> parentescoRepository, IListarParentescosExcelExporter listarParentescosExcelExporter)
        {
            _parentescoRepository = parentescoRepository;
            _listarParentescosExcelExporter = listarParentescosExcelExporter;
        }

        public async Task CriarOuEditar(CriarOuEditarParentesco input)
        {
            try
            {
                var parentesco = input.MapTo<Parentesco>();
                if (input.Id.Equals(0))
                {
                    await _parentescoRepository.InsertAsync(parentesco);
                }
                else
                {
                    await _parentescoRepository.UpdateAsync(parentesco);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarParentesco input)
        {
            try
            {
                await _parentescoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<ParentescoDto>> Listar(ListarParentescosInput input)
        {
            var contarParentescos = 0;
            List<Parentesco> parentesco;
            List<ParentescoDto> parentescosDtos = new List<ParentescoDto>();
            try
            {
                var query = _parentescoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarParentescos = await query
                    .CountAsync();

                parentesco = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                parentescosDtos = parentesco
                    .MapTo<List<ParentescoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<ParentescoDto>(
                contarParentescos,
                parentescosDtos
                );
        }

        public async Task<ListResultDto<ParentescoDto>> ListarTodos()
        {
            List<Parentesco> parentesco;
            List<ParentescoDto> parentescosDtos = new List<ParentescoDto>();
            try
            {
                parentesco = await _parentescoRepository
                  .GetAll()
                  .AsNoTracking()
                  .ToListAsync();

                parentescosDtos = parentesco
                    .MapTo<List<ParentescoDto>>();

                return new ListResultDto<ParentescoDto> { Items = parentescosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<ParentescoDto>> ListarAutoComplete(string input)
        {
            List<Parentesco> parentescos;
            List<ParentescoDto> parentescosDtos = new List<ParentescoDto>();
            try
            {
                var query = _parentescoRepository
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    );

                parentescos = await query
                    .AsNoTracking()
                    .ToListAsync();

                parentescosDtos = parentescos
                    .MapTo<List<ParentescoDto>>();

                return new ListResultDto<ParentescoDto> { Items = parentescosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarParentescosInput input)
        {
            try
            {
                var result = await Listar(input);
                var origens = result.Items;
                return _listarParentescosExcelExporter.ExportToFile(origens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarParentesco> Obter(long id)
        {
            try
            {
                var result = await _parentescoRepository.GetAsync(id);
                var parentesco = result.MapTo<CriarOuEditarParentesco>();
                return parentesco;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }
    }
}
