using Abp.AutoMapper;
using Abp.Collections.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto
{
    [AutoMap(typeof(Divisao))]
    public class DivisaoDto : CamposPadraoCRUDDto
    {
        public long? TipoPrescricaoId { get; set; }

        public int Ordem { get; set; }

        public bool IsDivisaoPrincipal { get; set; }

        public long? DivisaoPrincipalId { get; set; }

        public DivisaoDto DivisaoPrincipal { get; set; }

        public bool IsResposta { get; set; }

        public string TiposRespostasSelecionadas { get; set; }

        public TipoPrescricaoDto TipoPrescricao { get; set; }

        public bool IsQuantidade { get; set; }
        public bool IsUnidadeMedida { get; set; }
        public bool IsVelocidadeInfusao { get; set; }
        public bool IsDuracao { get; set; }
        public bool IsFormaAplicacao { get; set; }
        public bool IsFrequencia { get; set; }
        public bool IsUniddeOrganizacional { get; set; }
        public bool IsMedico { get; set; }
        public bool IsDataInicio { get; set; }
        public bool IsDiasAplicacao { get; set; }
        public bool IsObservacao { get; set; }
        public bool IsCopiarPrescricao { get; set; }
        public bool IsTipoMedicacao { get; set; }
        public bool IsExameImagem { get; set; }
        public bool IsExameLaboratorial { get; set; }
        public bool IsSetorExame { get; set; }
        public bool IsProdutoEstoque { get; set; }
        public bool IsControlaVolume { get; set; }
        public bool IsSangueDerivado { get; set; }
        public bool IsSeNecessario { get; set; }
        public bool IsUrgente { get; set; }
        public bool IsAgora { get; set; }
        public bool IsAcm { get; set; }
        public bool IsMedicamento { get; set; }

        public bool IsDoseUnica { get; set; }

        public static DivisaoDto Mapear(Divisao input)
        {
            if (input == null)
            {
                return null;
            }

            var result = new DivisaoDto
            {
                Codigo = input.Codigo,
                CreationTime = input.CreationTime,
                CreatorUserId = input.CreatorUserId,
                Descricao = input.Descricao,
                DivisaoPrincipalId = input.DivisaoPrincipalId,
                Id = input.Id,
                IsAcm = input.IsAcm,
                IsAgora = input.IsAgora,
                IsControlaVolume = input.IsControlaVolume,
                IsCopiarPrescricao = input.IsCopiarPrescricao,
                IsDataInicio = input.IsDataInicio,
                IsDiasAplicacao = input.IsDiasAplicacao,
                IsDivisaoPrincipal = input.IsDivisaoPrincipal,
                IsDuracao = input.IsDuracao,
                IsExameImagem = input.IsExameImagem,
                IsExameLaboratorial = input.IsExameLaboratorial,
                IsFormaAplicacao = input.IsFormaAplicacao,
                IsFrequencia = input.IsFrequencia,
                IsMedicamento = input.IsMedicamento,
                IsMedico = input.IsMedico,
                IsObservacao = input.IsObservacao,
                IsProdutoEstoque = input.IsProdutoEstoque,
                IsQuantidade = input.IsQuantidade,
                IsSangueDerivado = input.IsSangueDerivado,
                IsSeNecessario = input.IsSeNecessario,
                IsSetorExame = input.IsSetorExame,
                IsSistema = input.IsSistema,
                IsTipoMedicacao = input.IsTipoMedicacao,
                IsUnidadeMedida = input.IsUnidadeMedida,
                IsUniddeOrganizacional = input.IsUniddeOrganizacional,
                IsUrgente = input.IsUrgente,
                IsDoseUnica = input.IsDoseUnica,
                IsVelocidadeInfusao = input.IsVelocidadeInfusao,
                LastModificationTime = input.LastModificationTime,
                LastModifierUserId = input.LastModifierUserId,
                Ordem = input.Ordem,
                TipoPrescricaoId = input.TipoPrescricaoId
            };

            if (input.DivisaoPrincipal != null)
            {
                result.DivisaoPrincipal = Mapear(input.DivisaoPrincipal);
            }

            if (input.TipoPrescricao != null)
            {
                result.TipoPrescricao = TipoPrescricaoDto.Mapear(input.TipoPrescricao);
            }

            return result;
        }

        public static Divisao Mapear(DivisaoDto input)
        {
            if (input == null)
            {
                return null;
            }

            var result = new Divisao
            {
                Codigo = input.Codigo,
                CreationTime = input.CreationTime,
                CreatorUserId = input.CreatorUserId,
                Descricao = input.Descricao,
                DivisaoPrincipalId = input.DivisaoPrincipalId,
                Id = input.Id,
                IsAcm = input.IsAcm,
                IsAgora = input.IsAgora,
                IsControlaVolume = input.IsControlaVolume,
                IsCopiarPrescricao = input.IsCopiarPrescricao,
                IsDataInicio = input.IsDataInicio,
                IsDiasAplicacao = input.IsDiasAplicacao,
                IsDivisaoPrincipal = input.IsDivisaoPrincipal,
                IsDuracao = input.IsDuracao,
                IsExameImagem = input.IsExameImagem,
                IsExameLaboratorial = input.IsExameLaboratorial,
                IsFormaAplicacao = input.IsFormaAplicacao,
                IsFrequencia = input.IsFrequencia,
                IsMedicamento = input.IsMedicamento,
                IsMedico = input.IsMedico,
                IsObservacao = input.IsObservacao,
                IsProdutoEstoque = input.IsProdutoEstoque,
                IsQuantidade = input.IsQuantidade,
                IsSangueDerivado = input.IsSangueDerivado,
                IsSeNecessario = input.IsSeNecessario,
                IsSetorExame = input.IsSetorExame,
                IsSistema = input.IsSistema,
                IsTipoMedicacao = input.IsTipoMedicacao,
                IsUnidadeMedida = input.IsUnidadeMedida,
                IsUniddeOrganizacional = input.IsUniddeOrganizacional,
                IsUrgente = input.IsUrgente,
                IsDoseUnica = input.IsDoseUnica,
                IsVelocidadeInfusao = input.IsVelocidadeInfusao,
                LastModificationTime = input.LastModificationTime,
                LastModifierUserId = input.LastModifierUserId,
                Ordem = input.Ordem,
                TipoPrescricaoId = input.TipoPrescricaoId
            };

            if (input.DivisaoPrincipal != null)
            {
                result.DivisaoPrincipal = Mapear(input.DivisaoPrincipal);
            }

            if (input.TipoPrescricao != null)
            {
                result.TipoPrescricao = TipoPrescricaoDto.Mapear(input.TipoPrescricao);
            }

            return result;
        }

        public static IEnumerable<DivisaoDto> Mapear(List<Divisao> list)
        {
            if (!list.IsNullOrEmpty())
            {
                foreach (var input in list)
                {
                    yield return Mapear(input);
                }
            }
        }

        public static IEnumerable<Divisao> Mapear(List<DivisaoDto> list)
        {
            if (!list.IsNullOrEmpty())
            {
                foreach (var input in list)
                {
                    yield return Mapear(input);
                }
            }
        }

    }
}