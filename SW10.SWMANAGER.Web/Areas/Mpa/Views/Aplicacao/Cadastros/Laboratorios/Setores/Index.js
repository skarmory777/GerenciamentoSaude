(function () {
    $(function () {
        var _$SetorsTable = $('#SetorsTable');
        var _SetorsService = abp.services.app.setor;
        var _$filterForm = $('#SetorsFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Setor.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Setor.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.Setor.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Setores/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Setores/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarSetorModal'
        });

        _$SetorsTable.jtable({

            title: app.localize('Setors'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _SetorsService.listar
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
                                    deleteSetors(data.record);
                                });
                        }

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

        function getSetors(reload) {
            if (reload) {
                _$SetorsTable.jtable('reload');
            } else {
                _$SetorsTable.jtable('load', {
                    filtro: $('#SetorsTableFilter').val()
                });
            }
        }

        function deleteSetors(Setor) {

            abp.message.confirm(
                app.localize('DeleteWarning', Setor.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _SetorsService.excluir(Setor)
                            .done(function () {
                                getSetors(true);
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

        $('#CreateNewSetorButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarSetorsParaExcelButton').click(function () {
            _SetorsService
                .listarParaExcel({
                    filtro: $('#SetorsTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetSetorsButton, #RefreshSetorsListButton').click(function (e) {
            e.preventDefault();
            getSetors();
        });

        abp.event.on('app.CriarOuEditarSetorModalSaved', function () {
            getSetors(true);
        });

        getSetors();

        $('#SetorsTableFilter').focus();


    });
})();