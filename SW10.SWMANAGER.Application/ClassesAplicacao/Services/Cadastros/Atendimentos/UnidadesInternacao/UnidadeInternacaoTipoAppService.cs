using Abp.Application.Services.Dto;
using Abp.AutoMapper;
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
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao
{
    public class UnidadeInternacaoTipoAppService : SWMANAGERAppServiceBase, IUnidadeInternacaoTipoAppService
    {
        public async Task CriarOuEditar(CriarOuEditarUnidadeInternacaoTipo input)
        {
            using (var _unidadeInternacaoTipoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacaoTipo, long>>())
            {
                try
                {
                    var UnidadeInternacaoTipo = CriarOuEditarUnidadeInternacaoTipo.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _unidadeInternacaoTipoRepository.Object.InsertAsync(UnidadeInternacaoTipo);
                    }
                    else
                    {
                        await _unidadeInternacaoTipoRepository.Object.UpdateAsync(UnidadeInternacaoTipo);
                    }
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroSalvar"), ex);
                }
            }
        }

        public async Task Excluir(CriarOuEditarUnidadeInternacaoTipo input)
        {
            try
            {
                using (var _unidadeInternacaoTipoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacaoTipo, long>>())
                    await _unidadeInternacaoTipoRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<UnidadeInternacaoTipoDto>> Listar(ListarUnidadesInternacaoInput input)
        {
            using (var _unidadeInternacaoTipoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacaoTipo, long>>())
            {
                var contarUnidadesInternacao = 0;
                List<UnidadeInternacaoTipo> motivosAlta;
                List<UnidadeInternacaoTipoDto> motivosAltaDtos = new List<UnidadeInternacaoTipoDto>();
                try
                {
                    var query = _unidadeInternacaoTipoRepository.Object
                        .GetAll()
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(input.Filtro)
                        );

                    contarUnidadesInternacao = await query
                        .CountAsync();

                    motivosAlta = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    motivosAltaDtos = UnidadeInternacaoTipoDto.Mapear(motivosAlta);
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
                return new PagedResultDto<UnidadeInternacaoTipoDto>(
                    contarUnidadesInternacao,
                    motivosAltaDtos
                    );
            }
        }


        public async Task<CriarOuEditarUnidadeInternacaoTipo> Obter(long id)
        {
            using (var _unidadeInternacaoTipoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeInternacaoTipo, long>>())
            {
                try
                {
                    var query = await _unidadeInternacaoTipoRepository.Object
                        .GetAsync(id);

                    var motivoAlta = CriarOuEditarUnidadeInternacaoTipo.Mapear(query);

                    return motivoAlta;
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

    }
}
