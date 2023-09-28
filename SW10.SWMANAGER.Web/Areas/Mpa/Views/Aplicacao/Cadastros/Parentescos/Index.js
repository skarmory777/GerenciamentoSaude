(function () {
    $(function () {
        var _$ParentescosTable = $('#ParentescosTable');
        var _ParentescosService = abp.services.app.parentesco;
        var _$filterForm = $('#ParentescosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Parentesco.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Parentesco.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Parentesco.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Parentescos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Parentescos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarParentescoModal'
        });

        _$ParentescosTable.jtable({

            title: app.localize('Parentesco'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ParentescosService.listar
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
                                    deleteParentescos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '33%'
                },
                criacao: {
                    title: app.localize('Criacao'),
                    width: '33%',
                    display: function (data) {
                        return moment(data.record.criacao).format('L');
                    }
                }
               
            }
        });

        function getParentescos(reload) {
            if (reload) {
                _$ParentescosTable.jtable('reload');
            } else {
                _$ParentescosTable.jtable('load', {
                    filtro: $('#ParentescosTableFilter').val()
                });
            }
        }

        function deleteParentescos(Parentesco) {

            abp.message.confirm(
                app.localize('DeleteWarning', Parentesco.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ParentescosService.excluir(Parentesco)
                            .done(function () {
                                getParentescos(true);
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

        $('#CreateNewParentescoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarParentescosParaExcelButton').click(function () {
            _ParentescosService
                .listarParaExcel({
                    filtro: $('#ParentescosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetParentescosButton, #RefreshParentescosListButton').click(function (e) {
            e.preventDefault();
            getParentescos();
        });

        abp.event.on('app.CriarOuEditarParentescoModalSaved', function () {
            getParentescos(true);
        });

        getParentescos();

        $('#ParentescosTableFilter').focus();
    });
})();