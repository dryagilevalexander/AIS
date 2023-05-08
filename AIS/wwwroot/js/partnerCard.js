    async function myFunction(partnerId) {
        let url = '/Partners/PartnerCard/' + partnerId;
    let response = await fetch(url);
    if (response.ok) {
        let data = await response.text();
    $('#dialogContent').html(data);
    $('#modDialog').modal('show');
            }
    else {
        alert("Ошибка HTTP: " + response.status);
            }

        }