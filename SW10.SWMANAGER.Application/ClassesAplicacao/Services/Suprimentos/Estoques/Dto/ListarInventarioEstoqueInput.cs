using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class ListarInventarioEstoqueInput: ListarInput
    {
        public long? InventarioId { get; set; }
        //public long? GrupoId { get; set; }
        //public long? ClasseId { get; set; }
        //public long? SubClasseId { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "pro.DescricaoResumida";
            }
        }
    }
}
