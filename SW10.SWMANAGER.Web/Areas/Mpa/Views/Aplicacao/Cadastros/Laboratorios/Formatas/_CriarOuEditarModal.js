(function ($) {
    app.modals.CriarOuEditarFormataModal = function () {
        const _$FormataItemTable = $('#formataItemTable');
        const _FormataItensService = abp.services.app.formataItem;
        //Pages_Tenant_Laboratorio_Cadastros_FormataItem_Create
        const _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.FormataItem.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.FormataItem.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Laboratorio.Cadastros.FormataItem.Delete')
        };

        const _formatasService = abp.services.app.formata;
        let _modalManager;
        let _$FormatasInformationForm = null;
        let _$FormataItemsInformationForm = null;

        let _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Formatas/CriarOuEditarModalItem',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/_CriarOuEditarModalItem.js',
            modalClass: 'CriarOuEditarFormataItemModal'
        });
        let globalMentions = [];
        var summerNoteOptions = {
            callbacks: {
                onInit: () => {
                    $(".text-editor").summernote("code", he.decode($("[name='valorFormatacao']").val())); 
                }
            },
            toolbar: [
                ['insert', ['bricks']],
                ['paperSize', ['paperSize']],
                ['style', ['bold', 'italic', 'underline']],
                ['fontsize', ['fontsize']],
                ['fontname', ['fontname']],
                ['font', ['font', 'strikethrough', 'superscript', 'subscript']],
                ['color', ['color']],
                ['para', ['ul', 'ol', 'paragraph']],
                ['height', ['height']],
                ['misc', ['codeview', 'fullscreen']],
                ['table', ['table']]
            ],
            bricks: {
                templates: {
                    //"Texto Exame Realizado": "/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/templates/table_dynamic_content.html",
                    oneColumns: {
                        name: "1 coluna",
                        url: "/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/templates/grid-component.html",
                        properties: {
                            isModal: true,
                            columns: 1,
                            variables: getVariables
                        }
                    },
                    twoColumns: {
                        name: "2 colunas",
                        url: "/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/templates/grid-component.html",
                        properties: {
                            isModal: true,
                            columns: 2,
                            variables: getVariables
                        }
                    },
                    threeColumns: {
                        name: "3 colunas",
                        url: "/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/templates/grid-component.html",
                        properties: {
                            isModal: true,
                            columns: 3,
                            variables: getVariables
                        }
                    },
                    fourColumns: {
                        name: "4 colunas",
                        url: "/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/templates/grid-component.html",
                        properties: {
                            isModal: true,
                            columns: 4,
                            variables: getVariables
                        }
                    },
                    fiveColumns: {
                        name: "5 colunas",
                        url: "/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/templates/grid-component.html",
                        properties: {
                            isModal: true,
                            columns: 5,
                            variables: getVariables
                        }
                    },
                    customColumns: {
                        name: "Colunas Customizadas",
                        url: "/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Formatas/templates/grid-component.html",
                        properties: {
                            isModal: true,
                            columns: Infinity,
                            variables: getVariables
                        }
                    }
                }
            },
            lang: 'pt-br',
            height: 250,
            //width: 'au',
            //padding: 15,
            disableResizeEditor: true
        };
        this.init = function (modalManager) {
            _modalManager = modalManager;
            _$FormataInformationForm = _modalManager.getModal().find('form[name=FormataInformationsForm]');
            _$FormataInformationForm.validate();

            $('.modal').addClass("fullscreen");
            //$('.modal-dialog:last').css('width', '1250px');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            
            getFormataItem();
            _$FormataItemInformationForm = _modalManager.getModal().find('form[name=FormataItemInformationsForm]');
            _$FormataItemInformationForm.validate();

            $('.select2').css('width', '100%');

            
        };

        this.save = function () {
            if (!_$FormataInformationForm.valid()) {
                return;
            }

            var Formata = _$FormataInformationForm.serializeFormToObject();
            Formata.Formatacao = $('#formatacao').summernote('code');
            _modalManager.setBusy(true);

            _formatasService.criarOuEditar(Formata)
                .done(function () {
                    abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.CriarOuEditarFormataModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        function updateSummernote(dataMentions) {

            if (dataMentions !== null) {
                globalMentions = dataMentions.map((item) => { return { cod: item.ItemResultado.Codigo.trim(), desc: item.ItemResultado.Descricao.trim() } });
                dataMentions = globalMentions.map((item) => 'Codigo: ' + item.cod + ' - Descrição: ' + item.desc);
            }

            var currentSummerNoteOptions = {};
            $.extend(currentSummerNoteOptions,
                summerNoteOptions,
                {
                    hint: {
                        mentions: dataMentions,
                        match: /\B\[(\w*)$/,
                        search: function (keyword, callback) {
                            callback($.grep(this.mentions, function (item) {
                                return item.length && item.indexOf(keyword) === 0;
                            }));
                        },
                        content: function (item) {
                            var mentionItem = _.find(globalMentions, (findItem) => 'Codigo: ' + findItem.cod + ' - Descrição: ' + findItem.desc === item);
                            if (mentionItem) {
                                return '[' + mentionItem.cod + ']';
                            }
                        }
                    }
                });
            if ($('.text-editor').data("summernote")) {
                $("[name='valorFormatacao']").val($('.text-editor').summernote('code'));
            }
            $('.text-editor').summernote('destroy');
            $('.text-editor').summernote(currentSummerNoteOptions);

            //var valorFormatacao = $("[name='valorFormatacao']").val();
            //$(".text-editor").summernote("code", valorFormatacao);

        }

        function getVariables() {
            return globalMentions;
        }

        _$FormataItemTable.jtable({

            title: app.localize('FormataItem'),
            edit: false,
            create: false,
            //paging: true,
            sorting: false,
            //multiSorting: true,

            //actions: {
            //    listAction: {
            //        method: retornarLista //_FormataItensService.listar
            //    }
            //},

            fields: {
                Id: {
                    key: true,
                    list: false
                },
                IdGrid: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '6%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span style="width: 100%;text-align: center;display: inline-block;"></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    //_createOrEditModal.open({ id: data.record.id, formataId: data.record.formataId });
                                    editarRegistro(data.record.Id, data.record.IdGrid, data.record);
                                });
                        }

                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteItem(data.record);
                                });
                        }

                        return $span;
                    }
                },
                Codigo: {
                    title: app.localize('Codigo'),
                    width: '15%',

                    display: function (data) {
                        if (data.record.ItemResultado) {
                            return data.record.ItemResultado.Codigo;
                        }
                    }

                },
                Descricao: {
                    title: app.localize('Descricao'),
                    width: '15%',

                    display: function (data) {
                        if (data.record.ItemResultado) {
                            return data.record.ItemResultado.Descricao;
                        }
                    }
                },
                Ordem: {
                    title: app.localize('Ordem'),
                    width: '10%',

                    display: function (data) {
                        if (data.record.Ordem) {
                            return data.record.Ordem;
                        }
                    }
                }
            }
        });

        //function retornarLista() {
        //    var list = JSON.parse($('#formata-itens').val());
        //    var res = _FormataItensService.listarJson(list)
        //    return res;
        //}

        $('#CreateNewFormataItemButton').click(function (e) {
            e.preventDefault();
            //_createOrEditModal.open({ formataId: $('#id').val() });
            novoRegistro();
        });

        function getFormataItem() {
            if (_$FormataItemTable.data("hikJtable")) {
                _$FormataItemTable.data("hikJtable")._removeAllRows();
            }
            setTimeout(function () {
                var lista = JSON.parse($('#formata-itens').val());
                for (var i = 0; i < lista.length; i++) {
                    var item = lista[i];
                    _$FormataItemTable.jtable('addRecord', { record: item, clientOnly: true });
                }

                updateSummernote(lista);
            }, 0);
        }


        function deleteItem(item) {
            abp.message.confirm(
                app.localize('DeleteWarning', item.ItemResultado.Descricao),
                function (isConfirmed) {
                    if (isConfirmed) {

                        lista = JSON.parse($('#formata-itens').val());

                        for (var i = 0; i < lista.length; i++) {
                            if (lista[i].IdGrid == item.IdGrid) {
                                lista.splice(i, 1);
                                $('#formata-itens').val(JSON.stringify(lista));

                                _$FormataItemTable.jtable('deleteRecord', {
                                    key: item.IdGrid,
                                    clientOnly: true
                                });
                                //if (item.Id > 0) {
                                //    _FormataItensService.excluir(item)
                                //        .done(function () {
                                //            //getFormataItem();
                                //            abp.notify.success(app.localize('SuccessfullyDeleted'));
                                //        });

                                //}

                                break;
                            }

                        }
                        //_intervaloService.excluir(item)
                        //    .done(function () {
                        //        getFormataItem();
                        //        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        //    });
                    }
                }
            );
        }

        abp.event.on('app.CriarOuEditarFormataItemModalSaved', function () {
            getFormataItem();
        });

        

        //FormataItem

        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        function salvarRegistro() {
            if (!_$FormataItemInformationForm.valid()) {
                return;
            }
            if ($('#ordem').val() == null || $('#ordem').val() == '' || $('#ordem').val() == undefined || $('#ordem').val() == 0) {
                abp.notify.error(app.localize('PreencherCampo'));
                $('#ordem').focus();
                return;
            }
            var list = $('#formata-itens').val();

            var FormataItem = _$FormataItemInformationForm.serializeFormToObject();
            var form1 = FormataItem;

            if (form1.ItemResultado == null || form1.ItemResultado == '' || form1.ItemResultado == undefined) {
                $.ajax({
                    url: '/api/services/app/itemresultado/obter?id=' + form1.ItemResultadoId,
                    method: 'POST',
                    async: false,
                    cache: false,
                    success: function (data) {
                        form1.ItemResultado = data.result;
                        form1.ItemResultado.Codigo = data.result.codigo;
                        form1.ItemResultado.Descricao = data.result.descricao;
                        form1.ItemResultado.Ordem = data.result.ordem;
                    },
                })
            }

            if (list != '') {
                var lista = JSON.parse(list);
            }
            else {
                var lista = [];
            }

            if (form1.IdGrid != null && form1.IdGrid != '' && form1.IdGrid != undefined) {
                //var itemProcessado = false;
                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGrid == form1.IdGrid) {

                        lista[i].Codigo = form1.Codigo;
                        lista[i].Descricao = form1.Descricao;
                        lista[i].FormataId = form1.FormataId;
                        lista[i].Id = form1.Id;
                        lista[i].IdGrid = form1.IdGrid;
                        lista[i].ItemResultadoId = form1.ItemResultadoId;
                        lista[i].ItemResultado = form1.ItemResultado;
                        lista[i].Ordem = form1.Ordem;
                        lista[i].OrdemRegistro = form1.OrdemRegistro;
                        lista[i].Formula = form1.Formula;
                        lista[i].IsBi = form1.IsBi;
                        lista[i].IsRefExame = form1.IsRefExame;

                        //_$FormataItemTable.jtable('updateRecord', {
                        //    record: lista[i],
                        //    clientOnly: true
                        //});

                        //itemProcessado = true;
                        break;
                    }
                }
                //if (!itemProcessado) {
                //    form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                //    form1.FormataId = $('#formata-id').val();
                //    lista.push(form1);
                //}
            }
            else {
                form1.IdGrid = lista.length == 0 ? 1 : lista[lista.length - 1].IdGrid + 1;
                form1.FormataId = $('#formata-id').val();
                lista.push(form1);

                //_$FormataItemTable.jtable('addRecord', {
                //    record: form1,
                //    clientOnly: true
                //});

            }
            $('#formata-itens').val(JSON.stringify(lista));
            abp.notify.info(app.localize('ListaAtualizada'));
            abp.event.trigger('app.CriarOuEditarFormataItemModalSaved');
            //_modalManager.close();
            novoRegistro();
        }

        $('#item-resultado-id').select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: '/api/services/app/itemresultado/listardropdown',
                dataType: 'json',
                delay: 250,
                method: 'Post',

                data: function (params) {
                    if (params.page == undefined)
                        params.page = '1';

                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10,
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;

                    return {
                        results: data.result.items,
                        pagination: {
                            more: (params.page * 10) < data.result.totalCount
                        }
                    };
                },
                cache: true
            },
            escapeMarkup: function (markup) { return markup; }, // let our custom formatter work
            minimumInputLength: 0
        }).on('change', function () {
            if ($(this).val() != null && $(this).val() != '' && $(this).val() != undefined) {
                $('#dados-item-resultado').removeClass('hidden');
                preencherItemResultado($(this).val());
            }
            else {
                $('#dados-item-resultado').addClass('hidden');
                limparItemResultado();
            }
        });

        function preencherItemResultado(id) {
            $.ajax({
                url: '/api/services/app/itemresultado/obter?id=' + id,
                //data: { id: id },
                method: 'POST',
                success: function (data) {
                    var record = data.result;
                    if (record.laboratorioUnidadeId && record.laboratorioUnidade) {
                        $('#laboratorio-unidade-id')
                            .append('<option value="' + record.laboratorioUnidadeId + '">' + record.laboratorioUnidade.descricao + '</option>')
                            .val(record.laboratorioUnidadeId)
                            .trigger('change').attr('readonly', 'readonly');
                    }
                    if (record.tipoResultadoId && record.tipoResultado) {
                        $('#tipo-resultado-id')
                            .append('<option value="' + record.tipoResultadoId + '">' + record.tipoResultado.descricao + '</option>')
                            .val(record.tipoResultadoId)
                            .trigger('change').attr('readonly', 'readonly');
                    }
                    $('#casa-decimal').val(record.casaDecimal).attr('readonly', 'readonly');
                    $('#minimo-aceitavel-masculino').val(record.minimoAceitavelMasculino).attr('readonly', 'readonly');
                    $('#minimo-masculino').val(record.minimoMasculino).attr('readonly', 'readonly');
                    $('#normal-masculino').val(record.normalMasculino).attr('readonly', 'readonly');
                    $('#maximo-masculino').val(record.maximoMasculino).attr('readonly', 'readonly');
                    $('#maximo-aceitavel-masculino').val(record.maximoAceitavelMasculino).attr('readonly', 'readonly');
                    $('#minimo-aceitavel-feminino').val(record.minimoAceitavelFeminino).attr('readonly', 'readonly');
                    $('#minimo-feminino').val(record.minimoFeminino).attr('readonly', 'readonly');
                    $('#normal-feminino').val(record.normalFeminino).attr('readonly', 'readonly');
                    $('#maximo-feminino').val(record.maximoFeminino).attr('readonly', 'readonly');
                    $('#maximo-aceitavel-feminino').val(record.maximoAceitavelFeminino).attr('readonly', 'readonly');
                    $('#item-resultado-descricao').val(record.descricao).attr('readonly', 'readonly');
                },
            })
        }

        function limparItemResultado() {
            $('#laboratorio-unidade-id').val(null);
            $('#tipo-resultado-id').val(null);
            $('#casa-decimal').val('');
            $('#minimo-aceitavel-masculino').val('');
            $('#minimo-masculino').val('');
            $('#normal-masculino').val('');
            $('#maximo-masculino').val('');
            $('#maximo-aceitavel-masculino').val('');
            $('#minimo-aceitavel-feminino').val('');
            $('#minimo-feminino').val('');
            $('#normal-feminino').val('');
            $('#maximo-feminino').val('');
            $('#maximo-aceitavel-feminino').val('');
            $('#item-resultado-descricao').val('');
        }

        function novoRegistro() {
            $('#formata-id').val($('#formata-id').val());
            $('#item-resultado-id').val(null).trigger('change');
            $('#ordem').val('');
            $('#ordem-registro').val('');
            $('#formula').val('');
            $('#is-bi').removeAttr('checked');
            $('#is-ref-exame').removeAttr('checked');
            $('#id-grid').val('');
            $('#id-formata-item').val(0);

            $('#salvar-formata-item i').removeClass('fa-check').addClass('fa-plus');

            $('#exibir-sw-div-retratil-formata-item').trigger('click');
        }

        function editarRegistro(id, idGrid, record) {
            abp.ui.setBusy();
            var list = JSON.parse($('#formata-itens').val());
            var data;
            for (var i = 0; i < list.length; i++) {
                if (list[i].IdGrid == idGrid) {
                    data = list[i];
                    break;
                }
            }
            $('#formata-id').val(data.FormataId);
            if (data.ItemResultadoId != null && data.ItemResultadoId > 0) {
                if (record.itemResultado == null || record.itemResultado == '' || record.itemResultado == undefined) {
                    $.ajax({
                        url: '/api/services/app/itemresultado/obter?id=' + data.ItemResultadoId,
                        method: 'POST',
                        async: false,
                        cache: false,
                        success: function (data) {
                            record.itemResultado = data.result;
                        },
                    })
                }
                $('#item-resultado-id')
                    .append('<option value="' + data.ItemResultadoId + '">' + record.itemResultado.descricao + '</option>')
                    .val(data.ItemResultadoId)
                    .trigger('change');
            }
            $('#ordem').val(data.Ordem);
            $('#ordem-registro').val(data.OrdemRegistro);
            $('#formula').val(data.Formula);
            if (data.IsBi) {
                $('#is-bi').attr('checked', 'checked');
            }
            else {
                $('#is-bi').removeAttr('checked');
            }
            if (data.IsRefExame) {
                $('#is-ref-exame').attr('checked', 'checked');
            }
            else {
                $('#is-ref-exame').removeAttr('checked');
            }
            $('#id-grid').val(data.IdGrid);
            $('#id-formata-item').val(data.Id);

            $('#salvar-formata-item i').removeClass('fa-plus').addClass('fa-check');

            abp.ui.clearBusy();

            $('#exibir-sw-div-retratil-formata-item').trigger('click');
        }

        $('#salvar-formata-item').on('click', function (e) {
            e.preventDefault();
            salvarRegistro();
        })

        $('#cancelar-formata-item').on('click', function (e) {
            e.preventDefault();
            novoRegistro();
            $('#omitir-sw-div-retratil-formata-item').trigger('click');
        })
    };
})(jQuery);