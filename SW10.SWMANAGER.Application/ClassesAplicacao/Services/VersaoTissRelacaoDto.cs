using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMap(typeof(VersaoTissRelacao))]
    public abstract class VersaoTissRelacaoDto : EntityDto<long>
    {
        public long VersaoTissId { get; set; }

        public bool Incluido { get; set; }

        public bool Excluido { get; set; }

        //[ForeignKey("VersaoTissId")]
        public virtual VersaoTissDto VersaoTiss { get; set; }

    }
}
