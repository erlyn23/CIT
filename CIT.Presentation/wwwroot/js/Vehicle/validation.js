const formFields = {
    brand: $("#brand"),
    model: $("#model"),
    enrollment: $("#enrollment"),
    licensePlate: $("#licensePlate"),
    color: $("#color"),
    year: $("#year")
};

const validationsStatus = {
    brand: false,
    model: false,
    enrollment: false,
    licensePlate: false,
    color: false,
    year: false
};

const CHANGE_EVENT = 'change';
const FOCUSOUT_EVENT = 'focusout';

//Validación Marca
const validateBrand = function () {
    if (formFields.brand.val().length === 0) {
        setInvalid(formFields.brand, "#brandValidation", "<p>Debes escribir la marca</p>");
        validationsStatus.brand = true;
    } else {
        setValid(formFields.brand, "#brandValidation");
        validationsStatus.brand = false;
    }
}

formFields.brand.on(CHANGE_EVENT, validateBrand);
formFields.brand.on(FOCUSOUT_EVENT, validateBrand);
//Validación Marca


//Validación Modelo
const validateModel = function () {
    if (formFields.model.val().length === 0) {
        setInvalid(formFields.model, "#modelValidation", "<p>Debes escribir el modelo</p>");
        validationsStatus.model = true;
    } else {
        setValid(formFields.model, "#modelValidation");
        validationsStatus.model = false;
    }
}

formFields.model.on(CHANGE_EVENT, validateModel);
formFields.model.on(FOCUSOUT_EVENT, validateModel);
//Validación Modelo


//Validación Matrícula
const validateEnrollment = function () {
    if (formFields.enrollment.val().length === 0) {
        setInvalid(formFields.enrollment, "#enrollmentValidation", "<p>Debes escribir la matrícula</p>");
        validationsStatus.enrollment = true;
    } else if (formFields.enrollment.val().length > 7 || formFields.enrollment.val().length < 7) {
        setInvalid(formFields.enrollment, "#enrollmentValidation", "<p>Solo debes escribir 7 números</p>");
        validationsStatus.enrollment = true;
    } else if (isNaN(formFields.enrollment.val())) {
        setInvalid(formFields.enrollment, "#enrollmentValidation", "<p>Solo puedes escribir números</p>");
        validationsStatus.enrollment = true;
    } else {
        setValid(formFields.enrollment, "#enrollmentValidation");
        validationsStatus.enrollment = false;
    }
}

formFields.enrollment.on(CHANGE_EVENT, validateEnrollment);
formFields.enrollment.on(FOCUSOUT_EVENT, validateEnrollment);
//Validación Matrícula

//Validación Placa
const validateLicensePlate = function () {
    if (formFields.licensePlate.val().length === 0) {
        setInvalid(formFields.licensePlate, "#licensePlateValidation", "<p>Debes escribir la placa</p>");
        validationsStatus.licensePlate = true;
    } else if (formFields.licensePlate.val().length > 7 || formFields.licensePlate.val().length < 7) {
        setInvalid(formFields.licensePlate, "#licensePlateValidation", "<p>Solo debes escribir 7 carácteres</p>");
        validationsStatus.licensePlate = true;
    } else {
        setValid(formFields.licensePlate, "#licensePlateValidation");
        validationsStatus.licensePlate = false;
    }
}

formFields.licensePlate.on(CHANGE_EVENT, validateLicensePlate);
formFields.licensePlate.on(FOCUSOUT_EVENT, validateLicensePlate);
//Validación Placa


//Validación Color
const validateColor = function () {
    if (formFields.color.val().length === 0) {
        setInvalid(formFields.color, "#colorValidation", "<p>Debes escribir el color</p>");
        validationsStatus.color = true;
    } else if (formFields.color.val().length > 15) {
        setInvalid(formFields.color, "#colorValidation", "<p>Solo debes escribir menos de 15 letras</p>");
        validationsStatus.color = true;
    } else {
        setValid(formFields.color, "#colorValidation");
        validationsStatus.color = false;
    }
}

formFields.color.on(CHANGE_EVENT, validateColor);
formFields.color.on(FOCUSOUT_EVENT, validateColor);
//Validación Color


//Validación Año
const validateYear = function () {
    if (formFields.year.val() === 0) {
        setInvalid(formFields.year, "#yearValidation", "<p>Debes escribir el año del vehículo</p>");
        validationsStatus.year = true;
    } else if (formFields.year.val() < 1900 || formFields.year.val() > new Date().getFullYear()) {
        setInvalid(formFields.year, "#yearValidation", `<p>El año debe estar entre 1900 y ${new Date().getFullYear()}</p>`);
        validationsStatus.year = true;
    }
    else {
        setValid(formFields.year, "#yearValidation");
        validationsStatus.year = false;
    }
}
formFields.year.on(CHANGE_EVENT, validateYear);
formFields.year.on(FOCUSOUT_EVENT, validateYear);
//Validación Año

const setInvalid = function (element, validationId, validationHtml) {
    element.removeClass('is-valid');
    element.addClass('is-invalid');
    $(validationId).html(validationHtml);
    $("#saveVehicleBtn").prop('disabled', true);
}

const setValid = function (element, validationId) {
    element.removeClass('is-invalid');
    element.addClass('is-valid');
    $(validationId).html("");
    $("#saveVehicleBtn").prop('disabled', false);
}

const validateOnClick = function () {
    validateBrand();
    validateModel();
    validateEnrollment();
    validateLicensePlate();
    validateColor();
    validateYear();

    for (let field in validationsStatus) {
        if (validationsStatus[field]) {
            $("#finalValidation").html(`<p><i class="fas fa-exclamation"></i> Tienes errores de validación, por favor, completa bien los campos</p>`);
            $("#saveVehicleBtn").prop('disabled', true);
        } else {
            $("#finalValidation").html("");
            $("#saveVehicleBtn").prop('disabled', false);
        }
    }

    for (let field in validationsStatus) {
        if (validationsStatus[field]) 
            return true;
        else 
            return false;
    }

}