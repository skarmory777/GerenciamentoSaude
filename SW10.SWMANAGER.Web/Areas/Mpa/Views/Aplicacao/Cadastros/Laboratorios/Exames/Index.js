(function () {
    $(function () {
        var _$ExamesTable = $('#ExamesTable');
        var _ExamesService = abp.services.app.exame;
        var _$filterForm = $('#ExamesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Exame.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Exame.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Exame.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Exames/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Exames/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarExameModal'
        });

        _$ExamesTable.jtable({

            title: app.localize('Exames'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ExamesService.listar
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

                        //if (_permissions.delete) {
                        //    $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                        //        .appendTo($span)
                        //        .click(function () {
                        //            deleteExames(data.record);
                        //        });
                        //}

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Código'),
                    width: '15%'
                },
                descricao: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
            }
        });

        function getExames(reload) {
            if (reload) {
                _$ExamesTable.jtable('reload');
            } else {
               
                _$ExamesTable.jtable('load', {
                    filtro: $('#ExamesTableFilter').val()
                });
            }
        }

        function deleteExames(Exame) {

            abp.message.confirm(
                app.localize('DeleteWarning', Exame.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ExamesService.excluir(Exame)
                            .done(function () {
                                getExames(true);
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

        $('#CreateNewExameButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarExamesParaExcelButton').click(function () {
            _ExamesService
                .listarParaExcel({
                    filtro: $('#ExamesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetExamesButton, #RefreshExamesListButton').click(function (e) {
            e.preventDefault();
            getExames();
        });

        abp.event.on('app.CriarOuEditarExameModalSaved', function () {
            getExames(true);
        });

        getExames();

        $('#ExamesTableFilter').focus();


    });
})();