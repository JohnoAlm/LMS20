// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/*$(document).ready(function () {*/

//     Get the container element ul
//var navContainer = document.getElementById("myNavbar");
// Get all links with class="nav-item" inside the container eg li
//var navItems = navContainer.getElementsByClassName("nav-item");

// Loop through the lis and add the active class to the current/clicked button
//for (var i = 0; i < navItems.length; i++) {
//    navItems[i].addEventListener("click", function () {
//        console.log("så här ");
        //var current = document.getElementsByClassName("navItems");
        //current[0].className = current[0].className.replace("active", "");
        //this.className += "active" ;
//    })
//}
//$(document).ready(function () {


//});
/*});*/
//$(document).ready(function () {
//    const myPage = window.location;
//    console.log(myPage);
//});

let exampleModal = document.querySelector("#createCourseModal");
let validationSummary = document.querySelector('#validationSummary');

document.querySelectorAll('.deleteBtn').forEach(b => b.addEventListener('click', function () {

    console.log("CoursId: " + this.id);

    // ToDo: Implementera 

}));

$(document).ready(function () {
    console.log("Killroy igen 1");
    $("#formClose").click(function () {
        $("#createCourseForm").trigger("reset");
        console.log("Killroy igen 2");
    });
});

//$('#myModal').on('shown.bs.modal', function () {
//    $('#myInput').trigger('focus')
//})

    // Workaround for modal hide bug
    $('#createCourseModal').modal('hide');
    $('#createCourseModal').on('hidden.bs.modal', function () {
        $('.btn-close').trigger('click');
    });

}

function failCreate(response) {

    //console.log(response, 'failed to create');
    validationSummary.innerHTML = "Någonting gick fel";

}
