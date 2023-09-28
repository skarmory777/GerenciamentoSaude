using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [AutoMap(typeof(Projeto))]
    public class ProjetoDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataCriacao { get; set; }

        public string Nivel1 { get; set; }
        public string Nivel2 { get; set; }
        public string Nivel3 { get; set; }

        public string Conteudo { get; set; }

        public long? ProjetoId { get; set; }

        public ProjetoDto()
        {
            DataCriacao = DateTime.Now;
        }

        public string GetDataCriacaoFront()
        {
            return DataCriacao.ToString("dd/MM/yy");
        }
    }
}
