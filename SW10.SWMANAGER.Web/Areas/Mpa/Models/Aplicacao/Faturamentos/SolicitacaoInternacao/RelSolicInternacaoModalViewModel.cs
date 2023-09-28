using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.SolicitacaoInternacao
{
    [AutoMapFrom(typeof(FaturamentoContaDto))]
    //[AutoMapFrom(typeof(ContaMedicaViewModel))]
    public class RelSolicInternacaoModalViewModel : FaturamentoContaDto
    {
        public UserEditDto UpdateUser { get; set; }
        public string PacienteNome { get; set; }
        public string MedicoNome { get; set; }
        public string ConvenioNome { get; set; }
        public string PlanoNome { get; set; }
        public string GuiaNumero { get; set; }
        public string EmpresaNome { get; set; }
        public string UnidadeOrganizacionalNome { get; set; }
        public string AtendimentoCodigo { get; set; }

        public long? AtendimentoIdTeste { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public long? AtendimentoPlanoId { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorPlano { get; set; }
        public FaturamentoConfigConvenioDto[] configsPorEmpresa { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public RelSolicInternacaoModalViewModel(FaturamentoContaDto output)
        {
            output.MapTo(this);
        }

        public RelSolicInternacaoModalViewModel(ContaMedicaViewModel output)
        {
            this.Id = output.Id;
            this.Matricula = output.Matricula;
            this.CodDependente = output.CodDependente;
            this.NumeroGuia = output.NumeroGuia;
            this.Titular = output.Titular;
            this.GuiaOperadora = output.GuiaOperadora;
            this.GuiaPrincipal = output.GuiaPrincipal;
            this.Observacao = output.Observacao;
            this.SenhaAutorizacao = output.SenhaAutorizacao;
            this.IdentAcompanhante = output.IdentAcompanhante;
            this.PacienteId = output.PacienteId;
            this.MedicoId = output.MedicoId;
            this.PacienteNome = output.PacienteNome;
            this.MedicoNome = output.MedicoNome;
            this.ConvenioNome = output.ConvenioNome;
            this.PlanoNome = output.PlanoNome;
            this.GuiaNumero = output.GuiaNumero;
            this.EmpresaNome = output.EmpresaNome;
            this.UnidadeOrganizacionalNome = output.UnidadeOrganizacionalNome;
            this.AtendimentoCodigo = output.AtendimentoCodigo;
            this.TipoLeitoDescricao = output.TipoLeitoDescricao;
            this.ConvenioId = output.ConvenioId;
            this.PlanoId = output.PlanoId;
            this.GuiaId = output.GuiaId;
            this.EmpresaId = output.EmpresaId;
            this.AtendimentoId = output.AtendimentoId;
            this.UnidadeOrganizacionalId = output.UnidadeOrganizacionalId;
            //this.TipoLeitoId = output.TipoLeitoId;
            this.TipoAcomodacaoId = output.TipoLeitoId;
            this.DataInicio = output.DataInicio;
            this.DataFim = output.DataFim;
            this.DataPagamento = output.DataPagamento;
            this.ValidadeCarteira = output.ValidadeCarteira;
            this.DataAutorizacao = output.DataAutorizacao;
            this.DiaSerie1 = output.DiaSerie1;
            this.DiaSerie2 = output.DiaSerie2;
            this.DiaSerie3 = output.DiaSerie3;
            this.DiaSerie4 = output.DiaSerie4;
            this.DiaSerie5 = output.DiaSerie5;
            this.DiaSerie6 = output.DiaSerie6;
            this.DiaSerie7 = output.DiaSerie7;
            this.DiaSerie8 = output.DiaSerie8;
            this.DiaSerie9 = output.DiaSerie9;
            this.DiaSerie10 = output.DiaSerie10;
            this.DataEntrFolhaSala = output.DataEntrFolhaSala;
            this.DataEntrDescCir = output.DataEntrDescCir;
            this.DataEntrBolAnest = output.DataEntrBolAnest;
            this.DataEntrCDFilme = output.DataEntrCDFilme;
            this.DataValidadeSenha = output.DataValidadeSenha;
            this.IsAutorizador = output.IsAutorizador;
            this.TipoAtendimento = output.TipoAtendimento;
            //   this.StatusEntrega = output.StatusEntrega;
            this.Id = output.Id;

        }
    }
}