/*
Template Name: STUDIO ASP - Responsive Bootstrap 5 Admin Template
Version: 6.0.0
Author: Sean Ngu
Website: http://www.seantheme.com/studio/
*/

var handleRenderTags = function() {
	$('#jquery-tagit').tagit({
		fieldName: 'tags',
		availableTags: ['c++', 'java', 'php', 'javascript', 'ruby', 'python', 'c'],
		autocomplete: {
			delay: 0, 
			minLength: 2
		}
	});
};


/* Controller
------------------------------------------------ */
$(document).ready(function() {
	handleRenderTags();
});