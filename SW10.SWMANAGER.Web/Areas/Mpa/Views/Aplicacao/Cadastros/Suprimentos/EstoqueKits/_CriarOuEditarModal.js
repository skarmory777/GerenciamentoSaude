(function ($) {
    app.modals.CriarOuEditarEstoqueKitModal = function () {

        var _estoqueKitService = abp.services.app.estoqueKit;
        var _modalManager;
        var _$kitInformationForm = null;

        // Itens
        var _$ItensTable = $('#ItensTable');
        var _estoqueKitItemService = abp.services.app.estoqueKitItem;

        selectSWWithDefaultValue('.selectGrupo', "/api/services/app/Grupo/ListarDropdown");
        selectSWWithDefaultValue('.selectClasse', "/api/services/app/GrupoClasse/ListarDropdown", $('#grupoId'));

        $('#grupoId').on('change', function (e) {
            e.preventDefault();
            selectSWWithDefaultValue('.selectClasse', "/api/services/app/GrupoClasse/ListarDropdown", $('#grupoId'));
            selectSWWithDefaultValue('.itemSel2', "/api/services/app/Produto/ListarProdutoPorGrupoDropdown", $('#grupoId'));
        });

        $('#grupoClasseId').on('change', function (e) {
            e.preventDefault();

            selectSWMultiplosFiltros('.selectProduto', "/api/services/app/Produto/ListarProdutoPorGrupoEClasseDropdown", ['.selectGrupo', '.selectClasse']);
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.KitEstoqueItem.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.KitEstoqueItem.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Suprimentos.KitEstoqueItem.Delete')
        };

        _$ItensTable.jtable({
            title: app.localize('Itens'),
            paging: true,
            pageSize: 10,
            sorting: true,
            multiSorting: true,
            actions: {
                listAction: {
                    method: _estoqueKitItemService.listarItensKit
                }
            },
            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '5%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();

                                    $("#itemId").val(data.record.id);

                                    $("#cbo-item").append('<option value="' + data.record.produto.id + '">' + data.record.produto.codigo + ' - ' + data.record.produto.descricao + '</option>');
                                    $("#cbo-item").val(data.record.produto.id);
                                    $("#cbo-item").trigger('change');

                                    $('#quantidade').val(data.record.quantidade);

                                    $("#CreateNewItemButton").text(app.localize('UpdateNewItem'));
                                });
                        }
                        if (_permissions.delete) {
                            $('<span class="btn btn-default btn-xs" title="' + app.localize('delete') + '"><i class="fa fa-trash-alt"></i></span>')
                                .appendTo($span)
                                .click(function () {
                                    deleteItem(data.record);
                                });
                        }

                        return $span;
                    }
                },
                produtoCodigo: {
                    title: app.localize('Codigo'),
                    width: '5%',
                    display: function (data) {
                        return data.record.produto.codigo;
                    }
                },
                produtoDescricao: {
                    title: app.localize('Descricao'),
                    width: '10%',
                    display: function (data) {
                        return data.record.produto.descricao;
                    }
                },
                quantidade: {
                    title: app.localize('Quantidade'),
                    width: '5%'
                }
            }
        });

        function deleteItem(item) {
            abp.message.confirm(
                app.localize('DeleteWarning', item.produto.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _estoqueKitItemService.excluir(item)
                            .done(function () {
                                getItens(true);
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function getItens(reload) {
            if (reload) {
                _$ItensTable.jtable('reload');
            } else {
                _$ItensTable.jtable('load', { estoqueKitId: $('#id').val() });
            }
        }

        $('#CreateNewItemButton').click(function (e) {
            e.preventDefault();

            if ($('#quantidade').val() == '') {
                abp.notify.info(app.localize('QuantidadeRequerida'));
                return;
            }

            var itemSelecionadoId = $('#cbo-item :selected').val();
            if (itemSelecionadoId == '') {
                abp.notify.info(app.localize('ItemRequerido'));
                return;
            }

            var selIdLong = parseInt(itemSelecionadoId, 10);
            
            var estoqueKitItem = {};
            estoqueKitItem.Id = $("#itemId").val();
            estoqueKitItem.EstoqueKitId = $("#id").val();
            estoqueKitItem.Quantidade = $('#quantidade').val();
            estoqueKitItem.ProdutoId = selIdLong;

            _estoqueKitItemService.criarOuEditar(estoqueKitItem)
                .done(function () {
                    $("#cbo-item").val('');
                    $("#cbo-item").trigger('change');
                    $('#quantidade').val('');
                    $("#itemId").val('');
                    $("#CreateNewItemButton").text(app.localize('CreateNewItem'));

                    abp.notify.info(app.localize('SavedSuccessfully'));
                    getItens();
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        });

        getItens();

        $('#btn-salvar-custom').click(function (e) {
            salvarEstoqueKit(false);
        });

        function salvarEstoqueKit(fecharModal) {
            if (!_$kitInformationForm.valid()) {
                return;
            }

            var kit = _$kitInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _estoqueKitService.criarOuEditar(kit)
                .done(function (kitEstoqueId) {
                    abp.notify.info(app.localize('SavedSuccessfully'));

                    if (fecharModal) {
                        _modalManager.close();
                        abp.event.trigger('app.CriarOuEditarKitModalSaved');
                    }
                    else {
                        $('#btn-salvar-custom').hide();
                        $('#btn-salvar-original').show();
                        $('#itens-div').slideDown();

                        abp.event.trigger('app.CriarOuEditarKitModalSaved');

                        $('.modal-dialog').animate({ top: "0px" });

                        $('#id').val(kitEstoqueId);
                    }         
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        }

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$kitInformationForm = _modalManager.getModal().find('form[name=KitInformationsForm]');
            _$kitInformationForm.validate({ ignore: "" });
            $('.modal-dialog:last').css('width', '800px');
            $('.modal-dialog:last').css('top', '40px');


        };

        this.save = function () {
            salvarEstoqueKit(true);
        };
    };
})(jQuery);