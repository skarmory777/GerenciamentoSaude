using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;

namespace SW10.SWMANAGER.Authorization.Users.Dto
{
    [AutoMapTo(typeof(UserEmpresa))]
    public class UserEmpresaDto : CamposPadraoCRUDDto
    {
        public long UserId { get; set; }

        public virtual UserEditDto User { get; set; }

        public long EmpresaId { get; set; }

        public virtual EmpresaDto Empresa { get; set; }
    }
}
