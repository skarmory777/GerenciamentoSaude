(function () {
    $(function () {
        var _$FeriadosTable = $('#FeriadosTable');
        var _FeriadosService = abp.services.app.feriado;
        var _$filterForm = $('#FeriadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Feriado.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Feriado.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Feriado.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Feriados/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Feriados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFeriadoModal'
        });

        _$FeriadosTable.jtable({

            title: app.localize('Feriado'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _FeriadosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '33%',
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
                                    deleteFeriados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                diaMesAno: {
                    title: app.localize('DataFeriado'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.diaMesAno).format('L');
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                }
            }
        });

        function getFeriados(reload) {
            if (reload) {
                _$FeriadosTable.jtable('reload');
            } else {
                _$FeriadosTable.jtable('load', {
                    filtro: $('#FeriadosTableFilter').val()
                });
            }
        }

        function deleteFeriados(feriado) {

            abp.message.confirm(
                app.localize('DeleteWarning', feriado.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _FeriadosService.excluir(feriado)
                            .done(function () {
                                getFeriados(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms);
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

        $('#CreateNewFeriadoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarFeriadosParaExcelButton').click(function () {
            _FeriadosService
                .listarParaExcel({
                    filtro: $('#FeriadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFeriadosButton, #RefreshFeriadosListButton').click(function (e) {
            e.preventDefault();
            getFeriados();
        });

        abp.event.on('app.CriarOuEditarFeriadoModalSaved', function () {
            getFeriados(true);
        });

        getFeriados();

        $('#FeriadosTableFilter').focus();
    });
})();