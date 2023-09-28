(function () {
    $(function () {
        var _$resultadosExamesTable = $('#ResultadosExamesTable');
        var _resultadosExamesService = abp.services.app.resultado;
        var _$resultadosExamesFilterForm = $('#ResultadosExamesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.ResultadoExame.Delete')
        };

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        $('#date-field').daterangepicker($.extend(true, app.createDateRangePickerOptions(), _selectedDateRange), function (start, end, label) {
            _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
            _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
        });


        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ResultadosExames/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ResultadosExames/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarResultadoExameModal'
        });

        _$resultadosExamesTable.jtable({

            title: app.localize('ResultadosExames'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true, //Enable selecting
            multiselect: false, //Allow multiple selecting
            selectingCheckboxes: true, //Show checkboxes on first column

            actions: {
                listAction: {
                    method: _resultadosExamesService.listar
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
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteResultadosExames(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                },
                tecnicoId: {
                    title: app.localize('Tecnico'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.tecnico != null && data.record.tecnico != undefined) {
                            return data.record.tecnico.descricao;
                        }
                        else {
                            return data.record.tecnicoId;
                        }
                    }
                },
                empresa: {
                    title: app.localize('Empresa'),
                    width: '15%'
                }
            },
            selectionChanged: function () {
                //Get all selected rows
                var $selectedRows = _$resultadosExamesTable.jtable('selectedRows');
                //getContas();
                if ($selectedRows.length > 0) {
                    //Show selected rows
                    $selectedRows.each(function () {
                        var record = $(this).data('record');
                        $('#resultado-id-exame').val(record.id);
                        getExames();
                        $('#exibir-sw-div-retratil-exames-table').click();
                    });
                }
                else {
                    $('#resultado-id-exame').val('');
                    getExames();
                    $('#omitir-sw-div-retratil-exames-table').click();
                }
            },
        });

        function getResultadosExames(reload) {
            if (reload) {
                _$resultadosExamesTable.jtable('reload');
            } else {
                _$resultadosExamesTable.jtable('load', {
                    filtro: $('#ResultadosExamesTableFilter').val(),
                    faturamentoContaId: $('#conta-id').val(),
                });
            }
        }

        function deleteResultadosExames(ResultadoExame) {

            abp.message.confirm(
                app.localize('DeleteWarning', ResultadoExame.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _resultadosExamesService.excluir(ResultadoExame)
                            .done(function () {
                                getResultadosExames(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$contasFilterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        $('#tipo-filtro-data').on('change', function (e) {
            e.preventDefault();
            $('#filtro-data').val($(this).val())
            switch ($(this).val()) {
                case "Conta":
                    $('#date-field-area').removeClass('hidden');
                    break;
                case "Internado":
                    $('#date-field-area').addClass('hidden');
                    break;
                default:
                    $('#date-field-area').removeClass('hidden');
            }
        });

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedContasFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewResultadoExameButton').click(function () {
            _createOrEditModal.open({ contaId: $('#conta-id').val() });
        });

        $('#ExportarResultadosExamesParaExcelButton').click(function () {
            _resultadosExamesService
                .listarParaExcel({
                    filtro: $('#ResultadosExamesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetResultadosExamesButton, #RefreshResultadosExamesListButton').click(function (e) {
            e.preventDefault();
            getResultadosExames();
        });

        abp.event.on('app.CriarOuEditarResultadoExameModalSaved', function () {
            getResultadosExames(true);
        });

        getResultadosExames();

        $('#ResultadosExamesTableFilter').focus();

        minimizarMenu();
    });
})();