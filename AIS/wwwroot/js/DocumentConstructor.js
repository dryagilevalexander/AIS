class DocConstructor
{
    NumberOfContract;
    DateStart;
    DateEnd;
    PartnerId;
    SubjectOfContract;
    Cost;
    TypeOfStateRegId;
    ArticleOfLawId;
    DocumentTemplateId;
    IsCustomer;
    PlaceOfContract;
}

function change() {
    var val = document.getElementById('TypeOfStateRegId').value
    var parent = document.getElementById('articleoflaw');
    if (val == '1') {
        parent.style.display = '';
    } else {
        parent.style.display = 'none';
    }
}


async function ConstructContract() {

    var validateForm = $(document).find('#formContract');
    validateForm.validate();
    if (validateForm.valid())
    {
        currentContract = new DocConstructor();
        formContract = document.getElementById('formContract').value;
        currentContract.NumberOfContract = document.getElementById('NumberOfContract').value;
        currentContract.DateStart = document.getElementById('DateStart').value;
        currentContract.DateEnd = document.getElementById('DateEnd').value;
        currentContract.PartnerId = document.getElementById('PartnerId').value;
        currentContract.SubjectOfContract = document.getElementById('SubjectOfContract').value;
        currentContract.Cost = document.getElementById('Cost').value;
        currentContract.TypeOfStateRegId = document.getElementById('TypeOfStateRegId').value;
        currentContract.ArticleOfLawId = document.getElementById('ArticleOfLawId').value;
        currentContract.DocumentTemplateId = document.getElementById('DocumentTemplateId').value;
        currentContract.IsCustomer = document.getElementById('IsCustomer').value;
        currentContract.PlaceOfContract = document.getElementById('PlaceOfContract').value;

        let response = await fetch('/Documents/ShadowConstructDocument', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json;charset=utf-8'
            },
            body: JSON.stringify(currentContract)
        });

        if (response.status == 200) {
            let result = await response.text();
            addLink(result);
        }
        else {
            console.log("Server response: ", resonse.status);
        }
    }
}

function addLink(link) {
    // Создаем элемент ДИВ
    var div = document.createElement("div");
    div.className = "row pb-3";
    // Добавляем HTML-контент с пом. свойства innerHTML
    div.innerHTML = '<br><div class="col-sm-8"><a href = '+ link + '>Скачать проект договора</div>';
    // Добавляем новый узел в конец списка полей
    document.getElementById("projectDocumentLink").appendChild(div);
    // Возвращаем false, чтобы не было перехода по сслыке
    return true;
}
