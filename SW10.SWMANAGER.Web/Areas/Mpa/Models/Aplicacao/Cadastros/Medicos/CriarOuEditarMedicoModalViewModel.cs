using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Medicos
{
    [AutoMap(typeof(MedicoDto))]
    public class CriarOuEditarMedicoModalViewModel : MedicoDto
    {

        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Profissoes { get; set; }

        public SelectList Naturalidades { get; set; }

        public SelectList Estados { get; set; }

        public SelectList Cidades { get; set; }

        public SelectList Paises { get; set; }

        public ICollection<EspecialidadeDto> Especialidades { get; set; }

        public SelectList Sexos { get; set; }

        public SelectList Religioes { get; set; }

        public SelectList CoresPele { get; set; }

        public SelectList Escolaridades { get; set; }

        public SelectList EstadosCivis { get; set; }

        public SelectList TiposTelefone { get; set; }

        public SelectList TiposLogradouro { get; set; }

        public long? EspecialidadeId { get; set; }
        public virtual EspecialidadeDto Especialidade { get; set; }

        public CriarOuEditarMedicoModalViewModel(MedicoDto output)
        {
            output.MapTo(this);
        }
    }
}