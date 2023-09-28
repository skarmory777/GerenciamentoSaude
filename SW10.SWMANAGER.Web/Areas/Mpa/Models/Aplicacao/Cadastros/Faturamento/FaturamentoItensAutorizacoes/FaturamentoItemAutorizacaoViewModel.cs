using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.FaturamentoItensAutorizacoes
{
    [AutoMap(typeof(FaturamentoItemAutorizacaoDto))]
    public class FaturamentoItemAutorizacaoViewModel : FaturamentoItemAutorizacaoDto
    {
        public FaturamentoItemAutorizacaoViewModel()
        { }


        public FaturamentoItemAutorizacaoViewModel(FaturamentoItemAutorizacaoDto output)
        {
            output.MapTo(this);
        }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public string Filtro { get; set; }
    }
}