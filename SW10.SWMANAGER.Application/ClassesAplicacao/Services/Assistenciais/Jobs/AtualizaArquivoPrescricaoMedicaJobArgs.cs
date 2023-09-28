using System;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Serializable]
    public class AtualizaArquivoPrescricaoMedicaJobArgs 
    {
        public long PrescricaoMedicaId { get; set; }
        
        public int TenantId { get; set; }
        
        public DateTime? DataAgrupamento { get; set; }
    }
}