using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes
{
    [AutoMapFrom(typeof(PacienteDto))]
    public class CriarOuEditarPacienteModalViewModel : PacienteDto//CriarOuEditarPaciente
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public bool IsAtendimento { get; set; }
        public long? AbaAtendimentoId { get; set; }
        //public SelectList Profissoes { get; set; }

        //public SelectList Naturalidades { get; set; }

        //public SelectList Origens { get; set; }

        //public SelectList Planos { get; set; }

        //public SelectList Estados { get; set; }

        //public SelectList Cidades { get; set; }

        //public SelectList Paises { get; set; }

        //public SelectList Convenios { get; set; }

        //public SelectList Sexos { get; set; }

        //public SelectList Religioes { get; set; }

        //public SelectList CoresPele { get; set; }

        //public SelectList Escolaridades { get; set; }

        //public SelectList EstadosCivis { get; set; }

        //public SelectList TiposTelefone { get; set; }

        //public SelectList TiposLogradouro { get; set; }

        //public SelectList Nacionalidades { get; set; }

        //public SelectList TiposSanguineos { get; set; }

        //teste pablo 15/03/18
        public long PacienteId { get; set; }

        public ICollection<PacientePesoDto> PacientePesos { get; set; }
        //fim teste
        public CriarOuEditarPacienteModalViewModel(PacienteDto output)
        {
            output.MapTo(this);
        }
    }
}