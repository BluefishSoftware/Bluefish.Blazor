export function initialize(idSelector, opt, dnRef) {
	var el = document.querySelector(idSelector);
	if (el) {
		if (opt.previewItemTemplate) {
			var pEl = document.querySelector(opt.previewItemTemplate);
			if (pEl) {
				opt.previewTemplate = pEl.innerHTML;
			}
		}
		opt.init = function () {
			this.on("queuecomplete", function () {
				dnRef.invokeMethodAsync('Bluefish.Blazor.BfDropzone.OnAllUploadsComplete');
			});
		};
		el.dropzone = new Dropzone(idSelector, opt);
	}
}

export function clear(idSelector, opt) {
	var el = document.querySelector(idSelector);
	if (el.dropzone) {
		el.dropzone.removeAllFiles();
	}
}

export function click(idSelector) {
	var el = document.querySelector(idSelector);
	if (el && el.parentElement) {
		el.parentElement.click();
	}
}