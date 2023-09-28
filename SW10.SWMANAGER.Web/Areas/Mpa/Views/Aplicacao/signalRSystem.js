(function($) {
    let uuid = create_UUID();
    const notification =  abp.services.app.notification;
    getUserNotificationFromAppService();
    return;
    // Descomentar quando o signalR voltar a funcionar - Migrar versão abp
    // abp.event.on('app.notification.connected', getUserNotification );
    // //Check if SignalR is defined
    // if (!$ || !$.connection) {
    //     return;
    // }
    // //Create namespaces
    // app.signalr = app.signalr || {};
    // app.signalr.hubs = app.signalr.hubs || {};
    //
    // //Get the chat hub
    // app.signalr.hubs.notification = app.signalr.hubs.notification || $.connection.notificationHub;
    //
    // const notificationHub = app.signalr.hubs.notification;
    // if (!notificationHub) {
    //     return;
    // }
    //
    // let uuid = create_UUID();
    //
    // $.connection.hub.stateChanged(function (data) {
    //     if (data.newState === $.connection.connectionState.connected) {
    //         abp.event.trigger('app.notification.connected');
    //     }
    // });
    //
    // notificationHub.client.sendNotificationToOnline = function (result) {
    //     debugger
    //     getUserNotification();
    // }
    //
    // notificationHub.client.updateNotificationToOnline = function(result) {
    //     debugger
    //     getUserNotification();
    // }
    
    function createOrUpdateModal(resultData) {
        if(!uuid) {
            uuid = create_UUID();
        }
        const modalDialog = $(`.custom-user-notification-modal.${uuid}`);
        if(!resultData.hasNotification && modalDialog.length) {
            modalDialog.modal("hide");
        }
        else if(resultData.hasNotification && modalDialog.length) {
            updateModal(resultData.userNotifications);
        }
        else if(resultData.hasNotification) {
            openModal(resultData.userNotifications);
        }
    }
    
    function updateModal(userNotifications) {
        debugger;
        let content = createContent(userNotifications);
        const modalDialog = $(`.custom-user-notification-modal.${uuid}`);
        
        modalDialog.find(".modal-dialog").hide();
        modalDialog.find('.carousel').remove();
        
        modalDialog.find(`.btn-user-notification-message`).off('click', onBtnUserNotificationMessage);
        modalDialog.find(".modal-body").html(content.html());
        
        modalDialog.find('.carousel').carousel({interval: false, keyboard: false});
        modalDialog.find(`.btn-user-notification-message`).on('click', onBtnUserNotificationMessage);
        modalDialog.find(".modal-dialog").show();
    }
    
    function openModal(userNotifications) {
        let content = createContent(userNotifications);
        
        let modal = $(`
            <div class=" custom-user-notification-modal ${uuid} modal fade"  tabindex="-1" role="dialog" aria-hidden="true">
              <div class="modal-dialog" role="document" style="min-height: 600px; min-width: 60vw">
                <div class="modal-content">
                    <div class="modal-header" style="min-height: auto;padding: 15px 10px;margin: 0 !important;">
                        <h4 class="modal-title">Notificações </h4>
                    </div>
                  <div class="modal-body" style="margin: 0 !important;padding: 0 !important;">
                    ${content.html()}
                  </div>
                </div>
              </div>
            </div>`);

        $("body").append(modal);
        $(`.${uuid}`).modal({backdrop: 'static', keyboard: false})
            .on('shown.bs.modal', function () {
                const audio = new Audio("/Areas/Mpa/Common/swiftly-610.mp3")
                audio.play();
                const modalDialog = $(`.custom-user-notification-modal.${uuid}`);
                
                modalDialog.find('.carousel').carousel({interval: false, keyboard: false});
                modalDialog.find(`.btn-user-notification-message`).on('click', onBtnUserNotificationMessage);
            })
            .on('hidden.bs.modal', function (e) {
                const modalDialog = $(`.custom-user-notification-modal.${uuid}`);
                modalDialog.remove();
            });
    }

    function createContent(userNotifications) {
        let groups = _.groupBy(userNotifications, "source");
        let content = "";
        const keyGroups = _.keys(groups);
        if (keyGroups.length == 1) {
            content = contentOneGroup(groups[keyGroups[0]]);
        } else {
            content = contentMoreThanOneGroup(groups);
        }
        return content;
    }
    
    function onBtnUserNotificationMessage() {
        const btn = $(this);
        const carrouselItem = $(this).parents(".item");
        const carrousel = $(this).parents(".carousel");
        const modalDialog = $(`.custom-user-notification-modal.${uuid}`);
        btn.buttonBusy(true);
        //$.connection.notificationHub.server.setNotificationRead({id: btn.data("id")})
        notification.setUserNotificationRead(btn.data("id"))
            .then(() => {
                if (carrouselItem.data("index") < carrousel.data("total")) {
                    modalDialog.find('.carousel').carousel('next');
                } else {
                    modalDialog.modal("hide");
                }
                abp.notify.success(`Aviso lido`);
            })
            .fail(() => {
                abp.notify.fail(`Não foi possível executar a ação. Favor entrar em contato com o suporte`);
            })
            .always(() => {
                btn.buttonBusy(false);
            })
    }

    function contentOneGroup(group) {
        return createCarrousel(group);
    }

    function createCarrousel(group) {
        if (!group || !group.length) {
            return $("<div></div>");
        }
        const uuId = create_UUID();
        let li = ``;
        let item = ``;

        _.forEach(group, (groupItem, index) => {
            li += `<li data-target=".${uuId}" data-slide-to="${index}" class="${(index == 0 ? 'active' : '')}" style="display: none"></li>`;
            item += `
                <div class="item ${(index == 0 ? 'active' : '')}" data-index="${index}" style="background-color: #f5f5f5; ">
                    <div class="row">
                      <div class="col-md-12"> <h3 class="text-center font-weight-bold">${groupItem.title}</h3> </div>
                      <div class="col-md-12 text-center" style="margin-top: 30px; margin-bottom: 30px"> <span >${groupItem.message}</span> </div>
                      <div class="col-md-12 text-center" style="margin-top: 30px; margin-bottom: 30px"> <button type="button" class="btn btn-lg btn-danger text-center btn-user-notification-message" data-id="${groupItem.id}">OK</button></div>
                    </div>
                </div>`;
        })
        return $(`
            <div>
                <div class="carousel slide ${uuId}" data-ride="carousel" data-total="${group.length - 1}">
                    <ol class="carousel-indicators"> ${li} </ol>
                    <div class="carousel-inner"> ${item} </div>
                </div>
            </div>
            `);
    }

    function contentMoreThanOneGroup(groups) {

    }
    
    function create_UUID(){
        var dt = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
            var r = (dt + Math.random()*16)%16 | 0;
            dt = Math.floor(dt/16);
            return (c=='x' ? r :(r&0x3|0x8)).toString(16);
        });
        return uuid;
    }
    
    function getUserNotification() {
        notificationHub.server.getUserNotification().then((resultData) => {
            createOrUpdateModal(resultData);
        });
    }

    function getUserNotificationFromAppService() {
        notification.getUserNotification().then((resultData) => {
            createOrUpdateModal(resultData);
        });
    }
    
})(jQuery);