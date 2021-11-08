const formFields = {
    businessName: $("#businessName"),
    rnc: $("#rnc"),
    phone: $("#phone"),
    email: $("#email"),
    country: $("#country"),
    city: $("#city"),
    province: $("#province"),
    street1: $("#street1"),
    houseNumber: $("#houseNumber"),
    latitude: $("#latitude"),
    longitude: $("#longitude"),
    password: $("#password"),
    confirmPassword: $("#confirmPassword")
};

const validationsStatus = {
    businessName: false,
    rnc: false,
    phone: false,
    email: false,
    country: false,
    city: false,
    province: false,
    street1: false,
    houseNumber: false,
    latitude: false,
    longitude: false,
    password: false,
    confirmPassword: false
};

const CHANGE_EVENT = 'change';
const FOCUSOUT_EVENT = 'focusout';

//Validación Nombre del negocio
const validateBusinessName = function () {
    if (formFields.businessName.val().length === 0) {
        setInvalid(formFields.businessName, "#businessNameValidation", "<p>Debes escribir el nombre</p>");
        validationsStatus.businessName = true;
    } else if (formFields.businessName.val().length > 30) {
        setInvalid(formFields.businessName, "#businessNameValidation", "<p>Debes escribir menos de 30 letras</p>");
        validationsStatus.businessName = true;
    } else {
        setValid(formFields.businessName, "#businessNameValidation");
        validationsStatus.businessName = false;
    }
}

formFields.businessName.on(CHANGE_EVENT, validateBusinessName);
formFields.businessName.on(FOCUSOUT_EVENT, validateBusinessName);
//Validación Nombre del negocio



//Validación Rnc
const validateRnc = function () {
    if (formFields.rnc.val().length === 0) {
        setInvalid(formFields.rnc, "#rncValidation", "<p>Debes escribir el Rnc</p>");
        validationsStatus.rnc = true;
    } else if (formFields.rnc.val().length > 11 || formFields.rnc.val().length < 11) {
        setInvalid(formFields.rnc, "#rncValidation", "<p>Solo debes escribir 11 números</p>");
        validationsStatus.rnc = true;
    } else if (isNaN(formFields.rnc.val())) {
        setInvalid(formFields.rnc, "#rncValidation", "<p>Solo puedes escribir números</p>");
        validationsStatus.rnc = true;
    } else {
        setValid(formFields.rnc, "#rncValidation");
        validationsStatus.rnc = false;
    }
}

formFields.rnc.on(CHANGE_EVENT, validateRnc);
formFields.rnc.on(FOCUSOUT_EVENT, validateRnc);
//Validación Rnc

//Validación teléfono
const validatePhoneNumber = function () {
    if (formFields.phone.val().length === 0) {
        setInvalid(formFields.phone, "#phoneValidation", "<p>Debes escribir el teléfono</p>");
        validationsStatus.phone = true;
    } else if (formFields.phone.val().length > 10 || formFields.phone.val().length < 10) {
        setInvalid(formFields.phone, "#phoneValidation", "<p>Solo debes escribir 10 números</p>");
        validationsStatus.phone = true;
    } else if (isNaN(formFields.phone.val())) {
        setInvalid(formFields.phone, "#phoneValidation", "<p>Solo puedes escribir números</p>");
        validationsStatus.phone = true;
    } else {
        setValid(formFields.phone, "#phoneValidation");
        validationsStatus.phone = false;
    }
}

formFields.phone.on(CHANGE_EVENT, validatePhoneNumber);
formFields.phone.on(FOCUSOUT_EVENT, validatePhoneNumber);
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
    if (formFields.country.val() == null) {
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
    $("#registerBtn").prop('disabled', true);
}

const setValid = function (element, validationId) {
    element.removeClass('is-invalid');
    element.addClass('is-valid');
    $(validationId).html("");
    $("#registerBtn").prop('disabled', false);
}

const validateOnClick = function () {
    validateBusinessName();
    validateRnc();
    validateEmail();
    validatePhoneNumber();
    validateCountry();
    validateCity();
    validateProvince();
    validateStreet1();
    validateHouseNumber();
    validateLatitudeLongitude();
    validatePassword();
    validateConfirmPassword();

    for (let field in validationsStatus) {
        if (validationsStatus[field]) {
            $("#finalValidation").html(`<p><i class="fas fa-exclamation"></i> Tienes errores de validación, por favor, completa bien los campos</p>`);
            $("#registerBtn").prop('disabled', true);
        } else {
            $("#finalValidation").html("");
            $("#registerBtn").prop('disabled', false);
        }
    }

    for (let field in validationsStatus) {
        if (validationsStatus[field]) {
            return true;
        } else {
            return false;
        }
    }

}