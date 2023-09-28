using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    [AutoMapFrom(typeof(ProdutoDto))]
    public class CriarOuEditarProdutoModalViewModel : ProdutoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        /// <summary>
        /// 
        /// </summary>
        public SelectList Generos { get; set; }
        public SelectList UnidadesReferenciais { get; set; }
        public SelectList UnidadesGerenciais { get; set; }
        public SelectList Etiquetas { get; set; }
        public SelectList Grupos { get; set; }
        public SelectList Classes { get; set; }
        public SelectList SubClasses { get; set; }
        public SelectList CodigosMedicamentos { get; set; }
        public SelectList Unidades { get; set; }
        public SelectList DCBs { get; set; }

        public SelectList ProdutosPrincipais { get; set; }

        //dados fakes para prototipagem de tela
        public string NumeroNF { get; set; }
        public DateTime DataEmissaoNF { get; set; }
        public DateTime DataEntradaNF { get; set; }
        public float ValorNF { get; set; }
        public int Quantidade { get; set; }
        public string Unidade { get; set; }
        public string OrdemCompra { get; set; }
        public DateTime DataOrdemCompra { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }

        public CriarOuEditarProdutoModalViewModel(ProdutoDto output)
        {
            output.MapTo(this);
        }
    }
}