﻿(function () {

    $(function () {
        $('.modal-dialog').css('width', '1800px');

        const _$PreMovimentoTable = $('#tblSolicitacaoEmprestimo');
        const _preMovimentoService = abp.services.app.estoquePreMovimento;
        var _$filterForm = $('#PreMovimentoFilterForm');

        const _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };

        _$PreMovimentoTable.jtable({
            title: app.localize('Solicitacoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _preMovimentoService.listarSolicitacoesEmprestimos
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '4%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span style="display: flex; justify-content: center;"></span>');
                        if (_permissions.edit && data.record.preMovimentoEstadoId != 5 && data.record.preMovimentoEstadoId != 6 && data.record.preMovimentoEstadoId != 7 && data.record.preMovimentoEstadoId != 8) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    location.href = '/EmprestimoSolicitacao/CriarOuEditarModal/' + data.record.id + '?estTipoOperacaoId=' + $('.selectTipoSolicitacao').val();
                                });
                        }

                        // Somente é possível remover quando é "Pendente"
                        if (_permissions.delete && data.record.preMovimentoEstadoId === 4) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    deletePreMovimentos(data.record);
                                });
                        }

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Imprimir') + '"><i class="fas fa-file-medical" style="color:#36c6d3"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                if (data.record) {
                                    $.removeCookie("XSRF-TOKEN");
                                    printJS({ printable: '/Mpa/Solicitacao/imprimirSolicitacao?preMovimentoId=' + data.record.id, type: 'pdf', showModal: false });
                                }
                            })

                        return $span;
                    }
                },

                EntradaConfirmada: {
                    title: app.localize('Status'),
                    sorting: false,
                    width: '5%',
                    display: function (data) {
                        switch (data.record.preMovimentoEstadoId) {
                            case 1: {
                                return '<span class="label label-info">' + app.localize('Aguardando Confirmação') + '</span>';
                            }
                            case 2: {
                                return '<span class="label label-success">' + app.localize('Confirmado') + '</span>';
                            }
                            case 3: {
                                return '<span class="label label-info">' + app.localize('Pendente informação') + '</span>';
                            }
                            case 4: {
                                return '<span class="label label-info">' + app.localize('Pendente') + '</span>';
                            }
                            case 5: {
                                return '<span class="label label-warning">' + app.localize('Parcialmente Atendido') + '</span>';
                            }
                            case 6: {
                                return '<span class="label label-success">' + app.localize('Totalmente Atendido') + '</span>';
                            }
                            case 7: {
                                return '<span class="label label-danger">' + app.localize('Parcialmente Suspensa') + '</span>';
                            }
                            case 8: {
                                return '<span class="label label-danger">' + app.localize('Suspensa') + '</span>';
                            }
                            default: {
                                return '';
                            }
                        }
                    }
                },

                TipoOperacao: {
                    title: app.localize('TipoOperacao'),
                    width: '5%',
                    display: function (data) {
                        return data.record.tipoOperacao;
                    }
                },

                TipoMovimento: {
                    title: app.localize('TipoMovimento'),
                    width: '5%',
                    display: function (data) {
                        return data.record.tipoMovimento;
                    }
                },

                Empresa: {
                    title: app.localize('Empresa'),
                    width: '15%',
                    display: function (data) {
                        return data.record.empresa;
                    }
                },

                Estoque: {
                    title: app.localize('Estoque'),
                    width: '10%',
                    display: function (data) {
                        return data.record.estoque;
                    }
                },

                Emissao: {
                    title: app.localize('Emissao'),
                    width: '5%',
                    display: function (data) {
                        return moment(data.record.dataEmissaoSaida).format('L');
                    }
                },

                Documento: {
                    title: app.localize('Documento'),
                    width: '5%',
                    display: function (data) {
                        return data.record.documento;
                    }
                },

                Usuario: {
                    title: app.localize('Usuario'),
                    width: '10%',
                    display: function (data) {
                        return data.record.usuario;
                    }
                }
            }
        });

        var _selectedDateRange = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };


        function getPreMovimentos(reload) {
            if (reload) {
                _$PreMovimentoTable.jtable('reload');
            } else {
                _$PreMovimentoTable.jtable('load', {
                    filtro: $('.inputFiltro').val(),
                    peridoDe: _selectedDateRange.startDate,
                    peridoAte: _selectedDateRange.endDate, 
                    estoqueId: $('#EstoqueId').val(),
                    tipoMovimentoId: $('.selectTipoMovimento').val(),
                    tipoOperacaoId: $('.selectTipoSolicitacao').val(),
                    statusMovimentoId: $('.statusMovimento').val()
                });
            }
        }

        function deletePreMovimentos(PreMovimento) {

            abp.message.confirm(
                app.localize('DeleteWarning', PreMovimento.documento),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _preMovimentoService.excluir(PreMovimento.id)
                            .done(function () {
                                getPreMovimentos();
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

        $('#btnCreateNewEmprestimoRecebimento').click(function () {
            location.href = '/EmprestimoSolicitacao/CriarOuEditarModal?estTipoOperacaoId=' + $('.selectTipoSolicitacao').val();
        });

        $('#ExportarPreMovimentoParaExcelButton').click(function () {
            _preMovimentoService
                .listarParaExcel({
                    filtro: $('.inputFiltro').val(),
                    peridoDe: _selectedDateRange.startDate,
                    peridoAte: _selectedDateRange.endDate,
                    fornecedorId: $('#FornecedorId').val(),
                    isEntrada: true,
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#RefreshAtendimentosButton').click(function (e) {
            e.preventDefault();
            getPreMovimentos();
        });

        getPreMovimentos();

        $('#EntradasTableFilter').focus();
        _$filterForm.find('input.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
                getPreMovimentos();
            });

        selectSW('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");

        selectSW('.statusMovimento', "/api/services/app/EstoquePreMovimento/ListarDropdownPreMovimentoEstado");

        $(".selectTipoSolicitacao").select2({ placeholder: "Informe um tipo de solicitacao" })
            .on("change", function (event) {
                event.preventDefault();
                switch ($(".selectTipoSolicitacao").val()) {
                    case "1": {
                        selectSW('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownSaida");
                        break;
                    }
                    case "3": {
                        selectSW('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownEntrada");
                        break;
                    }
                    case "4": {
                        selectSW('.selectTipoMovimento', "/api/services/app/TipoMovimento/ListarDropdownDevolucao");
                        break;
                    }
                    default: {
                        $('.selectTipoMovimento').select2("destroy");
                    }
                }
                
            });


        $('.selectEstoque').change(function () { getPreMovimentos(); });
        $('.selectTipoMovimento').change(function () { getPreMovimentos(); });
        $('.statusMovimento').change(function () { getPreMovimentos(); });

        const debouncedInput = _.debounce(getPreMovimentos, 700);
        $('.inputFiltro').keyup(() => { debouncedInput() });
                       
        
        function openSolicitacao(data) {
            if (_permissions.edit && data.preMovimentoEstadoId != 5 && data.preMovimentoEstadoId != 6 && data.preMovimentoEstadoId != 7 && data.preMovimentoEstadoId != 8) {
                window.open('Solicitacao/CriarOuEditarModal/' + data.id)
            }
            else {
                abp.notify.error("Não é possível acessar a solicitação, pois ela esta bloqueada para acesso.");
            }
        }

        $(".buscarPorSolicitacao").on('keypress', function (event) {
            
            if (event.which === 13) {
                abp.ui.setBusy();
                let codigo = $(this).val();
                if (codigo == null || codigo == "" || codigo == undefined) {
                    abp.notify.error("Não foi possível achar a solicitação correspondente a etiqueta");
                    abp.ui.clearBusy()
                    return;
                }
                _preMovimentoService.buscarPorSolicitacao(codigo).then(res => {
                    if (!res) {
                        abp.notify.error("Não foi possível achar a solicitação correspondente a etiqueta");
                        return;
                    }
                    openSolicitacao(res);

                }).always(() => {
                    abp.ui.clearBusy()
                });
            }
        });
            


    });
})();