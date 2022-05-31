const button_more_progress = document.querySelectorAll(".boutton-description-toogle");

button_more_progress.forEach(moreButton => {
    moreButton.addEventListener("click", event => {
        moreButton.closest(".member").lastElementChild.classList.toggle("member-description-notactive");
        moreButton.closest(".member").lastElementChild.classList.toggle("member-description-active");
    });
});