using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto
{
    public class IdentificacaoPrestadorNaOperadoraDto : CamposPadraoCRUDDto
    {
        public long ConvenioId { get; set; }
        public ConvenioDto Convenio { get; set; }

        public long EmpresaId { get; set; }
        public EmpresaDto Empresa { get; set; }

        public static IdentificacaoPrestadorNaOperadoraDto Mapear(IdentificacaoPrestadorNaOperadora identificacaoPrestadorNaOperadora)
        {
            if (identificacaoPrestadorNaOperadora == null)
            {
                return null;
            }

            var identificacaoPrestadorNaOperadoraDto =
                new IdentificacaoPrestadorNaOperadoraDto
                    {
                        Id = identificacaoPrestadorNaOperadora.Id,
                        Codigo = identificacaoPrestadorNaOperadora.Codigo,
                        Descricao = identificacaoPrestadorNaOperadora.Descricao,
                        ConvenioId = identificacaoPrestadorNaOperadora.ConvenioId,
                        EmpresaId = identificacaoPrestadorNaOperadora.EmpresaId
                    };


            //if(identificacaoPrestadorNaOperadora.Convenio!=null)
            //{
            //    identificacaoPrestadorNaOperadoraDto.Convenio = ConvenioDto.Mapear(identificacaoPrestadorNaOperadora.Convenio);
            //}

            if (identificacaoPrestadorNaOperadora.Empresa != null)
            {
                identificacaoPrestadorNaOperadoraDto.Empresa = new EmpresaDto { Id = identificacaoPrestadorNaOperadora.Empresa.Id, Codigo = identificacaoPrestadorNaOperadora.Empresa.Codigo, Descricao = identificacaoPrestadorNaOperadora.Empresa.Descricao };
            }

            return identificacaoPrestadorNaOperadoraDto;
        }

        public static IdentificacaoPrestadorNaOperadora Mapear(IdentificacaoPrestadorNaOperadoraDto identificacaoPrestadorNaOperadoraDto)
        {
            IdentificacaoPrestadorNaOperadora identificacaoPrestadorNaOperadora = new IdentificacaoPrestadorNaOperadora();

            identificacaoPrestadorNaOperadora.Id = identificacaoPrestadorNaOperadoraDto.Id;
            identificacaoPrestadorNaOperadora.Codigo = identificacaoPrestadorNaOperadoraDto.Codigo;
            identificacaoPrestadorNaOperadora.Descricao = identificacaoPrestadorNaOperadoraDto.Descricao;
            identificacaoPrestadorNaOperadora.ConvenioId = identificacaoPrestadorNaOperadoraDto.ConvenioId;
            identificacaoPrestadorNaOperadora.EmpresaId = identificacaoPrestadorNaOperadoraDto.EmpresaId;

            if (identificacaoPrestadorNaOperadoraDto.Convenio != null)
            {
                identificacaoPrestadorNaOperadora.Convenio = ConvenioDto.Mapear(identificacaoPrestadorNaOperadoraDto.Convenio);
            }

            if (identificacaoPrestadorNaOperadoraDto.Empresa != null)
            {
                identificacaoPrestadorNaOperadora.Empresa = new Empresa { Id = identificacaoPrestadorNaOperadoraDto.Empresa.Id, Codigo = identificacaoPrestadorNaOperadoraDto.Empresa.Codigo, Descricao = identificacaoPrestadorNaOperadoraDto.Empresa.Descricao };
            }

            return identificacaoPrestadorNaOperadora;
        }
    }
}
