document.addEventListener("DOMContentLoaded", function () {
    const productList = document.getElementById("products-list");
    const links = document.querySelectorAll(".list-group-item a");

    links.forEach(link => {
        link.addEventListener("click", function (event) {
            event.preventDefault();
            const url = this.getAttribute("href");

            fetch(url)
                .then(response => response.text())
                .then(html => {
                    productList.innerHTML = html;
                })
                .catch(error => console.error("Error fetching product list:", error));
        });
    });
});