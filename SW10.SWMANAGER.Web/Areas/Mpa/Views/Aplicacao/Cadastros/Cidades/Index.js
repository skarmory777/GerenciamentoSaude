(function () {
    $(function () {
        var _$CidadesTable = $('#CidadesTable');
        var _CidadesService = abp.services.app.cidade;
        var _$filterForm = $('#CidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Cidades/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Cidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarCidadeModal'
        });

        _$CidadesTable.jtable({

            title: app.localize('Cidades'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _CidadesService.listar
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
                                    deleteCidades(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '15%'
                },
                capital: {
                    title: app.localize('Capital'),
                    width: '15%',
                    display: function (data) {
                        if (data.record.capital) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
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

        function getCidades(reload) {
            if (reload) {
                _$CidadesTable.jtable('reload');
            } else {
                _$CidadesTable.jtable('load', {
                    filtro: $('#CidadesTableFilter').val(),
                    estadoId: $('#cbo-estados').val() === '' ? 0 : $('#cbo-estados').val()
                });
            }
        }

        function deleteCidades(cidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', cidade.Nome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _CidadesService.excluir(cidade)
                            .done(function () {
                                getCidades(true);
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

        $('#CreateNewCidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarCidadesParaExcelButton').click(function () {
            _CidadesService
                .listarParaExcel({
                    filtro: $('#CidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetCidadesButton, #RefreshCidadesListButton').click(function (e) {
            e.preventDefault();
            getCidades();
        });

        //$('#cbo-estados').change(function (e) {
        //    e.preventDefault();
        //    getCidades();
        //});
        abp.event.on('app.CriarOuEditarCidadeModalSaved', function () {
            getCidades(true);
        });

        getCidades();

        $('#CidadesTableFilter').focus();


    });
})();