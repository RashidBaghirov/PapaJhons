let minusButtons = document.querySelectorAll('.pizza-minus');
let plusButtons = document.querySelectorAll('.pizza-plus');
let quantityInputs = document.querySelectorAll('.quantityInput');
let quantitySpans = document.querySelectorAll('.modal-count-value');

minusButtons.forEach((min, index) => {
    let prdPrice = min.parentElement.nextElementSibling.querySelector('.pizza-modal-price').innerText;
    min.onclick = function () {
        let count = parseInt(quantitySpans[index].innerText);
        if (count === 1) {
            quantitySpans[index].innerText = count;
        } else {
            count--;
            quantitySpans[index].innerText = count;
        }
        let prdTotalPrice = (prdPrice * count).toFixed(2);
        min.parentElement.nextElementSibling.querySelector('.pizza-modal-price').innerText = prdTotalPrice;

        quantityInputs[index].value = count;
    }
});

plusButtons.forEach((pl, index) => {
    let prdPrice = pl.parentElement.nextElementSibling.querySelector('.pizza-modal-price').innerText;
    pl.onclick = function () {
        let count = parseInt(quantitySpans[index].innerText);
        count++;
        quantitySpans[index].innerText = count;
        let prdTotalPrice = (prdPrice * count).toFixed(2);
        pl.parentElement.nextElementSibling.querySelector('.pizza-modal-price').innerText = prdTotalPrice;

        quantityInputs[index].value = count;
    }
});



function addToBasket(id) {
    var quantity = parseInt($('.modal-count-value').text());

    fetch('/AddBasket?id=' + id + '&quantity=' + quantity, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => {
            if (response.ok) {
                location.reload();
            } else {
                // Handle the error case
                console.error('Add to Basket request failed');
            }
        })
        .catch(error => {
            console.error('Error:', error);
        });
}
   });
}
