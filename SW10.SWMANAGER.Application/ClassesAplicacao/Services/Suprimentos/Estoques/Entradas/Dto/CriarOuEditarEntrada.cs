﻿using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Entradas;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto
{
    [AutoMap(typeof(Entrada))]
    public class CriarOuEditarEntrada : CamposPadraoCRUDDto
    {
        public long EmpresaId { get; set; }
        public long FornecedorId { get; set; }
        public long TipoDocumentoId { get; set; }
        public long CentroCustoId { get; set; }
        public long CfopId { get; set; }
        public long NumeroDocumento { get; set; }
        public long EstoqueId { get; set; }
        public DateTime Data { get; set; }
        public decimal AcrescimoDesconto { get; set; }
        public decimal Frete { get; set; }
        public decimal ValorDocumento { get; set; }

        public virtual EmpresaDto Empresa { get; set; }
        public virtual CriarOuEditarFornecedor Fornecedor { get; set; }
        //public virtual CriarOuEditarTipoDocumento TipoDocumento { get; set; }
        public virtual CriarOuEditarCentroCusto CentroCusto { get; set; }
        public virtual CriarOuEditarCfop Cfop { get; set; }
        public virtual EstoqueDto Estoque { get; set; }

        public virtual ICollection<CriarOuEditarEntradaItem> EntradaItem { get; set; }
    }
}
