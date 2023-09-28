using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos
{
    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto;
    using System.Threading.Tasks;

    public interface IRegistroArquivoAppService : IApplicationService
    {
        RegistroArquivoDto ObterPorId(long id);
        RegistroArquivoDto ObterPorRegistro(long registroId, long registroTabelaId);
        long GravarHTMLFormularioDinamico(RegistroHTML registroHtml);
        Task<ListResultDto<RegistroArquivoAtendimentoIndex>> ListarPorAtendimento(ListarRegistroInput input);
        long GravarImagemFormularioDinamico(RegistroHTML registroHtml);
        Task<IResultDropdownList<long>> ListarRegistroTabelas(DropdownInput dropdownInput);
        Task<ListResultDto<RegistroArquivoAtendimentoIndex>> ListarPorAtendimentoEReceituarioMedico(ListarRegistroInput input);

        // RetornoArquivo VisualizarImagemRegistroArquivo(long id);
    }
}
