
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
    debugger;
    $('.coachlink').click(function () { //click event of the link
        $('.coachlinediv').removeClass('coachlink-active');
        $(this).children(".coachlinediv").addClass('coachlink-active');
        var forumid = $(this).attr('forumid');
        $("#forumid").val(forumid);
        renderConvosSection(forumid);
        $('.nav-pills a[href="#1b"]').tab('show');
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
   $(renderChatDiv("/Forums/ConversationUsers", null, "#coachessection")).then( setupAllUp());
   
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


   