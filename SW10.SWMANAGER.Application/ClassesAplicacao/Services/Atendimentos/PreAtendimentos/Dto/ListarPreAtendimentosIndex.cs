using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.PreAtendimentos;
using System;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto
{
    [AutoMap(typeof(PreAtendimento))]
    public class ListarPreAtendimentosIndex : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public string NomeCompleto { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Nascimento { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataRegistro { get; set; }

        public int? Sexo { get; set; }

        public string Telefone { get; set; }

        public string Observacao { get; set; }
    }
}
