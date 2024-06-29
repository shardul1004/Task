var CreditDiv;
var WithdrawDiv;
document.addEventListener("DOMContentLoaded", () => {
    CreditDiv = document.querySelector("#CreditDiv");
    WithdrawDiv = document.querySelector("#WithdrawDiv");
    CreditDiv.style.display = "none";
    WithdrawDiv.style.display = "none";
});

function ToggleCredit() {
    if (CreditDiv.style.display === "none") {
        CreditDiv.style.display = "block";
        WithdrawDiv.style.display = "none";
    } else {
        CreditDiv.style.display = "none";
    }

}

function ToggleWithdraw() {
    if (WithdrawDiv.style.display === "none") {
        WithdrawDiv.style.display = "block";
        CreditDiv.style.display = "none";
    } else {
        WithdrawDiv.style.display = "none";
    }
}
