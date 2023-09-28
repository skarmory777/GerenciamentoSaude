using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Prestadores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssPrescricaoMedica")]
    public class PrescricaoMedica : CamposPadraoCRUD
    {
        [Index("Ate_Idx_DataPrescricao")]
        public DateTime DataPrescricao { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }

        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long? AtendimentoId { get; set; }

        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }

        public Medico Medico { get; set; }

        public string Observacao { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        public Atendimento Atendimento { get; set; }

        [ForeignKey("PrescricaoStatus"), Column("AssPrescricaoStatusId")]
        public long? PrescricaoStatusId { get; set; }
        public PrescricaoStatus PrescricaoStatus { get; set; }

        public long? LiberadoUserId { get; set; }

        [Index("Ass_Idx_DataLiberado")]
        public DateTime? DataLiberado { get; set; }

        public long? SuspensoUserId { get; set; }

        [Index("Ass_Idx_DataSuspenso")]
        public DateTime? DataSuspenso { get; set; }

        [ForeignKey("Leito"), Column("AteLeitoId")]
        public long? LeitoId { get; set; }
        public Leito Leito { get; set; }

    }
}
