(function () {
    $(function () {
        var _$NaturalidadesTable = $('#NaturalidadesTable');
        var _NaturalidadesService = abp.services.app.naturalidade;
        var _$filterForm = $('#NaturalidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Naturalidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Naturalidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Naturalidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Naturalidades/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Naturalidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarNaturalidadeModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Naturalidades/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$NaturalidadesTable.jtable({

            title: app.localize('Naturalidades'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _NaturalidadesService.listar
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
                                    deleteNaturalidades(data.record);
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

        function getNaturalidades(reload) {
            if (reload) {
                _$NaturalidadesTable.jtable('reload');
            } else {
                _$NaturalidadesTable.jtable('load', {
                    filtro: $('#NaturalidadesTableFilter').val()
                });
            }
        }

        function deleteNaturalidades(Naturalidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', Naturalidade.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _NaturalidadesService.excluir(Naturalidade)
                            .done(function () {
                                getNaturalidades(true);
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

        $('#CreateNewNaturalidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarNaturalidadesParaExcelButton').click(function () {
            _NaturalidadesService
                .listarParaExcel({
                    filtro: $('#NaturalidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetNaturalidadesButton, #RefreshNaturalidadesListButton').click(function (e) {
            e.preventDefault();
            getNaturalidades();
        });

        abp.event.on('app.CriarOuEditarNaturalidadeModalSaved', function () {
            getNaturalidades(true);
        });

        getNaturalidades();

        $('#NaturalidadesTableFilter').focus();
    });
})();