using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto
{
    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;

    public class TextoModeloDto : CamposPadraoCRUD
    {
        public long? Id { get; set; }

        public string Texto { get; set; }

        public bool IsAmbulatorioEmergencia { get; set; }

        public bool IsInternacao { get; set; }

        public bool IsMostraAtendimento { get; set; }

        public List<long?> LstEmpresaId { get; set; }

        public List<long?> LstFatGuiaId { get; set; }

        public List<GridEmpresaDto> Empresas { get; set; }

        public List<GridGuiaDto> Guias { get; set; }

        public long? EmpresaId { get; set; }

        public long? FatGuiaId { get; set; }

        /// <summary>
        /// Gets or sets the tipo modelo id.
        /// </summary>
        public virtual long? TipoModeloId { get; set; }

        /// <summary>
        /// Gets or sets the tamanho modelo id.
        /// </summary>
        public virtual long? TamanhoModeloId { get; set; }

        /// <summary>
        /// Gets or sets the tamanho modelo.
        /// </summary>
        public TamanhoModelo TamanhoModelo { get; set; }

        /// <summary>
        /// Gets or sets the tipo modelo.
        /// </summary>
        public TipoModelo TipoModelo { get; set; }
    }
}
