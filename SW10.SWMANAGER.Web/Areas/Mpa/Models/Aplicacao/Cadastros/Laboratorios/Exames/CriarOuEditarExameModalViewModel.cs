using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Exames
{
    [AutoMap(typeof(FaturamentoItemDto))]
    public class CriarOuEditarExameModalViewModel : FaturamentoItemDto
    {
        public UserEditDto UpdateUser { get; set; }
        public string InterpretacaoStr { get; set; }
        public string Extra1Str { get; set; }
        public string Extra2Str { get; set; }

        public bool IsEditMode
        {
            get { return Id > 0; }
        }
        public CriarOuEditarExameModalViewModel(FaturamentoItemDto output)
        {
            output.MapTo(this);
        }
    }
}