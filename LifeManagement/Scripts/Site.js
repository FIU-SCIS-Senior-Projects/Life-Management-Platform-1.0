function validateForm() {
    ;
    var requiredFields = "";
    $('input').each(function () {
        if ($(this).attr("data-val-required")) {
            if ($(this).val() == '')
                requiredFields += " - " + $(this).attr("name") + "\n";
        }
    });

    if (requiredFields != "") {
        alert("Please enter the following required field(s): \n" + requiredFields);
        return false;
    } else {
        return true;
    }
}
/***********uploading******************************/
function uploadfile(fileid, valuepairs, ajaxurl,successfunction,errorfunction) {
    var fileInput = $(fileid);
    $.each($(fileInput).get(0).files,
        function(index, value) {
            formdata.append(value.name, value);
        });
    
    valuepairs.each(function(index) {
            formdata.append($(this)[0], $(this)[1]);
        }
    )
   
    formdata.append("approved", approval);
    formdata.append("comments", $('#comments').val());
    $.ajax({
        type: 'POST',
        url: ajaxurl,
        data: formdata,
        contentType: false,
        cache: false,

        processData: false,
        contentType: false,
        success: successfunction,
        error: errorfunction
    });


}