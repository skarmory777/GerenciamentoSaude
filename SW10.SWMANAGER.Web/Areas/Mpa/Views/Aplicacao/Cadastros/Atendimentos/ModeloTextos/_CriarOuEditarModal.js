
(function ($) {
    $(function () {

        var _ModelosModeloTextoService = abp.services.app.modeloTexto;
        var _tipoModeloService = abp.services.app.tipoModelo;

        var summerNoteOptions = {
            toolbar: [
                ['printSize', ['printSize']],
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
            width: '100%',
            height:300,
            padding: 30,
            disableResizeEditor: true
            // NA DOCUMENTACAO TEM VARIOS OUTROS BAGULHOS INTERESSANTES
        };
        
        $(document).ready(function () {
            CamposRequeridos();
            $.summernote.options.lineHeights = ["0","0.2", "0.4", "0.6", "0.8", "1.0"];


            

            $('.text-editor').summernote(summerNoteOptions);
            
            $(".text-editor").summernote("code", $('#conteudo').val());
            $('.note-statusbar').hide();


            changeSelectTipoModelo();

            changeSelectTamanho();


            //var selectTipoModelo = $('.selectTipoModelo').select2();
            //selectTipoModelo.trigger("open");
            //$('.selectTipoModelo').select2().val($("#tipoModeloId").val()).trigger('change');
            //console.log($('.selectTamanho').select2().val());
            //$('.selectTamanho').val($("#tamanhoModeloId").val()).trigger('change');    

            //$('div.note-editable.panel-body').tagautocomplete({
            //    source: ['@EmpresaRazaoSocial', '@Empresa', '@DataHora', '@CodigoPaciente', '@Endereco',
            //             '@Contrato', '@Acompanhante', '@Responsavel', '@CodInternacao', '@Leito', '@DataValidade', '@Senha', '@DiasAutorizados',
            //             '@Numero', '@Complemento', '@Bairro', '@Estado', '@Cidade', '@Telefone', '@Cep',
            //             '@Pais', '@Nacionalidade', '@Filiacao', '@Sexo', '@Profissao', '@Paciente', '@Cid',
            //             '@Nascimento', '@Idade', '@Cpf', '@SituacaoCivil', '@DataAtendimento', '@Identidade', '@Medico',
            //             '@Especialidade', '@IndicadoPor', '@Origem', '@Tratamento', '@Convenio', '@Plano',
            //             '@Guia', '@NumberGuide', '@Titular', '@CodigoAtendimento', '@DataAlta', '@Alta',
            //             '@Matricula', '@DataPagto', '@IdAcompanhante', '@CodDep', '@Usuario', '@CodigoBarra',
            //             '@1DtHoraAtendimento', '@2DtHoraAtendimento', '@1MedicoAtendimento', '@2MedicoAtendimento',
            //             '@1ConvenioAtendimento', '@2ConvenioAtendimento', '@1Espec', '@2Espec', '@[', '@]'],
            //    after: function (itemSelection) {
            //        //to run after selection 
            //        console.log('After...!  ' + itemSelection);
            //    }
            //});

        });

        //var _preMovimentoService = abp.services.app.estoquePreMovimento;


        var _ErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Erros/ExibirErros',
        });

        //var _createOrEditLoteValidadeProdutoModal = new app.ModalManager({
        //    viewUrl: abp.appPath + 'Mpa/PreMovimentos/InformarLoteValidadeProdutoModal',
        //    modalClass: 'EstoquePreMovimentoLoteValidadeProdutoViewModel'
        //});

        $('#salvarMovimentoAutomatico').click(function (e) {
            e.preventDefault();

            var _$movimentoAutomaticoInformationsForm = $('form[name=movimentoAutomaticoInformationsForm');

            _$movimentoAutomaticoInformationsForm.validate();

            if (!_$movimentoAutomaticoInformationsForm.valid()) {
                return;
            }

            var movimentoAutomatico = _$movimentoAutomaticoInformationsForm.serializeFormToObject();

            movimentoAutomatico['LstFatGuiaId'] = [];
            movimentoAutomatico['LstEmpresaId'] = [];

            if (movimentoAutomatico.TiposGuias != '') {
                var lstEmpresaId = JSON.parse(movimentoAutomatico.TiposGuias);
                if (lstEmpresaId.length > 0) {
                    lstEmpresaId.forEach(v=> {
                        movimentoAutomatico['LstFatGuiaId'].push(v.tipoGuiaId);
                    });
                }
            }

            if (movimentoAutomatico.Especialidades != '') {
                var lstFatGuiaId = JSON.parse(movimentoAutomatico.Especialidades);
                if (lstFatGuiaId.length > 0) {
                    lstFatGuiaId.forEach(v=> {
                        movimentoAutomatico['LstEmpresaId'].push(v.especialidadeId);
                    });
                }
            }
            movimentoAutomatico['tipoModeloId'] = $('.selectTipoModelo').val();
            movimentoAutomatico['tamanhoModeloId'] = $('.selectTamanho').val();

            //_modalManager.setBusy(true);
            _ModelosModeloTextoService.criarOuEditar(movimentoAutomatico)
                 .done(function (data) {
                     if (data.errors.length > 0) {
                         _ErrorModal.open({ erros: data.errors });
                     }
                     else {
                         abp.notify.info(app.localize('SavedSuccessfully'));
                         location.href = '/Mpa/ModeloTexto';
                         //$('#id').val(result.result.returnObject.id);
                     }
                 })
                .always(function () {
                    //_modalManager.setBusy(false);
                });

        });

        $('.close').on('click', function () {
            location.href = '/mpa/ModeloTexto';
        });

        $('.close-button').on('click', function () {
            location.href = '/mpa/ModeloTexto';
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
                        return data.record.tipoGuiaDescricao;
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

            if ($('#especialidades').val() != '')
                JSON.parse($('#especialidades').val()).forEach((lstEmpresa) => {
                    _ModelosModeloTextoService.isGuide(lstEmpresa.especialidadeId, $('#tipoGuiaId').val())
                   .done(function (data) {
                       if (data) {
                           alert("Esse Tipo de Guia já foi vinculada para essa empresa!");
                           $('#tipoGuiaId').empty();
                           return false;
                       }
                   })
                  .always(function () {
                      //_modalManager.setBusy(false);
                  });
                });

            var tipoGuia = {};

            tipoGuia['IdGrid'] = listaTiposGuias.length == 0 ? 1 : listaTiposGuias[listaTiposGuias.length - 1].IdGrid + 1;

            var campotipoGuia = $('#tipoGuiaId').select2('data');
            if (campotipoGuia && campotipoGuia.length > 0) {
                tipoGuia.tipoGuiaDescricao = campotipoGuia[0].text;
            }
            tipoGuia.tipoGuiaId = $('#tipoGuiaId').val();

            if (listaTiposGuias.filter(e=> { return e.tipoGuiaId == tipoGuia.tipoGuiaId }).length > 0) {
                return;
            }

            listaTiposGuias.push(tipoGuia);

            _$tiposGuiaTable.jtable('addRecord', {
                record: tipoGuia
                  , clientOnly: true
            });

            $('#tiposGuias').val(JSON.stringify(listaTiposGuias));
            $('#tipoGuiaId').val('').trigger('change');

            $("#empresaId").empty();

        });

        function myLocalStorage() {
            let storageGrid = window.localStorage.getItem('gridEmpresaGuia');
            let obj = JSON.parse(storageGrid);
            return obj;
        }

        function getTiposLeitos() {

            if ($('#tiposGuias').val() != undefined && $('#tiposGuias').val() != '') {
                listaTiposGuias = JSON.parse($('#tiposGuias').val());
            } else if (document.querySelector('#id').value != '') {
                $('#conteudo').val(myLocalStorage().record.texto);
                let count = 1;
                myLocalStorage().record.guias.forEach((e) => {
                    e['IdGrid'] = count;
                    listaTiposGuias.push(e);
                    count++;
                });
                $('#tiposGuias').val(JSON.stringify(listaTiposGuias));
            }
            //listaTiposGuias = JSON.parse($('#tiposGuias').val());

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
                app.localize('DeleteWarning', tipoGuia.tipoGuiaDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {

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

            title: app.localize('Empresas'),
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
                        return data.record.especialidadeDescricao;
                    }
                },
            }
        });

        var listaEspecialidades = [];

        $('#inserirEspecialidade').click(function (e) {
            e.preventDefault();

            if ($('#empresaId').val() == '' || $('#empresaId').val() == null) {
                return;
            }

            if ($('#tiposGuias').val() != '')
                JSON.parse($('#tiposGuias').val()).forEach((lstGuia) => {
                    _ModelosModeloTextoService.isGuide($('#empresaId').val(), lstGuia.tipoGuiaId)
                   .done(function (data) {
                       if (data) {
                           alert("Essa empresa já foi vinculada para esse tipo de guia!");
                           $('#empresaId').empty();
                           return false;
                       }
                   })
                  .always(function () {
                      //_modalManager.setBusy(false);
                  });
                });

            var especialidade = {};

            especialidade.IdGrid = listaEspecialidades.length == 0 ? 1 : listaEspecialidades[listaEspecialidades.length - 1].IdGrid + 1;

            var campoEspecialidade = $('#empresaId').select2('data');
            if (campoEspecialidade && campoEspecialidade.length > 0) {
                especialidade.especialidadeDescricao = campoEspecialidade[0].text;
            }
            especialidade.especialidadeId = $('#empresaId').val();

            if (listaEspecialidades.filter(e=> { return e.especialidadeId == especialidade.especialidadeId }).length > 0) {
                return;
            }

            listaEspecialidades.push(especialidade);

            _$especialidadeTable.jtable('addRecord', {
                record: especialidade
                  , clientOnly: true
            });

            $('#especialidades').val(JSON.stringify(listaEspecialidades));
            $('#empresaId').val('').trigger('change');

        });

        function getEspecialidades() {

            if ($('#especialidades').val() != undefined && $('#especialidades').val() != '') {
                listaEspecialidades = JSON.parse($('#especialidades').val());
            } else if (document.querySelector('#id').value != '') {
                let count = 1;
                myLocalStorage().record.empresas.forEach((e) => {
                    e['IdGrid'] = count;
                    listaEspecialidades.push(e);
                    count++;
                });
                $('#especialidades').val(JSON.stringify(listaEspecialidades));
            }

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
                app.localize('DeleteWarning', especialidade.especialidadeDescricao),
                function (isConfirmed) {
                    if (isConfirmed) {

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


        selectSW('.selectEmpresa', "/api/services/app/empresa/ListarDropdown");
        selectSW('.selectTipoGuia', "/api/services/app/FaturamentoGuia/ListarDropdown");
        selectSW('.selectTipoModelo', "/api/services/app/tipoModelo/ListarDropdownAsync");
        selectSW('.selectTamanho', "/api/services/app/tipoModelo/ListarTamanhoModeloDropdownAsync");
        
        $('.selectTipoModelo').change(function(e) {
            e.preventDefault();
            changeSelectTipoModelo();
        });

        function changeSelectTipoModelo() {
            if ($('.selectTipoModelo').val() !== null && $('.selectTipoModelo').val() !== '') {

                if ($('.selectTipoModelo').val() == "1") {
                    $("#empresaEspecialidade").show();
                    $("#outrosCampos").show();
                } else {
                    $("#outrosCampos").hide();
                    $("#empresaEspecialidade").hide();
                }

                _tipoModeloService.obterVariaveisPorTipoModeloAsync($('.selectTipoModelo').val())
                    .done((data) => {
                        if (!data || data.length <= 0) {
                            updateSummernote([]);
                            return;
                        }
                        updateSummernote(data.map(x => x.descricao));
                    });
            }
            else {
                $("#outrosCampos").hide();
                $("#empresaEspecialidade").hide();
                updateSummernote([]);
            }

            changeSelectTamanho();

            function updateSummernote(dataMentions) {
                var currentSummerNoteOptions = {};
                $.extend(currentSummerNoteOptions,
                    summerNoteOptions,
                    {
                        hint: {
                            mentions: dataMentions,
                            match: /\B@(\w*)$/,
                            search: function (keyword, callback) {
                                callback($.grep(this.mentions, function (item) {
                                    return item.length && item.indexOf(keyword) === 0;
                                }));
                            },
                            content: function (item) {
                                return '@' + item;
                            }
                        }
                    });
                $('.text-editor').summernote('destroy');
                $('.text-editor').summernote(currentSummerNoteOptions);

                $(".text-editor").summernote("code", $('#conteudo').val());

            }
        }

        

        $('.selectTamanho').change(function (e) {
            e.preventDefault();
            changeSelectTamanho();
        });

        function changeSelectTamanho()
        {
            if ($('.selectTamanho').val() !== null && $('.selectTamanho').val() !== '') {
                _tipoModeloService.obterTamanhoAsync($('.selectTamanho').val())
                    .done((data) => {
                        if (data !== null) {

                            var maxWidth = Math.floor($(window).width() * 0.0264583333);
                            var width = 0;
                            var height = 0;

                            debugger;
                            if (maxWidth > (data.larguraCm * 3)) {
                                width = data.larguraCm * 3;
                                height = data.alturaCm;
                            } else {
                                width = data.larguraCm;
                                height = data.alturaCm;
                            }
                            

                            $(".note-editable").css({
                                'width': `${width}cm`,
                                'height': `${height}cm`,
                                'margin': 'auto'
                            });

                            $(".note-editor").css({
                                'height': `${height}cm`
                            });

                            $(".note-editing-area").css({
                                "background-color": '#c3c3c3'
                            });

                            $(".portlet.light.bordered.footer").css({
                                "margin-top": `calc(${height}cm/2)`
                            });


                        }
                    });
            }
        }

        $('#tipoGuiaId').change(function (e) {
            e.preventDefault();

            if ($('#tipoGuiaId').val() != null && $('#especialidades').val() != '')
                JSON.parse($('#especialidades').val()).forEach((lstEmpresa) => {
                    _ModelosModeloTextoService.isGuide(lstEmpresa.especialidadeId, $('#tipoGuiaId').val())
                   .done(function (data) {
                       if (data) {
                           alert("Esse Tipo de Guia já foi vinculada para essa empresa!");
                           $('#tipoGuiaId').empty();
                           return false;
                       }
                   })
                  .always(function () {
                      //_modalManager.setBusy(false);
                  });
                });
        });

        $('#empresaId').change(function (e) {
            e.preventDefault();

            if ($('#empresaId').val() != null && $('#tiposGuias').val() != '')
                JSON.parse($('#tiposGuias').val()).forEach((lstGuia) => {
                    _ModelosModeloTextoService.isGuide($('#empresaId').val(), lstGuia.tipoGuiaId)
                   .done(function (data) {
                       if (data) {
                           alert("Essa empresa já foi vinculada para esse tipo de guia!");
                           $('#empresaId').empty();
                           return false;
                       }
                   })
                  .always(function () {
                      //_modalManager.setBusy(false);
                  });
                });
        });


    });

})(jQuery);