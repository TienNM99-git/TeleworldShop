
const userInfo = {
    init: function () {
        userInfo.loadData();
        userInfo.registerEvent();
    },
    loadData: function () {
        $.ajax({
            url: '/User/GetUserInfo',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    user.BirthDay = new Date(parseInt(user.BirthDay.replace("/Date(", "").replace(")/", ""), 10));
                    $('#txtUserId').val(user.Id);
                    $('#txtUserName').val(user.UserName);
                    $('#txtName').val(user.FullName);
                    $('#txtEmail').val(user.Email);
                    $('#txtAddress').val(user.Address);
                    $('#txtPhone').val(user.PhoneNumber);
                    $('#txtBirthday').val(moment(user.BirthDay).format('YYYY-MM-DD'));
                }
            }
        });
    },
    registerEvent: function () {
        $.validator

        $('#frmUpdateUserInfo').validate({
            onkeyup: false,
            onfocusout: function (element) {
                $(element).valid();
            },
            rules: {
                name: "required",
                email: {
                    required: true,
                    email: true
                },
                address: "required",
                phone: {
                    required: true,
                    number: true
                },
                oldPassword: {
                    required: true,
                },
                newPassword: {
                    required: true,
                },
                retypePwd: {
                    equalTo: '[name="newPassword"]',
                }
            },
            messages: {
                oldPassword: "Current password is required",
                newPassword: "New password is required",
                name: "Fullname is required",
                name: "Address is required",
                email: {
                    required: "Email is required",
                    email: "Invalid email address"
                },
                phone: {
                    required: "Phone number is required",
                    number: "Invalid phone number"
                }
            },
            errorElement: 'div',
            errorClass: 'error-message'
        });
        $('#chkUpdatePwd').off('click').on('click', function () {
            const oldPwdInput = `<div class="info-form-control">
                    <label class="input-label">Old password</label>
                    <div>
                        <input type="password" class="bYlDgr" id="txtOldPassword" name="oldPassword" />
                    </div>
                </div>`;
            const newPwdInput = `<div class="info-form-control">
                    <label class="input-label">New password</label>
                    <div>
                        <input type="password" class="bYlDgr" id="txtNewPassword" name="newPassword" />
                    </div>
                </div>`;
            const retypePwdInput = `<div class="info-form-control">
                    <label class="input-label">Retype password</label>
                    <div>
                        <input type="password" class="bYlDgr" id="txtRetypePwd" name="retypePwd" />
                    </div>
                </div>`;
            if ($(this).prop('checked'))
                $('#frmUpdateUserInfo').append(oldPwdInput, newPwdInput, retypePwdInput);
            else {
                $('.info-form-control:last-child').remove();
                $('.info-form-control:last-child').remove();
                $('.info-form-control:last-child').remove();
            }
        });
        $('#btnUpdateUserInfo').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#frmUpdateUserInfo').valid();
            if (isValid) {
                userInfo.updateUserInfo();
            }
        });
    },
    updateUserInfo: function () {
        const newUser = {
            Id: $('#txtUserId').val(),
            UserName: $('#txtUserName').val(),
            FullName: $('#txtName').val(),
            PhoneNumber: $('#txtPhone').val(),
            BirthDay: $('#txtBirthday').val(),
            Address: $('#txtAddress').val(),
            OldPassword: $('#txtOldPassword').val(),
            Password: $('#txtNewPassword').val(),
        }
        $.ajax({
            url: '/User/UpdateUserInfo',
            type: 'POST',
            dataType: 'json',
            data: {
                appUserVm: JSON.stringify(newUser)
            },
            success: function (response) {
                $(".user-info-detail .alert").remove();
                const alert = `<div class="alert"></div>`
                $(".user-info-detail .title").before(alert);
                if (response.status) {
                    $('#txtOldPassword').val('');
                    $('#txtNewPassword').val('');
                    $('#txtRetypePwd').val('');
                    if ($("#chkUpdatePwd").prop("checked")) {
                        $("#chkUpdatePwd").trigger("click");
                    }
                    $(".user-info-detail .alert").removeClass("fail-alert");
                    $(".user-info-detail .alert").addClass("success-alert");
                    $(".user-info-detail .alert").html("Your profile has been update successfully");
                }
                else {
                    if (response.message === 'WrongPwd') {
                        const wrondPwd = `<div id="txtOldPassword-error" class="error-message">Wrong password</div>`
                        $("#txtOldPassword").focus();
                        $("#txtOldPassword").parent().append(wrondPwd);
                    }
                    $(".user-info-detail .alert").removeClass("success-alert");
                    $(".user-info-detail .alert").addClass("fail-alert");
                    $(".user-info-detail .alert").html("Can not update your profile");
                }
            }
        });
    }
}

userInfo.init();