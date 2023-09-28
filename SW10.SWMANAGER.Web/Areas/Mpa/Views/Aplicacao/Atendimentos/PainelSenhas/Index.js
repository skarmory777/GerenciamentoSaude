(function () {
    $(function () {

        var _senhaService = abp.services.app.senha;

        window.setInterval(function () {
            if ($('#painelId').val() != null && $('#painelId').val() != '') {
                _senhaService.carregarPainelSenha($('#painelId').val())
                    .done(function (result) {
                        if (result != null) {


                            var senha = "";

                            $('#lblLocalChamadaAterior3').text($('#lblLocalChamadaAterior2').text());
                            $('#lblSenhaAnterior3').text($('#lblSenhaAnterior2').text());


                            $('#lblLocalChamadaAterior2').text($('#lblLocalChamadaAterior1').text());
                            $('#lblSenhaAnterior2').text($('#lblSenhaAnterior1').text());


                            $('#lblLocalChamadaAterior1').text($('#lblLocalChamadaAtual').text());
                            $('#lblSenhaAnterior1').text($('#lblSenhaAtual').text());



                            $('#lblLocalChamadaAtual').text(result.localChamadaAtual);
                            if (result.nomePaciente != '' && result.nomePaciente != null)
                            {
                                $('#lblSenhaAtual').text(result.nomePaciente);
                               
                                senha = result.nomePaciente;
                            }
                            else {
                                $('#lblSenhaAtual').text(result.senhaAtual);

                                senha = result.senhaAtual;
                               
                            }


                            if (!(result.nomePaciente != '' && result.nomePaciente != null))
                            {
                                var msg = new SpeechSynthesisUtterance("senha");
                                window.speechSynthesis.speak(msg);
                            }


                            var msg = new SpeechSynthesisUtterance(senha);
                            window.speechSynthesis.speak(msg);
                          
                            msg = new SpeechSynthesisUtterance(result.localChamadaAtual);
                            window.speechSynthesis.speak(msg);


                        }
                    });

            }

        }, 5000);

        selectSW('.selectPainel', "/api/services/app/painel/ListarDropdown");



        $('#painelId').on("change", function (e) {
            e.preventDefault();
            $('#divPainel').addClass("hidden");
            $('#divPrincipal').addClass("portlet-fullscreen");
        });

        

    });
})(jQuery);

     