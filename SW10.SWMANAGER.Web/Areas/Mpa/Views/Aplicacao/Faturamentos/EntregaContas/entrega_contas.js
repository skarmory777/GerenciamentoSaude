(function () {
    $(function () {

        var _modulo = ModuloFaturamento;
        var _$ContasMedicasTable = $('#entregar-table');
        var _EntregaContasMedicasService = abp.services.app.faturamentoEntregaConta;
        var _EntregaLotesMedicasService = abp.services.app.faturamentoEntregaLote;
        var _ContasMedicasService = abp.services.app.conta;

        _$ContasMedicasTable.jtable({

            title: app.localize('EntregaContas'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            multiselect: true,
            selectingCheckboxes: true,

            actions: { listAction: { method: _ContasMedicasService.listarParaEntrega } },

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
                        if (_modulo.permissoes.delete) {
                            SmweSavior.jtExcluirBtn(data.record, _$ContasMedicasTable, {}, 'conta').appendTo($span);
                        }
                        return $span;
                    }
                }
                ,
                status: {
                    title: app.localize('Status'),
                    width: '4%',
                    display: function (data) {

                        var cor = data.record.statusEntregaCor;
                        return '<div style="text-align:center;" title="' + data.record.statusEntregaDescricao + '">   <span style="display:inline-block; width:20px; height:20px;  text-align:center; background-color: ' + cor + '; border-radius: 25px;">  </span> </div>  ';

                    }
                }
                ,
                usuarioConferenciaNome: {
                    title: app.localize('UsuarioConferencia'),
                    width: '4%'
                    //,
                    //display: function (data) {
                    //    debugger

                    //    var nome = data.record.usuarioConferenciaNome;
                        
                    //    return nome;
                    //}
                }
                ,
                codigoAtendimento: {
                    title: app.localize('Atendimento'),
                    width: '10%',
                    display: function (data) {
                        return data.record.atendimentoCodigo;
                    }
                }
                ,
                paciente: {
                    title: app.localize('Paciente'),
                    width: '22%',
                    display: function (data) {
                        return data.record.pacienteNome;
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
                        return data.record.matricula;
                    }
                }
                ,

                convenio: {
                    title: app.localize('Convenio'),
                    width: '13%',
                    display: function (data) {
                        return data.record.convenioNome;
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
        });

        SmweSavior.jtPopular(_$ContasMedicasTable, _modulo.jtableEntregaContasFiltro.get());

        $('#RefreshContasMedicasButton').click(function (e) {
            e.preventDefault();
            SmweSavior.jtPopular(_$ContasMedicasTable, _modulo.jtableEntregaContasFiltro.get());
        });

        $('#btn-entregar').on('click', function (e) {
            e.preventDefault();
            var selecionados = _$ContasMedicasTable.jtable('registrosSelecionados');
            var contasIds = [];

            selecionados.registros.forEach(function (item, index) {
                var id = item.id;
                contasIds.push(id);
            });

            var entregaContaInput = {
                ContasIds: contasIds
            };

            if(entregaContaInput.ContasIds.length == 0)
            {
                abp.notify.warn(app.localize('NenhumaContaSelecionada'));
                return;
            }

            _EntregaContasMedicasService.criarVarias(entregaContaInput)
              .done(function (data) {
                  abp.notify.info(app.localize('SavedSuccessfully'));
                  SmweSavior.jtPopular(_$ContasMedicasTable, _modulo.jtableEntregaContasFiltro.get());
                  SmweSavior.jtPopular($('#gerar-lotes-table'), _modulo.jtableGerarLotesFiltro.get());
              });

        });

        // Filtros chekbox tipo radio
        $('#is-ambulatorioemergencia-ent').swChecks2Radio('#is-internacao-ent');
        
        $('#is-internacao-ent, #is-ambulatorioemergencia-ent').on('change', function () {
            $('#RefreshContasMedicasButton').click();
        });

        $('#filtro-conferidas-entregar').on('click', function (e) {
            $('#RefreshContasMedicasButton').click();
        });

    });
})();

