let saladsMinus = document.querySelectorAll('.salads-minus');
let saladsPlus = document.querySelectorAll('.salads-plus');

saladsMinus.forEach(min => {
  let prdPrice = min.parentElement.nextElementSibling.children[0].innerText;
  min.onclick = function () {
    count = min.nextElementSibling.innerText;
    if (count == 1) {
      min.nextElementSibling.innerText = count;
    } else {
      count--;
      min.nextElementSibling.innerText = count;
    }
    prdTotalPrice = (prdPrice * count).toFixed(2);
    min.parentElement.nextElementSibling.children[0].innerText = prdTotalPrice;
    
  }
})

saladsPlus.forEach(pl => {
  let prdPrice = pl.parentElement.nextElementSibling.children[0].innerText;
  pl.onclick = function () {
    count = pl.previousElementSibling.innerText;
    count++;
    pl.previousElementSibling.innerText = count
    prdTotalPrice = (prdPrice * count).toFixed(2);
    pl.parentElement.nextElementSibling.children[0].innerText = prdTotalPrice;
    
  }
})