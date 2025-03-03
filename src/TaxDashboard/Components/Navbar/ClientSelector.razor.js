export function blurInput() {
    const inputElement = document.querySelector(`input[list="clientList"]`);
    if (inputElement) {
        inputElement.blur();
    }
}
