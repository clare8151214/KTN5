const mpay = document.querySelector("#mpay");
const mpayInput = document.querySelector("#mpayInput");
const country = document.querySelector("#country");
const numberInput = document.querySelector("#delivery-fee");
const spNum = document.querySelector("#spNum");
const spInput = document.querySelector("#spInput");
const calc = document.querySelector(".calc");

mpayInput.value = mpay.innerText; //傳入定期定額的金額到第一個input值
calcTotal();

//加碼贊助: 當加碼贊助input值改變，一併更新第二個input值
function sendSpnum() {
  spInput.value = spNum.value;
  calcTotal();
}

//運費：當下選單的值改變時，同時也更新第三個input值
function sendDfee() {
  const selectedCountry = country.value;
  if (selectedCountry == "台灣本島") {
    numberInput.value = 0;
  } else if (selectedCountry == "台灣外島(需額外運費)") {
    numberInput.value = 60;
  } else if (selectedCountry == "香港") {
    numberInput.value = 150;
  } else if (selectedCountry == "澳門") {
    numberInput.value = 200;
  } else {
    return;
  }
  calcTotal();
}

//計算總額
function calcTotal() {
  const result = document.querySelector("#result");
  result.value =
    parseInt(mpayInput.value) +
    parseInt(numberInput.value) +
    parseInt(spInput.value);
}
