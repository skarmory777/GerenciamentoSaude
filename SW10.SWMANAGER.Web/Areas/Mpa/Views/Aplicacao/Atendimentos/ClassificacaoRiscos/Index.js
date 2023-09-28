(function () {
    $(function () {
        var _$ClassificacoesRiscoTable = $('#ClassificacoesRiscoTable');
        var _ClassificacoesRiscoService = abp.services.app.classificacaoRisco;
        var _$filterForm = $('#ClassificacoesRiscoFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Atendimento.ClassificacaoRisco.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.ClassificacaoRisco.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.ClassificacaoRisco.Delete')
        };

        //var _createOrEditModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/ClassificacoesRisco/CriarOuEditarModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/ClassificacoesRisco/_CriarOuEditarModal.js',
        //    modalClass: 'CriarOuEditarClassificacaoRiscoModal'
        //});

        //var _userPermissionsModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/Users/PermissionsModal',
        //    scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/ClassificacoesRisco/_PermissionsModal.js',
        //    modalClass: 'UserPermissionsModal'
        //});

        _$ClassificacoesRiscoTable.jtable({

            title: app.localize('ClassificacoesRisco'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _ClassificacoesRiscoService.listarTodos
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
                                    deleteClassificacoesRisco(data.record.id);
                                });
                        }

                        return $span;
                    }
                }
                ,
                senha: {
                    title: app.localize('Senha'),
                    width: '4%'
                }
                ,
                prioridade: {
                    title: app.localize('Prioridade'),
                    width: '6%'
                }
                ,
                especialidade: {
                    title: app.localize('Especialidade'),
                    width: '12%',
                    display: function (data) {
                        if (data.record.especialidade) {
                            return data.record.especialidade.nome;
                        }
                        else {
                            return '';
                        }
                        
                    }
                }
                ,
                paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.paciente != null) {
                            return data.record.paciente.nomeCompleto;
                        } else {
                            return data.record.preAtendimento.nomeCompleto;
                        }
                    }
                }
                //telefone: {
                //    title: app.localize('Telefone'),
                //    width: '7%'
                //}
                //,
                //observacao: {
                //    title: app.localize('Observacao'),
                //    width: '12%'
                //}
            }
        });

        function getClassificacoesRisco(reload) {
            if (reload) {
                _$ClassificacoesRiscoTable.jtable('reload');
            } else {
                _$ClassificacoesRiscoTable.jtable('load', {
                    filtro: $('#ClassificacoesRiscoTableFilter').val()
                });
            }
        }

        function deleteClassificacoesRisco(ClassificacaoRisco) {
            abp.message.confirm(
                app.localize('DeleteWarning', ClassificacaoRisco.senha),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _ClassificacoesRiscoService.excluir(ClassificacaoRisco)
                            .done(function () {
                                getClassificacoesRisco(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        //function createRequestParams() {
        //    var prms = {};
        //    _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
        //    return $.extend(prms);
        //}

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

        $('#CreateNewClassificacaoRiscoButton').click(function () {
            //   _createOrEditModal.open();
            $('#criar-ou-editar').load('ClassificacaoRiscos/_CriarOuEditarClassificacaoRisco');
        });

        window.salvar = function (form, metodo) {
            var formId = '#' + form;
            var formData = $(formId).serialize();


            $.ajax({
                type: "POST",
                url: metodo,
                dataType: 'text',
                data: formData,
                success: function (result) {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    abp.notify.info(app.localize('ErroSalvar'));
                },
                beforeSend: function () {
                    // $('#salvar-consultor-tabela-campo').attr('disabled', 'disabled');
                },
                complete: function () {
                    //    var paginaId = '#' + pagina;
                    //    metodo = '/Atendimentos/_' + pagina;
                    //    $(paginaId).load(metodo);
                }
            });
        }

        $('#ExportarClassificacoesRiscoParaExcelButton').click(function () {
            _ClassificacoesRiscoService
                .listarParaExcel({
                    filtro: $('#ClassificacoesRiscoTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetClassificacoesRiscoButton, #RefreshClassificacoesRiscoListButton').click(function (e) {
            e.preventDefault();
            getClassificacoesRisco();
        });

        abp.event.on('app.CriarOuEditarClassificacaoRiscoModalSaved', function () {
            getClassificacoesRisco(true);
        });

        getClassificacoesRisco();

        $('#ClassificacoesRiscoTableFilter').focus();
    });
})();