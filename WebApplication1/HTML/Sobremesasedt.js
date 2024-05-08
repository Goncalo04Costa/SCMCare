/*PDS
Daniela Brito nº25591
Daniela Pereira nº25988
Sofia Carvalho nº25991
Diogo Fernandes nº26008
Gonçalo Costa nº26024
LESI-IPCA */

const params = new URLSearchParams(window.location.search);
const tipoOperacao = params.get('opr');
const id = params.get('id');

window.OnPageLoad = function () { }
window.onload = function () {
    if (tipoOperacao == 'ver') {
        //CarregarDados
    }
    else if (tipoOperacao == 'edt') {
        alert("editar");
    }
    else if (tipoOperacao == 'add') {
        alert("editar");
    }
}