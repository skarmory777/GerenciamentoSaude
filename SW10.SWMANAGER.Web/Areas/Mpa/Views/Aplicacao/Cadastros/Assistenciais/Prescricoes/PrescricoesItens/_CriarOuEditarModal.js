(function ($) {
    app.modals.CriarOuEditarPrescricaoItemModal = function () {
        var _prescricaoItemService = abp.services.app.prescricaoItem;
        var _divisaoService = abp.services.app.divisao;
        var _modalManager;
        var _$formPrescricaoItem = null;
        var tipoFatItem;
        var isMedicamento = false;
        let configuracaoPrescricaoItem;
       
        this.init = function() {
            _$formPrescricaoItem = $('form[name=PrescricaoItemInformationsForm]');
            _$formPrescricaoItem.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '90%', 'max-width': '990px' });
            $('#divisao-id').change();

            configuracaoPrescricaoItem = BuildConfiguracaoPrescricaoItem();

            configuracaoPrescricaoItem.renderSubPrescricaoItem();
        };

        $('.fa-close').on('click', function (e) {
            e.preventDefault();
            location.href = '/mpa/prescricoesitens';
        });

        $('.close-button').on('click', function (e) {
            e.preventDefault();
            location.href = '/mpa/prescricoesitens';
        });

        $('#salvar-item-prescricao').on('click', function (e) {
            e.preventDefault();
            if (!_$formPrescricaoItem.valid()) {
                return;
            }
            var prescricaoItem = _$formPrescricaoItem.serializeFormToObject();
            if (prescricaoItem.Quantidade) {
                prescricaoItem.Quantidade = prescricaoItem.Quantidade.replace(',', '.');
            }

            if (prescricaoItem.MinimoAceitavel) {
                prescricaoItem.MinimoAceitavel = prescricaoItem.MinimoAceitavel.replace(',', '.');
            }
            if (prescricaoItem.MaximoAceitavel) {
                prescricaoItem.MaximoAceitavel = prescricaoItem.MaximoAceitavel.replace(',', '.');
            }
            if (prescricaoItem.MinimoBloqueio) {
                prescricaoItem.MinimoBloqueio = prescricaoItem.MinimoBloqueio.replace(',', '.');
            }
            if (prescricaoItem.MaximoBloqueio) {
                prescricaoItem.MaximoBloqueio = prescricaoItem.MaximoBloqueio.replace(',', '.');
            }

            var formulaEstoque = localStorage["FormulaEstoqueList"];
            var formulaFaturamento = localStorage["FormulaFaturamentoList"];
            var formulaExameLaboratorial = localStorage["FormulaExameLaboratorialList"];
            var formulaExameImagem = localStorage["FormulaExameImagemList"];
            prescricaoItem.FormulaEstoqueList = formulaEstoque;
            prescricaoItem.FormulaFaturamentoList = formulaFaturamento;
            prescricaoItem.FormulaExameLaboratorialList = formulaExameLaboratorial;
            prescricaoItem.FormulaExameImagemList = formulaExameImagem;

           // prescricaoItem.FormulaEstoqueKitJson = localStorage["FormulaEstoqueKitJson"];

            $('#salvar-item-prescricao').buttonBusy(true);
            _prescricaoItemService.criarOuEditar(prescricaoItem)
                .done(function (data) {
                    $("#prescricao-item-id").val(data.id);
                    return configuracaoPrescricaoItem.save().then(function() {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        abp.event.trigger('app.CriarOuEditarPrescricaoItemModalSaved');
                        location.href = '/mpa/prescricoesitens';
                    })
                 })
                .always(function () {
                    $('#btn-save').buttonBusy(false);
                });
        });

        //init();
        CamposRequeridos();
        aplicarDateSingle();
        aplicarDateRange();
        aplicarSelect2Padrao();

        $('#divisao-id').on('change', function (e) {
            e.preventDefault();
            //_divisaoService.obter($(this).val());
            if ($(this).val() != '' && $(this).val() != null) {
                _divisaoService.obter($(this).val())
                .done(function (data) {
                    isMedicamento = data.isMedicamento;
                    if (data.isProdutoEstoque) {
                        $('#div-produto-id').removeClass('hidden');
                        if (!$('#div-faturamento-item-id').hasClass('hidden')) {
                            $('#div-faturamento-item-id').addClass('hidden');
                        }
                    }
                    else {
                        if (!$('#div-produto-id').hasClass('hidden')) {
                            $('#div-produto-id').addClass('hidden');
                        }
                        $('#div-faturamento-item-id').removeClass('hidden');
                        if (data.isExameImagem) {
                            tipoFatItem = 'I';
                        }
                        else if (data.isExameLaboratorial) {
                            tipoFatItem = 'L';
                        }
                        else {
                            tipoFatItem = 'F';
                        }
                    }

                    if (data.isQuantidade) {
                        $('#quantidade').removeAttr('disabled');
                    }
                    else {
                        $('#quantidade').val('').attr('disabled', 'disabled');
                    }
                    if (data.isUnidadeMedida) {
                        $('#unidade-id').removeAttr('disabled');
                    }
                    else {
                        $('#unidade-id').val(null).change().attr('disabled', 'disabled');
                    }
                    if (data.isVelocidadeInfusao) {
                        $('#velocidade-infusao-id').removeAttr('disabled');
                    }
                    else {
                        $('#velocidade-infusao-id').val(null).change().attr('disabled', 'disabled');
                    }
                    if (data.isFormaAplicacao) {
                        $('#forma-aplicacao-id').removeAttr('disabled');
                    }
                    else {
                        $('#forma-aplicacao-id').val(null).change().attr('disabled', 'disabled');
                    }
                    if (data.isFrequencia) {
                        $('#frequencia-id').removeAttr('disabled');
                    }
                    else {
                        $('#frequencia-id').val(null).change().attr('disabled', 'disabled');
                    }
                    if (data.isSeNecessario) {
                        $('#is-se-necessario').removeAttr('disabled');
                    }
                    else {
                        $('#is-se-necessario').removeAttr('checked').attr('disabled', 'disabled');
                    }
                    if (data.isUrgente) {
                        $('#is-urgente').removeAttr('disabled');
                    }
                    else {
                        $('#is-urgente').removeAttr('checked').attr('disabled', 'disabled');
                    }
                    if (data.isAgora) {
                        $('#is-agora').removeAttr('disabled');
                    }
                    else {
                        $('#is-agora').removeAttr('checked').attr('disabled', 'disabled');
                    }
                    if (data.isACM) {
                        $('#is-acm').removeAttr('disabled');
                    }
                    else {
                        $('#is-acm').removeAttr('checked').attr('disabled', 'disabled');
                    }
                    if (data.isDiasAplicacao) {
                        $('#total-dias').removeAttr('disabled');
                    }
                    else {
                        $('#total-dias').val('').attr('disabled', 'disabled');
                    }
                });
            }
        });

        $('#produto-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/produto/ListarMedicamentoPorEstoqueDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        filtros: [$('#estoque-id').val(), isMedicamento]
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
        }).on('change', function (e) {
            if ($('#descricao-prescricao-item').val() === "" || $('#descricao-prescricao-item').val() == undefined) {
                var data = $(this).select2('data');
                var txt = '';
                if (data && data.length > 0) {
                    txt = data[0].text;
                }
                var arr = txt.split(' - ');
                var codigo = arr[0];
                var texto = '';
                for (var i = 1; i < arr.length; i++) {
                    texto += arr[i] + ' ';
                }
                $('#descricao-prescricao-item').val(texto);
                $('#codigo-prescricao-item').val(codigo);
            }
        });

        $('#faturamento-item-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: tipoFatItem == 'I' ? '/api/services/app/faturamentoItem/listarExameImagemDropdown' : tipoFatItem == 'L' ? '/api/services/app/faturamentoItem/listarExameLaboratorialDropdown' : '/api/services/app/faturamentoItem/listarFatItemDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
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
        }).on('change', function (e) {
            if ($('#descricao-prescricao-item').val() === "" || $('#descricao-prescricao-item').val() == undefined) {
                var data = $(this).select2('data');
                var txt = '';
                if (data && data.length > 0) {
                    txt = data[0].text;
                }
                var arr = txt.split(' - ');
                var codigo = arr[0];
                var texto = '';
                for (var i = 1; i < arr.length; i++) {
                    texto += arr[i] + ' ';
                }
                $('#descricao-prescricao-item').val(texto);
                $('#codigo-prescricao-item').val(codigo);
            }
        });

        $('#unidade-id-prescricao-item').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/produtounidade/listarUnidadePorProdutoDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        filtros: [$('#produto-id').val()]
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

        $('#unidade-requisicao-id-prescricao-item').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/produtounidade/listarUnidadePorProdutoDropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                        filtros: [$('#produto-id').val()]
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
    };
})(jQuery);
