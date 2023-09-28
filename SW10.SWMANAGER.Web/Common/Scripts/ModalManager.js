var app = app || {};
(function ($) {

    var _loadedScripts = [];

    app.modals = app.modals || {};

    app.ModalManager = (function () {

        var _normalizeOptions = function (options) {
            if (!options.modalId) {
                options.modalId = 'Modal_' + (Math.floor((Math.random() * 1000000))) + new Date().getTime();
            }
        }

        function _removeContainer(modalId) {
            var _containerId = modalId + 'Container';
            var _containerSelector = '#' + _containerId;

            var $container = $(_containerSelector);
            if ($container.length) {
                $container.remove();
            }
        };

        function _createContainer(modalId) {
            _removeContainer(modalId);

            var _containerId = modalId + 'Container';
            return $('<div id="' + _containerId + '"></div>')
                .append(
                    '<div id="' + modalId + '" class="modal fade" tabindex="-1" role="modal" aria-hidden="true">' +
                    '  <div class="modal-dialog">' +
                    '    <div class="modal-content">' +
                    '   </div>' +
                    '  </div>' +
                    '</div>'
                ).appendTo('body');
        }

        return function (options) {

            _normalizeOptions(options);

            var _options = options;
            var _$modal = null;
            var _modalId = options.modalId;
            var _modalSelector = '#' + _modalId;
            var _modalObject = null;

            var _publicApi = null;
            var _args = null;
            var _getResultCallback = null;

            var _onCloseCallbacks = [];

            function _saveModal() {
                if (_modalObject && _modalObject.save) {
                    _modalObject.save();
                }
            }

            function _initAndShowModal(args) {
                _$modal = $(_modalSelector);
                if(args && args.hasOwnProperty('blockClose') && args.blockClose) {
                    debugger;
                    _$modal.modal({
                        backdrop: 'static',
                        keyboard: false
                    });    
                } else {
                    _$modal.modal({
                        backdrop: 'static'
                    });
                }

                _$modal.on('hidden.bs.modal', function () {
                    _removeContainer(_modalId);

                    for (var i = 0; i < _onCloseCallbacks.length; i++) {
                        _onCloseCallbacks[i]();
                    }
                });

                _$modal.on('show.bs.modal', function () {
                    abp.event.trigger(`modal::${options.modalClass}-show.bs.modal`);
                })

                _$modal.on('shown.bs.modal', function () {
                    if (options.focusFunction && _.isFunction(options.focusFunction)) {
                        options.focusFunction(_$modal);
                    }
                    else {
                        _$modal.find('input:not([type=hidden]):first').focus();
                    }
                    abp.event.trigger(`modal::${options.modalClass}-shown.bs.modal`);
                });

                var modalClass = app.modals[options.modalClass];
                if (modalClass) {
                    _modalObject = new modalClass();
                    if (_modalObject.init) {
                        _modalObject.init(_publicApi, _args);
                    }
                }

                _$modal.find('.save-button').click(function () {
                    _saveModal();
                });

                //_$modal.find('.modal-body').keydown(function (e) {
                //    if (e.which == 13) {
                //        e.preventDefault();
                //        _saveModal();
                //    }
                //});

                _$modal.modal('show');
            };

            var _open = function (args, getResultCallback) {

                _args = args || {};
                _getResultCallback = getResultCallback;

                _createContainer(_modalId)
                    .find('.modal-content')
                    .load(options.viewUrl, _args, function (response, status, xhr) {
                        if (status == "error") {
                            abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                            return;
                        };

                        if (options.scriptUrl) {
                            $.getScript(options.scriptUrl)
                                .done(function (script, textStatus) {
                                    _loadedScripts.push(options.scriptUrl);
                                    _initAndShowModal(args);
                                })
                                .fail(function (jqxhr, settings, exception) {
                                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                                });
                        } else {
                            _initAndShowModal(args);
                        }
                    });
            };

            var _close = function () {
                if (!_$modal) {
                    return;
                }
                _$modal.modal('hide');
            };

            var _onClose = function (onCloseCallback) {
                _onCloseCallbacks.push(onCloseCallback);
            }

            function _setBusy(isBusy) {
                if (!_$modal) {
                    return;
                }

                _$modal.find('.modal-footer button').buttonBusy(isBusy);
            }

            _publicApi = {

                open: _open,

                reopen: function () {
                    _open(_args);
                },

                close: _close,

                getModalId: function () {
                    return _modalId;
                },

                getModal: function () {
                    return _$modal;
                },

                getArgs: function () {
                    return _args;
                },

                getOptions: function () {
                    return _options;
                },

                setBusy: _setBusy,

                setResult: function () {
                    _getResultCallback && _getResultCallback.apply(_publicApi, arguments);
                },

                onClose: _onClose
            }

            return _publicApi;

        };
    })();

})(jQuery);