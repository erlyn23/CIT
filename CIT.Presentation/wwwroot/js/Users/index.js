getUserPages('usuariosLink');
getUserPermissions(2);
let map;

if (!localStorage.getItem('user')) window.href.location = '/Account/Index';

const appHeaders = {
    'content-type': 'application/json',
    'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`,
    'Page': 'Usuarios'
};


const loadViewMap = function (lat, lng) {
    mapboxgl.accessToken = 'pk.eyJ1IjoiZXJseW4yMyIsImEiOiJja2Q4NnFtYmkwMW5jMzRzZ3N0aTEwZWEzIn0.cO65NFyyEyHN8OSn-9uNYw';

    const userViewMap = new mapboxgl.Map({
        container: 'userViewMap',
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [lng, lat],
        zoom: 9
    });

    userViewMap.addControl(
        new MapboxGeocoder({
            accessToken: mapboxgl.accessToken,
            mapboxgl: mapboxgl
        })
    );
    userViewMap.addControl(new mapboxgl.NavigationControl());

    const marker = new mapboxgl.Marker();
    marker.setLngLat([lng, lat]).addTo(userViewMap);
}


const loadMap = function () {
    mapboxgl.accessToken = 'pk.eyJ1IjoiZXJseW4yMyIsImEiOiJja2Q4NnFtYmkwMW5jMzRzZ3N0aTEwZWEzIn0.cO65NFyyEyHN8OSn-9uNYw';

    map = new mapboxgl.Map({
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

const getModalData = function () {
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
            let htmlRole = "";
            if ($("#userId").val() === "" || $("#userId").val() === null)
                htmlRole = `<option value="0" disabled selected>Selecciona un rol</option>`;
            
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
            let countriesHtml = "";
            if ($("#userId").val() === "" || $("#userId").val() === null)
                countriesHtml = `<option value="0" disabled selected>Selecciona un rol</option>`;
            data.forEach(country => {
                countriesHtml += `<option value="${country.translations.es}">${country.translations.es}</option>`;
            });
            $("#country").html(countriesHtml);
        },
        errorCallback: function (error) {

        }
    });
}
$("#openCreateUser").on('click', function () {
    getModalData();
});

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
        errorCallback: function (err) { onErrorHandler(err) }
    });
}

const onGetUsers = function (data) {
    if (!hasAdd) {
        $("#openCreateUser").remove();
        $("#CreteUserModal").remove();
    }
    if (hasGet) {
        $("#paginator-container").pagination({
            dataSource: data,
            pageSize: 5,
            callback: function (data, pagination) {
                const html = templateUsersList(data);
                $("#usersList").html(html);
                setEditUserEvent(data);
                $("#loadingUsers").css({ 'display': 'none' });
                $("#usersTable").removeClass('d-none');
            }
        });
    }
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
                        <button type="button" id="setUserDetailBtn-${user.id}" class="btn btn-sm btn-success"><i class="fas fa-eye"></i></button>
                        <button type="button" id="editUserBtn-${user.id}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></button>
                        <button type="button" id="deleteUserBtn-${user.id}" class="btn btn-sm btn-danger" onclick="deleteUser(${user.id})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
    });
    return html;
}

const setUserData = function (user) {
    $("#createUserTitle").text("Editar usuario");

    $("#userId").val(user.id);

    if (user.photo != "NULL") {
        $("#uploadedPhoto").prop('src', user.photo);
        $("#sendPhoto").val(user.Photo);
    }
    getModalData();
    $("#firstName").val(user.name);
    $("#lastName").val(user.lastName);
    $("#identificationDocument").val(user.identificationDocument);
    $("#email").val(user.email);
    $("#phoneNumber").val(user.phone);
    $("#addressId").val(user.address.id);
    $(`#country option[value="${user.address.country}"]`).attr('selected', true);
    $("#city").val(user.address.city);
    $("#province").val(user.address.province);
    $("#street1").val(user.address.street1);
    $("#street2").val(user.address.street2);
    $("#houseNumber").val(user.address.houseNumber);
    $("#latitude").val(user.address.latitude);
    $("#longitude").val(user.address.longitude);
    const marker = new mapboxgl.Marker();

    marker.setLngLat([user.address.longitude, user.address.latitude]).addTo(map);
    $(`#userRole option[value="${user.userRole.roleId}"]`).attr('selected', true);
    $("#CreteUserModal").modal('show');
}

const setUserDetailData = function (user) {
    $("#userPhotoView").html(`<img src="/ProfilePhotos/user_profile_photo_${user.id}.jpg" alt="Foto del usuario" width="200" height="200" />`);
    $("#userIdView").text(user.id);
    $("#userNameView").text(`${user.name} ${user.lastName}`);
    $("#identificationDocumentView").text(user.identificationDocument);
    $("#emailView").text(user.email);
    $("#phoneView").text(user.phone);
    $("#countryView").text(user.address.country);
    $("#cityView").text(user.address.city);
    $("#street1View").text(user.address.street1);
    $("#street2View").text(user.address.street2);
    $("#provinceView").text(user.address.province);
    loadViewMap(user.address.latitude, user.address.longitude);
    $("#houseNumberView").text(user.address.houseNumber);
    $("#roleView").text(user.userRole.role.role);
    $("#UserModalDetail").modal('show');
}

const setEditUserEvent = function (users) {
    users.forEach(function (user) {

        if (hasEdit) {
            $("#editUserBtn-" + user.id).on('click', function () {
                setUserData(user);
            });
        } else
            $("#editUserBtn-" + user.id).remove();

        if (hasGet) {

            $("#setUserDetailBtn-" + user.id).on('click', function () {
                setUserDetailData(user);
            });

        } else $("#setUserDetailBtn-" + user.id).remove();

        if (!hasDelete)
            $("#deleteUserBtn-" + user.id).remove();

    });
}

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

        if ($("#userId").val() !== "") {
            newUser.id = $("#userId").val();
            newUser.address.id = $("#addressId").val();
        }

        let url = ($("#userId").val() !== "") ? '/Users/UpdateUser' : '/Users/SaveUser';
        let operation = ($("#userId").val() !== "") ? 'Modificar' : 'Agregar';
        doRequest({
            url: url,
            method: 'POST',
            data: newUser,
            headers: { ...appHeaders, 'Operation': operation },
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
        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Usuario guardado correctamente</p>`;
        $("#errorMessages").append(successMsg);
        $("#CreateUserModal").modal('hide');

        setTimeout(function () {
            $("#ErrorMessagesModal").modal('show');
        }, 1000);
        getUsers();
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

const deleteUser = function(userId){
    $("#DeleteConfirmUserModal").modal('show');

    $("#deleteUserBtn").on('click', function () {
        doRequest(
            {
                url: '/Users/DeleteUser/' + userId,
                method: 'GET',
                data: null,
                headers: { ...appHeaders, 'Operation': 'Eliminar' },
                successCallback: function (data) {
                    $("#DeleteConfirmUserModal").modal('hide');
                    alert(data);
                    getUsers();
                },
                errorCallback: function (error) { onErrorHandler(error) }
            }
        );
    });
}


const onErrorHandler = function (err) {
    $("#loadingUsers").addClass("d-none");
    $("#errorMessage").removeClass("d-none");
    $("#errorMessage").html("<p>Ha ocurrido un error al obtener usuarios: "+ err.responseText +"</p>")
}

getUsers();