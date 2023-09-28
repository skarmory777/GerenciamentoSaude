using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto
{
    [AutoMap(typeof(ItemResultado))]
    public class ItemResultadoDto : CamposPadraoCRUDDto
    {
        public int? CasaDecimal { get; set; }
        //  public double ErroMinimo { get; set; }
        //   public double ErroMaximo { get; set; }
        //  public double AlteradoMinimo { get; set; }
        //  public double AlteradoMaximo { get; set; }
        //  public double AceitavelMinimo { get; set; }
        //  public double AceitavelMaximo { get; set; }
        public string Referencia { get; set; }
        public string Formula { get; set; }
        //   public double Normal { get; set; }
        public int? TamFixo { get; set; }
        public string ObsAnormal { get; set; }
        //  public double ErroMinimoFeminino { get; set; }
        //   public double AlteradoMinimoFeminino { get; set; }
        //   public double NormalFeminino { get; set; }
        //  public double AceitavelMaximoFeminino { get; set; }
        //  public double ErroMaximoFeminino { get; set; }
        public string Interface { get; set; }
        public string InterfaceEnvio { get; set; }
        //public string Equipamento { get; set; }
        public double DivideInter { get; set; }
        public bool IsAntibiotico { get; set; }
        public bool IsBacteria { get; set; }
        public bool IsInteiro { get; set; }
        public bool IsObrigatorio { get; set; }
        public bool IsMultiValor { get; set; }
        public bool IsSoma100 { get; set; }
        public bool ParteInteira { get; set; }
        public bool IsInterface { get; set; }
        public bool IsTamFixo { get; set; }

        public long? TipoResultadoId { get; set; }
        public long? LaboratorioUnidadeId { get; set; }
        public long? TabelaId { get; set; }
        public long? EquipamentoId { get; set; }

        public TabelaDto Tabela { get; set; }
        public LaboratorioUnidadeDto LaboratorioUnidade { get; set; }
        public TipoResultadoDto TipoResultado { get; set; }
        public EquipamentoDto Equipamento { get; set; }



        public decimal? MinimoAceitavelMasculino { get; set; }
        public decimal? MaximoAceitavelMasculino { get; set; }
        public decimal? MinimoMasculino { get; set; }
        public decimal? MaximoMasculino { get; set; }
        public decimal? NormalMasculino { get; set; }


        public decimal? MinimoAceitavelFeminino { get; set; }
        public decimal? MaximoAceitavelFeminino { get; set; }
        public decimal? MinimoFeminino { get; set; }
        public decimal? MaximoFeminino { get; set; }
        public decimal? NormalFeminino { get; set; }

        #region Mapeamento

        public static ItemResultadoDto Mapear(ItemResultado input)
        {
            ItemResultadoDto result = new ItemResultadoDto();

            result.Id = input.Id;
            result.Codigo = input.Codigo;
            result.Descricao = input.Descricao;
            result.CasaDecimal = input.CasaDecimal;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DivideInter = input.DivideInter;
            result.EquipamentoId = input.EquipamentoId;
            result.Formula = input.Formula;
            result.Id = input.Id;
            result.Interface = input.Interface;
            result.InterfaceEnvio = input.InterfaceEnvio;
            result.IsAntibiotico = input.IsAntibiotico;
            result.IsBacteria = input.IsBacteria;
            result.IsInteiro = input.IsInteiro;
            result.IsInterface = input.IsInterface;
            result.IsMultiValor = input.IsMultiValor;
            result.IsObrigatorio = input.IsObrigatorio;
            result.IsSistema = input.IsSistema;
            result.IsSoma100 = input.IsSoma100;
            result.IsTamFixo = input.IsTamFixo;
            result.LaboratorioUnidadeId = input.LaboratorioUnidadeId;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
            result.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
            result.MaximoFeminino = input.MaximoFeminino;
            result.MaximoMasculino = input.MaximoMasculino;
            result.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
            result.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
            result.MinimoFeminino = input.MinimoFeminino;
            result.MinimoMasculino = input.MinimoMasculino;
            result.NormalFeminino = input.NormalFeminino;
            result.NormalMasculino = input.NormalMasculino;
            result.ObsAnormal = input.ObsAnormal;
            result.ParteInteira = input.ParteInteira;
            result.Referencia = input.Referencia;
            result.TabelaId = input.TabelaId;
            result.TamFixo = input.TamFixo;
            result.TipoResultadoId = input.TipoResultadoId;

            if (input.Equipamento != null)
            {
                result.Equipamento = EquipamentoDto.Mapear(input.Equipamento);
            }

            if (input.LaboratorioUnidade != null)
            {
                result.LaboratorioUnidade = LaboratorioUnidadeDto.Mapear(input.LaboratorioUnidade);
            }
            if (input.TipoResultado != null)
            {
                result.TipoResultado = TipoResultadoDto.Mapear(input.TipoResultado);
            }
            return result;
        }

        public static ItemResultado Mapear(ItemResultadoDto input)
        {
            var result = new ItemResultado();

            result.Id = input.Id;
            result.Codigo = input.Codigo;
            result.Descricao = input.Descricao;
            result.CasaDecimal = input.CasaDecimal;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DivideInter = input.DivideInter;
            result.EquipamentoId = input.EquipamentoId;
            result.Formula = input.Formula;
            result.Id = input.Id;
            result.Interface = input.Interface;
            result.InterfaceEnvio = input.InterfaceEnvio;
            result.IsAntibiotico = input.IsAntibiotico;
            result.IsBacteria = input.IsBacteria;
            result.IsInteiro = input.IsInteiro;
            result.IsInterface = input.IsInterface;
            result.IsMultiValor = input.IsMultiValor;
            result.IsObrigatorio = input.IsObrigatorio;
            result.IsSistema = input.IsSistema;
            result.IsSoma100 = input.IsSoma100;
            result.IsTamFixo = input.IsTamFixo;
            result.LaboratorioUnidadeId = input.LaboratorioUnidadeId;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.MaximoAceitavelFeminino = input.MaximoAceitavelFeminino;
            result.MaximoAceitavelMasculino = input.MaximoAceitavelMasculino;
            result.MaximoFeminino = input.MaximoFeminino;
            result.MaximoMasculino = input.MaximoMasculino;
            result.MinimoAceitavelFeminino = input.MinimoAceitavelFeminino;
            result.MinimoAceitavelMasculino = input.MinimoAceitavelMasculino;
            result.MinimoFeminino = input.MinimoFeminino;
            result.MinimoMasculino = input.MinimoMasculino;
            result.NormalFeminino = input.NormalFeminino;
            result.NormalMasculino = input.NormalMasculino;
            result.ObsAnormal = input.ObsAnormal;
            result.ParteInteira = input.ParteInteira;
            result.Referencia = input.Referencia;
            result.TabelaId = input.TabelaId;
            result.TamFixo = input.TamFixo;
            result.TipoResultadoId = input.TipoResultadoId;

            if (input.Equipamento != null)
            {
                result.Equipamento = EquipamentoDto.Mapear(input.Equipamento);
            }

            if (input.LaboratorioUnidade != null)
            {
                result.LaboratorioUnidade = LaboratorioUnidadeDto.Mapear(input.LaboratorioUnidade);
            }
            if (input.TipoResultado != null)
            {
                result.TipoResultado = TipoResultadoDto.Mapear(input.TipoResultado);
            }
            return result;
        }

        public static IEnumerable<ItemResultadoDto> Mapear(List<ItemResultado> input)
        {
            foreach (var item in input)
            {
                var result = new ItemResultadoDto();

                result.Id = item.Id;
                result.Codigo = item.Codigo;
                result.Descricao = item.Descricao;
                result.CasaDecimal = item.CasaDecimal;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DivideInter = item.DivideInter;
                result.EquipamentoId = item.EquipamentoId;
                result.Formula = item.Formula;
                result.Id = item.Id;
                result.Interface = item.Interface;
                result.InterfaceEnvio = item.InterfaceEnvio;
                result.IsAntibiotico = item.IsAntibiotico;
                result.IsBacteria = item.IsBacteria;
                result.IsInteiro = item.IsInteiro;
                result.IsInterface = item.IsInterface;
                result.IsMultiValor = item.IsMultiValor;
                result.IsObrigatorio = item.IsObrigatorio;
                result.IsSistema = item.IsSistema;
                result.IsSoma100 = item.IsSoma100;
                result.IsTamFixo = item.IsTamFixo;
                result.LaboratorioUnidadeId = item.LaboratorioUnidadeId;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.MaximoAceitavelFeminino = item.MaximoAceitavelFeminino;
                result.MaximoAceitavelMasculino = item.MaximoAceitavelMasculino;
                result.MaximoFeminino = item.MaximoFeminino;
                result.MaximoMasculino = item.MaximoMasculino;
                result.MinimoAceitavelFeminino = item.MinimoAceitavelFeminino;
                result.MinimoAceitavelMasculino = item.MinimoAceitavelMasculino;
                result.MinimoFeminino = item.MinimoFeminino;
                result.MinimoMasculino = item.MinimoMasculino;
                result.NormalFeminino = item.NormalFeminino;
                result.NormalMasculino = item.NormalMasculino;
                result.ObsAnormal = item.ObsAnormal;
                result.ParteInteira = item.ParteInteira;
                result.Referencia = item.Referencia;
                result.TabelaId = item.TabelaId;
                result.TamFixo = item.TamFixo;
                result.TipoResultadoId = item.TipoResultadoId;

                if (item.Equipamento != null)
                {
                    result.Equipamento = EquipamentoDto.Mapear(item.Equipamento);
                }

                if (item.LaboratorioUnidade != null)
                {
                    result.LaboratorioUnidade = LaboratorioUnidadeDto.Mapear(item.LaboratorioUnidade);
                }
                if (item.TipoResultado != null)
                {
                    result.TipoResultado = TipoResultadoDto.Mapear(item.TipoResultado);
                }

                yield return result;
            }
        }

        public static IEnumerable<ItemResultado> Mapear(List<ItemResultadoDto> input)
        {
            foreach (var item in input)
            {
                var result = new ItemResultado();

                result.Id = item.Id;
                result.Codigo = item.Codigo;
                result.Descricao = item.Descricao;
                result.CasaDecimal = item.CasaDecimal;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.DivideInter = item.DivideInter;
                result.EquipamentoId = item.EquipamentoId;
                result.Formula = item.Formula;
                result.Id = item.Id;
                result.Interface = item.Interface;
                result.InterfaceEnvio = item.InterfaceEnvio;
                result.IsAntibiotico = item.IsAntibiotico;
                result.IsBacteria = item.IsBacteria;
                result.IsInteiro = item.IsInteiro;
                result.IsInterface = item.IsInterface;
                result.IsMultiValor = item.IsMultiValor;
                result.IsObrigatorio = item.IsObrigatorio;
                result.IsSistema = item.IsSistema;
                result.IsSoma100 = item.IsSoma100;
                result.IsTamFixo = item.IsTamFixo;
                result.LaboratorioUnidadeId = item.LaboratorioUnidadeId;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.MaximoAceitavelFeminino = item.MaximoAceitavelFeminino;
                result.MaximoAceitavelMasculino = item.MaximoAceitavelMasculino;
                result.MaximoFeminino = item.MaximoFeminino;
                result.MaximoMasculino = item.MaximoMasculino;
                result.MinimoAceitavelFeminino = item.MinimoAceitavelFeminino;
                result.MinimoAceitavelMasculino = item.MinimoAceitavelMasculino;
                result.MinimoFeminino = item.MinimoFeminino;
                result.MinimoMasculino = item.MinimoMasculino;
                result.NormalFeminino = item.NormalFeminino;
                result.NormalMasculino = item.NormalMasculino;
                result.ObsAnormal = item.ObsAnormal;
                result.ParteInteira = item.ParteInteira;
                result.Referencia = item.Referencia;
                result.TabelaId = item.TabelaId;
                result.TamFixo = item.TamFixo;
                result.TipoResultadoId = item.TipoResultadoId;

                if (item.Equipamento != null)
                {
                    result.Equipamento = EquipamentoDto.Mapear(item.Equipamento);
                }

                if (item.LaboratorioUnidade != null)
                {
                    result.LaboratorioUnidade = LaboratorioUnidadeDto.Mapear(item.LaboratorioUnidade);
                }
                if (item.TipoResultado != null)
                {
                    result.TipoResultado = TipoResultadoDto.Mapear(item.TipoResultado);
                }

                yield return result;
            }
        }

        #endregion

    }
}
