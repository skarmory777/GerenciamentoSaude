using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Globais.HorasDias.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(PrescricaoItemHora))]
    public class PrescricaoItemHoraDto : CamposPadraoCRUDDto
    {
        public long? PrescricaoItemRespostaId { get; set; }
        public PrescricaoItemRespostaDto PrescricaoItemResposta { get; set; }
        public int DiaMedicamento { get; set; }
        public DateTime DataMedicamento { get; set; }
        public string Hora { get; set; }
        public long? HoraDiaId { get; set; }
        public HoraDiaDto HoraDia { get; set; }

        public static PrescricaoItemHoraDto Mapear(PrescricaoItemHora input)
        {
            var result = new PrescricaoItemHoraDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataMedicamento = input.DataMedicamento;
            result.Descricao = input.Descricao;
            result.DiaMedicamento = input.DiaMedicamento;
            result.Hora = input.Hora;
            result.HoraDiaId = input.HoraDiaId;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.PrescricaoItemRespostaId = input.PrescricaoItemRespostaId;

            if (input.PrescricaoItemResposta != null)
            {
                result.PrescricaoItemResposta = PrescricaoItemRespostaDto.Mapear(input.PrescricaoItemResposta);
            }

            return result;
        }

        public static PrescricaoItemHora Mapear(PrescricaoItemHoraDto input)
        {
            var result = new PrescricaoItemHora();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataMedicamento = input.DataMedicamento;
            result.Descricao = input.Descricao;
            result.DiaMedicamento = input.DiaMedicamento;
            result.Hora = input.Hora;
            result.HoraDiaId = input.HoraDiaId;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.PrescricaoItemRespostaId = input.PrescricaoItemRespostaId;

            return result;
        }

        public static IEnumerable<PrescricaoItemHoraDto> Mapear(List<PrescricaoItemHora> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoItemHoraDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DataMedicamento = item.DataMedicamento;
                result.Descricao = item.Descricao;
                result.DiaMedicamento = item.DiaMedicamento;
                result.Hora = item.Hora;
                result.HoraDiaId = item.HoraDiaId;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.PrescricaoItemRespostaId = item.PrescricaoItemRespostaId;

                yield return result;
            }
        }

        public static IEnumerable<PrescricaoItemHora> Mapear(List<PrescricaoItemHoraDto> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoItemHora();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DataMedicamento = item.DataMedicamento;
                result.Descricao = item.Descricao;
                result.DiaMedicamento = item.DiaMedicamento;
                result.Hora = item.Hora;
                result.HoraDiaId = item.HoraDiaId;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.PrescricaoItemRespostaId = item.PrescricaoItemRespostaId;

                yield return result;
            }
        }
    }
}
