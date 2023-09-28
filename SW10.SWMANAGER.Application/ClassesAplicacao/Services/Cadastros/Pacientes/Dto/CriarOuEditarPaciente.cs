using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto
{
    [AutoMap(typeof(Paciente), typeof(PacienteDto))]
    public class CriarOuEditarPaciente : PessoaFisicaDto
    {
        public int CodigoPaciente { get; set; }

        public long? Prontuario { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        //[DataType(DataType.MultilineText)]
        //public string Pendencia { get; set; }

        public string Indicacao { get; set; }

        public bool IsDoador { get; set; }

        public long? Cns { get; set; }

        //public long? OrigemId { get; set; }

        //public long? ConvenioId { get; set; }

        //public long? PlanoId { get; set; }

        // public long TipoSanguineoId { get; set; }

        //public string Matricula { get; set; }

        //[ForeignKey("OrigemId")]
        //public virtual OrigemDto Origem { get; set; }

        //[ForeignKey("ConvenioId")]
        //public virtual ConvenioDto Convenio { get; set; }

        //[ForeignKey("PlanoId")]
        //public virtual PlanoDto Plano { get; set; }

        //[ForeignKey("TipoSanguineoId")]
        //public virtual TipoSanguineoDto TipoSanguineo { get; set; }

        public virtual ICollection<PacientePesoDto> PacientePesos { get; set; }

        //public virtual ICollection<PacienteConvenioDto> PacienteConvenios { get; set; }

        //public virtual ICollection<PacientePlanoDto> PacientePlanos { get; set; }

    }
}