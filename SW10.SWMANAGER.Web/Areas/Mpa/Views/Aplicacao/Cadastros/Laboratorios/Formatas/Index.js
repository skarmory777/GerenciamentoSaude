(function () {
    $(function () {
        var _$FormatasTable = $('#FormatasTable');
        var _FormatasService = abp.services.app.formata;
        var _$filterForm = $('#FormatasFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Formata.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Formata.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Formata.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Formatas/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarFormataModal'
        });

        _$FormatasTable.jtable({

            title: app.localize('Formatas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _FormatasService.listar
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
                                    deleteFormatas(data.record);
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

        function getFormatas(reload) {
            if (reload) {
                _$FormatasTable.jtable('reload');
            } else {
                _$FormatasTable.jtable('load', {
                    filtro: $('#FormatasTableFilter').val()
                });
            }
        }

        function deleteFormatas(Formata) {

            abp.message.confirm(
                app.localize('DeleteWarning', Formata.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _FormatasService.excluir(Formata)
                            .done(function () {
                                getFormatas(true);
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

        $('#CreateNewFormataButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarFormatasParaExcelButton').click(function () {
            _FormatasService
                .listarParaExcel({
                    filtro: $('#FormatasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetFormatasButton, #RefreshFormatasListButton').click(function (e) {
            e.preventDefault();
            getFormatas();
        });

        abp.event.on('app.CriarOuEditarFormataModalSaved', function () {
            getFormatas(true);
        });

        getFormatas();

        $('#FormatasTableFilter').focus();


    });
})();