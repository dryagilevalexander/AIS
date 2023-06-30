function openForm() {
    document.getElementById("myForm").style.display = "block";
    document.getElementById("openChat").setAttribute("style", "display: none;");
}

function closeForm() {
    document.getElementById("myForm").style.display = "none";
    document.getElementById("openChat").setAttribute("style", "display: block;");
}