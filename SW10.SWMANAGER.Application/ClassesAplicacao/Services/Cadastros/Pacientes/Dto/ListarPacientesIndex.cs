using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto
{
    [AutoMap(typeof(Paciente))]
    public class ListarPacientesIndex
    {
        public long Id { get; set; }

        public string NomeCompleto { get; set; }

        public string Identidade { get; set; }

        public string Cpf { get; set; }

        public DateTime? Nascimento { get; set; }

        public string Telefone { get; set; }

        public string NomeMae { get; set; }

        public string NomePai { get; set; }

    }
}
