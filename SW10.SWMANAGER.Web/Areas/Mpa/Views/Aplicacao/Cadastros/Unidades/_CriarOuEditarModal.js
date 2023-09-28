(function ($) {
    app.modals.CriarOuEditarModal = function () {

        var _createOrEditUnidadeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Unidades/CriarOuEditarUnidadeModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Unidades/_CriarOuEditarUnidadeModal.js',
            modalClass: 'CriarOuEditarUnidadeModal'
        });

        var _unidadeService = abp.services.app.unidade;

        var _modalManager;
        var _$UnidadeInformationForm = null;

        function retirarMascara(valor) {
            while (valor.indexOf('_') != -1) valor = valor.replace('_', '');
            while (valor.indexOf('.') != -1) valor = valor.replace('.', '');

            valor = valor.replace(',', '.');

            return valor;
        }

        this.init = function (modalManager) {

            _modalManager = modalManager;

            _$UnidadeInformationForm = _modalManager.getModal().find('form[name=UnidadeInformationsForm]');
            _$UnidadeInformationForm.validate();

            $('.modal-dialog').css({ 'width': '100%', 'max-width': '800px' });
            //$('div.form-group select').addClass('form-control selectpicker');
            //Forçando o modal a aceitar sobreposição por causa dos selects do datarangepicker
            $.fn.modal.Constructor.prototype.enforceFocus = function () { };
            //Ativando autocomplete no combobox
            //$('.chosen-select').chosen({ no_results_text: app.localize("NotFound"), width: '100%' });
            $('ul.ui-autocomplete').css('z-index', '2147483647');

            //-----------------------------------
            ////debugger;
            //Define o label do botao Salvar
            if (($('#Id').val() == 0) || ($('#Id').val() == '') || ($('#Id').val() == undefined)) {
                //se inclusao
                $('.save-button').html('<i class="fa fa-save"></i> Salvar e Continuar');
            }
            else {                
                $('.save-button').html('<i class="fa fa-save"></i> Salvar');
                $('#linhaUnidadesRelacionadas').show();
                getUnidadeTable();
            };

            $('#sigla').focus();
        };

        this.save = function () {
            if (!_$UnidadeInformationForm.valid()) {
                return;
            }

            var valor = retirarMascara($('#fator').val());

            $('#fator').val(valor);

            var unidade = _$UnidadeInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);

            if (($('#Id').val() != "") && ($('#Id').val() != undefined) && ($('#Id').val() != 0)) {
                _unidadeService.criarOuEditar(unidade)
                     .done(function (data) {
                         abp.notify.info(app.localize('SavedSuccessfully'));
                         abp.event.trigger('app.CriarOuEditarUnidadeModalSaved');
                         //location.reload();//seguindo o projeto pronto
                         _modalManager.close();
                     })
                    .always(function () {
                        _modalManager.setBusy(false);
                    });

            } else {

                _unidadeService.criarGetId(unidade)
                     .done(function (data) {
                         //debugger;
                         abp.notify.info(app.localize('SavedSuccessfully'));
                         //abp.event.trigger('app.CriarOuEditarUnidadeModalSaved');

                         $('#Id').val(data.id);

                         $("#codigo").prop("readonly", "");
                         $("#codigo").val(data.id);
                         $("#codigo").prop("readonly", true);

                         $('#linhaUnidadesRelacionadas').show();

                         //getUnidadeTable();

                         $('.save-button').html('<i class="fa fa-save"></i> Salvar');

                         $('#sigla').focus();

                     })
                    .always(function () {
                        _modalManager.setBusy(false);

                        abp.event.trigger('app.CriarOuEditarUnidadeModalSaved');
                    });
            };
        };

        $('#btn-novo-produto-unidade').click(function (e) {
            e.preventDefault();

            if (($('#Id').val() == 0) || ($('#Id').val() == '') || ($('#Id').val() == undefined)) {
                abp.message.warn("Antes de incluir é necessário salvar a unidade principal", "Salve a unidade");
            }
            else {
                e.preventDefault();
                _createOrEditUnidadeModal.open();
            };
        });

        //---------------------------------------------------------------------------------------------------
        var _$UnidadesRelacionadasTable = $('#unidades-relacionadas-table');

        function criarGrid (){
            _$UnidadesRelacionadasTable.jtable({
                title: app.localize('UnidadesRelacionadas'),
                paging: true,
                sorting: true,
                //useBootstrap: true,
                multiSorting: true,
                edit: false,
                create: false,
                pageSize: 10,
                actions: {
                    listAction: {
                        method: _unidadeService.listarPorReferencial
                    }
                },
                fields:
                {
                    actions: {
                        title: app.localize('Actions'),
                        width: '10%',
                        sorting: false,
                        display: function (data) {
                            var $span = $('<span></span>');
                            //  if (_permissions.edit) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    _createOrEditUnidadeModal.open({ id: data.record.id });
                                });
                            // }

                            // if (_permissions.delete) {
                            $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                .appendTo($span)
                                .click(function (e) {
                                    e.preventDefault();
                                    deleteUnidade(data.record);
                                });
                            // }

                            return $span;
                        }
                    },
                    id: {
                        title: app.localize('codigo'),
                        width: '10%'
                    },
                    sigla: {
                        title: app.localize('sigla'),
                        width: '10%'
                    },

                    fator: {
                        title: app.localize('fator'),
                        width: '10%'
                    },
                    descricao: {
                        title: app.localize('Descricao'),
                        width: '45%'
                    },
                    creationTime: {
                        title: app.localize('CreationTime'),
                        width: '15%',
                        display: function (data) {
                            return moment(data.record.creationTime).format('L');
                        }
                    }
                }
            });
        };

        //Grid
        //----------------------------------------------------------------------------------------------------
        criarGrid();

        function resetGrid() {
            _$UnidadesRelacionadasTable.jtable('destroy');
            criarGrid();
        }

        function deleteUnidade(Unidade) {
            abp.message.confirm(
            app.localize('DeleteWarning', Unidade.descricao),
            function (isConfirmed) {
                if (isConfirmed) {
                    _unidadeService.excluir(Unidade)
                        .done(function () {
                            //sessionStorage['ProdutoId'] = Unidade.produtoId;
                            getUnidadeTable();
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                }
            });
        };

        function getUnidadeTable(reload) {
            //debugger;
            //resetGrid();
            criarGrid();

            if (reload) {
                _$UnidadesRelacionadasTable.jtable('reload');
            } else {
                _$UnidadesRelacionadasTable.jtable('load', { filtro: $('#Id').val() });
            }
        }

        if (($('#Id').val() != 0) && ($('#Id').val() != '') && ($('#Id').val() != undefined)) {
            getUnidadeTable();
        };

        abp.event.on('app.CriarOuEditarUnidadeFilhaModalSaved', function () {
            //debugger;
            getUnidadeTable();

        });

        $('#sigla').focus();

    };
})(jQuery);