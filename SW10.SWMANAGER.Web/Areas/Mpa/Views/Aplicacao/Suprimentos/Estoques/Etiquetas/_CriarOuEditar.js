(function ($) {
    app.modals.CriarOuEditarEtiqueta = function () {
        const codigoBarraAppService =  abp.services.app.codigoBarra;
        var _modalManager;
        $('.modal-dialog').css('width', '1000px');

        carregarUnidade();
        var form;
        this.init = function (modalManager) {
            _modalManager = _modalManager;
            form = $("form[name='EtiquetaInformationsForm']");
            form.validate();
        }


        function carregarUnidade() {
            var valor = $('#produtoId').val();
            $("#produtoUnidadeId").empty();

            if (valor != '' && valor != null) {

                $.ajax({
                    url: "/mpa/preMovimentos/SelecionarUnidades/" + valor,
                    success: function (data) {

                        $("#produtoUnidadeId").append('<option value>Selecione um valor</option>');

                        var selected = (data.Items.length == 1) ? " selected='selected' " : "";

                        $.each(data.Items, function (index, element) {
                            $("#produtoUnidadeId").append("<option " + selected + " value='" + element.Id + "'>" + element.Descricao + "</option>");
                        });

                        $('.selectpicker').selectpicker('refresh');
                    }
                });

            }

        };


        $('#btnGerarEtiqueta').click(function () {
            const btn = $(this);
            btn.buttonBusy(true);
            if (!form.valid()) {
                return;
            }
            codigoBarraAppService.gerarEtiquetas($('#quantidade').val(), $('#produtoId').val(), $("#id").val(), $("#produtoUnidadeId").val())
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {

                        abp.notify.info(app.localize('SavedSuccessfully'));
                        const reportParameters = {
                            "LoteValidadeId": $("#id").val(),
                            "Qtd": parseInt($('#quantidade').val()),
                            "DataFracionamento": moment(data.returnObject.dataFacionamento,'DD/MM/YYYY',true).format(),
                            "Modelo": $("#modelo").val()
                        };

                        $.removeCookie("XSRF-TOKEN");
                        printJS({ printable: `/Mpa/Etiquetas/ImprimirEtiqueta?${$.param(reportParameters)}`, type: 'pdf' });
                    }
                }).always(()=> { btn.buttonBusy(false); })

        });

    };
})(jQuery);