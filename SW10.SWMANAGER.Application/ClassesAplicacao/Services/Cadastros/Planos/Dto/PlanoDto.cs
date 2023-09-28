using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto
{
    [AutoMap(typeof(Plano))]
    public class PlanoDto : CamposPadraoCRUDDto
    {
        public virtual ConvenioDto Convenio { get; set; }
        public long? ConvenioId { get; set; }

        public bool IsDespesasAcompanhante { get; set; }

        public bool IsValidadeCarteiraIndeterminada { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsPlanoEmpresa { get; set; }


        public static PlanoDto Mapear(Plano input)
        {
            if (input == null) return null;

            var result = MapearBase<PlanoDto>(input);
            result.Codigo = input.Codigo;
            result.ConvenioId = input.ConvenioId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsAtivo = input.IsAtivo;
            result.IsDespesasAcompanhante = input.IsDespesasAcompanhante;
            result.IsPlanoEmpresa = input.IsPlanoEmpresa;
            result.IsSistema = input.IsSistema;
            result.IsValidadeCarteiraIndeterminada = input.IsValidadeCarteiraIndeterminada;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            if (input.Convenio != null)
            {
                result.Convenio = ConvenioDto.Mapear(input.Convenio);
            }

            return result;
        }

        public static Plano Mapear(PlanoDto input)
        {
            if (input == null)
            {
                return null;
            }

            var result = new Plano();
            result.Codigo = input.Codigo;
            result.ConvenioId = input.ConvenioId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsAtivo = input.IsAtivo;
            result.IsDespesasAcompanhante = input.IsDespesasAcompanhante;
            result.IsPlanoEmpresa = input.IsPlanoEmpresa;
            result.IsSistema = input.IsSistema;
            result.IsValidadeCarteiraIndeterminada = input.IsValidadeCarteiraIndeterminada;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            if (input.Convenio != null)
            {
                result.Convenio = ConvenioDto.Mapear(input.Convenio);
            }

            return result;
        }

        public static IEnumerable<PlanoDto> Mapear(List<Plano> input)
        {
            foreach (var item in input)
            {
                var result = new PlanoDto();
                result.Codigo = item.Codigo;
                result.ConvenioId = item.ConvenioId;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsAtivo = item.IsAtivo;
                result.IsDespesasAcompanhante = item.IsDespesasAcompanhante;
                result.IsPlanoEmpresa = item.IsPlanoEmpresa;
                result.IsSistema = item.IsSistema;
                result.IsValidadeCarteiraIndeterminada = item.IsValidadeCarteiraIndeterminada;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                if (item.Convenio != null)
                {
                    result.Convenio = ConvenioDto.Mapear(item.Convenio);
                }

                yield return result;
            }
        }

        public static IEnumerable<Plano> Mapear(List<PlanoDto> input)
        {
            foreach (var item in input)
            {
                var result = new Plano();
                result.Codigo = item.Codigo;
                result.ConvenioId = item.ConvenioId;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsAtivo = item.IsAtivo;
                result.IsDespesasAcompanhante = item.IsDespesasAcompanhante;
                result.IsPlanoEmpresa = item.IsPlanoEmpresa;
                result.IsSistema = item.IsSistema;
                result.IsValidadeCarteiraIndeterminada = item.IsValidadeCarteiraIndeterminada;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                if (item.Convenio != null)
                {
                    result.Convenio = ConvenioDto.Mapear(item.Convenio);
                }

                yield return result;
            }
        }

    }
}