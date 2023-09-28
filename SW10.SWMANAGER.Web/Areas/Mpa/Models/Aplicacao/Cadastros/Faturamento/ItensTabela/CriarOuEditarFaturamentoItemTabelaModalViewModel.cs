using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.ItensTabela
{
    [AutoMapFrom(typeof(FaturamentoItemTabelaDto))]
    public class CriarOuEditarFaturamentoItemTabelaModalViewModel : FaturamentoItemTabelaDto
    {
        public UserEditDto UpdateUser { get; set; }

        public SelectList Tabelas { get; set; }

        public SelectList Itens { get; set; }

        public SelectList SisMoedas { get; set; }


        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarFaturamentoItemTabelaModalViewModel(FaturamentoItemTabelaDto output)
        {
            output.MapTo(this);
        }
    }
}