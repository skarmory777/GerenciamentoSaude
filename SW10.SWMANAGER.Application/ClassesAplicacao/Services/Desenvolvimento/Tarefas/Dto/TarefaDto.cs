using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [AutoMap(typeof(Tarefa))]
    public class TarefaDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public decimal? Ordem { get; set; }
        public long? ProjetoId { get; set; }
        public virtual Projeto Projeto { get; set; }
        public string ProjetoNome { get; set; }
        public DateTime? DataRegistro { get; set; }
        public DateTime? DataPrevistaInicio { get; set; }
        public DateTime? DataPrevistaTermino { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataTermino { get; set; }

        public long? ResponsavelId { get; set; }

        public string ResponsavelNome { get; set; }

        public long? CriadorId { get; set; }

        public string CriadorNome { get; set; }

        public long? ModuloId { get; set; }
        public virtual DocRotulo Modulo { get; set; }

        public long? StatusId { get; set; }
        public virtual DocRotulo Status { get; set; }
        public string StatusDescricao { get; set; }
        public string StatusCor { get; set; }

        public long? PrioridadeId { get; set; }
        public virtual DocRotulo Prioridade { get; set; }
        public string PrioridadeDescricao { get; set; }

        public long? TipoTarefaId { get; set; }
        public virtual DocRotulo TipoTarefa { get; set; }

        public long? UserId { get; set; }
        public virtual UsuarioIdNome User { get; set; }


        public long? Nivel1Id { get; set; }
        public virtual DocRotulo Nivel1 { get; set; }

        public long? Nivel2Id { get; set; }
        public virtual DocRotulo Nivel2 { get; set; }

        public long? Nivel3Id { get; set; }
        public virtual DocRotulo Nivel3 { get; set; }

        public long? ClienteId { get; set; }
        public string Conteudo { get; set; }

        public DateTime? IntervaloInicio { get; set; }

        public string TempoDecorrido { get; set; }



        public TarefaDto()
        {
            DataRegistro = DateTime.Now;
        }

        //public string CalcularTempoDecorrido ()
        //{
        //    if(IntervaloInicio != null)
        //    {
        //        TimeSpan timeSpan = (DateTime.Now - (DateTime)IntervaloInicio);
        //        TempoDecorrido = string.Format("{0} horas, {1} minutos", timeSpan.Hours, timeSpan.Minutes);
        //        return TempoDecorrido;
        //    }
        //    else
        //    {
        //        return "N/A";
        //    }

        //}

        public string GetDataRegistroFront()
        {
            return DataRegistro?.ToString("dd/MM/yy");
        }

        public string GetDataPrevistaInicioFront()
        {
            return DataPrevistaInicio?.ToString("dd/MM/yy");
        }

        public string GetDataPrevistaTerminoFront()
        {
            return DataPrevistaTermino?.ToString("dd/MM/yy");
        }

        public string GetDataInicioFront()
        {
            return DataInicio?.ToString("dd/MM/yy");
        }

        public string GetDataTerminoFront()
        {
            return DataTermino?.ToString("dd/MM/yy");
        }
    }
}
