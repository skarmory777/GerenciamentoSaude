(function () {
    $(function () {
        var _$TiposDocumentoTable = $('#TiposDocumentoTable');
        var _TiposDocumentoService = abp.services.app.tipoDocumento;
        var _$filterForm = $('#TiposDocumentoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoDocumento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoDocumento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoDocumento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposDocumento/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposDocumento/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoDocumentoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposDocumento/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$TiposDocumentoTable.jtable({

            title: app.localize('TipoDocumento'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposDocumentoService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
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
                                    deleteTiposDocumento(data.record);
                                });
                        }

                        return $span;
                    }
                },
             
                descricao: {
                    title: app.localize('Descricao'),
                    width: '8%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '2%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getTiposDocumento(reload) {
            if (reload) {
                _$TiposDocumentoTable.jtable('reload');
            } else {
                _$TiposDocumentoTable.jtable('load', {
                    filtro: $('#TiposDocumentoTableFilter').val()
                });
            }
        }

        function deleteTiposDocumento(tipoDocumento) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoDocumento.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposDocumentoService.excluir(tipoDocumento)
                            .done(function () {
                                getTiposDocumento(true);
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

        $('#CreateNewTipoDocumentoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposDocumentoParaExcelButton').click(function () {
            _TiposDocumentoService
                .listarParaExcel({
                    filtro: $('#TiposDocumentoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposDocumentoButton, #RefreshTiposDocumentoListButton').click(function (e) {
            e.preventDefault();
            getTiposDocumento();
        });

        abp.event.on('app.CriarOuEditarTipoDocumentoModalSaved', function () {
            getTiposDocumento(true);
        });

        getTiposDocumento();

        $('#TiposDocumentoTableFilter').focus();
    });
})();