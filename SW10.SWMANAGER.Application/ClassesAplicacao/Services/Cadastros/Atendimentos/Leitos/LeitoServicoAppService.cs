using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public class LeitoServicoAppService : SWMANAGERAppServiceBase, ILeitoServicoAppService
    {
        public async Task CriarOuEditar(CriarOuEditarLeitoServico input)
        {
            try
            {
                using (var _leitoServicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoServico, long>>())
                {
                    var LeitoServico = CriarOuEditarLeitoServico.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _leitoServicoRepository.Object.InsertAsync(LeitoServico);
                    }
                    else
                    {
                        await _leitoServicoRepository.Object.UpdateAsync(LeitoServico);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarLeitoServico input)
        {
            try
            {
                using (var _leitoServicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoServico, long>>())
                    await _leitoServicoRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<LeitoServicoDto>> Listar(ListarLeitoServicosInput input)
        {
            var contarLeitoServicos = 0;
            List<LeitoServico> motivosAlta;
            List<LeitoServicoDto> motivosAltaDtos = new List<LeitoServicoDto>();
            try
            {
                using (var _leitoServicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoServico, long>>())
                {
                    var query = _leitoServicoRepository.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                    contarLeitoServicos = await query
                        .CountAsync();

                    motivosAlta = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    motivosAltaDtos = LeitoServicoDto.Mapear(motivosAlta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<LeitoServicoDto>(
                contarLeitoServicos,
                motivosAltaDtos
                );
        }


        public async Task<CriarOuEditarLeitoServico> Obter(long id)
        {
            try
            {
                using (var _leitoServicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoServico, long>>())
                {
                    var query = await _leitoServicoRepository.Object.GetAsync(id);

                    var motivoAlta = CriarOuEditarLeitoServico.Mapear(query);

                    return motivoAlta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

    }
}
