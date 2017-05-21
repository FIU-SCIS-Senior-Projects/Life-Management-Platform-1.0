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