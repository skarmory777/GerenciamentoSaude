// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumTipoModelo.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the EnumTipoModelo type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.ModeloTexto
{
    /// <summary>
    /// The enum tipo modelo.
    /// </summary>
    public enum EnumTipoModelo : long
    {
        /// <summary>
        /// The ficha paciente.
        /// </summary>
        FichaPaciente = 1,

        /// <summary>
        /// The etiqueta paciente.
        /// </summary>
        EtiquetaPaciente = 2,

        /// <summary>
        /// The etiqueta visitante.
        /// </summary>
        EtiquetaVisitante = 3,

        /// <summary>
        /// The pulseira.
        /// </summary>
        Pulseira = 4,

        /// <summary>
        /// The terminal senha.
        /// </summary>
        TerminalSenha = 5
    }
}