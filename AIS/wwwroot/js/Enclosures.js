var countOfFields = 1; // Текущее число полей
var curFieldNameId = 1; // Уникальное значение для атрибута name
var maxFieldLimit = 5; // Максимальное число возможных полей
function deleteField(a) {
    // Получаем доступ к ДИВу, содержащему поле
    var contDiv = (a.parentNode).parentNode;
    // Удаляем этот ДИВ из DOM-дерева
    contDiv.parentNode.removeChild(contDiv);
    // Уменьшаем значение текущего числа полей
    countOfFields--;
    // Возвращаем false, чтобы не было перехода по сслыке
    return false;
}

function addField() {
    // Проверяем, не достигло ли число полей максимума
    if (countOfFields >= maxFieldLimit) {
        alert("Число полей достигло своего максимума = " + maxFieldLimit);
        return false;
    }
    // Увеличиваем текущее значение числа полей
    countOfFields++;
    // Увеличиваем ID
    curFieldNameId++;
    // Создаем элемент ДИВ
    var div = document.createElement("div");
    div.className = "row pb-3";
    // Добавляем HTML-контент с пом. свойства innerHTML
    div.innerHTML = '<div class="col-sm-8"><input type="file" name="Enclosure" class="form-control" placeholder="Прикрепить файл" aria - label="Прикрепить файл"/></div><div class="col-sm-4"><button type="button" class="btn btn-success" onclick="return deleteField(this)">Удалить вложение</button></div>';
    // Добавляем новый узел в конец списка полей
    document.getElementById("parentId").appendChild(div);
    // Возвращаем false, чтобы не было перехода по сслыке
    return false;
}

function deleteFieldBySelector(a, b) {


    const xhr = new XMLHttpRequest();
    var currentUrl = '/Enclosures/DeleteMyEnclosure/' + b;
    xhr.open('POST', currentUrl);
    xhr.send();

    xhr.onload = () => {
        if (xhr.status == 200) {                // если код ответа 200
            var currentSelector = '.' + a;
            elems = document.querySelectorAll(currentSelector);
            for (i = 0; i < elems.length; i++) {
                elems[i].parentNode.removeChild(elems[i]);
            }


        } else {        // иначе выводим текст статуса
            console.log("Server response: ", xhr.statusText);
        }
    };
}