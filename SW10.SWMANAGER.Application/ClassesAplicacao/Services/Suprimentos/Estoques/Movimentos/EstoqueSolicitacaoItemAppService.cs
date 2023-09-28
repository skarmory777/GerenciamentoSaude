using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstoqueSolicitacaoItemAppService : AbpServiceBase, IEstoqueSolicitacaoItemAppService
    {
        public async Task<EstoquePreMovimentoItemSolicitacaoDto> Obter(long id)
        {
            try
            {
                using var estoqueSolicitacaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>();
                var solicitacaoItem = await estoqueSolicitacaoItemRepository.Object.GetAsync(id);

                var solicitacaoItemDto = EstoquePreMovimentoItemSolicitacaoDto.Mapear(solicitacaoItem);

                return solicitacaoItemDto;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
