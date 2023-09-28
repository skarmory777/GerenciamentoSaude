using Abp.Application.Services.Dto;
using Abp.AutoMapper;
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
    public class LeitoCaracteristicaAppService : SWMANAGERAppServiceBase, ILeitoCaracteristicaAppService
    {
        public async Task CriarOuEditar(CriarOuEditarLeitoCaracteristica input)
        {
            try
            {
                using (var _leitoCaracteristicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoCaracteristica, long>>())
                {
                    var LeitoCaracteristica = CriarOuEditarLeitoCaracteristica.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _leitoCaracteristicaRepository.Object.InsertAsync(LeitoCaracteristica);
                    }
                    else
                    {
                        await _leitoCaracteristicaRepository.Object.UpdateAsync(LeitoCaracteristica);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarLeitoCaracteristica input)
        {
            try
            {
                using (var _leitoCaracteristicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoCaracteristica, long>>())
                    await _leitoCaracteristicaRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PagedResultDto<LeitoCaracteristicaDto>> Listar(ListarLeitoCaracteristicasInput input)
        {
            var contarLeitoCaracteristicas = 0;
            List<LeitoCaracteristica> motivosAlta;
            List<LeitoCaracteristicaDto> motivosAltaDtos = new List<LeitoCaracteristicaDto>();
            try
            {
                using (var _leitoCaracteristicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoCaracteristica, long>>())
                {
                    var query = _leitoCaracteristicaRepository.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input.Filtro)
                    );

                    contarLeitoCaracteristicas = await query
                        .CountAsync();

                    motivosAlta = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    motivosAltaDtos = LeitoCaracteristicaDto.Mapear(motivosAlta);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<LeitoCaracteristicaDto>(
                contarLeitoCaracteristicas,
                motivosAltaDtos
                );
        }

        public async Task<CriarOuEditarLeitoCaracteristica> Obter(long id)
        {
            try
            {
                using (var _leitoCaracteristicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoCaracteristica, long>>())
                {
                    var query = await _leitoCaracteristicaRepository.Object.GetAsync(id);

                    var motivoAlta = CriarOuEditarLeitoCaracteristica.Mapear(query);


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
