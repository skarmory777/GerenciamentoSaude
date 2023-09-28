namespace SW10.SWMANAGER.ClassesAplicacao.Sefaz
{
    using Abp.Domain.Entities;
    using global::Sefaz.Entities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table(SefazTecnoSpeedConfiguracoes.SefazTecnoSpeedConfiguracoesTable)]
    public class AbpSefazTecnoSpeedConfiguracoes : SefazTecnoSpeedConfiguracoes, IEntity<long>
    {
        public AbpSefazTecnoSpeedConfiguracoes()
        {

        }

        public bool IsTransient()
        {
            if (EqualityComparer<long>.Default.Equals(Id, default(long)))
            {
                return true;
            }

            //Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
            if (typeof(long) == typeof(int))
            {
                return Convert.ToInt32(Id) <= 0;
            }

            if (typeof(long) == typeof(long))
            {
                return Convert.ToInt64(Id) <= 0;
            }

            return false;
        }
    }
}
