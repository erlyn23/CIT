const formFields = {
    userId: $("#userId"),
    loanId: $("#loanId"),
    date: $("#date"),
    pay: $("#pay")
};

const validationsStatus = {
    userId: false,
    loanId: false,
    date: false,
    pay: false
};

const CHANGE_EVENT = 'change';
const FOCUSOUT_EVENT = 'focusout';

//Validación fecha de pago
const validateDate = function () {
    if (formFields.date.val().length === 0) {
        setInvalid(formFields.date, "#dateValidation", "<p>Debes escribir la fecha del pago</p>");
        validationsStatus.date = true;
    } else {
        setValid(formFields.date, "#dateValidation");
        validationsStatus.date = false;
    }
}

formFields.date.on(CHANGE_EVENT, validateDate);
formFields.date.on(FOCUSOUT_EVENT, validateDate);
//Validación fecha de pago

//Validación pago 
const validatePay = function () {
    if (formFields.pay.val().length === 0 || formFields.pay.val() <= 0) {
        setInvalid(formFields.pay, "#payValidation", "<p>Debes escribir el pago y el valor no puede ser negativo</p>");
        validationsStatus.pay = true;
    } else {
        setValid(formFields.pay, "#payValidation");
        validationsStatus.pay = false;
    }
}

formFields.pay.on(CHANGE_EVENT, validatePay);
formFields.pay.on(FOCUSOUT_EVENT, validatePay);
//Validación pago 

const setInvalid = function (element, validationId, validationHtml) {
    element.removeClass('is-valid');
    element.addClass('is-invalid');
    $(validationId).html(validationHtml);
    $("#savePayBtn").prop('disabled', true);
}

const setValid = function (element, validationId) {
    element.removeClass('is-invalid');
    element.addClass('is-valid');
    $(validationId).html("");
    $("#savePayBtn").prop('disabled', false);
}

const validateOnClick = function () {
    validateDate();
    validatePay();

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