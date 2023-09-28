(function () {
    $(function () {
        var _$TiposAtestadosTable = $('#TiposAtestadosTable');
        var _TiposAtestadosService = abp.services.app.tipoAtestado;
        var _$filterForm = $('#TiposAtestadosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Assistencial.AtestadoMedico.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposAtestados/_CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/TiposAtestados/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoAtestadoModal'
        });

        _$TiposAtestadosTable.jtable({

            title: app.localize('TipoAtestado'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposAtestadosService.listar
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
                                    deleteTiposAtestados(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '30%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '20%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getTiposAtestados(reload) {
            if (reload) {
                _$TiposAtestadosTable.jtable('reload');
            } else {
                _$TiposAtestadosTable.jtable('load', {
                    filtro: $('#TiposAtestadosTableFilter').val()
                });
            }
        }

        function deleteTiposAtestados(tipoAtestado) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoAtestado.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposAtestadosService.excluir(tipoAtestado)
                            .done(function () {
                                getTiposAtestados(true);
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

        $('#CreateNewTipoAtestadoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposAtestadosParaExcelButton').click(function () {
            _TiposAtestadosService
                .listarParaExcel({
                    filtro: $('#TiposAtestadosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposAtestadosButton, #RefreshTiposAtestadosListButton').click(function (e) {
            e.preventDefault();
            getTiposAtestados();
        });

        abp.event.on('app.CriarOuEditarTipoAtestadoModalSaved', function () {
            getTiposAtestados(true);
        });

        getTiposAtestados();

        $('#TiposAtestadosTableFilter').focus();
    });
})();