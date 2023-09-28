using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    public class CompraRequisicaoIndexDto
    {
        public long Id { get; set; }

        public string Codigo { get; set; }

        public bool IsUrgente { get; set; }

        public bool IsOrdemCompraFinalizada { get; set; }

        public bool IsRequisicaoAprovada { get; set; }

        public string Modo { get; set; }

        public DateTime DataRequisicao { get; set; }

        public DateTime DataLimiteEntrega { get; set; }

        public string Empresa { get; set; }

        public string UnidadeOrganizacional { get; set; }

        public string AprovacaoStatus { get; set; }

        public string Estoque { get; set; }

        public string MotivoPedido { get; set; }

        public DateTime? DataInicioCotacao { get; set; }
    }
}
