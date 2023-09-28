using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    [AutoMap(typeof(Fila))]
    public class FilaDto : CamposPadraoCRUDDto
    {
        public int NumeroInicial { get; set; }
        public int NumeroFinal { get; set; }
        public bool IsZera { get; set; }
        public DateTime? HoraZera { get; set; }
        public bool IsAtivo { get; set; }
        public bool IsDomingo { get; set; }
        public bool IsSegunda { get; set; }
        public bool IsTerca { get; set; }
        public bool IsQuarta { get; set; }
        public bool IsQuinta { get; set; }
        public bool IsSexta { get; set; }
        public bool IsSabado { get; set; }
        public string Cor { get; set; }
        public bool IsNaoImprimeSenha { get; set; }
        public long? TipoLocalChamadaInicialId { get; set; }

        public TipoLocalChamadaDto TipoLocalChamadaInicial { get; set; }

        public long QtdImpressaoSenha { get; set; } = 1;

        public long? EmpresaId { get; set; }

        public EmpresaDto Empresa { get; set; }

        #region Mapear

        public static FilaDto Mapear(Fila fila)
        {
            FilaDto filaDto = new FilaDto
            {
                Id = fila.Id,
                Codigo = fila.Codigo,
                Descricao = fila.Descricao,
                NumeroInicial = fila.NumeroInicial,
                NumeroFinal = fila.NumeroFinal,
                IsZera = fila.IsZera,
                HoraZera = fila.HoraZera,
                IsAtivo = fila.IsAtivo,
                IsDomingo = fila.IsDomingo,
                IsSegunda = fila.IsSegunda,
                IsTerca = fila.IsTerca,
                IsQuarta = fila.IsQuarta,
                IsQuinta = fila.IsQuinta,
                IsSexta = fila.IsSexta,
                IsSabado = fila.IsSabado,
                Cor = fila.Cor,
                IsNaoImprimeSenha = fila.IsNaoImprimeSenha,
                TipoLocalChamadaInicialId = fila.TipoLocalChamadaInicialId,
                EmpresaId = fila.EmpresaId,
                QtdImpressaoSenha = fila.QtdImpressaoSenha
            };

            if (fila.TipoLocalChamadaInicial != null)
            {
                filaDto.TipoLocalChamadaInicial = TipoLocalChamadaDto.Mapear(fila.TipoLocalChamadaInicial);
            }

            if (fila.Empresa != null)
            {
                filaDto.Empresa = new EmpresaDto { Id = fila.Empresa.Id, NomeFantasia = fila.Empresa?.NomeFantasia };
            }

            return filaDto;
        }

        public static Fila Mapear(FilaDto filaDto)
        {
            Fila fila = new Fila
            {
                Id = filaDto.Id,
                Codigo = filaDto.Codigo,
                Descricao = filaDto.Descricao,
                NumeroInicial = filaDto.NumeroInicial,
                NumeroFinal = filaDto.NumeroFinal,
                IsZera = filaDto.IsZera,
                HoraZera = filaDto.HoraZera,
                IsAtivo = filaDto.IsAtivo,
                IsDomingo = filaDto.IsDomingo,
                IsSegunda = filaDto.IsSegunda,
                IsTerca = filaDto.IsTerca,
                IsQuarta = filaDto.IsQuarta,
                IsQuinta = filaDto.IsQuinta,
                IsSexta = filaDto.IsSexta,
                IsSabado = filaDto.IsSabado,
                Cor = filaDto.Cor,
                IsNaoImprimeSenha = filaDto.IsNaoImprimeSenha,
                TipoLocalChamadaInicialId = filaDto.TipoLocalChamadaInicialId,
                QtdImpressaoSenha = filaDto.QtdImpressaoSenha
            };

            if (filaDto.TipoLocalChamadaInicial != null)
            {
                fila.TipoLocalChamadaInicial = TipoLocalChamadaDto.Mapear(filaDto.TipoLocalChamadaInicial);
            }


            if (filaDto.Empresa != null)
            {
                fila.Empresa = new Empresa { Id = filaDto.Empresa.Id, NomeFantasia = filaDto.Empresa.NomeFantasia };
            }


            return fila;
        }

        #endregion

    }

}
