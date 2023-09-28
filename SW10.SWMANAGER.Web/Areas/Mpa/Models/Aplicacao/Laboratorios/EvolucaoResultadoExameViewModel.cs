using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Laboratorios
{
    [AutoMap(typeof(ResultadoLaudoDto))]
    public class EvolucaoResultadoExameViewModel : ResultadoLaudoDto
    {
        public EvolucaoResultadoExameViewModel(ResultadoLaudoDto output)
        {
            output.MapTo(this);
        }

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public string Filtro { get; set; }
    }
}