$.ajax({
    url: '/Users/GetNumbers',
    method: 'GET',
    headers: {
        'content-type': 'application/json',
        'Page': 'Users',
        'Operation': 'List',
        'Authorization': `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`
    },
    success: function (data) {
        console.log(data);
    },
    error: function (error) {
        console.log(error);
    }

});