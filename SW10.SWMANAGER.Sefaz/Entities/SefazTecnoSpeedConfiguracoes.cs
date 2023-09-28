namespace Sefaz.Entities
{
    using Sefaz.Dto;
    public class SefazTecnoSpeedConfiguracoes
    {
        public const string SefazTecnoSpeedConfiguracoesTable = "Sefaz_TecnoSpeedConfiguracoes";
        public long Id { get; set; }

        public bool Encode { get; set; }
        public bool Producao { get; set; }
        public string Grupo { get; set; }
        public string Cnpj { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string Delimitador { get; set; }

        public bool IsDeleted { get; set; }


        public static SefazConfig MapToSefazConfig(SefazTecnoSpeedConfiguracoes entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new SefazConfig
            {
                Encode = entity.Encode,
                Cnpj = entity.Cnpj,
                Grupo = entity.Grupo,
                Producao = entity.Producao,
                User = entity.User,
                Password = entity.Password,
                Delimitador = entity.Delimitador
            };

        }
    }
}
