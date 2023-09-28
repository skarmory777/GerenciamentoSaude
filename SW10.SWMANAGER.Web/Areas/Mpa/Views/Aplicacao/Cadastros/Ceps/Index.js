(function () {
    $(function () {
        var _$CepsTable = $('#CepsTable');
        var _CepsService = abp.services.app.cep;
        var _$filterForm = $('#CepsFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Ceps/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Ceps/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarCepModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Ceps/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$CepsTable.jtable({

            title: app.localize('Ceps'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _CepsService.listar
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
                                    deleteCeps(data.record);
                                });
                        }

                        return $span;
                    }
                },
                cep: {
                    title: app.localize('Cep'),
                    width: '15%'
                },
                logradouro: {
                    title: app.localize('Logradouro'),
                    width: '15%',
                    display: function (data) {
                        return data.record.logradouro
                    }
                },
                bairro: {
                    title: app.localize('Bairro'),
                    width: '15%',
                    display: function (data) {
                        return data.record.bairro
                    }
                },
                cidade: {
                    title: app.localize('Cidade'),
                    width: '15%',
                    display: function (data) {
                        return data.record.cidade.nome
                    }
                },
                estado: {
                    title: app.localize('Estado'),
                    sorting: false,
                    width: '15%',
                    display: function (data) {
                        return data.record.estado.nome + ' (' + data.record.estado.uf + ')'
                    }
                }
            }
        });

        function getCeps(reload) {
            if (reload) {
                _$CepsTable.jtable('reload');
            } else {
                _$CepsTable.jtable('load', {
                    filtro: $('#CepsTableFilter').val(),
                    estadoId: $('#cbo-estados').val() === '' ? 0 : $('#cbo-estados').val()
                });
            }
        }

        function deleteCeps(cep) {

            abp.message.confirm(
                app.localize('DeleteWarning', cep.cep),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _CepsService.excluir(cep)
                            .done(function () {
                                getCeps(true);
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

        $('#CreateNewCepButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarCepsParaExcelButton').click(function () {
            _CepsService
                .listarParaExcel({
                    filtro: $('#CepsTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetCepsButton, #RefreshCepsListButton').click(function (e) {
            e.preventDefault();
            getCeps();
        });

        $('#cbo-estados').change(function (e) {
            e.preventDefault();
            getCeps();
        });
        abp.event.on('app.CriarOuEditarCepModalSaved', function () {
            getCeps(true);
        });

        getCeps();

        $('#CepsTableFilter').focus();


    });
})();