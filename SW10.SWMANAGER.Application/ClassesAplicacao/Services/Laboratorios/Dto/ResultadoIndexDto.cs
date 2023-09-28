using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Dto
{
    public class ResultadoIndexDto : CamposPadraoCRUDDto
    {
        public bool isRn { get; set; }
        public long Nic { get; set; }
        public string Numero { get; set; }
        public DateTime? DataColeta { get; set; }
        public string MedicoSolicitante { get; set; }
        public string Tecnico { get; set; }
        public DateTime? DataTecnico { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string EntreguePor { get; set; }
        public string Paciente { get; set; }
        public string NomeMedicoSolicitante { get; set; }
        public string Cor { get; set; }
        public long? EmpresaId { get; set; }
        public string Empresa { get; set; }
    }
}
