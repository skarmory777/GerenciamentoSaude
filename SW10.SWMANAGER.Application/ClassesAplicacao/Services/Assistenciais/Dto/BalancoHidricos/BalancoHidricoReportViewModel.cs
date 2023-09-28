// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BalancoHidricoReportViewModel.cs" company="">
//   
// </copyright>
// <summary>
//   The balanco hidrico report view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto.BalancoHidricos
{
    /// <summary>
    /// The balanco hidrico report view model.
    /// </summary>
    public class BalancoHidricoReportViewModel : BalancoHidricoViewModel
    {
        public BalancoHidricoReportViewModel()
        {
            
        }

        public BalancoHidricoReportViewModel(BalancoHidricoViewModel viewModel, string url)
        {
            if (viewModel != null)
            {
                BalancoDate = viewModel.BalancoDate;
                Atendimento = viewModel.Atendimento;
                Model = viewModel.Model;
                DateLinhaTransferencia = viewModel.DateLinhaTransferencia;
                LinhaTransferencia = viewModel.LinhaTransferencia;
                BalancoHidrico24 = viewModel.BalancoHidrico24;
                BalancoHidrico24Hrs = viewModel.BalancoHidrico24Hrs;
                BalancoHidricoAnteriorId = viewModel.BalancoHidricoAnteriorId;
                LinhaTransferenciaDiaAtual = viewModel.LinhaTransferenciaDiaAtual;
            }

            Url = url;
        }
        
        public string Url { get; set; }
    }
}