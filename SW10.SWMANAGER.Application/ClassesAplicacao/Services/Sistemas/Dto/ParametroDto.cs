using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas.Dto
{
    public class ParametroDto : CamposPadraoCRUDDto
    {
        public long? EmpresaId { get; set; }

        public EmpresaDto Empresa { get; set; }

        public static ParametroDto Mapear(Parametro parametro)
        {
            ParametroDto parametroDto = new ParametroDto();

            parametroDto.Id = parametro.Id;
            parametroDto.Codigo = parametro.Codigo;
            parametroDto.Descricao = parametro.Descricao;
            parametroDto.EmpresaId = parametro.EmpresaId;

            if (parametro.Empresa != null)
            {
                parametroDto.Empresa = new EmpresaDto { Id = parametro.Empresa.Id, Descricao = parametro.Empresa.Descricao };
            }


            return parametroDto;
        }

        public static Parametro Mapear(ParametroDto parametroDto)
        {
            Parametro parametro = new Parametro();

            parametro.Id = parametroDto.Id;
            parametro.Codigo = parametroDto.Codigo;
            parametro.Descricao = parametroDto.Descricao;
            parametro.EmpresaId = parametroDto.EmpresaId;

            if (parametroDto.Empresa != null)
            {
                parametro.Empresa = new Empresa { Id = parametroDto.Empresa.Id, Descricao = parametroDto.Empresa.Descricao };
            }


            return parametro;
        }
    }
}
