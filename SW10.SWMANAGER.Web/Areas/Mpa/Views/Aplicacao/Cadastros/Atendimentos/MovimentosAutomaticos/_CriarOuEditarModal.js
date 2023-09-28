
(function ($) {
    $(function () {

        $(document).ready(function () {

            CamposRequeridos();
        });

        var _preMovimentoService = abp.services.app.estoquePreMovimento;
       

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        var _createOrEditLoteValidadeProdutoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/PreMovimentos/InformarLoteValidadeProdutoModal',
            modalClass: 'EstoquePreMovimentoLoteValidadeProdutoViewModel'
        });

        $('#salvarMovimentoAutomatico').click(function (e) {
            e.preventDefault()
                     
            var _$movimentoAutomaticoInformationsForm = $('form[name=movimentoAutomaticoInformationsForm');

            _$movimentoAutomaticoInformationsForm.validate();

            if (!_$movimentoAutomaticoInformationsForm.valid()) {
                return;
            }

            var movimentoAutomatico = _$movimentoAutomaticoInformationsForm.serializeFormToObject();
           
           

        //  //  _preMovimentoService.criarOuEditar(preMovimento)
        //    //      .done(function (data) {

        //    preMovimento.fornecedorId = preMovimento.FornecedorId;


            $.ajax({
                url: "/MovimentosAutomaticos/Salvar",
                data: { input: movimentoAutomatico },
                type: "POST",
                timeout: 864000,
                cache: false,
                async: false,
                beforeSend: function (result) {
                   
                    //  $('#btn-sincronizar').buttonBusy(true);
                },
                complete: function (result) {
                   
                    //$('#btn-sincronizar').buttonBusy(false);
                },
                success: function (result) {

                   

                    if (result.result.errors.length > 0) {
                        _ErrorModal.open({ erros: result.result.errors });
                    }
                    else {

                        abp.notify.info(app.localize('SavedSuccessfully'));
                        location.href = '/mpa/MovimentosAutomaticos';
                        $('#id').val(result.result.returnObject.id);
                    }

                }
                ,error: function (result, execption,a,b,c,d) {
               
                    //$('#btn-sincronizar').buttonBusy(false);
                },

                fail: function (result) {
                   
                    //$('#btn-sincronizar').buttonBusy(false);
                },





                //.always(function () {
                //  _modalManager.setBusy(false);
                //});

            });
        });

        $('.close').on('click', function () {
            location.href = '/mpa/MovimentosAutomaticos';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/MovimentosAutomaticos';
        });


        var _$tiposGuiaTable = $('#tiposGuiaTable');

        _$tiposGuiaTable.jtable({

            title: app.localize('TipoGuias'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,
        
            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                      
                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                deleteTipoGuia(data.record);
                            });

                        return $span;
                    }
                },

                //Codigo: {
                //    title: app.localize('Codigo'),
                //    width: '30%',
                //    display: function (data) {
                //        return data.record.TipoGuiaDescricao;
                //    }
                //},

                Descricao: {
                    title: app.localize('Descricao'),
                    width: '70%',
                    display: function (data) {
                        return data.record.TipoGuiaDescricao;
                    }
                },
            }
        });

        var listaTiposGuias = [];
        
        $('#inserirTipoGuia').click(function (e) {
            e.preventDefault();

            if ($('#tipoGuiaId').val() == '' || $('#tipoGuiaId').val() == null) {
                return;
            }


          //  var _$empresaInformationsForm = $('form[name=EmpresaInformationsForm]');
           // var empresa = _$empresaInformationsForm.serializeFormToObject();

            if ($('#tiposGuias').val() != '') {
                listaTiposGuias = JSON.parse($('#tiposGuias').val());
            }

            var tipoGuia = {};

            //if ($('#idGridEmpresa').val() != '') {

            //    for (var i = 0; i < listaTiposLeitos.length; i++) {
            //        if (listaTiposLeitos[i].IdGrid == $('#idGridEmpresa').val()) {

            //            var empresa = $('#empresaId').select2('data');
            //            if (empresa && empresa.length > 0) {

            //                listaEmpresa[i].EmpresaDescricao = empresa[0].text;
            //            }


            //            listaEmpresa[i].EmpresaId = $('#empresaId').val();

            //            _$empresaTable.jtable('updateRecord', {
            //                record: listaEmpresa[i]
            //            , clientOnly: true
            //            });

            //        }
            //    }
            //}
            //else {
            tipoGuia.IdGrid = listaTiposGuias.length == 0 ? 1 : listaTiposGuias[listaTiposGuias.length - 1].IdGrid + 1;

            var campotipoGuia = $('#tipoGuiaId').select2('data');
            if (campotipoGuia && campotipoGuia.length > 0) {
                tipoGuia.TipoGuiaDescricao = campotipoGuia[0].text;
                }
            tipoGuia.TipoGuiaId = $('#tipoGuiaId').val();

            listaTiposGuias.push(tipoGuia);
            _$tiposGuiaTable.jtable('addRecord', {
                record: tipoGuia
                  , clientOnly: true
                });
           // }

            $('#tiposGuias').val(JSON.stringify(listaTiposGuias));
           // $('#idGridEmpresa').val('');
            $('#tipoGuiaId').val('').trigger('change');
           // $('#empresaId').focus();

        });

        function getTiposLeitos() {

           

            listaTiposGuias = JSON.parse($('#tiposGuias').val());

            for (var i = 0; i < listaTiposGuias.length; i++) {
                var item = listaTiposGuias[i];

                _$tiposGuiaTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getTiposLeitos();

        function deleteTipoGuia(tipoGuia) {


            abp.message.confirm(
                app.localize('DeleteWarning', tipoGuia.TipoGuiaDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {

                       

                        listaTiposGuias = JSON.parse($('#tiposGuias').val());

                        for (var i = 0; i < listaTiposGuias.length; i++) {
                            if (listaTiposGuias[i].IdGrid == tipoGuia.IdGrid) {
                                listaTiposGuias.splice(i, 1);
                                $('#tiposGuias').val(JSON.stringify(listaTiposGuias));

                                _$tiposGuiaTable.jtable('deleteRecord', {
                                    key: tipoGuia.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                    }
                }
            );
        }




        var _$especialidadeTable = $('#especialidadeTable');

        _$especialidadeTable.jtable({

            title: app.localize('Especialidades'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                deleteEspecialidade(data.record);
                            });

                        return $span;
                    }
                },

                Descricao: {
                    title: app.localize('Descricao'),
                    width: '70%',
                    display: function (data) {
                        return data.record.EspecialidadeDescricao;
                    }
                },
            }
        });

        var listaEspecialidades = [];

        $('#inserirEspecialidade').click(function (e) {
            e.preventDefault();

            if ($('#especialidadeId').val() == '' || $('#especialidadeId').val() == null) {
                return;
            }

            if ($('#especialidades').val() != '') {
                listaEspecialidades = JSON.parse($('#especialidades').val());
            }

            var especialidade = {};

            especialidade.IdGrid = listaEspecialidades.length == 0 ? 1 : listaEspecialidades[listaEspecialidades.length - 1].IdGrid + 1;

            var campoEspecialidade = $('#especialidadeId').select2('data');
            if (campoEspecialidade && campoEspecialidade.length > 0) {
                especialidade.EspecialidadeDescricao = campoEspecialidade[0].text;
            }
            especialidade.EspecialidadeId = $('#especialidadeId').val();

            listaEspecialidades.push(especialidade);
            _$especialidadeTable.jtable('addRecord', {
                record: especialidade
                  , clientOnly: true
            });

            $('#especialidades').val(JSON.stringify(listaEspecialidades));
            $('#especialidadeId').val('').trigger('change');

        });

        function getEspecialidades() {

           

            listaEspecialidades = JSON.parse($('#especialidades').val());

            for (var i = 0; i < listaEspecialidades.length; i++) {
                var item = listaEspecialidades[i];

                _$especialidadeTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getEspecialidades();

        function deleteEspecialidade(especialidade) {


            abp.message.confirm(
                app.localize('DeleteWarning', especialidade.EspecialidadeDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {

                       

                        listaEspecialidades = JSON.parse($('#especialidades').val());

                        for (var i = 0; i < listaEspecialidades.length; i++) {
                            if (listaEspecialidades[i].IdGrid == especialidade.IdGrid) {
                                listaEspecialidades.splice(i, 1);
                                $('#especialidades').val(JSON.stringify(listaEspecialidades));

                                _$especialidadeTable.jtable('deleteRecord', {
                                    key: especialidade.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                    }
                }
            );
        }



        var _$convenioPlanoTable = $('#convenioPlanoTable');

        _$convenioPlanoTable.jtable({

            title: app.localize('Convenio/Plano'),
            sorting: true,
            edit: false,
            create: false,
            multiSorting: true,

            fields: {
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');

                        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                            .appendTo($span)
                            .click(function (e) {
                                e.preventDefault();
                                deleteEspecialidade(data.record);
                            });

                        return $span;
                    }
                },

                Convenio: {
                    title: app.localize('Convenio'),
                    width: '45%',
                    display: function (data) {
                        return data.record.ConvenioDescricao;
                    }
                },
                Plano: {
                    title: app.localize('Plano'),
                    width: '45%',
                    display: function (data) {
                        return data.record.PlanoDescricao;
                    }
                },
            }
        });

        var listaConveniosPlanos = [];


        function getConveniosPlanos() {

            listaConveniosPlanos = JSON.parse($('#conveniosPlanos').val());

            for (var i = 0; i < listaConveniosPlanos.length; i++) {
                var item = listaConveniosPlanos[i];

                _$convenioPlanoTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getConveniosPlanos();



        $('#inserirConvenioPlano').click(function (e) {
            e.preventDefault();

            if ($('#convenioId').val() == '' || $('#convenioId').val() == null) {
                return;
            }

            if ($('#conveniosPlanos').val() != '') {
                listaConveniosPlanos = JSON.parse($('#conveniosPlanos').val());
            }

            var convenioPlano = {};

            convenioPlano.IdGrid = listaConveniosPlanos.length == 0 ? 1 : listaConveniosPlanos[listaConveniosPlanos.length - 1].IdGrid + 1;

            var campoConvenio = $('#convenioId').select2('data');
            if (campoConvenio && campoConvenio.length > 0) {
                convenioPlano.ConvenioDescricao = campoConvenio[0].text;
            }
            convenioPlano.ConvenioId = $('#convenioId').val();



            var campoPlano = $('#planoId').select2('data');
            if (campoPlano && campoPlano.length > 0) {
                convenioPlano.PlanoDescricao = campoPlano[0].text;
            }
            convenioPlano.PlanoId = $('#planoId').val();



            listaConveniosPlanos.push(convenioPlano);
            _$convenioPlanoTable.jtable('addRecord', {
                record: convenioPlano
                  , clientOnly: true
            });

            $('#conveniosPlanos').val(JSON.stringify(listaConveniosPlanos));
            $('#convenioId').val('').trigger('change');
            $('#planoId').val('').trigger('change');

        });


        $('#isInternacao').change(function (e) {
            e.preventDefault();

           

            var check = $('#isInternacao').is(':checked');

            $('#divDiaria').attr('Hidden', !check);

            if (!check) {
            $('#isDiaria').attr('checked', check);
                alteraCamposDiaria();
            }


        });



        $('#isDiaria').change(function (e) {
            e.preventDefault();
           
            alteraCamposDiaria();
            
        });

        function alteraCamposDiaria()
        {
            var check = $('#isDiaria').is(':checked');

            $('#divCabranca').attr('Hidden', !check);

            if (!check) {
                $('#isCobraPernoite').attr('checked', false);
                $('#isCobraRefeicao').attr('checked', false);
                $('#isCobraFralda').attr('checked', false);
            }
        }





        //var _$tipoLeitoTable = $('#tipoLeitoTable');

        //_$tipoLeitoTable.jtable({

        //    title: app.localize('tipoLeito'),
        //    sorting: true,
        //    edit: false,
        //    create: false,
        //    multiSorting: true,

        //    fields: {
        //        IdGrid: {
        //            key: true,
        //            list: false
        //        },
        //        actions: {
        //            title: app.localize('Actions'),
        //            width: '8%',
        //            sorting: false,
        //            display: function (data) {
        //                var $span = $('<span></span>');

        //                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
        //                    .appendTo($span)
        //                    .click(function (e) {
        //                        e.preventDefault();
        //                        deleteTipoLeito(data.record);
        //                    });

        //                return $span;
        //            }
        //        },

        //        Descricao: {
        //            title: app.localize('Descricao'),
        //            width: '70%',
        //            display: function (data) {
        //                return data.record.TipoLeitoDescricao;
        //            }
        //        },
        //    }
        //});

        //var listaTipoLeitos = [];

        //$('#inserirTipoLeito').click(function (e) {
        //    e.preventDefault();

        //    if ($('#tipoLeitoId').val() == '' || $('#tipoLeitoId').val() == null) {
        //        return;
        //    }

        //    if ($('#tiposLeitos').val() != '') {
        //        listaTipoLeitos = JSON.parse($('#tiposLeitos').val());
        //    }

        //    var tipoLeito = {};

        //    tipoLeito.IdGrid = listaTipoLeitos.length == 0 ? 1 : listaTipoLeitos[listaTipoLeitos.length - 1].IdGrid + 1;

        //    var campotipoLeito = $('#tipoLeitoId').select2('data');
        //    if (campotipoLeito && campotipoLeito.length > 0) {
        //        tipoLeito.TipoLeitoDescricao = campotipoLeito[0].text;
        //    }
        //    tipoLeito.TipoLeitoId = $('#tipoLeitoId').val();

        //    listaTipoLeitos.push(tipoLeito);
        //    _$tipoLeitoTable.jtable('addRecord', {
        //        record: tipoLeito
        //          , clientOnly: true
        //    });

        //    $('#tiposLeitos').val(JSON.stringify(listaTipoLeitos));
        //    $('#tipoLeitoId').val('').trigger('change');

        //});

        //function getTiposLeitos() {

        //   

        //    listaTipoLeitos = JSON.parse($('#tiposLeitos').val());

        //    for (var i = 0; i < listaTipoLeitos.length; i++) {
        //        var item = listaTipoLeitos[i];

        //        _$tipoLeitoTable.jtable('addRecord', {
        //            record: item
        //            , clientOnly: true
        //        });
        //    }
        //}

        //getTiposLeitos();

        //function deleteTipoLeito(tipoLeito) {

        //    abp.message.confirm(
        //        app.localize('DeleteWarning', tipoLeito.TipoLeitoDescricao),
        //        function (isConfirmed) {
        //            if (isConfirmed) {

        //               

        //                listaTipoLeitos = JSON.parse($('#tiposLeitos').val());

        //                for (var i = 0; i < listaTipoLeitos.length; i++) {
        //                    if (listaTipoLeitos[i].IdGrid == tipoLeito.IdGrid) {
        //                        listaTipoLeitos.splice(i, 1);
        //                        $('#tiposLeitos').val(JSON.stringify(listaTipoLeitos));

        //                        _$tipoLeitoTable.jtable('deleteRecord', {
        //                            key: tipoLeito.IdGrid
        //                        , clientOnly: true
        //                        });

        //                        break;
        //                    }
        //                }

        //            }
        //        }
        //    );
        //}




       // tipoLeitoTable


        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdown");
        selectSW('.selectUnidadeOrganizacional', "/api/services/app/unidadeOrganizacional/ListarDropdown");
        selectSW('.selectTipoGuia', "/api/services/app/FaturamentoGuia/ListarDropdown");
        selectSW('.selectEspecialidade', "/api/services/app/Especialidade/ListarDropdown");
        selectSW('.selectTurno', "/api/services/app/Turno/ListarDropdown");
        selectSW('.selectCentroCusto', "/api/services/app/CentroCusto/ListarDropdown");
        selectSW('.selectTipoAcomodacao', "/api/services/app/TipoAcomodacao/ListarDropdown");
        selectSW('.selectFaturamentoItem', "/api/services/app/FaturamentoItem/ListarDropdownTodos");
        selectSW('.selectConvenio', "/api/services/app/Convenio/ListarDropdown");
        selectSW('.selectPlano', "/api/services/app/Plano/ListarPorConvenioExclusivoDropdown", $('#convenioId'));
        selectSW('.selectTerceirizado', "/api/services/app/Terceirizado/ListarDropdownNomeCompleto");
        

        $('#convenioId').change(function (e) {
            e.preventDefault();
           

            selectSW('.selectPlano', "/api/services/app/Plano/ListarPorConvenioExclusivoDropdown", $('#convenioId'));

        });

        

    });

})(jQuery);