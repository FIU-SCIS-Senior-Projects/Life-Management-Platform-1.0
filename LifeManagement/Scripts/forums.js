
$(document).ready(function() {
    setupAllUp();
});

function setupAllUp() {
    coachLinksSetup();
    sendMsgBtnSetup();
    setCoachLinks();
  
}
function sendMsgBtnSetup() {
    $("#sendmessage").click(function() {
        var msg = $("#message").val();
        var userid = $("#username").val();
        var forumid = $("#forumid").val();
       
    });
}

function coachLinksSetup() {
   
    $('.coachlink').click(function () { //click event of the link
        $('.coachlinediv').removeClass('coachlink-active');
        $(this).children(".coachlinediv").addClass('coachlink-active');
        var forumid = $(this).attr('forumid');
        $("#forumid").val(forumid);
        renderConvosSection(forumid);
        renderFileSection(forumid);
        $('.nav-pills a[href="#1b"]').tab('show');
    });

}
function saveFile() {
    if ($("#attachment").val() != "") {
        if (($("#attachment")[0].files[0].size) >= 2097152)       //2MB
        {
            alert("File i too big");
            return;
        }
    }

    var forumid = $("#forumid").val();
    var formdata = new FormData();
    formdata.append("forumid", forumid);
    var fileInput = $('#attachment');
    $.each($(fileInput).get(0).files, function (index, value) {
        formdata.append(value.name, value);
    });
    $.ajax({
        url: '/Attachments/SaveFile/',
        type: 'POST',
        data: formdata,
        contentType: false,
        cache: false,
        processData: false,
        success: function (res) {
            renderFileSection(forumid);

        },
        error: function (msg) {

        }
    });
   
}
function setCoachLinks() {
  
    $('.coach-link').click(function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
       var coachid = $(this).attr('coachid');
        $.ajax({
            url: '/Forums/Create/',
            type: 'POST',
            data: { coachid: coachid },
            success: function(res) {
                renderCoachesSection();
               
            },
            error: function(msg) {

            }
        });
        return false;
    });

}

function renderConvosSection(forumid) {
    var data = { forumid: forumid };
    renderChatDiv("/Forums/ConvosSection", data, "#convossection");
}

function renderCoachesSection() {
  renderChatDiv("/Forums/ConversationUsers", null, "#coachessection");
   
         }

function renderFileSection(forumid) {
    var data = { forumid: forumid };
    renderChatDiv("/Forums/FilesSection", data, "#filessection");
   
}

function renderChatDiv(url, data, divid) {

    $.ajax({
        url: url,
        data: data,
        type: 'GET',
        success: function(res) {
            $(divid).html(res);
        },
        error: function(msg) {
            $(divid).html(msg);
        }
    });

}


   