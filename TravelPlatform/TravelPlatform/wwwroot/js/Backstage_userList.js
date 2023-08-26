$(function () {
    GetUserList();
});

async function GetUserList() {
    await axios.get("/api/v1.0/User/GetUserList")
        .then((response) => {
            var datas = response.data.data;

            datas.forEach(function (data) {
                var id = data.id;
                var name = data.name;
                var email = data.email;
                var provider = data.provider == 'native' ? "ºô¯¸" : "Facebook";

                var item = `<tr><td>#</td>\
                            <td><a>${id}</a></td>\
                            <td>${name}</td>\
                            <td>${email}</td>\
                            <td>${provider}</td>`;

                $("#user-table tbody").append(item);
            });
        })
        .catch((error) => {
            console.log(error);
            alert("Sorry, we have some error...\nPlease try again");
        })
}