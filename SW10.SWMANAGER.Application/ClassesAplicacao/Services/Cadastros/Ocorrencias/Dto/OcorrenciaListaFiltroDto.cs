using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias.Dto
{
    public class OcorrenciaListaFiltroDto: ListarInput
    {
        public string SourceModel { get; set; }
        public string SourceId { get; set; }
        
        public string RelationModel { get; set; }
        public string RelationId { get; set; }
        
        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Ocorrencia.CreationTime desc";
            }
        }
    }
}