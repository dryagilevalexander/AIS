
    $(function () {
        setMask('#INN', '9999999999');
        setMask('#INNFl', '999999999999');
        setMask('#KPP', '999999999');
        setMask('#OGRN', '9999999999999');
        setMask('#Account', '99999999999999999999');
        setMask('#CorrespondentAccount', '99999999999999999999');
        setMask('#BIK', '999999999');
        setMask('#PhoneNumber', '+7(999)-999-9999');
        setMask('#PassportSeries', '9999');
        setMask('#PassportNumber', '999999');
        setMask('#PassportDivisionCode', '999-999');
    });

    $(function () {
        let $email = $('#Email');
        let cursor = $email[0].selectionStart;
        let prev = $email.val();

        $email.inputmask({
            mask: "*{1,50}[.*{1,50}][.*{1,50}]@*{1,50}.*{1,20}[.*{1,20}][.*{1,20}]",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
            definitions: {
                '*': {
                    validator: "[^_@.]"
                }
            }

        }).on('keyup paste', function () {
            if (this.value && /[^_a-zA-Z0-9@\-.]/i.test(this.value)) {
                this.value = prev;
                this.setSelectionRange(cursor, cursor);
                $input.trigger('input');
            } else {
                cursor = this.selectionStart;
                prev = this.value;
            }
        });
    });

function setMask(selector, mask) {
    $(selector).inputmask({
        mask,
        greedy: false,
        clearIncomplete: true,
        showMaskOnHover: false,
    });
}