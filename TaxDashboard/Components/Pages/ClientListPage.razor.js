export function addEventListeners(dotnetRef) {
    const modal = document.getElementById('deletingClientModal');
    modal.addEventListener('hidden.bs.modal', e => {
        dotnetRef.invokeMethodAsync('OnModalHidden');
    });
}