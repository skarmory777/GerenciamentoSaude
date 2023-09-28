/* bricks */
(function (factory) {
    if (typeof define === 'function' && define.amd) {
        define(['jquery'], factory);
    } else if (typeof module === 'object' && module.exports) {
        module.exports = factory(require('jquery'));
    } else {
        factory(window.jQuery);
    }
}(function ($) {
    $.extend(true, $.summernote.lang, {
        'pt-br': {
            bricks: {
                templates: {
                    'template1': "Template 2 colunas"
                }
            }
        }
    });
    $.extend($.summernote.options, {
        bricks: {
            icon: '<i class=" note-icon fas fa-th-large"></i> ',
            tooltip: undefined,
            templates: {}
        }
    });
    $.extend($.summernote.plugins, {
        'bricks': function (context) {
            let ui = $.summernote.ui,
                $note = context.layoutInfo.note,
                modules = context.modules,
                options = context.options,
                lang = options.langInfo,
                plugins = {},
                promises = [],
                templateItems = [];

            this.initialize = () => {
                context.memo('button.bricks', function () {
                    const button = ui.buttonGroup([
                        ui.button({
                            className: 'dropdown-toggle',
                            contents: '<i class=" note-icon fas fa-cube"></i>',
                            tooltip: lang.bricks && lang.bricks.tooltip ? lang.bricks.tooltip : undefined,
                            container: 'body',
                            data: {
                                toggle: 'dropdown'
                            }
                        })]);
                    return button.render();
                });

                if (options.bricks && options.bricks.templates) {
                    $.each(options.bricks.templates, (index, item) => {
                        if (item instanceof Object) {
                            promises.push($.get(item.url));
                            templateItems.push(item);
                        }
                        else {
                            promises.push($.get(item));
                            templateItems.push(index);
                        }
                    });

                    Promise.all(promises).then(resPromises => {
                        $.each(templateItems, (index, item) => {
                            addItem(plugins, item, resPromises[index]);
                        });
                        renderButton();
                    });
                }
            };
            
            function renderButton() {
                let pluginKeys = Object.keys(plugins);
                context.memo('button.bricks', function () {
                    const button = ui.buttonGroup([
                        ui.button({
                            className: 'dropdown-toggle',
                            contents: '<i class=" note-icon fas fa-cube"></i>',
                            tooltip: lang.bricks && lang.bricks.tooltip ? lang.bricks.tooltip : undefined,
                            container: 'body',
                            data: {
                                toggle: 'dropdown'
                            }
                        }),
                        ui.dropdown({
                            className: 'dropdown-template',
                            items: pluginKeys,
                            click: function (e) {
                                const $button = $(e.target);
                                const value = $button.data('value');
                                e.preventDefault();
                                var element = plugins[value];
                                if (element.properties.isModal) {
                                    var modalTemplate = $(`
                                    <div class="modal modal-bricks fade right"   tabindex="-1" role="dialog"  aria-labelledby="myModalLabel" aria-hidden="true" data-backdrop="false">
                                      <div class="modal-dialog" style="min-width:100%" role="document">
                                        <!--Content-->
                                        <div class="modal-content">
                                          <!--Header-->
                                          <div class="modal-header" style="min-height:30px !important">
                                            <h4 class="pull-left">${value}</h4>
                                            <button type="button" class="close pull-right" style="margin: 5px 10px !important;" data-dismiss="modal" aria-label="Close">
                                              <span aria-hidden="true" class="white-text">&times;</span>
                                            </button>
                                          </div>
                                          <!--Body-->
                                          <div class="modal-body">
                                            ${element.html}
                                          </div>
                                        </div>
                                        <!--/.Content-->
                                      </div>
                                    </div>`);
                                    modalTemplate.data("value", value);
                                    modalTemplate.data("properties", element.properties);
                                    $('.modal-bricks').remove();
                                    $('body').append(modalTemplate);
                                    var elem = $('div.modal.fade').filter(function () {
                                        let self = this;
                                        return $(self).data('value') == value;
                                    });

                                    elem.find('button.close').on('click', () => { elem.remove(); });

                                    elem.modal("show");
                                }
                                else {
                                    context.invoke('editor.insertNode', $.parseHTML(element.html)[0]);
                                }

                            }
                        })
                    ]);
                    return button.render();
                });
                context.layoutInfo.toolbar.empty();
                context.invoke('buttons.build', context.layoutInfo.toolbar, options.toolbar);
            }

            function addItem(plugins, item, html) {
                if (item instanceof Object) {
                    addObjectItem(plugins, item, html);
                }
                else {
                    addSimpleItem(plugins, item, html);
                }
            }
            function addObjectItem(plugins, item, html) {
                plugins[item.name] = {
                    text: lang.bricks && lang.bricks.templates && lang.bricks.templates[item.name] !== undefined ? lang.bricks[item.name] : item.name,
                    html: html,
                    properties: item.properties
                };
            }
            function addSimpleItem(plugins, item, html) {
                plugins[item] = {
                    text: lang.bricks && lang.bricks.templates && lang.bricks.templates[item] !== undefined ? lang.bricks[item] : item,
                    html: html,
                    properties: {}
                };
            }
        }
    });
}));
