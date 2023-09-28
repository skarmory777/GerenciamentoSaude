using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AtePainel")]
    public class Painel : CamposPadraoCRUD
    {
        public List<PainelTipoLocalChamada> PaineisTipoLocaisChamadas { get; set; }

    }
}
