(function ($) {
    app.modals.CriarOuEditarSubDivisaoModal = function () {

        var _divisoesService = abp.services.app.divisao;

        var _modalManager;
        var _$SubDivisaoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$SubDivisaoInformationForm = _modalManager.getModal().find('form[name=SubDivisaoInformationsForm]');
            _$SubDivisaoInformationForm.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
        };

        this.save = function () {
            if (!_$SubDivisaoInformationForm.valid()) {
                return;
            }
            var divisao = _$SubDivisaoInformationForm.serializeFormToObject();
            //divisao.TiposRespostasSelecionadas = '';
            //$('input[name="TiposRespostasSelecionadasSub"]:checked').each(function () {
            //    divisao.TiposRespostasSelecionadas += $(this).val() + ",";
            //});
            //if (divisao.TiposRespostasSelecionadas == '') {
            //    abp.notify.warn(app.localize('SelecaoObrigatoria'));
            //    return;
            //}
            //divisao.TiposRespostasSelecionadas = divisao.TiposRespostasSelecionadas.substring(0, divisao.TiposRespostasSelecionadas.length - 1);
            if ($('.chk-montagem-tela-sub:checked').length == 0) {
                $('#lnk-montagem-tela-tab-sub').trigger('click');
                abp.notify.warn(app.localize('SelecaoObrigatoria'));
                return;
            }
            if ($('.chk-configuracao-sub:checked').length == 0) {
                $('#lnk-configuracao-tab-sub').trigger('click');
                abp.notify.warn(app.localize('SelecaoObrigatoria'));
                return;
            }
            _modalManager.setBusy(true);
            _divisoesService.criarOuEditar(divisao)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarDivisaoModalSaved');
                     abp.event.trigger('app.CriarOuEditarSubDivisaoModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        $('#is-todos-montagem-tela-sub').on('click', function (e) {
            if ($(this).is(':checked')) {
                //$('input[name=TiposRespostasSelecionadas]').attr('checked', 'checked');
                $('.chk-montagem-tela-sub').attr('checked', 'checked');
            }
            else {
                $('.chk-montagem-tela-sub').removeAttr('checked');
            }
        });

        $('#is-todos-configuracao-sub').on('click', function (e) {
            if ($(this).is(':checked')) {
                //$('input[name=TiposRespostasSelecionadas]').attr('checked', 'checked');
                $('.chk-configuracao-sub').attr('checked', 'checked');
            }
            else {
                $('.chk-configuracao-sub').removeAttr('checked');
            }
        });

        $("#tipo-prescricao-id-sub").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/tipoPrescricao/listarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    //   //console.log('data: ', params, (params.page == undefined));
                    if (params.page == undefined)
                        params.page = '1';
                    //   //console.log('data: ', params);
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        });
        
        function contaChk() {
            var chkMontagem = $('.chk-montagem-tela');
            var chkMontagemChecked = $('.chk-montagem-tela:checked');
            var chkConfiguracao = $('.chk-configuracao');
            var chkConfiguracaoCheked = $('.chk-configuracao:checked');
            if (chkMontagem.length == chkMontagemChecked.length) {
                $('#is-todos-montagem-tela').attr('checked', 'checked');
            }
            else {
                $('#is-todos-montagem-tela').removeAttr('checked');
            }
            if (chkConfiguracao.length == chkConfiguracaoCheked.length) {
                $('#is-todos-configuracao').attr('checked', 'checked');
            }
            else {
                $('#is-todos-configuracao').removeAttr('checked');
            }
        }

        $('.chk-montagem-tela-sub').click(function (e) {
            contaChk();
        });

        $('.chk-configuracao-sub').click(function (e) {
            contaChk();
        });

        contaChk();
    };
})(jQuery);