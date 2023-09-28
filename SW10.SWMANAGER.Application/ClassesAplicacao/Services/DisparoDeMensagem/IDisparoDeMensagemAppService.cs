namespace SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem
{
    using Abp.Application.Services;
    using Abp.Application.Services.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto;
    using System.Threading.Tasks;

    /// <summary>
    /// Disparo De Mensagem
    /// </summary>
    public interface IDisparoDeMensagemAppService : IApplicationService
    {
        /// <summary>
        /// Listar Filtros
        /// </summary>
        /// <param name="input">input disparoDeMensagem</param> 
        /// <returns> retorno index</returns>
        Task<PagedResultDto<DisparoDeMensagemDto>> Listar(IndexFiltroDisparoDeMensagemViewModel input);

        DisparoDeMensagemDto Obter(long id);

        void Excluir(long id);

        /// <summary>
        /// Criar ou Editar disparo
        /// </summary>
        /// <param name="input">input disparoDeMensagem</param>
        /// <returns>retorno disparo de mensagem</returns>
        Task<DisparoDeMensagemDto> CriarOuEditar(DisparoDeMensagemDto input);


        Task ExcluirItem(long disparoDeMensagemId);

        /// <summary>
        /// Adicionar Item disparo
        /// </summary>
        /// <param name="input">input disparoDeMensagem</param>
        /// <returns>retorno disparo de mensagem</returns>
        Task<DisparoDeMensagemDto> AdicionarItem(DisparoDeMensagemItemDto input);

        /// <summary>
        /// Listar Filtros par disparo
        /// </summary>
        /// <param name="filterViewModel">filtro view Model</param>
        /// <returns>retorno index</returns>
        Task<PagedResultDto<IndexDisparoDeMensagemViewModel>> ListarParaDisparo(FiltroDisparoDeMensagemViewModel filterViewModel);
    }
}
