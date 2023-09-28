using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(PrescricaoMedica))]
    public class PrescricaoMedicaDto : CamposPadraoCRUDDto
    {
        public DateTime DataPrescricao { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }
        public long? AtendimentoId { get; set; }
        public long? PrestadorId { get; set; }
        public string Observacao { get; set; }


        public long? DivisaoId { get; set; }

        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public AtendimentoDto Atendimento { get; set; }

        public long? MedicoId { get; set; }

        public MedicoDto Medico { get; set; }

        public long? PrescricaoStatusId { get; set; }
        public PrescricaoStatusDto PrescricaoStatus { get; set; }


        public long? LeitoId { get; set; }
        public LeitoDto Leito { get; set; }

        public long? AtendimentoLeitoId { get; set; }

        public bool HabilitaAlteracaoLeito { get
            {
                return LeitoId != AtendimentoLeitoId && Id != 0;
            } 
        }

        public static PrescricaoMedicaDto Mapear(PrescricaoMedica input)
        {
            var result = new PrescricaoMedicaDto();
            result.AtendimentoId = input.AtendimentoId;
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataPrescricao = input.DataPrescricao;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Observacao = input.Observacao;
            result.PrescricaoStatusId = input.PrescricaoStatusId;
            result.MedicoId = input.MedicoId;
            result.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
            result.LeitoId = input.LeitoId;
            if (input.Atendimento != null)
            {
                result.Atendimento = AtendimentoDto.MapearFromCore(input.Atendimento);
                result.AtendimentoLeitoId = input.Atendimento.LeitoId;
            }
            if (input.PrescricaoStatus != null)
            {
                result.PrescricaoStatus = PrescricaoStatusDto.Mapear(input.PrescricaoStatus);
            }

            if (input.Medico != null)
            {
                result.Medico = MedicoDto.Mapear(input.Medico);
            }

            if (input.Leito != null)
            {
                result.Leito = LeitoDto.Mapear(input.Leito);
            }

            return result;
        }

        public static PrescricaoMedica Mapear(PrescricaoMedicaDto input)
        {
            var result = new PrescricaoMedica();
            result.AtendimentoId = input.AtendimentoId;
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataPrescricao = input.DataPrescricao;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Observacao = input.Observacao;
            result.PrescricaoStatusId = input.PrescricaoStatusId;
            result.MedicoId = input.MedicoId;
            result.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
            result.LeitoId = input.LeitoId;
            if (input.Atendimento != null)
            {
                result.Atendimento = AtendimentoDto.Mapear(input.Atendimento);
            }
            if (input.PrescricaoStatus != null)
            {
                result.PrescricaoStatus = PrescricaoStatusDto.Mapear(input.PrescricaoStatus);
            }

            if (input.Medico != null)
            {
                result.Medico = MedicoDto.Mapear(input.Medico);
            }

            if (input.Leito != null)
            {
                result.Leito = LeitoDto.Mapear(input.Leito);
            }

            return result;
        }

        public static IEnumerable<PrescricaoMedicaDto> Mapear(List<PrescricaoMedica> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoMedicaDto();
                result.AtendimentoId = item.AtendimentoId;
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DataPrescricao = item.DataPrescricao;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.Observacao = item.Observacao;
                result.PrescricaoStatusId = item.PrescricaoStatusId;
                result.MedicoId = item.MedicoId;
                result.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                result.LeitoId = item.LeitoId;
                if (item.Atendimento != null)
                {
                    result.Atendimento = AtendimentoDto.MapearFromCore(item.Atendimento);
                    result.AtendimentoLeitoId = item.Atendimento.LeitoId;
                }
                if (item.PrescricaoStatus != null)
                {
                    result.PrescricaoStatus = PrescricaoStatusDto.Mapear(item.PrescricaoStatus);
                }

                if (item.Medico != null)
                {
                    result.Medico = MedicoDto.Mapear(item.Medico);
                }

                if (item.Leito != null)
                {
                    result.Leito = LeitoDto.Mapear(item.Leito);
                }

                yield return result;
            }
        }

        public static IEnumerable<PrescricaoMedica> Mapear(List<PrescricaoMedicaDto> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoMedica();
                result.AtendimentoId = item.AtendimentoId;
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DataPrescricao = item.DataPrescricao;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.Observacao = item.Observacao;
                result.PrescricaoStatusId = item.PrescricaoStatusId;
                result.MedicoId = item.MedicoId;
                result.LeitoId = item.LeitoId;
                result.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                if (item.Atendimento != null)
                {
                    result.Atendimento = AtendimentoDto.Mapear(item.Atendimento);
                }
                if (item.PrescricaoStatus != null)
                {
                    result.PrescricaoStatus = PrescricaoStatusDto.Mapear(item.PrescricaoStatus);
                }

                if (item.Medico != null)
                {
                    result.Medico = MedicoDto.Mapear(item.Medico);
                }

                yield return result;
            }
        }

    }
}
