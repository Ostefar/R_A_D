$(document).ready(function () {
    $.ajax({
        url: "http://localhost:8050/User/ViewUsers",
        method: "GET",
        success: function (users) {
            $.each(users, function (index, user) {
                var address = user.addressLine1 + ", " + user.city;
                $("#users-table").append("<tr><td>" + user.id + "</td><td>" + user.firstName + "</td><td>" + user.lastName + "</td><td>" + user.email + "</td><td>" + user.phone + "</td><td>" + address + "</td><td>" + user.country + "</td></tr>");
            });
        },
        error: function (xhr, textStatus, errorThrown) {
            alert("Error: " + xhr.responseText);
        }
    });
});
