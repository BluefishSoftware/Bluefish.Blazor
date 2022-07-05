export function initialize(id, ref) {
    var el = document.getElementById(id);
    if (el) {
        el.addEventListener("shown.bs.modal", function () {
            if (ref) {
                ref.invokeMethodAsync("OnModalShown");
            }
        });
        el.addEventListener("hidden.bs.modal", function () {
            if (ref) {
                ref.invokeMethodAsync("OnModalHidden");
            }
        });
        el.addEventListener("keydown", function (ev) {
            if (ev.code == 'Enter') {
                // shift focus to save button and click
                //var buttonEl = document.getElementById(saveButtonId);
                //if (buttonEl) {
                //    buttonEl.click();
                //}
                ref.invokeMethodAsync("OnEnterKey");
            }
        });
        return new bootstrap.Modal(el);
    }
    return null;
}