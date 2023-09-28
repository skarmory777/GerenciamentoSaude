using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasMedicas
{
    public class ContasMedicasViewModel
    {
        public string Filtro { get; set; }

        public long? EmpresaId { get; set; }

        public long? ConvenioId { get; set; }

        public long? PacienteId { get; set; }

        public long? MedicoId { get; set; }

        public long? GuiaId { get; set; }

        public string NumeroGuia { get; set; }

        public List<FaturamentoContaStatusDto> ListaStatus { get; set; }
    }
}