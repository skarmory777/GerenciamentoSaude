(function ($) {
    app.modals.CriarOuEditarFaturamentoAutorizacaoModal = function () {

        var _autorizacoesService = abp.services.app.faturamentoAutorizacao;
        var _modalManager;
        var _$autorizacaoInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$autorizacaoInformationForm = _modalManager.getModal().find('form[name=AutorizacaoInformationsForm]');
            _$autorizacaoInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '900px');

            getDetalhes();
        };

        this.save = function () {
            if (!_$autorizacaoInformationForm.valid()) {
                return;
            }

            var autorizacao = _$autorizacaoInformationForm.serializeFormToObject();
            autorizacao.detalhe = JSON.parse($("#detalhes").val())
            autorizacao.detalhes = undefined;
            autorizacao.Mensagem = $('#mensagem').val();

            _modalManager.setBusy(true);

            _autorizacoesService.criarOuEditar(autorizacao)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarAutorizacaoModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        _AutorizacoesService = abp.services.app.faturamentoAutorizacao;
        _$DetalhesTable = $('#detalhes-table');
        _$DetalhesForm = $('#form-autorizacao');

        // Salvar Detalhe
        $('#salvar-detalhe').on('click', function (e) {
            salvarDetalhe();
        });

        function salvarDetalhe() {
            if ($("#detalhes").val() == "") {
                $("#detalhes").val("[]")
            }

            let detalhes = JSON.parse($("#detalhes").val())

            var detalheItem = {
                id: $('#detalheItemId').val() || 0,
                autorizacaoId: $('#autorizacaoId').val() || 0,
                uuid: $('#uuid').val() || uuidv4(),
                ConvenioId: $('#cbo-convenio').val(),
                PlanoId: $('#cbo-plano').val(),
                GrupoId: $('#cbo-grupo').val(),
                SubGrupoId: $('#cbo-subgrupo').val(),
                ItemId: $('#cbo-item').val(),
                UnidadeId: $('#cbo-unidade').val(),
                QtdMinimo: $('#min').val() || 0,
                QtdMaximo: $('#max').val() || 0,
                IsLimiteQtd: $('#is-limiteqtd').swChkValor()
            };

            var key = _.findKey(detalhes, (item) => detalheItem.uuid == item.uuid)

            if (key !== undefined) {
                detalhes[key] = detalheItem;
            } else {
                detalhes.push(detalheItem);
            }

            $("#detalhes").val(JSON.stringify(detalhes));
            limparDetalhe();
            getDetalhes();
        }

        function deleteDetalheItem(record) {
            let uuid = record.uuid;
            if ($("#detalhes").val() == "") {
                $("#detalhes").val("[]")
            }

            let detalhes = JSON.parse($("#detalhes").val())

            let key = _.findKey(detalhes, (item) => uuid == item.uuid)

            if (key !== undefined) {
                detalhes.splice(key, 1);
            }
            $("#detalhes").val(JSON.stringify(detalhes));
            limparDetalhe();
            getDetalhes();
        }

        function limparDetalhe() {
            $('#detalheItemId').val(0);
            $('#uuid').val("")
            $('#uuid').trigger("change");
            $('#cbo-convenio').val("");
            $('#cbo-convenio').trigger("change");
            $('#cbo-plano').val("");
            $('#cbo-plano').trigger("change");
            $('#cbo-grupo').val("")
            $('#cbo-grupo').trigger("change");
            $('#cbo-subgrupo').val("")
            $('#cbo-subgrupo').trigger("change");
            $('#cbo-item').val("")
            $('#cbo-item').trigger("change");
            $('#cbo-unidade').val("")
            $('#cbo-unidade').trigger("change");
            $('#min').val("")
            $('#max').val("")
            $('#is-limiteqtd').swChkValor("")
            $('#is-limiteqtd').trigger("change");
        }
        // Fim de Salvar Detalhe

        // Toggle Max-Min (IsLimiteQtd)
        $('#is-limiteqtd').on('change', function () {
            if ($(this).swChkValor()) {
                $('.qtd').fadeIn();
            } else {
                $('.qtd').fadeOut();
                $('.qtd').val('0');
            }
        });

        // DatePicker
        $('input[name="DataInicial"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker": false,
            "timePicker24Hour": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                $('input[name="DataInicial"]').val(selDate.format('L')).addClass('form-control edited');
            }
        );

        $('input[name="DataFinal"]').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            "timePicker": false,
            "timePicker24Hour": true,
            autoUpdateInput: false,
            maxDate: new Date('10/10/2080'),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
            function (selDate) {
                $('input[name="DataFinal"]').val(selDate.format('L')).addClass('form-control edited');
            }
        );
        // Fim de DatePickers

        // Crud sem modal
        var formDetalhe = {};

        // Atribuir campos da form
        var gerarFormDetalhe = function (_form) {
            formDetalhe.campos = _form.find('input, select');
        };
        gerarFormDetalhe(_$DetalhesForm);

        var setarFormDetalhe = function (registro) {

            // $('#inp-empresa').val(conta.empresaNome);
            if (!formDetalhe.campos)
                return;

            formDetalhe.campos.each(function () {
                var name = $(this).attr('name');
                var nomeServico = $(this).attr('data-servico');
                var servico = abp.services.app[nomeServico];

                if (name) {
                    var prop = name.charAt(0).toLowerCase() + name.slice(1);
                    var valor = registro[prop];
                    $(this).swSetCampo(valor, servico);

                    if (name == 'QtdMinimo') {
                        qtdMinCorrecao(valor);
                    }
                }
            });

            // Correcao 0 saindo 'vazio'
            function qtdMinCorrecao(valor) {
                if (valor == 0) {
                    $('#min').val(0);
                }
            }

        };

        function editDetalheItem(record) {
            debugger;
            $('#detalheItemId').val(record.id);
            $("#autorizacaoId").val(record.autorizacaoId);
            $('#uuid').val(record.uuid);
            $('#uuid').trigger("change");

            changeSelect2('#cbo-convenio', record.convenioId);
            changeSelect2('#cbo-plano', record.planoId);
            changeSelect2('#cbo-grupo', record.grupoId);
            changeSelect2('#cbo-subgrupo', record.subGrupoId);
            changeSelect2('#cbo-item', record.itemId);
            changeSelect2('#cbo-unidade', record.unidadeId);

            $('#min').val(record.qtdMinimo)
            $('#max').val(record.qtdMaximo)
            $('#is-limiteqtd').swSetCampo(record.isLimiteQtd)
            $('#is-limiteqtd').trigger("change");
        }

        function changeSelect2(idSelect2, idValue) {
            $(idSelect2).trigger("select2:selectById", idValue);
        }
        // Fim de SetarForm()
        // Fim de Crud sem modal

        // Checkbox para tipo de atendimento
        $('#is-ambulatorio-emergencia-tab').swChecks2Radio('#is-internacao');

        selectSWWithDefaultValue('.UnidadeSel2', "/api/services/app/unidadeOrganizacional/ListarDropdown", null, { AllowClear: true });
        selectSWWithDefaultValue('.GrupoSel2', "/api/services/app/faturamentoGrupo/ListarDropdown", null, { AllowClear: true });
        selectSWWithDefaultValue('.SubgrupoSel2', "/api/services/app/faturamentoSubGrupo/ListarDropdown", $('.GrupoSel2'), { AllowClear: true });
        selectSWWithDefaultValue('.ItemSel2', "/api/services/app/faturamentoItem/ListarDropdown", null, { AllowClear: true });

        selectSWWithDefaultValue('.ConvenioSel2', "/api/services/app/convenio/ListarDropdown", null, { AllowClear: true });
        selectSWWithDefaultValue('.PlanoSel2', "/api/services/app/plano/ListarDropdown", $('.ConvenioSel2'), { AllowClear: true });
        // JTABLE
        _$DetalhesTable.jtable({

            title: app.localize('Autorizacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            actions: { listAction: { method: _AutorizacoesService.listarDetalhes } },
            // Colunas
            fields: {
                id: { key: true, list: false },

                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        //      if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                editDetalheItem(data.record);
                            });
                        //   }

                        //     if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                deleteDetalheItem(data.record);
                            });
                        //     }

                        return $span;
                    }
                }
                ,
                convenioPlano: {
                    title: app.localize('Convenio'),
                    width: '15%',
                    display: function (data) {
                        var retorno;
                        if (data.record.convenio) {
                            if (data.record.convenio.sisPessoa) {
                                retorno = data.record.convenio.sisPessoa.nomeFantasia;
                            }
                        }
                        if (data.record.plano) {
                            retorno = retorno + '/' + data.record.plano.descricao;
                        }
                        return retorno;
                    }
                }
                ,
                grupoSub: {
                    title: app.localize('Grupo'),
                    width: '15%',
                    display: function (data) {
                        var retorno;
                        if (data.record.grupo) {
                            retorno = data.record.grupo.descricao;
                        }
                        if (data.record.subGrupo) {
                            retorno = retorno + '/' + data.record.subGrupo.descricao;
                        }
                        return retorno;
                    }
                }
                ,
                item: {
                    title: app.localize('Item'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.item) {
                            return data.record.item.descricao;
                        }
                        return '';
                    }
                }
                ,
                unidade: {
                    title: app.localize('Unidade'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.unidade) {
                            return data.record.unidade.descricao;
                        }
                        return '';
                    }
                }
                ,
                isLimite: {
                    title: app.localize('Limite'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.isLimiteQtd) {
                            return 'Min:' + data.record.qtdMinimo + 'Máx:' + data.record.qtdMaximo;   // '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('SemLimite') + '</span>';
                        }
                    }
                }
            }
        });

        // Carregar grid
        function getDetalhes(reload) {
            _$DetalhesTable.jtable('load', { filtro: $("#detalhes").val() });
        }

        function uuidv4() {
            return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }


    };
})(jQuery);