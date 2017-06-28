function renderDiv(url, data) {
  
    $.ajax({
        url: url,
        data: data,
        type: 'GET',
        success: function(res) {
            $("#usersection").html(res);
            
        },
        error: function(msg) {
            $("#usersection").html(msg);
        }
    });
  
}
