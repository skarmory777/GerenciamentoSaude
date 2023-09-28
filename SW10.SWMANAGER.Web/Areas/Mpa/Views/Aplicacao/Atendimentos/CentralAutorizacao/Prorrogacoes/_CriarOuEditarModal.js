
(function ($) {
    $(function () {

        $(document).ready(function () {

            CamposRequeridos();
        });

        $('.modal-dialog').css('width', '1800px');

        $('#dataAutorizacaoId').on('load', function () {
            var d = new Date();
            var n = d.getDate();
            $('#DataAutorizacao').val(moment().format("L LT"));
        });

        //var _permissions = {
        //    create: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Create'),
        //    edit: abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Edit'),
        //    'delete': abp.auth.hasPermission('Pages.Tenant.Suprimentos.Estoque.PreMovimento.Delete')
        //};

        var _autorizacaoProcedimento = abp.services.app.autorizacaoProcedimento;
        var _comentarioAutorizacaoProcedimentoAppService = abp.services.app.comentarioAutorizacaoProcedimento;

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });


        $('#salvar-autorizacaoProcedimento').click(function (e) {
            e.preventDefault()


            var _$autorizacaoInformationsForm = $('form[name=autorizacaoInformationsForm');

            //_$autorizacaoInformationsForm.validate();

            //if (!_$autorizacaoInformationsForm.valid()) {
            //    return;
            //}

            debugger;



            var autorizacaoProcedimento = _$autorizacaoInformationsForm.serializeFormToObject();

            autorizacaoProcedimento.AtendimentoId = $('#atendimentoId').val();


            if (listaComentarios.length > 0 && ($('#id').val() == '' || $('#id').val() == 0)) {
                autorizacaoProcedimento.comentarios = listaComentarios;
            }


            _autorizacaoProcedimento.criarOuEditarProrrogacaoInternacao(autorizacaoProcedimento)
              .done(function (data) {

                  if (data.errors.length > 0) {
                      _ErrorModal.open({ erros: data.errors });
                  }
                  else {
                      abp.notify.info(app.localize('SavedSuccessfully'));

                      //for (var i = 0; i < listaComentarios.length; i++) {
                      //    listaComentarios[i].autorizacaoProcedimentoId = data.returnObject.id;

                      //    _comentarioAutorizacaoProcedimentoAppService.criar(listaComentarios[i])
                      //        .done(function (data) {
                      //        });
                      //}


                      location.href = '/mpa/Prorrogacoes';
                  }
              })
                 .always(function () {
                 });





        });

        $('input[name="DataAutorizacao"]').daterangepicker({
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



               $('input[name="DataAutorizacao"]').val(selDate.format('L')).addClass('form-control edited');
               // obterIdade(selDate);
           });

        //abp.event.on('app.CriarOuEditarPreMovimentoItemModalSaved', function () {
        //    getEstoquePreMovimentoItemTable();
        //});

        $('.close').on('click', function () {
            location.href = '/mpa/Prorrogacoes';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/Prorrogacoes';
        });

        //var _imprimirEntrada = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/RelatorioEntrada'

        //});

        //$('#btnImprimir').on('click', function (e)
        //{
        //    _imprimirEntrada.open({ preMovimentoId: $('#id').val() });
        //});


        selectSW('.selectSolicitante', "/api/services/app/medico/ListarDropdown");
        selectSWMultiplosFiltros('.selectAtendimentoInternacao', "/api/services/app/Atendimento/ListarAtendimentosAmbulatorioInternacao", [{ valor: false }, { valor: true }]);

        selectSW('.selectConvenio', "/api/services/app/convenio/ListarDropdown");
        selectSW('.selectEspecialidade', '/api/services/app/medicoEspecialidade/ListarDropdownPorMedicoTodas', $('#solicitanteId'));

        // selectSWMultiplosFiltros('.selectFaturamentoItem', "/api/services/app/FaturamentoItemAutorizacao/ListarItemFaturamentoAutorizacaoPorConvenioDropdown", ['convenioId']);

        $('#solicitanteId').on('select2:select', function () {
            selectSW('.selectEspecialidade', '/api/services/app/medicoEspecialidade/ListarDropdownPorMedicoTodas', $('#solicitanteId'));
        });


        $('#convenioId').on('select2:select', function () {

            $('#formaAutorizacao').val('');
            $('#dadosContatos').val('');


            if ($('#convenioId').val() != null && $('#convenioId').val() != '') {
                var _convenioService = abp.services.app.convenio;

                _convenioService.obter($('#convenioId').val())
                .done(function (data) {
                    if (data != null && data.formaAutorizacao != null) {
                        $('#formaAutorizacao').val(data.formaAutorizacao.descricao);
                        $('#dadosContatos').val(data.dadosContato);
                    }
                });
            }

        });



        var _$AutorizacaoItemTable = $('#AutorizacaoItemTable');


        function retornarLista(postData, jtParams) {


            if ($('#itens').val() != '') {

                var js = $('#itens').val();
                var res = '{ \"Result\": \"OK\",\"Records\":\' ' + js + '}';       //_autorizacaoProcedimento.listarItensJson(js);
                // var res = _autorizacaoProcedimento.listarItensJson(js);
                // return  function (postData, jtParams) { return res;}


                return function (postData, jtParams) {
                    return {
                        "Resultado": "OK",
                        "Registros": [
                        { "StudentId": 39, "Nome": "Agatha Garcia", "EmailAddress": " agatha.garcia@jtable.org ", "Senha": "123", "Género": "F", "CidadeId": 55, "BirthDate": "/ Data (-1125111600000) /", "Educação": 2, "Sobre": "", "IsActive": true, "RecordDate": "/ Date (1369083600000) /" },
                        { "StudentId": 61, "Nome": "Agatha Lafore", "EmailAddress": " agatha.lafore@jtable.org ", "Senha": "123", "Gênero": "F", "CidadeId": 1, "BirthDate": "/ Data (1017694800000) /", "Sobre": "", "Educação": 3, "IsActive": true, "RecordDate": "/ Date (1393192800000) /" }],
                        "TotalRecordCount": 2
                    };
                };


            }
            else {
                var res = _autorizacaoProcedimento.listarItens({ filtro: $('#id').val() });
                return res;
            }
        }


        _$AutorizacaoItemTable.jtable
       ({
           title: app.localize('Itens'),
           // paging: true,
           sorting: true,
           edit: false,
           create: false,
           multiSorting: true,


           rowInserted: function (event, data) {
               if (data) {
                   if (data.record.ItemSelecionado) {
                       data.row.css("background", "#F5ECCE");
                   }
               }
           },



           //actions:
           //{
           //    listAction:  
           //    {
           //        method: retornarLista
           //    },
           //},
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
                               editAutorizacaoItem(data.record);
                           });

                       $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                         .appendTo($span)
                         .click(function (e) {
                             e.preventDefault();
                             deleteAutorizacaoItem(data.record);
                         });

                       return $span;
                   }
               },

               StatusDescricao: {
                   title: app.localize('Status'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.StatusDescricao) {
                           return data.record.StatusDescricao;
                       }
                   }
               },


               quantidadeSolicitada: {
                   title: app.localize('QuantidadeSolicitada'),
                   width: '12%',
                   display: function (data) {
                       if (data.record.QuantidadeSolicitada) {
                           return posicionarDireita(data.record.QuantidadeSolicitada);
                       }
                   }
               },

               quantidade: {
                   title: app.localize('QuantidadeAutorizada'),
                   width: '12%',
                   display: function (data) {
                       if (data.record.QuantidadeAutorizada) {
                           return posicionarDireita(data.record.QuantidadeAutorizada);
                       }
                   }
               },
               Autorizacao: {
                   title: app.localize('Autorizacao'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.Senha) {
                           return data.record.Senha;
                       }
                   }
               },
               DataAutorizacao: {
                   title: app.localize('DataAutorizacao'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.DataAutorizacao) {
                           return data.record.DataAutorizacao;
                       }
                   }
               },

               AutorizadoPor: {
                   title: app.localize('AutorizadoPor'),
                   width: '10%',
                   display: function (data) {
                       if (data.record.AutorizadoPor) {
                           return data.record.AutorizadoPor;
                       }
                   }
               },
           }
       });




        function getAutorizacaoItemTable(reload) {


            lista = JSON.parse($('#itens').val());


            //_$AutorizacaoItemTable.each(function () {
            //    var record = $(this).data('record');

            // _$AutorizacaoItemTable.jtable('deleteRecord', { key: record.Id, clientOnly: true });
            // });




            var allRows = _$AutorizacaoItemTable.jtable('selectedRows');

            if (allRows.length > 0) {
                _$AutorizacaoItemTable.jtable('deleteRows', { rows: allRows, clientOnly: true });
            }


            for (var i = 0; i < lista.length; i++) {
                var item = lista[i];

                item.DataAutorizacao = moment(item.DataAutorizacao).format('L');

                _$AutorizacaoItemTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                });
            }
        }


        function editAutorizacaoItem(autorizacaoItem) {

            $('#quantidadeAutorizadaId').val(autorizacaoItem.QuantidadeAutorizada);
            $('#autorizacaoId').val(autorizacaoItem.Senha);

            debugger;

            if (autorizacaoItem.DataAutorizacao != '' && autorizacaoItem.DataAutorizacao != null) {

                $('#dataAutorizacaoId').val(autorizacaoItem.DataAutorizacao);
            }
            else {
                $('#dataAutorizacaoId').val();
            }

            $('#quantidadeSolicitadaId').val(autorizacaoItem.QuantidadeSolicitada);
            $('#observacaoId').val(autorizacaoItem.Observacao);
            $('#StatusId').val(autorizacaoItem.StatusId).trigger("change");
            $('#autorizadoPorId').val(autorizacaoItem.AutorizadoPor);
            $('#numeroGuiaId').val(autorizacaoItem.NumeroGuia);

            $('#idGrid').val(autorizacaoItem.IdGrid);


            // $('#inserir > i').removeClass('fa');
            $('#salvar-Autorizacao-Item > i').removeClass('fa-plus');
            // $('#inserir > i').addClass('glyphicon');
            $('#salvar-Autorizacao-Item > i').addClass('fa-check');



        }

        function deleteAutorizacaoItem(autorizacaoItem) {
            abp.message.confirm(
                app.localize('DeleteWarning', autorizacaoItem.FaturamentoItem),
                function (isConfirmed) {
                    if (isConfirmed) {



                        lista = JSON.parse($('#itens').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == autorizacaoItem.IdGrid) {
                                lista.splice(i, 1);
                                $('#itens').val(JSON.stringify(lista));

                                _$AutorizacaoItemTable.jtable('deleteRecord', {
                                    key: autorizacaoItem.IdGrid
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

        var lista = [];

        $('#salvar-Autorizacao-Item').click(function (e) {
            e.preventDefault();

            debugger;

            var _$autorizacaoItemInformationsForm = $('form[name=AutorizacaoItemInformationsForm]');
            var autorizacaoItem = _$autorizacaoItemInformationsForm.serializeFormToObject();

            if ($('#itens').val() != '') {
                lista = JSON.parse($('#itens').val());
            }

            if ($('#idGrid').val() != '') {

                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == $('#idGrid').val()) {

                        debugger;
                        lista[i].Senha = $('#autorizacaoId').val();
                        lista[i].DataAutorizacao = $('#dataAutorizacaoId').val();
                        lista[i].QuantidadeAutorizada = $('#quantidadeAutorizadaId').val();
                        lista[i].QuantidadeSolicitada = $('#quantidadeSolicitadaId').val();
                        lista[i].Observacao = $('#observacaoId').val();
                        lista[i].StatusId = $('#StatusId').val();
                        lista[i].StatusDescricao = $("#StatusId option:selected").text()
                        lista[i].AutorizadoPor = $('#autorizadoPorId').val();

                        _$AutorizacaoItemTable.jtable('updateRecord', {
                            record: lista[i]
                        , clientOnly: true
                        });

                    }
                }
            }
            else {
                autorizacaoItem.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                //autorizacaoItem.FaturamentoItemId = $('#faturamentoItemId').val();
                //autorizacaoItem.FaturamentoItemDescricao = $('#faturamentoItemId').text(),
                autorizacaoItem.Senha = $('#autorizacaoId').val();
                autorizacaoItem.DataAutorizacao = $('#dataAutorizacaoId').val();
                autorizacaoItem.QuantidadeAutorizada = $('#quantidadeAutorizadaId').val();
                autorizacaoItem.QuantidadeSolicitada = $('#quantidadeSolicitadaId').val();
                autorizacaoItem.Observacao = $('#observacaoId').val();
                autorizacaoItem.StatusId = $('#StatusId').val();
                autorizacaoItem.StatusDescricao = $("#StatusId option:selected").text();
                autorizacaoItem.AutorizadoPor = $('#autorizadoPorId').val();

                lista.push(autorizacaoItem);


                _$AutorizacaoItemTable.jtable('addRecord', {
                    record: autorizacaoItem
                  , clientOnly: true
                });

            }

            $('#itens').val(JSON.stringify(lista));
            // $('#faturamentoItemId').val('').trigger("change");
            $('#idGrid').val('');
            $('#autorizacaoId').val(null).trigger("change");
            $('#quantidadeAutorizadaId').val('');
            $('#dataAutorizacaoId').val('');
            $('#quantidadeSolicitadaId').val('');
            $('#isOrteseId').attr("checked", false);
            $('#observacaoId').val('');
            $('#StatusId').val(null).trigger("change");
            $('#autorizadoPorId').val('');

            $('#faturamentoItemId').focus();

            $('#salvar-Autorizacao-Item > i').removeClass('fa-check');
            $('#salvar-Autorizacao-Item > i').addClass('fa-plus');

        });

        getAutorizacaoItemTable();

        $('#comentario-conteudo').summernote({
            height: 100,
            minHeight: 30,
            toolbar: [
            // [groupName, [list of button]]
            ['style', ['bold', 'underline', 'clear']],
            ['mais', ['picture', 'link']]
            ]
        });

        $('#btn-comentar').on('click', function (e) {
            var tarefaSelecionadaId = $('#id').val();
            //debugger
            if (tarefaSelecionadaId) {
                comentar();
            } else {
                abp.notify.warn(app.localize('TarefaAindaNaoFoiRegistrada'));
            }
        });

        var listaComentarios = [];

        function comentar() {


            var novoComentario = {
                AutorizacaoProcedimentoId: $('#id').val(),
                Conteudo: $('#comentario-conteudo').summernote('code'),
                DataRegistro: new Date()
            };


            if ($('#id').val() != '' && $('#id').val() != 0) {
                _comentarioAutorizacaoProcedimentoAppService.criar(novoComentario)
                       .done(function (data) {
                           $('#comentario-conteudo').summernote('code', '');
                           abp.notify.info(app.localize('SavedSuccessfully'));


                           // carregarComentarios($('#id').val());
                       });
            }
            else {
                $('#comentario-conteudo').summernote('code', '');
                listaComentarios.push(novoComentario);
            }

            var containerComentarios = $('#container-comentarios');

            containerComentarios.prepend('<div class="col-sm-12" style="border:1px solid #c2cad8; padding:10px;"><div class="row"><div class="col-sm-1"><span>' + '' + '</span></div><div class="col-sm-10"><div style="display:inline-block;">' + novoComentario.Conteudo + '</div></div><div class="col-sm-1"><span>' + moment(novoComentario.DataRegistro).format('L') + '</span></div></div></div>');


        }

        function carregarComentarios(autorizacaoId) {

            var containerComentarios1 = $('#container-comentarios');
            containerComentarios1.html('');

            function apenderComentarioLocal(item, index) {
                var usuario = item.nomeUsuario;
                var dataRegistro = moment(item.dataRegistro).format('L');
                var conteudo = item.conteudo;
                var containerComentarios = $('#container-comentarios');

                containerComentarios.prepend('<div class="col-sm-12" style="border:1px solid #c2cad8; padding:10px;"><div class="row"><div class="col-sm-1"><span>' + usuario + '</span></div><div class="col-sm-10"><div style="display:inline-block;">' + conteudo + '</div></div><div class="col-sm-1"><span>' + dataRegistro + '</span></div></div></div>');
            }

            _comentarioAutorizacaoProcedimentoAppService.listarPorAutorizacao(autorizacaoId)
                         .done(function (data) {
                             data.items.forEach(apenderComentarioLocal);
                         });
        }

        function ExibirComentario() {
            if ($('#id').val() != '') {
                carregarComentarios($('#id').val());
            }
        }

        ExibirComentario();

        $('.limpar').on('click', function () {

            limpar();

        });

        function limpar() {

         
            $('#idGrid').val('');
            $('#autorizacaoId').val(null).trigger("change");
            $('#quantidadeAutorizadaId').val('');
            $('#dataAutorizacaoId').val('');
            $('#quantidadeSolicitadaId').val('');
            $('#isOrteseId').attr("checked", false);
            $('#observacaoId').val('');
            $('#StatusId').val(null).trigger("change");
            $('#autorizadoPorId').val('');


            $('#salvar-Autorizacao-Item > i').removeClass('fa-check');
            $('#salvar-Autorizacao-Item > i').addClass('fa-plus');
        }


    });

})(jQuery);