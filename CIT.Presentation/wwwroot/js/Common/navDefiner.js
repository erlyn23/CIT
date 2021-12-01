const rolePages = [];


const dashboardHeaders = {
    'content-type': 'application/json',
    'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`,
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