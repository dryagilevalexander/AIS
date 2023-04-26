function openModal(parameters)
{
    const id = parameters.data;
    const url = parameters.url;
    const modal = $('#modal');

    if (id === undefined || url === undefined)
    {
            alert("Ошибка загрузки модального окна")
            return;
    }

        $.ajax({
                    type: 'GET',
                    url: url,
                    data:{ "id": id },
                        success: function (response)
                        {
                            modal.find(".modal-body").html(response);
                            jQuery.noConflict();
                            modal.modal('show');
                        },
                        failure: function ()
                        {
                           modal.modal('hide');
                        },
                        error: function (response)
                        {
                           alert(response.responseText);
                        }
            });
};