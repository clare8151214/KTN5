const cartItem = document.querySelectorAll(".cart-item");
const numValue = document.querySelectorAll(".numValue");
// cartItem.addEventListener("click", function (event) {
//   console.log("hi");
// });
cartItem.forEach(function (item) {
  item.addEventListener("click", function (event) {
    if (event.target.matches(".fa-circle-plus")) {
      const numberElement = event.target.nextElementSibling;
      numberElement.innerText = Number(numberElement.innerText) + 1;
    } else if (event.target.matches(".fa-circle-minus")) {
      const numberElement = event.target.previousElementSibling;
      if (!isValueZero(numberElement.innerText))
        numberElement.innerText = Number(numberElement.innerText) - 1;
    }
  });
});

function isValueZero(number) {
  if (number == 0) return true;
}
