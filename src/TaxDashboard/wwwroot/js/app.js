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

function addModalHiddenListener(dotnetRef, callbackName, modalId) {
    const modal = document.getElementById(modalId);
    modal.addEventListener('hidden.bs.modal', e => {
        dotnetRef.invokeMethodAsync(callbackName);
    });
}

function enableTooltips() {
    const tooltips = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    tooltips.forEach(t => {
        new bootstrap.Tooltip(t);
    });
}

function enablePopovers() {
    const tooltips = document.querySelectorAll('[data-bs-toggle="popover"]');
    tooltips.forEach(t => {
        new bootstrap.Popover(t);
    });
}