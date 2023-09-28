(function () {
    $(function () {
        var _$ConveniosTable = $('#TabelaPrecoConveniosTable');
        var _ConveniosService = abp.services.app.convenio;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Convenio.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/FaturamentoTabelaPrecoConvenios/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/TabelaPrecoConvenios/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTabelaPrecoConvenioModal'
        });

        _$ConveniosTable.jtable({

            title: app.localize('Convenios'),
            pageSize: 25,
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ConveniosService.listarConveniosTabelaPreco
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
                                    /*window.open(`/Mpa/FaturamentoTabelaPrecoConvenios/CriarOuEditar?id=${data.record.id}`)*/
                                    _createOrEditModal.open({ id: data.record.id });
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deleteConvenios(data.record);
                                });
                        }

                        return $span;
                    }
                },
                nomeFantasia: {
                    title: app.localize('NomeFantasia'),
                    width: '20%'
                },
                razaoSocial: {
                    title: app.localize('RazaoSocial'),
                    width: '20%'
                },
                registroANS: {
                    title: app.localize('NumeroRegistroAns'),
                    width: '15%'
                },
                isAtivo: {
                    title: app.localize('IsAtivo'),
                    width: '5%',
                    display: function (data) {
                        if (data.record.isAtivo) {
                            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                        } else {
                            return '<span class="label label-default">' + app.localize('No') + '</span>';
                        }
                    }
                },

            }
        });

        function getConvenios(reload) {
            if (reload) {
                _$ConveniosTable.jtable('reload');
            } else {
                _$ConveniosTable.jtable('load', {
                    filtro: $('#txtTabelaPrecoConveniosFilter').val()
                });
            }
        }

        function deleteConvenios(Convenio) {

            abp.message.confirm(
                app.localize('DeleteWarning', Convenio.razaoSocial),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ConveniosService.excluir(Convenio)
                            .done(function () {
                                getConvenios(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
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

        $('#CreateNewConvenioButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarConveniosParaExcelButton').click(function () {
            _ConveniosService
                .listarParaExcel({
                    filtro: $('#txtTabelaPrecoConveniosFilter').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#btnGetTabelaPrecoConvenios').click(function (e) {
            e.preventDefault();
            getConvenios();
        });

        abp.event.on('app.CriarOuEditarConvenioModalSaved', function () {
            getConvenios(true);
        });

        getConvenios();

        $('#txtTabelaPrecoConveniosFilter').focus();
    });
})();