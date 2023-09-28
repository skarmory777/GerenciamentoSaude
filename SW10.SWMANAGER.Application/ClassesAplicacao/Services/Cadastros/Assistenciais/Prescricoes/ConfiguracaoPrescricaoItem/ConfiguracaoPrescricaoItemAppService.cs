using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem.Dto;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem
{
    public class ConfiguracaoPrescricaoItemAppService : SWMANAGERAppServiceBase, IConfiguracaoPrescricaoItemAppService
    {
        public async Task<IList<ConfiguracaoPrescricaoItemDto>> CriarOuEditar(IList<ConfiguracaoPrescricaoItemDto> input)
        {
            using (var configuracaoPrescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                var entities = ConfiguracaoPrescricaoItemDto.MapearLista(input);
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var entity in entities.Where(x=> x.ConfiguracaoPrescricaoItemCampoId != 0))
                    {
                        entity.Id = await configuracaoPrescricaoItemRepository.Object.InsertOrUpdateAndGetIdAsync(entity).ConfigureAwait(false);
                    }

                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }

                return ConfiguracaoPrescricaoItemDto.MapearLista(entities);
            }
        }

        public async Task<IList<ConfiguracaoPrescricaoItemDto>> ObterPorDivisao(long id)
        {
            using (var configuracaoPrescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem, long>>())
            {
                return ConfiguracaoPrescricaoItemDto.MapearLista(await configuracaoPrescricaoItemRepository.Object.GetAll().Where(x => x.DivisaoId.HasValue && x.DivisaoId.Value == id).ToListAsync().ConfigureAwait(false));
            }
        }

        public async Task<IList<ConfiguracaoPrescricaoItemDto>> ObterPorPrescricaoItem(long id,  long? prescricaoItemId = null)
        {
            if (id == 0 && prescricaoItemId.HasValue)
            {
                var result = await ObterPorPrescricaoItemData(prescricaoItemId.Value).ConfigureAwait(false);

                foreach (var item in result)
                {
                    item.Id = 0;
                }

                return result;
            }

            return await ObterPorPrescricaoItemData(id).ConfigureAwait(false);

        }

        private async Task<IList<ConfiguracaoPrescricaoItemDto>> ObterPorPrescricaoItemData(long id)
        {
            using (var prescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.PrescricaoItem, long>>())
            using (var configuracaoPrescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem, long>>())
            {
                var prescricao = await prescricaoItemRepository.Object.FirstOrDefaultAsync(id).ConfigureAwait(false);
                var presricaoItems = ConfiguracaoPrescricaoItemDto.MapearLista(await configuracaoPrescricaoItemRepository.Object.GetAll().Where(x => x.PrescricaoItemId.HasValue && x.PrescricaoItemId.Value == id).ToListAsync().ConfigureAwait(false));

                if(presricaoItems.IsNullOrEmpty())
                {
                    presricaoItems = new List<ConfiguracaoPrescricaoItemDto>();
                }
                if (prescricao != null && prescricao.DivisaoId.HasValue)
                {
                    var divisaoItems = await this.ObterPorDivisao(prescricao.DivisaoId.Value).ConfigureAwait(false);
                    var tipos = typeof(ConfiguracaoPrescricaoItemCampo).GetFields(BindingFlags.Static | BindingFlags.Public).ToList().Select(x => (long)x.GetValue(null)).OrderBy(x=> x);
                    if (!divisaoItems.IsNullOrEmpty())
                    {
                        foreach (var tipoId in tipos)
                        {
                            var prescricaoItemTipo = presricaoItems.FirstOrDefault(x => x.ConfiguracaoPrescricaoItemCampoId == tipoId);
                            var divisaoTipo = divisaoItems.FirstOrDefault(x => x.ConfiguracaoPrescricaoItemCampoId == tipoId);

                            if (prescricaoItemTipo == null && divisaoTipo != null)
                            {
                                divisaoTipo.Id = 0;
                                divisaoTipo.DivisaoId = 0;
                                divisaoTipo.PrescricaoItemId = id;
                                presricaoItems.Add(divisaoTipo);
                                continue;
                            }
                        }
                    }
                    
                }

                return presricaoItems;
            }
        }

        public async Task<IDictionary<long, IList<ConfiguracaoPrescricaoItemDto>>> ObterPorPrescricaoItemAgrupado(IEnumerable<long> ids)
        {
            var dicResult = new Dictionary<long, IList<ConfiguracaoPrescricaoItemDto>>();
            foreach (var id in ids)
            {
                dicResult.Add(id, await this.ObterPorPrescricaoItemData(id).ConfigureAwait(false));
            }

            return dicResult;
        }

        public async Task Remover(ConfiguracaoPrescricaoItemDto input)
        {
            using (var configuracaoPrescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.ConfiguracaoPrescricaoItem, long>>())
            {
                var entity = ConfiguracaoPrescricaoItemDto.Mapear(input);
                await configuracaoPrescricaoItemRepository.Object.DeleteAsync(entity).ConfigureAwait(false);
            }
        }
    }
}
