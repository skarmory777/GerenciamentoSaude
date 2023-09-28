using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public class RowConfigAppService : SWMANAGERAppServiceBase, IRowConfigAppService
    {
        private readonly IRepository<RowConfig, long> _formConfigRepository;

        public RowConfigAppService(IRepository<RowConfig, long> formConfigRepository)
        {
            _formConfigRepository = formConfigRepository;
        }

        public async Task CriarOuEditar(RowConfig input)
        {
            try
            {
                //var formConfig = input.MapTo<RowConfig>();
                if (input.Id.Equals(0))
                {
                    await _formConfigRepository.InsertAsync(input);
                }
                else
                {
                    await _formConfigRepository.UpdateAsync(input);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(RowConfig input)
        {
            try
            {
                await _formConfigRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<RowConfig>> Listar(ListarInput input)
        {
            var contarGeradorFormularios = 0;
            List<RowConfig> formsConfig;
            //List<RowConfigDto> formsConfigDtos = new List<RowConfigDto>();
            try
            {
                var query = _formConfigRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        //m.Col1.Label.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        //m.Col2.Label.ToUpper().Contains(input.Filtro.ToUpper()) 
                        m.ColConfigs.Any(c => c.Label.ToUpper().Contains(input.Filtro.ToUpper()))
                    );

                contarGeradorFormularios = await query
                    .CountAsync();

                formsConfig = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                //formsConfigDtos = formsConfig
                //    .MapTo<List<RowConfigDto>>();

                return new PagedResultDto<RowConfig>(
                    contarGeradorFormularios,
                    formsConfig
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<RowConfig>> ListarTodos()
        {
            try
            {
                var query = await _formConfigRepository
                    .GetAllListAsync();

                //var formsConfig = query.MapTo<List<RowConfigDto>>();
                return new ListResultDto<RowConfig> { Items = query };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<RowConfig> Obter(long id)
        {
            try
            {
                var result = await _formConfigRepository
                    //.GetAll()
                    //.Include(x=>x.ColConfigs)
                    .GetAllListAsync(m => m.Id == id);

                var formConfig = result
                    .FirstOrDefault();
                //.MapTo<RowConfigDto>();

                return formConfig;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
