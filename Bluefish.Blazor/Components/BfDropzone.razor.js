var _url = "";

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
        // default url to those set in options
        _url = opt.url;
        opt.url = getUrl;
        el.dropzone = new Dropzone(idSelector, opt);
    }
}

function getUrl(files) {
    return _url;
}

export function setUrl(url) {
    _url = url;
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