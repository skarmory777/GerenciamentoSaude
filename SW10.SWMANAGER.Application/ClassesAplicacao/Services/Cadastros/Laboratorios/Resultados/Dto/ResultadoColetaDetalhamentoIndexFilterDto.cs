using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto
{
    public class ResultadoColetaDetalhamentoIndexFilterDto : ListarInput
    {
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = " ResultadoExame.CreationTime, Item.Descricao DESC";
            }
        }
    }
}