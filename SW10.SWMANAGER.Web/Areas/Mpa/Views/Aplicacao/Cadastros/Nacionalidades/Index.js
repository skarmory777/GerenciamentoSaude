(function () {
    $(function () {
        var _$NacionalidadesTable = $('#NacionalidadesTable');
        var _NacionalidadesService = abp.services.app.nacionalidade;
        var _$filterForm = $('#NacionalidadesFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Nacionalidade.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Nacionalidade.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Nacionalidade.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Nacionalidades/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Nacionalidades/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarNacionalidadeModal'
        });

        var _userPermissionsModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Nacionalidades/_PermissionsModal.js',
            modalClass: 'UserPermissionsModal'
        });
        _$NacionalidadesTable.jtable({

            title: app.localize('Nacionalidades'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _NacionalidadesService.listar
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
                                    deleteNacionalidades(data.record);
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
                        return moment(data.record.creationTime).format('L');
                    }
                }
            }
        });

        function getNacionalidades(reload) {
            if (reload) {
                _$NacionalidadesTable.jtable('reload');
            } else {
                _$NacionalidadesTable.jtable('load', {
                    filtro: $('#NacionalidadesTableFilter').val()
                });
            }
        }

        function deleteNacionalidades(Nacionalidade) {

            abp.message.confirm(
                app.localize('DeleteWarning', Nacionalidade.primeiroNome),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _NacionalidadesService.excluir(Nacionalidade)
                            .done(function () {
                                getNacionalidades(true);
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

        $('#CreateNewNacionalidadeButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportarNacionalidadesParaExcelButton').click(function () {
            _NacionalidadesService
                .listarParaExcel({
                    filtro: $('#NacionalidadesTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetNacionalidadesButton, #RefreshNacionalidadesListButton').click(function (e) {
            e.preventDefault();
            getNacionalidades();
        });

        abp.event.on('app.CriarOuEditarNacionalidadeModalSaved', function () {
            getNacionalidades(true);
        });

        getNacionalidades();

        $('#NacionalidadesTableFilter').focus();
    });
})();