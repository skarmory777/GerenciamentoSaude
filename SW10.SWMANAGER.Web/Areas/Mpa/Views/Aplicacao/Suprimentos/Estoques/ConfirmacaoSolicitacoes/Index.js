(function () {
    $(function () {
        $('.modal-dialog').css('width', '1800px');

        var _$PreMovimentoTable = $('#PreMovimentoTable');
        var _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _$filterForm = $('#PreMovimentoFilterForm');
        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        let _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };


        $(".buscarPorSolicitacao").focus();

        _$PreMovimentoTable.jtable({

            title: app.localize('Confirmacao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _preMovimentoService.listarSolicitacoes
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },

                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit && data.record.preMovimentoEstadoId != 6 && data.record.preMovimentoEstadoId != 7) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    exibirMovimentacaoParaConfirmacao(data.record);
                                });
                        }

                        return $span;
                    }
                },
                "PreMovimentoEstadoId": {
                    title: app.localize('Status'),
                    width: '6%',
                    display: function (data) {
                        switch (data.record.preMovimentoEstadoId) {
                            case 1: {
                                return '<span class="label label-info">' + app.localize('Aguardando Confirmação') + '</span>';
                            }
                            case 2: {
                                return '<span class="label label-success">' + app.localize('Confirmado') + '</span>';
                            }
                            case 3: {
                                return '<span class="label label-info">' + app.localize('Pendente informação') + '</span>';
                            }
                            case 4: {
                                return '<span class="label label-info">' + app.localize('Pendente') + '</span>';
                            }
                            case 5: {
                                return '<span class="label label-warning">' + app.localize('Parcialmente Atendido') + '</span>';
                            }
                            case 6: {
                                return '<span class="label label-success">' + app.localize('Totalmente Atendido') + '</span>';
                            }
                            case 7: {
                                return '<span class="label label-danger">' + app.localize('Parcialmente Suspensa') + '</span>';
                            }
                            case 8: {
                                return '<span class="label label-danger">' + app.localize('Suspensa') + '</span>';
                            }
                            default: {
                                return '';
                            }
                        }
                    }
                },
                TipoOperacao: {
                    title: app.localize('TipoOperacao'),
                    width: '8%',
                    display: function (data) {
                        return data.record.tipoOperacao;
                    }
                },
                TipoMovimento: {
                    title: app.localize('TipoMovimento'),
                    width: '10%',
                    display: function (data) {
                        return data.record.tipoMovimento;
                    }
                },
                Documento: {
                    title: app.localize('Solicitacao'),
                    width: '8%',
                    display: function (data) {
                        return data.record.documento;
                    }
                },
                "dataEmissaoSaida": {
                    title: app.localize('Emissao'),
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.dataEmissaoSaida).format('L');
                    }
                },
                NomePaciente: {
                    title: app.localize('Paciente'),
                    width: '15%',
                    display: function (data) {
                        return data.record.nomePaciente;
                    }
                },

                HoraPrescrita: {
                    title: app.localize('HoraPrescrita'),
                    width: '10%',
                    display: function (data) {
                        if (data.record.horaPrescrita) {
                            return moment(data.record.horaPrescrita).format('L HH:mm');
                        }
                    }
                },

                Empresa: {
                    title: app.localize('Empresa'),
                    width: '15%',
                    display: function (data) {
                        return data.record.empresa;
                    }
                },
                "UsuarioId": {
                    title: app.localize('Usuario'),
                    width: '10%',
                    display: function (data) {
                        return data.record.usuario;
                    }
                }
            }

        });

        function getPreMovimentos(reload) {
            if (reload) {
                _$PreMovimentoTable.jtable('reload');
            } else {
                _$PreMovimentoTable.jtable('load', {
                    filtro: $('#PreMovimentoTableFilter').val(),
                    tipoMovimentoId: $('#TipoMovimentoId').val(),
                    peridoDe: _selectedDateRange.startDate,
                    peridoAte: _selectedDateRange.endDate, 
                    //tipoOperacaoId: $('#EstTipoOperacaoId').val(),
                    statusMovimentoIds: $('#StatusMovimentoIds').val(),
                    isEntrada: false,
                    estoqueId: $('#estoqueId').val(),
                    tipoOperacaoId: $('.selectTipoSolicitacao').val(),
                    documento: $('#txtDocumento').val()
                });
            }
        }

        function getLocalStoragePreMovimentos() {
            var filter = {
                filtro: $('#PreMovimentoTableFilter').val(),
                tipoMovimentoId: getValueFromLocalStorage('selectTipoMovimento'),
                tipoOperacaoId: getValueFromLocalStorage('selectTipoOperacoes'),
                estoqueId: getValueFromLocalStorage('selectEstoque'),
                documento: $('#txtDocumento').val()
            };

            if (getValueFromLocalStorage('selectStatusMovimentoIds')) {
                let ids = getFromLocalStorage('selectStatusMovimentoIds');
                if (ids.indexOf(',') !== -1) {
                    ids = ids.split(',').map(x => { return parseInt(x, 10) });
                } else {
                    ids = [parseInt(ids, 10)];
                }
                filter.statusMovimentoIds = ids
            }

            if (getValueFromLocalStorage('date')) {
                let date = JSON.parse(getValueFromLocalStorage('date'));
                filter.peridoDe = date.start;
                filter.peridoAte = date.end;
            }
            else {
                filter.peridoDe = _selectedDateRange.startDate;
                filter.peridoAte = _selectedDateRange.endDate;
            }

            _$PreMovimentoTable.jtable('load', filter);
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewPreMovimentoButton').click(function () {
            location.href = 'Saidas/CriarOuEditarModal/';
        });

        $('#RefreshAtendimentosButton').click(function (e) {
            e.preventDefault();
            getPreMovimentos();

        });

        $(".selectTipoSolicitacao").select2({ placeholder: "Informe um tipo de solicitacao" }).on("change", function (event) {
            event.preventDefault();
            $('.selectTipoMovimento').val(null).trigger("select2.change");
            switch ($(".selectTipoSolicitacao").val()) {
                case "3": {
                    selectSWWithDefaultValue('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownSolicitacaoSaida");
                    atualizarSelectTipoMovimento();
                    break;
                }
                case "1": {
                    selectSWWithDefaultValue('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownEntrada");
                    atualizarSelectTipoMovimento();
                    break;
                }
                case "4": {
                    selectSWWithDefaultValue('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownDevolucao");
                    atualizarSelectTipoMovimento();
                    break;
                }
                default: {
                    $('.selectTipoMovimento').select2("destroy");
                    atualizarSelectTipoMovimento();
                }
            }
            localStorage['confirmacao_solicitacao[selectTipoSolicitacao]'] = $(".selectTipoSolicitacao").val();
        });

        _$filterForm.find('input.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                localStorage['confirmacao_solicitacao[date_choosenLabel]'] = _$filterForm.find('input.date-range-picker').data('daterangepicker')['chosenLabel']
                localStorage['confirmacao_solicitacao[date]'] = JSON.stringify({
                    start: start.format('DD/MM/YYYY'),
                    end: end.format('DD/MM/YYYY')
                });
            }
        );
                
        function exibirMovimentacaoParaConfirmacao(data) {
            if (_permissions.edit && data.preMovimentoEstadoId != 6 && data.preMovimentoEstadoId != 7) {
                location.href = 'ConfirmacaoSolicitacoes/ConfirmarSolicitacao/' + data.id;
            }
            else {
                abp.notify.error("Não é possível acessar a solicitação, pois ela esta bloqueada para acesso.");
            }
        }

        selectSWWithDefaultValue('.selectStatusMovimentoIds', "/api/services/app/estoquePreMovimento/ListarDropdownPreMovimentoEstado");

        $('.selectStatusMovimentoIds').on("change", (event) => {
            localStorage['confirmacao_solicitacao[selectStatusMovimentoIds]'] = $('.selectStatusMovimentoIds').val();
        })

        selectSWWithDefaultValue('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");

        $(".selectEstoque").on("change", (event) => {
            localStorage['confirmacao_solicitacao[selectEstoque]'] = $('.selectEstoque').val();
        });

        selectSWWithDefaultValue('.selectTipoOperacoes', "/api/services/app/tipooperacao/ListarDropdown");

        $(".selectTipoOperacoes").on("change", (event) => {
            localStorage['confirmacao_solicitacao[selectTipoOperacoes]'] = $('.selectTipoOperacoes').val();
        });

        $(".buscarPorSolicitacao").on('keypress', function (event) {

            if (event.which === 13) {
                abp.ui.setBusy();
                let codigo = $(this).val();
                if (codigo == null || codigo == "" || codigo == undefined) {
                    abp.notify.error("Não foi possível achar a solicitação correspondente a etiqueta");
                    abp.ui.clearBusy()
                    return;
                }
                _preMovimentoService.buscarPorSolicitacao(codigo).then(res => {
                    if (!res) {
                        abp.notify.error("Não foi possível achar a solicitação correspondente a etiqueta");
                        return;
                    }
                    exibirMovimentacaoParaConfirmacao(res);

                }).always(() => {
                    abp.ui.clearBusy()
                });
            }
        });


        function atualizarSelectTipoMovimento() {
            if (getValueFromLocalStorage('selectTipoMovimento')) {
                $(".selectTipoMovimento").trigger("select2:selectById", getValueFromLocalStorage('selectTipoMovimento'));
            } else {
                $('.selectTipoMovimento').val(null).trigger("select2.change");
            }
            
            $('.selectTipoMovimento').on('change', (event) => {
                localStorage['confirmacao_solicitacao[selectTipoMovimento]'] = $('.selectTipoMovimento').val();
            })
        }

        function getValueFromLocalStorage(id) {
            return getFromLocalStorage(id) && getFromLocalStorage(id) != 'null' ? getFromLocalStorage(id) : null;
        }

        function getFromLocalStorage(id) {
            return localStorage[`confirmacao_solicitacao[${id}]`];
        }

        function atualizarFiltros() {
            if (getValueFromLocalStorage('selectTipoSolicitacao')) {
                $(".selectTipoSolicitacao").val(getValueFromLocalStorage('selectTipoSolicitacao')).trigger("change");
            }

            if (getValueFromLocalStorage('date')) {
                let chosenLabel = getValueFromLocalStorage('date_choosenLabel');
                let dateVal;
                if(chosenLabel !== 'Intervalo personalizado') {
                    const datas = _$filterForm.find('input.date-range-picker').data('ranges')[chosenLabel];

                    dateVal = datas[0].format('DD/MM/YYYY') + ' - ' + datas[1].format('DD/MM/YYYY')
                    _selectedDateRange.startDate = datas[0].format('YYYY-MM-DDT00:00:00Z');
                    _selectedDateRange.endDate = datas[1].format('YYYY-MM-DDT23:59:59.999Z');
                } else {
                    let date = JSON.parse(getValueFromLocalStorage('date'));
                    dateVal = date.start + ' - ' + date.end;
                    _selectedDateRange.startDate = date.start;
                    _selectedDateRange.endDate = date.end;
                }
                _$filterForm.find('input.date-range-picker').val(dateVal).trigger('change');
            }

            if (getValueFromLocalStorage('selectStatusMovimentoIds')) {
                let ids = getValueFromLocalStorage('selectStatusMovimentoIds');
                if (ids.indexOf(',') !== -1) {
                    ids = ids.split(',').map(x => { return { id: parseInt(x, 10) } });
                } else {
                    ids = { id: parseInt(ids, 10) };
                }
                $('.selectStatusMovimentoIds').val(ids);
                $('.selectStatusMovimentoIds').trigger('select2:selectByIds', ids);
            }

            if (getValueFromLocalStorage('selectEstoque')) {
                $(".selectEstoque").trigger("select2:selectById", getValueFromLocalStorage('selectEstoque'))
            }

            if (getValueFromLocalStorage('selectTipoOperacoes')) {
                $(".selectTipoOperacoes").trigger("select2:selectById", getValueFromLocalStorage('selectTipoOperacoes'));
            }

            atualizarSelectTipoMovimento();
        }

        atualizarFiltros();
        getLocalStoragePreMovimentos();
        $('#EntradasTableFilter').focus();
    });
})();