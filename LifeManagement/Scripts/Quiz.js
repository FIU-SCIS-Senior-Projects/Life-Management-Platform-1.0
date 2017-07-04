function setAnswerLis() {
    $(".answer-li").click(function() {
        var question = $(this).attr("question");
       

        $(".answer-li").each(function() {
            if ($(this).attr("question") == question) {
                $(this).removeClass("answer-li-selected");
            }
        });

        $(this).addClass("answer-li-selected");
    });
}

function seeResult() {
    var quiztotal = 0;
    $(".answer-li-selected").each(function() {
        quiztotal = quiztotal + parseInt($(this).attr("value"));
    });
    $("#quiztotal").val(quiztotal);
}
$(document).ready(function() {
    setAnswerLis();
})