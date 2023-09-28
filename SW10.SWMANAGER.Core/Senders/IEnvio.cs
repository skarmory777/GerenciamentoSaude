namespace SW10.SWMANAGER.Senders
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface Envio
    /// </summary>
    /// <typeparam name="TType">Class do tipo IEnvioInformacoes </typeparam>
    public interface IEnvio<TType> where TType : IEnvioInformacoes
    {
        /// <summary>
        /// Enviar Async
        /// </summary>
        /// <returns>Task Result</returns>
        Task EnviarAsync();

        /// <summary>
        /// Enviar Async com parametro
        /// </summary>
        /// <param name="envioInformacao"> Envio Informacao</param>
        /// <returns>Task Result</returns>
        Task EnviarAsync(TType envioInformacao);

        /// <summary>
        /// Enviar sync
        /// </summary>
        void Enviar();

        /// <summary>
        /// Enviar com parametro
        /// </summary>
        /// <param name="envioInformacao">Envio Informacao</param>
        void Enviar(TType envioInformacao);
    }
}
