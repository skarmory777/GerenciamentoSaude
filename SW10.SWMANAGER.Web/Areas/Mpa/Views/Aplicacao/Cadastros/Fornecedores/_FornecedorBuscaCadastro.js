(function ($) {

    app.modals.CriarOuEditarTabelaDominioModal = function () {

        var _tabelaDominioService = abp.services.app.tabelaDominio;

        var _modalManager;
        var _$TabelaDominioInformationForm = null;

        var _$tabelaDominioVersoesTissInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$TabelaDominioInformationForm = _modalManager.getModal().find('form[name=TabelaDominioInformationsForm]');
            _$TabelaDominioInformationForm.validate();

            atualizarTabela();

            // Preencher dropdown com ultimo tipo e codigo salvo caso haja (e caso esteja em 'edit mode')
            if (typeof ($('[name=Id').val()) === 'undefined') {
                tipoAutoSet($("#ultimo-tipo-salvo").attr("data"), "#tipo-tabela-dominio-id");
                codigoAutoSet($("#ultimo-codigo-salvo").attr("data"), "#codigo-tabela-dominio-id");
            }

            $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
        };

        this.save = function () {
            if (!_$TabelaDominioInformationForm.valid()) {
                // ...
                abp.notify.info(app.localize('ErroSalvar'));
                return;
            }
            if ($('#tipo-tabela-dominio-id').val() == '') {
                abp.notify.info(app.localize('CampoVazio', app.localize($('#tipo-tabela-dominio-id').attr('name'))));
                return;
            }
            if ($('#filtro-tabela-dominio-grupo-id').val() == '') {
                abp.notify.info(app.localize('CampoVazio', app.localize($('#filtro-tabela-dominio-grupo-id').attr('name'))));
                return;
            }

            var tabelaDominio = _$TabelaDominioInformationForm.serializeFormToObject();
            _modalManager.setBusy(true);

            _tabelaDominioService.criarOuEditar(tabelaDominio)
                 .done(function () {

                     // Manter registro do ultimo tipo e codigo salvo em div: data
                     $("#ultimo-tipo-salvo").attr("data", $("#tipo-tabela-dominio-id option:selected").text());
                     $("#ultimo-codigo-salvo").attr("data", $("#codigo-tabela-dominio-id").val());

                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarTabelaDominioModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('#btn-nova-versao-tiss').click(function (e) {
            e.preventDefault()
            $('#tabela-dominio-versao-tiss-parcial').load('/TabelasDominio/_CriarOuEditarTabelaDominioVersaoTissModal?tabelaDominioId=' + $('#id').val());
        });

        function atualizarTabela() {
            $('#TabelaDominioVersoesTissTable').load('/TabelasDominio/_TabelaDominioVersoesTiss?id=' + $('#id').val());
        }

        $('#tipo-tabela-dominio-id').on('change', function () {
            updateCombos();
        });

        function updateCombos() {
            var _tabelaDominioId = $('#tipo-tabela-dominio-id').length > 0 ? $('#tipo-tabela-dominio-id').val() : 0;
            var grupoTabelaDominioId = $('#filtro-tabela-dominio-grupo-id').length > 0 ? $('#filtro-tabela-dominio-grupo-id').val() : 0;
            $('#grupos-tipo-tabela-dominio').load(
                '/Mpa/TabelasDominio/_MontarComboTabelaDominioGrupos/',
                {
                    tabelaDominioId: _tabelaDominioId
                },
                function () {
                    $('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' }).trigger("chosen:updated").addClass('edited');
                });
        };

        // Atribuindo ultimo Tipo salvo ao Dropdown
        function tipoAutoSet(ultimoTipoSalvo, dropdown) {
            if (ultimoTipoSalvo == "nenhum") {
                $(dropdown).selectedIndex = -1;
            }
            else {
                $(dropdown + " option:contains(" + ultimoTipoSalvo + ")").attr('selected', 'selected');
                updateCombos();
            }
        };

        // Atribuindo ultimo Codigo salvo ao input
        function codigoAutoSet(ultimoCodigoSalvo, input) {

            // Falta checar se codigo ja existe

            if (ultimoCodigoSalvo != "nenhum") {
                if (~ultimoCodigoSalvo.indexOf(".")) {
                    var casasDecimais = (parseInt(ultimoCodigoSalvo.substr(ultimoCodigoSalvo.indexOf(".") + 1)) + 1).toString();
                    var parteInteira = (parseInt(ultimoCodigoSalvo.substr(0, ultimoCodigoSalvo.indexOf('.'))));
                    if (casasDecimais == "100") {
                        ultimoCodigoSalvo = (parteInteira + 1).toString();
                    } else {
                        ultimoCodigoSalvo = parteInteira + "." + casasDecimais;
                    }
                } else {
                    ultimoCodigoSalvo = ultimoCodigoSalvo + ".1";
                }

                $(input).val(ultimoCodigoSalvo).addClass('edited');
            }
        };

    };
})(jQuery);
