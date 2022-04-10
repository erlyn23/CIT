getUserPages('prestamosLink');
getUserPermissions(4);

const loanHeaders = {
    ...appHeaders,
    'Page': 'Prestamos'
};

const getLoans = function () {
    doRequest({
        url: '/Loan/GetLoans',
        method: 'GET',
        data: null,
        headers: { ...loanHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) { onGetLoans(data); },
        errorCallback: function (err) { onErrorHandler(err) }
    });
}

const onGetLoans = function (data) {
    if (!hasAdd) {
        $("#openCreateLoan").remove();
        $("#CreteLoanModal").remove();
    }
    if (hasGet) {
        $("#paginator-container").pagination({
            dataSource: data,
            pageSize: 5,
            callback: function (data, pagination) {
                const html = templateLoansList(data);
                $("#loansList").html(html);
                setEditLoanEvent(data);
                $("#loadingLoans").css({ 'display': 'none' });
                $("#loansTable").removeClass('d-none');
            }
        });
    }
}

const templateLoansList = (loans) => {

    const tBody = $("#loansList");
    tBody.html("");
    let html = "";
    loans.forEach(loan => {
        const startDate = new Date(loan.startDate);
        const startDateView = `${startDate.getDate()}/${startDate.getMonth() + 1}/${startDate.getFullYear()}`;

        const endDate = new Date(loan.endDate);
        const endDateView = `${endDate.getDate()}/${endDate.getMonth() + 1}/${endDate.getFullYear()}`;
        html += `<tr>
                    <td>${loan.id}</td>
                    <td>${startDateView}</td>
                    <td>${endDateView}</td>
                    <td>${loan.payDay}</td>
                    <td>
                        <button type="button" id="setLoanDetailBtn-${loan.id}" class="btn btn-sm btn-success"><i class="fas fa-eye"></i></button>
                        <button type="button" id="editLoanBtn-${loan.id}" class="btn btn-sm btn-warning"><i class="fas fa-edit"></i></button>
                        <button type="button" id="deleteLoanBtn-${loan.id}" class="btn btn-sm btn-danger" onclick="deleteLoan(${loan.id})"><i class="fas fa-trash"></i></button>
                    </td>
                </tr>`;
    });
    return html;
}

const setLoanData = function (loan) {

    const startDate = new Date(loan.startDate);
    const startDateView = `${startDate.getFullYear()}-${startDate.getMonth()}-${startDate.getDate()}`;

    const endDate = new Date(loan.endDate);
    const endDateView = `${endDate.getFullYear()}-${endDate.getMonth()}-${endDate.getDate()}`;
    $("#createLoanTitle").text("Editar préstamo");

    $("#loanId").val(loan.id);
    $("#duesQuantity").val(loan.duesQuantity);
    $("#totalLoan").val(loan.totalLoan);
    document.getElementById("startDate").valueAsDate = new Date(loan.startDate);
    document.getElementById("endDate").valueAsDate = new Date(loan.endDate);
    $("#payDay").val(loan.payDay);
    $(`#interestRate`).val(loan.interestRate * 100);
    $("#mensualPay").val(loan.mensualPay);

    $("#CreateLoanModal").modal('show');
}

const setLoanDetailData = function (loan) {
    const startDate = new Date(loan.startDate);
    const startDateView = `${startDate.getDate()}/${startDate.getMonth() + 1}/${startDate.getFullYear()}`;

    const endDate = new Date(loan.endDate);
    const endDateView = `${endDate.getDate()}/${endDate.getMonth() + 1}/${endDate.getFullYear()}`;


    $("#loanIdView").text(loan.id);
    $("#duesQuantityView").text(loan.duesQuantity);
    $("#totalLoanView").text(loan.totalLoan);
    $("#startDateView").text(startDateView);
    $("#endDateView").text(endDateView);
    $("#payDayView").text(loan.payDay);
    $(`#interestRateView`).text(`${loan.interestRate * 100}%`);
    $("#mensualPayView").text(loan.mensualPay);


    $("#LoanModalDetail").modal('show');
}

const setEditLoanEvent = function (loans) {
    loans.forEach(function (loan) {

        if (hasEdit) {
            $("#editLoanBtn-" + loan.id).on('click', function () {
                setLoanData(loan);
            });
        } else
            $("#editLoanBtn-" + loan.id).remove();

        if (hasGet) {

            $("#setLoanDetailBtn-" + loan.id).on('click', function () {
                setLoanDetailData(loan);
            });

        } else $("#setLoanDetailBtn-" + loan.id).remove();

        if (!hasDelete)
            $("#deleteLoanBtn-" + loan.id).remove();

    });
}

$("#saveLoanBtn").on('click', function () {
    if (!validateOnClick()) {
        $("#saveLoanBtn > span").text("Enviando datos, por favor espere...");
        $("#saveLoanBtn").prop('disabled', true);
        const newLoan = {
            duesQuantity: formFields.duesQuantity.val(),
            totalLoan: formFields.totalLoan.val(),
            startDate: formFields.startDate.val(),
            endDate: formFields.endDate.val(),
            payDay: formFields.payDay.val(),
            interestRate: formFields.interestRate.val(),
            mensualPay: formFields.mensualPay.val()
        };

        if ($("#loanId").val() !== "") 
            newLoan.id = $("#loanId").val();

        let url = ($("#loanId").val() !== "") ? '/Loan/UpdateLoan' : '/Loan/AddLoan';
        let operation = ($("#loanId").val() !== "") ? 'Modificar' : 'Agregar';
        doRequest({
            url: url,
            method: 'POST',
            data: newLoan,
            headers: { ...loanHeaders, 'Operation': operation },
            successCallback: function (data) {
                onSuccessSaveLoan(data);
            },
            errorCallback: function (err) {
                onError(err);
            }
        });
    }
});


const onSuccessSaveLoan = function (data) {
    $("#saveLoanBtn > span").text("Guardar préstamo");
    $("#saveLoanBtn").prop('disabled', false);
    if (Array.isArray(data)) {
        $("#errorMessages").html("");
        for (let error of data) {
            let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>&nbsp; ${error}</p>`;
            $("#errorMessages").append(errorMsg);
            $("#CreateLoanModal").modal('hide');

            setTimeout(function () {
                $("#ErrorMessagesModal").modal('show');
            }, 1000);
        }
    } else {
        $("form").eq(0).trigger('reset');
        for (let field in formFields) {
            formFields[field].removeClass('is-valid');
        }
        $("#errorMessages").html("");
        let successMsg = `<p class="text-success"><i class="fas fa-check-circle"></i>&nbsp; Préstamo guardado correctamente</p>`;
        $("#errorMessages").append(successMsg);
        $("#CreateLoanModal").modal('hide');

        setTimeout(function () {
            $("#ErrorMessagesModal").modal('show');
        }, 1000);
        getLoans();
    }
}

const onError = function (err) {
    $("#saveLoanBtn > span").text("Guardar préstamo");
    $("#saveLoanBtn").prop('disabled', false);
    $("#errorMessages").html("");
    let errorMsg = `<p class="text-danger"><i class="fas fa-exclamation-circle"></i>${err.responseText}</p>`;
    $("#errorMessages").append(errorMsg);
    $("#CreateLoanModal").modal('hide');

    setTimeout(function () {
        $("#ErrorMessagesModal").modal('show');
    }, 1000);
}

const onErrorHandler = function (err) {
    $("#loadingLoans").addClass("d-none");
    $("#errorMessage").removeClass("d-none");
    $("#errorMessage").html("<p>Ha ocurrido un error al obtener préstamos: " + err.responseText + "</p>")
}

$("#closeLoanFormBtn").on('click', function () {
    $("form").eq(0).trigger('reset');
});

getLoans();