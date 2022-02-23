function myFunction() {
    var z = document.getElementById("pwd-0");
    if (z.type === "password") {
        z.type = "text";
    } else {
        z.type = "password";
    }
}