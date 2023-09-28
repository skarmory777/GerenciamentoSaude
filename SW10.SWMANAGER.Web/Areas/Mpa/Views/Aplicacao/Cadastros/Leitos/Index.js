(function () {
    $(function () {

        //  window.leitoUnidadeSelecionadaId = 0;

        // Retrair menu principal
        $('body').addClass('page-sidebar-closed');
        $('#menu-lateral').addClass('page-sidebar-menu-closed');

        var _organizationUnitService = abp.services.app.organizationUnit;
        var _unidadeOrganizacionalService = abp.services.app.unidadeOrganizacional;

        // Leitos
        var _LeitosService = abp.services.app.leito;

        var _permissions = {
            manageOrganizationTree: abp.auth.hasPermission('Pages.Administration.OrganizationUnits.ManageOrganizationTree'),
            manageMembers: abp.auth.hasPermission('Pages.Administration.OrganizationUnits.ManageMembers')
        };

        // Leitos Modal
        var _createOrEditModalLeitos = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/Leitos/CriarOuEditarModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/Aplicacao/Cadastros/Leitos/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarLeitoModal'
        });

        var _createModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/OrganizationUnits/CreateModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/OrganizationUnits/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarUnidadeOrganizacionalModal'
        });

        var _editModal = new app.ModalManager({
            viewUrl: abp.appPath + 'Mpa/OrganizationUnits/EditModal',
            scriptUrl: abp.appPath + 'Areas/Mpa/Views/OrganizationUnits/_CriarOuEditarModal.js',
            modalClass: 'CriarOuEditarUnidadeOrganizacionalModal'
        });

        var _userLookupModal = app.modals.LookupModal.create({
            title: app.localize('SelectAUser'),
            serviceMethod: abp.services.app.commonLookup.findUsers,
            canSelect: function (item) {
                var ouId = organizationTree.selectedOu.id;
                if (!ouId) {
                    return false;
                }

                return $.Deferred(function ($dfd) {
                    _organizationUnitService.isInOrganizationUnit({
                        userId: item.value,
                        organizationUnitId: ouId
                    }).done(function (result) {
                        if (result) {
                            abp.message.warn(app.localize('UserIsAlreadyInTheOrganizationUnit'));
                        }

                        $dfd.resolve(!result);
                    }).fail(function () {
                        $dfd.reject();
                    });
                });
            }
        });

        // Organization tree
        var organizationTree = {

            $tree: $('#OrganizationUnitEditTree'),

            $emptyInfo: $('#OrganizationUnitTreeEmptyInfo'),

            show: function () {
                organizationTree.$emptyInfo.hide();
                organizationTree.$tree.show();
            },

            hide: function () {
                organizationTree.$emptyInfo.show();
                organizationTree.$tree.hide();
            },

            unitCount: 0,

            setUnitCount: function (unitCount) {
                organizationTree.unitCount = unitCount;
                if (unitCount) {
                    organizationTree.show();
                } else {
                    organizationTree.hide();
                }
            },

            refreshUnitCount: function () {
                organizationTree.setUnitCount(organizationTree.$tree.jstree('get_json').length);
            },

            selectedOu: {
                id: null,
                displayName: null,
                code: null,

                set: function (ouInTree) {
                    if (!ouInTree) {
                        organizationTree.selectedOu.id = null;
                        organizationTree.selectedOu.displayName = null;
                        organizationTree.selectedOu.code = null;
                    } else {
                        organizationTree.selectedOu.id = ouInTree.id;
                        organizationTree.selectedOu.displayName = ouInTree.original.displayName;
                        organizationTree.selectedOu.code = ouInTree.original.code;
                    }

                    members.load(false);
                }
            },

            contextMenu: function (node) {

                var items = {
                    editUnit: {
                        label: app.localize('Edit'),
                        _disabled: !_permissions.manageOrganizationTree,
                        action: function (data) {
                            var instance = $.jstree.reference(data.reference);

                            _editModal.open({
                                id: node.id
                            }, function (updatedOu) {
                                node.original.displayName = updatedOu.displayName;
                                instance.rename_node(node, organizationTree.generateTextOnTree(updatedOu));
                            });
                        }
                    },

                    addSubUnit: {
                        label: app.localize('AddSubUnit'),
                        _disabled: !_permissions.manageOrganizationTree,
                        action: function () {
                            organizationTree.addUnit(node.id);
                        }
                    },

                    addMember: {
                        label: app.localize('AddMember'),
                        _disabled: !_permissions.manageMembers,
                        action: function () {
                            members.openAddModal();
                        }
                    },

                    'delete': {
                        label: app.localize("Delete"),
                        _disabled: !_permissions.manageOrganizationTree,
                        action: function (data) {
                            var instance = $.jstree.reference(data.reference);

                            abp.message.confirm(
                                app.localize('OrganizationUnitDeleteWarningMessage', node.original.displayName),
                                function (isConfirmed) {
                                    if (isConfirmed) {

                                        _unidadeOrganizacionalService.excluir(node.id).done(function () {

                                            //console.log('deletou unidade org');

                                            _organizationUnitService.deleteOrganizationUnit({
                                                id: node.id
                                            }).done(function () {
                                                abp.notify.success(app.localize('SuccessfullyDeleted'));
                                                instance.delete_node(node);
                                                organizationTree.refreshUnitCount();
                                            }).fail(function (err) {
                                                setTimeout(function () { abp.message.error(err.message); }, 500);
                                            });;

                                        }).fail(function (err) {
                                            setTimeout(function () { abp.message.error(err.message); }, 500);
                                        });;



                                    }
                                }
                            );
                        }
                    }
                }

                return items;
            },

            addUnit: function (parentId) {
                var instance = $.jstree.reference(organizationTree.$tree);

                _createModal.open({
                    parentId: parentId
                }, function (newOu) {
                    instance.create_node(
                        parentId ? instance.get_node(parentId) : '#',
                        {
                            id: newOu.id,
                            parent: newOu.parentId ? newOu.parentId : '#',
                            code: newOu.code,
                            displayName: newOu.displayName,
                            memberCount: 0,
                            text: organizationTree.generateTextOnTree(newOu),
                            state: {
                                opened: true
                            }
                        });

                    organizationTree.refreshUnitCount();
                });
            },

            generateTextOnTree: function (ou) {
                var itemClass = ou.memberCount > 0 ? ' ou-text-has-members' : ' ou-text-no-members';
                return '<span title="' + ou.code + '" class="ou-text' + itemClass + '" data-ou-id="' + ou.id + '">' + ou.displayName + ' (<span class="ou-text-member-count">' + ou.memberCount + '</span>) <i class="fa fa-caret-down text-muted"></i></span>';
            },

            incrementMemberCount: function (ouId, incrementAmount) {
                var treeNode = organizationTree.$tree.jstree('get_node', ouId);
                treeNode.original.memberCount = treeNode.original.memberCount + incrementAmount;
                organizationTree.$tree.jstree('rename_node', treeNode, organizationTree.generateTextOnTree(treeNode.original));
            },

            getTreeDataFromServer: function (callback) {
                _organizationUnitService.getOrganizationUnits({}).done(function (result) {
                    var treeData = _.map(result.items, function (item) {
                        return {
                            id: item.id,
                            parent: item.parentId ? item.parentId : '#',
                            code: item.code,
                            displayName: item.displayName,
                            memberCount: item.memberCount,
                            text: organizationTree.generateTextOnTree(item),
                            state: {
                                opened: true
                            }
                        };
                    });

                    callback(treeData);
                });
            },

            init: function () {
                organizationTree.getTreeDataFromServer(function (treeData) {
                    organizationTree.setUnitCount(treeData.length);

                    organizationTree.$tree
                        .on('changed.jstree', function (e, data) {
                            if (data.selected.length != 1) {
                                organizationTree.selectedOu.set(null);
                            } else {
                                var selectedNode = data.instance.get_node(data.selected[0]);
                                organizationTree.selectedOu.set(selectedNode);
                            }
                        })
                        .on('move_node.jstree', function (e, data) {

                            var parentNodeName = (!data.parent || data.parent == '#')
                                ? app.localize('Root')
                                : organizationTree.$tree.jstree('get_node', data.parent).original.displayName;

                            abp.message.confirm(
                                app.localize('OrganizationUnitMoveConfirmMessage', data.node.original.displayName, parentNodeName),
                                function (isConfirmed) {
                                    if (isConfirmed) {
                                        _organizationUnitService.moveOrganizationUnit({
                                            id: data.node.id,
                                            newParentId: data.parent
                                        }).done(function () {
                                            abp.notify.success(app.localize('SuccessfullyMoved'));
                                            organizationTree.reload();
                                        }).fail(function (err) {
                                            organizationTree.$tree.jstree('refresh'); //rollback
                                            setTimeout(function () { abp.message.error(err.message); }, 500);
                                        });
                                    } else {
                                        organizationTree.$tree.jstree('refresh'); //rollback
                                    }
                                }
                            );
                        })
                        .jstree({
                            'core': {
                                data: treeData,
                                multiple: false,
                                check_callback: function (operation, node, node_parent, node_position, more) {
                                    return true;
                                }
                            },
                            types: {
                                "default": {
                                    "icon": "fa fa-folder tree-item-icon-color icon-lg"
                                },
                                "file": {
                                    "icon": "fa fa-file tree-item-icon-color icon-lg"
                                }
                            },
                            contextmenu: {
                                items: organizationTree.contextMenu
                            },
                            sort: function (node1, node2) {
                                if (this.get_node(node2).original.displayName < this.get_node(node1).original.displayName) {
                                    return 1;
                                }

                                return -1;
                            },
                            plugins: [
                                'types',
                                'contextmenu',
                                'wholerow',
                                'sort',
                                'dnd'
                            ]
                        });

                    $('#AddRootUnitButton').click(function (e) {
                        e.preventDefault();
                        organizationTree.addUnit(null);
                    });

                    organizationTree.$tree.on('click', '.ou-text .fa-caret-down', function (e) {
                        e.preventDefault();

                        var ouId = $(this).closest('.ou-text').attr('data-ou-id');
                        setTimeout(function () {
                            organizationTree.$tree.jstree('show_contextmenu', ouId);
                        }, 100);
                    });
                });

                members.showTable();
            },

            reload: function () {
                organizationTree.getTreeDataFromServer(function (treeData) {
                    organizationTree.setUnitCount(treeData.length);
                    organizationTree.$tree.jstree(true).settings.core.data = treeData;
                    organizationTree.$tree.jstree('refresh');
                });
            }
        };

        // Members (leitos)
        var members = {

            $table: $('#OuMembersTable'),
            $emptyInfo: $('#OuMembersEmptyInfo'),
            $addUserToOuButton: $('#AddUserToOuButton'),
            $selectedOuRightTitle: $('#SelectedOuRightTitle'),

            showTable: function () {
                members.$emptyInfo.hide();
                members.$table.show();
                members.$addUserToOuButton.show();

                var unidadeNome = organizationTree.selectedOu.displayName;

                if (!unidadeNome) {
                    unidadeNome = ': Todas as unidades';
                } else {
                    unidadeNome = ': ' + unidadeNome;
                }

                members.$selectedOuRightTitle.text(unidadeNome);
            },

            hideTable: function () {
                //members.$selectedOuRightTitle.hide();
                members.$addUserToOuButton.hide();
                members.$table.hide();
                members.$emptyInfo.show();
            },

            load: function (todas) {
                //console.log(todas);
                if (todas) {
                    members.$table.jtable('load', {
                        uo: 0
                    });
                    //console.log('nenhuma selecionada');
                } else {
                    members.showTable();

                    members.$table.jtable('load', {
                        uo: organizationTree.selectedOu.id
                    });

                    //console.log('alguma selecionada');
                }
            },

            openAddModal: function () {
                var ouId = organizationTree.selectedOu.id;

                _createOrEditModalLeitos.open({
                    title: app.localize('SelectAUser')
                }, function (selectedItem) {
                    members.add(selectedItem.value);
                });
            },

            init: function () {
                members.$table.jtable({

                    title: app.localize('Leitos'),
                    paging: true,
                    sorting: true,
                    multiSorting: true,

                    actions: {
                        listAction: {
                            method: _LeitosService.listarPorUnidade
                        }
                    },

                    fields: {
                        id: {
                            key: true,
                            list: false
                        },
                        actions: {
                            title: app.localize('Actions'),
                            width: '6%',
                            sorting: false,
                            display: function (data) {
                                var $span = $('<span></span>');
                                //if (_permissions.edit) {
                                if (true) {
                                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
                                        .appendTo($span)
                                        .click(function () {
                                            _createOrEditModalLeitos.open({ id: data.record.id });
                                        });
                                }

                                //if (_permissions.delete) {
                                if (true) {
                                    $('<button class="btn btn-default btn-xs" title="' + app.localize('Delete') + '"><i class="fa fa-trash-alt"></i></button>')
                                        .appendTo($span)
                                        .click(function () {

                                            deleteLeitos(data.record);
                                        });
                                }

                                return $span;
                            }
                        }
                        ,
                        codigo: {
                            title: app.localize('Codigo'),
                            width: '3%',
                            display: function (data) {
                                if (data.record.leito) {
                                    return data.record.leito.codigo;
                                }

                            }
                        },
                        descricao: {
                            title: app.localize('Descricao'),
                            width: '8%',
                            display: function (data) {
                                if (data.record.leito) {
                                    return data.record.leito.descricao;
                                }

                            }
                        }
                        ,
                        localizacao: {
                            title: app.localize('Localizacao'),
                            width: '8%',
                            display: function (data) {
                                if (data.record.leito.unidadeOrganizacional) {
                                    return data.record.leito.unidadeOrganizacional.localizacao;
                                }
                            }
                        },

                        unidadeOrganizacional: {
                            title: app.localize('Unidade'),
                            width: '8%',
                            display: function (data) {
                                if (data.record.leito.unidadeOrganizacional) {
                                   
                                    return data.record.leito.unidadeOrganizacional.organizationUnit.displayName;
                                }
                            }
                        }
                        ,
                        ativo: {
                            title: app.localize('Ativo'),
                            width: '2%',
                            display: function (data) {
                                if (data.record.leito.ativo) {
                                    return '<div style="text-align:center;">' + '<span class="label label-success content-center text-center">' + app.localize('Yes') + '</span>' + '</div>';
                                } else {
                                    return '<div style="text-align:center;">' + '<span class="label label-default content-center text-center">' + app.localize('No') + '</span>' + '</div>';
                                }
                            }
                        }
                        ,
                        status: {
                            title: app.localize('Status'),
                            width: '4%',
                            display: function (data) {
                                if (data.record.leito.leitoStatus) {
                                    var cor = data.record.leito.leitoStatus.cor;
                                    var descricao = data.record.leito.leitoStatus.descricao;
                                    return '<div style="text-align:center;">   <span style="display:inline-block; width:20px; height:20px;  text-align:center; background-color: ' + cor + '; border-radius: 25px;">  </span> </div>  ';
                                }
                            }
                        }
                        ,
                        leitoAih: {
                            title: app.localize('LeitoAih'),
                            width: '3%',
                            display: function (data) {
                                if (data.record.leito) {
                                    return data.record.leito.leitoAih;
                                }

                            }
                        }
                        ,
                        ramal: {
                            title: app.localize('Ramal'),
                            width: '3%',
                            display: function (data) {
                                if (data.record.leito) {
                                    return data.record.leito.ramal;
                                }

                            }
                        }
                        ,
                        extra: {
                            title: app.localize('Extra'),
                            width: '2%',
                            display: function (data) {
                                if (data.record.extra) {
                                    return '<div style="text-align:center;">' + '<span class="label label-success content-center text-center">' + app.localize('Yes') + '</span>' + '</div>';
                                } else {
                                    return '<div style="text-align:center;">' + '<span class="label label-default content-center text-center">' + app.localize('No') + '</span>' + '</div>';
                                }
                            }
                        }
                        ,
                        //dataAtualizacao: {
                        //    title: app.localize('DataAtualizacao'),
                        //    width: '4%',
                        //    display: function (data) {
                        //        if (data.record.leito) {
                        //            return moment(data.record.dataAtualizacao).format('L');
                        //        }

                        //    }
                        //}
                    }
                });
                function getLeitos(reload) {
                    if (reload) {
                        $('#OuMembersTable').jtable('reload');
                    }
                    else {
                        $('#OuMembersTableTable').jtable('load', {
                            filtro: $('#LeitosFilter').val()
                        });
                    }
                }


                function deleteLeitos(leito) {

                    abp.message.confirm(
                        app.localize('DeleteWarning', leito.leito.descricao),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                _LeitosService.excluir(leito)
                                    .done(function () {
                                        getLeitos(true);
                                        abp.notify.success(app.localize('SuccessfullyDeleted'));
                                    });
                            }
                        }
                    );
                }


                $('#AddUserToOuButton').click(function (e) {
                    e.preventDefault();
                    members.openAddModal();
                });

                members.hideTable();
            }
        }

        members.init();
        members.$table.jtable('load', {
            filtro: '0'
        });
        organizationTree.init();

        $('#todas').on('click', function (e) {
            e.preventDefault();
            members.load(true);
        });

        // getleito();
    });

})();

//function carregarTodos() {
//    $('#todas').click();
//}

