using System.Data.Entity;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Parametrizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes
{
    public class ParametrizacoesDomainService : SWMANAGERDomainServiceBase, IParametrizacoesDomainService
    {
        public bool AssistencialHabilitaColetaAutomatica()
        {
            var dto = ObterParametrizacao();
            return dto != null && dto.IsHabilitaAssistencialColetaAutomatica;
        }

        private static ParametrizacoesDto ObterParametrizacao()
        {
            using (var parametrizacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Parametrizacao, long>>())
            {
                return ParametrizacoesDto.Mapear(parametrizacaoRepository.Object.GetAll().AsNoTracking().FirstOrDefault());
            }
        }
    }


    public interface IParametrizacoesDomainService: IDomainService
    {
        bool AssistencialHabilitaColetaAutomatica();
    }
}