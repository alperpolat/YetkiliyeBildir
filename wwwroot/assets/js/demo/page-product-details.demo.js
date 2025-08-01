/*
Template Name: STUDIO ASP - Responsive Bootstrap 5 Admin Template
Version: 6.0.0
Author: Sean Ngu
Website: http://www.seantheme.com/studio/
*/

var handleSummernote = function() {
	$('.summernote').summernote({
    height: 300
  });
};

var handleTag = function() {
  $('#tags').tagit({
    fieldName: 'tags',
		availableTags: ['c++', 'java', 'php', 'javascript', 'ruby', 'python', 'c'],
		autocomplete: {
			delay: 0, 
			minLength: 2
		}
	});
	
  $('#tag-size').tagit({
    fieldName: 'tag-size',
		availableTags: [],
		autocomplete: {
			delay: 0, 
			minLength: 2
		}
	});
	
  $('#tag-color').tagit({
    fieldName: 'tag-color',
		availableTags: [],
		autocomplete: {
			delay: 0, 
			minLength: 2
		}
	});
	
  $('#tag-material').tagit({
    fieldName: 'tag-material',
		availableTags: [],
		autocomplete: {
			delay: 0, 
			minLength: 2
		}
	});
};

var handlejQueryFileUpload = function() {
	$('#fileupload').fileupload({
		previewMaxHeight: 80,
		previewMaxWidth: 120,
		url: '//jquery-file-upload.appspot.com/',
		disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
		maxFileSize: 999000,
		acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i
	});
	$('#fileupload').on('fileuploadchange', function (e, data) {
		$('#fileupload .empty-row').hide();
	});
	$('#fileupload').on('fileuploadfail', function(e, data){
		if (data.errorThrown === 'abort') {
			if ($('#fileupload .files tr').not('.empty-row').length == 1) {
				$('#fileupload .empty-row').show();
			}
		}
	});
	
	if ($.support.cors) {
		$.ajax({
			url: '//jquery-file-upload.appspot.com/',
			type: 'HEAD'
		}).fail(function () {
			var alert = '<div class="alert alert-danger m-b-0 m-t-15">Upload server currently unavailable - ' + new Date() + '</div>';
			$('#fileupload #error-msg').removeClass('d-none').html(alert);
		});
	}
};


/* Controller
------------------------------------------------ */
$(document).ready(function() {
	handleSummernote();
	handleTag();
	handlejQueryFileUpload();
});