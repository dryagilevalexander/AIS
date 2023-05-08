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