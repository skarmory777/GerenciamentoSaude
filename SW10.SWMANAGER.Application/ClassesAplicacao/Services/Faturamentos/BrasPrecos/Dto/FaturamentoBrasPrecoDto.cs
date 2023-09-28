using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasPrecos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Dto
{
    [AutoMap(typeof(FaturamentoBrasPreco))]
    public class FaturamentoBrasPrecoDto : CamposPadraoCRUDDto
    {
        public virtual FaturamentoBrasItem BrasItem { get; set; }
        public long BrasItemId { get; set; }

        public virtual FaturamentoBrasApresentacao BrasApresentacao { get; set; }
        public long? BrasApresentacaoId { get; set; }

        public virtual FaturamentoBrasLaboratorio BrasLaboratorio { get; set; }
        public long? BrasLaboratorioId { get; set; }

        public decimal Preco { get; set; }

        public string Tipo { get; set; }

        public string CodigoBrasTiss { get; set; }

        public string CodigoBrasTuss { get; set; }
    }
}
