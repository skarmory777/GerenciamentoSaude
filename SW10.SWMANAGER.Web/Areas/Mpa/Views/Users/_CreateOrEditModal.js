(function ($) {
    app.modals.CreateOrEditUserModal = function () {

        var _userService = abp.services.app.user;

        var _modalManager;
        var _$userInformationForm = null;

        function _findAssignedRoleNames() {
            var assignedRoleNames = [];

            _modalManager.getModal()
                .find('.user-role-checkbox-list input[type=checkbox]')
                .each(function () {
                    if ($(this).is(':checked')) {
                        assignedRoleNames.push($(this).attr('name'));
                    }
                });

            return assignedRoleNames;
        }

        function _findAssignedEmpresas() {
            var assignedEmpresas = [];

            _modalManager.getModal()
                .find('.user-empresa-checkbox-list input[type=checkbox]')
                .each(function () {
                    if ($(this).is(':checked')) {
                        assignedEmpresas.push($(this).attr('name'));
                    }
                });

            return assignedEmpresas;
        }

        this.init = function (modalManager) {

            $('#profissional').attr('required', false);

            _modalManager = modalManager;

            _$userInformationForm = _modalManager.getModal().find('form[name=UserInformationsForm]');
            _$userInformationForm.validate();

            var passwordInputs = _modalManager.getModal().find('input[name=Password],input[name=PasswordRepeat]');
            var passwordInputGroups = passwordInputs.closest('.form-group');

            $('#EditUser_SetRandomPassword').change(function () {
                if ($(this).is(':checked')) {
                    passwordInputGroups.slideUp('fast');
                    if (!_modalManager.getArgs().id) {
                        passwordInputs.removeAttr('required');
                    }
                } else {
                    passwordInputGroups.slideDown('fast');
                    if (!_modalManager.getArgs().id) {
                        passwordInputs.attr('required', 'required');
                    }
                }
            });

            _modalManager.getModal()
                .find('.user-role-checkbox-list input[type=checkbox]')
                .change(function () {
                    $('#assigned-role-count').text(_findAssignedRoleNames().length);
                });


            $("#EditUser_IsHabilitaControleDeIpSim").change(changeIpControl);
            $("#EditUser_IsHabilitaControleDeIpNao").change(changeIpControl);
            $("#EditUser_IsHabilitaControleDeIpNda").change(changeIpControl);
        };
        
        function changeIpControl(e) {
            $(".enable-ip-control").find('.md-check').not($(this)).attr("checked",false);
            $(".enable-ip-control").find('.md-check').not($(this)).prop("checked",false);
        }
        

        this.save = function () {
           
            if (!_$userInformationForm.valid()) {
                return;
            }

            var assignedRoleNames = _findAssignedRoleNames();
            var assignedEmpresas = _findAssignedEmpresas();
            var user = _$userInformationForm.serializeFormToObject();

            if (user.SetRandomPassword) {
                user.Password = null;
            }
            
            user.IsHabilitaControleDeIp = $(".enable-ip-control").find('.md-check:checked').first().val()

            _modalManager.setBusy(true);
            _userService.createOrUpdateUser({
                user: user,
                assignedRoleNames: assignedRoleNames,
                assignedEmpresas: assignedEmpresas,
                sendActivationEmail: user.SendActivationEmail,
                SetRandomPassword: user.SetRandomPassword
            }).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditUserModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);