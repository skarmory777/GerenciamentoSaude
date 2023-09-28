using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.Web.Core;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios
{
    public class FiltroModel
    {
        public string Titulo { get; set; }
        public string NomeHospital { get; set; }
        public string NomeUsuario { get; set; }
        public string DataHora { get; set; }
        public IList<SelectListItem> Empresas { get; set; }
        public IList<TesteObjeto> Lista { get; set; }
        public string Value { get; set; }
        public string Text { get; set; }

        //Gustavo Rosa - 16/05
        public DateTime DataAtendimento { get; set; }
        public string TipoAtendimento { get; set; }
        public string Paciente { get; set; }
        public string Convenio { get; set; }
        public string Medico { get; set; }
        public string Especilidade { set; get; }
        public string UnidadeOrganizacional { set; get; }
        public string CodPaciente { get; set; }

        //inteiro
        public long ConvenioId { get; set; }
        public long MedicoId { get; set; }
        public long EspecilidadeId { set; get; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public string TipoPeriodo { get; set; }
        public string Filtrado { get; set; }


        //Fiim



        //----------RETIRAR PROPRIEDADES ANTIGAS-----------------------------------
        public string Estoque { get; set; }
        public int? GrupoProduto { get; set; }
        public int? Classe { get; set; }
        public int? SubClasse { get; set; }
        public long Empresa { get; set; }



        public bool EhMovimentacao { get; set; }

        public IList<DataSetReports.ReportProdutosRow> Dados { get; set; }
        public IList<DataSetReports.RelatorioMovimentoRow> DadosMovimentacao { get; set; }
        public IList<SelectListItem> Grupos { get; set; }
        public IList<SelectListItem> Default { get; set; }
        //----------RETIRAR PROPRIEDADES ANTIGAS-----------------------------------
    }

    public class TesteObjeto
    {
        public string CodAtendimento { get; set; }
        public string DataInternacao { get; set; }
        public string CodPaciente { get; set; }
        public string Paciente { get; set; }
        public string Origem { get; set; }
        public string Empresa { get; set; }
        public string Convenio { get; set; }
        public string Plano { get; set; }
        public string UnidOrganizacional { get; set; }
        public string Leito { get; set; }
        public string Medico { get; set; }
        public string Idade { get; set; }
        public string DiasInternado { get; set; }
        public object Value { get; internal set; }
        public object Text { get; internal set; }
    }

    public class AtendimentoEtiqueta
    {
        public string AtendimentoId { get; set; }
        public string Paciente { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Idade { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string CodigoAtendimento { get; set; }
        public string MatriculaConvenio { get; set; }
        public string Convenio { get; set; }
    }

    public class VisitanteEtiqueta
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public bool IsAcompanhante { get; set; }
        public bool IsVisitante { get; set; }
        public bool IsMedico { get; set; }
        public bool IsEmergencia { get; set; }
        public bool IsInternado { get; set; }
        public bool IsFornecedor { get; set; }
        public bool IsSetor { get; set; }
        public byte[] Foto { get; set; }
        public string FotoMimeType { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public long AteId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public long? AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }
        public long? LeitoId { get; set; }
        public LeitoDto Leito { get; set; }
        public long? FornecedorId { get; set; }
        public FornecedorDto Fornecedor { get; set; }
        public bool IsFinalizar { get; set; }
    }


}
