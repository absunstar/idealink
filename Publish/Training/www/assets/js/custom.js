$('#widget').draggable();

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

$(document).ready(function(){
    $('.customer-logos').slick({
      slidesToShow: 14,
      slidesToScroll: 1,
      autoplay: false,
      autoplaySpeed: 1000,
      arrows: false,
      dots: false,
      pauseOnHover: false,
      responsive: [{
        breakpoint: 768,
        settings: {
          slidesToShow: 4
        }
      }, {
        breakpoint: 520,
        settings: {
          slidesToShow: 3
        }
      }]
    });
  });

// Page Content Margin
var hh = $("header").height();
var fh = $("footer").height();
$("main").css("padding-top",hh).css("padding-bottom",fh);


	
// Open Responsive Menu
$(function () {
$('[data-toggle="offcanvas"]').on('click', function (){
$('.offcanvas-collapse').toggleClass('open');
if($("#header-top").hasClass('show')){
$("#header-top").removeClass('show');	
}else{
$("body").toggleClass("modal-open");
}
});
});
	
$(".header-top-mob").click(function(){
if($('.offcanvas-collapse').hasClass('open')){
$('.offcanvas-collapse').removeClass('open');
}else{
$("body").toggleClass("modal-open");
}
});

	
	
// Read Only
$('#tourbooktourname').prop('readonly', 'true');
	

	
// Form Validation	
$( "#signinform" ).validate({
rules: {
signinemail: {
required: true,
email: true
},
signinpassword: {
required: true,
minlength: 8
},
},
messages: {
signinemail: "Please enter a valid email address",
signinpassword: {
required: "Please provide a password",
minlength: "Your password must be at least 8 characters long",
},
},
errorElement: "em",
errorPlacement: function ( error, element ) {
error.addClass( "help-block" );
if ( element.prop( "type" ) === "checkbox" ) {
error.insertAfter( element.parent( "label" ) );
} else {
error.insertAfter( element );
}
},
highlight: function ( element, errorClass, validClass ) {
$( element ).parents( ".md-form" ).addClass( "has-error" ).removeClass( "has-success" );
},
unhighlight: function (element, errorClass, validClass) {
$( element ).parents( ".md-form" ).addClass( "has-success" ).removeClass( "has-error" );
}
});



// Animations init
new WOW().init();


	
// Max Text
$.fn.findNext = function(selector, steps, scope)
{
if (steps)
{
steps = Math.floor(steps);
}
else if (steps === 0)
{
return this;
}
else
{
var next = this.next(selector);
if (next.length)
return next;
steps = 1;
}
scope = (scope) ? $(scope) : $(document);
var kids = this.find(selector);
hay = $(this);
while(hay[0] != scope[0])
{
hay = hay.parent();     
var rs = hay.find(selector).not(kids).add($(this));
var id = rs.index(this) + steps;
if (id > -1 && id < rs.length)
return $(rs[id]);
}
return $([]);
}

$(".card-title").each(function() {
var cartTitleHieght = $(this).height();
var cartDestHieght = $(this).findNext('.card-dest').height();
descLen = $(this).findNext('.max-text').text().length;
if(cartTitleHieght + cartDestHieght <= 48) {
$(this).findNext(".max-text").text($(this).findNext(".max-text").text().substr(0,200)+'...');
} else if (cartTitleHieght + cartDestHieght <= 72) {
$(this).findNext(".max-text").text($(this).findNext(".max-text").text().substr(0,150)+'...');
} else if (cartTitleHieght + cartDestHieght <= 96) {
$(this).findNext(".max-text").text($(this).findNext(".max-text").text().substr(0,100)+'...');
}
});
	
$(function(){
$(".max-text-02").each(function(i){
len=$(this).text().length;
if(len>160)
{
$(this).text($(this).text().substr(0,160)+'...');
}
});
});
	


// Light Gallery
$('#lightgallery-01').lightGallery();
$('#lightgallery-02').lightGallery();
$('#lightgallery-03').lightGallery();


	
// Show All Tabs
$(".tab-pane").removeClass("fade").addClass("active").addClass("show");
$("#show_all").on("click", function() {
$(".dropdown-item").removeClass("active");
$(this).addClass("active").parent("li").siblings().find("a").removeClass("active");
$(".tab-pane").removeClass("fade").addClass("active").addClass("show");
});
$(".dropdown-item").not("#show_all").on("click", function() {

$(".tab-pane").not(this.hash).removeClass("active").removeClass("show");
});

	

// Carousel Multi
function multiCarousel(screenWidthFrom, screenWidthTo, SlidesToShow, greaterOrLower) {
if (greaterOrLower == 0) {
if ($(window).innerWidth() >= screenWidthFrom && $(window).innerWidth() <= screenWidthTo) {
$('.autoplay').slick({
slidesToShow: SlidesToShow,
slidesToScroll: 1,
autoplay: true,
autoplaySpeed: 2000,
});
}
}
if (greaterOrLower == 1) {
if ($(window).innerWidth() >= screenWidthFrom) {
$('.autoplay').slick({
slidesToShow: SlidesToShow,
slidesToScroll: 1,
autoplay: true,
autoplaySpeed: 2000,
});
}
}
}
multiCarousel(0, 480, 1, 0);
multiCarousel(481, 992, 2, 0);
multiCarousel(993, 1400, 3, 0);
multiCarousel(1600, null, 4, 1);

function multiCarousel2(screenWidthFrom, screenWidthTo, SlidesToShow, greaterOrLower) {
if (greaterOrLower == 0) {
if ($(window).innerWidth() >= screenWidthFrom && $(window).innerWidth() <= screenWidthTo) {
$('.autoplay-02').slick({
slidesToShow: SlidesToShow,
slidesToScroll: 1,
autoplay: true,
autoplaySpeed: 2000,
});
}
}
if (greaterOrLower == 1) {
if ($(window).innerWidth() >= screenWidthFrom) {
$('.autoplay-02').slick({
slidesToShow: SlidesToShow,
slidesToScroll: 1,
autoplay: true,
autoplaySpeed: 2000,
});
}
}
}
multiCarousel2(0, 480, 1, 0);
multiCarousel2(481, 992, 1, 0);
multiCarousel2(993, 1400, 1, 0);
multiCarousel2(1600, null, 1, 1);	



// Carousel if have one item
if($('.carousel-inner .carousel-item').is(':only-child')) { 
$('.carousel-indicators').css('display', 'none');
$('.carousel-control-prev').css('display', 'none');
$('.carousel-control-next').css('display', 'none');
}


	
// Slider Range Price
$( function() {
$( "#slider-price" ).slider({
range: true,
min: 250,
max: 1500,
values: [ 250, 1500 ],
slide: function( event, ui ) {
$( "#amount-price" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
}
});
$( "#amount-price" ).val( "$" + $( "#slider-price" ).slider( "values", 0 ) +
" - $" + $( "#slider-price" ).slider( "values", 1 ) );
} );
	
	
// Slider Range Price
$( function() {
$( "#slider-days" ).slider({
range: true,
min: 4,
max: 15,
values: [ 4, 15 ],
slide: function( event, ui ) {
$( "#amount-days" ).val( "" + ui.values[ 0 ] + " - " + ui.values[ 1 ] );
}
});
$( "#amount-days" ).val( "" + $( "#slider-days" ).slider( "values", 0 ) +
" - " + $( "#slider-days" ).slider( "values", 1 ) );
} );



// jsSocials
$("#shareIcons").jsSocials({
showLabel: false,
showCount: false,
shares: ["facebook", "twitter", "googleplus", "linkedin", "whatsapp", "email", "pinterest", "stumbleupon"]
});




});


