export function focus(id) {
	var el = document.getElementById(id);
	if (el && el.focus) {
		el.focus();
	}
}

export function selectText(id, start, end) {
	var el = document.getElementById(id);
	if (!el) return;
	if (!start) start = 0;
	if (!end) end = el.value.length;
	if (el.createTextRange) {
		var selRange = el.createTextRange();
		selRange.collapse(true);
		selRange.moveStart('character', start);
		selRange.moveEnd('character', end);
		selRange.select();
		el.focus();
	} else if (el.setSelectionRange) {
		el.focus();
		el.setSelectionRange(start, end);
	} else if (typeof el.selectionStart != 'undefined') {
		el.selectionStart = start;
		el.selectionEnd = end;
		el.focus();
	}
}

export function findAncestor(el, cls) {
	while ((el = el.parentElement) && !el.classList.contains(cls));
	return el;
}

export function openUrl(url, target) {
	window.open(url, target);
}

export function debounce(func, wait) {
	let timeout;
	return function executedFunction(...args) {
		const later = () => {
			timeout = null;
			func(...args);
		};
		clearTimeout(timeout);
		timeout = setTimeout(later, wait);
	};
}