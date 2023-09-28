(function () {
    $(function () {
        var _$OrcamentosTable = $('#OrcamentosTable');
        var _OrcamentosService = abp.services.app.orcamento;
        var _$filterForm = $('#OrcamentosFilterForm');

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Atendimento.Orcamento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Atendimento.Orcamento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Atendimento.Orcamento.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Orcamentos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Atendimentos/Orcamentos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarOrcamentoModal'
        });

        _$OrcamentosTable.jtable({

            title: app.localize('Orcamentos'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _OrcamentosService.listarTodos
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
                                    deleteOrcamentos(data.record.id);
                                });
                        }

                        return $span;
                    }
                }
                ,
                descricao: {
                    title: app.localize('Descricao'),
                    width: '8%'
                }
                ,
                data: {
                    title: app.localize('Data'),
                    width: '4%',
                    display: function (data) {
                        return moment(data.record.data).format('L');
                    }
                }
                ,

                convenio: {
                    title: app.localize('Convenio'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.convenio) {
                            if (data.record.convenio.sisPessoa)
                                return data.record.convenio.sisPessoa.nomeFantasia;
                        }

                    }
                }
                ,
                plano: {
                    title: app.localize('Plano'),
                    width: '7%',
                    display: function (data) {
                        if (data.record.plano)
                            if (data.record.plano.convenio)
                                return data.record.plano.descricao;
                    }
                }
                ,
                empresa: {
                    title: app.localize('Empresa'),
                    width: '7%',
                    display: function (data) {
                        return data.record.empresa.nomeFantasia;
                    }
                }
                ,
                paciente: {
                    title: app.localize('Paciente'),
                    width: '20%',
                    display: function (data) {
                        if (data.record.paciente)
                            return data.record.paciente.nomeCompleto;
                    }
                    //if (data.record.paciente != null) {
                    //    return data.record.paciente.nomeCompleto;
                    //} else if (data.record.preAtendimento != null) {
                    //    return data.record.preAtendimento.nomeCompleto;
                    // }

                }
            }
        });

        function getOrcamentos(reload) {
            if (reload) {
                _$OrcamentosTable.jtable('reload');
            } else {
                _$OrcamentosTable.jtable('load', {
                    filtro: $('#OrcamentosTableFilter').val()
                });
            }
        }

        function deleteOrcamentos(Orcamento) {
            abp.message.confirm(
                app.localize('DeleteWarning', Orcamento.senha),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _OrcamentosService.excluir(Orcamento)
                            .done(function () {
                                getOrcamentos(true);
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

        $('#CreateNewOrcamentoButton').click(function () {
            _createOrEditModal.open();
            //$('#criar-ou-editar').load('Orcamentos/_CriarOuEditarOrcamento');
        });

        // Salvar
        window.salvar = function (form, metodo) {
            var formId = '#' + form;
            var formData = $(formId).serialize();

            //console.log(formData);

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

        $('#ExportarOrcamentosParaExcelButton').click(function () {
            _OrcamentosService
                .listarParaExcel({
                    filtro: $('#OrcamentosTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetOrcamentosButton, #RefreshOrcamentosListButton').click(function (e) {
            e.preventDefault();
            getOrcamentos();
        });

        abp.event.on('app.CriarOuEditarOrcamentoModalSaved', function () {
            getOrcamentos(true);
        });

        getOrcamentos();

        $('#OrcamentosTableFilter').focus();
    });
})();