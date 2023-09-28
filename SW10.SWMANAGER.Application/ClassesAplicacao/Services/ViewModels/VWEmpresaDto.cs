using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    [AutoMap(typeof(VWEmpresa))]
    public class VWEmpresaDto : EntityDto<long>
    {
        public string Nome { get; set; }
    }
}
