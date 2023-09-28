using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.CodigoCredenciado;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.CodigosCredenciados.Dto
{
    public class CodigoCredenciadoIndex : CamposPadraoCRUDDto
    {
        public bool IsAmbulatorioEmergencia { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsFuturoEspecialidade { get; set; }
        public long? ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }
        public long? EmpresaId { get; set; }
        public string EmpresaDescricao { get; set; }
        public EmpresaDto Empresa { get; set; }
        public long IdGrid { get; set; }

        #region Mapeamento
        public static CodigoCredenciadoDto Mapear(CodigoCredenciado input)
        {
            var result = new CodigoCredenciadoDto();
            result.Codigo = input.Codigo;
            result.ConvenioId = input.ConvenioId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.EmpresaId = input.EmpresaId;
            result.Id = input.Id;
            result.IsAmbulatorioEmergencia = input.IsAmbulatorioEmergencia;
            result.IsFuturoEspecialidade = input.IsFuturoEspecialidade;
            result.IsInternacao = input.IsInternacao;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            if (input.Convenio != null)
            {
                result.Convenio = ConvenioDto.Mapear(input.Convenio);
            }

            if (input.Empresa != null)
            {
                result.Empresa = EmpresaDto.Mapear(input.Empresa);
            }

            return result;
        }

        public static CodigoCredenciado Mapear(CodigoCredenciadoDto input)
        {
            var result = new CodigoCredenciado();
            result.Codigo = input.Codigo;
            result.ConvenioId = input.ConvenioId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.EmpresaId = input.EmpresaId;
            result.Id = input.Id;
            result.IsAmbulatorioEmergencia = input.IsAmbulatorioEmergencia;
            result.IsFuturoEspecialidade = input.IsFuturoEspecialidade;
            result.IsInternacao = input.IsInternacao;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            return result;
        }

        public static IEnumerable<CodigoCredenciadoDto> Mapear(List<CodigoCredenciado> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }

        public static IEnumerable<CodigoCredenciado> Mapear(List<CodigoCredenciadoDto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        #endregion
    }
}
