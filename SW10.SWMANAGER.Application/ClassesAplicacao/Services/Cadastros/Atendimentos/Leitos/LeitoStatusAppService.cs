using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public class LeitoStatusAppService : SWMANAGERAppServiceBase, ILeitoStatusAppService
    {
        public async Task CriarOuEditar(CriarOuEditarLeitoStatus input)
        {
            try
            {
                using (var motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
                {
                    var LeitoStatus = CriarOuEditarLeitoStatus.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await motivoAltaRepository.Object.InsertAsync(LeitoStatus);
                    }
                    else
                    {
                        var leitoStatusEntity = motivoAltaRepository.Object.GetAll()
                                                                     .Where(w => w.Id == input.Id)
                                                                     .FirstOrDefault();

                        if (leitoStatusEntity != null)
                        {
                            leitoStatusEntity.Codigo = input.Codigo;
                            leitoStatusEntity.Descricao = input.Descricao;
                            leitoStatusEntity.Cor = input.Cor;
                            leitoStatusEntity.IsBloqueioAtendimento = input.IsBloqueioAtendimento;

                            await motivoAltaRepository.Object.UpdateAsync(leitoStatusEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarLeitoStatus input)
        {
            try
            {
                using (var motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
                {
                    await motivoAltaRepository.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<LeitoStatusDto>> Listar(ListarLeitosStatusInput input)
        {
            var contarLeitosStatus = 0;
            List<LeitoStatus> motivosAlta;
            List<LeitoStatusDto> motivosAltaDtos = new List<LeitoStatusDto>();
            try
            {
                using (var motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
                {
                    var query = motivoAltaRepository.Object.GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Descricao.Contains(input.Filtro));

                    contarLeitosStatus = await query
                        .CountAsync();

                    motivosAlta = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    motivosAltaDtos = LeitoStatusDto.Mapear(motivosAlta).ToList();

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<LeitoStatusDto>(contarLeitosStatus, motivosAltaDtos);
        }

        public async Task<CriarOuEditarLeitoStatus> Obter(long id)
        {
            try
            {
                using (var motivoAltaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
                {
                    var query = await motivoAltaRepository.Object.GetAsync(id);

                    var motivoAlta = CriarOuEditarLeitoStatus.Mapear(query);

                    return motivoAlta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<IResultDropdownList<long>> ListarDropDownTrasferencia(DropdownInput dropdownInput)
        {

            using (var leitstatusoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
            {
                return await base.ListarDropdownLambda(
                           dropdownInput,
                           leitstatusoRepository.Object,
                           w => w.Id != 2 //Não trazer status de ocupado
                           ,
                           p => new DropdownItems { id = p.Id, text = p.Descricao },
                           o => o.Descricao).ConfigureAwait(false);
            }
        }

    }
}
