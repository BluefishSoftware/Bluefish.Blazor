﻿export function InitialiseTextEditor(id, value, reference, height, width, resize) {
	var options = {
		selector: '#' + id, setup: function (ed) {
			ed.on('Change', function (e) {
				var text = e.target.getContent();
				return reference.invokeMethodAsync("OnChange", text);
			});
		},
		menubar: false,
		//placeholder: 'Type here...',
		resize: resize,
		statusbar: false
	};
	if (width) {
		options.width = width;
	}
	if (height) {
		options.height = height;
	}
	tinymce.init(options)
		.then(function (editors) {
			if (value) {
				tinymce.get(id).setContent(value);
			}
		});
}

export function SetTextEditorValue(id, value) {
	return tinymce.get(id).setContent(value);
}

//export function GetTextEditorValue(id) {
//	return tinymce.get(id).getContent();
//}

export function RemoveTextEditor(id) {
	var editor = tinymce.get(id);
	if (editor) {
		editor.off('init');
		editor.off('beforeInput');
		editor.remove();
	}
}