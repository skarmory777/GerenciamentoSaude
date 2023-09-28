using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Intervalos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades
{
    [Table("AteAgendamentoConsultaMedicoDisponibilidade")]
    public class AgendamentoConsultaMedicoDisponibilidade : CamposPadraoCRUD
    {
        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long MedicoId { get; set; }
        public Medico Medico { get; set; }

        [ForeignKey("MedicoEspecialidade"), Column("SisMedicoEspecialidadeId")]
        public long MedicoEspecialidadeId { get; set; }
        public MedicoEspecialidade MedicoEspecialidade { get; set; }

        [ForeignKey("Intervalo"), Column("AteIntervaloId")]
        public long IntervaloId { get; set; }
        public Intervalo Intervalo { get; set; }

        [Index("Ate_Idx_DataInicio")]
        public DateTime DataInicio { get; set; }
        [Index("Ate_Idx_DataFim")]
        public DateTime DataFim { get; set; }

        [Index("Ate_Idx_HoraInicio")]
        public DateTime HoraInicio { get; set; }

        [Index("Ate_Idx_HoraFim")]
        public DateTime HoraFim { get; set; }

        public bool Domingo { get; set; }

        public bool Segunda { get; set; }

        public bool Terca { get; set; }

        public bool Quarta { get; set; }

        public bool Quinta { get; set; }

        public bool Sexta { get; set; }

        public bool Sabado { get; set; }

        public long? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

    }
}
