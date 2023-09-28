(function () {
    app.modals.CreateOrEditRoleModal = function () {

        var _modalManager;
        var _roleService = abp.services.app.role;
        var _$roleInformationForm = null;
        var _permissionsTree;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            _permissionsTree = new PermissionsTree();
            _permissionsTree.init(_modalManager.getModal().find('.permission-tree'));

            _$roleInformationForm = _modalManager.getModal().find('form[name=RoleInformationsForm]');
            _$roleInformationForm.validate({ ignore: "" });

            $("#EditRole_IsHabilitaControleDeIpSim").change(changeIpControl);
            $("#EditRole_IsHabilitaControleDeIpNao").change(changeIpControl);
            $("#EditRole_IsHabilitaControleDeIpNda").change(changeIpControl);
        };

        function changeIpControl(e) {
            $(".enable-ip-control").find('.md-check').not($(this)).attr("checked",false);
            $(".enable-ip-control").find('.md-check').not($(this)).prop("checked",false);
        }

        this.save = function() {
            if (!_$roleInformationForm.valid()) {
                return;
            }

            var role = _$roleInformationForm.serializeFormToObject();

            role.IsHabilitaControleDeIp = $(".enable-ip-control").find('.md-check:checked').first().val()

            _modalManager.setBusy(true);
            _roleService.createOrUpdateRole({
                role: role,
                grantedPermissionNames: _permissionsTree.getSelectedPermissionNames()
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditRoleModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})();