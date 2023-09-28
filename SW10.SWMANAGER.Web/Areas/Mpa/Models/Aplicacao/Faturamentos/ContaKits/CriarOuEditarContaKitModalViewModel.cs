#region Usings
using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits;

#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasKits
{
    [AutoMapFrom(typeof(FaturamentoContaKitDto))]
    public class CriarOuEditarContaKitModalViewModel : FaturamentoContaKitViewModel// FaturamentoContaKitDto
    {
        public UserEditDto UpdateUser { get; set; }

        public string UnidadeOrganizacional { get; set; }
        public string Terceirizado { get; set; }
        public string MedicoEspecialidade { get; set; }
        public string Auxiliar1 { get; set; }
        public string Auxiliar1Especialidade { get; set; }
        public string Auxiliar2 { get; set; }
        public string Auxiliar2Especialidade { get; set; }
        public string Auxiliar3 { get; set; }
        public string Auxiliar3Especialidade { get; set; }
        public string Instrumentador { get; set; }
        public string InstrumentadorEspecialidade { get; set; }
        public string Anestesista { get; set; }
        public string AnestesistaEspecialidade { get; set; }
        public string ProcedimentoPrincipal { get; set; }
        public bool IsMedCredenciado { get; set; }
        public bool IsAux1Credenciado { get; set; }
        public bool IsAux2Credenciado { get; set; }
        public bool IsAux3Credenciado { get; set; }
        public bool IsInstrCredenciado { get; set; }
        public bool IsAnestCredenciado { get; set; }

        public double Percentual { get; set; }

        public string Hora { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarContaKitModalViewModel(FaturamentoContaKitDto output)
        {
            output.MapTo(this);
        }

        public CriarOuEditarContaKitModalViewModel(FaturamentoContaKitViewModel output)
        {
            this.Id = output.Id;
            this.Codigo = output.Codigo;
            this.Descricao = output.Descricao;
            this.FaturamentoContaId = output.FaturamentoContaId;
            this.FaturamentoKitId = output.FaturamentoKitId;
            this.FaturamentoKitDescricao = output.FaturamentoKitDescricao;
            this.Data = output.Data;
            this.Qtde = output.Qtde;
            this.CentroCustoId = output.CentroCustoId;
            this.CentroCustoDescricao = output.CentroCustoDescricao;
            this.TurnoDescricao = output.TurnoDescricao;
            this.TipoLeitoDescricao = output.TipoLeitoDescricao;
            this.MedicoNome = output.MedicoNome;
            this.TurnoId = output.TurnoId;
            this.TipoLeitoId = output.TipoLeitoId;
            this.HoraIncio = output.HoraIncio;
            this.HoraFim = output.HoraFim;
            this.MedicoId = output.MedicoId;
        }
    }
}