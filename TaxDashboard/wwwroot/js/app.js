function showModal(id) {
    const modal = new bootstrap.Modal(document.getElementById(id));
    modal.show();
}
function hideModal(id) {
    const dismissBtn = document.querySelector('[data-bs-dismiss=modal]');
    dismissBtn.click();
    //const modal = new bootstrap.Modal(document.getElementById(id));
    //console.log(modal);
    //modal.hide();
}