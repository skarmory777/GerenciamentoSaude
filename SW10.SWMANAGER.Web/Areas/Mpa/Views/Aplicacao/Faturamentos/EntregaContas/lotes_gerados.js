(function () {
    $(function () {
        var _modulo = ModuloFaturamento;
        var _$LotesGeradosTable = $('#lotes-gerados-table');
        var _EntregaContasMedicasService = abp.services.app.faturamentoEntregaConta;
        var _EntregaLotesMedicasService = abp.services.app.faturamentoEntregaLote;
        var _ContasMedicasService = abp.services.app.conta;
        var _tissService = abp.services.app.tISS;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErrosWarnings',
        });

        _$LotesGeradosTable.jtable({

            title: app.localize('Lotes'),
            paging: true,
            sorting: true,
            multiSorting: true,
            openChildAsAccordion: true,

            actions: { listAction: { method: _EntregaLotesMedicasService.listar } },

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
                        if (_modulo.permissoes.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function () {
                                    _modulo.modalCrudContasMedicas.open({ id: data.record.id });
                                });
                        }
                        if (_modulo.permissoes.delete) {
                            //$('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            //    .appendTo($span)
                            //    .click(function () {
                            //        SmweSavior.jtExcluirItem(data.record, _$LotesGeradosTable, {}, 'conta');
                            //        // deleteContasMedicas(data.record);
                            //    });

                            // AO DELETAR (DESFAZER) LOTE, NECESSARIO ATUALIZAR O STATUS DE TODAS AS CONTAS NELE CONTIDAS!!!
                            SmweSavior.jtExcluirBtn(data.record, _$LotesGeradosTable, {}, 'faturamentoEntregaLote').appendTo($span);
                        }

                        if (data.record.isLoteGerado) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Download') + '"><i class="fa fa-download"></i></button>')
                                   .appendTo($span)
                                   .click(function () {
                                       ExportarLoteXML(data.record);
                                   });
                        }
                        else {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('GerarLote') + '"><i class="fa fa-retweet"></i></button>')
                                  .appendTo($span)
                                  .click(function () {
                                      gerarLote(data.record.id);
                                  });
                        }

                        return $span;
                    }
                }

                // Colunas JTable

                // JTable filha (Entregas do respectivo Lote)
                ,
                ContasMedicas: {
                    title: '',
                    width: '5%',
                    sorting: false,
                    edit: false,
                    create: false,
                    openChildAsAccordion: true,

                    // Selecting nao ta funcionando
                    selecting: true,
                    multiselect: true,
                    selectingCheckboxes: true,


                    display: function (dataLote) {
                        var $img = $('<span class="btn">Entregas</span>');
                        $img.click(function () {
                            _$LotesGeradosTable.jtable('openChildTable',
                                    $img.closest('tr'),
                                    {
                                        title: 'Entregas - ' + 'Lote ' + dataLote.record.codEntregaLote || '',
                                        actions: { listAction: { method: _EntregaContasMedicasService.listarParaLotesGerados } },

                                        fields: {
                                            id: {
                                                type: 'hidden',
                                                defaultValue: dataLote.record.id
                                            }
                                            ,
                                            actions: {
                                                title: app.localize('Actions'),
                                                width: '7%',
                                                sorting: false,
                                                display: function (data) {
                                                    var $span = $('<span></span>');
                                                    var data2 = data;

                                                    //if (_modulo.permissoes.edit) {
                                                    //    $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                                    //        .appendTo($span)
                                                    //        .click(function () {
                                                    //            _modulo.modalCrudContasMedicas.open({ id: data.record.id });
                                                    //        });
                                                    //}
                                                    if (_modulo.permissoes.delete) {

                                                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Remover') + '"><i class="fa fa-reply"></i></button>')
                                                            .appendTo($span)
                                                            .click(function () {
                                                                _EntregaContasMedicasService.removerDoLote(data2.record.id)
                                                                .done(function () {

                                                                    abp.notify.info(app.localize('RemovidaDoLote'));

                                                                })

                                                            });
                                                    }

                                                    return $span;
                                                }
                                            }
                                            ,
                                            codigoAtendimento: {
                                                title: app.localize('Atendimento'),
                                                width: '10%',
                                                display: function (data) {
                                                    if (data.record.contaMedica) {
                                                        if (data.record.contaMedica.atendimento) {
                                                            return data.record.contaMedica.atendimento.codigo;
                                                        }
                                                    }
                                                }
                                            }
                                            ,
                                            paciente: {
                                                title: app.localize('Paciente'),
                                                width: '22%',
                                                display: function (data) {
                                                    if (data.record.contaMedica) {
                                                        if (data.record.contaMedica.atendimento) {
                                                            if (data.record.contaMedica.atendimento.paciente) {
                                                                return data.record.contaMedica.atendimento.paciente.nomeCompleto;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            ,
                                            valorInicial: {
                                                title: app.localize('ValorInicial'),
                                                width: '13%',
                                                display: function (data) {
                                                    return ''; //data.record.convenioNome;
                                                }
                                            }
                                            ,
                                            matricula: {
                                                title: app.localize('Matricula'),
                                                width: '13%',
                                                display: function (data) {
                                                    return data.record.contaMedica.matricula;
                                                }
                                            }
                                            ,
                                            convenio: {
                                                title: app.localize('Convenio'),
                                                width: '13%',
                                                display: function (data) {

                                                    if (data.record.contaMedica) {
                                                        if (data.record.contaMedica.convenio) {
                                                            return data.record.contaMedica.convenio.nomeFantasia;
                                                        }
                                                    }


                                                }
                                            }
                                            ,
                                            dataIncio: {
                                                title: app.localize('DataInicio'),
                                                width: '8%',
                                                display: function (data) {
                                                    if (data.record.dataIncio) {
                                                        return moment(data.record.dataIncio).format("L LT");
                                                    }
                                                }
                                            }
                                            ,
                                            dataFim: {
                                                title: app.localize('DataFim'),
                                                width: '8%',
                                                display: function (data) {
                                                    if (data.record.dataFim) {
                                                        return moment(data.record.dataFim).format("L LT");
                                                    }
                                                }
                                            }
                                        }
                                    }, function (data) {
                                        data.childTable.jtable('load', { LoteId: dataLote.record.id });
                                    });
                        });
                        //Return image to show on the person row
                        return $img;
                    }
                }

                // fim - jtable filha (entregas do lote)

                ,
                convenio: {
                    title: app.localize('Convenio'),
                    width: '7%',
                    display: function (data) {
                        return data.record.convenio && data.record.convenio.nomeFantasia;// data.record.convenio.sisPessoa && data.record.convenio.sisPessoa.nomeCompleto || '';
                    }
                }
                ,
                codEntregaLote: {
                    title: app.localize('CodEntrega'),
                    width: '7%'
                }
                ,
                numeroProcesso: {
                    title: app.localize('NumProcesso'),
                    width: '7%'
                }
                //,
                //dataInicial: {
                //    title: app.localize('Inicio'),
                //    width: '8%',
                //    display: function (data) {
                //        if (data.record.dataInicial) {
                //            return moment(data.record.dataInicial).format("L LT");
                //        }
                //    }
                //}
                //,
                //dataFinal: {
                //    title: app.localize('Fim'),
                //    width: '8%',
                //    display: function (data) {
                //        if (data.record.dataFinal) {
                //            return moment(data.record.dataFinal).format("L LT");
                //        }
                //    }
                //}
                ,
                dataEntrega: {
                    title: app.localize('Entrega'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.dataEntrega) {
                            return moment(data.record.dataEntrega).format("L LT");
                        }
                    }
                }
                ,
                valorFatura: {
                    title: app.localize('Fatura'),
                    width: '8%'
                }
                //,
                //valorTaxas: {
                //    title: app.localize('Taxas'),
                //    width: '8%'
                //}
                ,
                isAmbulatorio: {
                    title: app.localize('Tipo'),
                    width: '8%',
                    display: function (data) {
                        return data.record.isAmbulatorio && 'Amb./Emergência' || 'Internação';
                    }
                }
                //,
                //desativado: {
                //    title: app.localize('Ativo'),
                //    width: '8%',
                //    display: function (data) {
                //        return !data.record.desativado;
                //    }
                //}

            }
        });

        SmweSavior.jtPopular(_$LotesGeradosTable, _modulo.jtableLotesGeradosFiltro.get());

        $('#RefreshContasMedicasButtonLotesGerados').click(function (e) {
            e.preventDefault();
            SmweSavior.jtPopular(_$LotesGeradosTable, _modulo.jtableLotesGeradosFiltro.get());
        });

        // Filtros chekbox tipo radio
        $('#is-ambulatorioemergencia-lot').swChecks2Radio('#is-internacao-lot');

        $('#is-internacao-lot, #is-ambulatorioemergencia-lot').on('change', function () {
            $('#RefreshContasMedicasButtonLotesGerados').click();
        });


        function ExportarLoteXML(record) {
            // LoteXML = 12
            location.href = abp.appPath + 'mpa/RegistroArquivo/DownloadArvivoPorRegistro?registroId=' + record.id + '&registroTabelaId=' + '12' + '&tipoArquivo=' + 'text/xml';

        }


        function gerarLote(loteId) {
            _tissService.gerarLoteXML(loteId)
                             .done(function (data) {

                                

                                 if (data.errors.length > 0) {
                                     _ErrorModal.open({ erros: data.errors, warnings: data.warnings });
                                 }
                                 else {

                                     abp.notify.info(app.localize('SavedSuccessfully'));

                                     // LoteXML = 12
                                     location.href = abp.appPath + 'mpa/RegistroArquivo/DownloadArvivoPorRegistro?registroId=' + loteId + '&registroTabelaId=' + '12' + '&tipoArquivo=' + 'text/xml';

                                     SmweSavior.jtPopular($('#lotes-gerados-table'), _modulo.jtableLotesGeradosFiltro);
                                     SmweSavior.jtPopular(_$GerarLotesTable, _modulo.jtableGerarLotesFiltro.get());

                                 }



                             });
        }

    });
})();

