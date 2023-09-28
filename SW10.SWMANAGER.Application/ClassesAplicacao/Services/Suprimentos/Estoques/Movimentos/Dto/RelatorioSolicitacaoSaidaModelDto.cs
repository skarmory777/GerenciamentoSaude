using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class RelatorioSolicitacaoSaidaModelDto
    {
        public long PreMovimentoId { get; set; }
        public string Titulo { get; set; }
        public string NomeHospital { get; set; }
        public string NomeUsuario { get; set; }
        public string DataHora { get; set; }
        public string Estoque { get; set; }
        public string Documento { get; set; }
        public string Paciente { get; set; }
        public string Setor { get; set; }
        public string Medico { get; set; }
        public string UsuarioSolicitacao { get; set; }
        public string DataHoraSolicitacao { get; set; }

        public List<RelatorioSolicitacaoSaidaItemModelDto> Itens { get; set; }
    }

    public class RelatorioSolicitacaoSaidaItemModelDto
    {
        public string CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Sigla { get; set; }
        public string Quantidade { get; set; }
    }

}
