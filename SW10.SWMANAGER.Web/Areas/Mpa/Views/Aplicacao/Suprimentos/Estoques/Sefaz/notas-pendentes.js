
(function ($) {
    app.modals.sefazNotasPendentes = function () {

        const sefazService = abp.services.app.sefaz;

        const dateRangeOptions = app.createDateRangePickerOptions();
        const defaultRange = dateRangeOptions.ranges[app.localize('Last30Days')];

        const _selectedDateRange = {
            startDate: defaultRange[0],
            endDate: defaultRange[1]
        };
        
        const _$filterForm = $('#PreMovimentoFilterForm');
        const _$notasPendentesTable = $('#notasPendentesTable');
        let modalManagerNotasPendentes;
        this.init = function (modalManager) {
            $('.modal-dialog').css('width', '90%');
            modalManagerNotasPendentes = modalManager;
            getNotasPendentesTable();
            let model = modalManagerNotasPendentes.getArgs();
            selectSW({
                classe: '.selectForncedor-pendentes',
                url: "/api/services/app/sefaz/ListarFornecedores",
                dataHandler: (data, properties) => {
                    data.empresaId = model.empresaId;
                    return data;
                },
                onChange: () => {
                    getNotasPendentesTable();
                }
            });

            

            _$filterForm.find('input.date-range-picker').daterangepicker(
                $.extend(true, dateRangeOptions, _selectedDateRange),
                function (start, end, label) {
                    _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                    _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                    getNotasPendentesTable();
                }
            ).on('apply.daterangepicker',
                function (ev, picker) {
                    getNotasPendentesTable();
                })
                .on('cancel.daterangepicker', function (ev, picker) {
                _selectedDateRange.startDate = null;
                _selectedDateRange.endDate = null;
                $(this).val('');
                getNotasPendentesTable();
            });
            _$notasPendentesTable.focus();

            $(".filtro-texto-pendentes").on('keyup', _.debounce(function (e) {
                getNotasPendentesTable();
            }, 500));
        }

        _$notasPendentesTable.jtable({

            title: app.localize('Notas Pentendes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: sefazService.listarNotasPendentes
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                escolherNota: {
                    title: '',
                    width: '1%',
                    sorting: false,
                    display: function (data) {
                        let $span = $('<span style="display:flow-root;text-align:center;"></span>');
                        $span.append(`<button class="btn" style="background-color:#337ab7;padding: 2px 4px;" data-toggle="tooltip" title="Selecionar Nota"><i class="fa fa-cloud-download" style="color:white;margin-top: 3px;font-size: 12px;"></i></button>`)

                        $span.find("button").click(function (event) {
                            abp.event.trigger("selecionaNotaPendente", data.record.chaveNfe);
                            modalManagerNotasPendentes.close();
                        });
                        return $span;
                    }
                },
                "dataEmissao": {
                    title: app.localize('Emissao'),
                    width: '12%',
                    display: function (data) {
                        if (data.record.dataEmissao) {
                            return moment(data.record.dataEmissao).format('DD/MM/YYYY HH:mm:ss');
                        }
                    }
                },
                "emitente": {
                    title: app.localize('Emitente'),
                    width: '30%',
                    display: function (data) {
                        return data.record.emitente;
                    }
                },
                "identificadorEmitente": {
                    title: app.localize('CPF / CNPJ'),
                    width: '12%',
                    display: function (data) {
                        return data.record.identificadorEmitente;
                    }
                },
                "serie": {
                    title: app.localize('Serie'),
                    width: '5%',
                    display: function (data) {
                        return data.record.serie;
                    }
                },
                "numeroNota": {
                    title: app.localize('Número'),
                    width: '7%',
                    display: function (data) {
                        return data.record.numeroNota;
                    }
                },
                "chaveNfe": {
                    title: app.localize('Chave'),
                    width: '22%',
                    display: function (data) {
                        return data.record.chaveNfe;
                    }
                },
                "valorNota": {
                    title: app.localize('Valor'),
                    width: '11%',
                    display: function (data) {
                        if (data.record.valorNota == null || data.record.valorNota == undefined) {
                            return;
                        }

                        return posicionarDireita(data.record.valorNota.toLocaleString("pt-BR", { style: "currency", currency: "BRL" }));
                    }
                }
            }

        });

        _$notasPendentesTable.focus();


        function getNotasPendentesTable(reload) {
            const model = modalManagerNotasPendentes.getArgs();
            model.fornecedor = $('.selectForncedor-pendentes').val();
            model.startDate = _selectedDateRange.startDate;
            model.endDate = _selectedDateRange.endDate;
            model.filtro = $(".filtro-texto-pendentes").val();


            if (reload) {
                _$notasPendentesTable.jtable('reload');
            } else {
                _$notasPendentesTable.jtable('load', model);
            }
        }
    };
})(jQuery);