
// DocItens
var docItensTable = $('#doc-itens-jtable');
var docItemAppService = abp.services.app.docItem;
var docItensFiltroForm = $('#doc-itens-filtro-form');
// DocCapitulos
var docCapsTable = $('#doc-caps-jtable');
var docRotuloAppService = abp.services.app.docRotulo;
var docCapsFiltroForm = $('#doc-caps-filtro-form');

// Permissoes -  A DEFINIR
var _permissions = {
    create: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.DocItens.Create'),
    edit: abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.DocItens.Edit'),
    'delete': abp.auth.hasPermission('Pages.Tenant.Cadastros.Faturamento.DocItens.Delete')
};

// DOC ITENS
docItensTable.jtable({
    title: app.localize('Itens'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,
    // Botoes da tabela
    actions: {
        listAction: {
            method: docItemAppService.listarTodos
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
            width: '8%',
            sorting: false,
            display: function (data) {
                var $span = $('<span></span>');
                //  if (_permissions.edit) {
                $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                    .appendTo($span)
                    .click(function () {



                    });
                //    }

                //    if (_permissions.delete) {
                $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                    .appendTo($span)
                    .click(function () {
                        deleteDocItens(data.record);
                    });
                //     }

                return $span;
            }
        }
        ,
        codigo: {
            title: app.localize('Codigo'),
            width: '5%'
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '20%'
        }
        ,
        titulo: {
            title: app.localize('Titulo'),
            width: '10%'
        }
        ,
        capitulo: {
            title: app.localize('Capitulo'),
            width: '4%'
        }
        ,
        ordem: {
            title: app.localize('Ordem'),
            width: '4%'
        }
        ,
        data: {
            title: app.localize('Data'),
            width: '10%',
            display: function (data) {
                return moment(data.record.dataPublicacao).format('L');
            }
        }
        ,
        versao: {
            title: app.localize('Versao'),
            width: '6%'
        }
    },
    // Selecao de registro
    selectionChanged: function () {
        var docItensSelecionados = docItensTable.jtable('selectedRows');

        // temp - para abrir formRetratil
        $('#exibir-form-retratil-doc-itens').click();

        if (docItensSelecionados.length > 0) {
            docItensSelecionados.each(function () {
                var registro = $(this).data('record');
                setFormRetratilDocItens(registro);
            });
        } else {
            resetFormRetratilDocItens();
        }
    }
});

// Setar e Resetar FormRetratil DocItens
function setFormRetratilDocItens(registro) {

    registro.codigo && $('#doc-item-codigo').val(registro.codigo) || $('#doc-item-codigo').val('');
    registro.descricao && $('#doc-item-descricao').val(registro.descricao) || $('#doc-item-descricao').val('');
    registro.titulo && $('#doc-item-titulo').val(registro.titulo) || $('#doc-item-titulo').val('');
    registro.ordem && $('#doc-item-ordem').val(registro.ordem) || $('#doc-item-ordem').val('');
    registro.dataPublicacao && $("input[name='DataPublicacao']").val(moment(registro.dataPublicacao).format('L')) || $("input[name='DataPublicacao']").val(moment(new Date()).format('L'));
    registro.versao && $('#doc-item-versao').val(registro.versao) || $('#doc-item-versao').val('');
    registro.capitulo && setCboCap(registro.capitulo) || resetCboCap();
    
    if (registro.conteudo)
        $('#doc-item-conteudo').summernote('code', registro.conteudo);
    else
        $('#doc-item-conteudo').summernote('code','');

    // Elementos botoes labels etc
    //$('#div-form').removeClass('contorno-placebo');
    //$('#div-form').addClass('contornado');
    //$('#icone-btn-salvar').removeClass('fa fa-plus');
    //$('#icone-btn-salvar').addClass('glyphicon glyphicon-edit');
    //$('#titulo-config').html('@L("Editando")');
    //   $('#cabec-config').addClass('titulo-azul');
    //$('#btn-apagar-config').fadeIn();
}
function resetFormRetratilDocItens() {
    $('#doc-item-codigo').val('');
    $('#doc-item-descricao').val('');
    $('#doc-item-titulo').val('');
    $('#doc-item-ordem').val('');
    $("input[name='DataPublicacao']").val(moment(new Date()).format('L'));
    $('#doc-item-versao').val('');
    $('#doc-item-conteudo').summernote('code', '');
    resetCboCap();
}

// (Re)Carregar JTable
function getDocItens(reload) {
    if (reload) {
        docItensTable.jtable('reload');
    } else {
        docItensTable.jtable('load', {
            filtro: $('#doc-itensfiltro-tabela').val()
        });
    }
}
$('#btn-get-doc-itens').click(function (e) {
    e.preventDefault();
    getDocItens();
});
getDocItens();

// DocItem Conteudo - Summernote (editor de texto)
$('#doc-item-conteudo').summernote({
    height: 500
});

// Salvar DocItem - function
function salvarDocItem() {
    var docItemDto = {
        Codigo: $('#doc-item-codigo').val(),
        Titulo: $('#doc-item-titulo').val(),
        Descricao: $('#doc-item-descricao').val(),
        Ordem: $('#doc-item-ordem').val(),
        DataPublicacao: $("input[name='DataPublicacao']").val(),
        Versao: $('#doc-item-versao').val(),
        Capitulo: $('#cbo-doc-item-cap').val(),
        Conteudo: $('#doc-item-conteudo').summernote('code')
    }
    // debugger
    var itemsSelecionados = docItensTable.jtable('selectedRows');
    if (itemsSelecionados.length > 0) {
        itemsSelecionados.each(function () {
            var record = $(this).data('record');
            abp.services.app.docItem.obter(record.id)
           .done(function (itemObtido) {
               itemObtido.Codigo = $('#doc-item-codigo').val();
               itemObtido.Titulo = $('#doc-item-titulo').val();
               itemObtido.Descricao = $('#doc-item-descricao').val();
               itemObtido.Ordem = $('#doc-item-ordem').val();
               itemObtido.DataPublicacao = $("input[name='DataPublicacao']").val();
               itemObtido.Versao = $('#doc-item-versao').val();
               itemObtido.Capitulo = $('#cbo-doc-item-cap').val();
               itemObtido.Conteudo = $('#doc-item-conteudo').summernote('code');

               abp.services.app.docItem.criarOuEditar(itemObtido)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     getDocItens();
                     // var jTable = $('#" + jTableId + "'); jTable.find('.jtable-row-selected').click();
                     resetFormRetratilDocItens()
                 });
           });

        });
    } else {
        abp.services.app.docItem.criarOuEditar(docItemDto)
          .done(function () {
              abp.notify.info(app.localize('SavedSuccessfully'));
              getDocItens();
              //var jTable = $('#" + jTableId + "'); jTable.find('.jtable-row-selected').click();
              resetFormRetratilDocItens()
          });
    }
}
// Apagar DocItem
function deleteDocItens(docItem) {
    abp.message.confirm(
        app.localize('DeleteWarning', docItem.descricao),
        function (isConfirmed) {
            if (isConfirmed) {
                docItemAppService.excluir(docItem)
                    .done(function () {
                        getDocItens(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                    });
            }
        }
    );
}

// FIM - DOC ITENS

// ===========================================================

// DOC CAPITULOS

docCapsTable.jtable({

    title: app.localize('Caps'),
    paging: true,
    sorting: true,
    multiSorting: true,
    selecting: true,


    actions: {
        listAction: {
            method: docRotuloAppService.listarCapitulos
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
                if (_permissions.edit) {
                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                        .appendTo($span)
                        .click(function () {
                            _createOrEditModal.open({ id: data.record.id });
                        });
                }

                if (_permissions.delete) {
                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                        .appendTo($span)
                        .click(function () {
                            deleteDocCap(data.record);
                        });
                }

                return $span;
            }
        }
        ,
        codigo: {
            title: app.localize('Codigo'),
            width: '7%'
        }
        ,
        titulo: {
            title: app.localize('Titulo'),
            width: '20%'
        }
        ,
        descricao: {
            title: app.localize('Descricao'),
            width: '25%'
        }
        ,
        ordem: {
            title: app.localize('Ordem'),
            width: '7%'
        }

    },

    selectionChanged: function () {
        var docCapsSelecionados = docCapsTable.jtable('selectedRows');

        // temp - para abrir formRetratil
        $('#exibir-form-retratil-doc-caps').click();

        if (docCapsSelecionados.length > 0) {
            docCapsSelecionados.each(function () {
                var registro = $(this).data('record');
                setFormRetratilDocCaps(registro);
            });
        } else {
            resetFormRetratilDocCaps();
        }
    }
});

// Setar e Resetar FormRetratil DocCaps
function setFormRetratilDocCaps(registro) {
    registro.codigo && $('#doc-cap-codigo').val(registro.codigo) || $('#doc-cap-codigo').val('');
    registro.descricao && $('#doc-cap-descricao').val(registro.descricao) || $('#doc-cap-descricao').val('');
    registro.titulo && $('#doc-cap-titulo').val(registro.titulo) || $('#doc-cap-titulo').val('');
    registro.ordem && $('#doc-cap-ordem').val(registro.ordem) || $('#doc-cap-ordem').val('');
}
function resetFormRetratilDocCaps() {
    $('#doc-cap-codigo').val('');
    $('#doc-cap-descricao').val('');
    $('#doc-cap-titulo').val('');
    $('#doc-cap-ordem').val('');
};

// (Re)Carregar JTable
function getDocCaps(reload) {
    if (reload) {
        docCapsTable.jtable('reload');
    } else {
        docCapsTable.jtable('load', {
            filtro: $('#doc-capsfiltro-tabela').val()
        });
    }
}
$('#btn-get-doc-caps').click(function (e) {
    e.preventDefault();
    getDocCaps();
});
getDocCaps();

// Salvar DocCap
function salvarDocCap() {
    var docCapituloDto = {
        Codigo: $('#doc-cap-codigo').val(),
        Titulo: $('#doc-cap-titulo').val(),
        Descricao: $('#doc-cap-descricao').val(),
        Ordem: $('#doc-cap-ordem').val(),
        IsCapitulo: true
        //,
        //IsSessao: false,
        //IsAssunto: false
    }
    //   debugger
    var capsSelecionados = docCapsTable.jtable('selectedRows');
    if (capsSelecionados.length > 0) {
        capsSelecionados.each(function () {
            var record = $(this).data('record');

            abp.services.app.docRotulo.obter(record.id)
           .done(function (capituloObtido) {
               capituloObtido.Codigo = $('#doc-cap-codigo').val();
               capituloObtido.Titulo = $('#doc-cap-titulo').val();
               capituloObtido.Descricao = $('#doc-cap-descricao').val();
               capituloObtido.Ordem = $('#doc-cap-ordem').val();
               capituloObtido.IsCapitulo = true;
            //   capituloObtido.IsSessao = $('#chk-doc-cap-is-sessao').is(':checked');
            //   capituloObtido.IsAssunto = $('#chk-doc-cap-is-assunto').is(':checked');

               abp.services.app.docRotulo.criarOuEditar(capituloObtido)
                 .done(function () {
                     abp.notify.info(app.localize('SavedSuccessfully'));
                     getDocCaps();
                     //resetarForm();
                 });
           });

        });

    } else {
        //debugger
        abp.services.app.docRotulo.criarOuEditar(docCapituloDto)
          .done(function () {
              abp.notify.info(app.localize('SavedSuccessfully'));
              getDocCaps();
              //resetarForm();
          });
    }
}
// Apagar DocCap
function deleteDocCap(docCap) {
    abp.message.confirm(
        app.localize('DeleteWarning', docCap.descricao),
        function (isConfirmed) {
            if (isConfirmed) {
                docCapAppService.excluir(docCap)
                    .done(function () {
                        getDocCaps(true);
                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                    });
            }
        }
    );
}

// FIM DOC CAPS

// ===========================================================

function createRequestParams() {
    var prms = {};
    docItensFiltroForm.serializeArray().map(function (x) { prms[x.name] = x.value; });
    return $.extend(prms);
}


//abp.event.on('app.CriarOuEditarDocItemModalSaved', function () {
//    getDocItens(true);
//});
