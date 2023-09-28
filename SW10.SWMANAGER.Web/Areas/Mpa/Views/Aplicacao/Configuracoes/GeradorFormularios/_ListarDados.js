(function () {
    $(function () {
        var _$GeradorFormulariosTable = $('#GeradorFormulariosTable');
        var _GeradorFormulariosService = abp.services.app.formResposta;
        var _$filterForm = $('#GeradorFormulariosFilterForm');
        var nCol1 = '';
        varnCol2 = '';

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Configuracoes.GeradorFormulario.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Configuracoes.GeradorFormulario.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Configuracoes.GeradorFormulario.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GeradorFormularios/_EditarPreenchimento',
            //scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/CriarFormulario.js',
            //scriptUrl: abp.appPath + 'Scripts/Formulario/FormularioApp.js',
            modalClass: 'CriarOuEditarGeradorFormularioModal'
        });

        _$GeradorFormulariosTable.jtable({

            title: $('#form-name').val(), //app.localize('GeradorFormularios'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _GeradorFormulariosService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        //if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Preencher') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _createOrEditModal.open({ id: data.record.id });
                                //location.href = '/Mpa/GeradorFormularios/EditarPreenchimento/' + data.record.id;
                            });
                        //}
                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Detalhes') + '"><i class="fa fa-file-text-o"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //deleteGeradorFormularios(data.record);
                                    location.href = '/Mpa/GeradorFormularios/DetalharPreenchimento/' + data.record.id;
                                });
                        //}
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Excluir') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function () {
                                        deleteGeradorFormularios(data.record);
                                        //location.href = '/Mpa/GeradorFormularios/DetalharPreenchimento/' + data.record.id;
                                    });
                        }
                        return $span;
                    }
                },
                dataResposta: {
                    title: app.localize('DataAlteracao'),
                    width: '20%',
                    display: function (data) {
                        //return moment(data.record.dataAlteracao).format('L LT');
                        if (data.record.colRespostas.length > 0) {
                            console.log(data.record.colRespostas[0]);
                            return moment(data.record.colRespostas[0].creationTime).format('L LT');
                        }
                    }
                },
                col1: {
                    title: 'col1',
                    width: '30%',
                    display: function (data) {
                        if (data.record.colRespostas.length > 0) {
                            $('span:contains("col1")').html(data.record.colRespostas[0].coluna.name);
                            return data.record.colRespostas[0].valor;
                        }
                    },
                },
                col2: {
                    title: 'col2',
                    width: '30%',
                    display: function (data) {
                        if (data.record.colRespostas.length > 1) {
                            $('span:contains("col2")').html(data.record.colRespostas[1].coluna.name);
                            return data.record.colRespostas[1].valor;
                        }
                    }
                }
            }
        });

        function getGeradorFormularios(reload) {
            if (reload) {
                _$GeradorFormulariosTable.jtable('reload');
            } else {
                _$GeradorFormulariosTable.jtable('load', {
                    filtro: $('#GeradorFormulariosTableFilter').val(),
                    formId: $('#form-id').val()
                });
            }
        }

        function deleteGeradorFormularios(GeradorFormulario) {

            abp.message.confirm(
                app.localize('DeleteWarning', GeradorFormulario.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _GeradorFormulariosService.excluir(GeradorFormulario)
                            .done(function () {
                                getGeradorFormularios(true);
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

        $('#CreateNewGeradorFormularioButton').click(function () {
            _createOrEditModal.open();
            //location.href = '/Mpa/GeradorFormularios/Preencher/' + $('#form-id').val();
        });

        $('#ExportarGeradorFormulariosParaExcelButton').click(function () {
            _GeradorFormulariosService
                .listarParaExcel({
                    filtro: $('#GeradorFormulariosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val(),
                    formId: $('#form-id').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetGeradorFormulariosButton, #RefreshGeradorFormulariosListButton').click(function (e) {
            e.preventDefault();
            getGeradorFormularios();
        });

        abp.event.on('app.CriarOuEditarGeradorFormularioModalSaved', function () {
            getGeradorFormularios(true);
        });

        getGeradorFormularios();

        $('#GeradorFormulariosTableFilter').focus();
    });
})();