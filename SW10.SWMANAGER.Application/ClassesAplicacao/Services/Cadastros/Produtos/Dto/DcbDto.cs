using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(DCB))]
    public class DcbDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Código DCB - Denominação Comum Brasileira
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// Descrição DCB - Denominação Comum Brasileira
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Entenda-se como Códigos CAS os números de registro presentes no banco de dados do Chemical Abstract Service - CAS, que são designados às substâncias, de maneira seqüencial, à medida que estas são colocadas na Base de Dados do CAS. Desta forma, cada número de registro CAS é um identificador numérico único, que designa apenas uma substância e que não possui significado químico algum. Os números de registro do CAS podem conter mais de nove dígitos, divididos por hífens em três partes, sendo o último dígito o verificador.
        /// </summary>
        public string CodigoCAS { get; set; }


    }
}
