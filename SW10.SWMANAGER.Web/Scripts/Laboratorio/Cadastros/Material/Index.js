(function () {
    $(function () {
        var _$MaterialTable = $('#MaterialTable');
        var _MaterialService = abp.services.app.materialLaboratorio;
        var _$filterForm = $('#MaterialFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Material.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Material.Edit'),
            delete: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Material.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Laboratorio/Cadastros/MaterialCriarOuEditarModal',
            scriptUrl: abp.appPath + 'scripts/laboratorio/cadastros/material/_criaroueditarmodal.js',
            modalClass: 'CriarOuEditarMaterialModal'
        });


        _$MaterialTable.jtable({

            title: app.localize('Material'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MaterialService.paginar
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
                                    deleteMaterial(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                }
            }
        });

        function getMaterial(reload) {
            if (reload) {
                _$MaterialTable.jtable('reload');
            } else {
                _$MaterialTable.jtable('load', {
                    filtro: $('#MaterialTableFilter').val()
                });
            }
        }

        function deleteMaterial(material) {

            abp.message.confirm(
                app.localize('DeleteWarning', material.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MaterialService.excluir(material)
                            .done(function () {
                                getMaterial(true);
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

        $('#CreateNewMaterialButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarMaterialParaExcelButton').click(function () {
            _MaterialService
                .listarParaExcel({
                    filtro: $('#MaterialTableFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMaterialButton, #RefreshMaterialListButton').click(function (e) {
            e.preventDefault();
            getMaterial();
        });

        abp.event.on('app.CriarOuEditarMaterialModalSaved', function () {
            getMaterial(true);
        });

        getMaterial();

        $('#MaterialTableFilter').focus();
    });
})();