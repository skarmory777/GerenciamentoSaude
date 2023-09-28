using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto
{
    [AutoMap(typeof(Equipamento))]
    public class EquipamentoDto : CamposPadraoCRUDDto
    {
        public int TipoLayout { get; set; }
        public string DiretorioOrdem { get; set; }
        public string DiretorioResultado { get; set; }
        //public Informacao Informacao { get; set; }

        #region Mapeamento
        public static EquipamentoDto Mapear(Equipamento input)
        {
            var result = new EquipamentoDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.DiretorioOrdem = input.DiretorioOrdem;
            result.DiretorioResultado = input.DiretorioResultado;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.TipoLayout = input.TipoLayout;

            return result;
        }

        public static Equipamento Mapear(EquipamentoDto input)
        {
            var result = new Equipamento();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.DiretorioOrdem = input.DiretorioOrdem;
            result.DiretorioResultado = input.DiretorioResultado;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.TipoLayout = input.TipoLayout;

            return result;
        }

        public static IEnumerable<EquipamentoDto> Mapear(List<Equipamento> input)
        {
            foreach (var item in input)
            {
                var result = new EquipamentoDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.DiretorioOrdem = item.DiretorioOrdem;
                result.DiretorioResultado = item.DiretorioResultado;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.TipoLayout = item.TipoLayout;

                yield return result;
            }
        }

        public static IEnumerable<Equipamento> Mapear(List<EquipamentoDto> input)
        {
            foreach (var item in input)
            {
                var result = new Equipamento();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.DiretorioOrdem = item.DiretorioOrdem;
                result.DiretorioResultado = item.DiretorioResultado;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.TipoLayout = item.TipoLayout;

                yield return result;
            }
        }
        #endregion
    }
}
