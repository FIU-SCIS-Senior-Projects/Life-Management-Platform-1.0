

$(document).ready(function () {
    setReviewDivs();
   
})

function setReviewDivs() {
    $('.review-div').click(function() {
        $(this).toggleClass("review-expanded");
    })
}

