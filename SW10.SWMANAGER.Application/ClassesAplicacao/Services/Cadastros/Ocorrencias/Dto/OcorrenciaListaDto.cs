using System;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias.Dto
{
    public class OcorrenciaListaDto: CamposPadraoCRUDDto
    {
        public DateTime Data { get; set; }
        public string TipoOcorrenciaDescricao { get; set; }
        
        public string SubTipoOcorrenciaDescricao { get; set; }
        
        public string SourceModel { get; set; }
        
        public long? SourceId { get; set; }
        
        public string Texto { get; set; }

        private string _origem;
        public string Origem
        {
            get => _origem;
            set => _origem = Ocorrencia.GetNameByEntityModel(value);
        }
    }
    
    public class TipoOcorrenciaListaDto: CamposPadraoCRUDDto
    {
        
    }
}