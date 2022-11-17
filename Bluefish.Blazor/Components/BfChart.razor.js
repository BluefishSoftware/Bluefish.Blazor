var numeralInit = false;

function initNumeral() {
    if (!numeralInit) {

        numeral.register('locale', 'gb', {
            delimiters: {
                thousands: ',',
                decimal: ','
            },
            abbreviations: {
                thousand: 'k',
                million: 'm',
                billion: 'b',
                trillion: 't'
            },
            ordinal: function (number) {
                return number === 1 ? 'er' : 'ème';
            },
            currency: {
                symbol: '£'
            }
        });
        numeral.locale('gb');

        numeralInit = true;
    }
}

export function initialize(el, config, ref) {

    // use numeral package
    if (typeof numeral !== 'undefined') {

        // ensure numeral is initialized
        initNumeral();

        // insert function to style Y axis ticks
        if (config?.options?.scales?.y?.ticks?.format) {
            var fmt = config.options.scales.y.ticks.format;
            config.options.scales.y.ticks.callback = function (value, index, values) {
                if (fmt.includes('%')) {
                    value = value / 100;
                }
                return numeral(value).format(fmt)
            }
        }

        // insert function to style Y2 axis ticks
        if (config?.options?.scales?.y2?.ticks?.format) {
            var fmt = config.options.scales.y2.ticks.format;
            config.options.scales.y2.ticks.callback = function (value, index, values) {
                if (fmt.includes('%')) {
                    value = value / 100;
                }
                return numeral(value).format(fmt)
            }
        }

        // tooltip callbacks
        if (config?.data?.datasets) {
            for (var i = 0; i < config.data.datasets.length; i++) {
                var ds = config.data.datasets[i];
                if (ds.format) {
                    if (!ds.tooltip) {
                        ds.tooltip = {}
                    }
                    if (!ds.tooltip.callbacks) {
                        ds.tooltip.callbacks = {}
                    }
                    ds.tooltip.callbacks = {
                        label: function (item, obj) {
                            var value = item.raw;
                            if (item.dataset.format.includes('%')) {
                                value = value / 100;
                            }
                            return numeral(value).format(item.dataset.format);
                        }
                    };
                }
            }
        }
    }

    return new Chart(el, config);
}

export function update(chart, data) {
    if (chart) {
        if (data) {
            chart.data = data;
        }
        chart.update();
    }
}

export function updateScaleX(chart, scale) {
    if (chart?.options?.scales?.x) {
        chart.options.scales.x = scale
    }
}

export function updateScaleY2(chart, min) {
    if (chart?.options?.scales?.y2) {
        chart.options.scales.y2.min = min;
    }
}

export function updateValue(chart, datasetIdx, idx, value) {
    if (chart) {
        if (datasetIdx < chart.data.datasets.length && idx < chart.data.datasets[datasetIdx].data.length) {
            chart.data.datasets[datasetIdx].data[idx] = value;
        }
    }
}

export function clearValues(chart, datasetIdx) {
    if (chart) {
        if (datasetIdx < chart.data.datasets.length) {
            chart.data.datasets[datasetIdx].data = [];
        }
    }
}

export function setValues(chart, datasetIdx, values) {
    if (chart) {
        if (datasetIdx < chart.data.datasets.length) {
            chart.data.datasets[datasetIdx].data = values;
        }
    }
}