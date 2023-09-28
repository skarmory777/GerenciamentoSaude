namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{

    public class AtendimentoDetalhadoDsDto //:EntityDto<long>
    {
        public string CodigoAtendimento { get; set; }
        public string Atendimento { get; set; }
        public string CodPaciente { get; set; }
        public string Paciente { get; set; }
        public string DataAtendimento { get; set; }
        public string Unidade { get; set; }
        public string Convenio { get; set; }
        public string Medico { get; set; }
        public string Empresa { get; set; }
        public string Origem { get; set; }
        public string Especialidade { get; set; }
        public string Plano { get; set; }
        public string TipoAtendimento { get; set; }
        public string Guia { get; set; }
        public string NumeroGuia { get; set; }
        public string DataAlta { get; set; }
        public string DataAltaMedica { get; set; }
        public string Senha { get; set; }
        public string Nascimento { get; set; }
        public string IdadeAno { get; set; }
    }
}
