using System;
using System.Collections.Generic;
using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    public class LaboratorioPainelDetalhamentoCounters
    {
        public double InicialValor { get; set; }
        
        
        public double EmColetaValor { get; set; }
        
        
        public double ColetadoValor { get; set; }
        
        
        public double InterfaceadoValor { get; set; }
        
        public double EnviadoEquipamentoValor { get; set; }
        
        
        public double DigitadoValor { get; set; }
        
        
        public double ConferidoValor { get; set; }
        
        public double PendenteValor { get; set; }
        
    }

    public class ExamesDetalhamentoViewModel : CamposPadraoCRUDDto
    {
        public string Status { get; set; }
        public string StatusCor { get; set; }

        public string Exame { get; set; }
        public string ExameDescricao { get; set; }
        public string ExameMneumonico { get; set; }

        public string DescricaoMaterial { get; set; }
        public string CodigoMaterial { get; set; }

        public DateTime? DataSolicitacao { get; set; }

        public string MedicoSolicitante { get; set; }
        public string NumeroConselho { get; set; }
        public string CodigoConselho { get; set; }

        public bool ExisteResultadoExame { get; set; }
        
        public bool IsPendencia { get; set; }
        
        public string MotivoPendencia { get; set; }
        public string PendenciaDateTime { get; set; }
        public string UsuarioPendencia { get; set; } 
    }

    public class ExamesDetalhamentoInput : ListarInput
    {
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "FatItem.Descricao desc";
            }
        }
    }
}