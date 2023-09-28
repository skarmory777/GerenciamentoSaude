(function () {
    $(function () {

        var _modulo = ModuloFaturamento;
        var _$GerarLotesTable = $('#gerar-lotes-table');
        var _EntregaContasMedicasService = abp.services.app.faturamentoEntregaConta;
        var _EntregaLotesMedicasService = abp.services.app.faturamentoEntregaLote;
        var _ContasMedicasService = abp.services.app.conta;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErrosWarnings',
        });

        var _ErrorStringModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErrosString',
        });



        _$GerarLotesTable.jtable({

            title: app.localize('EntregaContas'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            multiselect: true,
            selectingCheckboxes: true,

            actions: { listAction: { method: _EntregaContasMedicasService.listarEntregues } },

            fields: {
                id: { key: true, list: false },
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
                        //if (_modulo.permissoes.delete) {
                        //    SmweSavior.jtExcluirBtn(data.record, _$GerarLotesTable, {}, 'conta').appendTo($span);
                        //}
                        return $span;
                    }
                }
                ,
                status: {
                    title: app.localize('Status'),
                    width: '4%',
                    display: function (data) {

                        var cor = data.record.contaMedica.status.cor;
                        var descricao = data.record.contaMedica.status.descricao;
                        return '<div style="text-align:center;" title="' + descricao + '">   <span style="display:inline-block; width:20px; height:20px;  text-align:center; background-color: ' + cor + '; border-radius: 25px;">  </span> </div>  ';

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

                            //if (data.record.contaMedica.paciente) {
                            //    return data.record.contaMedica.paciente.nomeCompleto;
                            //}
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
                        return data.record.contaMedica.atendimento.matricula;
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
                        if (data.record.contaMedica.dataIncio) {
                            return moment(data.record.contaMedica.dataIncio).format("L LT");
                        }
                    }
                }
                ,
                dataFim: {
                    title: app.localize('DataFim'),
                    width: '8%',
                    display: function (data) {
                        if (data.record.contaMedica.dataFim) {
                            return moment(data.record.contaMedica.dataFim).format("L LT");
                        }
                    }
                }
            }
        });

        //var jtableFiltro = {

        //    Filtro: $('#ContasMedicasTableFilterGerarLotes').val(),
        //    EmpresaId: $('#comboEmpresa-gerar-lotes').val(),
        //    PacienteId: $('#comboPaciente-gerar-lotes').val(),
        //    ConvenioId: $('#comboConveniogerar-lotes').val(),
        //    MedicoId: $('#comboMedico-gerar-lotes').val(),
        //    NumeroGuia: $('#num-guia-gerar-lotes').val(),
        //    IsEmergencia: $('#filtro-emergencia-gerar-lotes').swChkValor(),
        //    IsInternacao: $('#filtro-internacao-gerar-lotes').swChkValor(),

        //    // AtendimentoId : $('#').val() ,
        //    // GuiaId : $('#').val() ,
        //    // StartDate : $('#').val() ,
        //    // EndDate : $('#').val() ,

        //    get() {
        //        this.Filtro = $('#ContasMedicasTableFilterGerarLotes').val();
        //        this.EmpresaId = $('#comboEmpresa-gerar-lotes').val();
        //        this.PacienteId = $('#comboPaciente-gerar-lotes').val();
        //        this.ConvenioId = $('#comboConvenio-gerar-lotes').val();
        //        this.MedicoId = $('#comboMedico-gerar-lotes').val();
        //        this.NumeroGuia = $('#num-guia-gerar-lotes').val();
        //        this.IsEmergencia = $('#filtro-emergencia-gerar-lotes').swChkValor();
        //        this.IsInternacao = $('#filtro-internacao-gerar-lotes').swChkValor();
        //        // AtendimentoId = $('#').val() ;
        //        // GuiaId        = $('#').val() ;
        //        // StartDate     = $('#').val() ;
        //        // EndDate       = $('#').val() ;
        //        return this;
        //    }
        //};

        SmweSavior.jtPopular(_$GerarLotesTable, _modulo.jtableGerarLotesFiltro.get());

        $('#RefreshContasMedicasButtonGerarLotes').click(function (e) {
            e.preventDefault();
            SmweSavior.jtPopular(_$GerarLotesTable, _modulo.jtableGerarLotesFiltro.get());
        });

        $('#btn-gerar-lotes').on('click', function (e) {
            e.preventDefault();
            
           

            var lista = [];

           // var erro = {};

            var selecionados = _$GerarLotesTable.jtable('registrosSelecionados');

            var numProc = $('#gerar-lotes-num-processo').val();
            var codEntr = $('#gerar-lotes-cod-entrega').val();
            if (numProc == '' || codEntr == '') {

                var erro = {};

                erro.CodigoErro = "";
                erro.Descricao = app.localize('NecessarioInformarNumProcCodEntr');
               
                lista.push(erro);
            }

            if (selecionados.registros.length == 0) {

                var erro2 = {};

                erro2.CodigoErro = "";
                erro2.Descricao = app.localize('NenhumaContaSelecionada');

                lista.push(erro2);
            }


            


            if ($('#filtroTipoGuia-gerar-lotes').val()== '') {

                var erro3 = {};

                erro3.CodigoErro = "";
                erro3.Descricao = app.localize('Tipo de Guia não selecionada');

                lista.push(erro3);
            }


            //if (selecionados.registros.length == 0) {

            //    var erro2 = {};

            //    erro2.CodigoErro = "";
            //    erro2.Descricao = app.localize('NenhumaContaSelecionada');

            //    lista.push(erro2);
            //}



            if (lista.length > 0) {
                var erros = JSON.stringify(lista)

                _ErrorStringModal.open({ errosString: lista });
            }
            else {

                abp.message.confirm(
                    app.localize('GerarLoteWarning', 'Registros selecionados'),
                    function (isConfirmed) {
                        if (isConfirmed) {
                            var contasIds = [];

                            selecionados.registros.forEach(function (item, index) {
                                var id = item.id;
                                contasIds.push(id);
                            });

                            var convenioId = $('#comboConvenio-gerar-lotes').val();
                            var entregaContaInput = {
                                LoteId: 0,
                                ContasIds: contasIds,
                                ConvenioId: convenioId,
                                NumeroProcesso: $('#gerar-lotes-num-processo').val(),
                                CodigoEntrega: $('#gerar-lotes-cod-entrega').val(),
                                IsAmbulatorio: $('#is-ambulatorioemergencia-ger').swChkValor(),
                                IsInternacao: $('#is-internacao-ger').swChkValor(),
                                EmpresaId: $('#comboEmpresa-gerar-lotes').val()
                            };

                            _EntregaLotesMedicasService.criarOuEditar(entregaContaInput)
                              .done(function (data) {

                                 

                                  if (data.errors.length > 0) {
                                      _ErrorModal.open({ erros: data.errors, warnings: data.warnings });
                                  }
                                  else {

                                      abp.notify.info(app.localize('SavedSuccessfully'));

                                      if (data != 0) {
                                          // LoteXML = 12
                                          location.href = abp.appPath + 'mpa/RegistroArquivo/DownloadArvivoPorRegistro?registroId=' + data.returnObject + '&registroTabelaId=' + '12' + '&tipoArquivo=' + 'text/xml';
                                      }
                                  }

                                  SmweSavior.jtPopular($('#lotes-gerados-table'), _modulo.jtableLotesGeradosFiltro);
                                  SmweSavior.jtPopular(_$GerarLotesTable, _modulo.jtableGerarLotesFiltro.get());

                              });
                        }
                    }
                );
            }
        });

        $('#btn-cancelar-entregas').on('click', function (e) {
            e.preventDefault();
            var selecionados = _$GerarLotesTable.jtable('registrosSelecionados');

            if (selecionados.registros.length == 0) {
                abp.notify.warn(app.localize('NenhumaContaSelecionada'));
                return;
            }

            var contasIds = [];

            selecionados.registros.forEach(function (item, index) {
                // var id = item.contaMedicaId;
        //        debugger
                var id = item.id;

                contasIds.push(id);
            });

            var cancelaContaInput = {
                //LoteId: 0,
                ContasIds: contasIds
            };

            _EntregaContasMedicasService.cancelarEntregas(cancelaContaInput)
              .done(function (data) {
                  abp.notify.info(app.localize('EntergasCanceladas'));
                  SmweSavior.jtPopular($('#lotes-gerados-table'), _modulo.jtableLotesGeradosFiltro);
                  SmweSavior.jtPopular(_$GerarLotesTable, _modulo.jtableGerarLotesFiltro.get());
              });

        });

        // Filtros chekbox tipo radio
        $('#filtro-emergencia-gerar-lotes').swChecks2Radio('#filtro-internacao-gerar-lotes');

        $('#comboConvenio-gerar-lotes').on('change', function () {
            var selecao = $(this).val();
            if (selecao) {
                $('#btn-gerar-lotes').removeClass('btn-bloqueado');
            } else {
                $('#btn-gerar-lotes').addClass('btn-bloqueado');
            }
            $('#RefreshContasMedicasButtonGerarLotes').click();
        });


        $('#filtroTipoGuia-gerar-lotes').on('change', function () {
            //var selecao = $(this).val();
            //if (selecao) {
            //    $('#btn-gerar-lotes').removeClass('btn-bloqueado');
            //} else {
            //    $('#btn-gerar-lotes').addClass('btn-bloqueado');
            //}
           

            $('#RefreshContasMedicasButtonGerarLotes').click();
        });

        // Filtros chekbox tipo radio
        $('#is-ambulatorioemergencia-ger').swChecks2Radio('#is-internacao-ger');

        $('#is-internacao-ger, #is-ambulatorioemergencia-ger').on('change', function () {
           

            $('#RefreshContasMedicasButtonGerarLotes').click();
        });
    });
})();

