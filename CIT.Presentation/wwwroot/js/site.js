// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


// Write your JavaScript code.


function getStartedSession() {
    const user = JSON.parse(localStorage.getItem('user'));

    if (user)
        window.location.href = '/Dashboard/Index';
}

getStartedSession();


$("#password").on('keyup', function () {
    doRequest({
        url: '/Account/GetLenderBusinessByUser',
        method: 'POST',
        data: { email: $("#email").val(), password: $("#password").val(), lenderBusinessId: 0 },
        headers: null,
        successCallback: function (data) {
            $("#lenderBusinessIdContainer").removeClass('d-none');
            data.forEach(lenderBusiness => {
                const option = document.createElement('option');
                option.value = lenderBusiness.id;
                option.text = lenderBusiness.businessName;
                document.getElementById("lenderBusinessId").appendChild(option);
            });
        },
        errorCallback: function (error) {
            $("#lenderBusinessIdContainer").addClass('d-none');
            $("#lenderBusinessId").html(`<option value="0" selected disabled>Selecciona un negocio...</option>`);
        }
    })
});

$("#signInBtn").on('click', function (e) {
    const authModel = {
        email: $("#email").val(),
        password: $("#password").val(),
        lenderBusinessId: $("#lenderBusinessId").val()
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

const loadCountries = function () {
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
    });
}

loadCountries();

$("#registerBtn").on('click', function () {
    if (!validateOnClick()) {
        const newUser = {
            businessName: $("#businessName").val(),
            rnc: $("#rnc").val(),
            phone: $("#phone").val(),
            email: $("#email").val(),
            password: $("#password").val(),
            confirmPassword: $("#confirmPassword").val(),
            photo: $("#photoBase64").val(),
            address: {
                country: $("#country").val(),
                city: $("#city").val(),
                province: $("#province").val(),
                street1: $("#street1").val(),
                street2: $("#street2").val(),
                houseNumber: $("#houseNumber").val(),
                latitude: $("#latitude").val(),
                longitude: $("#longitude").val()
            }
        };

        doRequest({
            url: '/Account/Register', method: 'POST', data: newUser, headers: null,
            successCallback: function (data) {
                if (Array.isArray(data)) {
                    $("#errorMessages").html("");
                    for (let error of data) {
                        let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${error}</p>`;
                        $("#errorMessages").append(errorMsg);
                        $("#LoadingModal").modal('hide');

                        setTimeout(function () {
                            $("#ErrorMessagesModal").modal('show');
                        }, 1000);
                    }
                } else {
                    $("form").eq(0).trigger('reset');
                    $("#errorMessages").html("");
                    let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Negocio registrado correctamente, por favor, verificar su correo electrónico para activar su cuenta</p>`;
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
    }
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