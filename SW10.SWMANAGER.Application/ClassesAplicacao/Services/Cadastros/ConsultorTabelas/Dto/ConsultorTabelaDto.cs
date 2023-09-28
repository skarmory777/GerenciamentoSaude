using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto
{
    [AutoMap(typeof(ConsultorTabela))]
    public class ConsultorTabelaDto : CamposPadraoCRUDDto
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string Descricao { get; set; }

        [StringLength(500)]
        public string Observacao { get; set; }

        [StringLength(50)]
        public string ItemMenu { get; set; }
    }
}
