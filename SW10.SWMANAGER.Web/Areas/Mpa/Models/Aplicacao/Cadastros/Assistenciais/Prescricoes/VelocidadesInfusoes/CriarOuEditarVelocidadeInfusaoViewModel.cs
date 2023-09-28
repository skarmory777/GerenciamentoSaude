using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusoes
{
    [AutoMap(typeof(VelocidadeInfusaoDto))]
    public class CriarOuEditarVelocidadeInfusaoViewModel : VelocidadeInfusaoDto
    {

        public List<FormaAplicacaoDto> ListFormaAplicacao { get; set; }
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarVelocidadeInfusaoViewModel(VelocidadeInfusaoDto output)
        {
            output.MapTo(this);
        }
    }
}