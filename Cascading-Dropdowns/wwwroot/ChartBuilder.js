let loaded = false;

let PrevCalculationRadio = '';

const PercentilesVal = 'PERCENTILES';

AddListeners();

Blazor.addEventListener('enhancedload', function () {
    AddListeners();
    var radioChecked = document.querySelector('input[type="radio"][checked]');
    if (radioChecked !== null) {
        radioChecked.checked = true;
    }
});

function AddListeners() {
    const go_btn = document.getElementById('goBtn');
    if (!loaded && go_btn !== undefined) {
        loaded = true;
        go_btn.addEventListener('click', function (t) {
            SetHidden(t);
        });

        document.addEventListener('change', function (e) {

            if (e.target.classList.contains('calc-radio')) {
                var curr_val = e.target.value;
                if (
                    (curr_val === PercentilesVal && PrevCalculationRadio !== PercentilesVal)
                    ||
                    (curr_val !== PercentilesVal && PrevCalculationRadio === PercentilesVal)
                ) {
                    SetHidden(e);
                }
                PrevCalculationRadio = curr_val;
            } else {
                if (e.target.dataset['change_type'] !== undefined) {
                    SetHidden(e);
                }
            }
        });
    }
}

function SetHidden(t) {
    var type = t.target.dataset['change_type'];
    document.getElementById('submitType').value = type;
    const submit = document.getElementById('submitBtn');
    submit.click();
}