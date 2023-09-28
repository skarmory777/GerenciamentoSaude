(function () {

    $(function () {



        var _$loteValidadeTable = $('#loteValidadeTable');

       // selectSW('.selectEstoque', "/api/services/app/estoque/ResultDropdownList");
        selectSW('.selectProduto', "/api/services/app/produto/ListarProdutoDropdown");
       // selectSW('.selectUnidade', "/api/services/app/TipoMovimento/ListarDropdownDevolucao");


        $('#produtoId').change(function () {
            var valor = $('#produtoId').val();
            $("#produtoUnidadeId").empty();

            if (valor != '' && valor != null) {

                $.ajax({
                    url: "/mpa/preMovimentos/SelecionarUnidades/" + valor,
                    success: function (data) {

                        $("#produtoUnidadeId").append('<option value>Selecione um valor</option>');

                        var selected = (data.Items.length == 1) ? " selected='selected' " : "";

                        $.each(data.Items, function (index, element) {
                            $("#produtoUnidadeId").append("<option " + selected + " value='" + element.Id + "'>" + element.Descricao + "</option>");
                        });

                        $('.selectpicker').selectpicker('refresh');
                    }
                });

                debugger;

                var _produtoService = abp.services.app.produto;
                _produtoService.obter(valor).done(function (data) {

                    if (data.isValidade || data.isLote) {
                        getLotesValidade();
                    }

                });
               
            }
        });

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        $('#btnGerarEtiqueta').click(function () {

            abp.services.app.codigoBarra.gerarEtiquetas(0, $('#produtoId').val(), $("#loteValidadeId").val(), $("#produtoUnidadeId").val())
                .done(function (data) {
                    if (data.errors.length > 0) {
                        _ErrorModal.open({ erros: data.errors });
                    }
                    else {

                        abp.notify.info(app.localize('SavedSuccessfully'));

                    }
                });
            
        });


        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Etiquetas/CriarEtiqueta',
           scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Etiquetas/_CriarOuEditar.js',
           modalClass: 'CriarOuEditarEtiqueta'
        });


        _$loteValidadeTable.jtable({

            title: app.localize('Lote/Validade'),
            paging: true,
            sorting: true,
            multiSorting: true,
            //selecting: false, //Enable selecting
            //multiselect: false, //Allow multiple selecting
            //selectingCheckboxes: true, //Show checkboxes on first column

            actions: {
                listAction: {
                    method: abp.services.app.estoqueLoteValidade.listarPorProduto
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },

                actions: {
                    title: app.localize('Actions'),
                    width: '8%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditModal.open({ id: data.record.id, qtd: data.record.quantidade });
                                    //location.href = 'Saidas/CriarOuEditarModal/' + data.record.id;
                                });
                        return $span;
                    }
                }
                ,

                Lote: {
                    title: app.localize('Lote'),
                    width: '20%',
                    display: function (data) {
                        return data.record.lote;
                    }
                },

                Validade: {
                    title: app.localize('Validade'),
                    width: '20%',
                    display: function (data) {
                        return moment(data.record.validade).format('L');
                    }
                },

                Laboratorio: {
                    title: app.localize('Laboratorio'),
                    width: '20%',
                    display: function (data) {
                        return data.record.laboratorio;
                    }
                },

                Quantidade: {
                    title: app.localize('Quantidade'),
                    width: '20%',
                    display: function (data) {
                        return data.record.quantidade;
                    }
                },

                
                
            }

        });



        function getLotesValidade(reload) {
            if (reload) {
                _$loteValidadeTable.jtable('reload');
            } else {
                _$loteValidadeTable.jtable('load', {
                    produtoId: $('#produtoId').val()
                   
                });
            }
        }


    });
})();