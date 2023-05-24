/*==================== SCROLL SECTIONS ACTIVE LINK ====================*/
const sections = document.querySelectorAll('section[id]')

function scrollActive() {
    const scrollY = window.pageYOffset

    sections.forEach(current => {
        const sectionHeight = current.offsetHeight
        const sectionTop = current.offsetTop - 300;
        sectionId = current.getAttribute('id')

        if (scrollY > sectionTop && scrollY <= sectionTop + sectionHeight) {
            document.querySelector('.step-content.' + sectionId + ' .step-circle').classList.add('stepColor')
        } else {
            document.querySelector('.step-content.' + sectionId + ' .step-circle').classList.remove('stepColor')
        }
    })
}
window.addEventListener('scroll', scrollActive)

const planChoice = document.querySelector("#planChoice"),
    match = document.querySelector("#match"),
    confirm = document.querySelector("#confirm")
planChoice.addEventListener("click", (e) => {
    if (e.target.classList.contains("card")) {
        var section = document.getElementById('match');
        section.scrollIntoView();
    }
})
match.addEventListener("click", (e) => {
    var section = document.getElementById('confirm');
    // section.scrollIntoView();
})

confirm.addEventListener("click", (e) => {
    var section = document.getElementById('checkout');
    section.scrollIntoView();
})


const produtItemContainer = document.querySelector(".product__container")
produtItemContainer.addEventListener("click", (e) => {
    if (e.target.tagName == "IMG") {
        e.target.parentElement.parentElement.classList.toggle("selectedItem")
    }
})

