const linkColor = document.getElementById('usersLink')
document.querySelectorAll('.nav_link').forEach(l => l.classList.remove('active'));
linkColor.classList.add('active');


if (!localStorage.getItem('user')) window.href.location = '/Account/Index';

const appHeaders = {
    'content-type': 'application/json',
    'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`,
    'Page': 'Usuarios'
};

const loadMap = function () {
    mapboxgl.accessToken = 'pk.eyJ1IjoiZXJseW4yMyIsImEiOiJja2Q4NnFtYmkwMW5jMzRzZ3N0aTEwZWEzIn0.cO65NFyyEyHN8OSn-9uNYw';

    const map = new mapboxgl.Map({
        container: 'userMap',
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [-69.929611, 18.483402],
        zoom: 9
    });

    map.addControl(
        new MapboxGeocoder({
            accessToken: mapboxgl.accessToken,
            mapboxgl: mapboxgl
        })
    );
    map.addControl(new mapboxgl.NavigationControl());

    const marker = new mapboxgl.Marker();

    map.on('click', function (event) {
        const coordinates = event.lngLat;

        $("#latitude").val(coordinates.lat);
        $("#longitude").val(coordinates.lng);
        marker.setLngLat(coordinates).addTo(map);
    });
}

loadMap();

$("#openCreateUser").on('click', function () {
    doRequest({
        url: '/Roles/GetRoles',
        method: 'GET',
        data: null,
        headers: {
            'content-type': 'application/json',
            'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`,
            'Page': 'Roles',
            'Operation': 'Obtener'
        },
        successCallback: function (data) {
            let htmlRole = `<option value="" disabled selected>Selecciona un rol</option>`;
            data.forEach(role => {
                htmlRole += `<option value="${role.roleId}">${role.role}</option>`;
            });

            $("#userRole").html(htmlRole);
        },
        errorCallback: function (error) {

        }
    });

    doRequest({
        url: 'https://restcountries.com/v2/all',
        method: 'GET',
        data: null,
        headers: {
            'content-type': 'application/json'
        },
        successCallback: function (data) {
            let countriesHtml = `<option value="" disabled selected>Selecciona un país</option>`;
            data.forEach(country => {
                countriesHtml += `<option value="${country.translations.es}">${country.translations.es}</option>`;
            });
            $("#country").html(countriesHtml);
        },
        errorCallback: function (error) {

        }
    })
});

$("#saveUserBtn").on('click', function () {
    if (!validateOnClick()) {
        $("#saveUserBtn > span").text("Enviando datos, por favor espere...");
        $("#saveUserBtn").prop('disabled', true);
        const newUser = {
            name: formFields.firstName.val(),
            lastName: formFields.lastName.val(),
            identificationDocument: formFields.identificationDocument.val(),
            phone: formFields.phoneNumber.val(),
            email: formFields.email.val(),
            password: formFields.password.val(),
            confirmPassword: formFields.confirmPassword.val(),
            photo: $("#sendPhoto").val(),
            address: {
                country: formFields.country.val(),
                city: formFields.city.val(),
                province: formFields.province.val(),
                street1: formFields.street1.val(),
                street2: $("#street2").val(),
                houseNumber: formFields.houseNumber.val(),
                latitude: formFields.latitude.val(),
                longitude: formFields.longitude.val()
            },
            userRole: {
                roleId: formFields.userRole.val()
            }
        };

        doRequest({
            url: '/Users/SaveUser',
            method: 'POST',
            data: newUser,
            headers: { ...appHeaders, 'Operation': 'Agregar' },
            successCallback: function (data) {
                onSuccessSaveUser(data);
            },
            errorCallback: function (err) {
                onError(err);
            }
        });
    } 
});


const onSuccessSaveUser = function (data) {
    $("#saveUserBtn > span").text("Guardar usuario");
    $("#saveUserBtn").prop('disabled', false);
    if (Array.isArray(data)) {
        $("#errorMessages").html("");
        for (let validationObject of data) {
            for (let errorValidation of validationObject.errors) {
                let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${errorValidation.errorMessage}</p>`;
                $("#errorMessages").append(errorMsg);
                $("#CreateUserModal").modal('hide');

                setTimeout(function () {
                    $("#ErrorMessagesModal").modal('show');
                }, 1000);
            }
        }
    } else {
        $("form").eq(0).trigger('reset');
        $("#uploadedPhoto").prop('src', '');
        for (let field in formFields) {
            formFields[field].removeClass('is-valid');
        }
        $("#errorMessages").html("");
        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Usuario registrado correctamente, ya puedes iniciar sesión</p>`;
        $("#errorMessages").append(successMsg);
        $("#CreateUserModal").modal('hide');

        setTimeout(function () {
            $("#ErrorMessagesModal").modal('show');
        }, 1000);
    }
}

const onError = function (err) {
    $("#saveUserBtn > span").text("Guardar usuario");
    $("#saveUserBtn").prop('disabled', false);
    $("#errorMessages").html("");
    let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>${err.responseText}</p>`;
    $("#errorMessages").append(errorMsg);
    $("#CreateUserModal").modal('hide');

    setTimeout(function () {
        $("#ErrorMessagesModal").modal('show');
    }, 1000);
}

$("#userPhoto").on('change', function (e) {
    getBase64File(e.target.files[0]);
});

function getBase64File(file) {
    const reader = new FileReader();
    reader.readAsDataURL(file);

    reader.onload = function () {
        let base64 = reader.result;

        $("#sendPhoto").val(base64);
        $("#uploadedPhoto").prop('src', base64);
    }

    reader.onerror = function (error) {
        console.log(error);
    }
}

const getUsers = function () {
    doRequest({
        url: '/Users/GetUsers',
        method: 'GET',
        data: null,
        headers: { ...appHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) { onGetUsers(data); },
        errorCallback: function (err) { onErrorGetUsers(err) }
    });
}

const onGetUsers = function (data) {
    $("#paginator-container").pagination({
        dataSource: data,
        pageSize: 5,
        callback: function (data, pagination) {
            const html = templateUsersList(data);
            $("#usersList").html(html);
            $("#loadingUsers").css({ 'display': 'none' });
            $("#usersTable").removeClass('d-none');
        }
    });
}

const templateUsersList = (users) => {

    const tBody = $("#usersList");
    tBody.html("");
    let html = "";
    users.forEach(user => {
        html += `<tr>
                    <td>${user.id}</td>
                    <td>${user.name}</td>
                    <td>${user.email}</td>
                    <td>${user.userRole.role.role}</td>
                    <td>
                        <button type="button" id="editUserBtn-${user.id}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></button>
                        <button type="button" class="btn btn-sm btn-danger" onclick="deleteRole(${user.id})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
    });
    return html;
}

const onErrorGetUsers = function (err) {

}

getUsers();