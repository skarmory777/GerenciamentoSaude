namespace SW10.SWMANAGER.ClassesAplicacao
{
    public abstract class PessoaJuridica : Pessoa
    {
        public string RazaoSocial { get; set; }

        public virtual string NomeFantasia { get; set; }

        public string Cnpj { get; set; }

        public string InscricaoEstadual { get; set; }

        public string InscricaoMunicipal { get; set; }

    }
}
