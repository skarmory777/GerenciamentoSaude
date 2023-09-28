(function () {
    $(function () {
        var _$ProfissoesTable = $('#ProfissoesTable');
        var _ProfissoesService = abp.services.app.profissao;
        var _$filterForm = $('#ProfissoesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Profissao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Profissao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Profissao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Profissoes/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Profissoes/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarProfissaoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Profissoes/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$ProfissoesTable.jtable({

            title: app.localize('Profissoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ProfissoesService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '33%',
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
                                    deleteProfissoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.creationTime).format("L LT");
                    }
                }
            }
        });

        function getProfissoes(reload) {
            if (reload) {
                _$ProfissoesTable.jtable('reload');
            } else {
                _$ProfissoesTable.jtable('load', {
                    filtro: $('#ProfissoesTableFilter').val()
                });
            }
        }

        function deleteProfissoes(Profissao) {

            abp.message.confirm(
                app.localize('DeleteWarning', Profissao.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ProfissoesService.excluir(Profissao)
                            .done(function () {
                                getProfissoes(true);
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

        $('#CreateNewProfissaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarProfissoesParaExcelButton').click(function () {
            _ProfissoesService
                .listarParaExcel({
                    filtro: $('#ProfissoesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetProfissoesButton, #RefreshProfissoesListButton').click(function (e) {
            e.preventDefault();
            getProfissoes();
        });

        abp.event.on('app.CriarOuEditarProfissaoModalSaved', function () {
            getProfissoes(true);
        });

        getProfissoes();

        $('#ProfissoesTableFilter').focus();
    });
})();