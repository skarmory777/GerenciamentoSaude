(function () {
    $(function () {
        var _$KitsExamesTable = $('#KitsExamesTable');
        var _KitsExamesService = abp.services.app.kitExame;
        var _$filterForm = $('#KitsExamesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.KitExame.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.KitExame.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.KitExame.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/KitsExames/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/KitsExames/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarKitExameModal'
        });

        _$KitsExamesTable.jtable({

            title: app.localize('KitsExames'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _KitsExamesService.listar
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
                                    deleteKitsExames(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Código'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
                regConselho: {
                    title: app.localize('Registro Conselho'),
                    width: '15%'
                },
            }
        });

        function getKitsExames(reload) {
            if (reload) {
                _$KitsExamesTable.jtable('reload');
            } else {
                _$KitsExamesTable.jtable('load', {
                    filtro: $('#KitsExamesTableFilter').val()
                });
            }
        }

        function deleteKitsExames(KitExame) {

            abp.message.confirm(
                app.localize('DeleteWarning', KitExame.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _KitsExamesService.excluir(KitExame)
                            .done(function () {
                                getKitsExames(true);
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

        $('#CreateNewKitExameButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarKitsExamesParaExcelButton').click(function () {
            _KitsExamesService
                .listarParaExcel({
                    filtro: $('#KitsExamesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetKitsExamesButton, #RefreshKitsExamesListButton').click(function (e) {
            e.preventDefault();
            getKitsExames();
        });

        abp.event.on('app.CriarOuEditarKitExameModalSaved', function () {
            getKitsExames(true);
        });

        getKitsExames();

        $('#KitsExamesTableFilter').focus();


    });
})();