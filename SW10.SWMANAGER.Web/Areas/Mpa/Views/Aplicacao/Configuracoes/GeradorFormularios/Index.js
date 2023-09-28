(function () {
    $(function () {
        var _$GeradorFormulariosTable = $('#GeradorFormulariosTable');
        var _GeradorFormulariosService = abp.services.app.formConfig;
        var _$filterForm = $('#GeradorFormulariosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Configuracoes.GeradorFormulario.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Configuracoes.GeradorFormulario.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Configuracoes.GeradorFormulario.Delete')
        };

        //var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/GeradorFormularios/CriarFormulario',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/CriarFormulario.js',
        //    //scriptUrl: abp.appPath + 'Scripts/Formulario/FormularioApp.js',
        //    modalClass: 'CriarOuEditarGeradorFormularioModal'
        //});

        var _associarUnidadeOrganizacional = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GeradorFormularios/_AssociarUnidadeOrganizacional',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_AssociarUnidadeOrganizacional.js',
            modalClass: 'AssociarUnidadeOrganizacionalModal'
        });

        var _associarOperacao = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GeradorFormularios/_AssociarOperacao',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_AssociarOperacao.js',
            modalClass: 'AssociarOperacaoModal'
        });

        var _associarEspecialidade = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/GeradorFormularios/_AssociarEspecialidade',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_AssociarEspecialidade.js',
            modalClass: 'AssociarEspecialidadeModal'
        });

        _$GeradorFormulariosTable.jtable({

            title: app.localize('GeradorFormularios'),
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
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="glyphicon glyphicon-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                //_createOrEditModal.open({ id: data.record.id });
                                location.href = '/Mpa/GeradorFormularios/EditarFormularioConfig/' + data.record.id;
                            });
                        }
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Preencher') + '"><i class="icon-pencil"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                //_createOrEditModal.open({ id: data.record.id });
                                location.href = '/Mpa/GeradorFormularios/Preencher/' + data.record.id;
                            });
                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Listar') + '"><i class="fa fa-list-ol"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //deleteGeradorFormularios(data.record);
                                    location.href = '/Mpa/GeradorFormularios/ListarDados/' + data.record.id;
                                });
                        //}
                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('ClonarFormulario') + '"><i class="glyphicon glyphicon-copyright-mark"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //deleteGeradorFormularios(data.record);
                                    location.href = '/Mpa/GeradorFormularios/ClonarFormulario/' + data.record.id;
                                });
                        //}
                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('AssociarUnidadeOrganizacional') + '"><i class="fa fa-share-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //deleteGeradorFormularios(data.record);
                                    //location.href = '/Mpa/GeradorFormularios/ClonarFormulario/' + data.record.id;
                                    _associarUnidadeOrganizacional.open({ formId: data.record.id });
                                });
                        //}
                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('AssociarOperacao') + '"><i class="fa fa-globe"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //deleteGeradorFormularios(data.record);
                                    //location.href = '/Mpa/GeradorFormularios/ClonarFormulario/' + data.record.id;
                                    _associarOperacao.open({ formId: data.record.id });
                                });
                        //}
                        //if (_permissions.delete) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('AssociarEspecialidade') + '"><i class="fa fa-check-square-o"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    //deleteGeradorFormularios(data.record);
                                    //location.href = '/Mpa/GeradorFormularios/ClonarFormulario/' + data.record.id;
                                    _associarEspecialidade.open({ formId: data.record.id });
                                });
                        //}
                        return $span;
                    }
                },
                nome: {
                    title: app.localize('Nome'),
                    width: '40%'
                },
                dataAlteracao: {
                    title: app.localize('DataAlteracao'),
                    width: '40%',
                    display: function (data) {
                        return moment(data.record.dataAlteracao).format('L LT');
                    }
                }
            }
        });

        function getGeradorFormularios(reload) {
            if (reload) {
                _$GeradorFormulariosTable.jtable('reload');
            } else {
                _$GeradorFormulariosTable.jtable('load', {
                    filtro: $('#GeradorFormulariosTableFilter').val()
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
            //_createOrEditModal.open();
            location.href = '/Mpa/GeradorFormularios/CriarFormulario';
        });

        $('#ExportarGeradorFormulariosParaExcelButton').click(function () {
            _GeradorFormulariosService
                .listarParaExcel({
                    filtro: $('#GeradorFormulariosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
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

        // Campos reservados
        $('#campos-reservados-btn').on('click', function (e) {
            e.preventDefault();

            location.href = '/Mpa/GeradorFormularios/CamposReservados';
        });
    });
})();