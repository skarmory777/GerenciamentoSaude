using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao
{
    public class UnidadeInternacaoAppService : SWMANAGERAppServiceBase, IUnidadeInternacaoAppService
    {
        public async Task CriarOuEditar(CriarOuEditarUnidadeInternacao input)
        {
            using (var _unidadeInternacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacao, long>>())
            {
                try
                {
                    var UnidadeInternacao = CriarOuEditarUnidadeInternacao.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _unidadeInternacaoRepository.Object.InsertAsync(UnidadeInternacao);
                    }
                    else
                    {
                        await _unidadeInternacaoRepository.Object.UpdateAsync(UnidadeInternacao);
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroSalvar"), ex);
                }
            }
        }

        public async Task Excluir(CriarOuEditarUnidadeInternacao input)
        {
            try
            {
                using (var _unidadeInternacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacao, long>>())
                    await _unidadeInternacaoRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<UnidadeInternacaoDto>> Listar(ListarUnidadesInternacaoInput input)
        {
            using (var _unidadeInternacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacao, long>>())
            {
                var contarUnidadesInternacao = 0;
                List<UnidadeInternacao> unidadesInternacao;
                List<UnidadeInternacaoDto> unidadesInternacaoDtos = new List<UnidadeInternacaoDto>();
                try
                {
                    var query = _unidadeInternacaoRepository.Object
                        //.GetAll()
                        .GetAllIncluding(
                            m => m.UnidadeInternacaoTipo
                        )
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(input.Filtro)
                        );

                    contarUnidadesInternacao = await query
                        .CountAsync();

                    unidadesInternacao = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    unidadesInternacaoDtos = UnidadeInternacaoDto.Mapear(unidadesInternacao);
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
                return new PagedResultDto<UnidadeInternacaoDto>(
                    contarUnidadesInternacao,
                    unidadesInternacaoDtos
                    );
            }
        }

        public async Task<CriarOuEditarUnidadeInternacao> Obter(long id)
        {
            using (var _unidadeInternacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacao, long>>())
            {
                try
                {
                    var query = await _unidadeInternacaoRepository.Object
                        .GetAll()
                        .Include(m => m.UnidadeInternacaoTipo)
                        .Where(m => m.Id == id)
                        .FirstOrDefaultAsync();

                    var unidadeInternacao = CriarOuEditarUnidadeInternacao.Mapear(query);

                    return unidadeInternacao;
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

    }
}
