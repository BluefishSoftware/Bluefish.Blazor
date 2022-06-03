export function initialize(id, ref) {
	var el = document.getElementById(id);
	if (el) {
		el.addEventListener("shown.bs.dropdown", function () {
			if (ref) {
				ref.invokeMethodAsync("OnDropDownShown");
			}
		});
		el.addEventListener("hidden.bs.dropdown", function () {
			if (ref) {
				ref.invokeMethodAsync("OnDropDownHidden");
			}
		});
		return new bootstrap.Dropdown(el);
	}
	return null;
}