    (function ($) {
    app.modals.SelecionarSolicitacaoPorPrescricaoModal = function () {
        let modalManager;
        const _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        };
        const selecionarSolicitacaoTable = $("#selecionarSolicitacaoTable");
        this.init = function (_modalManager) {
            modalManager = _modalManager;
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $(_modalManager.getModal()).find(".modal-dialog").css({ 'width': '80%'});
            createTable();
            abp.ui.clearBusy()
        }
        
        function createTable() {
            selecionarSolicitacaoTable.jtable({

                title: app.localize('Solicitacoes'),
                paging: false,
                sorting: false,
                multiSorting: false,

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
                            let $span = $('<span style="display: flex; justify-content: center;"></span>');
                            if (_permissions.edit && data.record.preMovimentoEstadoId != 5 && data.record.preMovimentoEstadoId != 6 && data.record.preMovimentoEstadoId != 7 && data.record.preMovimentoEstadoId != 8) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                    .appendTo($span)
                                    .click(function () {
                                        //_createOrEditModal.open({ id: data.record.id });
                                        window.open('Solicitacao/CriarOuEditarModal/' + data.record.id)
                                    });
                            }

                            return $span;
                        }
                    },


                    EntradaConfirmada: {
                        title: app.localize('Status'),
                        width: '6%',
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
                        width: '6%',
                        display: function (data) {
                            return data.record.tipoOperacao;
                        }
                    },
                    TipoMovimento: {
                        title: app.localize('TipoMovimento'),
                        width: '6%',
                        display: function (data) {
                            return data.record.tipoMovimento;
                        }
                    },
                    Empresa: {
                        title: app.localize('Empresa'),
                        width: '10%',
                        display: function (data) {
                            return data.record.empresa;
                        }
                    },

                    Estoque: {
                        title: app.localize('Estoque'),
                        width: '15%',
                        display: function (data) {
                            return data.record.estoque;
                        }
                    },

                    Emissao: {
                        title: app.localize('Emissao'),
                        width: '7%',
                        display: function (data) {
                            return moment(data.record.dataEmissaoSaida).format('L');
                        }
                    },
                    Documento: {
                        title: app.localize('Documento'),
                        width: '10%',
                        display: function (data) {
                            return data.record.documento;
                        }
                    },
                    NomePaciente: {
                        title: app.localize('Paciente'),
                        width: '10%',
                        display: function (data) {
                            return data.record.nomePaciente;
                        }
                    },
                    HoraPrescrita: {
                        title: app.localize('HoraPrescrita'),
                        width: '5%',
                        display: function (data) {
                            if (data.record.horaPrescrita) {
                                return moment(data.record.horaPrescrita).format('L HH:mm');
                            }
                        }
                    }
                }

            });

            loadTable();
        }
        
        function loadTable(){
            const data = modalManager.getArgs();
            debugger
            _.forEach(data, (item) => {
                selecionarSolicitacaoTable.jtable('addRecord',{record:item, clientOnly :true, animationsEnabled :false});
            })
            
        }

    };
})(jQuery);