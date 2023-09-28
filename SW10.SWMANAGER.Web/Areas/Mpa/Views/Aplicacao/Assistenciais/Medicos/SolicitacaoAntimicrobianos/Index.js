(function () {

    let solicitacaoAntimicrobiano = abp.services.app.solicitacaoAntimicrobiano

    //var _permissions = {
    //    create: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Create'),
    //    edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Edit'),
    //    'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.CadastrosGlobais.Cep.Delete')
    //};

    var _createOrEditModal = new app.ModalManager({
        viewUrl: abp.appPath + 'Mpa/SolicitacaoAntimicrobianos/CriarOuEditarModal',
        scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacaoAntimicrobianos/_CriarOuEditarModal.js',
        modalClass: 'solicitacaoAntimicrobianoModal'
    });

    $solicitacaoAntimicrobianoTable = $("#solicitacaoAntimicrobianoTable");
    $solicitacaoAntimicrobianoTable.jtable({

        title: app.localize('Solicitacao Antimicrobiano'),
        paging: true,
        sorting: true,
        multiSorting: true,

        actions: {
            listAction: {
                method: solicitacaoAntimicrobiano.listar
            }
        },

        fields: {
            id: {
                key: true,
                list: false
            },
            actions: {
                title: app.localize('Actions'),
                width: '8%',
                sorting: false,
                display: function (data) {
                    var $span = $('<span></span>');
                    //if (_permissions.edit) {
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-print"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                imprimir(data.record.id);
                            });
                    //}

                    //if (_permissions.delete) {
                    //    $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                    //        .appendTo($span)
                    //        .click(function () {
                    //            deleteCeps(data.record);
                    //        });
                    //}

                    return $span;
                }
            },
            "DataSolicitacao": {
                title: app.localize('Data Solicitação'),
                width: '8%',
                display: function (data) {
                    if (data.record.dataSolicitacao) {
                        return moment(data.record.dataSolicitacao).format("DD/MM/YYYY")
                    }
                }
            },
            "TempoProvavelUso": {
                title: app.localize('Tempo Provável Uso'),
                width: '5%',
                display: function (data) {
                    return data.record.tempoProvavelUso;
                }
            },
            "DataMaximaTempoProvavel": {
                title: app.localize('Data Máxima Tempo Provável'),
                width: '5%',
                display: function (data) {
                    if (data.record.dataMaximaTempoProvavel) {
                        return moment(data.record.dataMaximaTempoProvavel).format("DD/MM/YYYY")
                    }
                }
            },
            "atendimento.codigo": {
                title: app.localize('Atendimento'),
                width: '8%',
                display: function (data) {
                    return data.record.atendimento.codigo
                }
            },
            "atendimento.paciente.sisPessoa.nomeCompleto": {
                title: app.localize('Paciente'),
                width: '20%',
                display: function (data) {
                    return data.record.atendimento.paciente.sisPessoa.nomeCompleto
                }
            },
            "medico.sisPessoa.nomeCompleto": {
                title: app.localize('Medico'),
                width: '20%',
                display: function (data) {
                    return data.record.medico.sisPessoa.nomeCompleto
                }
            },
            "prescricaoItem.descricao": {
                title: app.localize('Item'),
                width: '20%',
                display: function (data) {
                    return data.record.prescricaoItem.descricao
                }
            },
            "frequencia.descricao": {
                title: app.localize('Frequencia'),
                width: '10%',
                display: function (data) {
                    if(data.record.frequencia) {
                        return data.record.frequencia.descricao
                    }
                }
            },
            "unidade.descricao": {
                title: app.localize('Unidade'),
                width: '10%',
                display: function (data) {
                    if(data.record.unidade) {
                        return data.record.unidade.descricao
                    }
                }
            },
            "velocidadeInfusao.descricao": {
                title: app.localize('ViaAplicacao'),
                width: '10%',
                display: function (data) {
                    if(data.record.velocidadeInfusao) {
                        return data.record.velocidadeInfusao.descricao
                    }
                }
            },
            "formaAplicacao.descricao": {
                title: app.localize('FormaAplicacao'),
                width: '10%',
                display: function (data) {
                    if(data.record.formaAplicacao) {
                        return data.record.formaAplicacao.descricao
                    }
                }
            }
        }
    });

    function imprimir(id) {
        $.removeCookie("XSRF-TOKEN");
        printJS({
            printable: `/Mpa/AssistenciaisRelatorios/ImprimirSolicitacaoAntimicrobiano?ids=${id}`,
            type: 'pdf'
        })
    }

    function getSolicitacaoAntimicrobianos(reload) {
        if (reload) {
            $solicitacaoAntimicrobianoTable.jtable('reload');
        } else {
            $solicitacaoAntimicrobianoTable.jtable('load');
        }
    }

    getSolicitacaoAntimicrobianos()

})(jQuery);