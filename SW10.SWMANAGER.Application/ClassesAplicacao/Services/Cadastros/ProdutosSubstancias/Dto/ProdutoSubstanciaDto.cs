﻿using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosSubstancia;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Dto
{
    [AutoMap(typeof(ProdutoSubstancia))]
    public class ProdutoSubstanciaDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

    }
}