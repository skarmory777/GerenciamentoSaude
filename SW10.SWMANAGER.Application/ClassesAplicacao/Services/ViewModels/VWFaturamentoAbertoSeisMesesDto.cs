﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    [AutoMap(typeof(VWFaturamentoAbertoSeisMeses))]
    public class VWFaturamentoAbertoSeisMesesDto : EntityDto<long>
    {
        public string Convenio { get; set; }
        [Column("ValorLancamentoAberto")]
        public decimal LancamentoAberto { get; set; }
        [Column("Mes01")]
        public decimal PrimeiroMes { get; set; }
        [Column("Mes02")]
        public decimal SegundoMes { get; set; }
        [Column("Mes03")]
        public decimal TerceiroMes { get; set; }
        [Column("Mes04")]
        public decimal QuartoMes { get; set; }
        [Column("Mes05")]
        public decimal QuintoMes { get; set; }
        [Column("Mes06")]
        public decimal SextoMes { get; set; }
        [Column("Total")]
        public decimal ValorTotal { get; set; }
        [Column("IDEmpresa")]
        public long? EmpresaId { get; set; }
    }
}
