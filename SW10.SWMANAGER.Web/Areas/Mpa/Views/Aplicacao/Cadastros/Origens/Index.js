(function () {
    $(function () {
        var _$OrigensTable = $('#OrigensTable');
        var _OrigensService = abp.services.app.origem;
        var _$filterForm = $('#OrigensFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Origem.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Origem.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Origem.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Origens/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Origens/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarOrigemModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Origens/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$OrigensTable.jtable({

            title: app.localize('Origens'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _OrigensService.listar
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
                                    deleteOrigens(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '15%'
                }
            }
        });

        function getOrigens(reload) {
            if (reload) {
                _$OrigensTable.jtable('reload');
            } else {
                _$OrigensTable.jtable('load', {
                    filtro: $('#OrigensTableFilter').val()
                });
            }
        }

        function deleteOrigens(Origem) {

            abp.message.confirm(
                app.localize('DeleteWarning', Origem.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _OrigensService.excluir(Origem)
                            .done(function () {
                                getOrigens(true);
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

        $('#CreateNewOrigemButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarOrigensParaExcelButton').click(function () {
            _OrigensService
                .listarParaExcel({
                    filtro: $('#OrigensTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetOrigensButton, #RefreshOrigensListButton').click(function (e) {
            e.preventDefault();
            getOrigens();
        });

        abp.event.on('app.CriarOuEditarOrigemModalSaved', function () {
            getOrigens(true);
        });

        getOrigens();

        $('#OrigensTableFilter').focus();
    });
})();