export function setActiveNavLink(id) {
    document.querySelectorAll('.nav-link.active').forEach(link => link.classList.remove('active'));

    const clickedElement = document.getElementById(id);
    if (!clickedElement)
        return;

    clickedElement.classList.add('active');

    let secondParent = clickedElement.parentElement.parentElement;

    // If we found the second parent, find the corresponding nav-link
    if (secondParent && secondParent.id) {
        const targetLink = document.querySelector(`[data-bs-target="#${secondParent.id}"]`)
        targetLink.classList.add('active');
    }
}