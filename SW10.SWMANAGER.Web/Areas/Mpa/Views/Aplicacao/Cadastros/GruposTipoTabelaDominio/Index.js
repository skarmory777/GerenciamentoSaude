(function () {
    $(function () {
        var _$GruposTipoTabelaDominioTable = $('#GruposTipoTabelaDominioTable');
        var _GruposTipoTabelaDominioService = abp.services.app.grupoTipoTabelaDominio;
        var _$filterForm = $('#GruposTipoTabelaDominioFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.GrupoTipoTabelaDominio.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.GrupoTipoTabelaDominio.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.DominioTiss.GrupoTipoTabelaDominio.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GruposTipoTabelaDominio/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GruposTipoTabelaDominio/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarGrupoTipoTabelaDominioModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/GruposTipoTabelaDominio/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });

        
        _$GruposTipoTabelaDominioTable.jtable({

            title: app.localize('GrupoTipoTabelaDominio'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _GruposTipoTabelaDominioService.listarFull
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
                                    deleteGruposTipoTabelaDominio(data.record);
                                });
                        }
                        
                        return $span;
                    }
                },
                tipoTabelaDominio: {
                    title: app.localize('TipoTabelaDominio'),
                    width: '12%',
                    display: function (data) {
                        return data.record.tipoTabelaDominio.descricao;
                    }
                },
                descricao: {
                    title: app.localize('Grupo'),
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

        function getGruposTipoTabelaDominio(reload) {
            if (reload) {
                _$GruposTipoTabelaDominioTable.jtable('reload');
            } else {
                _$GruposTipoTabelaDominioTable.jtable('load', {
                    filtro: $('#GruposTipoTabelaDominioTableFilter').val()
                });
            }
        }

        function deleteGruposTipoTabelaDominio(grupoTipoTabelaDominio) {

            abp.message.confirm(
                app.localize('DeleteWarning', grupoTipoTabelaDominio.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _GruposTipoTabelaDominioService.excluir(grupoTipoTabelaDominio)
                            .done(function () {
                                getGruposTipoTabelaDominio(true);
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

        $('#CreateNewGrupoTipoTabelaDominioButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarGruposTipoTabelaDominioParaExcelButton').click(function () {
            _GruposTipoTabelaDominioService
                .listarParaExcel({
                    filtro: $('#GruposTipoTabelaDominioTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetGruposTipoTabelaDominioButton, #RefreshGruposTipoTabelaDominioListButton').click(function (e) {
            e.preventDefault();
            getGruposTipoTabelaDominio();
        });

        abp.event.on('app.CriarOuEditarGrupoTipoTabelaDominioModalSaved', function () {
            getGruposTipoTabelaDominio(true);
        });

        getGruposTipoTabelaDominio();

        $('#GruposTipoTabelaDominioTableFilter').focus();
    });
})();