(function () {
    $(function () {
        var _$MateriaisTable = $('#MateriaisTable');
        var _MateriaisService = abp.services.app.material;
        var _$filterForm = $('#MateriaisFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Material.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Material.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Material.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Materiais/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Materiais/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMaterialModal'
        });

        _$MateriaisTable.jtable({

            title: app.localize('Materiais'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MateriaisService.listar
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
                                    deleteMateriais(data.record);
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

        function getMateriais(reload) {
            if (reload) {
                _$MateriaisTable.jtable('reload');
            } else {
                _$MateriaisTable.jtable('load', {
                    filtro: $('#MateriaisTableFilter').val()
                });
            }
        }

        function deleteMateriais(Material) {

            abp.message.confirm(
                app.localize('DeleteWarning', Material.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MateriaisService.excluir(Material)
                            .done(function () {
                                getMateriais(true);
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

        $('#ExportarMateriaisParaExcelButton').click(function () {
            _MateriaisService
                .listarParaExcel({
                    filtro: $('#MateriaisTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMateriaisButton, #RefreshMateriaisListButton').click(function (e) {
            e.preventDefault();
            getMateriais();
        });

        abp.event.on('app.CriarOuEditarMaterialModalSaved', function () {
            getMateriais(true);
        });

        getMateriais();

        $('#MateriaisTableFilter').focus();


    });
})();