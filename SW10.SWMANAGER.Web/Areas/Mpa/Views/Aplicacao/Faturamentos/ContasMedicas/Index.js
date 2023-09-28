(function () {
    $(function () {
        var _modulo = ModuloFaturamento;
        var _$ContasMedicasTable = $('#ContasMedicasTable');
        var _ContasMedicasService = abp.services.app.conta;

        // Date range filtro
        var _selectedDateRangeLocal = {
            startDate: moment().startOf('day'),
            endDate: moment().endOf('day')
        };

        var _$filterForm = $('#ContasMedicasFilterForm');

        $('.date-range-picker').daterangepicker(
            $.extend(true, app.createDateRangePickerOptions(), _selectedDateRangeLocal),
            function (start, end, label) {
                _selectedDateRangeLocal.startDate = start.format('YYYY-MM-DDT00:00:00Z');
                _selectedDateRangeLocal.endDate = end.format('YYYY-MM-DDT23:59:59.999Z');
            });
        // Fim - date range filtro

        _$ContasMedicasTable.jtable({

            title: app.localize('ContasMedicas'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: { listAction: { method: _ContasMedicasService.listar } },

            fields: {
                id: { key: true, list: false },

                actions: {
                    title: app.localize('Actions'),
                    width: '10%',
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
                            //        SmweSavior.jtExcluirItem(data.record, _$ContasMedicasTable, {}, 'conta');
                            //        // deleteContasMedicas(data.record);
                            //    });

                            SmweSavior.jtExcluirBtn(data.record, _$ContasMedicasTable, {}, 'conta').appendTo($span);
                        }

                        if (data.record.statusEntregaCodigo == 1 || data.record.statusEntregaCodigo == 2) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('RecalcularValores') + '"><i class="fa fa-recycle"></i><i class="fa fa-money-bill-alt"></i>  </button>')
                                   .appendTo($span)
                                   .click(function () {
                                       _ContasMedicasService.recalcularValores(data.record.id)
                                       .done(function () {
                                           abp.notify.info(app.localize('RecalculoRealizadoComSucesso'));
                                       });
                                   });
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

        $('#AdvacedContasMedicasFiltersArea').swPiqueEsconde('#ShowAdvancedFiltersSpan', '#HideAdvancedFiltersSpan');

        $('#CreateNewContaMedicaButton').click(function () {
            _modulo.modalCrudContasMedicas.open();
        });

        $('#ExportarContasMedicasParaExcelButton').click(function () {
            _ContasMedicasService
                .listarParaExcel({
                    filtro: $('#ContasMedicasTableFilter').val(),
                    //sorting: $(''),
                    maxResultCount: $('span.jtable-page-size-change select').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        $('#GetContasMedicasButton, #RefreshContasMedicasListButton').click(function (e) {
            e.preventDefault();
            getContas();
            //_modulo.jtPopular(_$ContasMedicasTable);
        });

        abp.event.on('app.CriarOuEditarContaMedicaModalSaved', function () {
            getContas();
            //_modulo.jtPopular(_$ContasMedicasTable, true);
        });

        var filtroJTableLocal = {
            filtro: $('#ContasMedicasTableFilter').val(),
            empresaId: $('#comboEmpresa option:selected').val(),
            convenioId: $('#comboConvenio option:selected').val(),
            pacienteId: $('#comboPaciente option:selected').val(),
            medicoId: $('#comboMedico option:selected').val(),
            guiaId: $('#filtroTipoGuia option:selected').val(),

            StartDate: _selectedDateRangeLocal.startDate,
            EndDate: _selectedDateRangeLocal.endDate,

            get() {
                this.Filtro = $('#ContasMedicasTableFilter').val();
                this.EmpresaId = $('#comboEmpresa option:selected').val();
                this.convenioId = $('#comboConvenio option:selected').val();
                this.pacienteId = $('#comboPaciente option:selected').val();
                this.medicoId = $('#comboMedico option:selected').val();
                this.guiaId = $('#filtroTipoGuia option:selected').val();
                this.StartDate = _selectedDateRangeLocal.startDate;
                this.EndDate = _selectedDateRangeLocal.endDate;
                return this;
            }
        };

        function getContas() {
            _$ContasMedicasTable.jtable('load', filtroJTableLocal.get());
        }


        getContas();

   //     _modulo.jtPopular(_$ContasMedicasTable);

        $('#ContasMedicasTableFilter').focus();

        $('#RefreshContasMedicasButton').click(function (e) {
            e.preventDefault();
            getContas();
            //_modulo.jtPopular(_$ContasMedicasTable);
        });

        // Filtros chekbox tipo radio
        $('#is-ambulatorioemergencia-amb').swChecks2Radio('#is-internacao-int');
        //$("#tipo-atendimento-amb").click();

    });
})();