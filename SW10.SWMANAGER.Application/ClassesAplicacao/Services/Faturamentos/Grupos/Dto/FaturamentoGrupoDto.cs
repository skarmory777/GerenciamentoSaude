using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupos;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto
{
    [AutoMap(typeof(FaturamentoGrupo))]
    public class FaturamentoGrupoDto : CamposPadraoCRUDDto
    {
        public static long Pacote = 1;
        public string CodTipoOutraDespesa { get; set; }
        public virtual FaturamentoTipoGrupoDto TipoGrupo { get; set; }//lauto relacionamento???
        public long? TipoGrupoId { get; set; }
        public virtual FaturamentoSubGrupo SubGrupo { get; set; }// USANDO CORE TEMPORARIO
        public long? SubGrupoId { get; set; }
        public string DescricaoTuss { get; set; }
        public string Observacao { get; set; }
        public string CodAmb { get; set; }
        public string CodTuss { get; set; }
        public string CodCbhpm { get; set; }
        public int Sexo { get; set; }
        public int QtdLaudo { get; set; }
        public int TipoLaudo { get; set; }
        public int DuracaoMinima { get; set; }
        public bool IsAtivo { get; set; }
        public bool IsOutraDespesa { get; set; }
        public bool IsObrigaMedico { get; set; }
        public bool IsTaxaUrgencia { get; set; }
        public bool IsPediatria { get; set; }
        public bool IsProcedimentoSerie { get; set; }
        public bool IsRequisicaoExame { get; set; }
        public bool IsPermiteRevisao { get; set; }
        public bool IsPrecoManual { get; set; }
        public bool IsAutorizacao { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsCirurgia { get; set; }
        public bool IsPorte { get; set; }
        public bool IsConsultor { get; set; }
        public bool IsLaboratorio { get; set; }
        public bool IsPlantonista { get; set; }
        public bool IsOpme { get; set; }
        public bool IsExtraCaixa { get; set; }
        public bool IsLaudo { get; set; }
        public long? FaturamentoCodigoDespesaId { get; set; }
        public FaturamentoCodigoDespesaDto FaturamentoCodigoDespesa { get; set; }

        public static FaturamentoGrupoDto Mapear(FaturamentoGrupo faturamentoGrupo)
        {
            if(faturamentoGrupo == null )
            {
                return null;
            }

            FaturamentoGrupoDto faturamentoGrupoDto = new FaturamentoGrupoDto();

            faturamentoGrupoDto.Id = faturamentoGrupo.Id;
            faturamentoGrupoDto.Codigo = faturamentoGrupo.Codigo;
            faturamentoGrupoDto.Descricao = faturamentoGrupo.Descricao;
            faturamentoGrupoDto.CodTipoOutraDespesa = faturamentoGrupo.CodTipoOutraDespesa;
            faturamentoGrupoDto.TipoGrupoId = faturamentoGrupo.TipoGrupoId ?? 1;
            faturamentoGrupoDto.IsAtivo = faturamentoGrupo.IsAtivo;
            faturamentoGrupoDto.IsOutraDespesa = faturamentoGrupo.IsOutraDespesa;
            faturamentoGrupoDto.IsObrigaMedico = faturamentoGrupo.IsObrigaMedico;
            faturamentoGrupoDto.IsTaxaUrgencia = faturamentoGrupo.IsTaxaUrgencia;
            faturamentoGrupoDto.IsPediatria = faturamentoGrupo.IsPediatria;
            faturamentoGrupoDto.IsProcedimentoSerie = faturamentoGrupo.IsProcedimentoSerie;
            faturamentoGrupoDto.IsRequisicaoExame = faturamentoGrupo.IsRequisicaoExame;
            faturamentoGrupoDto.IsPermiteRevisao = faturamentoGrupo.IsPermiteRevisao;
            faturamentoGrupoDto.IsPrecoManual = faturamentoGrupo.IsPrecoManual;
            faturamentoGrupoDto.IsAutorizacao = faturamentoGrupo.IsAutorizacao;
            faturamentoGrupoDto.IsInternacao = faturamentoGrupo.IsInternacao;
            faturamentoGrupoDto.IsAmbulatorio = faturamentoGrupo.IsAmbulatorio;
            faturamentoGrupoDto.IsCirurgia = faturamentoGrupo.IsCirurgia;
            faturamentoGrupoDto.IsPorte = faturamentoGrupo.IsPorte;
            faturamentoGrupoDto.IsConsultor = faturamentoGrupo.IsConsultor;
            faturamentoGrupoDto.IsLaboratorio = faturamentoGrupo.IsLaboratorio;
            faturamentoGrupoDto.IsPlantonista = faturamentoGrupo.IsPlantonista;
            faturamentoGrupoDto.IsOpme = faturamentoGrupo.IsOpme;
            faturamentoGrupoDto.IsExtraCaixa = faturamentoGrupo.IsExtraCaixa;
            faturamentoGrupoDto.IsLaudo = faturamentoGrupo.IsLaudo;
            faturamentoGrupoDto.FaturamentoCodigoDespesaId = faturamentoGrupo.FaturamentoCodigoDespesaId;

            if (faturamentoGrupo.FaturamentoCodigoDespesa != null)
            {
                faturamentoGrupoDto.FaturamentoCodigoDespesa = new FaturamentoCodigoDespesaDto { Id = faturamentoGrupo.FaturamentoCodigoDespesa.Id, Codigo = faturamentoGrupo.FaturamentoCodigoDespesa.Codigo, Descricao = faturamentoGrupo.FaturamentoCodigoDespesa.Descricao };
            }

            if (faturamentoGrupo.TipoGrupo != null)
            {
                faturamentoGrupoDto.TipoGrupo = new FaturamentoTipoGrupoDto { Id = faturamentoGrupo.TipoGrupo.Id, Codigo = faturamentoGrupo.TipoGrupo.Codigo, Descricao = faturamentoGrupo.TipoGrupo.Descricao };
            }

            return faturamentoGrupoDto;

        }

        public static FaturamentoGrupo Mapear(FaturamentoGrupoDto faturamentoGrupoDto)
        {
            if(faturamentoGrupoDto == null)
            {
                return null;
            }
            FaturamentoGrupo result = new FaturamentoGrupo();

            result.Id = faturamentoGrupoDto.Id;
            result.Codigo = faturamentoGrupoDto.Codigo;
            result.Descricao = faturamentoGrupoDto.Descricao;
            result.CodTipoOutraDespesa = faturamentoGrupoDto.CodTipoOutraDespesa;
            result.TipoGrupoId = faturamentoGrupoDto.TipoGrupoId ?? 1;
            result.IsAtivo = faturamentoGrupoDto.IsAtivo;
            result.IsOutraDespesa = faturamentoGrupoDto.IsOutraDespesa;
            result.IsObrigaMedico = faturamentoGrupoDto.IsObrigaMedico;
            result.IsTaxaUrgencia = faturamentoGrupoDto.IsTaxaUrgencia;
            result.IsPediatria = faturamentoGrupoDto.IsPediatria;
            result.IsProcedimentoSerie = faturamentoGrupoDto.IsProcedimentoSerie;
            result.IsRequisicaoExame = faturamentoGrupoDto.IsRequisicaoExame;
            result.IsPermiteRevisao = faturamentoGrupoDto.IsPermiteRevisao;
            result.IsPrecoManual = faturamentoGrupoDto.IsPrecoManual;
            result.IsAutorizacao = faturamentoGrupoDto.IsAutorizacao;
            result.IsInternacao = faturamentoGrupoDto.IsInternacao;
            result.IsAmbulatorio = faturamentoGrupoDto.IsAmbulatorio;
            result.IsCirurgia = faturamentoGrupoDto.IsCirurgia;
            result.IsPorte = faturamentoGrupoDto.IsPorte;
            result.IsConsultor = faturamentoGrupoDto.IsConsultor;
            result.IsLaboratorio = faturamentoGrupoDto.IsLaboratorio;
            result.IsPlantonista = faturamentoGrupoDto.IsPlantonista;
            result.IsOpme = faturamentoGrupoDto.IsOpme;
            result.IsExtraCaixa = faturamentoGrupoDto.IsExtraCaixa;
            result.IsLaudo = faturamentoGrupoDto.IsLaudo;
            result.FaturamentoCodigoDespesaId = faturamentoGrupoDto.FaturamentoCodigoDespesaId;

            if (faturamentoGrupoDto.FaturamentoCodigoDespesa != null)
            {
                result.FaturamentoCodigoDespesa = new FaturamentoCodigoDespesa { Id = faturamentoGrupoDto.FaturamentoCodigoDespesa.Id, Codigo = faturamentoGrupoDto.FaturamentoCodigoDespesa.Codigo, Descricao = faturamentoGrupoDto.FaturamentoCodigoDespesa.Descricao };
            }

            return result;

        }

        public static IEnumerable<FaturamentoGrupoDto> Mapear(List<FaturamentoGrupo> faturamentoGrupo)
        {
            foreach (var item in faturamentoGrupo)
            {
                FaturamentoGrupoDto result = new FaturamentoGrupoDto();

                result.Id = item.Id;
                result.Codigo = item.Codigo;
                result.Descricao = item.Descricao;
                result.CodTipoOutraDespesa = item.CodTipoOutraDespesa;
                result.TipoGrupoId = item.TipoGrupoId ?? 1;
                result.IsAtivo = item.IsAtivo;
                result.IsOutraDespesa = item.IsOutraDespesa;
                result.IsObrigaMedico = item.IsObrigaMedico;
                result.IsTaxaUrgencia = item.IsTaxaUrgencia;
                result.IsPediatria = item.IsPediatria;
                result.IsProcedimentoSerie = item.IsProcedimentoSerie;
                result.IsRequisicaoExame = item.IsRequisicaoExame;
                result.IsPermiteRevisao = item.IsPermiteRevisao;
                result.IsPrecoManual = item.IsPrecoManual;
                result.IsAutorizacao = item.IsAutorizacao;
                result.IsInternacao = item.IsInternacao;
                result.IsAmbulatorio = item.IsAmbulatorio;
                result.IsCirurgia = item.IsCirurgia;
                result.IsPorte = item.IsPorte;
                result.IsConsultor = item.IsConsultor;
                result.IsLaboratorio = item.IsLaboratorio;
                result.IsPlantonista = item.IsPlantonista;
                result.IsOpme = item.IsOpme;
                result.IsExtraCaixa = item.IsExtraCaixa;
                result.IsLaudo = item.IsLaudo;
                result.FaturamentoCodigoDespesaId = item.FaturamentoCodigoDespesaId;

                if (item.FaturamentoCodigoDespesa != null)
                {
                    result.FaturamentoCodigoDespesa = new FaturamentoCodigoDespesaDto { Id = item.FaturamentoCodigoDespesa.Id, Codigo = item.FaturamentoCodigoDespesa.Codigo, Descricao = item.FaturamentoCodigoDespesa.Descricao };
                }

                yield return result;
            }
        }

        public static IEnumerable<FaturamentoGrupo> Mapear(List<FaturamentoGrupoDto> faturamentoGrupoDto)
        {
            foreach (var item in faturamentoGrupoDto)
            {
                FaturamentoGrupo result = new FaturamentoGrupo();

                result.Id = item.Id;
                result.Codigo = item.Codigo;
                result.Descricao = item.Descricao;
                result.CodTipoOutraDespesa = item.CodTipoOutraDespesa;
                result.TipoGrupoId = item.TipoGrupoId ?? 1;
                result.IsAtivo = item.IsAtivo;
                result.IsOutraDespesa = item.IsOutraDespesa;
                result.IsObrigaMedico = item.IsObrigaMedico;
                result.IsTaxaUrgencia = item.IsTaxaUrgencia;
                result.IsPediatria = item.IsPediatria;
                result.IsProcedimentoSerie = item.IsProcedimentoSerie;
                result.IsRequisicaoExame = item.IsRequisicaoExame;
                result.IsPermiteRevisao = item.IsPermiteRevisao;
                result.IsPrecoManual = item.IsPrecoManual;
                result.IsAutorizacao = item.IsAutorizacao;
                result.IsInternacao = item.IsInternacao;
                result.IsAmbulatorio = item.IsAmbulatorio;
                result.IsCirurgia = item.IsCirurgia;
                result.IsPorte = item.IsPorte;
                result.IsConsultor = item.IsConsultor;
                result.IsLaboratorio = item.IsLaboratorio;
                result.IsPlantonista = item.IsPlantonista;
                result.IsOpme = item.IsOpme;
                result.IsExtraCaixa = item.IsExtraCaixa;
                result.IsLaudo = item.IsLaudo;
                result.FaturamentoCodigoDespesaId = item.FaturamentoCodigoDespesaId;

                if (item.FaturamentoCodigoDespesa != null)
                {
                    result.FaturamentoCodigoDespesa = new FaturamentoCodigoDespesa { Id = item.FaturamentoCodigoDespesa.Id, Codigo = item.FaturamentoCodigoDespesa.Codigo, Descricao = item.FaturamentoCodigoDespesa.Descricao };
                }

                yield return result;
            }
        }
    }
}
