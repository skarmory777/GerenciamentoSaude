(function ($) {
    app.modals.CriarOuEditarFornecedorModal = function () {

        var _fornecedorService = abp.services.app.fornecedor;
        var _sisPessoaService = abp.services.app.sisPessoa;
        var _cepService = abp.services.app.cep;
        var _cidadeService = abp.services.app.cidade;

        var _modalManager;

        $(document).ready(function () {
            $('#Cpf').mask('000.000.000-00', { reverse: true });
            $('#Cnpj').mask('00.000.000/0000-00', { reverse: true });
            $('#CEP').mask('00000-000');
            $("#tipo-pessoa-id").val("F");
            $('#Cnpj').prop('required', false);
            $('#razaoSocial').prop('required', false);
            $(".required-label").hide();
            $('#divPessoaJuridica').hide();
        });



        this.init = function (modalManager) {
            _modalManager = modalManager;


            $('.modal-dialog').css({ 'width': '90%', 'max-width': '800px' });



        };



        //$('#CEP').on('change', function (e) {
        //    _cepService.consultaCep($('#CEP').val())
        //             .done(function (data) {
        //                 if (data) {
        //                     
        //                     $('#logradouro').val(data.end);

        //                 }
        //             });

        //});




        $('#tipo-pessoa-id').on('change', function (e) {

            if ($('#tipo-pessoa-id').val() == "F") {
                $(".required-label").hide()
                // showing div
                $('#divCPF').show();
                $('#divPessoaFisica').show();
                // hiding div
                $('#divCNPJ').hide();
                $('#divPessoaJuridica').hide();
                ajustarRequiredPf();
            }
            if ($('#tipo-pessoa-id').val() == "J") {
                $(".required-label").hide()
                // showing div
                $('#divCNPJ').show();
                $('#divPessoaJuridica').show();
                // hiding div
                $('#divCPF').hide();
                $('#divPessoaFisica').hide();
                ajustarRequiredPj();
            }

        });

        $('.save-button').on('click', function (e) {

            var FornecedorForm = $('form[name=FornecedorForm]');
            FornecedorForm.validate();

            if (!FornecedorForm.valid()) {
                return;
            }

            var cadastroPessoaFisica = {
                Id: FornecedorForm.serializeFormToObject().Id,
                Cpf: $("#Cpf").val(),
                NomeCompleto: $("#nomeCompleto").val(),
                Rg: $("#Rg").val(),
                EmissaoRg: $("#emissaoRg").val(),
                Emissor: $("#emissor").val(),
                NacionalidadeId: $("#nacionalidadeId").val(),
                Nascimento: $("#nascimento").val(),
                NaturalidadeId: $("#naturalidadeId").val(),
                ProfissaoId: $("#profissaoId").val(),
                NomePai: $("#nomePai").val(),
                NomeMae: $("#nomeMae").val(),
                Enderecos: FornecedorForm.serializeFormToObject().Enderecos,
                FisicaJuridica: "F"
            }

            var cadastroPessoaJuridica = {
                Id: FornecedorForm.serializeFormToObject().Id,
                Cnpj: $("#Cnpj").val(),
                InscricaoMunicipal: $("#inscricaoMunicipal").val(),
                InscricaoEstadual: $("#inscricaoEstadual").val(),
                RazaoSocial: $("#razaoSocial").val(),
                NomeFantasia: $("#nomeFantasia").val(),
                Enderecos: FornecedorForm.serializeFormToObject().Enderecos,
                FisicaJuridica: "J"
            }

            cadastroPessoaFisica.Cpf = retirarMascara(cadastroPessoaFisica.Cpf);
            cadastroPessoaJuridica.Cnpj = retirarMascara(cadastroPessoaJuridica.Cnpj);

            if ($('#tipo-pessoa-id').val() == "F") {
                _fornecedorService.criarOuEditar(cadastroPessoaFisica)
                    .done(function () {


                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.CriarOuEditarFornecedorModalSaved');
                    })
            }

            if ($('#tipo-pessoa-id').val() == "J") {
                _fornecedorService.criarOuEditar(cadastroPessoaJuridica)
                    .done(function () {


                        abp.notify.info(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.CriarOuEditarFornecedorModalSaved');
                    })
            }
        });

        $('#Cpf').on('change', function (e) {
            e.preventDefault();

           

            if (retirarMascara($('#Cpf').val()) != '') {
                _fornecedorService.obterPorCPF(retirarMascara($('#Cpf').val()))
                .done(function (data) {

                    if (data) {
                        abp.notify.info('Já existe fornecedor com o CPF informado.');
                    }
                    else {
                        _sisPessoaService.obterPorCPF(retirarMascara($('#Cpf').val()))
                       .done(function (data) {
                           
                           if (data) {

                               $('#sisPessoaId').val(data.id);

                               $('#nomeCompleto').val(data.nomeCompleto);
                               $('#Rg').val(data.rg);
                               $('#emissaoRg').val(moment(data.emissaoRg).format('L'));
                               $('#emissor').val(data.emissor);


                               $('#nacionalidadeId').append($("<option/>") //add option tag in select
                                 .val(data.nacionalidade.id) //set value for option to post it
                                 .text(data.nacionalidade.descricao))
                                 .val(data.nacionalidade.id);


                               $('#naturalidadeId').append($("<option/>") //add option tag in select
                                 .val(data.naturalidade.id) //set value for option to post it
                                 .text(data.naturalidade.descricao))
                                 .val(data.naturalidade.id);


                               $('#profissaoId').append($("<option/>") //add option tag in select
                                 .val(data.profissao.id) //set value for option to post it
                                 .text(data.profissao.descricao))
                                 .val(data.profissao.id);


                               $('#nascimento').val(moment(data.nascimento).format('L'));
                               $('#nomePai').val(data.nomePai);
                               $('#nomeMae').val(data.nomeMae);

                               $('#enderecos').val(data.enderecosJson);

                               getEnderecosTable();
                           }
                       });

                    }
                })
               .always(function () {
                   //  _modalManager.setBusy(false);
               });
            }
        });


        $('#Cnpj').on('change', function (e) {
            e.preventDefault();

           

            if (retirarMascara($('#Cnpj').val()) != '') {
                _fornecedorService.obterPorCNPJ(retirarMascara($('#Cnpj').val()))
                .done(function (data) {

                    if (data) {
                        abp.notify.info('Já existe fornecedor com o CPF informado.');
                    }
                    else {
                        _sisPessoaService.obterPorCnpj(retirarMascara($('#Cnpj').val()))
                       .done(function (data) {
                          
                           if (data) {

                               $('#sisPessoaId').val(data.id);

                               $('#inscricaoMunicipal').val(data.inscricaoMunicipal);
                               $('#inscricaoEstadual').val(data.inscricaoEstadual);
                               $('#razaoSocial').val(data.razaoSocial);
                               $('#nomeFantasia').val(data.nomeFantasia);

                               $('#enderecos').val(data.enderecosJson);

                               getEnderecosTable();

                           }
                       });

                    }
                })
               .always(function () {
                   //  _modalManager.setBusy(false);
               });
            }
        });

        $('#cidadeId').on('change', function (e) {

            
            if ($('#cidadeId').val() != null && $('#cidadeId').val() != '') {

                _cidadeService.obter($('#cidadeId').val())
                         .done(function (data) {
                             if (data) {
                                 $('#estadoId').append($("<option>") //add option tag in select
                                                .val(data.estadoId) //set value for option to post it
                                                .text(data.estado.nome)) //set a text for show in select
                                               .val(data.estadoId) //select option of select2
                                               .trigger("change");
                             }
                         });
            }
        });
        

        $('.minhadata').daterangepicker({
            "singleDatePicker": true,
            "showDropdowns": true,
            autoUpdateInput: false,
            maxDate: new Date(),
            changeYear: true,
            yearRange: 'c-10:c+10',
            showOn: "both",
            "locale": {
                "format": moment.locale().toUpperCase() === 'PT-BR' ? "DD/MM/YYYY" : moment.locale().toUpperCase() === 'US' ? "MM/DD/YYYY" : "YYYY-MM-DD",
                "separator": " - ",
                "applyLabel": "Apply",
                "cancelLabel": "Cancel",
                "fromLabel": "From",
                "toLabel": "To",
                "customRangeLabel": "Custom",
                "daysOfWeek": [
                    app.localize('Dom'),
                    app.localize('Seg'),
                    app.localize('Ter'),
                    app.localize('Qua'),
                    app.localize('Qui'),
                    app.localize('Sex'),
                    app.localize('Sab')
                ],
                "monthNames": [
                    app.localize("Jan"),
                    app.localize("Fev"),
                    app.localize("Mar"),
                    app.localize("Abr"),
                    app.localize("Mai"),
                    app.localize("Jun"),
                    app.localize("Jul"),
                    app.localize("Ago"),
                    app.localize("Set"),
                    app.localize("Out"),
                    app.localize("Nov"),
                    app.localize("Dez"),
                ],
                "firstDay": 0
            }
        },
        function (selDate) {
            $('.minhadata').val(selDate.format('l')).addClass('form-control edited');
            // obterIdade(selDate);
        });

        selectSW('.selectNacionalidade', "/api/services/app/nacionalidade/ListarDropdown");
        selectSW('.selectNaturalidade', "/api/services/app/naturalidade/ListarDropdown");
        selectSW('.selectProfissao', "/api/services/app/profissao/ListarDropdown");
        selectSW('.selectTipoLogradouro', "/api/services/app/tipoLogradouro/ListarDropdown");
        selectSW('.selectCidade', "/api/services/app/cidade/ListarDropdown");
        selectSW('.selectEstado', "/api/services/app/estado/ListarDropdown");
        selectSW('.selectPais', "/api/services/app/pais/ListarDropdown");

        function retirarMascara(valor) {

            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace('-', '');
            valor = valor.replace('/', '');

            return valor;
        }

        var _$EnderecosTable = $('#EnderecosTable');
        var lista = [];

        $('#btnInserir').click(function (e) {
            e.preventDefault();

            if ($("#logradouro").val() == '') {
                abp.message.warn("É necessário o preenchimento dos campos");
                return;
            }

            var _$EnderecoInformationsForm = $('form[name=EnderecoInformationsForm]');
            var endereco = _$EnderecoInformationsForm.serializeFormToObject();

            if ($('#enderecos').val() != '') {
                lista = JSON.parse($('#enderecos').val());
            }

            if ($('#idGrid').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGrid').val()) {

                        lista[i].Cep = $('#cep').val();
                        lista[i].TipoLogradouroId = $('#tipoLogradouroId').val();

                        var tipoLogradouro = $('#tipoLogradouroId').select2('data');
                        if (tipoLogradouro && tipoLogradouro.length > 0) {

                            lista[i].TipoLogradouroDescricao = tipoLogradouro[0].text;
                        }

                        lista[i].Logradouro = $('#logradouro').val();
                        lista[i].Numero = $('#numero').val();
                        lista[i].Complemento = $('#complemento').val();
                        lista[i].Bairro = $('#bairroId').val();

                        lista[i].CidadeId = $('#cidadeId').val();
                        var cidade = $('#cidadeId').select2('data');
                        if (cidade && cidade.length > 0) {

                            lista[i].CidadeDescricao = cidade[0].text;
                        }

                        lista[i].EstadoId = $('#estadoId').val();
                        var estado = $('#eatadoId').select2('data');
                        if (estado && estado.length > 0) {

                            lista[i].EstadoDescricao = estado[0].text;
                        }
                        
                        
                        
                        lista[i].PaisId = $('#paisId').val();


                        var pais = $('#paisId').select2('data');
                        if (pais && pais.length > 0) {

                            lista[i].PaisDescricao = pais[0].text;
                        }



                        _$EnderecosTable.jtable('updateRecord', {
                            record: lista[i]
                        , clientOnly: true
                        });

                    }
                }
            }
            else {
                endereco.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                endereco.TipoLogradouroId = $('#tipoLogradouroId').val();

                var tipoLogradouro = $('#tipoLogradouroId').select2('data');
                if (tipoLogradouro && tipoLogradouro.length > 0) {

                    endereco.TipoLogradouroDescricao = tipoLogradouro[0].text;
                }

                endereco.Logradouro = $('#logradouro').val();

                endereco.Numero = $('#numero').val();
                endereco.Complemento = $('#complemento').val();
                endereco.Bairro = $('#bairro').val();

                var cidade = $('#cidadeId').select2('data');
                if (cidade && cidade.length > 0) {

                    endereco.CidadeDescricao = cidade[0].text;
                }

                var estado = $('#estadoId').select2('data');
                if (estado && estado.length > 0) {

                    endereco.EstadoDescricao = estado[0].text;
                }

                var pais = $('#paisId').select2('data');
                if (pais && pais.length > 0) {

                    endereco.PaisDescricao = pais[0].text;
                }

                lista.push(endereco);

                _$EnderecosTable.jtable('addRecord', {
                    record: endereco
                  , clientOnly: true
                });
            }

            $('#enderecos').val(JSON.stringify(lista));

            $('#tipoLogradouroId').val('').trigger("change");
            $('#logradouro').val('');
            $('#numero').val('');
            $('#complemento').val('');
            $('#bairroId').val('');
            $('#cidadeId').val('').trigger("change");
            $('#estadoId').val(null).trigger("change");
            $('#paisId').val(null).trigger("change");
            
            $('#idGrid').val('');


            $('#tipoLogradouroId').focus();

        });


        _$EnderecosTable.jtable
   ({
       title: app.localize('Enderecos'),
       // paging: true,
       sorting: true,
       edit: false,
       create: false,
       multiSorting: true,


       fields:
       {
           IdGrid: {
               key: true,
               list: false
           },
           actions: {
               title: app.localize('Actions'),
               width: '7%',
               sorting: false,
               display: function (data) {
                   var $span = $('<span></span>');

                   $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                       .appendTo($span)
                       .click(function (e) {
                           e.preventDefault();
                           //_createOrEditPreMovimentoItemModal.open({ item: JSON.stringify(data.record) });
                           editEndereco(data.record);
                       });

                   $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                     .appendTo($span)
                     .click(function (e) {
                         e.preventDefault();
                         deleteEndereco(data.record);
                     });

                   return $span;
               }
           },



           TipoLogradouroDescricao: {
               title: app.localize('TipoLogradouro'),
               width: '30%',
               display: function (data) {
                   if (data.record.TipoLogradouroDescricao) {
                       return data.record.TipoLogradouroDescricao;
                   }
               }
           },

           Logradouro: {
               title: app.localize('Logradouro'),
               width: '30%',
               display: function (data) {
                   if (data.record.Logradouro) {
                       return data.record.Logradouro;
                   }
               }
           }


       }
   });


        function getEnderecosTable(reload) {

            lista = JSON.parse($('#enderecos').val());

            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];
                _$EnderecosTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }

        getEnderecosTable();

        function editEndereco(endereco) {

            
            $('#tipoLogradouroId')
                .append($("<option>") //add option tag in select
              .val(endereco.TipoLogradouroId) //set value for option to post it
              .text(endereco.TipoLogradouroDescricao)
            ) //set a text for show in select
      .val(endereco.TipoLogradouroId) //select option of select2
            .trigger("change");



            $('#logradouro').val(endereco.Logradouro);


            $('#numero').val(endereco.Numero);
            $('#complemento').val(endereco.Complemento);
            $('#bairroId').val(endereco.Bairro);

            if (endereco.CidadeId != null && endereco.CidadeId != '') {

                $('#cidadeId').val(endereco.CidadeId).append($("<option>") //add option tag in select
                  .val(endereco.CidadeId) //set value for option to post it
                  .text(endereco.CidadeDescricao)
                ) //set a text for show in select
          .val(endereco.CidadeId)
                    .trigger("change");
            }

            if (endereco.EstadoId != null && endereco.EstadoId != '') {

                $('#estadoId').val(endereco.EstadoId).append($("<option>") //add option tag in select
                  .val(endereco.EstadoId) //set value for option to post it
                  .text(endereco.EstadoDescricao)
                ) //set a text for show in select
                .val(endereco.EstadoId)
                .trigger("change");
            }

            

            if (endereco.PaisId != null && endereco.PaisId != '') {

                $('#paisId').val(endereco.PaisId).append($("<option>") //add option tag in select
                  .val(endereco.PaisId) //set value for option to post it
                  .text(endereco.PaisDescricao)
                ) //set a text for show in select
                .val(endereco.PaisId)
                .trigger("change");
            }



            $('#idGrid').val(endereco.IdGrid);

        }

        function deleteEndereco(endereco) {
            abp.message.confirm(
                app.localize('DeleteWarning', endereco.Logradouro),
                function (isConfirmed) {
                    if (isConfirmed) {
                        


                        lista = JSON.parse($('#enderecos').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == endereco.IdGrid) {
                                lista.splice(i, 1);
                                $('#enderecos').val(JSON.stringify(lista));

                                _$EnderecosTable.jtable('deleteRecord', {
                                    key: endereco.IdGrid
                                , clientOnly: true
                                });

                                break;
                            }
                        }

                        // getAutorizacaoItemTable();
                    }
                }
            );
        }
        aplicarDateRange();
        aplicarDateSingle();
        CamposRequeridos();

    };

    function ajustarRequiredPj() {
        // adding required
        $('#Cnpj').prop('required', true);
        $('#razaoSocial').prop('required', true);
        // removing required
        $('#Cpf').prop('required', false);
        $('#nomeCompleto').prop('required', false);
        $('#Rg').prop('required', false);
        $('#Rg').prop('required', false);
        $('#nacionalidadeId').prop('required', false);
        $('#nascimento').prop('required', false);
        $('#naturalidadeId').prop('required', false);
        $('#profissaoId').prop('required', false);
    }
    
    function ajustarRequiredPf(){
            // adding required
            $('#Cpf').prop('required', true)
            $('#nomeCompleto').prop('required', true)
            $('#Rg').prop('required', true)
            $('#Rg').prop('required', true)
            $('#nacionalidadeId').prop('required', true)
            $('#nascimento').prop('required', true)
            $('#naturalidadeId').prop('required', true)
            $('#profissaoId').prop('required', true);
            // removing required 
            $('#Cnpj').prop('required', false);
            $('#razaoSocial').prop('required', false)
        }
})(jQuery);