const formUserFullNameFields = {
    userFullName: $("#userFullName"),
    userId: $("#userId")
};

const validationFullNameStatus = {
    userFullName: false,
    userId: false
};

//Validación nombre del usuario
const validateFullName = function () {
    if (formUserFullNameFields.userFullName.val().length === 0) {
        setInvalid(formUserFullNameFields.userFullName, "#userValidation", "<p>Debes escoger el nombre del usuario</p>");
        validationFullNameStatus.userFullName = true;
    } else {
        setValid(formUserFullNameFields.userFullName, "#userValidation");
        validationFullNameStatus.loanName = false;
    }
}
formUserFullNameFields.userFullName.on(CHANGE_EVENT, validateFullName);
formUserFullNameFields.userFullName.on(FOCUSOUT_EVENT, validateFullName);
//Validación nombre del usuario

formUserFullNameFields.userFullName.on('keyup', function (e) {
    if (e.target.value.length > 0) {
        $("#userSourceContainer").removeClass('d-none');
        doRequest({
            url: `/Users/GetUsersByName/?name=${formUserFullNameFields.userFullName.val()}`,
            method: 'GET',
            data: null,
            headers: appHeaders,
            successCallback: function (data) {
                $("#userSource").html("");
                if (data) {
                    data.forEach(user => {
                        const liElement = document.createElement('li');
                        liElement.addEventListener('click', () => {
                            formUserFullNameFields.userFullName.val(`${user.name} ${user.lastName}`);
                            formUserFullNameFields.userId.val(user.id);
                            $("#userSourceContainer").addClass('d-none');
                        });
                        liElement.innerText = `${user.id} - ${user.name} ${user.lastName}`;
                        $("#userSource").append(liElement);
                    });
                }
            },
            errorCallback: function (err) {
                console.log(err);
            }
        });
    } else
        $("#userSourceContainer").addClass('d-none');
});