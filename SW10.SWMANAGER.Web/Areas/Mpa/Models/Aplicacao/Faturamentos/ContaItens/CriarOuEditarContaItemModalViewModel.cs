using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TiposGrupo;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasItens
{
    [AutoMapFrom(typeof(FaturamentoContaItemDto))]
    public class CriarOuEditarContaItemModalViewModel : FaturamentoContaItemDto
    {
        public UserEditDto UpdateUser { get; set; }

        public string FaturamentoItemDescricao { get; set; }
        public string UnidadeOrganizacionalDescricao { get; set; }
        public string CentroCustoDescricao { get; set; }
        public string TerceirizadoDescricao { get; set; }
        public string TurnoDescricao { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public string MedicoNome { get; set; }

        public string Auxiliar1Nome { get; set; }
        public string Auxiliar2Nome { get; set; }
        public string Auxiliar3Nome { get; set; }
        public string AnestesistaNome { get; set; }
        public string InstrumentadorNome { get; set; }

        public string FornecedorNome { get; set; }
        public string MedicoEspecialidadeNome { get; set; }
        public string SisMoedaNome { get; set; }

        //public string Aux1Nome { get; set; }
        //public string Aux2Nome { get; set; }
        //public string Aux3Nome { get; set; }
        public string Auxiliar1EspecialidadeNome { get; set; }
        public string Auxiliar2EspecialidadeNome { get; set; }
        public string Auxiliar3EspecialidadeNome { get; set; }
        public string AnestNome { get; set; }
        public string AnestEspecialidadeNome { get; set; }
        public string InstrumentadorEspecialidadeNome { get; set; }

        public string ProcedimentoPrincipal { get; set; }

        public FaturamentoTipoGrupo TipoGrupo { get; set; }

        public string ItemCobrado { get; set; }


        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarContaItemModalViewModel(FaturamentoContaItemDto output)
        {
            output.MapTo(this);
        }


        public CriarOuEditarContaItemModalViewModel(FaturamentoContaItemViewModel output)
        {
            this.Id = output.Id;
            this.Descricao = output.Descricao;
            this.FaturamentoItemId = output.FaturamentoItemId;
            this.FaturamentoContaId = output.FaturamentoContaId;
            this.Data = output.Data;
            this.Qtde = output.Qtde;
            this.UnidadeOrganizacionalId = output.UnidadeOrganizacionalId;
            this.FaturamentoItemDescricao = output.FaturamentoItemDescricao;
            this.FaturamentoItem = output.FatItem;
            this.UnidadeOrganizacionalDescricao = output.UnidadeOrganizacionalDescricao;
            this.CentroCustoDescricao = output.CentroCustoDescricao;
            this.TerceirizadoDescricao = output.TerceirizadoDescricao;
            this.TurnoDescricao = output.TurnoDescricao;
            this.TipoLeitoDescricao = output.TipoLeitoDescricao;
            this.MedicoNome = output.MedicoNome;
            this.FornecedorNome = output.FornecedorNome;
            this.MedicoEspecialidadeNome = output.MedicoEspecialidadeNome;
            this.SisMoedaNome = output.SisMoedaNome;
            this.TerceirizadoId = output.TerceirizadoId;
            this.CentroCustoId = output.CentroCustoId;
            this.TurnoId = output.TurnoId;
            this.TipoLeitoId = output.TipoLeitoId;
            this.ValorTemp = output.ValorTemp;
            this.MedicoId = output.MedicoId;
            this.IsMedCredenciado = output.IsMedCredenciado;
            this.IsGlosaMedico = output.IsGlosaMedico;
            this.MedicoEspecialidadeId = output.MedicoEspecialidadeId;
            this.FaturamentoContaKitId = output.FaturamentoContaKitId;
            this.IsCirurgia = output.IsCirurgia;
            this.ValorAprovado = output.ValorAprovado;
            this.ValorTaxas = output.ValorTaxas;
            this.IsValorItemManual = output.IsValorItemManual;
            this.ValorItem = output.ValorItem;
            this.HMCH = output.HMCH;
            this.ValorFilme = output.ValorFilme;
            this.ValorFilmeAprovado = output.ValorFilmeAprovado;
            this.ValorCOCH = output.ValorCOCH;
            this.ValorCOCHAprovado = output.ValorCOCHAprovado;
            this.Percentual = output.Percentual;
            this.IsInstrCredenciado = output.IsInstrCredenciado;
            this.ValorTotalRecuperado = output.ValorTotalRecuperado;
            this.ValorTotalRecebido = output.ValorTotalRecebido;
            this.MetragemFilme = output.MetragemFilme;
            this.MetragemFilmeAprovada = output.MetragemFilmeAprovada;
            this.COCH = output.COCH;
            this.COCHAprovado = output.COCHAprovado;
            //STATUS ENTREGA NOVO  this.StatusEntrega                   = output.StatusEntrega;
            this.IsRecuperaMedico = output.IsRecuperaMedico;
            this.IsAux1Credenciado = output.IsAux1Credenciado;
            this.IsRecebeAuxiliar1 = output.IsRecebeAuxiliar1;
            this.IsGlosaAuxiliar1 = output.IsGlosaAuxiliar1;
            this.IsRecuperaAuxiliar1 = output.IsRecuperaAuxiliar1;
            this.IsAux2Credenciado = output.IsAux2Credenciado;
            this.IsRecebeAuxiliar2 = output.IsRecebeAuxiliar2;
            this.IsGlosaAuxiliar2 = output.IsGlosaAuxiliar2;
            this.IsRecuperaAuxiliar2 = output.IsRecuperaAuxiliar2;
            this.IsAux3Credenciado = output.IsAux3Credenciado;
            this.IsRecebeAuxiliar3 = output.IsRecebeAuxiliar3;
            this.IsGlosaAuxiliar3 = output.IsGlosaAuxiliar3;
            this.IsRecuperaAuxiliar3 = output.IsRecuperaAuxiliar3;
            this.IsRecebeInstrumentador = output.IsRecebeInstrumentador;
            this.IsGlosaInstrumentador = output.IsGlosaInstrumentador;
            this.IsRecuperaInstrumentador = output.IsRecuperaInstrumentador;
            this.Observacao = output.Observacao;
            this.QtdeRecuperada = output.QtdeRecuperada;
            this.QtdeAprovada = output.QtdeAprovada;
            this.QtdeRecebida = output.QtdeRecebida;
            this.ValorMoedaAprovado = output.ValorMoedaAprovado;
            this.SisMoedaId = output.SisMoedaId;
            this.DataAutorizacao = output.DataAutorizacao;
            this.SenhaAutorizacao = output.SenhaAutorizacao;
            this.NomeAutorizacao = output.NomeAutorizacao;
            this.ObsAutorizacao = output.ObsAutorizacao;
            this.HoraIncio = output.HoraIncio;
            this.HoraFim = output.HoraFim;
            this.ViaAcesso = output.ViaAcesso;
            this.Tecnica = output.Tecnica;
            this.ClinicaId = output.ClinicaId;
            this.FornecedorId = output.FornecedorId;
            this.NumeroNF = output.NumeroNF;
            this.IsImportaEstoque = output.IsImportaEstoque;
            this.Auxiliar1Id = output.Auxiliar1Id;
            this.Auxiliar1Nome = output.Auxiliar1Nome;
            this.IsAux1Credenciado = output.IsAux1Credenciado;
            this.Auxiliar1EspecialidadeId = output.Auxiliar1EspecialidadeId;
            this.Auxiliar1EspecialidadeNome = output.Auxiliar1EspecialidadeNome;
            this.Auxiliar2Id = output.Auxiliar2Id;
            this.Auxiliar2Nome = output.Auxiliar2Nome;
            this.IsAux2Credenciado = output.IsAux2Credenciado;
            this.Auxiliar2EspecialidadeId = output.Auxiliar2EspecialidadeId;
            this.Auxiliar2EspecialidadeNome = output.Auxiliar2EspecialidadeNome;
            this.Auxiliar3Id = output.Auxiliar3Id;
            this.Auxiliar3Nome = output.Auxiliar3Nome;
            this.IsAux3Credenciado = output.IsAux3Credenciado;
            this.Auxiliar3EspecialidadeId = output.Auxiliar3EspecialidadeId;
            this.Auxiliar3EspecialidadeNome = output.Auxiliar3EspecialidadeNome;
            this.AnestesistaId = output.AnestesistaId;
            this.AnestesistaNome = output.AnestesistaNome;
            this.IsAnestCredenciado = output.IsAnestCredenciado;
            this.EspecialidadeAnestesistaId = output.AnestesistaEspecialidadeId;
            this.AnestEspecialidadeNome = output.AnestesistaEspecialidadeNome;
            this.InstrumentadorId = output.InstrumentadorId;
            this.InstrumentadorNome = output.InstrumentadorNome;
            this.IsInstrCredenciado = output.IsInstrCredenciado;
            this.InstrumentadorEspecialidadeId = output.InstrumentadorEspecialidadeId;
            this.InstrumentadorEspecialidadeNome = output.InstrumentadorEspecialidadeNome;
            this.FaturamentoPacoteId = output.FaturamentoPacoteId;
            this.FaturamentoPacote = output.FaturamentoPacote;

        }

    }
}