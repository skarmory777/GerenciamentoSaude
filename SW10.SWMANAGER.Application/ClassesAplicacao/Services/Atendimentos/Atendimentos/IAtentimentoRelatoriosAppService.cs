using Abp.Application.Services;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos
{
    public interface IAtentimentoRelatoriosAppService : IApplicationService
    {
        byte[] RetornaArquivoMapaDiaSintatico(long? UnidadeOrganizacionalId, long? StatusId);
    }
}
