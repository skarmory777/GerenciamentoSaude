var abp = abp || {};
(function ($) {
    if (!sweetAlert || !$) {
        return;
    }

    /* DEFAULTS *************************************************/

    abp.libs = abp.libs || {};
    abp.libs.sweetAlert = {
        config: {
            'default': {

            },
            info: {
                type: 'info'
            },
            success: {
                type: 'success'
            },
            warn: {
                type: 'warning'
            },
            error: {
                type: 'error'
            },
            confirm: {
                type: 'warning',
                title: 'Are you sure?',
                showCancelButton: true,
                cancelButtonText: 'Cancel',
                confirmButtonColor: "#DD6B55",
                confirmButtonText: 'Yes'
            }
        }
    };

    /* MESSAGE **************************************************/

    var showMessage = function (type, message, title, enableHtml, callback) {
        if (!title) {
            title = message;
            message = undefined;
        }
        enableHtml = enableHtml || false;
        var content = { title: title,  html:enableHtml };
        
        if(!enableHtml) {
            content.text = message;
        } else {
            content.html = message;
        }
        
        var opts = $.extend(
            abp.libs.sweetAlert.config.default,
            abp.libs.sweetAlert.config[type],
            content
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts, function () {
                callback && callback();
                $dfd.resolve();
            });
        });
    };

    abp.message.info = function (message, title, enableHtml) {
        return showMessage('info', message, title,enableHtml);
    };

    abp.message.success = function (message, title,enableHtml, callback) {
        return showMessage('success', message, title,enableHtml, callback);
    };

    abp.message.warn = function (message, title,enableHtml) {
        return showMessage('warn', message, title,enableHtml);
    };

    abp.message.error = function (message, title,enableHtml) {
        return showMessage('error', message, title,enableHtml);
    };

    abp.message.confirm = function (message, titleOrCallback, callback, enableHtml) {
        var userOpts = {
            text: message
        };

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            {html:enableHtml},
            abp.libs.sweetAlert.config.default,
            abp.libs.sweetAlert.config.confirm,
            userOpts
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts, function (isConfirmed) {
                callback && callback(isConfirmed);
                $dfd.resolve(isConfirmed);
            });
        });
    };

    abp.message.confirmHtml = function (message, titleOrCallback, callback) {
        
        var userOpts = {
            text: message,
            html:true
        };

        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };
        
        var opts = $.extend(
            {},
            abp.libs.sweetAlert.config.default,
            abp.libs.sweetAlert.config.confirm,
            userOpts
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts, function (isConfirmed) {
                callback && callback(isConfirmed);
                $dfd.resolve(isConfirmed);
            });
        });
    };

    abp.message.customConfirm = function (message, titleOrCallback, callback, options) {
        var userOpts =$.extend({},{text: message}, options);
        if ($.isFunction(titleOrCallback)) {
            callback = titleOrCallback;
        } else if (titleOrCallback) {
            userOpts.title = titleOrCallback;
        };

        var opts = $.extend(
            abp.libs.sweetAlert.config.default,
            abp.libs.sweetAlert.config.confirm,
            userOpts
        );

        return $.Deferred(function ($dfd) {
            sweetAlert(opts, function (isConfirmed) {
                callback && callback(isConfirmed);
                $dfd.resolve(isConfirmed);
            });
        });
    };

    abp.event.on('abp.dynamicScriptsInitialized', function () {
        abp.libs.sweetAlert.config.confirm.title = abp.localization.abpWeb('AreYouSure');
        abp.libs.sweetAlert.config.confirm.cancelButtonText = abp.localization.abpWeb('Cancel');
        abp.libs.sweetAlert.config.confirm.confirmButtonText = abp.localization.abpWeb('Yes');
    });

})(jQuery);