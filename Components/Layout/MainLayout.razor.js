export function AddEventListeners() {
    document.querySelectorAll('.nav-link:not([data-bs-target])').forEach(link => {
        link.addEventListener('click', (e) => {
            document.querySelectorAll('.nav-link.active').forEach(link => link.classList.remove('active'));

            const clickedElement = e.target;
            clickedElement.classList.add('active');

            let secondParent = clickedElement.parentElement.parentElement;

            // If we found the second parent, find the corresponding nav-link
            if (secondParent && secondParent.id) {
                const targetLink = document.querySelector(`[data-bs-target="#${secondParent.id}"]`)
                targetLink.classList.add('active');
            }
        });
    })
}