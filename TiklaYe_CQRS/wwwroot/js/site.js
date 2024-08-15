document.addEventListener('DOMContentLoaded', function () {
    var form = document.querySelector('form');
    var inputs = document.querySelectorAll('.form-control');

    // Başlangıçta tüm hata mesajlarını gizler.
    inputs.forEach(function (input) {
        var errorMessage = document.querySelector(`span[data-valmsg-for="${input.name}"]`);
        if (errorMessage) {
            errorMessage.style.display = 'none';
        }
    });

    // Form gönderildiğinde hata mesajlarını kontrol eder ve gösterir.
    form.addEventListener('submit', function (event) {
        var hasErrors = false;

        inputs.forEach(function (input) {
            var errorMessage = document.querySelector(`span[data-valmsg-for="${input.name}"]`);

            if (input.value.trim() === '' && input.hasAttribute('data-val-required')) {
                if (errorMessage) {
                    errorMessage.style.display = 'block'; // Hata mesajını göster
                }
                input.classList.add('input-validation-error');
                hasErrors = true;
            } else {
                if (errorMessage) {
                    errorMessage.style.display = 'none'; // Hata mesajını gizle
                }
                input.classList.remove('input-validation-error');
            }
        });

        // Eğer hata varsa formun gönderilmesini durdurur.
        if (hasErrors) {
            event.preventDefault();
        }
    });

    // Inputlarda değişiklik yapıldığında hata mesajlarını kontrol eder.
    inputs.forEach(function (input) {
        input.addEventListener('input', function () {
            var errorMessage = document.querySelector(`span[data-valmsg-for="${input.name}"]`);

            if (input.value.trim() === '' && input.hasAttribute('data-val-required')) {
                if (errorMessage) {
                    errorMessage.style.display = 'block'; // Hata mesajını göster
                }
                input.classList.add('input-validation-error');
            } else {
                if (errorMessage) {
                    errorMessage.style.display = 'none'; // Hata mesajını gizle
                }
                input.classList.remove('input-validation-error');
            }
        });
    });
});