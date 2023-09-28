using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Cabecalho.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Cabecalho
{
    public interface ICabecalhoAppService : IApplicationService
    {
        CabecalhoDto Obter();

        DefaultReturn<CabecalhoDto> Editar(CabecalhoDto cabecalhoDto);
    }
}
