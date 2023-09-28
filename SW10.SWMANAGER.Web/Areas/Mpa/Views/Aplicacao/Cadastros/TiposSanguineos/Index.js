(function () {
    $(function () {
        var _$TiposSanguineosTable = $('#TiposSanguineosTable');
        var _TiposSanguineosService = abp.services.app.tipoSanguineo;
        var _$filterForm = $('#TiposSanguineosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoSanguineo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposSanguineos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposSanguineos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoSanguineoModal'
        });

        _$TiposSanguineosTable.jtable({

            title: app.localize('TiposSanguineos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposSanguineosService.listar
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
                                    deleteTiposSanguineos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '33%'
                },
                diaMesAno: {
                    title: app.localize('Data'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.diaMesAno).format('L');
                    }
                }
            }
        });

        function getTiposSanguineos(reload) {
            if (reload) {
                _$TiposSanguineosTable.jtable('reload');
            } else {
                _$TiposSanguineosTable.jtable('load', {
                    filtro: $('#TiposSanguineosTableFilter').val()
                });
            }
        }

        function deleteTiposSanguineos(tipoSanguineo) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoSanguineo.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposSanguineosService.excluir(tipoSanguineo)
                            .done(function () {
                                getTiposSanguineos(true);
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

        $('#CreateNewTipoSanguineoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposSanguineosParaExcelButton').click(function () {
            _TiposSanguineosService
                .listarParaExcel({
                    filtro: $('#TiposSanguineosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposSanguineosButton, #RefreshTiposSanguineosListButton').click(function (e) {
            e.preventDefault();
            getTiposSanguineos();
        });

        abp.event.on('app.CriarOuEditarTipoSanguineoModalSaved', function () {
            getTiposSanguineos(true);
        });

        getTiposSanguineos();

        $('#TiposSanguineosTableFilter').focus();
    });
})();