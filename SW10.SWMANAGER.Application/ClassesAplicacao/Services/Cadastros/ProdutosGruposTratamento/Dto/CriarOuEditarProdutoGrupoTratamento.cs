﻿
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosGruposTratamento;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Dto
{
    [AutoMap(typeof(ProdutoGrupoTratamento))]
    public class CriarOuEditarProdutoGrupoTratamento : CamposPadraoCRUDDto
    {

        public string Descricao { get; set; }

    }
}
