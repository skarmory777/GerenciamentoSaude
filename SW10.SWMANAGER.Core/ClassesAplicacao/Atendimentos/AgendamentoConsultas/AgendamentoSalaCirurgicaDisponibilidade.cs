using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Intervalos;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas
{
    public class AgendamentoSalaCirurgicaDisponibilidade : CamposPadraoCRUD
    {

        //public AgendamentoCirurgico AgendamentoCirurgico { get; set; }

        //public long AgendamentoCirurgicoId { get; set; }

        public long SalaCirurgicaId { get; set; }

        [ForeignKey("SalaCirurgicaId")]
        public SalaCirurgica SalaCirurgica { get; set; }


        public long TipoCirurgiaId { get; set; }

        [ForeignKey("TipoCirurgiaId")]
        public TipoCirurgia TipoCirurgia { get; set; }


        public long IntervaloId { get; set; }

        [ForeignKey("IntervaloId")]
        public Intervalo Intervalo { get; set; }

        [Index("Ate_Idx_DataInicio")]
        public DateTime DataInicio { get; set; }

        [Index("Ate_Idx_DataFim")]
        public DateTime DataFim { get; set; }

        [Index("Ate_Idx_HoraInicio")]
        public DateTime HoraInicio { get; set; }

        [Index("Ate_Idx_HoraFim")]
        public DateTime HoraFim { get; set; }

        public bool Domingo { get; set; }

        public bool Segunda { get; set; }

        public bool Terca { get; set; }

        public bool Quarta { get; set; }

        public bool Quinta { get; set; }

        public bool Sexta { get; set; }

        public bool Sabado { get; set; }

        public long? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }


    }
}
