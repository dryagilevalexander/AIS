
    $(function () {
        let $kpp = $('#KPP');
        let $ogrn = $('#OGRN');
        let $account = $('#Account');
        let $correspondentaccount = $('#CorrespondentAccount');
        let $bik = $('#BIK');
        let $inn = $('#INN');
        let $innfl = $('#INNFl')
        let $phonenumber = $('#PhoneNumber');
        let $passportseries = $('#PassportSeries');
        let $passportnumber = $('#PassportNumber');
        let $passportdivisioncode = $('#PassportDivisionCode');

        $kpp.inputmask({
            mask: "999999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $ogrn.inputmask({
            mask: "9999999999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $account.inputmask({
            mask: "99999999999999999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $correspondentaccount.inputmask({
            mask: "99999999999999999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $bik.inputmask({
            mask: "999999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $inn.inputmask({
            mask: "9999999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $innfl.inputmask({
            mask: "999999999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $phonenumber.inputmask({
            mask: "+7(999)-999-9999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $passportseries.inputmask({
            mask: "9999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $passportnumber.inputmask({
            mask: "999999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });

        $passportdivisioncode.inputmask({
            mask: "999-999",
            greedy: false,
            clearIncomplete: true,
            showMaskOnHover: false,
        });
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