const tempUser = JSON.parse(localStorage.getItem('temp'));

const getLenderBusinessesByUser = function() {
    doRequest({
        url: `/Account/GetLenderBusinessesByUser?email=${tempUser.email}`,
        headers: null,
        data: null,
        method: 'GET',
        successCallback: function (data) {
            onGetLenderBusinesses(data);
        },
        errorCallback: function (error) {
            console.log(error);
        }
    });
}

const onGetLenderBusinesses = function (data) {
    const lenderBusinessListContainer = $("#lenderBusinessListContainer");
    let htmlTemplate = "";

    data.forEach(lenderBusiness => {
        htmlTemplate += `
            <div class="col-md-3 mx-4 my-4">
                <div class="card box-shadow shadow-sm">
                    <div class="card-body">
                        <div class="text-center">
                            <h5>${lenderBusiness.id} - ${lenderBusiness.businessName}</h5>
                        </div>
                        <div class="text-center"> 
                            <p><span style="font-weight: bold;">RNC: </span> ${lenderBusiness.rnc}</p>
                        </div>
                        <div class="d-grid gap-2">
                            <button class="btn btn-outline-success" onclick="enterInLenderBusiness(${lenderBusiness.id})"><i class="fas fa-sign-in-alt"></i> Entrar</button>
                        </div>
                    </div>
                </div>
            </div>
        `;
    });

    lenderBusinessListContainer.html(htmlTemplate);
}

getLenderBusinessesByUser();

const enterInLenderBusiness = function (lenderBusinessId) {
    doRequest({
        url: '/Account/SignInInLenderBusiness',
        method: 'POST',
        headers: null,
        data: { email: tempUser.email, lenderBusinessId: lenderBusinessId },
        successCallback: function (data) {
            localStorage.setItem('user', JSON.stringify(data));
            window.location.href = '/Dashboard/Index';
        }
    });
}