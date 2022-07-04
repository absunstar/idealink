
$(document).ready(function () {


// Scrolling Header
var lastScrollTop = 0;
$(window).scroll(function(){
var st = $(this).scrollTop();
if (st > lastScrollTop){
$("header").addClass("scrolling");
} else {
$("header").removeClass("scrolling");
}
lastScrollTop = st;
});



// Page Content Margin
var hh = $("header").height();
var fh = $("footer").height();
$("main").css("padding-top",hh).css("padding-bottom",fh);



	

	



});


