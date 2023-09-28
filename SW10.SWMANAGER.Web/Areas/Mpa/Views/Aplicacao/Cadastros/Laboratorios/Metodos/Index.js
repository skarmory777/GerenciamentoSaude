(function () {
    $(function () {
        var _$MetodosTable = $('#MetodosTable');
        var _MetodosService = abp.services.app.metodo;
        var _$filterForm = $('#MetodosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Metodo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Metodo.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Metodo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Metodos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Metodos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarMetodoModal'
        });

        _$MetodosTable.jtable({

            title: app.localize('Metodos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _MetodosService.listar
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
                                    deleteMetodos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Descrição'),
                    width: '15%'
                },
            }
        });

        function getMetodos(reload) {
            if (reload) {
                _$MetodosTable.jtable('reload');
            } else {
                _$MetodosTable.jtable('load', {
                    filtro: $('#MetodosTableFilter').val()
                });
            }
        }

        function deleteMetodos(metodo) {

            abp.message.confirm(
                app.localize('DeleteWarning', metodo.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _MetodosService.excluir(metodo)
                            .done(function () {
                                getMetodos(true);
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

        $('#CreateNewMetodoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarMetodosParaExcelButton').click(function () {
            _MetodosService
                .listarParaExcel({
                    filtro: $('#MetodosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetMetodosButton, #RefreshMetodosListButton').click(function (e) {
            e.preventDefault();
            getMetodos();
        });

        abp.event.on('app.CriarOuEditarMetodoModalSaved', function () {
            getMetodos(true);
        });

        getMetodos();

        $('#MetodosTableFilter').focus();


    });
})();