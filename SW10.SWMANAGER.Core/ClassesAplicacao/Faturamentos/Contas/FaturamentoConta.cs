using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas
{
    [Table("FatConta")]
    public class FaturamentoConta : CamposPadraoCRUD
    {
        [StringLength(20)]
        public string Matricula { get; set; }

        [StringLength(20)]
        public string CodDependente { get; set; }

        [StringLength(20)]
        public string NumeroGuia { get; set; }

        [StringLength(100)]
        public string Titular { get; set; }

        public int OrigemTitular { get; set; }

        [ForeignKey("Paciente"), Column("SisPacienteId")]
        public long? PacienteId { get; set; }
        public Paciente Paciente { get; set; }

        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }
        public Medico Medico { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Plano"), Column("PlanoId")]
        public long? PlanoId { get; set; }
        public Plano Plano { get; set; }

        // Modelo antigo
        [ForeignKey("Guia"), Column("FatGuiaId")]
        public long? GuiaId { get; set; }
        public Guia Guia { get; set; }
        // Novo modelo
        [ForeignKey("FatGuia"), Column("Fat_Guia_Id")]
        public long? FatGuiaId { get; set; }
        public FaturamentoGuia FatGuia { get; set; }

        [ForeignKey("Empresa"), Column("SisEmpresaId")]
        public long? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [ForeignKey("Atendimento"), Column("SisAtendimentoId")]
        public long? AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("Status"), Column("FatContaStatusId")]
        public long? StatusId { get; set; }
        public FaturamentoContaStatus Status { get; set; }

        [Index("Fat_Idx_DataInicio")]
        [DataType(DataType.DateTime)]
        public DateTime? DataInicio { get; set; }

        [Index("Fat_Idx_DataFim")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFim { get; set; }

        [Index("Fat_Idx_DataPagamento")]
        [DataType(DataType.DateTime)]
        public DateTime? DataPagamento { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ValidadeCarteira { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataAutorizacao { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie1 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie2 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie3 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie4 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie5 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie6 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie7 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie8 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie9 { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DiaSerie10 { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataEntrFolhaSala { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataEntrDescCir { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataEntrBolAnest { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataEntrCDFilme { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataValidadeSenha { get; set; }

        [StringLength(30)]
        public string GuiaOperadora { get; set; }

        [StringLength(20)]
        public string GuiaPrincipal { get; set; }

        [Index("Fat_Idx_TipoAtendimento")]
        public int TipoAtendimento { get; set; }

        public bool IsAutorizador { get; set; }

        //[ForeignKey("TipoLeito"), Column("TipoLeitoId")]
        //public long? TipoLeitoId { get; set; }
        //public TipoLeito TipoLeito { get; set; }

        [ForeignKey("TipoAcomodacao"), Column("TipoAcomodacaoId")]
        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacao TipoAcomodacao { get; set; }

        public string Observacao { get; set; }

        [StringLength(20)]
        public string SenhaAutorizacao { get; set; }
        [StringLength(20)]
        public string IdentAcompanhante { get; set; }

        // Entrega de contas
        public string MotivoPendencia { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DataConferencia { get; set; }

        [ForeignKey("UsuarioConferencia"), Column("FatUsuarioConferenciaId")]
        public long? UsuarioConferenciaId { get; set; }
        public User UsuarioConferencia { get; set; }
        
        
        [ForeignKey("ContaMedica"), Column("FatContaMedicaId")]
        public long? ContaMedicaId { get; set; }
        
        public FaturamentoConta ContaMedica { get; set; }
        [Index("Fat_Idx_IsAtivo")]
        public bool IsAtivo { get; set; }
        [Index("Fat_Idx_Versao")]
        public long Versao { get; set; }


        //[ForeignKey("Alta"), Column("AltaId")]
        //public long? AltaId { get; set; }
        //public Alta Alta { get; set; }

        public List<FaturamentoContaItem> ContaItens { get; set; }

        public static bool PossuiItensNoPeriodo(FaturamentoConta conta, List<FaturamentoContaItem> itens, DateTime periodoInicio, DateTime periodoFim)
        {
            var itensDaConta = itens.Where(x => x.FaturamentoContaId == conta.Id).FirstOrDefault(y => y.Data >= periodoInicio && y.Data <= periodoFim);
            return itensDaConta != null;
        }

    }

}


