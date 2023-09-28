(function () {
    $(function () {
        var _$InstituicoesTransferenciaTable = $('#InstituicoesTransferenciaTable');
        var _InstituicoesTransferenciaService = abp.services.app.instituicaoTransferencia;
        var _$filterForm = $('#InstituicoesTransferenciaFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.InstituicaoTransferencia.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.InstituicaoTransferencia.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.InstituicaoTransferencia.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/InstituicoesTransferencia/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/InstituicoesTransferencia/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/InstituicoesTransferencia/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$InstituicoesTransferenciaTable.jtable({

            title: app.localize('InstituicaoTransferencia'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _InstituicoesTransferenciaService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '2%',
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
                                    deleteInstituicoesTransferencia(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codInstituicaoTransferencia: {
                    title: app.localize('CodInstituicaoTransferencia'),
                    width: '4%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '8%'
                },
                creationTime: {
                    title: app.localize('CreationTime'),
                    width: '2%',
                    display: function (data) {
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getInstituicoesTransferencia(reload) {
            if (reload) {
                _$InstituicoesTransferenciaTable.jtable('reload');
            } else {
                _$InstituicoesTransferenciaTable.jtable('load', {
                    filtro: $('#InstituicoesTransferenciaTableFilter').val()
                });
            }
        }

        function deleteInstituicoesTransferencia(InstituicaoTransferencia) {

            abp.message.confirm(
                app.localize('DeleteWarning', InstituicaoTransferencia.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _InstituicoesTransferenciaService.excluir(InstituicaoTransferencia)
                            .done(function () {
                                getInstituicoesTransferencia(true);
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

        $('#CreateNewInstituicaoTransferenciaButton').click(function () {
            //console.log('cerate new botao apertado');
            _createOrEditModal.open();
        });

        $('#ExportarInstituicoesTransferenciaParaExcelButton').click(function () {
            _InstituicoesTransferenciaService
                .listarParaExcel({
                    filtro: $('#InstituicoesTransferenciaTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetInstituicoesTransferenciaButton, #RefreshInstituicoesTransferenciaListButton').click(function (e) {
            e.preventDefault();
            getInstituicoesTransferencia();
        });

        abp.event.on('app.CriarOuEditarInstituicaoTransferenciaModalSaved', function () {
            getInstituicoesTransferencia(true);
        });

        getInstituicoesTransferencia();

        $('#InstituicoesTransferenciaTableFilter').focus();
    });
})();