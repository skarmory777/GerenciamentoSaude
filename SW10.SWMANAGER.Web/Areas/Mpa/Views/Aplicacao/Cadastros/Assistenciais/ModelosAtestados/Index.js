(function () {
    $(function () {
        var _$ModelosAtestadosTable = $('#ModelosAtestadosTable');
        var _ModelosAtestadosService = abp.services.app.modeloAtestado;
        var _$filterForm = $('#ModelosAtestadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/ModelosAtestados/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/ModelosAtestados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModeloAtestadoModal'
        });

        _$ModelosAtestadosTable.jtable({

            title: app.localize('ModeloAtestado'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ModelosAtestadosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
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
                                    deleteModelosAtestados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                titulo: {
                    title: app.localize('Titulo'),
                    width: '20%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '30%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getModelosAtestados(reload) {
            if (reload) {
                _$ModelosAtestadosTable.jtable('reload');
            } else {
                _$ModelosAtestadosTable.jtable('load', {
                    filtro: $('#ModelosAtestadosTableFilter').val()
                });
            }
        }

        function deleteModelosAtestados(modeloAtestado) {

            abp.message.confirm(
                app.localize('DeleteWarning', modeloAtestado.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ModelosAtestadosService.excluir(modeloAtestado)
                            .done(function () {
                                getModelosAtestados(true);
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

        $('#CreateNewModeloAtestadoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarModelosAtestadosParaExcelButton').click(function () {
            _ModelosAtestadosService
                .listarParaExcel({
                    filtro: $('#ModelosAtestadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetModelosAtestadosButton, #RefreshModelosAtestadosListButton').click(function (e) {
            e.preventDefault();
            getModelosAtestados();
        });

        abp.event.on('app.CriarOuEditarModeloAtestadoModalSaved', function () {
            getModelosAtestados(true);
        });

        getModelosAtestados();

        $('#ModelosAtestadosTableFilter').focus();
    });
})();