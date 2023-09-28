using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabFormata")]
    public class Formata : CamposPadraoCRUD
    {
        public string Formatacao { get; set; }

        public List<FormataItem> Itens { get; set; }
    }
}