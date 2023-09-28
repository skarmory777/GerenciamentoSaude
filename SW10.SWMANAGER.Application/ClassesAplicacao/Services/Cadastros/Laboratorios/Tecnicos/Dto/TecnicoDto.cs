using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto
{
    [AutoMap(typeof(Tecnico))]
    public class TecnicoDto : CamposPadraoCRUDDto
    {
        public string RegConselho { get; set; }


        #region Mapeamento

        public static Tecnico Mapear(TecnicoDto tecnicoDto)
        {
            Tecnico tecnico = new Tecnico();

            tecnico.Id = tecnicoDto.Id;
            tecnico.Codigo = tecnicoDto.Codigo;
            tecnico.Descricao = tecnicoDto.Descricao;
            tecnico.RegConselho = tecnicoDto.RegConselho;

            return tecnico;
        }

        public static TecnicoDto Mapear(Tecnico tecnico)
        {
            TecnicoDto tecnicoDto = new TecnicoDto();

            tecnicoDto.Id = tecnico.Id;
            tecnicoDto.Codigo = tecnico.Codigo;
            tecnicoDto.Descricao = tecnico.Descricao;
            tecnicoDto.RegConselho = tecnico.RegConselho;

            return tecnicoDto;
        }

        #endregion
    }
}
