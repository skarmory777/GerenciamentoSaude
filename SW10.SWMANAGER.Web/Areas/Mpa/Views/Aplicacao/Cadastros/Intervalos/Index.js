(function () {
    $(function () {
        var _$IntervalosTable = $('#IntervalosTable');
        var _intervaloService = abp.services.app.intervalo;
        var _$filterForm = $('#IntervalosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Intervalo.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Intervalo.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Intervalo.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Intervalos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Intervalos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarIntervaloModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Intervalos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$IntervalosTable.jtable({

            title: app.localize('Intervalos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _intervaloService.listar
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
                                    deleteIntervalos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
                intervaloMinutos: {
                    title: app.localize('IntervaloMinutos'),
                    width: '15%'
                },
                atendimentosPorHora: {
                    title: app.localize('AtendimentosPorHora'),
                    sorting: false,
                    width: '15%'
                }
            }
        });

        function getIntervalos(reload) {
            if (reload) {
                _$IntervalosTable.jtable('reload');
            } else {
                _$IntervalosTable.jtable('load', {
                    filtro: $('#IntervalosTableFilter').val()
                });
            }
        }

        function deleteIntervalos(Intervalo) {

            abp.message.confirm(
                app.localize('DeleteWarning', Intervalo.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _intervaloService.excluir(Intervalo)
                            .done(function () {
                                getIntervalos(true);
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

        $('#CreateNewIntervaloButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarIntervalosParaExcelButton').click(function () {
            _intervaloService
                .listarParaExcel({
                    filtro: $('#IntervalosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetIntervalosButton, #RefreshIntervalosListButton').click(function (e) {
            e.preventDefault();
            getIntervalos();
        });

        abp.event.on('app.CriarOuEditarIntervaloModalSaved', function () {
            getIntervalos(true);
        });

        getIntervalos();

        $('#IntervalosTableFilter').focus();
    });
})();