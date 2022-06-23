getUserPages('amortizacionesLink');
getUserPermissions(6);

const amortizationHeaders = {
    ...appHeaders,
    'Page': 'Amortizaciones'
};

const createAmortization = function (loanId) {
    $("#loadingAmortization").removeClass('d-none');
    doRequest({
        url: `/Amortization/CreateAmortization?loanId=${loanId}`,
        method: 'GET',
        data: null,
        headers: { ...amortizationHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) { onGetAmortization(data); },
        errorCallback: function (err) { onErrorHandler(err) }
    });
}

const onGetAmortization = function (data) {
    if (hasGet) {
        const html = templateAmortizationList(data);
        $("#amortizationList").html(html);
        $("#loadingAmortization").addClass('d-none');
    }
}

const templateAmortizationList = (data) => {

    const tBody = $("#amortizationList");
    tBody.html("");
    let html = "";
    data.periods.forEach((period, index) => {
        html += `<tr>
                    <td>${period}</td>
                    <td>${data.dues[index]}</td>
                    <td>${data.interests[index]}</td>
                    <td>${data.capitalPayments[index]}</td>
                    <td>${data.balance[index]}</td>
                </tr>`;
    });
    return html;
}


$("#createAmortizationBtn").on('click', function () {
    if (!validateOnClick()) {
        const loanId = $("#loanId").val();

        if (loanId.length > 0) {
            createAmortization(loanId);
            $("#errorMessage").addClass("d-none");
            $("#errorMessage").text("");
            $("#excelBtnContainer").removeClass("d-none");
        } else {
            $("#errorMessage").removeClass('d-none');
            $("#errorMessage").text("Debes escoger un préstamo para crear su tabla de amortización.");
            $("#excelBtnContainer").addClass('d-none');
        }
    }
});

$("#exportToExcelBtn").on('click', function () {
    const excelExported = XLSX.utils.table_to_book(document.getElementById("amortizationTable"));

    XLSX.writeFile(excelExported, `AmortizationTable_${$("#userId").val()}_${$("#loanId").val()}.xlsx`);
});

$("#cancelAmortizationBtn").on('click', function () {
    $("#errorMessage").addClass('d-none');
    $("#errorMessage").text("");
    $("#amortizationList").html("");
    $("#excelBtnContainer").addClass('d-none');

    formLoanName.loanId.val("");
    formLoanName.loanNameFilter.removeClass('is-valid');
    formLoanName.loanNameFilter.removeClass('is-invalid');
    formLoanName.loanNameFilter.val("");
    $("#loanNameValidation").html('');

    $("#userId").val("");
});