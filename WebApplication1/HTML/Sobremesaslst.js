/*PDS
Daniela Brito nº25591
Daniela Pereira nº25988
Sofia Carvalho nº25991
Diogo Fernandes nº26008
Gonçalo Costa nº26024
LESI-IPCA */

const params = new URLSearchParams(window.location.search);
const tipoUtilizador = params.get('usr');

function CarregarDados() {	
    $.ajax({
        url: '/api/Sobremesas',
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            CriarTabela(response);
        },
        error: function(xhr, status, error) {
            // Handle error
            console.error(xhr.responseText);
        }
    });
}

function CriarTabela(dados) {
    var tabela = document.getElementById('tabela');
    tabela.innerHTML = ''; // Limpa a tabela antes de criar novamente

    var tabelaHead = tabela.createTHead().insertRow(0);
    var tabelaBody = tabela.createTBody();

    var colunas = [
        { nome: 'Id', classe: 'col-2' },
        { nome: 'Nome', classe: 'col-8' },
        { nome: 'Opções', classe: 'col-2' }
    ];

    colunas.forEach(tituloColuna => {
        var th = document.createElement('th');
        var titulo = document.createTextNode(tituloColuna.nome);
        th.appendChild(titulo);
        th.className = tituloColuna.classe;
        tabelaHead.appendChild(th);
    });

    // Verifica se há dados
    if (dados && dados.length > 0) {
        dados.forEach(item => {
            var linha = tabelaBody.insertRow();

            var c1 = linha.insertCell(0);
            var c2 = linha.insertCell(1);
            var c3 = linha.insertCell(2);

            var botoes = '<a title="Ver" style="cursor:pointer" onClick="ver(' + item.id + ');"><span class="material-icons">visibility</span></a>';
            botoes += '<a title="Editar" style="cursor:pointer" onClick="editar(' + item.id + ');"><span class="material-icons">create</span></a>';
            botoes += '<a title="Remover" style="cursor:pointer" onClick="remover(' + item.id + ');"><span class="material-icons">highlight_off</span></a>';

            c1.innerHTML = item.id;
            c2.innerHTML = item.nome;
            c3.innerHTML = botoes;
        });
    } else {
        // Caso não haja dados, exibe uma mensagem na tabela
        var linha = tabelaBody.insertRow();
        var celula = linha.insertCell();
        celula.colSpan = colunas.length;
        celula.innerHTML = "Nenhum dado disponível";
    }
}

function adicionar(id) {
    window.open("../Sobremesasedt.html?opr=add", "_blank");
}

function ver(id) {
    window.open("../Sobremesasedt.html?opr=ver&id="+id, "_blank");
}

function editar(id) {
    window.open("../Sobremesasedt.html?opr=edt&id=" + id, "_blank");
}

function remover(id) {
    $.ajax({
        url: '/api/Sobremesas/'+id,
        method: 'DELETE',
        dataType: 'json',
        success: function (response) {
            console.log("Sobremesa apagada com sucesso.");
            CarregarDados();
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error(xhr.responseText);
        }
    });
}

window.OnPageLoad = function () { }
window.onload = function () {
    CarregarDados();
}