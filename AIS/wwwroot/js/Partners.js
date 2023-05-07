async function getPartialView()
{
    const id = document.getElementById('PartnerTypeId').value;
    const partialCreatePartner = $('#partialCreatePartner');
    let url;

    if (id === '1') {
        url = '/Process/CreatePartnerOrganization/';
    }

    if (id === '2') {
        url = '/Process/CreatePartnerIP/';
    }

    if (id === '3') {
        url = '/Process/CreatePartnerFL/';
    }

    let response = await fetch(url);
    if (response.ok) {
        let data = await response.text();
        partialCreatePartner.html(data);
        multiselectForPartialView();
    }
    else {
        alert("Ошибка HTTP: " + response.status);
    }
}

function multiselectForPartialView() {
    $('.form-select').multiselect({
        buttonWidth: '100%'
    });
}