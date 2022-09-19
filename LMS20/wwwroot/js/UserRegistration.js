$(function () {

    var registerUserButton = $("#addstudent button[name = 'registrera']").click(onUserRegisterClick);

    function onUserRegisterClick()
    {
        var url = "UserAuth/RegisterUser";

        var antiForgeryToken = $("#addstudent input[name = '__RequestVerificationToken']").val();
        var email = $("#addstudent input[name = 'Email']").val();
        var password = $("#addstudent input[name = 'Password']").val();
        var confirmPassword = $("#addstudent input[name = 'ConfirmPassword']").val();
        var firstName = $("#addstudent input[name = 'FirstName']").val();
        var lastName = $("#addstudent input[name = 'LastName']").val();

        var user = {

            __RequestAntiForgeryToken: antiForgeryToken,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword,
            FirstName: firstName,
            LastName: lastName,
            AcceptUserAgreement: true
        };

        $.ajax({
            type: "POST",
            url: url,
            data: user,
            
            
            success: function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name = 'RegistrationInValid']").val() == 'true';

                if (hasErrors) {
                    $("#addstudent").html(data);
                    var registerUserButton = $("#addstudent button[name = 'registrera']").click(onUserRegisterClick);
                    

                    $("#addstudentform").removeData("validator");
                    $("#addstudentform").removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse("#addstudentform");
                }
                else
                {
                    location.href = '/Courses/Index';
                }

            },

            error: function (xhr, ajaxOptions,thrownError)
            {
                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }

        });
    }
});