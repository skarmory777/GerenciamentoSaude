using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasImports;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Dto
{
    [AutoMap(typeof(FaturamentoBrasImport))]
    public class FaturamentoBrasImportDto : CamposPadraoCRUDDto
    {
        public string CodigoLaboratorio { get; set; }

        public string Laboratorio { get; set; }

        public string CodigoProduto { get; set; }

        public string Produto { get; set; }

        public string CodigoApresentacao { get; set; }

        public string Apresentacao { get; set; }

        public string PrecoUnitario { get; set; }

        public string PrecoTotal { get; set; }

        public string NumeroUnidades { get; set; }

        public string Tipo { get; set; }

        public string Versao { get; set; }

        public string Extra { get; set; }

        public string IsAtualizado { get; set; }

        public string CodigoBarra { get; set; }

        public string CodigoBrasTiss { get; set; }

        public string CodigoBrasTuss { get; set; }

        public string CodigoHierarquico { get; set; }

        public static FaturamentoBrasImportDto Mapear(FaturamentoBrasImport faturamentoBrasImport)
        {
            if (faturamentoBrasImport == null)
            {
                return null;
            }

            var faturamentoBrasImportDto = MapearBase<FaturamentoBrasImportDto>(faturamentoBrasImport);

            faturamentoBrasImportDto.CodigoLaboratorio = faturamentoBrasImport.CodigoLaboratorio;
            faturamentoBrasImportDto.Laboratorio = faturamentoBrasImport.Laboratorio;
            faturamentoBrasImportDto.CodigoProduto = faturamentoBrasImport.CodigoProduto;
            faturamentoBrasImportDto.Produto = faturamentoBrasImport.Produto;
            faturamentoBrasImportDto.CodigoApresentacao = faturamentoBrasImport.CodigoApresentacao;
            faturamentoBrasImportDto.Apresentacao = faturamentoBrasImport.Apresentacao;
            faturamentoBrasImportDto.PrecoUnitario = faturamentoBrasImport.PrecoUnitario;
            faturamentoBrasImportDto.PrecoTotal = faturamentoBrasImport.PrecoTotal;
            faturamentoBrasImportDto.NumeroUnidades = faturamentoBrasImport.NumeroUnidades;
            faturamentoBrasImportDto.Tipo = faturamentoBrasImport.Tipo;
            faturamentoBrasImportDto.Versao = faturamentoBrasImport.Versao;
            faturamentoBrasImportDto.Extra = faturamentoBrasImport.Extra;
            faturamentoBrasImportDto.IsAtualizado = faturamentoBrasImport.IsAtualizado;
            faturamentoBrasImportDto.CodigoBarra = faturamentoBrasImport.CodigoBarra;
            faturamentoBrasImportDto.CodigoBrasTiss = faturamentoBrasImport.CodigoBrasTiss;
            faturamentoBrasImportDto.CodigoBrasTuss = faturamentoBrasImport.CodigoBrasTuss;
            faturamentoBrasImportDto.CodigoHierarquico = faturamentoBrasImport.CodigoHierarquico;

            return faturamentoBrasImportDto;
        }


        public static FaturamentoBrasImport Mapear(FaturamentoBrasImportDto faturamentoBrasImportDto)
        {
            if (faturamentoBrasImportDto == null)
            {
                return null;
            }

            var faturamentoBrasImport = MapearBase<FaturamentoBrasImport>(faturamentoBrasImportDto);

            faturamentoBrasImport.CodigoLaboratorio = faturamentoBrasImportDto.CodigoLaboratorio;
            faturamentoBrasImport.Laboratorio = faturamentoBrasImportDto.Laboratorio;
            faturamentoBrasImport.CodigoProduto = faturamentoBrasImportDto.CodigoProduto;
            faturamentoBrasImport.Produto = faturamentoBrasImportDto.Produto;
            faturamentoBrasImport.CodigoApresentacao = faturamentoBrasImportDto.CodigoApresentacao;
            faturamentoBrasImport.Apresentacao = faturamentoBrasImportDto.Apresentacao;
            faturamentoBrasImport.PrecoUnitario = faturamentoBrasImportDto.PrecoUnitario;
            faturamentoBrasImport.PrecoTotal = faturamentoBrasImportDto.PrecoTotal;
            faturamentoBrasImport.NumeroUnidades = faturamentoBrasImportDto.NumeroUnidades;
            faturamentoBrasImport.Tipo = faturamentoBrasImportDto.Tipo;
            faturamentoBrasImport.Versao = faturamentoBrasImportDto.Versao;
            faturamentoBrasImport.Extra = faturamentoBrasImportDto.Extra;
            faturamentoBrasImport.IsAtualizado = faturamentoBrasImportDto.IsAtualizado;
            faturamentoBrasImport.CodigoBarra = faturamentoBrasImportDto.CodigoBarra;
            faturamentoBrasImport.CodigoBrasTiss = faturamentoBrasImportDto.CodigoBrasTiss;
            faturamentoBrasImport.CodigoBrasTuss = faturamentoBrasImportDto.CodigoBrasTuss;
            faturamentoBrasImport.CodigoHierarquico = faturamentoBrasImportDto.CodigoHierarquico;

            return faturamentoBrasImport;
        }

    }
}
