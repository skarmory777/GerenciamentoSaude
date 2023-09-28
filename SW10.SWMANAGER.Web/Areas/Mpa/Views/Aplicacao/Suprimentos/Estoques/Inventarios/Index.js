(function () {

    $(function () {


        selectSW('.selectEstoque', "/api/services/app/estoque/ListarDropdown");



        var _$filterForm = $('#InventarioIndexFilterForm');

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        _$filterForm.find('input.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                getInventarioTable();
            });

        $(".selectEstoque").change((e) => {
            getInventarioTable();
        });

        $('#createNewInventarioButton').click(function () {
            location.href = 'Inventarios/CriarOuEditarModal/';
            //_createOrEditModal.open();
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        var _$InventariosTable = $('#inventariosTable');
        var _InventarioreMovimentoService = abp.services.app.inventario;
        // var _$filterForm = $('#PreMovimentoFilterForm');

        $(".Inventario .loader").css("display", "none");


        _$InventariosTable.jtable
            ({
                title: app.localize('Inventarios'),
                paging: true,
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,
                actions:
                {
                    listAction:
                    {
                        method: _InventarioreMovimentoService.listar
                    },
                },
                fields:
                {
                    id: {
                        key: true,
                        list: false
                    },
                    actions: {
                        title: app.localize('Actions'),
                        width: '11%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');

                            //if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    location.href = 'inventarios/CriarOuEditarModal/' + data.record.id;

                                });

                            if (data.record.statusId != 4) //Diferente de Fechado
                            {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Fechar') + '"><i class="fa fa-close"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        abp.message.confirm(
                                            "Deseja fechar o Inventário?",
                                            function (isConfirmed) {
                                                if (isConfirmed) {
                                                    $(".grid").css("display", "none");
                                                    $(".Inventario .loader").css("display", "block");
                                                    _InventarioreMovimentoService.fecharInventario(data.record.id)
                                                        .done(function (data) {
                                                            if (data.errors.length > 0) {
                                                                _ErrorModal.open({ erros: data.errors });
                                                            }
                                                            else {
                                                                abp.notify.info(app.localize('SavedSuccessfully'));

                                                                $(".Inventario .loader").css("display", "none");
                                                                $(".Inventario .container-content").css("display", "block");

                                                                $(".grid").css("display", "block");
                                                                getInventarioTable();
                                                            }
                                                        });
                                                }
                                            }
                                        );
                                    });
                            }

                            if (data.record.statusId != 4) //Diferente de Fechado
                            {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Lista') + '"><i class="fa fa-print"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        const reportUrl = 'ListaInventario';
                                        const reportParameters = {
                                            "InventarioId": data.record.id,
                                            "Estoque": data.record.estoque,
                                            "NumeroInventario": data.record.numero,
                                            "Dominio": abp.multiTenancy.currentTenancyName,
                                        };
                                        abp.services.app.jasperReports.relatorioUrl(reportUrl, JSON.stringify(reportParameters)).then(res => {
                                            window.open(res);
                                        });
                                    });
                            }


                            return $span;
                        }
                    },

                    Estoque: {
                        title: app.localize('Estoque'),
                        width: '30%',
                        display: function (data) {
                            if (data.record.estoque) {
                                return data.record.estoque;
                            }
                        }
                    },
                    Numero: {
                        title: app.localize('Numero'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.numero) {
                                return data.record.numero;
                            }
                        }
                    },

                    DataInventario: {
                        title: app.localize('Data'),
                        width: '10%',
                        display: function (data) {
                            if (data.record.dataInventario) {
                                return moment(data.record.dataInventario).format('L');
                            }
                        }
                    },
                    Status: {
                        title: app.localize('Status'),
                        width: '10%',
                        display: function (data) {
                            return data.record.status;
                        }
                    },


                }
            });


        function getInventarioTable(reload) {

            if (reload) {
                _$InventariosTable.jtable('reload');
            } else {
                _$InventariosTable.jtable('load', {
                    estoqueID: $('#estoqueId').val(),
                    startDate: _selectedDateRange.startDate,//  $('#PeridoDe').val(),
                    endDate: _selectedDateRange.endDate,
                });
            }
        }


        $('#RefreshAtendimentosButton').click(function (e) {

            e.preventDefault();
            getInventarioTable();

        });

        getInventarioTable();

    });
})();