async function addTemplates() {
    $('#DocumentTemplates').empty();
    $('#DocumentTemplates').append('<option selected disabled>Выбор шаблона</option>');
    var typeOfContractId = document.getElementById('TypeOfContractId').value;
    var partnerId = document.getElementById('PartnerId').value;

    let dataForTemplate =
    {
        tCid: typeOfContractId,
        pId: partnerId

    }

    let response = await fetch('/Process/AddAccessibleTemplates', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(dataForTemplate)
    });

    if (response.status == 200)
    {
        let result = await response.json();
        for (i in result) {
            $('#DocumentTemplates').append('<option value="' + result[i]['id'] + '">' + result[i]['nameOfTemplate'] + '</option>');
        }
    }
    else
    {
        console.log("Server response: ", resonse.status);
    }
}