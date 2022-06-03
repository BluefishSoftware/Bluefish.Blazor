var refs = {};

export function initialize(id, ref) {
	var el = document.getElementById(id);
	if (el) {
		el.addEventListener("keydown", onKeyDown);
		refs[id] = ref;
	}
}

export function dispose(id, keys, ref) {
	var el = document.getElementById(id);
	if (el) {
		el.removeEventListener("keydown", onKeyDown);
		if (refs[id]) {
			delete refs[id];
		}
	}
}

function onKeyDown(ev) {
	if (ev.code == 'Escape' || ev.code == 'Enter') {
		var id = this.id;
		if (id && refs[id]) {
			if (ev.code == 'Escape') {
				refs[id].invokeMethodAsync("OnEscapeKey");
			}
			else if (ev.code == 'Enter') {
				refs[id].invokeMethodAsync("OnEnterKey");
			}
		}
	}
}