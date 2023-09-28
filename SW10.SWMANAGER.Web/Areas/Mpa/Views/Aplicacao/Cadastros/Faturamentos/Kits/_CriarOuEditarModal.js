(function ($) {
    app.modals.CriarOuEditarFaturamentoKitModal = function () {

        var _kitsService = abp.services.app.faturamentoKit;
        var _modalManager;
        var _$kitInformationForm = null;

        // Itens
        var _$ItensTable = $('#ItensTable');
        var _ItensService = abp.services.app.faturamentoItem;
        var _$filterForm = $('#ItensFilterForm');

        var _itens = [];
        // Extraindo ids de itens previamente associados ao kit
        $("#itensDiv > input").each(function () {
            _itens.push($(this).val());
        });
        
        

        function getItens(reload) {
            if ($('#strItensQtds').val() != '') {
                _itens = JSON.parse($('#strItensQtds').val());
            }

            var allRows = _$ItensTable.find('.jtable-data-row')
            
            if (allRows != null && allRows.length > 0) {
                $.each(allRows, function () {
                    var id = $(this).attr('data-record-key');

                    _$ItensTable.jtable('deleteRecord', { key: id, clientOnly: true });

                });
            }
                                   
            for (var i = 0; i < _itens.length; i++) {
                var item = _itens[i];
                _$ItensTable.jtable('addRecord', {
                    record: item
                    , clientOnly: true
                })
            }


            //if (reload) {
            //    _$ItensTable.jtable('reload');
            //} else {
            //    _$ItensTable.jtable('load', { kitFaturamentoId: $('#id').val() });
            //}
        }

        function deleteItens(item) {
            debugger;
            for (i = 0; i < _itens.length; i++) {
                if (_itens[i].ItemId == item.ItemId) {
                    _itens.splice(i, 1);
                }
            }
            $('#strItensQtds').val(JSON.stringify(_itens));
            getItens();
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$kitInformationForm = _modalManager.getModal().find('form[name=KitInformationsForm]');
            _$kitInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '800px');
            $('.modal-dialog:last').css('top', '40px');

            var _permissions = {
                create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Itens.Create'),
                edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Itens.Edit'),
                'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Itens.Delete')
            };

            var _createOrEditModal = new app.ModalManager({
                viewUrl: abp.appPath + 'Mpa/FaturamentoItens/CriarOuEditarModal',
                scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/_CriarOuEditarModal.js',
                modalClass: 'CriarOuEditarFaturamentoItemModal'
            });

            _$ItensTable.jtable({
                title: app.localize('Itens'),
                sorting: true,
                edit: false,
                create: false,
                multiSorting: true,

                fields: {
                    ItemId: {
                        key: true,
                        list: false
                    },
                    actions: {
                        title: app.localize('Actions'),
                        width: '2%',
                        sorting: false,
                        display: function (data) {


                            var $span = $('<span></span>');

                            if (_permissions.delete) {
                                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                    .appendTo($span)
                                    .click(function (e) {
                                        e.preventDefault();
                                        deleteItens(data.record);
                                    });
                            }
                            return $span;
                        }
                    },
                    Descricao: {
                        title: app.localize('Descricao'),
                        width: '15%',
                        display: function (data) {
                            if (data.record.Descricao) {
                                if (data.record.Codigo)
                                    return data.record.Codigo + " - " + data.record.Descricao;
                                else
                                    return data.record.Descricao;
                            }
                        }
                    }
                    ,
                    Quantidade: {
                        title: app.localize('Qtd'),
                        width: '5%',
                        display: function (data) {
                            if (data.record.Quantidade) {
                                return data.record.Quantidade;
                            }
                        }
                    }                                        
                }
            });

            getItens();
        };

        this.save = function () {
            if (!_$kitInformationForm.valid()) {
                return;
            }

            var kit = _$kitInformationForm.serializeFormToObject();
            
            kit.strItensQtds = JSON.stringify(_itens);


            _modalManager.setBusy(true);

            _kitsService.criarOuEditar(kit)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     _modalManager.close();
                     abp.event.trigger('app.CriarOuEditarKitModalSaved');
                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };

        // Inserir Item
        $('#inserir-item').click(function () {
            var itemSelecionadoId = $('#cbo-item :selected').val();
            var selIdLong = parseInt(itemSelecionadoId, 10);
            var quantidade = parseFloat($('#quantidade').val());
            var descricao = $('#cbo-item :selected').text();
            adicionarItemSeNaoHouver(selIdLong, quantidade, descricao);
            $('#strItensQtds').val(JSON.stringify(_itens));
            getItens();
        });


        function adicionarItemSeNaoHouver(ItemId, Quantidade, Descricao) {
            var jaExiste = false;
            for (i = 0; i < _itens.length; i++) {
                if (_itens[i].ItemId == ItemId) {
                    _itens[i].Quantidade = Quantidade;
                    jaExiste = true;
                }
            }
            if (!jaExiste) {
                var item = { ItemId, Quantidade, Descricao };
                item.ItemId = ItemId
                item.Quantidade = Quantidade;
                item.Descricao = Descricao;

                _itens.push(item);
            }
        }
    };
})(jQuery);