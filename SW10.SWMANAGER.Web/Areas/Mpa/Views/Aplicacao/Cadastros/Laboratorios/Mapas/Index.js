(function () {
    $(function () {
        var _$MapasTable = $('#MapasTable');
        var _MapasService = abp.services.app.mapa;
        var _$filterForm = $('#MapasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Mapa.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Mapa.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Mapa.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Mapas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Mapas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMapaModal'
        });

        _$MapasTable.jtable({

            title: app.localize('Mapas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MapasService.listar
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
                                    deleteMapas(data.record);
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
            }
        });

        function getMapas(reload) {
            if (reload) {
                _$MapasTable.jtable('reload');
            } else {
                _$MapasTable.jtable('load', {
                    filtro: $('#MapasTableFilter').val()
                });
            }
        }

        function deleteMapas(Mapa) {

            abp.message.confirm(
                app.localize('DeleteWarning', Mapa.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MapasService.excluir(Mapa)
                            .done(function () {
                                getMapas(true);
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

        $('#CreateNewMapaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarMapasParaExcelButton').click(function () {
            _MapasService
                .listarParaExcel({
                    filtro: $('#MapasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMapasButton, #RefreshMapasListButton').click(function (e) {
            e.preventDefault();
            getMapas();
        });

        abp.event.on('app.CriarOuEditarMapaModalSaved', function () {
            getMapas(true);
        });

        getMapas();

        $('#MapasTableFilter').focus();


    });
})();