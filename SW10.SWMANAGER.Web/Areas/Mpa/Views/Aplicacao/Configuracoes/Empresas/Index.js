(function () {
    $(function () {
        var _$EmpresasTable = $('#EmpresasTable');
        var _EmpresasService = abp.services.app.empresa;
        var _$filterForm = $('#EmpresasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Configuracoes.Empresa.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Configuracoes.Empresa.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Configuracoes.Empresa.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Empresas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/Empresas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarEmpresaModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Empresas/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$EmpresasTable.jtable({

            title: app.localize('Empresas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _EmpresasService.listar
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
                                    deleteEmpresas(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nomeFantasia: {
                    title: app.localize('NomeFantasia'),
                    width: '15%'
                },
                razaoSocial: {
                    title: app.localize('RazaoSocial'),
                    width: '15%'
                },
                numeroRegistroAns: {
                    title: app.localize('NumeroRegistroAns'),
                    width: '15%'
                },
                isAtivo: {
                    title: app.localize('IsAtivo'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.isAtivo) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },

            }
        });

        function getEmpresas(reload) {
            if (reload) {
                _$EmpresasTable.jtable('reload');
            } else {
                _$EmpresasTable.jtable('load', {
                    filtro: $('#EmpresasTableFilter').val()
                });
            }
        }

        function deleteEmpresas(Empresa) {

            abp.message.confirm(
                app.localize('DeleteWarning', Empresa.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _EmpresasService.excluir(Empresa)
                            .done(function () {
                                getEmpresas(true);
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

        $('#CreateNewEmpresaButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarEmpresasParaExcelButton').click(function () {
            _EmpresasService
                .listarParaExcel({
                    filtro: $('#EmpresasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetEmpresasButton, #RefreshEmpresasListButton').click(function (e) {
            e.preventDefault();
            getEmpresas();
        });

        abp.event.on('app.CriarOuEditarEmpresaModalSaved', function () {
            getEmpresas(true);
        });

        getEmpresas();

        $('#EmpresasTableFilter').focus();
    });
})();