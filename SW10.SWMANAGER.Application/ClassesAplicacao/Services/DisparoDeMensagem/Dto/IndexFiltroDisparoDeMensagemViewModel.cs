namespace SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto
{
    using Abp.Extensions;

    public class IndexFiltroDisparoDeMensagemViewModel : ListarInput
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
