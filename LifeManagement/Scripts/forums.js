
$(document).ready(function() {
    coachLinksSetup();
    sendMsgBtnSetup();
})

function sendMsgBtnSetup() {
    $("#sendmessage").click(function() {
        var msg = $("#message").val();
        var userid = $("#username").val();
        var forumid = $("#forumid").val();
       
    })
}
function coachLinksSetup() {
    $('.coachlink').click(function () { //click event of the link
        $('.coachlinediv').removeClass('coachlink-active');
        $(this).children(".coachlinediv").addClass('coachlink-active');
        var forumid = $(this).attr('forumid');
        $("#forumid").val(forumid);
        renderConvosSection(forumid);
        $('.nav-pills a[href="#1b"]').tab('show')
    });

}
 
         function renderConvosSection(forumid) {
             var data = { forumid: forumid };
             renderChatDiv("/Forums/ConvosSection", data, "#convossection");
         }

         function renderChatDiv(url, data,divid) {

             $.ajax({
                 url: url,
                 data: data,
                 type: 'GET',
                 success: function (res) {
                     $(divid).html(res);
                 },
                 error: function (msg) {
                     $(divid).html(msg);
                 }
             });

         }


     