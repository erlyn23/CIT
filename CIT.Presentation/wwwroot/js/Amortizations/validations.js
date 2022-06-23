const formFields = {
    userId: $("#userId"),
    loanId: $("#loanId")
};

const validationsStatus = {
    userId: false,
    loanId: false
};

const CHANGE_EVENT = 'change';
const FOCUSOUT_EVENT = 'focusout';

const setInvalid = function (element, validationId, validationHtml) {
    element.removeClass('is-valid');
    element.addClass('is-invalid');
    $(validationId).html(validationHtml);
    $("#crateAmortizationBtn").prop('disabled', true);
}

const setValid = function (element, validationId) {
    element.removeClass('is-invalid');
    element.addClass('is-valid');
    $(validationId).html("");
    $("#crateAmortizationBtn").prop('disabled', false);
}

const validateOnClick = function () {
    validateLoanName();

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