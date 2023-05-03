class DocConstructor
{
    NumberOfСourtСase;
    DateOfOrder;
    PartnerId;
    CourtId;
    SubjectOfOrder;
}

async function ConstructStatement() {

    statement = new DocConstructor();
    formStatement = document.getElementById('formStatement').value;
    statement.NumberOfCourtCase = document.getElementById('NumberOfCourtCase').value;
    statement.DateOfOrder = document.getElementById('DateOfOrder').value;
    statement.PartnerId = document.getElementById('PartnerId').value;
    statement.CourtId = document.getElementById('CourtId').value;
    statement.SubjectOfOrder = document.getElementById('SubjectOfOrder').value;

    let response = await fetch('/Process/ShadowConstructStatementCancelOfCourtOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify(statement)
    });

    if (response.status == 200) {
        let result = await response.text();
        addLink(result);
    }
    else {
        console.log("Server response: ", resonse.status);
    }
}

function addLink(link) {
    // Создаем элемент ДИВ
    var div = document.createElement("div");
    div.className = "row pb-3";
    // Добавляем HTML-контент с пом. свойства innerHTML
    div.innerHTML = '<br><div class="col-sm-8"><a href = '+ link + '>Скачать проект заявления</div>';
    // Добавляем новый узел в конец списка полей
    document.getElementById("projectDocumentLink").appendChild(div);
    // Возвращаем false, чтобы не было перехода по сслыке
    return true;
}
