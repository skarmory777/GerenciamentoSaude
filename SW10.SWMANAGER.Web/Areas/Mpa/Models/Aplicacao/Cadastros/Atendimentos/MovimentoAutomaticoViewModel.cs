using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos
{
    public class MovimentoAutomaticoViewModel : MovimentoAutomaticoDto
    {
        public MovimentoAutomaticoViewModel()
        { }

        public MovimentoAutomaticoViewModel(MovimentoAutomaticoDto movimentoAutomaticoDto)
        {
            this.Id = movimentoAutomaticoDto.Id;
            this.Codigo = movimentoAutomaticoDto.Codigo;
            this.Descricao = movimentoAutomaticoDto.Descricao;
            this.EmpresaId = movimentoAutomaticoDto.EmpresaId;
            this.UnidadeOrganizacionalId = movimentoAutomaticoDto.UnidadeOrganizacionalId;
            this.CentroCustoId = movimentoAutomaticoDto.CentroCustoId;
            this.TerceirizadoId = movimentoAutomaticoDto.TerceirizadoId;
            this.TurnoId = movimentoAutomaticoDto.TurnoId;
            this.TipoAcomodacaoId = movimentoAutomaticoDto.TipoAcomodacaoId;
            this.Quantidade = movimentoAutomaticoDto.Quantidade;
            this.IsAmbulatorio = movimentoAutomaticoDto.IsAmbulatorio;
            this.IsInternacao = movimentoAutomaticoDto.IsInternacao;
            this.IsNovoAtendimento = movimentoAutomaticoDto.IsNovoAtendimento;
            this.IsDiaria = movimentoAutomaticoDto.IsDiaria;
            this.IsCobraPernoite = movimentoAutomaticoDto.IsCobraPernoite;
            this.IsCobraRefeicao = movimentoAutomaticoDto.IsCobraRefeicao;
            this.IsCobraFralda = movimentoAutomaticoDto.IsCobraFralda;
            this.Empresa = movimentoAutomaticoDto.Empresa;
            this.UnidadeOrganizacional = movimentoAutomaticoDto.UnidadeOrganizacional;

            this.Turno = movimentoAutomaticoDto.Turno;
            this.CentroCusto = movimentoAutomaticoDto.CentroCusto;
            this.TipoAcomodacao = movimentoAutomaticoDto.TipoAcomodacao;
            this.Terceirizado = movimentoAutomaticoDto.Terceirizado;


        }


        public UserEditDto UpdateUser { get; set; }

        public string Filtro { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

    }
}