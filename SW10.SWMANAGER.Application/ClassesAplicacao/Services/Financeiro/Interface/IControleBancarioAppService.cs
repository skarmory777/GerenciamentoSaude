using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using System;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface
{
    public interface IControleBancarioAppService : IApplicationService
    {
        Task<DefaultReturn<QuitacaoDto>> CriarLancamento(QuitacaoDto input);

        Task<DefaultReturn<QuitacaoDto>> CriarTransferencia(TransferenciaDto input);

        Task<DefaultReturn<QuitacaoDto>> ExcluirTransferencias(Guid transferenciaIdentificador);
    }
}
