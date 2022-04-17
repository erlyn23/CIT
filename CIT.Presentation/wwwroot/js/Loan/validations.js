const formFields = {
    loanName: $("#loanName"),
    description: $("#description"),
    duesQuantity: $("#duesQuantity"),
    totalLoan: $("#totalLoan"),
    startDate: $("#startDate"),
    endDate: $("#endDate"),
    payDay: $("#payDay"),
    interestRate: $("#interestRate"),
    mensualPay: $("#mensualPay")
};

const validationsStatus = {
    loanName: false,
    description: false,
    duesQuantity: false,
    totalLoan: false,
    startDate: false,
    endDate: false,
    payDay: false,
    interestRate: false,
    mensualPay: false
};

const CHANGE_EVENT = 'change';
const FOCUSOUT_EVENT = 'focusout';

//Validación nombre del préstamo
const validateLoanName = function () {
    if (formFields.loanName.val().length === 0) {
        setInvalid(formFields.loanName, "#loanName", "<p>Debes escribir el nombre del préstamo</p>");
        validationsStatus.loanName = true;
    } else if (formFields.loanName.val().length > 20) {
        setInvalid(formFields.loanName, "#loanName", "<p>El nombre del préstamo debe tener menos de 20 carácteres</p>");
        validationsStatus.loanName = true;
    } else {
        setValid(formFields.loanName, "#loanNameValidation");
        validationsStatus.loanName = false;
    }
}
//Validación nombre del préstamo

//Validación de la descripción
const validateLoanName = function () {
    if (formFields.description.val().length === 0) {
        setInvalid(formFields.description, "#description", "<p>Debes escribir la descripción del préstamo</p>");
        validationsStatus.description = true;
    } else if (formFields.description.val().length > 250) {
        setInvalid(formFields.description, "#description", "<p>La descripción del préstamo debe tener menos de 250 carácteres</p>");
        validationsStatus.description = true;
    } else {
        setValid(formFields.description, "#descriptionValidation");
        validationsStatus.description = false;
    }
}
//Validación de la descripción

//Validación Cantidad de Cuotas
const validateDuesQuantity = function () {
    if (formFields.duesQuantity.val().length === 0 || formFields.duesQuantity.val() <= 0) {
        setInvalid(formFields.duesQuantity, "#duesQuantityValidation", "<p>Debes escribir la cantidad de cuotas y el valor no puede ser negativo</p>");
        validationsStatus.duesQuantity = true;
    } else {
        setValid(formFields.duesQuantity, "#duesQuantityValidation");
        validationsStatus.duesQuantity = false;
    }
}

formFields.duesQuantity.on(CHANGE_EVENT, validateDuesQuantity);
formFields.duesQuantity.on(FOCUSOUT_EVENT, validateDuesQuantity);
//Validación Cantidad de Cuotas

//Validación Préstamo total
const validateTotalLoan = function () {
    if (formFields.totalLoan.val().length === 0 || formFields.totalLoan.val() <= 0) {
        setInvalid(formFields.totalLoan, "#totalLoanValidation", "<p>Debes escribir el préstamo total y el valor no puede ser negativo</p>");
        validationsStatus.totalLoan = true;
    } else {
        setValid(formFields.totalLoan, "#totalLoanValidation");
        validationsStatus.totalLoan = false;
    }
}

formFields.totalLoan.on(CHANGE_EVENT, validateTotalLoan);
formFields.totalLoan.on(FOCUSOUT_EVENT, validateTotalLoan);
//Validación Préstamo total


//Validación fecha de inicio
const validateStartDate = function () {
    if (formFields.startDate.val().length === 0) {
        setInvalid(formFields.startDate, "#startDateValidation", "<p>Debes escribir la fecha de inicio</p>");
        validationsStatus.startDate = true;
    } else {
        setValid(formFields.startDate, "#startDateValidation");
        validationsStatus.startDate = false;
    }
}

formFields.startDate.on(CHANGE_EVENT, validateStartDate);
formFields.startDate.on(FOCUSOUT_EVENT, validateStartDate);
//Validación fecha de inicio

//Validación fecha final
const validateEndDate = function () {
    if (formFields.endDate.val().length === 0) {
        setInvalid(formFields.endDate, "#endDateValidation", "<p>Debes escribir la fecha final</p>");
        validationsStatus.endDate = true;
    } else if (formFields.endDate.val() <= formFields.startDate.val()) {
        setInvalid(formFields.endDate, "#endDateValidation", "<p>La fecha final no puede ser menor o igual a la fecha inicial</p>");
        validationsStatus.endDate = true;
    } else {
        setValid(formFields.endDate, "#endDateValidation");
        validationsStatus.endDate = false;
    }
}

formFields.endDate.on(CHANGE_EVENT, validateEndDate);
formFields.endDate.on(FOCUSOUT_EVENT, validateEndDate);
//Validación fecha final


//Validación día de pago
const validatePayDay = function () {
    if (formFields.payDay.val().length === 0 || formFields.payDay.val() <= 0) {
        setInvalid(formFields.payDay, "#payDayValidation", "<p>Debes escribir el día de pago y el valor no puede ser negativo</p>");
        validationsStatus.payDay = true;
    } else if (formFields.payDay.val() <= 1 || formFields.payDay.val() >= 31) {
        setInvalid(formFields.payDay, "#payDayValidation", "<p>El día de pago debe estar entre 1 y 31</p>");
        validationsStatus.payDay = true;
    } else {
        setValid(formFields.payDay, "#payDayValidation");
        validationsStatus.payDay = false;
    }
}

formFields.payDay.on(CHANGE_EVENT, validatePayDay);
formFields.payDay.on(FOCUSOUT_EVENT, validatePayDay);
//Validación día de pago

//Validación tasa de interés
const validateInterestRate = function () {
    if (formFields.interestRate.val().length === 0 || formFields.interestRate.val() <= 0) {
        setInvalid(formFields.interestRate, "#interestRateValidation", "<p>Debes escribir la tasa de interés y el valor no puede ser negativo</p>");
        validationsStatus.interestRate = true;
    } else {
        setValid(formFields.interestRate, "#interestRateValidation");
        validationsStatus.interestRate = false;
    }
}

formFields.interestRate.on(CHANGE_EVENT, validateInterestRate);
formFields.interestRate.on(FOCUSOUT_EVENT, validateInterestRate);
//Validación tasa de interés


//Validación pago mensual
const validateMensualPay = function () {
    if (formFields.mensualPay.val().length === 0 || formFields.mensualPay.val() <= 0) {
        setInvalid(formFields.mensualPay, "#mensualPayValidation", "<p>Debes escribir el pago mensual y el valor no puede ser negativo</p>");
        validationsStatus.mensualPay = true;
    } else {
        setValid(formFields.mensualPay, "#mensualPayValidation");
        validationsStatus.mensualPay = false;
    }
}

formFields.mensualPay.on(CHANGE_EVENT, validateMensualPay);
formFields.mensualPay.on(FOCUSOUT_EVENT, validateMensualPay);
//Validación pago mensual

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
    validateDuesQuantity();
    validateTotalLoan();
    validateStartDate();
    validateEndDate();
    validateInterestRate();
    validatePayDay();
    validateMensualPay();

    for (let field in validationsStatus) {
        if (validationsStatus[field]) {
            $("#finalValidation").html(`<p><i class="fas fa-exclamation"></i> Tienes errores de validación, por favor, completa bien los campos</p>`);
            $("#saveLoanBtn").prop('disabled', true);
        } else {
            $("#finalValidation").html("");
            $("#saveLoanBtn").prop('disabled', false);
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