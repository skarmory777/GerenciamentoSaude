(function () {
    $(function () {
        var _$kitsTable = $('#KitsTable');
        var _estoqueKitsService = abp.services.app.estoqueKit;
        var _$filterForm = $('#KitsFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.KitEstoque.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.KitEstoque.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.KitEstoque.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/EstoqueKits/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Suprimentos/EstoqueKits/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarEstoqueKitModal'
        });

        _$kitsTable.jtable({
            title: app.localize('Kits'),
            paging: true,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _estoqueKitsService.listar
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '1%',
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
                                    deleteKits(data.record);
                                });

                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '1%'
                },
                descricao: {
                    title: app.localize('Descricao')
                }
            }
        });

        function getKits(reload) {
            if (reload) {
                _$kitsTable.jtable('reload');
            } else {
                _$kitsTable.jtable('load', {
                    filtro: $('#KitsTableFilter').val()
                });
            }
        }

        function deleteKits(kit) {
            abp.message.confirm(
                app.localize('DeleteWarning', kit.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _estoqueKitsService.excluir(kit)
                            .done(function () {
                                getKits(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        $('#CreateNewKitButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarKitsParaExcelButton').click(function () {
            _estoqueKitsService
                .listarParaExcel({
                    filtro: $('#KitsTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetKitsButton, #RefreshKitsListButton').click(function (e) {
            e.preventDefault();
            getKits();
        });

        abp.event.on('app.CriarOuEditarKitModalSaved', function () {
            getKits(true);
        });

        getKits();

        $('#KitsTableFilter').focus();
    });
})();