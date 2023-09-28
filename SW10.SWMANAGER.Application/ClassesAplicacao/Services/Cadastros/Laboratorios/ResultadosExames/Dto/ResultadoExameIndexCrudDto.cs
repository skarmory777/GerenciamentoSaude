using System;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto
{
    public class ResultadoExameIndexCrudDto : CamposPadraoCRUDDto
    {
        public long? FaturamentoItemId { get; set; }
        public string Exame { get; set; }
        public long? FaturamentoContaItemId { get; set; }
        public string FaturamentoContaItem { get; set; }
        public long? MaterialId { get; set; }
        public string Material { get; set; }
        public int? Quantidade { get; set; }
        public long? UsuarioDigitadoId { get; set; }
        public DateTime? DataDigitado { get; set; }
        public long? UsuarioConferidoId { get; set; }
        public DateTime? DataConferido { get; set; }
        public long? UsuarioPendenteId { get; set; }
        public DateTime? DataPendente { get; set; }
        public string MotivoPendenteExame { get; set; }
        public bool IsSigiloso { get; set; }
        public string ObservacaoExame { get; set; }
        public long? SolicitacaoItemId { get; set; }
        public long? IdGridResultadoExame { get; set; }
        public long? ExameStatusId { get; set; }
        public string ExameStatus { get; set; }
        public string ExameStatusCor { get; set; }
        public string Cor { get; set; }
        public long? AtendimentoId { get; set; }
        public long? EmpresaId { get; set; }
        public string Empresa { get; set; }


        public static ResultadoExameIndexCrudDto Mapear(SolicitacaoExameItemDto dto, long? atendimentoId)
        {
            return new ResultadoExameIndexCrudDto
            {
                AtendimentoId = atendimentoId,
                FaturamentoItemId = dto.FaturamentoItemId,
                FaturamentoContaItem = dto.FaturamentoItem?.Descricao,
                ExameStatusId = ExameStatusDto.Inicial,
                ExameStatus = "Inicial",
                Exame = dto.FaturamentoItem?.Descricao ?? "",
                Quantidade = 1,
                Material = dto.Material?.Descricao,
                MaterialId = dto.MaterialId,
                SolicitacaoItemId = dto.Id
            };
        }
    }
}
