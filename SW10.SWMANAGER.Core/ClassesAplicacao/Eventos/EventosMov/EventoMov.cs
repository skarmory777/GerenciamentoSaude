using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Eventos.Eventos;
using SW10.SWMANAGER.ClassesAplicacao.Eventos.EventosGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Eventos.EventosMov
{
    [Table("QuaEventoMov")]
    public class EventoMov : CamposPadraoCRUD
    {
        public DateTime Data { get; set; }
        public override string Descricao { get; set; }
        public bool IsConferido { get; set; }
        public bool IsObito { get; set; }
        public bool IsNaoConformidade { get; set; }
        public bool IsSentinela { get; set; }
        public bool IsAdverso { get; set; }
        public DateTime DataInclusao { get; set; }
        public bool IsAlteraClinica { get; set; }
        public byte[] Obs { get; set; }
        public int? GrauSentinela { get; set; }
        public DateTime DataConferido { get; set; }
        public string Medicamento { get; set; }
        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public string Fabricante { get; set; }
        public DateTime DataNotificacao { get; set; }
        public string NomeNotificacao { get; set; }
        public string TipoEvento { get; set; }
        public DateTime PrazoResposta { get; set; }
        public byte[] CausaMaterial { get; set; }
        public byte[] CausaMetodo { get; set; }
        public byte[] CausaComunicacao { get; set; }
        public byte[] CausaEfeito { get; set; }
        public byte[] CausaMaodeObra { get; set; }
        public byte[] CausaMeioAmbiente { get; set; }
        public byte[] CausaMedida { get; set; }
        public byte[] PlanoOque { get; set; }
        public byte[] PlanoComo { get; set; }
        public byte[] PlanoQuem { get; set; }
        public byte[] PlanoQuando { get; set; }
        public byte[] PlanoPorQue { get; set; }
        public byte[] PlanoQuanto { get; set; }
        public byte[] PlanoStatus { get; set; }
        public DateTime PlanoPrazo { get; set; }
        public byte[] AcaoImediata { get; set; }
        public string ClassificaEvento { get; set; }
        public int? StatusEvento { get; set; }
        public byte[] ObsQualidade { get; set; }
        public DateTime DataUsuarioFinaliza { get; set; }
        public long? IDUsuarioEncaminha { get; set; }
        public DateTime DataUsuarioEncaminha { get; set; }
        public DateTime DataUsuarioResponde { get; set; }
        public byte[] ObsGestor { get; set; }
        public byte[] CausaCultura { get; set; }
        public string EmailNotificador { get; set; }



        [ForeignKey("Evento"), Column("QuaEventoId")]
        public long? EventoId { get; set; }
        public Evento Evento { get; set; }


        [ForeignKey("EventoGrupo"), Column("QuaEventoGrupoId")]
        public long? EventoGrupoId { get; set; }
        public EventoGrupo EventoGrupo { get; set; }



        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }


        [ForeignKey("Setor"), Column("LabSetorId")]
        public long? SetorId { get; set; }
        public Setor Setor { get; set; }


        [ForeignKey("Produto"), Column("EstProdutoId")]
        public long? ProdutoId { get; set; }
        public Produto Produto { get; set; }


        [ForeignKey("Paciente"), Column("SisPacienteId")]
        public long? PacienteId { get; set; }
        public Paciente Paciente { get; set; }





        [ForeignKey("CentroCusto"), Column("CentroCustoId")]
        public long? CentroCustoId { get; set; }
        public CentroCusto CentroCusto { get; set; }



        //CONFIRMAR ESSES CAMPOS COM MARCIO

        //public long? DestinoId { get; set; }
        //public long? FuncaoId { get; set; }
        //public long? LoteProdutoId { get; set; }
        //public long? UsuarioConferidoId { get; set; }
        //public long UsuarioInclusaoId { get; set; }
        //public long? UsuarioRespondeId { get; set; }
        //public long? UsuarioRespondeOldId { get; set; }
        //public long? UsuarioFinalizaId { get; set; }
        //public long? UsuarioDestinoId { get; set; }

    }
}
