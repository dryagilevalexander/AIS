async function getPartialView()
{
    const id = document.getElementById('PartnerTypeId').value;
    const partialCreatePartner = $('#partialCreatePartner');
    let url;

    if (id === '1') {
        url = '/Partners/CreatePartnerOrganization/';
    }

    if (id === '2') {
        url = '/Partners/CreatePartnerIP/';
    }

    if (id === '3') {
        url = '/Partners/CreatePartnerFL/';
    }

    let response = await fetch(url);
    if (response.ok) {
        let data = await response.text();
        partialCreatePartner.html(data);

        $('.form-select').multiselect({
            buttonWidth: '100%'
        });
    }
    else {
        alert("Ошибка HTTP: " + response.status);
    }
}

async function fillInAutomatically() {
    
    const inn = document.getElementById('INN').value;
    let url = '/Partners/FillInAutomaticallyPartnerOrganization/' + inn;

    let response = await fetch(url);
    if (response.ok) {
        let data = await response.text();
        var obj = JSON.parse(data);
        document.getElementById('KPP').value = obj.kpp;
        document.getElementById('Address').value = obj.address;
        document.getElementById('Name').value = obj.name;
        document.getElementById('ShortName').value = obj.shortName;
        document.getElementById('OGRN').value = obj.ogrn;
        var directorTypeId = document.getElementById('DirectorTypeId');
        directorTypeId.value = obj.directorTypeId;
        document.getElementById('DirectorName').value = obj.directorName;
    }
    else {
        alert("Ошибка HTTP: " + response.status);
    }

    
}