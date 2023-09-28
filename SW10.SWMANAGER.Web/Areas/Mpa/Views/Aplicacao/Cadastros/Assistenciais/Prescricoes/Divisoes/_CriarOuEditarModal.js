(function ($) {
    app.modals.CriarOuEditarDivisaoModal = function () {

        var _divisoesService = abp.services.app.divisao;


        var _modalManager;
        var _$DivisaoInformationForm = null;

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Create'),
            edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Edit'),
            'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Assistenciais.Prescricao.Divisao.Delete')
        };

        var _createOrEditSubDivisaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Divisoes/_CriarOuEditarSubDivisaoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_CriarOuEditarSubDivisaoModal.js',
            modalClass: 'CriarOuEditarSubDivisaoModal'
        });

        var _createOrEditTipoPrescricaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Divisoes/_CriarOuEditarTipoPrescricaoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_CriarOuEditarTipoPrescricaoModal.js',
            modalClass: 'CriarOuEditarTipoPrescricaoModal'
        });

        var _selecionarDivisoesModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Divisoes/_SelecionarSubDivisaoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_SelecionarSubDivisaoModal.js',
            modalClass: 'SelecionarSubDivisaoModal'
        });

        var _selecionarTiposPrescricaoModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Divisoes/_SelecionarTiposPrescricaoModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/Divisoes/_SelecionarTiposPrescricaoModal.js',
            modalClass: 'SelecionarTiposPrescricaoModal'
        });

        var configuracaoPrescricaoItem;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _$DivisaoInformationForm = _modalManager.getModal().find('form[name=DivisaoInformationsForm]');
            _$DivisaoInformationForm.validate();
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            $('.modal-dialog').css({ 'width': '95%' });
            $('.select2').css('width', '100%');
        };

        $('.chk-montagem-tela').click(function (e) {
            contaChk();
        });

        $('.chk-configuracao').click(function (e) {
            contaChk();
        });

        this.save = function () {
            if (!_$DivisaoInformationForm.valid()) {
                return;
            }

            var divisao = _$DivisaoInformationForm.serializeFormToObject();
            divisao.TiposRespostasSelecionadas = '';
            //$('input[name="TiposRespostasSelecionadas"]:checked').each(function () {
            //    divisao.TiposRespostasSelecionadas += $(this).val() + ",";
            //});
            //if (divisao.TiposRespostasSelecionadas=='') {
            //    abp.notify.warn(app.localize('SelecaoObrigatoria'));
            //    return;
            //}
            //if ($('.chk-montagem-tela:checked').length == 0) {
            //    $('#lnk-montagem-tela-tab').trigger('click');
            //    abp.notify.warn(app.localize('SelecaoObrigatoria'));
            //    return;
            //}
            //if ($('.chk-configuracao:checked').length == 0) {
            //    $('#lnk-configuracao-tab').trigger('click');
            //    abp.notify.warn(app.localize('SelecaoObrigatoria'));
            //    return;
            //}

            //divisao.TiposRespostasSelecionadas = divisao.TiposRespostasSelecionadas.substring(0, divisao.TiposRespostasSelecionadas.length - 1);
            _modalManager.setBusy(true);
            _divisoesService.criarOuEditar(divisao)
                .done(function (item) {
                    $("#id").val(item.id);
                    configuracaoPrescricaoItem.save().then(() => {
                        abp.notify.success(app.localize('SavedSuccessfully'));
                        _modalManager.close();
                        abp.event.trigger('app.CriarOuEditarDivisaoModalSaved');
                    })
                    //location.reload();//seguindo o projeto pronto
                })
                .always(function () {
                    _modalManager.setBusy(false);

                });
        };

        $('#sub-divisoes-table').jtable({

            title: app.localize('SubDivisoes'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _divisoesService.listarSubDivisoes
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditSubDivisaoModal.open({ divisaoPrincipalId: data.record.divisaoPrincipalId, id: data.record.id });
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteSubDivisoes(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%'
                },
                //isMedico: {
                //    title: app.localize('IsMedico'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.isMedico) {
                //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default">' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
                //isEnfermagem: {
                //    title: app.localize('IsEnfermagem'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.isEnfermagem) {
                //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default">' + app.localize('No') + '</span>';
                //        }
                //    }
                //},
                //isDivisaoPrincipal: {
                //    title: app.localize('IsDivisaoPrincipal'),
                //    width: '10%',
                //    display: function (data) {
                //        if (data.record.isDivisaoPrincipal) {
                //            return '<span class="label label-success">' + app.localize('Yes') + '</span>';
                //        } else {
                //            return '<span class="label label-default">' + app.localize('No') + '</span>';
                //        }
                //    }
                //}
            }
        });

        $('#tipos-prescricao-table').jtable({

            title: app.localize('TiposPrescricao'),
            paging: true,
            sorting: true,
            multiSorting: true,

            actions: {
                listAction: {
                    method: _divisoesService.listarTiposPrescricao
                }
            },

            fields: {
                id: {
                    key: true,
                    list: false
                },
                actions: {
                    title: app.localize('Actions'),
                    width: '20%',
                    sorting: false,
                    display: function (data) {
                        var $span = $('<span></span>');
                        if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditTipoPrescricaoModal.open({ divisaoId: data.record.divisaoId, id: data.record.id });
                                });
                        }
                        if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteTipoPrescricao(data.record);
                                });
                        }

                        return $span;
                    }
                },
                codigo: {
                    title: app.localize('Codigo'),
                    width: '10%'
                },
                descricao: {
                    title: app.localize('Descricao'),
                    width: '40%'
                },
            }
        });

        function getSubDivisoes() {
            $('#sub-divisoes-table').jtable('load', {
                divisaoPrincipalId: $('#divisao-principal-troca').val()
            });
        }

        function getTiposPrescricao() {
            $('#tipos-prescricao-table').jtable('load', {
                divisaoId: $('#id').val()
            });
        }

        function selecionarSubDivisoes() {
            $('#sub-divisoes-table').jtable('load', {
                divisaoPrincipalId: $('#divisao-principal-id-selecionar').val()
            });
        }

        function deleteSubDivisoes(divisao) {
            abp.message.confirm(
                app.localize('DeleteWarning', divisao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _divisoesService.excluir(divisao)
                            .done(function () {
                                getSubDivisoes();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function deleteTipoPrescricao(tipoPrescricao) {
            abp.message.confirm(
                app.localize('DeleteWarning', tipoPrescricao.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _divisoesService.excluirTipoPrescricao(tipoPrescricao)
                            .done(function () {
                                getTiposPrescricao();
                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                            });
                    }
                }
            );
        }

        function contaChk() {
            var chkMontagem = $('.chk-montagem-tela');
            var chkMontagemChecked = $('.chk-montagem-tela:checked');
            var chkConfiguracao = $('.chk-configuracao');
            var chkConfiguracaoCheked = $('.chk-configuracao:checked');
            if (chkMontagem.length == chkMontagemChecked.length) {
                $('#is-todos-montagem-tela').attr('checked', 'checked');
            }
            else {
                $('#is-todos-montagem-tela').removeAttr('checked');
            }
            if (chkConfiguracao.length == chkConfiguracaoCheked.length) {
                $('#is-todos-configuracao').attr('checked', 'checked');
            }
            else {
                $('#is-todos-configuracao').removeAttr('checked');
            }
        }

        $("#divisao-principal-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/divisao/listarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    //   //console.log('data: ', params, (params.page == undefined));
                    if (params.page == undefined)
                        params.page = '1';
                    //   //console.log('data: ', params);
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10
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
        });

        $("#tipo-prescricao-id").select2({
            allowClear: true,
            placeholder: app.localize("SelecioneLista"),
            ajax: {
                url: "/api/services/app/tipoPrescricao/listarDropdown",
                dataType: 'json',
                delay: 250,
                method: 'Post',
                data: function (params) {
                    //   //console.log('data: ', params, (params.page == undefined));
                    if (params.page == undefined)
                        params.page = '1';
                    //   //console.log('data: ', params);
                    return {
                        search: params.term,
                        page: params.page,
                        totalPorPagina: 10
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
        });

        $('#CreateNewSubDivisaoButton').click(function (e) {
            e.preventDefault();
            _createOrEditSubDivisaoModal.open({ divisaoPrincipalId: $('#divisao-principal-troca').val() });
        });

        $('#CreateNewTipoPrescricaoButton').click(function (e) {
            e.preventDefault();
            _createOrEditTipoPrescricaoModal.open({ divisaoId: $('#id').val() });
        });

        $('#SelecionarSubDivisaoButton').click(function (e) {
            e.preventDefault();
            _selecionarDivisoesModal.open({ filtro: $('#divisao-principal-troca').val() });
        });

        $('#SelecionarTipoPrescricaoButton').click(function (e) {
            e.preventDefault();
            _selecionarTiposPrescricaoModal.open({ divisaoId: $('#id').val() });
        });

        $('#GetSubDivisoesButton, #RefreshSubDivisoesListButton').click(function (e) {
            e.preventDefault();
            getSubDivisoes();
        });

        $('#GetTiposPrescricaoButton, #RefreshTiposPrescricaoListButton').click(function (e) {
            e.preventDefault();
            getTiposPrescricao();
        });

        $('#chk-is-divisao-principal').on('click', function (e) {
            if ($(this).is(':checked')) {
                $('#lst-sub-divisao-tab').removeClass('hidden');
                $('#sub-divisao-tab').removeClass('hidden');
                $('#lst-divisao-principal-tab').addClass('hidden');
                $('#divisao-principal-tab').addClass('hidden');
                $('#divisao-principal-troca').val($('#id').val());
                getSubDivisoes();
            }
            else {
                $('#lnk-montagem-tela-tab').trigger('click');
                $('#lst-divisao-principal-tab').removeClass('hidden');
                $('#divisao-principal-tab').removeClass('hidden');
                $('#lst-sub-divisao-tab').addClass('hidden');
                $('#sub-divisao-tab').addClass('hidden');
                $(this).removeAttr('checked');
            }
        });

        $('#divisao-principal-id').on('change', function (e) {
            //$('#sub-divisoes-area').removeClass('hidden');
            $('#divisao-principal-troca').val($(this).val());
            //getSubDivisoes();
        });

        $('#is-todos-montagem-tela').on('click', function (e) {
            if ($(this).is(':checked')) {
                //$('input[name=TiposRespostasSelecionadas]').attr('checked', 'checked');
                $('.chk-montagem-tela').attr('checked', 'checked');
            }
            else {
                $('.chk-montagem-tela').removeAttr('checked');
            }
        });

        $('#is-todos-configuracao').on('click', function (e) {
            if ($(this).is(':checked')) {
                //$('input[name=TiposRespostasSelecionadas]').attr('checked', 'checked');
                $('.chk-configuracao').attr('checked', 'checked');
            }
            else {
                $('.chk-configuracao').removeAttr('checked');
            }
        });

        abp.event.on('app.CriarOuEditarSubDivisaoModalSaved', function () {
            getSubDivisoes();
        });

        abp.event.on('app.SelecionarSubDivisaoModalSaved', function () {
            getSubDivisoes();
        });

        abp.event.on('app.CriarOuEditarTipoPrescricaoModalSaved', function () {
            getTiposPrescricao();
        });

        abp.event.on('app.SelecionarTipoPrescricaoModalSaved', function () {
            getTiposPrescricao();
        });

        if (!$('#is-divisao-principal').is(':checked')) {
            getSubDivisoes();
        }

        contaChk();
        configuracaoPrescricaoItem = BuildConfiguracaoPrescricaoItem();

        configuracaoPrescricaoItem.renderDivisao();


        
    };
})(jQuery);