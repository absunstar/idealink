var Shuffle = window.Shuffle;

class Demo {
constructor(element) {
this.element = element;
this.shuffle = new Shuffle(element, {
itemSelector: '.tour-item',
});

this.addSorting();

}

addSorting() {
const buttonGroup = document.querySelector('.sort-options');
if (!buttonGroup) {
return;
}
buttonGroup.addEventListener('change', this._handleSortChange.bind(this));
}

_handleSortChange(evt) {
const buttons = Array.from(evt.currentTarget.children);
buttons.forEach((button) => {
if (button.value === evt.target.value) {
button.selected = true;
} else {
button.selected = false;
}
});

const { value } = evt.target;
let options = {};

function sortByMinDays(element) {
return element.getAttribute('data-days').toLowerCase();
}

function sortByMaxDays(element) {
return element.getAttribute('data-days').toUpperCase();
}

function sortByLowPrice(element) {
return element.getAttribute('data-price').toLowerCase();
}

function sortByHightPrice(element) {
return element.getAttribute('data-price').toUpperCase();
}

if (value === 'min-days') {
options = {
by: sortByMinDays,
};
} else if (value === 'max-days') {
options = {
reverse: true,
by: sortByMaxDays,
};
} else if (value === 'low-price') {
options = {
by: sortByLowPrice,
};
} else if (value === 'hight-price') {
options = {
reverse: true,  
by: sortByHightPrice,
};
}
this.shuffle.sort(options);
}

}

document.addEventListener('DOMContentLoaded', () => {
window.demo = new Demo(document.getElementById('grid'));
});


var width = $('.container-grid').css('width');
var widthClean = width.replace('px', '');
var newWidth = widthClean - 16;
var newWidth = $('.container-grid').css('max-width', newWidth);

$(document).ready(function() {
setTimeout(function() {
var height = $('#grid').css('height');
var heightClean = height.replace('px', '');
var newheight = parseInt(heightClean) + 15;
var newheight = $('#grid').css('height', newheight);
	}, 500);
	});

$('.sort-options').on('change', function() {
	setTimeout(function() {
var height = $('#grid').css('height');
var heightClean = height.replace('px', '');
var newheight = parseInt(heightClean) + 15;
var newheight = $('#grid').css('height', newheight);
		}, 500);
});




