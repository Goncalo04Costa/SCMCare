$(document).ready(function () {
    $('#avariaForm').on('submit', function (event) {
        event.preventDefault();

        var data = $('#data').val();
        var equipamentosId = $('#equipamentosId').val();
        var descricao = $('#descricao').val();
        var estado = $('#estado').val();

        var avaria = {
            data: data,
            equipamentosId: parseInt(equipamentosId),
            descricao: descricao,
            estado: parseInt(estado)
        };

        $.ajax({
            url: 'https://localhost:5001/api/Avarias',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(avaria),
            success: function (response) {
                $('#responseMessage').text(response).addClass('alert alert-success');
                $('#avariaForm')[0].reset();
            },
            error: function (xhr, status, error) {
                $('#responseMessage').text(`Erro: ${xhr.responseText}`).addClass('alert alert-danger');
            }
        });
    });
});
