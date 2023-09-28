(function () {
    $(function () {
        var _$TiposAcomodacaoTable = $('#TiposAcomodacaoTable');
        var _TiposAcomodacaoService = abp.services.app.tipoAcomodacao;
        var _$filterForm = $('#TiposAcomodacaoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoAcomodacao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoAcomodacao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.TipoAcomodacao.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/TiposAcomodacao/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposAcomodacao/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarTipoAcomodacaoModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/TiposAcomodacao/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$TiposAcomodacaoTable.jtable({

            title: app.localize('TipoAcomodacao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _TiposAcomodacaoService.listar
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
                                    deleteTiposAcomodacao(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
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

        function getTiposAcomodacao(reload) {
            if (reload) {
                _$TiposAcomodacaoTable.jtable('reload');
            } else {
                _$TiposAcomodacaoTable.jtable('load', {
                    filtro: $('#TiposAcomodacaoTableFilter').val()
                });
            }
        }

        function deleteTiposAcomodacao(tipoAcomodacao) {

            abp.message.confirm(
                app.localize('DeleteWarning', tipoAcomodacao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _TiposAcomodacaoService.excluir(tipoAcomodacao)
                            .done(function () {
                                getTiposAcomodacao(true);
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

        $('#CreateNewTipoAcomodacaoButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarTiposAcomodacaoParaExcelButton').click(function () {
            _TiposAcomodacaoService
                .listarParaExcel({
                    filtro: $('#TiposAcomodacaoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount:$('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetTiposAcomodacaoButton, #RefreshTiposAcomodacaoListButton').click(function (e) {
            e.preventDefault();
            getTiposAcomodacao();
        });

        abp.event.on('app.CriarOuEditarTipoAcomodacaoModalSaved', function () {
            getTiposAcomodacao(true);
        });

        getTiposAcomodacao();

        $('#TiposAcomodacaoTableFilter').focus();
    });
})();