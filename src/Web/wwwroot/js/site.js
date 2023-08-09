// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))

const elements = document.querySelectorAll('.btn-clipboard')
elements.forEach(item => item.addEventListener('click', copyToClipboard))


async function copyToClipboard() {
    try {
        const link = document.getElementById('computed-link').href
        await navigator.clipboard.writeText(link);
    } catch (error) {
        // failed
    }
}