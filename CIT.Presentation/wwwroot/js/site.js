// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#signInBtn").on('click', function (e) {
    const authModel = {
        email: $("#email").val(),
        password: $("#password").val()
    };

    $.ajax('/Account/Index', {
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(authModel),
        success: function (data) {
            console.log(data);
        },
        error: function (err) {
            console.log(err);
        }
    });
});