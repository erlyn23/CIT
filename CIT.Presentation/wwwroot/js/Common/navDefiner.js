const rolePages = [];

if (!localStorage.getItem('user')) window.location = '/Account/Index';

const appHeaders = {
    'content-type': 'application/json',
    'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`
}

const dashboardHeaders = {
    ...appHeaders,
    'Page': 'Dashboard'
};

const getUserPages = function (activeLink) {
    doRequest({
        url: '/Dashboard/GetPagesByRole',
        method: 'GET',
        data: null,
        headers: { ...dashboardHeaders, 'Operation': 'Obtener' },
        successCallback: function (data) {
            onGetNavPages(data, activeLink);
        },
        errorCallback: function (error) {
            onErrorGetPages(error);
        }
    });
};

let hasGet = false;
let hasAdd = false;
let hasEdit = false;
let hasDelete = false;
const getUserPermissions = function (pageId) {
    doRequest({
        url: `/Dashboard/GetPermissionsByPageAndRole?pageId=${pageId}`,
        method: 'GET',
        data: null,
        headers: { ...appHeaders },
        successCallback: function (data) {
            hasGet = hasPermission(data, 'Obtener');
            hasAdd = hasPermission(data, 'Agregar');
            hasEdit = hasPermission(data, 'Modificar');
            hasDelete = hasPermission(data, 'Eliminar');
        },
        errorCallback: function (error) {
            onErrorGetPages(error);
        }
    });
}

const hasPermission = function (data, operationName) {
    return data.findIndex(d => d.operationName === operationName) >= 0;
}

const onGetNavPages = function (data, activeLink) {
    let htmlNav = "";
    data.sort((a, b) => a.pageId - b.pageId);
    for (let page of data) {
        htmlNav += `<a class="nav_link" href="${page.route}" id="${page.pageName.toLowerCase()}Link">
                        <i class='bx ${page.iconClass} nav_icon'></i>
                        <span class="nav_name">${page.pageName}</span>
                    </a>`;
    }

    $("#nav-list").html(htmlNav);

    const linkColor = document.getElementById(activeLink)
    document.querySelectorAll('.nav_link').forEach(l => l.classList.remove('active'));
    linkColor.classList.add('active');
}

const onErrorGetPages = function (error) {
    alert(error);
}