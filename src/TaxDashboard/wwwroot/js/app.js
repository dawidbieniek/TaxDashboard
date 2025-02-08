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
    const popovers = document.querySelectorAll('[data-bs-toggle="popover"]');
    popovers.forEach(t => {
        new bootstrap.Popover(t);
    });
}

function enablePopover(id) {
    const popover = document.getElementById(id);
    if (popover) {
        new bootstrap.Popover(popover);
    }
    else {
        console.log(`Couldn\'t find element with id: ${id}`)
    }
}

function hidePopover(id) {
    const popover = document.getElementById(id);
    if (popover) {
        const pi = bootstrap.Popover.getInstance(`#${id}`);
        if (pi)
            pi.hide();
    }
    else {
        console.log(`Couldn\'t find popover with id: ${id}`)
    }
    
}