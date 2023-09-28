numeral.locale("pt-br");
function GridException(message) {
    this.message = message;
    this.name = "GridException";
}
function uuidv4() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
const baseColumn = (field, headerName, ...options) => {
    return _.extend({},{
        headerName,
        field,
        headerTooltip: headerName,
        tooltipField:field,
        id: `${field}-${headerName}`
    }, ...options);
};
const agGridUserPreference = abp.services.app.agGridUserPreference;
const __HOOKS = {
    BEFORE_CREATED:'beforeCreated',
    AFTER_CREATED:'afterCreated',
};
const __EVENTS = {
    FILTER_CHANGED: 'filterChanged',
    GRID_READY: 'GridReady',
    GRID_UPDATED:'gridUpdated',
    DATA_LOADED:'dataLoaded',
}
function getBaseOptions (gridName, options) {
    const  columnChangeHandler = _.debounce(doColumnChange, 150, false);
    const baseOptions = {
        components: {
            //'qtdCellRenderer': qtdCellRenderer,
            'actionRenderer': actionRenderer,
            'dateTimeRenderer': dateTimeRenderer,
            'moneyRenderer': cellRenderer.money,
            'booleanRender': booleanRender,
            'integerRender': cellRenderer.integer,
            'floatRender': cellRenderer.float,
            'statusRenderer': statusRenderer,
        },
        defaultColDef: {
            minWidth: 100,
            sortable: true,
            filter: true,
            resizable: true
        },
        //pagination: true,
        enableBrowserTooltips: false,
        tooltipShowDelay: 0,
        //height: "300",
        rowData: null,
        columnDefs: null,
        beforeCreate: undefined,
        changePage(options) {
            return function (e) {
                const paginateOptions = options.data.paginateOptions;
                e.stopImmediatePropagation();
                const el = $(this);
                paginateOptions.currentPage = parseInt(el.data("page-num"));
                if (paginateOptions.currentPage === -1) {
                    paginateOptions.skipCount = -1;
                } else {
                    paginateOptions.skipCount = paginateOptions.maxResultCount * paginateOptions.currentPage
                }
                options.refresh(options);
            }
        },
        changeFilter() {
            abp.event.trigger(`${gridOptions.gridName}::changeFilter`);
        },
        onChangeFilter() {
            gridOptions.data.paginateOptions.currentPage = 0;
            gridOptions.refresh(gridOptions);
        },
        isReady: false,
        createEventListeners() {
            _.forEach(this.events, (event,eventName) => {
                if(_.isArray(event)){
                    _.forEach(event, (callback) => {
                        if(_.isFunction(callback)) {
                            abp.event.on(`${gridOptions.gridName}::${eventName}`, callback);
                        }
                    })
                }else if(_.isFunction(event)) {
                    abp.event.on(`${gridOptions.gridName}::${eventName}`,event);
                }
            })
        },
        createHookListeners() {
            _.forEach(this.hooks, (hook,hookName) => {
                if(_.isArray(hook)){ 
                    _.forEach(hook, (callback) => {
                        if(_.isFunction(callback)) {
                            abp.event.on(`${gridOptions.gridName}::${hookName}`, callback);
                        }
                    })
                }else if(_.isFunction(hook)) {
                    abp.event.on(`${gridOptions.gridName}::${hookName}`,hook);
                }
            })
        },
        __gridReady(params) {
            if (!_.isEmpty(gridOptions.api)) {
                gridOptions.isReady = true;
            }
            if (gridOptions.data.autoInitialLoad) {
                gridOptions.refresh(params);
            }
        },
        onGridReady(params) {
            this.createEventListeners();
            this.createHookListeners();
            abp.event.on(`${gridOptions.gridName}::${__EVENTS.GRID_READY}`, this.__gridReady);
            gridOptions.gridEl = $(params.api.context.contextParams.providedBeanInstances.eGridDiv);

            gridOptions.createColumnsChooserModal(params);
            if (!_.isEmpty(gridOptions.preferences)) {
                gridOptions.setPreferences();
            }
            setTimeout(() => {
                abp.event.trigger(`${gridOptions.gridName}::${__EVENTS.GRID_READY}`, {params});
            }, 500);
        },
        setPreferences() {
            if (!_.isEmpty(gridOptions.preferences.columnState)) {
                gridOptions.columnApi.applyColumnState(JSON.parse(gridOptions.preferences.columnState));
            }
            if (!_.isEmpty(gridOptions.preferences.columnGroupState)) {
                gridOptions.columnApi.setColumnGroupState(JSON.parse(gridOptions.preferences.columnGroupState));
            }

            if (!_.isEmpty(gridOptions.preferences.sortModel)) {
                gridOptions.api.setSortModel(JSON.parse(gridOptions.preferences.sortModel));
            }
            if (!_.isEmpty(gridOptions.preferences.filterModel)) {
                gridOptions.api.setFilterModel(JSON.parse(gridOptions.preferences.filterModel));
            }
            gridOptions.api.sizeColumnsToFit();
        },
        openColumnsChooser() {
            gridOptions.gridEl.find('.column-chooser-modal').modal();
        },
        createColumnsChooserModal() {
            let columns = "";
            _.forEach(gridOptions.api.columnDefs, (column) => {
                columns += `
                    <div class="col-md-6" style="padding-top: 10px;padding-bottom: 10px">
                        <div class="pretty p-switch">
                            <input type="checkbox" class="column-chooser-item" id="${column.field}" name="${column.field}" data-id="${column.field}" checked="${!column.hide}" ${column.lockVisible ? "disabled" : ""} />
                            <div class="state p-success">
                                <label> ${column.headerName}</label>
                            </div>
                        </div>
                    </div>`;
            });

            const columnChooserModal = `
                <div class="modal fade column-chooser-modal" role="dialog">
                    <div class="modal-dialog">
                        <!-- Modal content-->
                        <div class="modal-content">
                            <div class="modal-header">
                                <h4 class="modal-title">Selecione as colunas</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    ${columns}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>`;
            gridOptions.gridEl.append(columnChooserModal);
            gridOptions.gridEl.find(".column-chooser-modal .column-chooser-item").on("change", function (e) {
                const item = $(this);
                gridOptions.api.columnController.setColumnVisible(item.data("id"), item.attr("checked") === "checked")
                const params = {force: true};
                gridOptions.api.refreshCells(params);
                gridOptions.api.doLayout();
                gridOptions.api.sizeColumnsToFit();
            })
        },
        refresh(options) {
            const dataOptions = gridOptions.data;
            let callback = dataOptions.callback;
            if (_.isUndefined(callback)) {
                throw new GridException("não existe callback definido");
                return;
            }
            const dataRequest = _.isFunction(dataOptions.getData) ? internalData(dataOptions.getData()) : internalData({})
            gridOptions.api.showLoadingOverlay();
            callback(dataRequest).then(res => {
                gridOptions.rowData = res.items;
                dataOptions.paginateOptions.totalCount = res.totalCount;
                gridOptions.api.hideOverlay();
                gridOptions.atualizarGrid(gridOptions.gridEl, gridOptions)
            });

            function internalData(data) {
                return _.extend({}, {
                    maxResultCount: dataOptions.paginateOptions.maxResultCount,
                    skipCount: dataOptions.paginateOptions.skipCount,
                    currentPage: dataOptions.paginateOptions.currentPage
                }, data)
            }
        },
        atualizarGrid(grid, options) {
            const params = {force: true};
            options.api.refreshCells(params);
            //grid.css('width', $('.portlet.light').width());
            options.api.setRowData(options.rowData);
            if (!options.rowData || !options.rowData.length) {
                options.api.showNoRowsOverlay();
            }
            options.api.doLayout();
            options.buildPagination(options);
            options.api.sizeColumnsToFit();

            abp.event.trigger(`${options.gridName}::${__EVENTS.GRID_UPDATED}`);

        },
        buildPagination(options) {
            if (!options.data.enablePagination || options.data.paginateOptions.totalCount === 0) {
                return;
            }
            const paginateOptions = options.data.paginateOptions;
            const agPagingPanel = gridOptions.gridEl.find(".ag-paging-panel")
            const totalPages = Math.ceil(paginateOptions.totalCount / paginateOptions.maxResultCount);
            if(totalPages === 0 || totalPages === 1) {
                agPagingPanel.find("li.page-item a.page-link").off("click", options.changePage(this));
                agPagingPanel.addClass("ag-hidden");
                return;
            }

            // if (paginateOptions.currentPage !== i) {
            //     pages += `<li class="page-item"> <a data-page-num="${i}" class="page-link" href="javascript:void(0);">${(i + 1)}</a> </li>`
            // } else {
            //     pages += `<li class="page-item active" aria-current="page"> <span class="page-link">${(i + 1)}</span> </li>`
            // }
            const breakLabel = '...';
            let pages = ``;
            let breakView = breakViewComponent(breakLabel);
            const items = [];
            if(totalPages <= paginateOptions.pageRangeDisplayed) {
                for (let index = 0; index < totalPages; index++) {
                    items.push(pageComponent(index, paginateOptions.currentPage));
                }
            } else {
                let leftSide = paginateOptions.pageRangeDisplayed / 2;
                let rightSide = paginateOptions.pageRangeDisplayed - leftSide;

                if (paginateOptions.currentPage > totalPages - paginateOptions.pageRangeDisplayed / 2) {
                    rightSide = totalPages - paginateOptions.currentPage;
                    leftSide = paginateOptions.pageRangeDisplayed - rightSide;
                } else if (paginateOptions.currentPage < paginateOptions.pageRangeDisplayed / 2) {
                    leftSide = paginateOptions.currentPage;
                    rightSide = paginateOptions.pageRangeDisplayed - leftSide;
                }
                let page;
                for (let index = 0; index < totalPages; index++) {
                    page = index +1;
                    
                    if (page <= paginateOptions.marginPagesDisplayed) {
                        items.push(pageComponent(index, paginateOptions.currentPage));
                        continue;
                    }
                    if (page > totalPages - paginateOptions.marginPagesDisplayed) {
                        items.push(pageComponent(index, paginateOptions.currentPage));
                        continue;
                    }

                    if (index >= paginateOptions.currentPage - leftSide && index <= paginateOptions.currentPage + rightSide) {
                        items.push(pageComponent(index, paginateOptions.currentPage));
                        continue;
                    }

                    if (breakLabel && items[items.length - 1] !== breakView) {
                        items.push(breakView);
                    }
                }   
            }

            _.forEach(items, pageItem => {
                pages += pageItem;
            })

            
            agPagingPanel.css("height",'54px');
            agPagingPanel.empty();

            let lastItemPage = (paginateOptions.maxResultCount * (paginateOptions.currentPage + 1));
            if (lastItemPage > paginateOptions.totalCount) {
                lastItemPage = paginateOptions.totalCount
            }
            const nav = `
            <div style="width: 100%; display: inline-flex; justify-content: space-between;">
                <div class="select-num-per-pages-container" style="margin: 10px 0;display: flex;">
                    <select class="form-control select-num-per-pages"  style="height: auto !important;">
                      <option value="50" ${paginateOptions.maxResultCount === 50 ? "selected" : ""}>50</option>
                      <option value="100" ${paginateOptions.maxResultCount === 100 ? "selected" : ""}>100</option>
                      <option value="250" ${paginateOptions.maxResultCount === 250 ? "selected" : ""}>250</option>
                      <option value="500" ${paginateOptions.maxResultCount === 500 ? "selected" : ""}>500</option>
                      <option value="-1" ${paginateOptions.maxResultCount === -1 ? "selected" : ""}>Todos</option>
                    </select>
                    <button class="btn btn-default btn-column-chooser" style="padding: 6px 12px !important;" type="button"><i class="fas fa-th"></i></button>
                </div>
                <nav class="ag-custom-pagination">
                  <ul class="pagination">
                    <li class="page-item">
                      <span class="page-link page-first">Primeira</span>
                    </li>
                    <li class="page-item disabled">
                      <span class="page-link page-previous"><i class="fas fa-angle-double-left"></i></span>
                    </li>
                    ${pages}
                    <li class="page-item">
                      <a class="page-link page-next"><i class="fas fa-angle-double-right"></i></a>
                    </li>
                    <li class="page-item">
                      <span class="page-link last">Última</span>
                    </li>
                  </ul>
                </nav>
                <div style="margin: 10px 0;display: flex;">
                    <span style="line-height: 1.42857;padding: 6px 1.5px;color: #337ab7;font-weight: 700;">Mostrando de</span>  
                    <span style="line-height: 1.42857;padding: 6px 1.5px;color: #337ab7;font-weight: 700;">${paginateOptions.skipCount + 1}</span> 
                    <span style="line-height: 1.42857;padding: 6px 1.5px;color: #337ab7;font-weight: 700;">até</span> 
                    <span style="line-height: 1.42857;padding: 6px 1.5px;color: #337ab7;font-weight: 700;">${lastItemPage}</span> 
                    <span style="line-height: 1.42857;padding: 6px 1.5px;color: #337ab7;font-weight: 700;">de</span> 
                    <span style="line-height: 1.42857;padding: 6px 1.5px;color: #337ab7;font-weight: 700;">${paginateOptions.totalCount}</span>
                    <span style="line-height: 1.42857;padding: 6px 1.5px;color: #337ab7;font-weight: 700;">registros</span>
                </div>
            </div>`

            agPagingPanel.append($(nav));
            agPagingPanel.find(".select-num-per-pages").select2({
                minimumResultsForSearch: -1,
                //dropdownCssClass: 'select-num-per-pages-dp'
            });
            agPagingPanel.find("li.page-item a.page-link").on("click", options.changePage(this));
            agPagingPanel.removeClass("ag-hidden");
            agPagingPanel.find(".btn-column-chooser").on('click', gridOptions.openColumnsChooser);

            function pageComponent(index, currentPage){
                if (currentPage !== index) {
                    return  `<li class="page-item"> <a data-page-num="${index}" class="page-link" href="javascript:void(0);">${(index + 1)}</a> </li>`
                }
                return `<li class="page-item active" aria-current="page"> <span class="page-link">${(index + 1)}</span> </li>`
            }
            function breakViewComponent(breakLabel) {
                return `<li class="disabled"> <a href="#">${breakLabel}</a> </li>`
            }
        },
        data: {
            callback:undefined,
            enablePagination: false,
            autoInitialLoad: false,
            paginateOptions: {
                pageCount:20,
                pageRangeDisplayed:3,
                marginPagesDisplayed: 3,
                maxResultCount: 50,
                skipCount: 0,
                currentPage: 0,
                totalCount: 0
            },
        },
        hooks: {},
        events:{},
        hasEvent(eventName) {
          return !_.isEmpty(this.events) && !_.isEmpty(this.events[eventName]) && _.isFunction(this.events[eventName])  
        },
        getEvent(eventName) {
            if(this.hasEvent(eventName)) {
                return this.events[eventName];
            }
            return undefined;
        },
        hasHook(hookName) {
            return !_.isUndefined(this.hooks) && !_.isNull(this.hooks) && !_.isEmpty(this.hooks[hookName]) && (_.isFunction(this.hooks[hookName]) || _.isArray(this.hooks[hookName]))
        },
        getHook(hookName) {
            if(this.hasHook(hookName)) {
                return this.hooks[hookName];
            }
            return undefined;
        },
        dispatchHook(hookName,args) {
            if(gridOptions.hasHook(hookName)) {
                _.forEach(gridOptions.getHook(hookName), (hookCb) => {
                    hookCb(args);
                });
            }
        },
        //onDisplayedColumnsChanged: debouncedColumnChange,
        onColumnMoved: columnChangeHandler,
        onColumnResized: columnChangeHandler,
        onColumnVisible: columnChangeHandler,
        onColumnPinned: columnChangeHandler,
        createLoader() {
            if(!this.el) {
                return;
            }
            const loader = `<div class="loader" style="">
               <img src="/libs/spinner.io/Spinner.svg">
               <p class="loading">Carregando<span>.</span><span>.</span><span>.</span></p>
            </div>`;
            $(this.el).append($(loader));
        }
    };

    const gridOptions = _.merge({}, baseOptions, options, {gridName});
    return gridOptions;
    function doColumnChange(event) {
        if(gridOptions.isReady && gridOptions.columnApi) {
            agGridUserPreference.savePreferences({
                gridIdentifier: gridOptions.gridName,
                columnState: JSON.stringify(gridOptions.columnApi.getColumnState()),
                columnGroupState: JSON.stringify(gridOptions.columnApi.getColumnGroupState()),
                sortModel: JSON.stringify(gridOptions.api.getSortModel()),
                filterModel: JSON.stringify(gridOptions.api.getFilterModel())
            }).then(() => {
                // abp.notify.info("Preferências atualizadas");
            });
        }
    }
}
const AgGridHelper = {
    HOOKS: __HOOKS,
    EVENTS: __EVENTS,
    createAgGrid(gridName, options) {
        createHooks(options);
        createEvents(options);
        const gridOptions = getBaseOptions(gridName, options)
        if (!gridOptions.gridName) {
            throw new GridException("não existe nome definido");
            return Promise.reject("não existe nome definido");
        }
        
        return {
            getApi:() => gridOptions.api,
            getColumnApi:() => gridOptions.columnApi,
            //gridOptions,
            render(el) {
                if(gridOptions.isReady) {
                    this.refresh();
                    return;
                }
                gridOptions.el = el;
                gridOptions.createLoader();
                return agGridUserPreference.getPreferences(gridOptions.gridName).then(getPreferencesAndCreate).then((options) => {
                    return options.then(() => {
                        gridOptions.dispatchHook(__HOOKS.AFTER_CREATED, {el: gridOptions.el, gridOptions});
                    })
                });
            },
            setColumns(columnDefs) {
                this.setProperty("columnDefs",columnDefs)
                return this;
            },
            setProperty(property, value) {
                this.gridOptions[property] = value;
                return this;
            },
            setEvent(eventName,callback) {
                if(_.isNull(this.gridOptions.events)) {
                    this.gridOptions.events = {};
                }
                this.gridOptions.events[eventName] = callback;
                return this;
            },
            setHook(hookName, callback) {
                if(_.isNull(this.gridOptions.hooks)) {
                    this.gridOptions.hooks = {};
                }
                this.gridOptions.hooks[hookName] = callback;
                return this;
            },
            getSelectedRows() {
                return gridOptions.api.getSelectedRows()
            },
            changeFilter() {
                return this;
            },
            refresh() {
                gridOptions.refresh(gridOptions);
            }
        }
        
        function createEvents(options) {
            if (_.isEmpty(options.events)) {
                options.events = []
                _.forEach(__EVENTS, (eventName) => {
                    if (options[eventName] && _.isFunction(options[eventName])) {
                        if(_.isEmpty(options.events[eventName])) {
                            options.events[eventName] = []
                        }
                        options.events[eventName].push(options[eventName]);
                    }
                });
            }
        }

        function createHooks(options) {
            if(_.isEmpty(options.hooks)) {
                options.hooks = []
            }
            _.forEach(__HOOKS, (hookName) => {
                if(_.isEmpty(options.hooks[hookName])) {
                    options.hooks[hookName] = []
                }
                if(options[hookName] && _.isFunction(options[hookName])) {
                    options.hooks[hookName].push(options[hookName]);
                }
            });
        }
        
        function getPreferencesAndCreate(agGridPreference) {
            gridOptions.preferences = agGridPreference;
            return new Promise(resolve => setTimeout(() => {
                gridOptions.dispatchHook(__HOOKS.BEFORE_CREATED, {el: gridOptions.el, gridOptions});
                $(gridOptions.el).empty();
                new agGrid.Grid(gridOptions.el, gridOptions);
                resolve(gridOptions);
            }, 500));
        }
    },
    columns: {
        base: baseColumn,
        checkboxSelection(...options) {
            return baseColumn('', '', { checkboxSelection: true, width: 25, suppressSizeToFit: true }, ...options);
        },
        dateTime(field,headerName,...options) {
            return baseColumn(field,headerName, {width: 140, cellRenderer: 'dateTimeRenderer', suppressSizeToFit:true},...options)
        },
        integer(field,headerName,...options) {
            return baseColumn(field,headerName, {width: 100, cellRenderer: 'integerRenderer', suppressSizeToFit:true},...options)
        },
        float(field,headerName,...options) {
            return baseColumn(field,headerName, {width: 100, cellRenderer: 'floatRenderer', suppressSizeToFit:true},...options)
        },
        perc(field,headerName,...options) {
            return baseColumn(field,headerName, {width: 100, cellRenderer: 'percRenderer', suppressSizeToFit:true},...options)
        },
        money(field, headerName, ...options) {
            return baseColumn(field,headerName, {width: 100, cellRenderer: 'moneyRenderer', suppressSizeToFit:true},...options)
        },
        boolean(field,headerName,...options) {
            return baseColumn(field,headerName, {width: 50, cellRenderer: 'booleanRender',suppressSizeToFit:true},...options)
        },
        status(field,headerName,...options) {
            return baseColumn(field,headerName, {width: 100, cellRenderer: 'statusRenderer'},...options)
        },
        action(permission, ...options) {
            return baseColumn('id', app.localize('Ações'), {
                filter: false,
                width: 100,
                suppressMenu: true,
                cellRenderer: 'actionRenderer',
                lockVisible:true,
                suppressSizeToFit:true,
                cellRendererParams: _.extend({},{
                    actionRenderAction: onActionRender
                },permission)
            }, ...options)
        }
    }
}

const dateCellRender = (format) => {
    return (data) => {
        if (data.value !== undefined && data.value !== null) {
            const momentObj = moment(data.value)
            if (momentObj.isValid()) {
                return momentObj.format(format)
            }
        }
        return "";
    }
}

const numericCellRender = (format) => {
    return (data) => {
        if (data.value !== undefined && data.value !== null) {
            const momentObj = numeral(data.value)
            if (momentObj.value != null) {
                return momentObj.format(format)
            }
        }
        return "";
    }
}


const  cellRenderer = {
    date: dateCellRender("DD/MM/YYYY HH:mm:ss"),
    float: numericCellRender("0,0.00"),
    integer: numericCellRender("0,0"),
    perc: numericCellRender("0.00%"),
    money: numericCellRender("$0,0.00"),
    dateOptions: {
        dateTime: dateCellRender("DD/MM/YYYY HH:mm:ss"),
        dateOnly: dateCellRender("DD/MM/YYYY"),
        dateCustom: dateCellRender
    },
    floatOptions: {
        umaCasas: numericCellRender("0,0.0"),
        duasCasas: numericCellRender("0,0.00"),
        tresCasas: numericCellRender("0,0.000"),
        quatroCasas: numericCellRender("0,0.0000"),
        cincoCasas: numericCellRender("0,0.00000"),
    },
    percOptions: {
        umaCasas: numericCellRender("0.0%"),
        duasCasas: numericCellRender("0.00%"),
        tresCasas: numericCellRender("0.000%"),
        quatroCasas: numericCellRender("0.0000%"),
        cincoCasas: numericCellRender("0.00000%"),
    },
    moneyOptions: {
        tresCasas: numericCellRender("$0,0.000"),
        quatroCasas: numericCellRender("$0,0.0000"),
        cincoCasas: numericCellRender("$0,0.00000")
    }
}

function dateTimeRenderer() {

}

dateTimeRenderer.prototype.init = function (params) {
    const span = document.createElement('span');
    let $span = $('<span></span>');
    if (params.value) {
        const momentObj = moment(params.value).utcOffset(params.value)
        if(momentObj.isValid()) {
            $span.html(momentObj.format("DD/MM/YYYY HH:mm:ss"))
        }
    }
    this.eGui = $span[0];
};

dateTimeRenderer.prototype.getGui = function () {
    return this.eGui;
};

//function moneyRenderer() {

//}

//moneyRenderer.prototype.init = function (params) {
//    let $span = $('<span></span>');
//    debugger;
//    if (params.value) {
//        $span.html(cellRenderer.money(params.value))
//    }
//    this.eGui = $span[0];
//};

//moneyRenderer.prototype.getGui = function () {
//    return this.eGui;
//};

function booleanRender() {
    
}

booleanRender.prototype.init = function (params) {
    const span = document.createElement('span');
    let $span = $('<span></span>');
    
    $span.css('width', '100%');
    $span.css('text-align', 'center');
    $span.css('display', 'block');
    let defaultTrueIcon = $("<i class=\"fa fa-check\" style='color:rgb(56, 142, 60);'></i>");
    let defaultFalseIcon = $("<i class=\"fa fa-ban\" style='color:#E26A6A'></i>");
    if (params.value != null) {
        $span.append(params.value === true || params.value === 'true' ? defaultTrueIcon: defaultFalseIcon)
    }
    this.eGui = $span[0];
};

booleanRender.prototype.getGui = function () {
    return this.eGui;
};

function momneyRenderer() {

}

momneyRenderer.prototype.init = function (params) {
    const span = document.createElement('span');
    let $span = $('<span></span>');
    if (params.value) {
        agGridHelper.cellRenderer.money(params.value)
    }
    this.eGui = $span[0];
};

momneyRenderer.prototype.getGui = function () {
    return this.eGui;
};

function statusRenderer() {
    
}

statusRenderer.prototype.init = function (params) {
    const span = document.createElement('span');
    let $span = $('<span></span>');
    let corFundo = "";
    let corTexto = "";
    let statusClass = "";
    if (params.value != null) {
        if (params.corFundo) {
            if (params.node.data[params.corFundo]) {
                corFundo = params.node.data[params.corFundo];
            } else {
                corFundo = params.corFundo;
            }
        }
        if (params.corTexto) {
            if (params.node.data[params.corTexto]) {
                corTexto = params.node.data[params.corTexto];
            } else {
                corTexto = params.corTexto;
            }
        }
        statusClass = params.statusClass
        
        $span.addClass('badge badge-ag-grid').addClass(statusClass).css('color','#'+ corTexto).css('background-color','#'+corFundo).html(params.value);
    }
    this.eGui = $span[0];
};

statusRenderer.prototype.getGui = function () {
    return this.eGui;
};

function onActionRender(params, $content) {
    const gridOptions = params.api.gridOptionsWrapper.gridOptions;
    if (params.enableEdit) {
        $('<button class="btn btn-warning btn-xs" title="' + app.localize('Edit') + '"><i class="fa fa-edit"></i></button>')
            .appendTo($content)
            .click(function (e) {
                e.preventDefault();
                gridOptions.api.selectNode(params.node, true);
                gridOptions.api.ensureNodeVisible(params.node, 'middle');
                gridOptions.editarItem(params.data);
            });
    }
    if (params.enableDelete) {
        $('<button class="btn btn-danger btn-xs" title="' + app.localize('Excluir') + '"><i class="fa fa-trash"></i></button>')
            .appendTo($content)
            .click(function (e) {
                e.preventDefault();
                gridOptions.excluirItem(e, params.data);
            });
    }

    if (params.customAction) {
        _.forEach(params.customAction, (item) => {
            $(`<button class="btn ${item.class} btn-xs" title="${app.localize(item.title)}"><i class="${item.icon}"></i></button>`)
                .appendTo($content)
                .click(function (e) {
                    e.preventDefault();
                    if (item.action && _.isFunction(item.action)) {
                        item.action(e, params.data);
                    }
                });
        })
    }
}

function actionRenderer() {
}

actionRenderer.prototype.init = function (params) {
    const span = document.createElement('span');
    let $span = $('<span></span>');
    if (params.actionRenderAction && _.isFunction(params.actionRenderAction)) {
        params.actionRenderAction(params, $span);
    }

    this.eGui = $span[0];
};

actionRenderer.prototype.getGui = function () {
    return this.eGui;
};


