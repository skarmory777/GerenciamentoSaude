using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class PrescricaoMedicaIndexDto : CamposPadraoCRUDDto
    {
        public long? PrescricaoStatusId { get; set; }
        public string Status { get; set; }
        public string StatusCor { get; set; }
        public DateTime DataPrescricao { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        
        public bool ImprimeAcrescimosESuspensoes { get; set; }
    }
}
