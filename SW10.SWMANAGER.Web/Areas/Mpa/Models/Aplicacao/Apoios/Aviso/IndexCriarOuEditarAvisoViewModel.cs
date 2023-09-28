using System.Collections.Generic;
using SW10.SWMANAGER.Authorization.Roles.Dto;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Avisos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Aviso
{
    public class IndexCriarOuEditarAvisoViewModel
    {
        public long? Id { get; set; }

        public AvisoDto Aviso { get; set; }
        public IEnumerable<RoleListDto> Roles { get; set; }
    }

    public class IndexAvisoViewModel
    {
        public string Filtro { get; set; }
    }
}