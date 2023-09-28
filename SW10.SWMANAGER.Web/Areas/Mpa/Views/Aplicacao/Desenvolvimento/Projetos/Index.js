// SIGNAL R =========================================================================================
//Retrair menu principal


$('body').addClass('page-sidebar-closed');
$('#menu-lateral').addClass('page-sidebar-menu-closed');

$("#msgErro").css("display", "none");
$("#msgErroRespo").css("display", "none");
$("#msgErroStatus").css("display", "none");
//$('#msgErro').hide();

_selectedDateRange = { startDate: moment().startOf('day'), endDate: moment().endOf('day') };
var smweHubApp = $.connection.sMWEHubApp;
var smweHubWeb = $.connection.sMWEHubWeb;
smweHubApp.client.apenderNovoComentario = function (data) {
    var tarefaSelecionadaId = $('#tarefa-selecionada-id').val();
    if (tarefaSelecionadaId == data.tarefaId) {
        apenderComentario(data);
    }
};
smweHubApp.client.srGetTarefasExecutando = function () {
    ////console.log('srGetTarefasExecutandoFuncionou');
    getTarefasExecutando();
};
abp.signalr.connect();
// FIM - SIGNAL R

// Tarefas
var tarefasTable = $('#tarefas-jtable');
var tarefaAppService = abp.services.app.tarefa;
var tarefasFiltroForm = $('#tarefas-filtro-form');


// Projetos
var projetosTable = $('#projetos-jtable');
var projetoAppService = abp.services.app.projeto;
var projetosFiltroForm = $('#projetos-filtro-form');
// Modulos
var modulosTable = $('#modulos-jtable');
//var docRotuloAppService = abp.services.app.docRotulo;
var modulosFiltroForm = $('#modulos-filtro-form');
// Status
var statusTable = $('#status-jtable');
//var docRotuloAppService = abp.services.app.docRotulo;
var statusFiltroForm = $('#status-filtro-form');
// Prioridade
var prioridadesTable = $('#prioridade-jtable');
//var docRotuloAppService = abp.services.app.docRotulo;
var prioridadeFiltroForm = $('#prioridade-filtro-form');
// Execucao
var execucaoTable = $('#execucao-jtable');
// Horas Realizadas
var horasTable = $('#horas-jtable');
// Comentarios
var comentarioAppService = abp.services.app.comentario;

// DocRotulos
var docRotuloAppService = abp.services.app.docRotulo;

// Usuario logado
comentarioAppService.getUsuarioLogadoAsync().done(function (data) {
    //     debugger
    var usuarioLogadoId = data;
    $('#usuario-logado-id').val(usuarioLogadoId);
});
// fim - usuario logado

// PERMISSOES -  A DEFINIR ==================================================================================
//var _permissions = {
//    create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tarefas.Create'),
//    edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tarefas.Edit'),
//    'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.Tarefas.Delete')
//};

// TAREFAS =================================================================================================

//controle select2---------------------------------
$('#tarefa-filtro-data').hide("slow");
$(".select2TarefaFiltroResp").select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: "/api/services/app/user/ListarDropdown",
        dataType: 'json',
        delay: 250,
        method: 'Post',
        data: function (params) {
            if (params.page == undefined)
                params.page = '1';
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
$(".select2TarefasFiltroStatus").select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: "/api/services/app/docRotulo/ListarStatusDropdown",
        dataType: 'json',
        delay: 250,
        method: 'Post',
        data: function (params) {
            if (params.page == undefined)
                params.page = '1';
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
$(".select2TarefasFiltroPrioridade").select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: "/api/services/app/docRotulo/ListarPrioridadesDropdown",
        dataType: 'json',
        delay: 250,
        method: 'Post',
        data: function (params) {
            if (params.page == undefined)
                params.page = '1';
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
$(".select2TarefasFiltroModulo").select2({
    allowClear: true,
    placeholder: app.localize("SelecioneLista"),
    ajax: {
        url: "/api/services/app/docRotulo/ListarModulosDropdown",
        dataType: 'json',
        delay: 250,
        method: 'Post',
        data: function (params) {
            if (params.page == undefined)
                params.page = '1';
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
//fim--Controle select2---------------------------------

//filtros automaticos---------------------------------
//$('#cbo-tarefa-filtro-resp').on('change', function () {
//    //getTarefas();
//});
//$('#tarefa-filtro-data').on('change', function (e) {
//    //getTarefas();
//});

//$('#cbo-tarefa-filtro-status').on('change', function () {
//  //  getTarefas();
//});
//$('#cbo-tarefa-filtro-modulo').on('change', function () {
//   // getTarefas();
//});
//$('#cbo-tarefa-filtro-prior').on('change', function () {
//   /// getTarefas();
//});
//$('#filtrar-data').on('change', function () {
//  //  getTarefas();
//});
$('#cbo-tarefa-filtro-tipoData').on('change', function () {
    // var QtdAcomodacaoD = $("#QtdAcomodacaoDuplo").val();
    //console.log("tipoData: ", $('#cbo-tarefa-filtro-tipoData').val());
    var tipoData = $('#cbo-tarefa-filtro-tipoData').val();
    if (tipoData != null && tipoData != "") {
        $('#tarefa-filtro-data').show("slow");
    } else {
        $('#tarefa-filtro-data').hide("slow");
    }
});
$('#cbo-tarefa-filtro-status').on('change', function () {
    // //console.log("Aqui");
    //getTarefas();
});
$('#GetTarefasButton').on('click', function () {
    // //console.log("filtro: ", $('#filtro').val());
    // getTarefas();
});
$('#RefreshTarefasButton').on('click', function () {
    getTarefas();
});
$('#sw-form-retratil-btn-remover-selecao-tarefas').on('click', function () {
    //resetFormRetratilTarefas();
    $('#tarefa-selecionada-id').val('');

    $('#tarefa-codigo').val('');
    $('#tarefa-descricao').val('');
    $('#tarefa-prioridade').val('');
    $("input[name='DataPrevistaInicio']").val(moment(new Date()).format("L"));
    $("input[name='DataPrevistaTermino']").val('');
    $("input[name='DataInicio']").val('');
    $("input[name='DataTermino']").val('');
    $("input[name='DataRegistro']").val(moment(new Date()).format("L"));
    resetCboTarefaProj();
    resetCboTarefaMod();
    resetCboTarefaStatus();
    resetCboTarefaPrioridade();
    resetCboTarefaResp();
    $('#tarefa-conteudo').summernote('code', '');
    //  resetCboTarefaCliente();

    //$('#btn-intervalo-pause').addClass('botao-disabled');
    //$('#btn-intervalo-play').addClass('botao-disabled');

    $('#tempo-decorrido').html('');
    var containerComentarios1 = $('#container-comentarios');
    containerComentarios1.html('');

    $('#sw-form-retratil-cabecalho-tarefas').html('<b>Nova</b>');
    $('#omitir-form-retratil-tarefas').click();
});
$('#cbo-tarefa-resp').on('change', function () {
    if ($('#cbo-tarefa-resp').val() == "") {
        $('#cbo-tarefa-resp').addClass("requered");

        $('#msgErroRespo').show();
        //alert("Data previsão de inicio obrigatório !") msgErro
        ////console.log("return");
        abp.notify.info(app.localize('Campos obrigatórios'));
        return
    } else {
        $('#msgErroRespo').hide();
        $('#cbo-tarefa-resp').removeClass("requered");

    }
});
$('#GetTarefasButton').on('click', function () {
    // //console.log("filtro: ", $('#filtro').val());
    // getTarefas();
});
var focus = function () {
    $("#tarefa-descricao").focus();
}
$('#novaTarefa').on('click', function () {
    $('#exibir-form-retratil-tarefas').click();
    setTimeout(focus, 200);
});

//fim---filtros---------------------------------

//$('#btn-get-tarefas').click(function (e) {
//    e.preventDefault();
//    getTarefas();
//});

$('#tarefa-conteudo').summernote({
    height: 500
});

tarefasTable.jtable({
    title: app.localize('Tarefas'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    selectingCheckboxes: true,
    // Botoes da tabela
    actions: {
        listAction: {
            method: tarefaAppService.listarFiltrando
        }
    },
    // Campos da tabels
    fields: {
        id: {
            key: true,
            list: false
        },
        actions: {
            title: app.localize('Actions'),
            width: '7%',
            sorting: false,
            display: function (data) {
                var $span = $('<span></span>');
                //  if (_permissions.edit) {
                //$('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //    .appendTo($span)
                //    .click(function () {
                //    });
                //    }

                //    if (_permissions.delete) {
                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                    .appendTo($span)
                    .click(function () {
                        deleteTarefa(data.record);
                    });
                //     }
                return $span;
            }
        }
        ,

        identidade: {
            title: app.localize('Id'),
            width: '5%',
            display: function (data) {
                return data.record.id;
            }
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '5%'
        }
        ,
        ordem: {
            title: app.localize('Ordem'),
            width: '5%'
        }
        ,
        projetoNome: {
            title: app.localize('Projeto'),
            width: '8%',
            display: function (data) {
                if (data.record.projeto) {
                    return data.record.projeto.descricao;
                }
            }
        }
        ,
        moduloNome: {
            title: app.localize('Módulo'),
            width: '7%',
            display: function (data) {
                if (data.record.modulo) {
                    return data.record.modulo.descricao;
                }
            }
        }
        ,
        dataRegistro: {
            title: app.localize('Criacao'),
            width: '6%',
            display: function (data) {
                if (data.record.dataPrevistaInicio != null) {
                    return moment(data.record.dataRegistro).format('L');
                } else {
                    return "";
                }
            }
        }
        ,
        dataPrevistaInicio: {
            title: app.localize('PrevisaoInicio'),
            width: '6%',
            display: function (data) {
                if (data.record.dataPrevistaInicio != null) {
                    return moment(data.record.dataPrevistaInicio).format('L');
                } else {
                    return ""
                }
            }
        }
        ,
        dataInicio: {
            title: app.localize('DataInicio'),
            width: '6%',
            display: function (data) {
                if (data.record.dataInicio != null) {
                    return moment(data.record.dataInicio).format('L');
                } else {
                    return ""
                }

            }
        }
        ,
        dataPrevistaTermino: {
            title: app.localize('PrevisaoTermino'),
            width: '6%',
            display: function (data) {
                if (data.record.dataPrevistaTermino != null) {
                    return moment(data.record.dataPrevistaTermino).format('L');
                } else {
                    return ""
                }
            }
        }
        ,
        dataTermino: {
            title: app.localize('DataTermino'),
            width: '6%',
            display: function (data) {
                if (data.record.dataTermino != null) {
                    return moment(data.record.dataTermino).format('L');
                } else {
                    return ""
                }
            }
        }
        ,
        Criador: {
            title: app.localize('Criador'),
            width: '8%',
            display: function (data) {
                if (data.record.user) {
                    return data.record.user.nome;
                }
            }
        }
        ,
        responsavelNome: {
            title: app.localize('Responsavel'),
            width: '8%'
        }
        ,
        statusDescricao: {
            title: app.localize('Status'),
            width: '6%',
            display: function (data) {
                return '<div style="text-align:center; display:inline-block;">  <span style="display:inline-block; width:20px; height:20px; top:100px;  text-align:center; background-color:' + data.record.statusCor + '; border-radius: 25px;">  </span> </div>    ' + data.record.statusDescricao;
            }
        }
        ,
        prioridadeDescricao: {
            title: app.localize('Prioridade'),
            width: '4%'
        },
    },
    // Selecao de registro
    selectionChanged: function () {
        var tarefasSelecionados = tarefasTable.jtable('selectedRows');
        if (tarefasSelecionados.length > 0) {
            // temp - para abrir formRetratil

            setTimeout(focus, 100);

            tarefasSelecionados.each(function () {
                var registro = $(this).data('record');
                //      //console.log(JSON.stringify(registro));
                $('#tarefa-selecionada-id').val(registro.id);
                $('#responsavel-tarefa-id').val(registro.responsavelId);

                tarefaAppService.obter(registro.id)
                  .done(function (data) {

                      setFormRetratilTarefas(data);
                      $('#exibir-form-retratil-tarefas').click();
                  });

            });
        } else {
            resetFormRetratilTarefas();
            $('#omitir-form-retratil-tarefas').click();
        }
    }
});

getTarefas();

//functions--------------------------------
//Setar e Resetar FormRetratil Tarefas
function setFormRetratilTarefas(registro) {

    abp.ui.setBusy();

    registro.codigo && $('#tarefa-codigo').val(registro.codigo) || $('#tarefa-codigo').val('');
    registro.descricao && $('#tarefa-descricao').val(registro.descricao) || $('#tarefa-descricao').val('');
    registro.ordem && $('#tarefa-ordem').val(registro.ordem) || $('#tarefa-ordem').val('');
    registro.id && $('#tarefa-id-form').val(registro.id.toString()) || $('#tarefa-id-form').val('');
    registro.dataPrevistaInicio && $("input[name='DataPrevistaInicio']").val(moment(registro.dataPrevistaInicio).format('L')) || $("input[name='DataPrevistaInicio']").val(moment(new Date()).format('L'));
    registro.dataPrevistaTermino && $("input[name='DataPrevistaTermino']").val(moment(registro.dataPrevistaTermino).format('L')) || $("input[name='DataPrevistaTermino']").val("");
    registro.dataInicio && $("input[name='DataInicio']").val(moment(registro.dataInicio).format('L')) || $("input[name='DataInicio']").val("");
    registro.dataTermino && $("input[name='DataTermino']").val(moment(registro.dataTermino).format('L')) || $("input[name='DataTermino']").val("");
    registro.dataRegistro && $("input[name='DataRegistro']").val(moment(registro.dataRegistro).format('L')) || $("input[name='DataRegistro']").val(moment(new Date()).format('L'));
    registro.projetoId && setCboTarefaProj(registro.projetoId) || resetCboTarefaProj();
    registro.moduloId && setCboTarefaMod(registro.moduloId) || resetCboTarefaMod();
    registro.statusId && setCboTarefaStatus(registro.statusId) || resetCboTarefaStatus();
    registro.prioridadeId && setCboTarefaPrioridade(registro.prioridadeId) || resetCboTarefaPrioridade();
    registro.responsavelId && setCboTarefaResp(registro.responsavelId) || resetCboTarefaResp();

    if (registro.conteudo)
        $('#tarefa-conteudo').summernote('code', registro.conteudo);
    else
        $('#tarefa-conteudo').summernote('code', '');

    // Botoes Intervalo
    if (registro.intervaloInicio) {
        $('#btn-intervalo-pause').removeClass('botao-disabled');
        $('#btn-intervalo-play').addClass('botao-disabled');
    } else {
        $('#btn-intervalo-pause').addClass('botao-disabled');
        $('#btn-intervalo-play').removeClass('botao-disabled');
    }

    comentarioAppService.getUsuarioLogadoAsync().done(function (data) {
        //     debugger
        var usuarioLogadoId = data;
        var respId = $('#responsavel-tarefa-id').val();
        if (respId == usuarioLogadoId) {
            $('#btn-intervalo-pause').show();
            $('#btn-intervalo-play').show();
        }
        else {
            $('#btn-intervalo-pause').hide();
            $('#btn-intervalo-play').hide();
        }

        abp.ui.clearBusy();
    });

    tarefaAppService.calcularTempoDecorrido(registro.id).done(function (data) {
        //    //console.log(JSON.stringify(data));//temp
        $('#tempo-decorrido').html(data);
    });

    carregarComentarios(registro.id);

    $('#sw-form-retratil-cabecalho-tarefas').html('<b>Editando</b>');

    //var myVar = setInterval(function () {
    //    tarefaAppService.calcularTempoDecorrido(registro.id)
    //              .done(function (data) {
    //                  //console.log(JSON.stringify(data));//temp
    //                  $('#tempo-decorrido').html(data);
    //              });
    //}, 1000);

    // Elementos botoes labels etc
    //$('#div-form').removeClass('contorno-placebo');
    //$('#div-form').addClass('contornado');
    //$('#icone-btn-salvar').removeClass('fa fa-plus');
    //$('#icone-btn-salvar').addClass('glyphicon glyphicon-edit');
    //$('#titulo-config').html('@L("Editando")');
    //   $('#cabec-config').addClass('titulo-azul');
    //$('#btn-apagar-config').fadeIn();


}

function resetFormRetratilTarefas() {
    $('#tarefa-selecionada-id').val('');
    $('#tarefa-codigo').val('');
    $('#tarefa-ordem').val('');
    $('#tarefa-id-form').val('');
    $('#tarefa-descricao').val('');
    $('#tarefa-prioridade').val('');
    $("input[name='DataPrevistaInicio']").val(moment(new Date()).format("L"));
    $("input[name='DataPrevistaTermino']").val('');
    $("input[name='DataInicio']").val('');
    $("input[name='DataTermino']").val('');
    $("input[name='DataRegistro']").val(moment(new Date()).format("L"));
    resetCboTarefaProj();
    resetCboTarefaMod();
    resetCboTarefaStatus();
    resetCboTarefaPrioridade();
    resetCboTarefaResp();
    $('#tarefa-conteudo').summernote('code', '');
    //  resetCboTarefaCliente();

    $('#btn-intervalo-pause').addClass('botao-disabled');
    $('#btn-intervalo-play').addClass('botao-disabled');

    $('#tempo-decorrido').html('');
    var containerComentarios1 = $('#container-comentarios');
    containerComentarios1.html('');

    $('#sw-form-retratil-cabecalho-tarefas').html('<b>Nova</b>');
};

function getTarefas(reload) {
    ////console.log("getTarefas");
    if (reload) {
        tarefasTable.jtable('reload');
        ////console.log("sim");
    } else {

        tarefasTable.jtable('load', {
            responsavelId: $('#cbo-tarefa-filtro-resp').val(),
            statusId: $('#cbo-tarefa-filtro-status').val(),
            moduloId: $('#cbo-tarefa-filtro-modulo').val(),
            prioridadeId: $('#cbo-tarefa-filtro-prior').val(),
            startDate: _selectedDateRange.startDate,
            endDate: _selectedDateRange.endDate,
            sorting: "Ordem",
            filtro: $('#filtro').val(),
            tipoData: $('#cbo-tarefa-filtro-tipoData').val(),
            tarefaId: $('#tarefa-id').val(),
            IsMostrarGlobal: $('#mostrar-global').is(':checked')
        });

        var tipoData = $('#cbo-tarefa-filtro-tipoData').val();
        if (tipoData != null && tipoData != "") {
            $('#tarefa-filtro-data').show("slow");
        } else {
            $('#tarefa-filtro-data').hide("slow");
        }

        //if ($('#filtrar-data').is(':checked')) {
        //    $('#tarefa-filtro-data').show("slow");
        //} else {
        //    $('#tarefa-filtro-data').hide("slow");
        //}
        ////console.log("não");
    }
}

function salvarTarefa() {
    ////console.log("DataPrevistaInicio; ", $("input[name='DataPrevistaInicio']").val());
    var erro = 0;
    if ($("input[name='DataPrevistaInicio']").val() == "") {
        $("input[name='DataPrevistaInicio']").css("border-color", "red");

        $('#msgErro').show();
        //alert("Data previsão de inicio obrigatório !") msgErro
        ////console.log("return");
        abp.notify.info(app.localize('Campos obrigatórios'));
        return
    } else {
        $('#msgErro').hide();
        $("input[name='DataPrevistaInicio']").css("border-color", "#c2cad8");
    }


    if ($('#cbo-tarefa-resp').val() == "") {
        $('#cbo-tarefa-resp').addClass("requered");

        $('#msgErroRespo').show();
        //alert("Data previsão de inicio obrigatório !") msgErro
        ////console.log("return");
        abp.notify.info(app.localize('Campos obrigatórios'));
        return
    } else {
        $('#msgErroRespo').hide();
        $('#cbo-tarefa-resp').removeClass("requered");

    }

    if ($('#cbo-tarefa-status').val() == "") {
        alert("Por favor preencher Status !");
        abp.notify.info(app.localize('Campos obrigatórios'));
        return
    }


    //debugger
    var tarefaDto = {
        Codigo: $('#tarefa-codigo').val(),
        Descricao: $('#tarefa-descricao').val(),
        Ordem: $('#tarefa-ordem').val(),
        Id: $('#tarefa-id-form').val(),
        PrioridadeId: $('#cbo-tarefa-prioridade').val(),
        ProjetoId: $('#cbo-tarefa-projeto').val(),
        DataRegistro: $("input[name='DataRegistro']").val(),
        DataPrevistaInicio: $("input[name='DataPrevistaInicio']").val(),
        DataPrevistaTermino: $("input[name='DataPrevistaTermino']").val(),
        DataInicio: $("input[name='DataInicio']").val(),
        DataTermino: $("input[name='DataTermino']").val(),
        ResponsavelId: $('#cbo-tarefa-resp').val(),
        CriadorId: $('#usuario-logado-id').val(),
        ModuloId: $('#cbo-tarefa-modulo').val(),
        StatusId: $('#cbo-tarefa-status').val(),
        ClienteId: $('#cbo-tarefa-cliente').val(),
        Conteudo: $('#tarefa-conteudo').summernote('code')
    }
    // 

    //console.log("pegando tarefaDto da tela : ", tarefaDto);

    var itemsSelecionados = tarefasTable.jtable('selectedRows');
    if (itemsSelecionados.length > 0) {
        //console.log("não");
        itemsSelecionados.each(function () {
            var record = $(this).data('record');
            abp.services.app.tarefa.obter(record.id)
           .done(function (itemObtido) {
               itemObtido.Codigo = $('#tarefa-codigo').val();
               itemObtido.Descricao = $('#tarefa-descricao').val();
               itemObtido.Ordem = $('#tarefa-ordem').val();
               itemObtido.Id = $('#tarefa-id-form').val();
               itemObtido.PrioridadeId = $('#cbo-tarefa-prioridade').val();
               itemObtido.ProjetoId = $('#cbo-tarefa-projeto').val();
               itemObtido.DataRegistro = $("input[name='DataRegistro']").val();
               itemObtido.DataPrevistaInicio = $("input[name='DataPrevistaInicio']").val();
               itemObtido.DataPrevistaTermino = $("input[name='DataPrevistaTermino']").val();
               itemObtido.DataInicio = $("input[name='DataInicio']").val();
               itemObtido.DataTermino = $("input[name='DataTermino']").val();
               itemObtido.ResponsavelId = $('#cbo-tarefa-resp').val();
               itemObtido.CriadorId = $('#usuario-logado-id').val();
               itemObtido.ModuloId = $('#cbo-tarefa-modulo').val();
               itemObtido.StatusId = $('#cbo-tarefa-status').val();
               //itemObtido.ClienteId = $('#cbo-tarefa-cliente').val();
               itemObtido.Conteudo = $('#tarefa-conteudo').summernote('code');

               ////console.log($('#usuario-logado-id').val());

               abp.services.app.tarefa.criarOuEditar(itemObtido)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     tarefasTable.find('.jtable-row-selected').click();
                     getTarefas();

                     //console.log("Edição:", itemObtido);

                     //servidor
                     if (tarefaDto.PrioridadeId == 26) {
                         enviarEmail(itemObtido);
                     }

                     //local
                     ////console.log("inclusão:", itemObtido.PrioridadeId);
                     //if (itemObtido.PrioridadeId == 16) {
                     //    enviarEmail(itemObtido);
                     //}
                 });
           });

        });
    }
    else {
        //console.log("sim");
        //console.log("inserindo tarefaDto : ", tarefaDto);
        // //console.log($('#usuario-logado-id').val());

        abp.services.app.tarefa.criarOuEditar(tarefaDto)
          .done(function () {
              abp.notify.info(app.localize('SavedSuccessfully'));
              getTarefas();
              //var jTable = $('#" + jTableId + "'); jTable.find('.jtable-row-selected').click();
              resetFormRetratilTarefas();

              ////console.log("inclusão:", tarefaDto);

              //servidor
              if (tarefaDto.PrioridadeId == 26) {
                  enviarEmail(tarefaDto);
              }

              //local
              ////console.log("inclusão:", tarefaDto.PrioridadeId);
              //if (tarefaDto.PrioridadeId == 16) {
              //    enviarEmail(tarefaDto);
              //}
          });
    }

    $('#omitir-form-retratil-tarefas').click();
}

function deleteTarefa(tarefa) {
    abp.message.confirm(
        app.localize('DeleteWarning', tarefa.descricao),
        function (isConfirmed) {
            if (isConfirmed) {
                tarefaAppService.excluir(tarefa)
                    .done(function () {
                        getTarefas(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        $('#omitir-form-retratil-tarefas').click();
                    });
            }
        }
    );
}


function enviarEmail(item) {
    ////console.log("itemObtido: ", item);
    abp.message.confirm(
    'Um e-mail sera enviado para o responsável.',
    'Enviar e-mail?',
    function (isConfirmed) {
        if (isConfirmed) {

            var _url = '/Mpa/Projetos/EnviarEmail';
            $.ajax({
                type: "POST",
                url: _url,
                data: {
                    ResponsavelId: item.ResponsavelId,
                    Descricao: item.Descricao,
                    DataPrevistaInicio: item.DataPrevistaInicio
                },
                success: function (result) {
                },

                error: function (xhr, ajaxOptions, thrownError) {
                },
                complete: function () { }
            });

        }
    }
);

    //abp.message.confirm(
    //    app.localize('DeleteWarning', itemObtido.descricao),
    //    function (isConfirmed) {
    //        if (isConfirmed) {
    //            projetoAppService.excluir(projeto)
    //                .done(function () {
    //                    getProjetos(true);
    //                    abp.notify.success(app.localize('SuccessfullyDeleted'));
    //                    $('#omitir-form-retratil-projetos').click();
    //                });
    //        }
    //    }
    //);
}
//fim--functions-----------------------------

//controle datas--------------------------------
$('input[name="DataInicio"]').daterangepicker({
    "singleDatePicker": true,
    "showDropdowns": true,
    autoUpdateInput: false,
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
    $('input[name="DataInicio"]').val(selDate.format('L')).addClass('form-control edited');
});

$('input[name="DataTermino"]').daterangepicker({
    "singleDatePicker": true,
    "showDropdowns": true,
    autoUpdateInput: false,
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
    $('input[name="DataTermino"]').val(selDate.format('L')).addClass('form-control edited');
});

$('input[name="DataPrevistaInicio"]').daterangepicker({
    "singleDatePicker": true,
    "showDropdowns": true,
    autoUpdateInput: false,
    changeYear: true,
    yearRange: 'c-10:c+10',
    showOn: "both",
    selecting: new Date,
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
    $('input[name="DataPrevistaInicio"]').val(selDate.format('L')).addClass('form-control edited');
});

$('input[name="DataPrevistaTermino"]').daterangepicker({
    "singleDatePicker": true,
    "showDropdowns": true,
    autoUpdateInput: false,
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
    $('input[name="DataPrevistaTermino"]').val(selDate.format('L')).addClass('form-control edited');
});

$('input[name="DataRegistro"]').daterangepicker({
    "singleDatePicker": true,
    "showDropdowns": true,
    autoUpdateInput: false,
    maxDate: 2080 / 01 / 01,
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
    $('input[name="DataRegistro"]').val(selDate.format('L')).addClass('form-control edited');
});
// FIM - TAREFAS=================================================================================


// COMENTARIOS ==================================================================================

$('#comentario-conteudo').summernote({
    height: 40,
    minHeight: 30,
    toolbar: [
    // [groupName, [list of button]]
    ['style', ['bold', 'underline', 'clear']],
    ['mais', ['picture', 'link']]
    ]
});

$('#btn-comentar').on('click', function (e) {
    var tarefaSelecionadaId = $('#tarefa-selecionada-id').val();
    //debugger
    if (tarefaSelecionadaId) {
        comentar();
    } else {
        abp.notify.warn(app.localize('TarefaAindaNaoFoiRegistrada'));
    }
});

$("#container-comentarios").resizable();

//$(function () {
//    $("#container-comentarios").resizable();
//});

function comentar() {
    //   
    comentarioAppService.getUsuarioLogadoAsync()
        .done(function (data) {
            //    //console.log(JSON.stringify(data));
            var novoComentario = {
                UsuarioId: data,
                TarefaId: $('#tarefa-selecionada-id').val(),
                DataRegistro: new Date(),
                Conteudo: $('#comentario-conteudo').summernote('code')
            };

            comentarioAppService.criarOuEditar(novoComentario)
                .done(function (data) {
                    $('#comentario-conteudo').summernote('code', '');
                    abp.notify.info(app.localize('SavedSuccessfully'));
                });
        });
}

function carregarComentarios(tarefaId) {

    var containerComentarios1 = $('#container-comentarios');
    containerComentarios1.html('');

    function apenderComentarioLocal(item, index) {
        //    //console.log(item);
        var usuario = item.nomeUsuario;
        var dataRegistro = moment(item.dataRegistro).format('llll');
        var conteudo = item.conteudo;
        var containerComentarios = $('#container-comentarios');

        containerComentarios.prepend('<div class="col-sm-12" style="border:1px solid #c2cad8; padding:10px;"><div class="row"><div class="col-sm-1"><span>' + usuario + '</span></div><div class="col-sm-10"><div style="display:inline-block;">' + conteudo + '</div></div><div class="col-sm-1"><span>' + dataRegistro + '</span></div></div></div>');
    }

    comentarioAppService.listarPorTarefa(tarefaId)
                 .done(function (data) {
                     data.items.forEach(apenderComentarioLocal);
                 });
}

function apenderComentario(novoComentario) {
    //    //console.log(item);

    var usuario = novoComentario.nomeUsuario;
    var dataRegistro = moment(novoComentario.dataRegistro).format('llll');
    var conteudo = novoComentario.conteudo;
    var containerComentarios = $('#container-comentarios');
    containerComentarios.prepend('<div class="col-sm-12" style="border:1px solid #c2cad8; padding:10px;"><div class="row"><div class="col-sm-1"><span>' + usuario + '</span></div><div class="col-sm-10"><div style="display:inline-block;">' + conteudo + '</div></div><div class="col-sm-1"><span>' + dataRegistro + '</span></div></div></div>');
}

// FIM - COMENTARIOS==========================================================================


// PROJETOS ==================================================================================

projetosTable.jtable({

    title: app.localize('Projetos'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    selectingCheckboxes: true,
    actions: {
        listAction: {
            method: projetoAppService.listarTodos
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
                //  if (_permissions.edit) {
                //$('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //    .appendTo($span)
                //    .click(function () {
                //        //  _createOrEditModal.open({ id: data.record.id });
                //    });
                //     }

                //     if (_permissions.delete) {
                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                    .appendTo($span)
                    .click(function () {
                        deleteProjeto(data.record);
                    });
                //     }

                return $span;
            }
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '7%',
            display: function (data) {
                return data.record.descricao;
            }
        }
        ,
        nivel1: {
            title: app.localize('Nivel1'),
            width: '12%'
        }
        ,
        nivel2: {
            title: app.localize('Nivel2'),
            width: '12%'
        }
        ,
        nivel3: {
            title: app.localize('Nivel3'),
            width: '12%'
        }
        ,
        dataCriacao: {
            title: app.localize('DataCriacao'),
            width: '20%',
            display: function (data) {
                return moment(data.record.dataCriacao).format('L');
            }
        }
    },

    selectionChanged: function () {
        var projetosSelecionados = projetosTable.jtable('selectedRows');

        // temp - para abrir formRetratil
        $('#exibir-form-retratil-projetos').click();

        if (projetosSelecionados.length > 0) {
            projetosSelecionados.each(function () {
                var registro = $(this).data('record');
                setFormRetratilProjetos(registro);
            });
        } else {
            resetFormRetratilProjetos();
            $('#omitir-form-retratil-projetos').click();
        }
    }
});

// Setar e Resetar FormRetratil Projetos
function setFormRetratilProjetos(registro) {
    registro.codigo && $('#projeto-codigo').val(registro.codigo) || $('#projeto-codigo').val('');
    registro.descricao && $('#projeto-descricao').val(registro.descricao) || $('#projeto-descricao').val('');
    registro.nivel1 && $('#projeto-nivel1').val(registro.nivel1) || $('#projeto-nivel1').val('');
    registro.nivel2 && $('#projeto-nivel2').val(registro.nivel2) || $('#projeto-nivel2').val('');
    registro.nivel3 && $('#projeto-nivel3').val(registro.nivel3) || $('#projeto-nivel3').val('');
    registro.dataCriacao && $("input[name='DataCriacao']").val(moment(registro.dataCriacao).format('L')) || $("input[name='DataCriacao']").val(moment(new Date()).format('L'));
    //setCboProjetoProj(registro.projetoId);
}

function resetFormRetratilProjetos() {
    $('#projeto-codigo').val('');
    $('#projeto-descricao').val('');
    $('#projeto-nivel1').val('');
    $('#projeto-nivel2').val('');
    $('#projeto-nivel3').val('');
    //resetCboProjetoProj();
    $("input[name='DataCriacao']").val(moment(new Date()).format('L'));
};

// (Re)Carregar JTable
function getProjetos(reload) {
    if (reload) {
        projetosTable.jtable('reload');
    } else {
        projetosTable.jtable('load', {
            filtro: $('#projetosfiltro-tabela').val()
        });
    }
}

$('#btn-get-projetos').click(function (e) {
    e.preventDefault();
    getProjetos();
});
getProjetos();

// Salvar Projeto
function salvarProjeto() {
    var projetoDto = {
        Codigo: $('#projeto-codigo').val(),
        Descricao: $('#projeto-descricao').val(),
        Nivel1: $('#projeto-nivel1').val(),
        Nivel2: $('#projeto-nivel2').val(),
        Nivel3: $('#projeto-nivel3').val(),
        DataCriacao: $("input[name='DataCriacao']").val()
    }
    //   
    var projsSelecionados = projetosTable.jtable('selectedRows');
    if (projsSelecionados.length > 0) {
        projsSelecionados.each(function () {
            var record = $(this).data('record');

            abp.services.app.projeto.obter(record.id)
           .done(function (projetoObtido) {
               projetoObtido.Codigo = $('#projeto-codigo').val();
               projetoObtido.Descricao = $('#projeto-descricao').val();
               projetoObtido.Nivel1 = $('#projeto-nivel1').val();
               projetoObtido.Nivel2 = $('#projeto-nivel2').val();
               projetoObtido.Nivel3 = $('#projeto-nivel3').val();
               projetoObtido.DataCriacao = $("input[name='DataCriacao']").val();
               projetoObtido.ProjetoId = $('#cbo-projeto-proj').val();

               abp.services.app.projeto.criarOuEditar(projetoObtido)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     projetosTable.find('.jtable-row-selected').click();
                     getProjetos();
                     //resetarForm();
                 });
           });

        });

    } else {
        //
        abp.services.app.projeto.criarOuEditar(projetoDto)
          .done(function () {
              abp.notify.info(app.localize('SavedSuccessfully'));
              getProjetos();
              //resetarForm();
          });
    }
    $('#omitir-form-retratil-projetos').click();

}
// Apagar Projeto
function deleteProjeto(projeto) {
    abp.message.confirm(
        app.localize('DeleteWarning', projeto.descricao),
        function (isConfirmed) {
            if (isConfirmed) {
                projetoAppService.excluir(projeto)
                    .done(function () {
                        getProjetos(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        $('#omitir-form-retratil-projetos').click();
                    });
            }
        }
    );
}

// FIM - PROJETOS


// MODULOS ==================================================================================

modulosTable.jtable({

    title: app.localize('Modulos'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    selectingCheckboxes: true,
    actions: {
        listAction: {
            method: docRotuloAppService.listarModulos
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
                // if (_permissions.edit) {
                //$('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //    .appendTo($span)
                //    .click(function () {
                //        //  _createOrEditModal.open({ id: data.record.id });
                //    });
                // }

                //  if (_permissions.delete) {
                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                    .appendTo($span)
                    .click(function () {
                        deleteModulo(data.record);
                    });
                // }

                return $span;
            }
        }
        ,
        codigo: {
            title: app.localize('Codigo'),
            width: '7%'
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '25%'
        }

    },

    selectionChanged: function () {
        var modulosSelecionados = modulosTable.jtable('selectedRows');

        // temp - para abrir formRetratil  
        $('#exibir-form-retratil-modulos').click();
        if (modulosSelecionados.length > 0) {
            modulosSelecionados.each(function () {
                var registro = $(this).data('record');
                setFormRetratilModulos(registro);
            });
        } else {
            resetFormRetratilModulos();
            $('#omitir-form-retratil-modulos').click();
        }
    }
});

// Setar e Resetar FormRetratil Modulos
function setFormRetratilModulos(registro) {
    ////console.log("setFormRetratilModulos: ", registro);
    registro.codigo && $('#modulo-codigo').val(registro.codigo) || $('#modulo-codigo').val('');
    registro.descricao && $('#modulo-descricao').val(registro.descricao) || $('#modulo-descricao').val('');
    registro.titulo && $('#modulo-titulo').val(registro.titulo) || $('#modulo-titulo').val('');
    registro.ordem && $('#modulo-ordem').val(registro.ordem) || $('#modulo-ordem').val('');
}

function resetFormRetratilModulos() {
    $('#modulo-codigo').val('');
    $('#modulo-descricao').val('');
    $('#modulo-titulo').val('');
    $('#modulo-ordem').val('');
};

// (Re)Carregar JTable
function getModulos(reload) {
    if (reload) {
        modulosTable.jtable('reload');
    } else {
        modulosTable.jtable('load', {
            filtro: $('#modulosfiltro-tabela').val()
        });
    }
}

$('#btn-get-modulos').click(function (e) {
    e.preventDefault();
    getModulos();
});

getModulos();

// Salvar Modulo
function salvarModulo() {
    //  
    var moduloituloDto = {
        Codigo: $('#modulo-codigo').val(),
        Titulo: $('#modulo-titulo').val(),
        Descricao: $('#modulo-descricao').val(),
        Ordem: $('#modulo-ordem').val(),
        IsModulo: true
    }
    //   
    var modsSelecionados = modulosTable.jtable('selectedRows');
    if (modsSelecionados.length > 0) {
        modsSelecionados.each(function () {
            var record = $(this).data('record');

            abp.services.app.docRotulo.obter(record.id)
           .done(function (moduloObtido) {
               moduloObtido.Codigo = $('#modulo-codigo').val();
               moduloObtido.Titulo = $('#modulo-titulo').val();
               moduloObtido.Descricao = $('#modulo-descricao').val();
               moduloObtido.Ordem = $('#modulo-ordem').val();
               moduloObtido.IsModulo = true;

               abp.services.app.docRotulo.criarOuEditar(moduloObtido)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     getModulos();
                     //resetarForm();
                 });
           });

        });

    } else {
        //
        abp.services.app.docRotulo.criarOuEditar(moduloituloDto)
          .done(function () {
              abp.notify.info(app.localize('SavedSuccessfully'));
              getModulos();
              //resetarForm();
          });
    }

    $('#omitir-form-retratil-modulos').click();
}
// Apagar Modulo
function deleteModulo(modulo) {
    //   
    abp.message.confirm(
        app.localize('DeleteWarning', modulo.descricao),
        function (isConfirmed) {
            if (isConfirmed) {
                docRotuloAppService.excluir(modulo)
                    .done(function () {
                        getModulos(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        $('#omitir-form-retratil-modulos').click();
                    });
            }
        }
    );
}

// FIM - MODULOS


// PRIORIDADE ==================================================================================

prioridadesTable.jtable({

    title: app.localize('Prioridade'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    selectingCheckboxes: true,

    actions: {
        listAction: {
            method: docRotuloAppService.listarPrioridades
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
                //if (_permissions.edit) {
                //$('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //    .appendTo($span)
                //    .click(function () {
                //        //  _createOrEditModal.open({ id: data.record.id });
                //    });
                //}

                //  if (_permissions.delete) {
                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                    .appendTo($span)
                    .click(function () {
                        deletePrioridade(data.record);
                    });
                // }

                return $span;
            }
        }
        ,
        //codigo: {
        //    title: app.localize('Codigo'),
        //    width: '7%'
        //}
        //,
        descricao: {
            title: app.localize('Descricao'),
            width: '25%'
        }

    },

    selectionChanged: function () {
        var prioridadesSelecionadas = prioridadesTable.jtable('selectedRows');

        // temp - para abrir formRetratil
        $('#exibir-form-retratil-prioridade').click();
        if (prioridadesSelecionadas.length > 0) {
            prioridadesSelecionadas.each(function () {
                var registro = $(this).data('record');
                setFormRetratilPrioridade(registro);
            });
        } else {
            resetFormRetratilPrioridade();
            $('#omitir-form-retratil-prioridade').click();
        }
    }
});

// Setar e Resetar FormRetratil Prioridade
function setFormRetratilPrioridade(registro) {
    ////console.log("registro: ", registro)
    registro.codigo && $('#prioridade-codigo').val(registro.codigo) || $('#prioridade-codigo').val('');
    registro.descricao && $('#prioridade-descricao').val(registro.descricao) || $('#prioridade-descricao').val('');
    registro.titulo && $('#prioridade-titulo').val(registro.titulo) || $('#prioridade-titulo').val('');
    registro.ordem && $('#prioridade-ordem').val(registro.ordem) || $('#prioridade-ordem').val('');
}

function resetFormRetratilPrioridade() {
    $('#prioridade-codigo').val('');
    $('#prioridade-descricao').val('');
    $('#prioridade-titulo').val('');
    $('#prioridade-ordem').val('');
};

// (Re)Carregar JTable
function getPrioridades(reload) {
    if (reload) {
        prioridadesTable.jtable('reload');
    } else {
        prioridadesTable.jtable('load', {
            filtro: $('#prioridadefiltro-tabela').val()
        });
    }
}

$('#btn-get-prioridade').click(function (e) {
    e.preventDefault();
    getPrioridades();
});

getPrioridades();

// Salvar Status
function salvarPrioridade() {
    //  
    var prioridadeDto = {
        Codigo: $('#prioridade-codigo').val(),
        Titulo: $('#prioridade-titulo').val(),
        Descricao: $('#prioridade-descricao').val(),
        Ordem: $('#prioridade-ordem').val(),
        IsPrioridade: true
    }
    //   
    var prioridadesSelecionadas = prioridadesTable.jtable('selectedRows');
    if (prioridadesSelecionadas.length > 0) {
        prioridadesSelecionadas.each(function () {
            var record = $(this).data('record');

            abp.services.app.docRotulo.obter(record.id)
           .done(function (prioridadeObtido) {
               prioridadeObtido.Codigo = $('#prioridade-codigo').val();
               prioridadeObtido.Titulo = $('#prioridade-titulo').val();
               prioridadeObtido.Descricao = $('#prioridade-descricao').val();
               prioridadeObtido.Ordem = $('#prioridade-ordem').val();
               prioridadeObtido.IsPrioridade = true;

               abp.services.app.docRotulo.criarOuEditar(prioridadeObtido)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     getPrioridades();
                     //resetarForm();
                 });
           });

        });

    } else {
        //
        abp.services.app.docRotulo.criarOuEditar(prioridadeDto)
          .done(function () {
              abp.notify.info(app.localize('SavedSuccessfully'));
              getPrioridades();
          });
    }
    $('#omitir-form-retratil-prioridade').click();

}
// Apagar Status
function deletePrioridade(prioridade) {
    //   
    abp.message.confirm(
        app.localize('DeleteWarning', prioridade.descricao),
        function (isConfirmed) {
            if (isConfirmed) {
                docRotuloAppService.excluir(prioridade)
                    .done(function () {
                        getPrioridades(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        $('#omitir-form-retratil-prioridade').click();
                    });
            }
        }
    );
}

// FIM - PRIORIDADE


// STATUS ==================================================================================

statusTable.jtable({

    title: app.localize('Status'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    selectingCheckboxes: true,

    actions: {
        listAction: {
            method: docRotuloAppService.listarStatus
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
                // if (_permissions.edit) {
                //$('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                //    .appendTo($span)
                //    .click(function () {
                //        //  _createOrEditModal.open({ id: data.record.id });
                //    });
                // }

                //  if (_permissions.delete) {
                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                    .appendTo($span)
                    .click(function () {
                        deleteStatus(data.record);
                    });
                // }

                return $span;
            }
        }
        ,
        codigo: {
            title: app.localize('Codigo'),
            width: '7%'
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '25%'
        }
         ,
        cor: {
            title: app.localize('Cor'),
            width: '8%',
            display: function (data) {
                var cor = data.record.cor;
                return '<div style="text-align:center;">   <span style="display:inline-block; width:20px; height:20px;  text-align:center; background-color: ' + cor + '; border-radius: 25px;">  </span>  </div>  ';
            }

        }
        ,
        isMostrarGlobal: {
            title: app.localize('Mostrar'),
            width: '25%',
            display: function (data) {
                var teste;
                var label;
                if (!data.record.isMostrarGlobal) {
                    teste = "Yes";
                    label = "success";
                } else {
                    teste = "No";
                    label = "default";
                }
                return '<div style="text-align:center;">' + '<span class="label label-' + label + ' content-center text-center">' + app.localize(teste) + '</span>' + '</div>';
            }
        }
    },

    selectionChanged: function () {
        var prioridadesSelecionadas = statusTable.jtable('selectedRows');

        // temp - para abrir formRetratil
        $('#exibir-form-retratil-status').click();


        if (prioridadesSelecionadas.length > 0) {
            prioridadesSelecionadas.each(function () {
                var registro = $(this).data('record');
                setFormRetratilStatus(registro);
            });
        } else {
            resetFormRetratilStatus();
            $('#omitir-form-retratil-status').click();
        }
    }
});

// Setar e Resetar FormRetratil Status
function setFormRetratilStatus(registro) {
    registro.codigo && $('#status-codigo').val(registro.codigo) || $('#status-codigo').val('');
    registro.descricao && $('#status-descricao').val(registro.descricao) || $('#status-descricao').val('');
    registro.titulo && $('#status-titulo').val(registro.titulo) || $('#status-titulo').val('');
    registro.ordem && $('#status-ordem').val(registro.ordem) || $('#status-ordem').val('');
    registro.cor && $('#status-cor').val(registro.cor) || $('#status-cor').val('');
    registro.isMostrarGlobal && $('#mostrar-global').attr("checked", true) || $('#mostrar-global').attr("checked", false);
}

function resetFormRetratilStatus() {
    $('#status-codigo').val('');
    $('#status-descricao').val('');
    $('#status-titulo').val('');
    $('#status-ordem').val('');
    $('#status-cor').val('');
    $('#mostrar-global').attr("checked", false);
};

// (Re)Carregar JTable
function getStatus(reload) {
    if (reload) {
        statusTable.jtable('reload');
    } else {
        statusTable.jtable('load', {
            filtro: $('#statusfiltro-tabela').val()
        });
    }
}

$('#btn-get-status').click(function (e) {
    e.preventDefault();
    getStatus();
});
getStatus();

// Salvar Status
function salvarStatus() {
    // 
    var statusDto = {
        Codigo: $('#status-codigo').val(),
        Titulo: $('#status-titulo').val(),
        Descricao: $('#status-descricao').val(),
        IsMostrarGlobal: $('#mostrar-global').is(':checked'),
        Ordem: $('#status-ordem').val(),
        Cor: $('#status-cor').val(),
        IsStatus: true
    }
    //   
    var prioridadesSelecionadas = statusTable.jtable('selectedRows');
    if (prioridadesSelecionadas.length > 0) {
        prioridadesSelecionadas.each(function () {
            var record = $(this).data('record');

            abp.services.app.docRotulo.obter(record.id)
           .done(function (statusObtido) {
               statusObtido.Codigo = $('#status-codigo').val();
               statusObtido.Titulo = $('#status-titulo').val();
               statusObtido.Descricao = $('#status-descricao').val();
               statusObtido.IsMostrarGlobal = $('#mostrar-global').is(':checked');
               statusObtido.Ordem = $('#status-ordem').val();
               statusObtido.Cor = $('#status-cor').val();
               statusObtido.IsStatus = true;

               //console.log("statusObtido: ", statusObtido);

               abp.services.app.docRotulo.criarOuEditar(statusObtido)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     getStatus();
                     //resetarForm();
                 });
           });

        });

    } else {
        //
        //console.log("statusDto: ", statusDto);
        abp.services.app.docRotulo.criarOuEditar(statusDto)
          .done(function () {
              abp.notify.info(app.localize('SavedSuccessfully'));
              getStatus();
          });
    }
    $('#omitir-form-retratil-status').click();
}
// Apagar Status
function deleteStatus(status) {
    //   
    abp.message.confirm(
        app.localize('DeleteWarning', status.descricao),
        function (isConfirmed) {
            if (isConfirmed) {
                docRotuloAppService.excluir(status)
                    .done(function () {
                        getStatus(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                        $('#omitir-form-retratil-status').click();
                    });
            }
        }
    );
}

$('#mostrar-global').on("change", function () {


    //console.log($('#mostrar-global').is(':checked'));
    //var teste = $('#mostrar-global').val();
    ////console.log("mostrar-global:", teste);
    //if (teste = false) {
    //    $('#mostrar-global').val(true);
    //} else {
    //    $('#mostrar-global').val(false);
    //}

    ////console.log("mostrar-global:", $(this).val());
});

// FIM - STATUS


// TAREFA-INTERVALOS ==================================================================================

// Iniciar contagem
$('#btn-intervalo-play').click(function (e) {
    e.preventDefault();
    var itemsSelecionados = tarefasTable.jtable('selectedRows');
    if (itemsSelecionados.length > 0) {
        itemsSelecionados.each(function () {
            var record = $(this).data('record');

            abp.message.confirm(
                app.localize('IniciarContagemTarefaWarning', record.descricao),
                function (isConfirmed) {
                    if (isConfirmed) {
                        // AppService
                        tarefaAppService.iniciarContagemIntervalo(record.id)
                          .done(function (data) {
                              //S //console.log(JSON.stringify(data));//temp
                              if (data) {
                                  abp.notify.info(app.localize('Contagem iniciada.'));
                                  $('#btn-intervalo-pause').removeClass('botao-disabled');
                                  $('#btn-intervalo-play').addClass('botao-disabled');
                              } else {
                                  abp.notify.info(app.localize('Contagem já está sendo realizada.'));
                                  $('#btn-intervalo-pause').removeClass('botao-disabled');
                                  $('#btn-intervalo-play').addClass('botao-disabled');
                              }
                              // getTarefasExecutando();
                              smweHubWeb.server.getTarefasExecutando();
                          });
                        // fim AppService
                    }
                }
            );


        });
    }
});

// Interromper contagem
$('#btn-intervalo-pause').click(function (e) {
    e.preventDefault();

    var itemsSelecionados = tarefasTable.jtable('selectedRows');
    if (itemsSelecionados.length > 0) {
        itemsSelecionados.each(function () {
            var record = $(this).data('record');
            tarefaAppService.pararContagemIntervalo(record.id)
              .done(function (data) {
                  ////console.log(JSON.stringify(data));//temp
                  if (data) {
                      abp.notify.info(app.localize('Contagem interrompida.'));
                      $('#btn-intervalo-pause').addClass('botao-disabled');
                      $('#btn-intervalo-play').removeClass('botao-disabled');
                  } else {
                      abp.notify.info(app.localize('Contagem não está sendo realizada.'));
                      $('#btn-intervalo-pause').addClass('botao-disabled');
                      $('#btn-intervalo-play').removeClass('botao-disabled');
                  }
                  // getTarefasExecutando();
                  smweHubWeb.server.getTarefasExecutando();
              });
        });
    }
});

function interromperContagemIntervalo(tarefaId) {

    tarefaAppService.pararContagemIntervalo(tarefaId)
      .done(function (data) {
          //   //console.log(JSON.stringify(data));//temp
          if (data) {
              abp.notify.info(app.localize('Contagem interrompida.'));
              $('#btn-intervalo-pause').addClass('botao-disabled');
              $('#btn-intervalo-play').removeClass('botao-disabled');
          } else {
              abp.notify.info(app.localize('Contagem não está sendo realizada.'));
              $('#btn-intervalo-pause').addClass('botao-disabled');
              $('#btn-intervalo-play').removeClass('botao-disabled');
          }
          // getTarefasExecutando();
          smweHubWeb.server.getTarefasExecutando();
      });

}

// FIM - TAREFA-INTERVALOS



// EXECUCAO ==================================================================================

execucaoTable.jtable({
    title: app.localize('Tarefas'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    // Botoes da tabela
    actions: {
        listAction: {
            method: tarefaAppService.listarTarefasExecutando
        }
    },
    // Campos da tabels
    fields: {
        id: {
            key: true,
            list: false
        },
        actions: {
            title: app.localize('Actions'),
            width: '7%',
            sorting: false,
            display: function (data) {
                var $span = $('<span></span>');

                var usuarioLogadoId = $('#usuario-logado-id').val();
                //debugger
                if (usuarioLogadoId == data.record.responsavelId) {
                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Inicio') + '"><i class="fa fa-stop"></i></button>')
                        .appendTo($span)
                        .click(function (e) {
                            e.preventDefault();
                            interromperContagemIntervalo(data.record.id);
                        });
                }

                return $span;
            }
        }
        ,
        responsavelNome: {
            title: app.localize('Responsavel'),
            width: '7%'
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '15%'
        }
        ,
        inicio: {
            title: app.localize('Inicio'),
            width: '15%',
            display: function (data) {
                return moment(data.record.inicio).format('lll');
            }
        }
        ,
        tempoDecorrido: {
            title: app.localize('TempoDecorrido'),
            width: '15%'
        }
    },
    // Selecao de registro
    selectionChanged: function () {
        var tarefasSelecionados = execucaoTable.jtable('selectedRows');

        // temp - para abrir formRetratil
        //   $('#exibir-form-retratil-tarefas').click();

        if (tarefasSelecionados.length > 0) {
            tarefasSelecionados.each(function () {
                var registro = $(this).data('record');
                tarefaAppService.obter(registro.id)
                  .done(function (data) {
                      //        setFormRetratilExecucao(data);
                  });
            });
        } else {
            //resetFormRetratilExecucao();
        }
    }
});

// (Re)Carregar JTable Execucao
function getTarefasExecutando(reload) {
    if (reload) {
        execucaoTable.jtable('reload');
    } else {
        execucaoTable.jtable('load', {
            filtro: $('#execucaofiltro-tabela').val()
        });
    }
}
//$('#btn-get-execucao').click(function (e) {
//    e.preventDefault();
//    getTarefasExecutando();
//});
getTarefasExecutando();

//FIM - EXECUCAO



//HORAS REALIZADAS ==================================================================================

horasTable.jtable({
    title: app.localize('Tarefas'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    // Botoes da tabela
    actions: {
        listAction: {
            method: tarefaAppService.listarTarefasHorasRealizadas
        }
    },
    // Campos da tabels
    fields: {
        id: {
            key: true,
            list: false
        },
        //actions: {
        //    title: app.localize('Actions'),
        //    width: '7%',
        //    sorting: false,
        //    display: function (data) {
        //        var $span = $('<span></span>');
        //        //  if (_permissions.edit) {
        //        $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
        //            .appendTo($span)
        //            .click(function () {



        //            });
        //        //    }

        //        //    if (_permissions.delete) {
        //        $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
        //            .appendTo($span)
        //            .click(function () {
        //                deleteTarefa(data.record);
        //            });
        //        //     }

        //        return $span;
        //    }
        //}
        //,
        responsavelNome: {
            title: app.localize('Responsavel'),
            width: '7%'
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '15%'
        }
        ,
        inicio: {
            title: app.localize('Inicio'),
            width: '15%',
            display: function (data) {
                return moment(data.record.inicio).format('lll');
            }
        }
        ,
        fim: {
            title: app.localize('Fim'),
            width: '15%',
            display: function (data) {
                return moment(data.record.fim).format('lll');
            }
        }
        ,
        tempoDecorrido: {
            title: app.localize('TempoDecorrido'),
            width: '15%'
        }
    },
    // Selecao de registro
    selectionChanged: function () {
        var tarefasSelecionados = horasTable.jtable('selectedRows');

        // temp - para abrir formRetratil
        //   $('#exibir-form-retratil-tarefas').click();

        if (tarefasSelecionados.length > 0) {
            tarefasSelecionados.each(function () {
                var registro = $(this).data('record');
                tarefaAppService.obter(registro.id)
                  .done(function (data) {
                      //        setFormRetratilExecucao(data);
                  });
            });
        } else {
            //resetFormRetratilExecucao();
        }
    }
});

// (Re)Carregar JTable Execucao
function getTarefasHoras(reload) {
    if (reload) {
        horasTable.jtable('reload');
    } else {
        horasTable.jtable('load', {
            filtro: $('#cbo-horas-resp').val(),
            startDateNotNull: _selectedDateRange.startDate,
            endDateNotNull: _selectedDateRange.endDate
        });
    }
}

$('#cbo-horas-resp').on('change', function (e) {
    //getTarefasHoras();
});
// ao inves de usar ids, usar classe para filtros que causarem reload na jtable
$('#tarefa-filtro-daterange').on('change', function () {
    //   //console.log('selDateRange' + JSON.stringify(_selectedDateRange));
    //getTarefas();
});

$('#horas-range-picker').on('change', function () {
    //  //console.log('selDateRange' + JSON.stringify(_selectedDateRange));
    getTarefasHoras();
});

$('#btn-get-tarefas-horas').click(function (e) {
    e.preventDefault();
    getTarefasHoras();
});

// FIM - HORAS REALIZADAS


// MORRIS (graficos) ==================================================================================

abp.services.app.tarefa.calcularProducaoPorReponsavel()
    .done(function (data) {
        var _data = [];
        function popularDados(item, index) {
            var novoDado = {
                responsavel: item.key,
                horas: item.value.toFixed(2)
            };
            _data.push(novoDado);
        }

        data.dados.forEach(popularDados);

        new Morris.Bar({
            element: 'hist-producao-resp',
            data: _data,
            xkey: 'responsavel',
            ykeys: ['horas'],
            labels: ['Horas'],
            resize: true,
            barColors: function (row, series, type) {
                if (type === 'bar') {
                    var red = Math.ceil(255 * row.y / this.ymax);
                    var blue = Math.ceil(255 * row.y / this.ymax);
                    var green = Math.ceil(255 * row.y / this.ymax);
                    return 'rgb(0,' + green + ',' + blue + ')';
                }
                else {
                    return '#000';
                }
            }
        });

    });

// FIM - MORRIS



// ===============================================================================================================



// outras funcoes

function createRequestParams() {
    var prms = {};
    tarefasFiltroForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
    return $.extend(prms);
}

// (Re)Carregar JTable
//function getTarefas(reload) {
//    ////console.log("getTarefas");
//    if (reload) {
//        tarefasTable.jtable('reload');
//        ////console.log("sim");
//    } else {

//        tarefasTable.jtable('load', {
//            responsavelId: $('#cbo-tarefa-filtro-resp').val(),
//            statusId: $('#cbo-tarefa-filtro-status').val(),
//            moduloId: $('#cbo-tarefa-filtro-modulo').val(),
//            prioridadeId: $('#cbo-tarefa-filtro-prior').val(),
//            startDate: _selectedDateRange.startDate,
//            endDate: _selectedDateRange.endDate,
//            filtrarData: $('#filtrar-data').is(':checked'),
//            sorting: "Ordem",
//            filtro: $('#filtro').val()
//        });

//        if ($('#filtrar-data').is(':checked')) {
//            $('#tarefa-filtro-data').show("slow");
//        } else {
//            $('#tarefa-filtro-data').hide("slow");
//        }
//        ////console.log("não");
//    }
//}

// Filtro Tarefa Resp Setter
//function setFiltroTarefaResp(id) {
//    abp.services.app.WHAT.obter(id)
//        .done(function (data) {
//            var option = new Option(data.descricao || data.nomeFantasia || data.name, data.id, true, true);
//            var comboSel2 = $('#" + id + "');
//            comboSel2.append(option).trigger('change');
//            comboSel2.trigger({ type: 'select2:select', params: { data: data } });
//        });
//    }


/*
Mudanças feitas pelo marcus em 11/01/2018
*/
//$('#exibir-relatorio-projetos').on('click', function (e) {
//    e.preventDefault();
//    if ($('#frame-relatorio-projetos').attr('src') == undefined) {
//        $('#aba-dashboard').trigger('click');
//        $('#relatorio-projetos').removeClass('hidden');
//        refreshRelatorioProjetos();
//    }
//    else {
//        $('#relatorio-projetos').addClass('hidden');
//        $('#frame-relatorio-projetos').attr('src', null);
//    }
//});
//function refreshRelatorioProjetos() {
//    $('#frame-relatorio-projetos').attr('src', 'https://app.powerbi.com/view?r=eyJrIjoiNzA4NDBmNjUtYzBiOC00ZmE4LTk3OWMtY2M0ODg4NDAyM2E0IiwidCI6IjQ1OTQyMGFlLTJlNWYtNDljZS05MTNkLWU1MWNmOTYxM2JiMyJ9')
//}

//$('#exibir-bi-' + $('#bi-id').val()).on('click', function (e) {
//    e.preventDefault();
//    if ($('#show-bi-' + $('#bi-id').val()).hasClass('expand')) {
//        $('#show-bi-' + $('#bi-id').val()).trigger('click');
//    }
//    //refreshRelatorioProjetos();
//});
