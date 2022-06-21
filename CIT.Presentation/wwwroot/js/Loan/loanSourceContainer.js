const formLoanName = {
    loanNameFilter: $("#loanNameFilter"),
    loanId: $("#loanId")
};

const validationLoanNameStatus = {
    loanNameFilter: false,
    loanId: false
};

//Validación nombre del préstamo
const validateLoanName = function () {
    if (formLoanName.loanNameFilter.val().length === 0) {
        setInvalid(formLoanName.loanNameFilter, "#loanValidation", "<p>Debes escoger el préstamo</p>");
        validationLoanNameStatus.loanNameFilter = true;
    } else {
        setValid(formLoanName.loanNameFilter, "#loanValidation");
        validationLoanNameStatus.loanName = false;
    }
}
formLoanName.loanNameFilter.on(CHANGE_EVENT, validateLoanName);
formLoanName.loanNameFilter.on(FOCUSOUT_EVENT, validateLoanName);
//Validación nombre del préstamo

formLoanName.loanNameFilter.on('keyup', function (e) {
    if (e.target.value.length > 0) {
        $("#loanSourceContainer").removeClass('d-none');
        doRequest({
            url: `/Loan/GetLoansByName?loanName=${formLoanName.loanNameFilter.val()}`,
            method: 'GET',
            data: null,
            headers: appHeaders,
            successCallback: function (data) {
                $("#loanSource").html("");
                if (data) {
                    data.forEach(loan => {
                        const liElement = document.createElement('li');
                        liElement.addEventListener('click', () => {
                            formLoanName.loanNameFilter.val(`${loan.loanName}`);
                            formLoanName.loanId.val(loan.id);
                            formFields.userId.val(loan.userId);
                            $("#loanSourceContainer").addClass('d-none');
                        });
                        liElement.innerText = `${loan.id} - ${loan.loanName}`;
                        $("#loanSource").append(liElement);
                    });
                }
            },
            errorCallback: function (err) {
                console.log(err);
            }
        });
    } else
        $("#loanSourceContainer").addClass('d-none');
});