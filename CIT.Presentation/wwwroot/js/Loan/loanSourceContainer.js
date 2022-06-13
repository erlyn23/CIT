const formLanName = {
    loanNameFilter: $("#loanNameFilter"),
    loanId: $("#loanId")
};

const validationFullNameStatus = {
    loanNameFilter: false,
    loanId: false
};

//Validación nombre del usuario
const validateFullName = function () {
    if (formLanName.loanNameFilter.val().length === 0) {
        setInvalid(formLanName.loanNameFilter, "#loanValidation", "<p>Debes escoger el préstamo</p>");
        validationLoanNameStatus.loanNameFilter = true;
    } else {
        setValid(formLanName.loanNameFilter, "#loanValidation");
        validationLoanNameStatus.loanName = false;
    }
}
formLanName.loanNameFilter.on(CHANGE_EVENT, validateFullName);
formLanName.loanNameFilter.on(FOCUSOUT_EVENT, validateFullName);
//Validación nombre del usuario

formLanName.loanNameFilter.on('keyup', function (e) {
    if (e.target.value.length > 0) {
        $("#loanSourceContainer").removeClass('d-none');
        doRequest({
            url: `/Loan/GetLoansByName/?loanName=${formLoanName.loanNameFilter.val()}`,
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
        $("#userSourceContainer").addClass('d-none');
});