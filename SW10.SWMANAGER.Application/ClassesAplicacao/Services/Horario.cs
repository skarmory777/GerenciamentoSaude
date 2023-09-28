namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    public class Horario
    {
        public string Hora { get; set; }
        public string Minuto { get; set; }
        public bool pm { get; set; }

        public Horario(string hora = "00", string min = "00", bool pm = false)
        {
            Hora = hora;
            Minuto = Minuto;
            this.pm = pm;
        }
    }
}
