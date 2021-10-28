const token = `Bearer ${JSON.parse(localStorage.getItem('user'))?.token}`;

$.ajax({
    url: '/Users/GetNumbers',
    method: 'GET',
    headers: {
        'content-type': 'application/json',
        'Authorization': token,
        'Page': 'Users',
        'Operation': 'List',
    },
    success: function (data) {
        console.log(data);
    },
    error: function (error) {
        console.log(error);
    }

});