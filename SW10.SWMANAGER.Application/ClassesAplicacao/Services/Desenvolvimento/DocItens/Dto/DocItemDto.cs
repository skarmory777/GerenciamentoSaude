using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [AutoMap(typeof(DocItem))]
    public class DocItemDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public string Titulo { get; set; }
        public float Ordem { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime DataPublicacao { get; set; }
        public string Versao { get; set; }
        //public List<DocAssuntoDto> Assuntos { get; set; }
        public long? Capitulo { get; set; }
        public string Conteudo { get; set; }

        public DocItemDto()
        {
            DataPublicacao = DateTime.Now;
            Capitulo = 0;
        }


        public string GetCapituloFront()
        {
            return Capitulo != null ? Capitulo.ToString() : string.Empty;
        }

        public string GetOrdemFront()
        {
            return Ordem != 0 ? Ordem.ToString() : string.Empty;
        }

        public string GetDataPublicacaoFront()
        {
            return DataPublicacao.ToString("dd/MM/yy");
        }
    }
}
