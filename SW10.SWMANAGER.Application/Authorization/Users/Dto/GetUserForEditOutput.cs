using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditDto User { get; set; }

        public UserRoleDto[] Roles { get; set; }

        //Lista de Empresas do Usuário

        public ICollection<UserEmpresaDto> UserEmpresas { get; set; }

    }
}