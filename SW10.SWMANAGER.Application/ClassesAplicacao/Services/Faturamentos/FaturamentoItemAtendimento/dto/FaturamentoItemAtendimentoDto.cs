using System;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturamentoItemAtendimento.dto
{
    public class FaturamentoItemAtendimentoDto: CamposPadraoCRUDDto
    {
        public static string PrescricaoItemResposta = "AssPrescricaoItemResposta";
        
        public long AtendimentoId { get; set; }
        
        public AtendimentoDto Atendimento { get; set; }
        
        public long? MedicoId { get; set; }
        
        public MedicoDto Medico { get; set; }
        
        public long? LeitoId { get; set; }
        
        public LeitoDto Leito { get; set; }
        
        public DateTime Data { get; set; }
        
        public long? FaturamentoItemId { get; set; }
        
        public FaturamentoItemDto FaturamentoItem { get; set; }
        
        public decimal? Quantidade { get; set; }
        
        public string Entidade { get; set; }
        
        public long EntidadeId { get; set; }
        


        public static FaturamentoItemAtendimentoDto Mapear(ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento model)
        {
            var dto = MapearBase<FaturamentoItemAtendimentoDto>(model);
            dto.AtendimentoId = model.AtendimentoId;
            dto.MedicoId = model.MedicoId;
            dto.LeitoId = model.LeitoId;
            dto.FaturamentoItemId = model.FaturamentoItemId;
            dto.Data = model.Data;
            dto.Quantidade = model.Quantidade;
            dto.Entidade = model.Entidade;
            dto.EntidadeId = model.EntidadeId;

            if (model.Atendimento != null)
            {
                dto.Atendimento = AtendimentoDto.Mapear(model.Atendimento);
            }
            
            if (model.Leito != null)
            {
                dto.Leito = LeitoDto.Mapear(model.Leito);
            }
            
            if (model.Medico != null)
            {
                dto.Medico = MedicoDto.Mapear(model.Medico);
            }
            
            if (model.FaturamentoItem != null)
            {
                dto.FaturamentoItem = FaturamentoItemDto.Mapear(model.FaturamentoItem);
            }

            return dto;
        }
        
        public static ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento Mapear(FaturamentoItemAtendimentoDto dto)
        {
            var model = MapearBase<ClassesAplicacao.Faturamentos.FaturamentoItemAtendimento>(dto);
            model.AtendimentoId = dto.AtendimentoId;
            model.MedicoId = dto.MedicoId;
            model.LeitoId = dto.LeitoId;
            model.FaturamentoItemId = dto.FaturamentoItemId;
            model.Data = dto.Data;
            model.Quantidade = dto.Quantidade;
            model.Entidade = dto.Entidade;
            model.EntidadeId = dto.EntidadeId;

            if (dto.Atendimento != null)
            {
                model.Atendimento = AtendimentoDto.Mapear(dto.Atendimento);
            }
            
            if (dto.Leito != null)
            {
                model.Leito = LeitoDto.Mapear(dto.Leito);
            }
            
            if (dto.Medico != null)
            {
                model.Medico = MedicoDto.Mapear(dto.Medico);
            }
            
            if (dto.FaturamentoItem != null)
            {
                model.FaturamentoItem = FaturamentoItemDto.Mapear(dto.FaturamentoItem);
            }

            return model;
        }
    }
}