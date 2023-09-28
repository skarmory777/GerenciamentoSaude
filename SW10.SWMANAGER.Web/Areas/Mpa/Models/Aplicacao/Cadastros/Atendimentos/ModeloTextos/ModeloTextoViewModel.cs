using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.ModeloTextos
{
    public class ModeloTextoViewModel : TextoModeloDto
    {
        public ModeloTextoViewModel()
        { }

        public ModeloTextoViewModel(TextoModeloDto modeloTextoDto)
        {
            this.Id = modeloTextoDto.Id;
            this.Codigo = modeloTextoDto.Codigo;
            this.Descricao = modeloTextoDto.Descricao;
            this.IsAmbulatorioEmergencia = modeloTextoDto.IsAmbulatorioEmergencia;
            this.IsInternacao = modeloTextoDto.IsInternacao;
            this.IsMostraAtendimento = modeloTextoDto.IsMostraAtendimento;
            this.TipoModeloId = modeloTextoDto.TipoModeloId;
            this.TamanhoModeloId = modeloTextoDto.TamanhoModeloId;
            this.TamanhoModelo = modeloTextoDto.TamanhoModelo;
            this.TipoModelo = modeloTextoDto.TipoModelo;
        }


        public UserEditDto UpdateUser { get; set; }

        public string Filtro { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

    }
}