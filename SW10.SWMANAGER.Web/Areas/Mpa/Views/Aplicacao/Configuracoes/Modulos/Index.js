(function () {
    $(function () {
        var _$ModulosTable = $('#ModulosTable');
        var _ModulosService = abp.services.app.modulo;
        var _$filterForm = $('#ModulosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Configuracoes.Modulo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Configuracoes.Modulo.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Configuracoes.Modulo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Modulos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/Modulos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModuloModal'
        });

        _$ModulosTable.jtable({

            title: app.localize('Modulos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ModulosService.listar
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
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteModulos(data.record);
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

            }
        });

        function getModulos(reload) {
            if (reload) {
                _$ModulosTable.jtable('reload');
            } else {
                _$ModulosTable.jtable('load', {
                    filtro: $('#ModulosTableFilter').val()
                });
            }
        }

        function deleteModulos(Modulo) {

            abp.message.confirm(
                app.localize('DeleteWarning', Modulo.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ModulosService.excluir(Modulo)
                            .done(function () {
                                getModulos(true);
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

        $('#CreateNewModuloButton').click(function (e) {
            e.preventDefault();
            _createOrEditModal.open();
        });

        $('#ExportarModulosParaExcelButton').click(function () {
            _ModulosService
                .listarParaExcel({
                    filtro: $('#ModulosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetModulosButton, #RefreshModulosListButton').click(function (e) {
            e.preventDefault();
            getModulos();
        });

        abp.event.on('app.CriarOuEditarModuloModalSaved', function () {
            getModulos(true);
        });

        getModulos();

        $('#ModulosTableFilter').focus();
    });
})();