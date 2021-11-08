// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


function getStartedSession() {
    const user = JSON.parse(localStorage.getItem('user'));

    if (user)
        window.location.href = '/Dashboard/Index';
}

getStartedSession();


$("#signInBtn").on('click', function (e) {
    const authModel = {
        email: $("#email").val(),
        password: $("#password").val()
    };

    doRequest({
        url: '/Account/Index', method: 'POST', data: authModel, headers: null,
        successCallback: function(data) {

            $("#errorMessages").html("");
            let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Sesión iniciada</p>`;
            $("#errorMessages").append(successMsg);
            $("#LoadingModal").modal('hide');

            setTimeout(function () {
                $("#ErrorMessagesModal").modal('show');
                localStorage.setItem('user', JSON.stringify(data));
                window.location.href = '/Dashboard/Index';
            }, 1000);

        },
        errorCallback: function(err) {


            $("#errorMessages").html("");
            let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${err.responseText}</p>`;
            $("#errorMessages").append(errorMsg);
            $("#LoadingModal").modal('hide');
            setTimeout(function () {
                $("#ErrorMessagesModal").modal('show');
            }, 1000);


        }
    });
});

$("#registerBtn").on('click', function () {

    const newUser = {
        businessName: $("#businessName").val(),
        rnc: $("#Rnc").val(),
        phone: $("#phone").val(),
        email: $("#email").val(),
        password: $("#password").val(),
        confirmPassword: $("#confirmPassword").val(),
        photo: $("#photoBase64").val()
    };

    doRequest({
            url: '/Account/Register', method: 'POST', data: newUser, headers: null,
                successCallback: function (data) {
                    if (Array.isArray(data)) {
                        $("#errorMessages").html("");
                        for (let validationObject of data) {
                            for (let errorValidation of validationObject.errors) {
                                let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${errorValidation.errorMessage}</p>`;
                                $("#errorMessages").append(errorMsg);
                                $("#LoadingModal").modal('hide');

                                setTimeout(function () {
                                    $("#ErrorMessagesModal").modal('show');
                                }, 1000);
                            }
                        }
                    } else {
                        $("form").eq(0).trigger('reset');
                        $("#errorMessages").html("");
                        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Usuario registrado correctamente, ya puedes iniciar sesión</p>`;
                        $("#errorMessages").append(successMsg);
                        $("#LoadingModal").modal('hide');

                        setTimeout(function () {
                            $("#ErrorMessagesModal").modal('show');
                        }, 1000);
                    }
            },
            errorCallback: function (err) {
                $("#errorMessages").html("");
                let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>${err.responseText}</p>`;
                $("#errorMessages").append(errorMsg);
                $("#LoadingModal").modal('hide');
                
                setTimeout(function () {
                    $("#ErrorMessagesModal").modal('show');
                }, 1000);

            }
        }
    );
});

$("#photo").on('change', function (e) {
    getBase64File(e.target.files[0]);
});

function getBase64File(file) {
    const reader = new FileReader();
    reader.readAsDataURL(file);

    reader.onload = function () {
        let base64 = reader.result;

        $("#photoBase64").val(base64);
    }

    reader.onerror = function (error) {
        console.log(error);
    }
}