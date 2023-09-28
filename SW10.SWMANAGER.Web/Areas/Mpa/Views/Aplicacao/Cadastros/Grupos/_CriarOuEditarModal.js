(function ($) {
    app.modals.CriarOuEditarModal = function () {

        var _grupoService = abp.services.app.grupo;
        var _grupoClasseService = abp.services.app.grupoClasse;
        var _grupoSubClasseService = abp.services.app.grupoSubClasse;

        var fixaModal = true;

        //operacao 1 - add
        //operacao 2 - delete

        var operacao = 1;

        var _modalManager;

        var _$GrupoInformationForm = null;

        function retornarLista(filtro) {

            if ($('#classes-list').val() == '[]') {
                $('#classes-list').val('');
            }
            var list = $('#classes-list').val();
            if (list != '') {
                var js = list;
                var json = JSON.parse(js);
                var res = _grupoClasseService.listarJson(json);  //  '"{Result":"OK","Records":' + js + '}'
                return res;
            }
            else {
                var res = _grupoClasseService.listar({ filtro: $('#id').val() });
                return res;
            }
        }

        this.init = function (modalManager) {

            _modalManager = modalManager;

            _modalManager.getModal().find('#div-btn-fixa-modal').show();

            var btnFixaModal = _modalManager.getModal().find('#btn-fixa-modal:last');
            btnFixaModal.addClass('blue');

            btnFixaModal.on('click', function (e) {
                fixaModal = !fixaModal;
                if (fixaModal) {
                    btnFixaModal.addClass('blue');
                } else {
                    btnFixaModal.removeClass('blue');
                }
            });

            _$GrupoInformationForm = _modalManager.getModal().find('form[name=GrupoInformationsForm]');
            _$GrupoInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '900px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            //$('ul.ui-autocomplete').css('z-index', '2147483647');

            //-----------------------------------
            $('.save-button').html('<i class="fa fa-save"></i> Salvar');

            $('#descricao-grupo').focus();
        };

        this.save = function () {
            var editMode = $('#is-edit-mode').val();

            if (!_$GrupoInformationForm.valid()) {
                return;
            }

            var grupo = _$GrupoInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            _grupoService.criarOuEditar(grupo)
                 .done(function (data) {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     // Fixar modal ou nao, apos save
                     if (!fixaModal) {
                         _modalManager.close();
                     } else {
                         if (editMode) {
                             //$('#label-gerais').click();
                             abp.notify.info(app.localize('SavedSuccessfully'));
                         } else {
                             //limparFormulario();
                         };

                         $('#descricao-grupo').focus();
                         abp.event.trigger('app.CriarOuEditarGrupoModalSaved');
                         abp.event.trigger('app.CriarOuEditarGrupoClasseModalSaved');
                         //getClassesTable();
                         createClasse();
                     };

                     //location.reload();//seguindo o projeto pronto

                 })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };


        $('#btn-nova-classe').click(function (e) {
            e.preventDefault();

            createClasse();
        });

        //---------------------------------------------------------------------------------------------------
        var _$ClassesTable = $('#classes-table');

        function criarGrid() {
            _$ClassesTable.jtable({
                title: app.localize('GruposClasse'),
                paging: true,
                sorting: true,
                multiSorting: true,
                selecting: true,
                //selectingCheckboxes: true,

                actions: {
                    listAction: {
                        method: retornarLista
                    }
                },

                fields: {
                    id: {
                        key: true,
                        list: false
                    },
                    actions: {
                        title: app.localize('Actions'),
                        width: '10%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');

                            //if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();

                                    editClasse(data.record.idGridClasse);
                                });
                            //}

                            //if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    //deleteTabelas(data.record);
                                    deleteClasse(data.record);
                                });
                            //}

                            return $span;
                        }
                    }
                    ,
                    descricao: {
                        title: app.localize('Descricao'),
                        width: '78%',
                        display: function (data) {
                            if (data.record.isDeleted) {
                                return '<span style="text-decoration:line-through;color:red;">' + data.record.descricao + '</span>';
                            }
                            else {
                                return data.record.descricao;
                            }
                        }
                    }
                    ,
                    idGridClasse: {
                        title: app.localize('IdGrid'),
                        width: '10%'
                    }
                    //,
                    //isDeleted: {
                    //    title: app.localize('IsDeleted'),
                    //    width: '10%',
                    //    display: function (data) {
                    //        
                    //        if (data.record.isDeleted) {
                    //            return 'Sim'
                    //        } else {
                    //            return 'Nao'
                    //        }

                    //    }
                    //}
                    ,
                    creationTime: {
                        title: app.localize('CreationTime'),
                        width: '12%',
                        display: function (data) {
                            if (data.record.isDeleted) {
                                return '<span style="text-decoration:line-through;color:red;">' + moment(data.record.creationTime).format('L') + '</span>';
                            }
                            else {
                                return moment(data.record.creationTime).format('L');
                            }
                        }
                    }
                }
                //,
                //selectionChanged: function () {
                //    var configSelecionadas = _$ClassesTable.jtable('selectedRows');

                //    if (configSelecionadas.length > 0) {
                //        configSelecionadas.each(function () {

                //            //var record = $(this).data('record');

                //            //if (record.dataIncio)
                //            //    $("input[name='DataIncio']").val(moment(record.dataIncio).format('L'));
                //            //else
                //            //    $("input[name='DataIncio']").val(moment(new Date()).format('L'));

                //            //if (record.dataFim)
                //            //    $("input[name='DataFim']").val(moment(record.dataFim).format('L'));
                //            //else
                //            //    $("input[name='DataFim']").val('');

                //            //if (record.empresa)
                //            //    setarSel2($('#cbo-empresas'), record.empresa.id, abp.services.app.empresa);
                //            //else
                //            //    $('#cbo-empresas').empty().trigger('change');

                //            //if (record.grupo)
                //            //    setarSel2($('#cbo-grupos'), record.grupo.id, abp.services.app.faturamentoGrupo);
                //            //else
                //            //    $('#cbo-grupos').empty().trigger('change');

                //            //if (record.tabela)
                //            //    setarSel2($('#cbo-tabelas'), record.tabela.id, abp.services.app.faturamentoTabela);
                //            //else
                //            //    $('#cbo-tabelas').empty().trigger('change');

                //            //if (record.plano)
                //            //    setarSel2($('#cbo-planos'), record.plano.id, abp.services.app.plano);
                //            //else
                //            //    $('#cbo-planos').empty().trigger('change');


                //            //if (record.subGrupo)
                //            //    setarSel2($('#cbo-subgrupos'), record.subGrupo.id, abp.services.app.faturamentoSubGrupo);
                //            //else
                //            //    $('#cbo-subgrupos').empty().trigger('change');

                //            //$('#div-form').removeClass('contorno-placebo');
                //            //$('#div-form').addClass('contornado');
                //            //$('#icone-btn-salvar').removeClass('fa fa-plus');
                //            //$('#icone-btn-salvar').addClass('glyphicon glyphicon-edit');
                //            //$('#titulo-config').html('@L("Editando")');
                //            ////   $('#cabec-config').addClass('titulo-azul');
                //            //$('#btn-apagar-config').fadeIn();
                //        });
                //    } else {
                //        // Resetar form
                //        resetarForm();
                //    }
                //}
            });
        }

        //Grid
        //----------------------------------------------------------------------------------------------------

        criarGrid();

        //---------------------------------------------------------------------------------------------------

        function deleteClasse(classe) {
            var lista = JSON.parse($('#classes-list').val());
            //var classe;
            var indice;
            for (var i = 0; i < lista.length; i++) {
                if (lista[i].IdGridClasse == classe.idGridClasse) {
                    //classe = lista[i];
                    indice = i;
                    break;
                }
            }

            var msgDialog;

            if (lista[indice].IsDeleted) {
                msgDialog = app.localize('RestaurarWarning', lista[indice].Descricao);
            } else {
                msgDialog = app.localize('DeleteWarning', lista[indice].Descricao);
            };

            abp.message.confirm(
                msgDialog,
                function (isConfirmed) {
                    if (isConfirmed) {
                        //var lista = JSON.parse($('#classes-list').val());

                        if (lista[indice].IsDeleted) {
                            lista[indice].IsDeleted = false;
                            lista[indice].DeleterUserId = '';
                        }
                        else {
                            lista[indice].IsDeleted = true;
                            lista[indice].DeleterUserId = abp.session.userId;
                        }

                        $('#classes-list').val(JSON.stringify(lista));
                        //localStorage["ClassesList"] = JSON.stringify(lista);
                        abp.notify.info(app.localize('ListaAtualizada'));
                        //abp.event.trigger('app.CriarOuEditarPrescricaoModalSaved');
                        getClassesTable (true);
                    }
                }
            );
        }

        function editClasse(idGrid) {
            operacao = 2;

            $('#operacao-classe').empty();
            $('#operacao-classe').append(app.localize('EditarRegistro'));

            var list = JSON.parse($('#classes-list').val());

            for (var i = 0; i < list.length; i++) {
                if (list[i].IdGridClasse == idGrid) {
                    var data = { result: list[i] };
                    break;
                }
            }

            $('#idGridClasse').val(data.result.IdGridClasse);
            $('#codigo-classe').val(data.result.Codigo);
            $('#descricao-classe').val(data.result.Descricao);
            $('#descricao-classe').focus();

            $('#icone-btn-salvar').removeClass('fa-plus').addClass('fa-check');
        }

        function createClasse() {
            //localStorage['DivisaoId'] = divisaoId;

            operacao = 1;

            $('#operacao-classe').empty();
            $('#operacao-classe').append(app.localize('NovoRegistro'));

            if ($('#classes-list').val() != '') {
                var list = JSON.parse($('#classes-list').val());
            }
            else {
                var list = [];
            }

            limparControlesClasse();

            $('#descricao-classe').focus();

            $('#icone-btn-salvar').removeClass('fa-check').addClass('fa-plus');
        }

        $('#salvar-classe').on('click', function (e) {
            e.preventDefault();

            var classeForm = _modalManager.getModal().find('form[name=ClasseInformationsForm]');

            classeForm.validate();

            if (!classeForm.valid()) {
                return;
            }

            //---

            var classesList = $('#classes-list').val();

            if (classesList != '') {
                var lista = JSON.parse(classesList);
            }
            else {
                var lista = [];
            }

            var form1 = classeForm.serializeFormToObject();

            if (operacao == 1) {
                form1.IdGridClasse = lista.length == 0 ? 1 : lista[lista.length - 1].IdGridClasse + 1;
                //form1.Descricao = $('jhghjk')
                //form1.PrescricaoId = $('#id-' + localStorage["AtendimentoId"]).val();

                //form1.Id = null;
                form1.Id = 0;
                form1.GrupoId = $('#Id').val();
                form1.IsSistema = false;
                form1.IsDeleted = false;
                form1.CreationTime = moment($.now()).format('YYYY-MM-DD hh:mm:ss.SSS');
                form1.CreatorUserId = abp.session.userId;

                lista.push(form1);

            } else if (operacao == 2) {
                for (var i = 0; i < lista.length; i++) {
                    if (lista[i].IdGridClasse == form1.IdGridClasse) {
                        lista[i].Descricao = form1.Descricao;
                        itemProcessado = true;
                        break;
                    }
                }
            };

            $('#classes-list').val(JSON.stringify(lista));
            abp.notify.info(app.localize('ListaAtualizada'));
            abp.event.trigger('app.CriarOuEditarGrupoClasseModalSaved');
            //getClassesTable();
            createClasse();

        });

        function limparControlesClasse() {
            $('#idGridClasse').val('');
            $('#codigo-classe').val('');
            $('#descricao-classe').val('');
        };

        function resetGrid() {
            //_$GruposRelacionadasTable.jtable('destroy');
            ////criarGrid();
        }


        abp.event.on('app.CriarOuEditarGrupoClasseModalSaved', function () {
            getClassesTable(true);
        });

        function getClassesTable(reload) {
            criarGrid();

            if (reload) {
                _$ClassesTable.jtable('reload');
            } else {
                _$ClassesTable.jtable('load', { filtro: $('#id').val() });
            }
        }

        getClassesTable();

        $('#descricao').focus();

    };
})(jQuery);