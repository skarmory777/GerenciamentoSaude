using Abp.AutoMapper;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class EstoquePreMovimentoKitEstoqueItemDto
    {
        public long PreMovimentoId { get; set; }
        public long KitEstoqueId { get; set; }
        public decimal Quantidade { get; set; }
        public long EstoqueId { get; set; }        
    }
}
