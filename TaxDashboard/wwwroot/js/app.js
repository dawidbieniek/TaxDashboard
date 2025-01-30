function showModal(id) {
    const modal = new bootstrap.Modal(document.getElementById(id));
    modal.show();
}
function hideModal(id) {
    const dismissBtn = document.querySelector('[data-bs-dismiss=modal]');
    dismissBtn?.click();
}

function showToast(id) {
    const toast = new bootstrap.Toast(document.getElementById(id));
    toast.show();
}
function hideToast(id) {
    const dismissBtn = document.querySelector('[data-bs-dismiss=toast]');
    dismissBtn?.click();
}