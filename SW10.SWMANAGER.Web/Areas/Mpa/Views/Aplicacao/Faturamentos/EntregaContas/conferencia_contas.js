(function () {
    $(function () {

        var _modulo = ModuloFaturamento;
        var _$ConfContasMedicasTable = $('#conferencia-table');
        var _ContasMedicasService = abp.services.app.conta;

        _$ConfContasMedicasTable.jtable({
            title: app.localize('ContasConferir'),
            paging: true,
            sorting: true,
            multiSorting: true,

            selecting: true,
            multiselect: true,
            selectingCheckboxes: true,

            actions: { listAction: { method: _ContasMedicasService.listarNaoConferidas } },

            fields: {
                id: { key: true, list: false },
                actions: {
                    title: app.localize('Actions'),
                    width: '7%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                            .appendTo($span)
                            .click(function () {
                                _modulo.modalConferenciaConta.open({ id: data.record.id });
                            });
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

        SmweSavior.jtPopular(_$ConfContasMedicasTable, _modulo.jtableConferenciaContasFiltro.get());

        $('#RefreshContasMedicasButtonConf').click(function (e) {
            e.preventDefault();
            SmweSavior.jtPopular(_$ConfContasMedicasTable, _modulo.jtableConferenciaContasFiltro.get());
        });

        abp.event.on('app.ConferenciaModalSaved', function () {
            SmweSavior.jtPopular(_$ConfContasMedicasTable, _modulo.jtableConferenciaContasFiltro.get());
            SmweSavior.jtPopular($('#entregar-table'), _modulo.jtableEntregaContasFiltro.get());
        });

        // Filtros chekbox tipo radio
        $('#is-ambulatorioemergencia-conf').swChecks2Radio('#is-internacao-conf');

        $('#is-internacao-conf, #is-ambulatorioemergencia-conf').on('change', function () {
            $('#RefreshContasMedicasButtonConf').click();
        });

        $('#btn-conferir').on('click', function (e) {
            e.preventDefault();
            var sel = _$ConfContasMedicasTable.jtable('registrosSelecionados');
            var input = { ContasIds: [] };

            sel.registros.forEach(function (item, index) {
                var id = item.id;
                input.ContasIds.push(id);
            });

            abp.services.app.conta.conferirContas(input)
                 .done(function () {
                     abp.notify.info(app.localize('ConferenciaConfirmada'));
                     abp.event.trigger('app.CriarOuEditarContaMedicaModalSaved');
                     abp.event.trigger('app.ConferenciaModalSaved');
                 });
        })
    });
})();

