using System.ComponentModel;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NFeServices.Ferramentas.Enums
{
    public enum TipoEventoSW
    {
        [Description("Carta de Correção")]
        CartaCorrecao = 110110,

        [Description("Cancelamento")]
        Cancelamento = 110111,

        [Description("Encerramento Homologado")]
        EncerramentoHomologado = 110112,

        [Description("EPEC CT-e")]
        EPECCTe = 110113,

        [Description("Inclusão de Condutor")]
        InclusaodeCondutor = 110114,

        [Description("EPEC NF-e")]
        EPECNFe = 110140,

        [Description("Registro Multimodal")]
        RegistroMultimodal = 110160,

        [Description("Confirmação da Operação")]
        ConfirmacaodaOperação = 210200,

        [Description("Ciência da Operação")]
        CienciadaOperacao = 210210,

        [Description("Desconhecimento da Operação")]
        DesconhecimentodaOperacao = 210220,

        [Description("Operação não Realizada")]
        OperacaonaoRealizada = 210240,

        [Description("Registro de Passagem")]
        RegistrodePassagem = 310620,

        [Description("Registro de Passagem BRID")]
        RegistrodePassagemBRID = 510620,

        [Description("CT-e Autorizado para NF-e")]
        CTeAutorizadoparaNFe = 610600,

        [Description("MDF-e Autorizado")]
        MDFeAutorizado = 610610,

        [Description("Registro de Passagem para NF-e Cancelado")]
        RegistrodePassagemparaNFeCancelado = 610501,

        [Description("Registro de Passagem para NF-e RFID")]
        RegistrodePassagemparaNFeRFID = 610550,

        [Description("CT-e Cancelado")]
        CTeCancelado = 610601,

        [Description("MDF-e Cancelado")]
        MDFeCancelado = 610611,

        [Description("Vistoria Suframa")]
        VistoriaSuframa = 990900,
    }
}
