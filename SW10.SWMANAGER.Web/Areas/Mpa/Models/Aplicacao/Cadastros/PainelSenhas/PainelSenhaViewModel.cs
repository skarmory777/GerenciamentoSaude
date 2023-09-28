using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.PainelSenhas
{
    public class PainelSenhaViewModel : PainelDto
    {
        public string Filtro { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public PainelSenhaViewModel(PainelDto painelDto)
        {
            this.Id = painelDto.Id;
            this.Codigo = painelDto.Codigo;
            this.Descricao = painelDto.Descricao;
        }


    }
}