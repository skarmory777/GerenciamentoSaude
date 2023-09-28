using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos
{
    public class FaturarAtendimentoIndexDto : CamposPadraoCRUDDto
    {
        public string AteStatus { get; set; }
        public string AteCorFundo { get; set; }
        public string AteCorTexto { get; set; }
        public string FatStatus { get; set; }
        public string FatCor { get; set; }
        public string CodigoAtendimento { get; set; }
        public string CodigoPaciente { get; set; }
        public string NomeCompleto { get; set; }
        public string Convenio { get; set; }
        public DateTime? DataRegistro { get; set; }
        public DateTime? DataAlta { get; set; }
        public DateTime? DataInicio { get; set; }

        public long? FatContaAtendimentoId { get; set; }
        public bool IsParcial { get; set; }

        public string DataParcial
        {
            get
            {
                if (!DataInicio.HasValue)
                {
                    return null;
                }

                var subStract = DateTime.Now.Subtract(DataInicio.Value);
                var dataParcial = "";
                if (subStract.TotalDays > 0)
                {
                    dataParcial += $"{subStract.TotalDays.ToString("0.00")} {(subStract.TotalDays == 1 ? " dia" : " dias")}";
                }
                return dataParcial;
            }
        }
    }
}