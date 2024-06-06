/*PDS
Daniela Brito nº25591
Daniela Pereira nº25988
Sofia Carvalho nº25991
Diogo Fernandes nº26008
Gonçalo Costa nº26024
LESI-IPCA */

$(document).ready(function () {
    function CarregarDados() {
        $.ajax({
            url: '/api/materiais', 
            method: 'GET',
            dataType: 'json',
            success: function (response) {
                adicionarTabela(response);
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    }

    function adicionarTabela(data) {
        const table = $('.rounded-table');
        data.forEach(material => {
            const isStockLow = ContaCorrenteMateriais.Quantidade <= material.Limite;
            const checkBox = isStockLow ? '<input type="checkbox" checked disabled>' : '<input type="checkbox" disabled>';
            const row = `
                <tr>
                    <td>${material.Nome}</td>
                    <td>${ContaCorrenteMateriais.Quantidade}</td>
                    <td>${material.Funcionario}</td>
                    <td>${checkBox}</td>
                    <td><button class="btn btn-primary">Encomendar</button></td>
                </tr>
            `;
            table.append(row);
        });
    }

    CarregarDados();
});
