(function () {
    $(function () {
        var _$NotasFiscaisTable = $('#NotasFiscaisTable');
        var _notasFiscaisService = abp.services.app.notaFiscal;
        var _$filterForm = $('#NotasFiscaisFilterForm');

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/NotasFiscais/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Controladorias/NotasFiscais/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarNotaFiscalModal'
        });

        var _selectedDateRange = {
            startDate: moment().add('-6', 'day').startOf('day'),
            endDate: moment().endOf('day')
        };

        _$filterForm.find('input.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRange),
            function (start, end, label) {
                _selectedDateRange.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRange.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });

        _$NotasFiscaisTable.jtable({

            title: app.localize('NotasFiscais'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _notasFiscaisService.listarIndex
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '12%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('ExibirNota') + '"><i class="fa fa-search"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _createOrEditModal.open({ id: data.record.id });
                            });
                        $('<button id="btn-consultar-situacao" class="btn btn-default btn-xs" title="' + app.localize('SituacaoNota') + '"><i class="icon-refresh"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                situacaoNota(data.record.chaveAcesso);
                            });
                        if (!data.record.isManifestacaoDestinatario) {
                            $('<button id="btn-confirmar-nota" class="btn btn-default btn-xs" title="' + app.localize('ConfirmarNota') + '"><i class="icon-check"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    confirmarNota(data.record.chaveAcesso);
                                });
                        }
                        return $span;
                    }
                },
                isManifestacaoDestinatario: {
                    title: app.localize('NotaConfirmada'),
                    sorting: true,
                    width: '8%',
                    display: function (data) {
                        if (data.record.isManifestacaoDestinatario) {
                            return '<div style="text-align:center;">' + '<span class="label label-success content-center text-center">' + app.localize('Yes') + '</span>' + '</div>';
                        } else {
                            return '<div style="text-align:center;">' + '<span class="label label-default content-center text-center">' + app.localize('No') + '</span>' + '</div>';
                        }
                    }
                },
                cStat: {
                    title: app.localize('Situacao'),
                    sorting: true,
                    width: '8%',
                    display: function (data) {
                        switch (data.record.cStat) {
                            case 100:
                                return '<span class="label label-success">' + app.localize('Autorizada') + '</span>';
                                break;
                            case 101:
                                return '<span class="label label-danger">' + app.localize('Cancelada') + '</span>';
                                break;
                            case 102:
                                return '<span class="label label-warning">' + app.localize('Inutilizada') + '</span>';
                                break;
                            case 110:
                                return '<span class="label label-default">' + app.localize('Denegada') + '</span>';
                                break;
                            default:
                                return '<span class="label label-primary">' + app.localize('NaoConsultada') + '</span>';
                                break;
                        }
                    }
                },
                numero: {
                    title: app.localize('NotaFiscal'),
                    sorting: true,
                    width: '8%'
                },
                chaveAcesso: {
                    title: app.localize('ChaveAcesso'),
                    sorting: true,
                    width: '20%'
                },
                valorNota: {
                    title: app.localize('ValorNota'),
                    sorting: true,
                    width: '8%',
                    display: function (data) {
                        display_value = data.record.valorNota;
                        if (!display_value) { display_value = ''; } //handles showing blank cell for null values
                        return '<div style="text-align:right;">' + display_value.toFixed(2) + '</div>';
                    },
                },
                dataEmissao: {
                    title: app.localize('DataEmissao'),
                    sorting: true,
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.dataEmissao).format('L LT');
                    }
                },
                dataRecebimento: {
                    title: app.localize('DataRecebimento'),
                    sorting: true,
                    width: '10%',
                    display: function (data) {
                        return moment(data.record.dataRecebimento).format('L LT');
                    }
                },
                nomeEmpresa: {
                    title: app.localize('Destinatario'),
                    sorting: true,
                    width: '10%',
                },
                cnpj: {
                    title: app.localize('Cnpj'),
                    sorting: true,
                    width: '10%',
                },
                nome: {
                    title: app.localize('Nome'),
                    sorting: true,
                    width: '15%'
                    //display: function (data) {
                    //    return data.record.nome.length > 40
                    //        ? data.record.nome.substr(0, 40) + "..."
                    //        : data.record.nome;
                    //}
                },
                nsu: {
                    title: app.localize('Nsu'),
                    sorting: true,
                    width: '10%'
                }
            }
        });

        function getNotasFiscais() {
            //console.log("getNotasFiscais");
            if ($('#cbo-empresas').empresaId != null) {
                setEmpresaTempData();
            }
            
            if ($('#cbo-empresas').val().length > 0) {
                $('#btn-sincronizar').removeAttr('disabled');
            }
            else {
                $('#btn-sincronizar').attr('disabled', 'disabled');
            }

            _$NotasFiscaisTable.jtable('load', createRequestParams());
        }

        function createRequestParams() {
            var prms = {};
            _$filterForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
            return $.extend(prms, _selectedDateRange);
        }

        $('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedNotasFiscaisFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedNotasFiscaisFiltersArea').slideUp();
        });

        $('#ExportarNotasFiscaisParaExcelButton').click(function () {
            _notasFiscaisService
                .listarParaExcel({
                    filtro: $('#Filtro').val(),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetNotasFiscaisButton, #RefreshNotasFiscaisButton').click(function (e) {
            //console.log("RefreshNotasFiscaisButton");
            e.preventDefault();
            getNotasFiscais();
        });

        getNotasFiscais();

        $('#Filtro').focus();


        $('#btn-sincronizar').click(function (e) {
            e.preventDefault();
            if ($('#cbo-empresas').val().length > 0) {
                $('#btn-sincronizar').removeAttr('disabled');
                sincronizarNotasSefaz($('#user-name').val(), $('#password').val());
            }
            else {
                $('#btn-sincronizar').attr('disabled', 'disabled');
                abp.notify.error(app.localize('EmpresaNaoSelecionada'));
                //return false;
            }
        });

        function setEmpresaTempData() {
            $.ajax({
                data: { empresaId: $('#cbo-empresas').val() },
                url: '/Empresas/SetEmpresaTempData',
                cache: false,
                async: false
            });

        }

        $('#cbo-empresas').change(function (e) {
            e.preventDefault();
            $('#GetNotasFiscaisButton').trigger('click');
            if ($(this).val().length > 0) {
                $('#btn-sincronizar').removeAttr('disabled');
            }
            else {
                $('#btn-sincronizar').attr('disabled', 'disabled');
            }
            setEmpresaTempData();
        });

        //$('#btn-nfe-distribuicao-dfe').click(function (e) {
        //    e.preventDefault();
        //    location.href = '/NotasFiscais/NFeDistribuicaoDfe';
        //});

        //function sincronizarNotasSefaz(username, password) {
        //    if (confirm(app.localize('ConfirmSincronizarNotaFiscal'))) {
        //        $.ajax({
        //            url: "/NotasFiscais/Sincronizar",
        //            data: {
        //                "user": username,
        //                "pass": password
        //            },
        //            type: "GET",
        //            beforeSend: function () {
        //                $('#btn-sincronizar').buttonBusy(true);
        //            },
        //            complete: function () {
        //                $('#btn-sincronizar').buttonBusy(false);
        //            },
        //            success: function (response) {
        //                var aResponse = response.result.split(',');
        //                if (aResponse.indexOf('EXCEPTION') != -1) {
        //                    abp.notify.info(app.localize('ErroSincronizarNotaFiscal'));
        //                    return false;
        //                }
        //                if (aResponse.indexOf('Nenhum') != -1) {
        //                    abp.notify.info(app.localize('NenhumRegistroEncontrado'));
        //                    return false;
        //                }
        //                buscarNotasWebApi(username, password);
        //            }
        //        });
        //    }
        //}

        function sincronizarNotasSefaz() {
            if (confirm(app.localize('ConfirmSincronizarNotaFiscal'))) {
                $.ajax({
                    url: "/NotasFiscais/NfeDistribuicaoDfe",
                    type: "GET",
                    timeout: 864000,
                    cache: false,
                    async: true,
                    beforeSend: function () {
                        $('#btn-sincronizar').buttonBusy(true);
                    },
                    complete: function () {
                        $('#btn-sincronizar').buttonBusy(false);
                    },
                    success: function (response) {
                        abp.notify.info(response);
                        getNotasFiscais();
                    }
                });
            }
        }

        function confirmarNota(chaveAcesso) {
            if (confirm(app.localize('InputConfirmaNota'))) {
                $.ajax({
                    data: {
                        chaveAcesso: chaveAcesso //$('#chave-acesso').val()
                    },
                    dataType: 'json',
                    contentType: 'application/json',
                    dataContext: 'application/json;charset=utf-8',
                    url: '/mpa/NotasFiscais/ConfirmarNota',
                    async: true,
                    type: 'GET',
                    cache: false,
                    success: function (data) {
                        var result = JSON.stringify(data.result);
                        if (result.indexOf('ERRO') !== -1) {
                            abp.notify.error(app.localize("Erro") + "<br />" + data.result.replace('ERRO:', ''));
                            getNotasFiscais();
                            return false;
                        }
                        else {
                            abp.notify.success(app.localize("NotaConfirmada"));
                        }
                    },
                    beforeSend: function () {
                        $('#btn-confirmar-nota').buttonBusy(true)
                    },
                    complete: function () {
                        $('#btn-confirmar-nota').buttonBusy(false)

                    }
                });
            }
        }

        function situacaoNota(chaveAcesso) {
            $.ajax({
                data: {
                    chaveAcesso: chaveAcesso //$('#chave-acesso').val()
                },
                dataType: 'json',
                contentType: 'application/json',
                dataContext: 'application/json;charset=utf-8',
                url: '/mpa/NotasFiscais/AtualizarSituacaoNotaFiscal',
                async: true,
                type: 'GET',
                cache: false,
                success: function (data) {
                    var result = JSON.stringify(data.result);
                    if (result.indexOf('ERRO') !== -1) {
                        abp.notify.error(app.localize("Erro") + "<br />" + data.result.replace('ERRO:', ''));
                        return false;
                    }
                    else {
                        abp.notify.success(app.localize("SituacaoAtualizada"));
                    }
                    getNotasFiscais();
                },
                beforeSend: function () {
                    $('#btn-confirmar-situacao').buttonBusy(true)
                },
                complete: function () {
                    $('#btn-confirmar-situacao').buttonBusy(false)

                }
            });

        }

        //function buscarNotasWebApi(username, password) {
        //    var filtro = "origem=2 and dtemissao>='" + moment(_selectedDateRange.startDate).format() + "' and dtemissao<='" + moment(_selectedDateRange.endDate).format() + "'";
        //    var settings = {
        //        "async": true,
        //        "crossDomain": true,
        //        "data": {
        //            "cnpj": "02746015000129",
        //            "grupo": "AMERICAN",
        //            "campos": "handle,chave,situacao,codnf,nrecibo,nprotenvio,nprotcanc,nprotinutil,nregdpec,modoentrada,modosaida,cnpj,motivo,dtautorizacao,dtcadastro,dtcancelamento,dtemissao,impresso,envemail,email,docdestinatario,nomedestinatario,idgrupo,idintegracao,nlote,numero,dhdpec,nomegrupo,eventos,ambiente,impressora,origem,sincronizadopm,cstat,importado,destinada,xmldestinatario",
        //            "filtro": filtro
        //        },
        //        "url": "https://managersaas.tecnospeed.com.br:8081/ManagerAPIWeb/nfe/consulta",
        //        "method": "GET",
        //        "headers": {
        //            "authorization": "Basic " + btoa(username + ":" + password),
        //            "cache-control": "no-cache"
        //        }
        //    }

        //    $.ajax(settings)
        //        .done(function (response) {
        //            var vLines = response.split('\r\n');
        //            var exTest = response.indexOf("EXCEPTION");
        //            var emptyTest = response.indexOf("Nenhum registro encontrado");
        //            if (exTest != -1) {
        //                abp.notify.info(app.localize('ErroSincronizarNotaFiscal'));
        //            }
        //            else if (emptyTest != -1) {
        //                abp.notify.info(app.localize('NenhumRegistroEncontrado'));
        //            }
        //            else {
        //                $('#btn-sincronizar').buttonBusy(true)
        //                _notasFiscaisService.sincronizar(vLines)
        //                .done(function (data) {
        //                    getNotasFiscais();
        //                })
        //                .always(function () {
        //                    $('#btn-sincronizar').buttonBusy(false)
        //                });
        //            }
        //        });
        //}

        $('body').addClass('page-sidebar-closed');

        $('.page-sidebar-menu').addClass('page-sidebar-menu-closed');

    });
})();