using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto
{
    [AutoMap(typeof(Empresa))]
    public class EmpresaDto : PessoaJuridicaDto
    {
        public byte[] Logotipo { get; set; }

        public string LogotipoMimeType { get; set; }

        public int CodigoSus { get; set; }

        public int Cnes { get; set; }

        public bool IsAtiva { get; set; }

        public bool IsComprasUnificadas { get; set; }

        //ACERTAR REFERENCIA
        //public long? EstoqueMasterId { get; set; }
        //[ForeignKey("EstoqueMasterId")]
        //public virtual EstoqueMaster EstoqueMaster { get; set; }

        public long? EstoqueId { get; set; }
        public virtual EstoqueDto Estoque { get; set; }

        public long? ConvenioId { get; set; }
        public virtual ConvenioDto Convenio { get; set; }

        public long? PlanoId { get; set; }
        public virtual PlanoDto Plano { get; set; }

        //public long NumeroRegistroAns { get; set; }
        //public long CodigoCredenciadoEmpresa { get; set; }

        public static EmpresaDto Mapear(Empresa empresa)
        {
            if (empresa == null)
            {
                return null;
            }

            var empresaDto = MapearBase<EmpresaDto>(empresa);

            empresaDto.Id = empresa.Id;
            empresaDto.Codigo = empresa.Codigo;
            empresaDto.Descricao = empresa.Descricao;
            empresaDto.RazaoSocial = empresa.RazaoSocial;
            empresaDto.NomeFantasia = empresa.NomeFantasia;
            empresaDto.Cnpj = empresa.Cnpj;
            empresaDto.InscricaoEstadual = empresa.InscricaoEstadual;
            empresaDto.InscricaoMunicipal = empresa.InscricaoMunicipal;
            empresaDto.Cnes = empresa.Cnes;
            empresaDto.Logotipo = empresa.Logotipo;

            empresaDto.EstoqueId = empresa.EstoqueId;
            empresaDto.Estoque = EstoqueDto.Mapear(empresa.Estoque);

            empresaDto.ConvenioId = empresa.ConvenioId;
            empresaDto.Convenio = ConvenioDto.Mapear(empresa.Convenio);

            empresaDto.PlanoId = empresa.PlanoId;
            empresaDto.Plano = PlanoDto.Mapear(empresa.Plano);

            return empresaDto;
        }
        
        public static Empresa Mapear(EmpresaDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = MapearBase<Empresa>(dto);

            entity.Id = dto.Id;
            entity.Codigo = dto.Codigo;
            entity.Descricao = dto.Descricao;
            entity.RazaoSocial = dto.RazaoSocial;
            entity.NomeFantasia = dto.NomeFantasia;
            entity.Cnpj = dto.Cnpj;
            entity.InscricaoEstadual = dto.InscricaoEstadual;
            entity.InscricaoMunicipal = dto.InscricaoMunicipal;
            entity.Cnes = dto.Cnes;
            entity.Logotipo = dto.Logotipo;

            entity.EstoqueId = dto.EstoqueId;
            entity.Estoque = EstoqueDto.Mapear(dto.Estoque);

            entity.ConvenioId = dto.ConvenioId;
            entity.Convenio = ConvenioDto.Mapear(dto.Convenio);

            entity.PlanoId = dto.PlanoId;
            entity.Plano = PlanoDto.Mapear(dto.Plano);

            return entity;
        }
    }
}
