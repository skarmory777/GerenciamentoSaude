(function () {
    $(function () {
        var _$TiposLogradouroTable = $('#TiposLogradouroTable');
        var _TipoLogradouroService = abp.services.app.tipoLogradouro;
        var _$filterForm = $('#TiposLogradouroFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TiposLogradouro.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TiposLogradouro.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TiposLogradouro.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposLogradouro/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposLogradouro/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoLogradouroModal'
        });

        _$TiposLogradouroTable.jtable({

            title: app.localize('TiposLogradouro'),
            paging: true,
            //sorting: true,
            //multiSorting: true,

            actions: {
                listAction: {
                    method: _TipoLogradouroService.listar
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
                                    deleteTipoLogradouro(data.record);
                                });
                        }

                        return $span;
                    }
                }
                ,
                abreviacao: {
                    title: app.localize('Abreviacao'),
                    width: '12%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '70%'
                }
            }
        });

        function getTiposLogradouro(reload) {
            if (reload) {
                _$TiposLogradouroTable.jtable('reload');
            } else {
                _$TiposLogradouroTable.jtable('load', {
                    filtro: $('#TiposLogradouroTableFilter').val()
                });
            }
        }

        function deleteTipoLogradouro(tipoLogradouro) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoLogradouro.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TipoLogradouroService.excluir(tipoLogradouro)
                            .done(function () {
                                getTiposLogradouro(true);
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

        $('#CreateNewTipoLogradouroButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposLogradouroParaExcelButton').click(function () {
            _TipoLogradouroService
                .listarParaExcel({
                    filtro: $('#TiposLogradouroTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposLogradouroButton, #RefreshTiposLogradouroListButton').click(function (e) {
            e.preventDefault();
            getTiposLogradouro();
        });

        abp.event.on('app.CriarOuEditarTipoLogradouroModalSaved', function () {
            getTiposLogradouro(true);
        });

        getTiposLogradouro();

        $('#TiposLogradouroTableFilter').focus();
    });
})();