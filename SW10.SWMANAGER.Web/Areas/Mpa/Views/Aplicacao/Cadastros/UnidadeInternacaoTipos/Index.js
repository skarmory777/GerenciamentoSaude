(function () {
    $(function () {
        var _$UnidadeInternacaoTiposTable = $('#UnidadeInternacaoTiposTable');
        var _UnidadeInternacaoTiposService = abp.services.app.unidadeInternacaoTipo;
        var _$filterForm = $('#UnidadeInternacaoTiposFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/UnidadeInternacaoTipos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/UnidadeInternacaoTipos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarUnidadeInternacaoTipoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/UnidadeInternacaoTipos/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$UnidadeInternacaoTiposTable.jtable({

            title: app.localize('UnidadeInternacaoTipos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _UnidadeInternacaoTiposService.listar
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '7%',
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
                                    deleteUnidadeInternacaoTipos(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '8%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '25%'
                }
                //,
                //tipoAlta: {
                //    title: app.localize('TipoAlta'),
                //    width: '25%',
                //    display: function (data) {
                //        if (data.record.unidadeInternacaoTipoAlta) {
                //            return data.record.unidadeInternacaoTipoAlta.descricao;
                //        }
                //    }
                //}
            }
        });

        function getUnidadeInternacaoTipos(reload) {
            if (reload) {
                _$UnidadeInternacaoTiposTable.jtable('reload');
            } else {
                _$UnidadeInternacaoTiposTable.jtable('load', {
                    filtro: $('#UnidadeInternacaoTiposTableFilter').val()
                });
            }
        }

        function deleteUnidadeInternacaoTipos(UnidadeInternacaoTipo) {

            abp.message.confirm(
                app.localize('DeleteWarning', UnidadeInternacaoTipo.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _UnidadeInternacaoTiposService.excluir(UnidadeInternacaoTipo)
                            .done(function () {
                                getUnidadeInternacaoTipos(true);
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

        $('#CreateNewUnidadeInternacaoTipoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarUnidadeInternacaoTiposParaExcelButton').click(function () {
            _UnidadeInternacaoTiposService
                .listarParaExcel({
                    filtro: $('#UnidadeInternacaoTiposTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetUnidadeInternacaoTiposButton, #RefreshUnidadeInternacaoTiposListButton').click(function (e) {
            e.preventDefault();
            getUnidadeInternacaoTipos();
        });

        abp.event.on('app.CriarOuEditarUnidadeInternacaoTipoModalSaved', function () {
            getUnidadeInternacaoTipos(true);
        });

        getUnidadeInternacaoTipos();

        $('#UnidadeInternacaoTiposTableFilter').focus();
    });
})();