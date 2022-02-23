function myFunction() {
    var x = document.getElementById("pwd-1");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
    
    var y = document.getElementById("pwd-2");
    if (y.type === "password") {
        y.type = "text";
    } else {
        y.type = "password";
    }

    var xx = document.getElementById("pwd-3");
    if (xx.type === "password") {
        xx.type = "text";
    } else {
        xx.type = "password";
    }
    
    var yy = document.getElementById("pwd-4");
    if (yy.type === "password") {
        yy.type = "text";
    } else {
        yy.type = "password";
    }
}
