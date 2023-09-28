using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas
{
    [Table("FatEntregaLote")]
    public class FaturamentoEntregaLote : CamposPadraoCRUD
    {
        [ForeignKey("Empresa"), Column("SisEmpresaId")]
        public long? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        public string CodEntregaLote { get; set; }
        public string NumeroProcesso { get; set; }
        [Index("Fat_Idx_DataInicial")]
        [DataType(DataType.DateTime)]
        public DateTime? DataInicial { get; set; }
        [Index("Fat_Idx_DataFinal")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFinal { get; set; }
        [Index("Fat_Idx_DataEntrega")]
        [DataType(DataType.DateTime)]
        public DateTime? DataEntrega { get; set; }

        public float ValorFatura { get; set; }
        public float ValorTaxas { get; set; }
        [Index("Fat_Idx_IsAmbulatorio")]
        public bool IsAmbulatorio { get; set; }
        [Index("Fat_Idx_IsInternacao")]
        public bool IsInternacao { get; set; }
        public bool Desativado { get; set; }

        [ForeignKey("UsuarioLote"), Column("FatUsuarioLoteId")]
        public long? UsuarioLoteId { get; set; }
        public User UsuarioLote { get; set; }
        [Index("Fat_Idx_IsLoteGerado")]
        public bool IsLoteGerado { get; set; }

        public List<FaturamentoEntregaConta> Contas { get; set; }
    }

}


