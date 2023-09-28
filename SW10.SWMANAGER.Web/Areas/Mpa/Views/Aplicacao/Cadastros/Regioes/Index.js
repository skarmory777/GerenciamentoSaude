(function () {
    $(function () {
        var _$RegioesTable = $('#RegioesTable');
        var _RegioesService = abp.services.app.regiao;
        var _$filterForm = $('#RegioesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Regiao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Regiao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Regiao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Regioes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Regioes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarRegiaoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Regioes/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$RegioesTable.jtable({

            title: app.localize('Regiao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _RegioesService.listar
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
                                    deleteRegioes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '4%'
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

        function getRegioes(reload) {
            if (reload) {
                _$RegioesTable.jtable('reload');
            } else {
                _$RegioesTable.jtable('load', {
                    filtro: $('#RegioesTableFilter').val()
                });
            }
        }

        function deleteRegioes(regiao) {

            abp.message.confirm(
                app.localize('DeleteWarning', regiao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _RegioesService.excluir(regiao)
                            .done(function () {
                                getRegioes(true);
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

        $('#CreateNewRegiaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarRegioesParaExcelButton').click(function () {
            _RegioesService
                .listarParaExcel({
                    filtro: $('#RegioesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetRegioesButton, #RefreshRegioesListButton').click(function (e) {
            e.preventDefault();
            getRegioes();
        });

        abp.event.on('app.CriarOuEditarRegiaoModalSaved', function () {
            getRegioes(true);
        });

        getRegioes();

        $('#RegioesTableFilter').focus();
    });
})();