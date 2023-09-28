(function () {
    $(function () {
        var _modulo = ModuloFaturamento;
        var _$ContasMedicasTable = $('#ContasMedicasTable');
        var _ContasMedicasService = abp.services.app.conta;

        _$ContasMedicasTable.jtable({

            title: app.localize('EntregaContas'),
            paging: true,
            sorting: true,
            multiSorting: true,
            selecting: true,
            multiselect: true,
            selectingCheckboxes: true,

            actions: {
                listAction: {
                    method: _ContasMedicasService.listar
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
                        return $span;
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
                        return ''; //data.record.convenioNome;
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

        $('#GetContasMedicasButton, #RefreshContasMedicasListButton, #RefreshContasMedicasButton').click(function (e) {
            e.preventDefault();
            _modulo.jtPopular(_$ContasMedicasTable);
        });

        abp.event.on('app.CriarOuEditarContaMedicaModalSaved', function () {
            _modulo.jtPopular(_$ContasMedicasTable, true);
        });

     //   _modulo.jtPopular(_$ContasMedicasTable);

        $('#ContasMedicasTableFilter').focus();


        selectSW('.selectGuia', "/api/services/app/guia/ListarDropdown");

        
    });
})();