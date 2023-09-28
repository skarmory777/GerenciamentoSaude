using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Avisos.Dto
{
    public class  IndexFiltroAvisoViewModel : ListarInput
    {
        public string Filtro { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataProgramada DESC";
            }
        }
    }
}