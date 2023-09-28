using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto
{
    public class ListarMedicoIndex
    {
        public long Id { get; set; }

        public string Codigo { get; set; }

        public string NomeCompleto { get; set; }

        public string Identidade { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public DateTime? Nascimento { get; set; }

        public string NumeroConselho { get; set; }
    }
}
