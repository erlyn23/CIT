const formFields = {
    firstName: $("#firstName"),
    lastName: $("#lastName"),
    identificationDocument: $("#identificationDocument"),
    phoneNumber: $("#phoneNumber"),
    email: $("#email"),
    country: $("#country"),
    city: $("#city"),
    province: $("#province"),
    street1: $("#street1"),
    houseNumber: $("#houseNumber"),
    latitude: $("#latitude"),
    longitude: $("#longitude"),
    userRole: $("#userRole"),
    password: $("#password"),
    confirmPassword: $("#confirmPassword")
};

const validationsStatus = {
    firstName: false,
    lastName: false,
    identificationDocument: false,
    phoneNumber: false,
    email: false,
    country: false,
    city: false,
    province: false,
    street1: false,
    houseNumber: false,
    latitude: false,
    longitude: false,
    userRole: false,
    password: false,
    confirmPassword: false
};

const CHANGE_EVENT = 'change';
const FOCUSOUT_EVENT = 'focusout';

//Validación Nombre
    const validateFirstName = function () {
        if (formFields.firstName.val().length === 0) {
            setInvalid(formFields.firstName, "#firstNameValidation", "<p>Debes escribir el nombre</p>");
            validationsStatus.firstName = true;
        } else if (formFields.firstName.val().length > 30) {
            setInvalid(formFields.firstName, "#firstNameValidation", "<p>Debes escribir menos de 30 letras</p>");
            validationsStatus.firstName = true;
        } else {
            setValid(formFields.firstName, "#firstNameValidation");
            validationsStatus.firstName = false;
        }
    }

    formFields.firstName.on(CHANGE_EVENT, validateFirstName);
    formFields.firstName.on(FOCUSOUT_EVENT, validateFirstName);
//Validación Nombre


//Validación apellido
    const validateLastName = function () {
        if (formFields.lastName.val().length === 0) {
            setInvalid(formFields.lastName, "#lastNameValidation", "<p>Debes escribir el apellido</p>");
            validationsStatus.lastName = true;
        } else if (formFields.lastName.val().length > 30) {
            setInvalid(formFields.lastName, "#lastNameValidation", "<p>Debes escribir menos de 30 letras</p>");
            validationsStatus.lastName = true;
        } else {
            setValid(formFields.lastName, "#lastNameValidation");
            validationsStatus.lastName = false;
        }
    }

    formFields.lastName.on(CHANGE_EVENT, validateLastName);
    formFields.lastName.on(FOCUSOUT_EVENT, validateLastName);
//Validación apellido


//Validación Cédula
    const validateIdentificationDocument = function () {
        if (formFields.identificationDocument.val().length === 0) {
            setInvalid(formFields.identificationDocument, "#identificationDocumentValidation", "<p>Debes escribir la cédula</p>");
            validationsStatus.identificationDocument = true;
        } else if (formFields.identificationDocument.val().length > 11 || formFields.identificationDocument.val().length < 11) {
            setInvalid(formFields.identificationDocument, "#identificationDocumentValidation", "<p>Solo debes escribir 11 números</p>");
            validationsStatus.identificationDocument = true;
        } else if (isNaN(formFields.identificationDocument.val())) {
            setInvalid(formFields.identificationDocument, "#identificationDocumentValidation", "<p>Solo puedes escribir números</p>");
            validationsStatus.identificationDocument = true;
        } else {
            setValid(formFields.identificationDocument, "#identificationDocumentValidation");
            validationsStatus.identificationDocument = false;
        }
    }

    formFields.identificationDocument.on(CHANGE_EVENT, validateIdentificationDocument);
    formFields.identificationDocument.on(FOCUSOUT_EVENT, validateIdentificationDocument);
//Validación Cédula

//Validación teléfono
    const validatePhoneNumber = function () {
        if (formFields.phoneNumber.val().length === 0) {
            setInvalid(formFields.phoneNumber, "#phoneValidation", "<p>Debes escribir el teléfono</p>");
            validationsStatus.phoneNumber = true;
        } else if (formFields.phoneNumber.val().length > 10 || formFields.phoneNumber.val().length < 10) {
            setInvalid(formFields.phoneNumber, "#phoneValidation", "<p>Solo debes escribir 10 números</p>");
            validationsStatus.phoneNumber = true;
        } else if (isNaN(formFields.phoneNumber.val())) {
            setInvalid(formFields.phoneNumber, "#phoneValidation", "<p>Solo puedes escribir números</p>");
            validationsStatus.phoneNumber = true;
        } else {
            setValid(formFields.phoneNumber, "#phoneValidation");
            validationsStatus.phoneNumber = false;
        }
    }

    formFields.phoneNumber.on(CHANGE_EVENT, validatePhoneNumber);
    formFields.phoneNumber.on(FOCUSOUT_EVENT, validatePhoneNumber);
//Validación teléfono


//Validación correo
    const validateEmail = function () {
        if (formFields.email.val().length === 0) {
            setInvalid(formFields.email, "#emailValidation", "<p>Debes escribir el correo</p>");
            validationsStatus.email = true;
        } else if (formFields.email.val().length > 50) {
            setInvalid(formFields.email, "#emailValidation", "<p>Solo debes escribir menos de 50 letras</p>");
            validationsStatus.email = true;
        } else if (!formFields.email.val().includes("@") && !formFields.email.val().includes(".")) {
            setInvalid(formFields.email, "#emailValidation", "<p>Debes escribir un correo válido</p>");
            validationsStatus.email = true;
        } else {
            setValid(formFields.email, "#emailValidation");
            validationsStatus.email = false;
        }
    }

    formFields.email.on(CHANGE_EVENT, validateEmail);
    formFields.email.on(FOCUSOUT_EVENT, validateEmail);
//Validación correo


//Validación país
    const validateCountry = function () {
        if (formFields.country.val().length === 0 || !$("#countrySourceContainer").hasClass("d-none")) {
            setInvalid(formFields.country, "#countryValidation", "<p>Debes escoger un país</p>");
            validationsStatus.country = true;
        } else {
            setValid(formFields.country, "#countryValidation");
            validationsStatus.country = false;
        }
    }
    formFields.country.on(CHANGE_EVENT, validateCountry);
    formFields.country.on(FOCUSOUT_EVENT, validateCountry);
//Validación País

//Validación ciudad
    const validateCity = function () {
        if (formFields.city.val().length === 0) {
            setInvalid(formFields.city, "#cityValidation", "<p>Debes escribir la ciudad</p>");
            validationsStatus.city = true;
        } else {
            setValid(formFields.city, "#cityValidation");
            validationsStatus.city = false;
        }
    }

    formFields.city.on(CHANGE_EVENT, validateCity);
    formFields.city.on(FOCUSOUT_EVENT, validateCity);
//Validación ciudad

//Validación provincia
    const validateProvince = function () {
        if (formFields.province.val().length === 0) {
            setInvalid(formFields.province, "#provinceValidation", "<p>Debes escribir la provincia</p>");
            validationsStatus.province = true;
        } else {
            setValid(formFields.province, "#provinceValidation");
            validationsStatus.province = false;
        }
    }

    formFields.province.on(CHANGE_EVENT, validateProvince);
    formFields.province.on(FOCUSOUT_EVENT, validateProvince);
//Validación provincia

//Validación calle1
    const validateStreet1 = function () {
        if (formFields.street1.val().length === 0) {
            setInvalid(formFields.street1, "#street1Validation", "<p>Debes escribir al menos una calle</p>");
            validationsStatus.street1 = true;
        } else {
            setValid(formFields.street1, "#street1Validation");
            validationsStatus.street1 = false;
        }
    }

    formFields.street1.on(CHANGE_EVENT, validateStreet1);
    formFields.street1.on(FOCUSOUT_EVENT, validateStreet1);
//Validación calle1


//Validación número de casa
    const validateHouseNumber = function () {
        if (formFields.houseNumber.val().length === 0) {
            setInvalid(formFields.houseNumber, "#houseNumberValidation", "<p>Debes escribir el número de casa o puerta</p>");
            validationsStatus.houseNumber = true;
        } else if (isNaN(formFields.houseNumber.val())) {
            setInvalid(formFields.houseNumber, "#houseNumberValidation", "<p>Solo puedes escribir números</p>");
            validationsStatus.houseNumber = true;
        } else {
            setValid(formFields.houseNumber, "#houseNumberValidation");
            validationsStatus.houseNumber = false;
        }
    }

    formFields.houseNumber.on(CHANGE_EVENT, validateHouseNumber);
    formFields.houseNumber.on(FOCUSOUT_EVENT, validateHouseNumber);
//Validación número de casa


//Validación latitud y longitud
    const validateLatitudeLongitude = function () {
        if ((!formFields.latitude.val() || formFields.latitude.val().length === 0) || (!formFields.longitude.val() || formFields.longitude.val().length === 0)) {
            $("#latLngValidation").html(`<p>Debes escoger la ubicación exacta</p>`);
            validationsStatus.latitude = true;
            validationsStatus.longitude = true;
        } else {
            $("#latLngValidation").html("");
            validationsStatus.latitude = false;
            validationsStatus.longitude = false;
        }
    }

    formFields.latitude.on(CHANGE_EVENT, validateLatitudeLongitude);
    formFields.longitude.on(CHANGE_EVENT, validateLatitudeLongitude);
//Validación latitud y longitud


//Validación rol de usuario

    const validateUserRole = function () {
        if (formFields.userRole.val() == null) {
            setInvalid(formFields.userRole, "#roleValidation", "<p>Debes escoger el rol</p>");
            validationsStatus.userRole = true;
        } else {
            setValid(formFields.userRole, "#roleValidation");
            validationsStatus.userRole = false;
        }
    }

    formFields.userRole.on(CHANGE_EVENT, validateUserRole);
    formFields.userRole.on(FOCUSOUT_EVENT, validateUserRole);

//Validación rol de usuario


//Validación contraseña
    const validatePassword = function () {
        if (formFields.password.val().length === 0) {
            setInvalid(formFields.password, "#passwordValidation", "<p>Debes escribir la contraseña</p>");
            validationsStatus.password = true;
        } else if (formFields.confirmPassword.val().length > 0 && formFields.password.val() !== formFields.confirmPassword.val()) {
            setInvalid(formFields.password, "#passwordValidation", "<p>Las contraseñas no coinciden</p>");
            validationsStatus.password = true;
        } else {
            setValid(formFields.password, "#passwordValidation");
            validationsStatus.password = false;
        }
    }
    formFields.password.on(CHANGE_EVENT, validatePassword);
    formFields.password.on(FOCUSOUT_EVENT, validatePassword);
//Validación contraseña

//Validación confirmar contraseña
    const validateConfirmPassword = function () {
        if (formFields.confirmPassword.val().length === 0) {
            setInvalid(formFields.confirmPassword, "#confirmPasswordValidation", "<p>Debes escribir la contraseña</p>");
            validationsStatus.confirmPassword = true;
        } else if (formFields.password.val().length > 0 && formFields.confirmPassword.val() !== formFields.password.val()) {
            setInvalid(formFields.confirmPassword, "#confirmPasswordValidation", "<p>Las contraseñas no coinciden</p>");
            validationsStatus.confirmPassword = true;
        } else {
            setValid(formFields.confirmPassword, "#confirmPasswordValidation");
            validationsStatus.confirmPassword = false;
        }
    }
    formFields.confirmPassword.on(CHANGE_EVENT, validateConfirmPassword);
    formFields.confirmPassword.on(FOCUSOUT_EVENT, validateConfirmPassword);
//Validación confirmar contraseña


const setInvalid = function (element, validationId, validationHtml) {
    element.removeClass('is-valid');
    element.addClass('is-invalid');
    $(validationId).html(validationHtml);
    $("#saveUserBtn").prop('disabled', true);
}

const setValid = function (element, validationId) {
    element.removeClass('is-invalid');
    element.addClass('is-valid');
    $(validationId).html("");
    $("#saveUserBtn").prop('disabled', false);
}

const validateOnClick = function () {
    validateFirstName();
    validateLastName();
    validateIdentificationDocument();
    validateEmail();
    validatePhoneNumber();
    validateCountry();
    validateCity();
    validateProvince();
    validateStreet1();
    validateHouseNumber();
    validateLatitudeLongitude();
    validateUserRole();
    validatePassword();
    validateConfirmPassword();

    for (let field in validationsStatus) {
        if (validationsStatus[field]) {
            $("#finalValidation").html(`<p><i class="fas fa-exclamation"></i> Tienes errores de validación, por favor, completa bien los campos</p>`);
            $("#saveUserBtn").prop('disabled', true);
        } else {
            $("#finalValidation").html("");
            $("#saveUserBtn").prop('disabled', false);
        }
    }


    let somethingIsInvalid = false;
    for (let field in validationsStatus) {
        if (validationsStatus[field]) {
            somethingIsInvalid = true;
            break;
        }   
    }

    return somethingIsInvalid;
    
}