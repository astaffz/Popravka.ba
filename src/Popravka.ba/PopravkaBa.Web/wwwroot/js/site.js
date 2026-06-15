// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ===== Prevent duplicate form submissions =====
// Slow POSTs (image upload on "Objavi oglas", account creation + email on "Registracija")
// leave a visible delay after the click, tempting impatient users to press submit again
// and fire a second POST. Block the repeat submit and disable the submit controls once a
// form is on its way.
(function () {
    'use strict';

    document.addEventListener('submit', function (e) {
        var form = e.target;
        if (!(form instanceof HTMLFormElement)) return;

        // Another handler (client-side validation, a confirm() dialog) already cancelled
        // this submit — leave the form untouched so the user can try again.
        if (e.defaultPrevented) return;

        if (form.dataset.submitting === 'true') {
            e.preventDefault();
            return;
        }
        form.dataset.submitting = 'true';

        // Defer disabling so the clicked button's name/value still makes it into the
        // request payload, then block any further clicks.
        window.setTimeout(function () {
            form.querySelectorAll('button[type="submit"], input[type="submit"], input[type="image"], button:not([type])')
                .forEach(function (btn) { btn.disabled = true; });
        }, 0);
    });

    // Reset the guard when the page is restored from the back/forward cache so a returning
    // user isn't left with a permanently disabled button.
    window.addEventListener('pageshow', function (e) {
        if (!e.persisted) return;
        document.querySelectorAll('form[data-submitting="true"]').forEach(function (form) {
            form.dataset.submitting = '';
            form.querySelectorAll('button[type="submit"], input[type="submit"], input[type="image"], button:not([type])')
                .forEach(function (btn) { btn.disabled = false; });
        });
    });
})();
