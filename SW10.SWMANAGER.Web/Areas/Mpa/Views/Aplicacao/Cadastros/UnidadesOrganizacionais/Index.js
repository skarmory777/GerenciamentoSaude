(function () {
    $(function () {
        var _$UnidadesInternacaoTable = $('#UnidadesInternacaoTable');
        var _UnidadesInternacaoService = abp.services.app.unidadeInternacao;
        var _$filterForm = $('#UnidadesInternacaoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Atendimento.UnidadeInternacao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/UnidadesInternacao/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/UnidadesInternacao/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarUnidadeInternacaoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/UnidadesInternacao/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$UnidadesInternacaoTable.jtable({

            title: app.localize('UnidadesInternacao'),
            paging: true,
            //sorting: true,
            //multiSorting: true,

            actions: {
                listAction: {
                    method: _UnidadesInternacaoService.listar
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
                                    deleteUnidadesInternacao(data.record);
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
                ,
                localizacao: {
                    title: app.localize('Localizacao'),
                    width: '25%'
                }
                ,
                isHospitalDia: {
                    title: app.localize('HospitalDia'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.isHospitalDia) {
                            return '<div style="text-align:center;">' + '<span class="label label-success content-center text-center">' + app.localize('Yes') + '</span>' + '</div>';
                        } else {
                            return '<div style="text-align:center;">' + '<span class="label label-default content-center text-center">' + app.localize('No') + '</span>' + '</div>';
                        }
                    }
                }
                ,
                isAtivo: {
                    title: app.localize('IsAtivo'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.isAtivo) {
                            return '<div style="text-align:center;">' + '<span class="label label-success content-center text-center">' + app.localize('Yes') + '</span>' + '</div>';
                        } else {
                            return '<div style="text-align:center;">' + '<span class="label label-default content-center text-center">' + app.localize('No') + '</span>' + '</div>';
                        }
                    }
                }
            }
        });

        function getUnidadesInternacao(reload) {
            if (reload) {
                _$UnidadesInternacaoTable.jtable('reload');
            } else {
                _$UnidadesInternacaoTable.jtable('load', {
                    filtro: $('#UnidadesInternacaoTableFilter').val()
                });
            }
        }

        function deleteUnidadesInternacao(UnidadeInternacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', UnidadeInternacao.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _UnidadesInternacaoService.excluir(UnidadeInternacao)
                            .done(function () {
                                getUnidadesInternacao(true);
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

        $('#CreateNewUnidadeInternacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarUnidadesInternacaoParaExcelButton').click(function () {
            _UnidadesInternacaoService
                .listarParaExcel({
                    filtro: $('#UnidadesInternacaoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetUnidadesInternacaoButton, #RefreshUnidadesInternacaoListButton').click(function (e) {
            e.preventDefault();
            getUnidadesInternacao();
        });

        abp.event.on('app.CriarOuEditarUnidadeInternacaoModalSaved', function () {
            getUnidadesInternacao(true);
        });

        getUnidadesInternacao();

        $('#UnidadesInternacaoTableFilter').focus();
    });
})();