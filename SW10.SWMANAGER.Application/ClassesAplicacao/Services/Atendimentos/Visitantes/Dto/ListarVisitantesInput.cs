namespace SW10.SWMANAGER.ClassesAplicacao.Services.Vistantes.Dto
{
    public class ListarVisitantesInput : ListarInput
    {

        public string CurrentPageName { get; set; }

        public long Fornecedor { get; set; }

        public string Nome { get; set; }

        public string Paciente { get; set; }

        public long? PacienteId { get; set; }

        public string Documento { get; set; }

        //public string Nome { get; set; }

        //public string Documento { get; set; }

        //public bool IsAcompanhante { get; set; }

        //public bool IsVisitandte { get; set; }

        //public bool IsMedico { get; set; }

        //public DateTime DataEntrada { get; set; }

        //public DateTime DataSaida { get; set; }

        //public long? UnidadeOrganizacionalId { get; set; }

        //public long? AtendimentoId { get; set; }

        //public long? FornedcedorId { get; set; }
    }
}
