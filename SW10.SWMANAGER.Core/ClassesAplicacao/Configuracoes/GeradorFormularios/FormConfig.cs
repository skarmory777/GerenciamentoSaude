using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios
{
    [Table("SisFormConfig")]
    public class FormConfig : CamposPadraoCRUD
    {
        public string Nome { get; set; }
        public virtual List<RowConfig> Linhas { get; set; }

        [Index("Sis_Idx_DataAlteracao")]
        public DateTime DataAlteracao { get; set; }
        
        public string FontSize { get; set; }
        /// <summary>
        /// Este campo será manipulado pelo sistema, se o formulário já possuir registros, não poderá ser editado
        /// </summary>
        public bool IsProducao { get; set; }
    }

    [Table("SisFormRowConfig")]
    public class RowConfig : CamposPadraoCRUD
    {
        //[ForeignKey("Col1Id")]
        //public virtual ColConfig Col1 { get; set; }
        //[ForeignKey("Col2Id")]
        //public virtual ColConfig Col2 { get; set; }
        //public int Ordem { get; set; }

        //public long Col1Id { get; set; }
        //public long? Col2Id { get; set; }
        //public long FormConfigId { get; set; }

        //[ForeignKey("FormConfigId")]
        //public virtual FormConfig FormConfig { get; set; }
        public virtual List<ColConfig> ColConfigs { get; set; }
        public int Ordem { get; set; }
        public int TotCols
        {
            get
            {
                if (this.ColConfigs != null)
                {
                    var count = this.ColConfigs.Count == 0 ? 12 : this.ColConfigs.Count;
                    return 12 / count;
                }

                return 1;



                //      return this.ColConfigs != null ? 12 / count : 12;
            }
        }

    }

    [Table("SisFormColConfig")]
    public class ColConfig : CamposPadraoCRUD
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Placeholder { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public bool Colspan { get; set; }
        public bool Readonly { get; set; }

        public string Orientation { get; set; }
        public string PrependText { get; set; }
        public string AppendText { get; set; }

        public string Offset { get; set; }
        public string Size { get; set; }

        public string Properties { get; set; }

        public List<ColMultiOption> MultiOption { get; set; }
        public List<FormData> Valores { get; set; }
        public int Ordem { get; set; }

        public int? Preenchimento { get; set; }
        // 1 - Em branco
        // 2 - Ultimo do Atendimento atual
        // 3 - Ultimo lancamento

        public const int EmBranco = 1;

        public const int AtendimentoAtual = 2;

        public const int UltimoLancamento = 3;
        
        public const int UltimoLancamentoPorAtendimento = 4;

        public long? ColConfigReservadoId { get; set; }

        public bool? SalvarTodos { get; set; }
        // false - Salvar apenas no atendimento atual
        // true - Todos do atendimento atual
    }

    [Table("SisFormColMultiOption")]
    public class ColMultiOption : CamposPadraoCRUD
    {
        //public string Opcao { get; set; }
        //public bool Selecionado { get; set; }

        //public long ColConfigId { get; set; }

        //[ForeignKey("ColConfigId")]
        //public virtual ColConfig ColConfig { get; set; }

        //public virtual bool IsChecked
        //{
        //    get
        //    {
        //        var result = false;
        //        if (ColConfig != null && ColConfig.Valores != null && ColConfig.Valores.Count() > 0)
        //        {
        //            var selected = ColConfig.Valores.Where(m => m.Valor.ToUpper().Equals(Opcao));
        //            result = selected.Count() > 0;
        //        }

        //        return result;
        //    }
        //}

        public string Opcao { get; set; }
        public bool Selecionado { get; set; }

    }

}