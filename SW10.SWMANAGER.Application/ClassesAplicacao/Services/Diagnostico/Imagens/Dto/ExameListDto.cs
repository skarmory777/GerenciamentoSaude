using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto
{
    [AutoMapFrom(typeof(LaudoMovimento))]
    public class ExameListDto : EntityDto<long>
    {
        public string Codigo { get; set; }
        public long AtendimentoId { get; set; }
        public long LaudoMovimentoStatusId { get; set; }
        public long? ConvenioId { get; set; }
        public long? LeitoId { get; set; }
        public bool IsContraste { get; set; }
        public string QtdeConstraste { get; set; }
        public string Obs { get; set; }

        public virtual ExameAtendimentoDto Atendimento { get; set; }
        public virtual LaudoMovimentoStatusDto LaudoMovimentoStatus { get; set; }
        public List<LaudoMovimentoItemDto> Itens { get; set; }

        #region LaudoMovimentoItemDto
        //[AutoMapFrom(typeof(LaudoMovimentoItem))]
        //public class LaudoMovimentoItemDto :CamposPadraoCRUDDto
        //{
        //    public long? TecnicoId { get; set; }
        //    public ExameFaturamentoItemDto FaturamentoItem { get; set; }
        //    public ExameSolicitacaoExameItemDto SolicitacaoExameItem { get; set; }

        //    // Novo
        //    public string Parecer { get; set; }
        //    public long? UsuarioParecerId { get; set; }
        //    public DateTime? ParecerData { get; set; }
        //    public string Laudo { get; set; }
        //    public long? UsuarioLaudoId { get; set; }
        //    public DateTime? LaudoData { get; set; }
        //    public string ConcordanciaLaudo { get; set; }
        //    public string JustificativaConcoLaudo { get; set; }
        //    public string Revisao { get; set; }
        //    public long? UsuarioRevisaoId { get; set; }
        //    public DateTime? RevisaoData { get; set; }
        //    public string Retificacao { get; set; }
        //    public long? UsuarioRetificacaoId { get; set; }
        //    public DateTime? RetificacaoData { get; set; }
        //    public int Status { get; set; }
        //}

        [AutoMapFrom(typeof(FaturamentoItem))]
        public class ExameFaturamentoItemDto
        {
            public string Codigo { get; set; }

            public string Descricao { get; set; }
        }

        [AutoMapFrom(typeof(SolicitacaoExameItem))]
        public class ExameSolicitacaoExameItemDto
        {
            public string Codigo { get; set; }
        }
        #endregion

        #region LaudoMovimentoStatusDto
        [AutoMapFrom(typeof(LaudoMovimentoStatus))]
        public class LaudoMovimentoStatusDto
        {
            public string Descricao { get; set; }
        }
        #endregion

        #region ExameAtendimentoDto
        [AutoMapFrom(typeof(Atendimento))]
        public class ExameAtendimentoDto
        {
            public string Codigo { get; set; }

            public virtual EXamePacienteDto Paciente { get; set; }

            public virtual ExameMedicoDto Medico { get; set; }
        }

        [AutoMapFrom(typeof(Paciente))]
        public class EXamePacienteDto
        {
            public string NomeCompleto { get; set; }
        }

        [AutoMapFrom(typeof(Medico))]
        public class ExameMedicoDto
        {
            public string NomeCompleto { get; set; }
        }
        #endregion
    }
}
