/*===== SHOW NAVBAR  =====*/ 
const toggle = document.querySelector("#header-toggle"),
      sideBar = document.querySelector(".dashboard__management-sidebar")

toggle.addEventListener("click", function(){
    this.classList.toggle("rotate")
    sideBar.classList.toggle("shorten")
})

//const link = document.querySelectorAll(".dashboard__management-sidebar-items a")

//link.forEach(i => {
//    i.addEventListener("click", (e) => {
//        e.target.classList.add("blue")        
//    })
//})
