
const customConfirmModalTemplate =
    `<div class="modal fade confirmation-options" tabindex="-1" role="dialog">
                <div class="modal-dialog modal-sm modal-dialog-center" role="document">
                    <div class="modal-content" style="border-radius: 5px">
                        <div class="modal-header" style="border-top-left-radius: 5px;border-top-right-radius: 5px;">
                            <button type="button" style="margin-right: 5px;margin-top: 10px !important;font-size: 14px !important;color: #F80E3F;" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                            <h4 class="modal-title" style="margin-top: 3px;margin-left: 5px;"></h4>
                        </div>
                        <div class="modal-body">
                            <div class="icon"></div>
                            <h5 class="message" style="text-align:center"></h5>
                            <div class="customContent"></div>
                        </div>
                        <div class="modal-footer" style="text-align:center"></div>
                    </div>
                </div>
            </div>`;

function customConfirmModalRenderButton(btnOptions, customConfirmModalInstance) {
    let btnHtml = $("<button type='button'></button");
    btnHtml.css({ 'padding': '10px', 'margin-bottom': '10px' });
    let html = "";
    if (btnOptions.icon) {
        html += `<i class="${btnOptions.icon}"></i>`
    }
    if (btnOptions.text) {
        html += " " + btnOptions.text;
    }

    if (btnOptions.class) {
        btnHtml.addClass(btnOptions.class)
    }
    if (btnOptions.style) {
        btnHtml.css(btnOptions.style)
    }

    btnHtml.html(html)

    if (btnOptions.callback && _.isFunction(btnOptions.callback)) {
        btnHtml.on("click", (event) => {
            btnOptions.callback(event, customConfirmModalInstance);
        });
    }
    return btnHtml;
}

function customConfirmModal({ title, message, icon, buttons, modalOptions, styles, confirmModalOptions, customContent }) {
    const id = create_UUID();
    const templateHtml = $(customConfirmModalTemplate);
    return baseCustomConfirmModal({ id, templateHtml, title, message, icon, buttons, modalOptions, styles, confirmModalOptions, customContent } )
}

function customConfirmModalAsync({ title, message, icon, buttons, modalOptions, styles, confirmModalOptions, customContent, promiseCallback}) {
    const id = create_UUID();
    const templateHtml = $(customConfirmModalTemplate);
    
    return new Promise(promiseResolve => {
        return baseCustomConfirmModal({ id, templateHtml, title, message, icon, buttons, modalOptions, styles, confirmModalOptions, customContent }, promiseResolve, promiseCallback)
    })
}

function baseCustomConfirmModal({ id, templateHtml, title, message, icon, buttons, modalOptions, styles, confirmModalOptions, customContent }, promiseResolve, promiseCallback) {

    const customConfirmModalInstance = {
        id,
        instance: templateHtml,
        removeModal: (params) => {
            if (customConfirmModalInstance.instance.data("bs.modal")) {
                try {
                    customConfirmModalInstance.instance.modal("dispose");
                }
                catch {
                    if (customConfirmModalInstance.instance.data("bs.modal").$backdrop) {
                        customConfirmModalInstance.instance.data("bs.modal").$backdrop.remove();
                    }
                    //modal already disposed
                }
            }
            customConfirmModalInstance.instance.remove();

            return customConfirmModalInstance.resolver(params || false)

        },
        close: (params) => {
            customConfirmModalInstance.instance.modal("hide");
            return customConfirmModalInstance.removeModal(params || false);
        },
        show: () => {
            customConfirmModalInstance.instance.modal("show");
        },
        resolver: (params) => {
            if (promiseResolve) {
                if (promiseCallback && _.isFunction(promiseCallback)) {
                    return promiseResolve(promiseCallback(params))
                }
                return promiseResolve(params)
            }
        }
    };

    customConfirmModalInstance.instance.addClass(`custom-confirm-modal-${id}`);
    customConfirmModalInstance.instance.data("custom-confirm-modal", id);
    customConfirmModalInstance.instance.find(".modal-title").html(title);
    customConfirmModalInstance.instance.find(".message").html(message);

    if (icon) {
        var iconHtml = $("<i></i>");
        iconHtml.addClass(icon);
        iconHtml.addClass('fa-6x');
        customConfirmModalInstance.instance.find(".icon").append(iconHtml);
        customConfirmModalInstance.instance.find(".icon")
            .css({
                'padding': '30px',
                'width': '100%',
                'text-align': 'center'
            });
    }

    if (customContent) {
        customConfirmModalInstance.instance.find(".customContent").append($(customContent))
    }

    if (styles) {
        if (styles["modal-dialog"]) {
            customConfirmModalInstance.instance.find(".modal-dialog").css(styles["modal-dialog"]);
        }
    }

    _.forEach(buttons, (btn, key) => {
        const btnHtml = customConfirmModalRenderButton(btn, customConfirmModalInstance, promiseResolve);
        btnHtml.addClass(`btn-${key}`);
        btnHtml.appendTo(customConfirmModalInstance.instance.find(".modal-footer"));
    });
    if (confirmModalOptions) {
        if (confirmModalOptions.cancelButton == true || (confirmModalOptions.cancelButton && confirmModalOptions.cancelButton.enable)) {
            let defaultCancel = {
                key: create_UUID(),
                text: "Cancelar",
                icon: 'far fa-times-circle',
                class: "btn btn-danger",
                callback: (event, instance) => {
                    customConfirmModalInstance.close(false)
                }
            }
            if (confirmModalOptions.cancelButton.icon) {
                defaultCancel.icon = confirmModalOptions.cancelButton.icon;
            }

            if (confirmModalOptions.cancelButton.text) {
                defaultCancel.text = confirmModalOptions.cancelButton.text;
            }
            if (confirmModalOptions.cancelButton.key) {
                defaultCancel.key = confirmModalOptions.cancelButton.key;
            }
            if (confirmModalOptions.cancelButton.class) {
                defaultCancel.text = confirmModalOptions.cancelButton.text;
            }

            if (confirmModalOptions.cancelButton.callback && _.isFunction(confirmModalOptions.cancelButton.callback)) {
                defaultCancel.callback = confirmModalOptions.cancelButton.callback;
            }

            const btnCancelHtml = customConfirmModalRenderButton(defaultCancel, customConfirmModalInstance);
            btnCancelHtml.addClass(`btn-${defaultCancel.key}`);
            btnCancelHtml.appendTo(customConfirmModalInstance.instance.find(".modal-footer"));
        }
    }

    $("body").append(templateHtml);
    
    templateHtml.on("hidden.bs.modal", (event) => {
        if (confirmModalOptions && confirmModalOptions.onHideModal && _.isFunction(confirmModalOptions.onHideModal)) {
            return confirmModalOptions.onHideModal(event, customConfirmModalInstance);
        } else {
            return customConfirmModalInstance.removeModal(false);
        }
    });

    templateHtml.on("show.bs.modal", (event) => {
        if (confirmModalOptions && confirmModalOptions.onShowModal && _.isFunction(confirmModalOptions.onShowModal)) {
            return confirmModalOptions.onShowModal(event, customConfirmModalInstance);
        }
    });

    if (modalOptions) {
        customConfirmModalInstance.instance.modal(modalOptions);
    } else {
        customConfirmModalInstance.instance.modal();
    }

    return customConfirmModalInstance;
}

function customConfirmModalCreateButton(htmlText, htmlClass, htmlStyle, callback) {
    return {
        text: htmlText,
        class: htmlClass,
        style: htmlStyle,
        callback: callback
    }
}

function create_UUID() {
    var dt = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (dt + Math.random() * 16) % 16 | 0;
        dt = Math.floor(dt / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
    return uuid;
}

const customConfirmModalHelper = {
    CreateModal: customConfirmModal,
    CreateModalAsync: customConfirmModalAsync,
    CreateButton: customConfirmModalCreateButton
};

