
$(document).ready(setpopups);

function setpopups() {


    $('.lmmodal').click(function () { //click event of the link
        var modalwidth = $(this).attr('modal-width');
        modalwidth = modalwidth == null ? "500px" : modalwidth;
        var modalcolor = $(this).attr('modal-color');
        modalcolor= modalcolor == null ? "white" : modalcolor;

        var htmlmodal = $(this).attr('htmlmodal');
        htmlmodal == null ? false : htmlmodal;

        if (!htmlmodal) {
            //load the dialog via ajax
            $.ajax({
                url: $(this).attr('url'),
                type: "GET",
                dataType: "html",
                cache: false,
                success: function (data) {
                    $('#myDialogContainer').html(data); //write the dialog content into the diaog container
                    $(".modal-dialog").css("width", modalwidth);
                    $(".modal-content").css("background-color", modalcolor);
                    $("#basicModal").modal("show"); //open it!
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('Error');
                }
            });
        }
        else {
            var htmlmodalid = $(this).attr('htmlmodalid');
            var data = $(htmlmodalid).html()
            $('#myDialogContainer').html(data); //write the dialog content into the diaog container
            $(".modal-dialog").css("width", modalwidth)
            $("#basicModal").modal("show"); //open it!

        }

    });
}
