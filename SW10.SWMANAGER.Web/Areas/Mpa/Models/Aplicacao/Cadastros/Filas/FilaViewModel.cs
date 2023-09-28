using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Filas
{
    public class FilaViewModel : FilaDto
    {
        public string Filtro { get; set; }

        public UserEditDto UpdateUser { get; set; }

        public string HoraZeraStr;

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public FilaViewModel(FilaDto filaDto)
        {
            this.Id = filaDto.Id;
            this.Codigo = filaDto.Codigo;
            this.Descricao = filaDto.Descricao;
            this.NumeroFinal = filaDto.NumeroFinal;
            this.NumeroInicial = filaDto.NumeroInicial;
            this.TipoLocalChamadaInicialId = filaDto.TipoLocalChamadaInicialId;
            this.TipoLocalChamadaInicial = filaDto.TipoLocalChamadaInicial;
            this.IsZera = filaDto.IsZera;
            this.HoraZera = filaDto.HoraZera;
            if (filaDto.HoraZera != null)
            {
                this.HoraZeraStr = string.Format("{0:HH:mm}", filaDto.HoraZera);
            }
            this.Cor = filaDto.Cor;
            this.IsNaoImprimeSenha = filaDto.IsNaoImprimeSenha;
            this.QtdImpressaoSenha = filaDto.QtdImpressaoSenha;



            this.IsAtivo = filaDto.IsAtivo;
            this.IsDomingo = filaDto.IsDomingo;
            this.IsSegunda = filaDto.IsSegunda;
            this.IsTerca = filaDto.IsTerca;
            this.IsQuarta = filaDto.IsQuarta;
            this.IsQuinta = filaDto.IsQuinta;
            this.IsSexta = filaDto.IsSexta;
            this.IsSabado = filaDto.IsSabado;

            this.EmpresaId = filaDto.EmpresaId;
            this.Empresa = filaDto.Empresa;

        }
    }
}