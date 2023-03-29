function change() {
    var val = document.getElementById('PartnerTypeId').value;
    var legalEntity = document.getElementById('legalEntity');
    var businessman = document.getElementById('businessman');
    var individual = document.getElementById('individual');
    var legalEntityPartnerTypeId = document.getElementById('legalEntityPartnerTypeId');
    var businessmanPartnerTypeId = document.getElementById('businessmanPartnerTypeId');
    var individualPartnerTypeId = document.getElementById('individualPartnerTypeId');
    document.form1.reset();
    document.form2.reset();
    document.form3.reset();
    legalEntityPartnerTypeId.value = val;
    businessmanPartnerTypeId.value = val;
    individualPartnerTypeId.value = val;
    if (val == '1') {
        legalEntity.style.display = '';
    } else {
        legalEntity.style.display = 'none';
    }
    if (val == '2') {
        businessman.style.display = '';
    } else {
        businessman.style.display = 'none';
    }
    if (val == '3') {
        individual.style.display = '';
    } else {
        individual.style.display = 'none';
    }
  
}