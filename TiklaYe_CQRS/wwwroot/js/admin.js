document.addEventListener("DOMContentLoaded", function () {
    const productList = document.getElementById("products-list");
    const links = document.querySelectorAll(".list-group-item a");

    // NodeList'i bir diziye dönüştürüp döngüye alıyoruz
    Array.from(links).forEach(link => {
        link.addEventListener("click", function (event) {
            event.preventDefault();
            const url = this.getAttribute("href");

            fetch(url)
                .then(response => {
                    if (!response.ok) {
                        throw new Error("Network response was not ok");
                    }
                    return response.text();
                })
                .then(html => {
                    productList.innerHTML = html;
                })
                .catch(error => {
                    console.error("Error fetching product list:", error);
                    productList.innerHTML = "<p>Ürün liste yüklenirken bir hata oluştu.</p>";
                });
        });
    });
});

function toggleDropdown() {
    const dropdownMenu = document.getElementById('dropdownMenu');
    dropdownMenu.classList.toggle('hidden');
}

document.addEventListener('click', function (event) {
    if (!event.target.closest('.material-icons')) {
        const dropdowns = document.getElementsByClassName('dropdown-menu');
        Array.from(dropdowns).forEach(dropdown => {
            if (dropdown.classList.contains('show')) {
                dropdown.classList.remove('show');
            }
        });
    }
});