using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.Organizations.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto
{
    [AutoMap(typeof(UnidadeOrganizacional))]
    public class UnidadeOrganizacionalDto : CamposPadraoCRUDDto
    {
        public string Localizacao { get; set; }

        public bool IsAtivo { get; set; }

        public bool ControlaAlta { get; set; }

        public bool IsInternacao { get; set; }

        public bool IsLocalUtilizacao { get; set; }

        public bool IsAmbulatorioEmergencia { get; set; }

        public bool IsHospitalDia { get; set; }

        public bool IsSetorExames { get; set; }

        public bool IsEstoque { get; set; }

        public string HoraInicioPrescricao { get; set; }

        public long? UnidadeInternacaoTipoId { get; set; }
        public virtual UnidadeInternacaoTipoDto UnidadeInternacaoTipo { get; set; }


        public long OrganizationUnitId { get; set; }


        public virtual OrganizationUnitDto OrganizationUnit { get; set; }

        public long? EstoqueId { get; set; }
        public EstoqueDto Estoque { get; set; }

        public bool IsControlaLeito { get; set; }


        #region Mapeamento

        public static UnidadeOrganizacional Mapear(UnidadeOrganizacionalDto unidadeOrganizacionalDto)
        {
            if (unidadeOrganizacionalDto == null)
            {
                return null;
            }


            UnidadeOrganizacional unidadeOrganizacional = new UnidadeOrganizacional();

            unidadeOrganizacional.Id = unidadeOrganizacionalDto.Id;
            unidadeOrganizacional.Codigo = unidadeOrganizacionalDto.Codigo;
            unidadeOrganizacional.Descricao = unidadeOrganizacional.Descricao;
            unidadeOrganizacional.Localizacao = unidadeOrganizacionalDto.Localizacao;
            unidadeOrganizacional.IsAtivo = unidadeOrganizacionalDto.IsAtivo;
            unidadeOrganizacional.ControlaAlta = unidadeOrganizacionalDto.ControlaAlta;
            unidadeOrganizacional.IsInternacao = unidadeOrganizacionalDto.IsInternacao;
            unidadeOrganizacional.IsLocalUtilizacao = unidadeOrganizacionalDto.IsLocalUtilizacao;
            unidadeOrganizacional.IsAmbulatorioEmergencia = unidadeOrganizacionalDto.IsAmbulatorioEmergencia;
            unidadeOrganizacional.IsHospitalDia = unidadeOrganizacionalDto.IsHospitalDia;
            unidadeOrganizacional.IsSetorExames = unidadeOrganizacionalDto.IsSetorExames;
            unidadeOrganizacional.IsEstoque = unidadeOrganizacionalDto.IsEstoque;
            unidadeOrganizacional.IsControlaLeito = unidadeOrganizacionalDto.IsControlaLeito;
            unidadeOrganizacional.HoraInicioPrescricao = unidadeOrganizacionalDto.HoraInicioPrescricao;

            if (unidadeOrganizacionalDto.UnidadeInternacaoTipo != null)
            {
                unidadeOrganizacional.UnidadeInternacaoTipo = UnidadeInternacaoTipoDto.Mapear(unidadeOrganizacionalDto.UnidadeInternacaoTipo);
            }

            if (unidadeOrganizacionalDto.OrganizationUnit != null)
            {
                unidadeOrganizacional.OrganizationUnit = OrganizationUnitDto.Mapear(unidadeOrganizacionalDto.OrganizationUnit);
            }


            unidadeOrganizacional.EstoqueId = unidadeOrganizacionalDto.EstoqueId;


            return unidadeOrganizacional;
        }


        public static UnidadeOrganizacionalDto Mapear(UnidadeOrganizacional unidadeOrganizacional)
        {
            if (unidadeOrganizacional == null)
            {
                return null;
            }

            UnidadeOrganizacionalDto unidadeOrganizacionalDto =
                MapearBase<UnidadeOrganizacionalDto>(unidadeOrganizacional);

            unidadeOrganizacionalDto.Id = unidadeOrganizacional.Id;
            unidadeOrganizacionalDto.Codigo = unidadeOrganizacional.Codigo;
            unidadeOrganizacionalDto.Descricao = unidadeOrganizacional.Descricao;
            unidadeOrganizacionalDto.Localizacao = unidadeOrganizacional.Localizacao;
            unidadeOrganizacionalDto.IsAtivo = unidadeOrganizacional.IsAtivo;
            unidadeOrganizacionalDto.ControlaAlta = unidadeOrganizacional.ControlaAlta;
            unidadeOrganizacionalDto.IsInternacao = unidadeOrganizacional.IsInternacao;
            unidadeOrganizacionalDto.IsLocalUtilizacao = unidadeOrganizacional.IsLocalUtilizacao;
            unidadeOrganizacionalDto.IsAmbulatorioEmergencia = unidadeOrganizacional.IsAmbulatorioEmergencia;
            unidadeOrganizacionalDto.IsHospitalDia = unidadeOrganizacional.IsHospitalDia;
            unidadeOrganizacionalDto.IsSetorExames = unidadeOrganizacional.IsSetorExames;
            unidadeOrganizacionalDto.IsEstoque = unidadeOrganizacional.IsEstoque;
            unidadeOrganizacionalDto.IsControlaLeito = unidadeOrganizacional.IsControlaLeito;
            unidadeOrganizacionalDto.HoraInicioPrescricao = unidadeOrganizacional.HoraInicioPrescricao;

            if (unidadeOrganizacional.UnidadeInternacaoTipo != null)
            {
                unidadeOrganizacionalDto.UnidadeInternacaoTipo = UnidadeInternacaoTipoDto.Mapear(unidadeOrganizacional.UnidadeInternacaoTipo);
            }

            if (unidadeOrganizacional.OrganizationUnit != null)
            {
                unidadeOrganizacionalDto.OrganizationUnit = OrganizationUnitDto.Mapear(unidadeOrganizacional.OrganizationUnit);
            }

            return unidadeOrganizacionalDto;
        }


        #endregion



        public static UnidadeOrganizacionalDto MapearFromCore(UnidadeOrganizacional uo)
        {
            if (uo == null)
            {
                return null;
            }

            UnidadeOrganizacionalDto uoDto = new UnidadeOrganizacionalDto();

            uoDto.Id = uo.Id;
            uoDto.Codigo = uo.Codigo;
            uoDto.Descricao = uo.Descricao;
            uoDto.Localizacao = uo.Localizacao;
            uoDto.IsAtivo = uo.IsAtivo;
            uoDto.ControlaAlta = uo.ControlaAlta;
            uoDto.IsInternacao = uo.IsInternacao;
            uoDto.IsLocalUtilizacao = uo.IsLocalUtilizacao;
            uoDto.IsAmbulatorioEmergencia = uo.IsAmbulatorioEmergencia;
            uoDto.IsHospitalDia = uo.IsHospitalDia;
            uoDto.IsSetorExames = uo.IsSetorExames;
            uoDto.IsEstoque = uo.IsEstoque;
            uoDto.UnidadeInternacaoTipoId = uo.UnidadeInternacaoTipoId;
            uoDto.OrganizationUnitId = uo.OrganizationUnitId;
            uoDto.EstoqueId = uo.EstoqueId;
            uoDto.IsControlaLeito = uo.IsControlaLeito;
            uoDto.HoraInicioPrescricao = uo.HoraInicioPrescricao;


            if (uo.Estoque != null)
            {
                uoDto.Estoque = new EstoqueDto { Id = uo.Estoque.Id, Codigo = uo.Estoque.Codigo, Descricao = uo.Estoque.Descricao };
            }

            if (uo.OrganizationUnit != null)
            {
                uoDto.OrganizationUnit = new OrganizationUnitDto { Id = uo.OrganizationUnit.Id, Code = uo.OrganizationUnit.Code, DisplayName = uo.OrganizationUnit.DisplayName };
            }

            return uoDto;
        }
    }
}
