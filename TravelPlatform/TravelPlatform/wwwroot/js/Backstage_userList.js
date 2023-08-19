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
                var sex = data.sex === 'men' ? '¨k' : '¤k';
                var birthday = data.birthday.split('T')[0];
                var email = data.email;
                var phoneNumber = data.phoneNumber;
                var region = data.region;

                var item = `<tr><td>#</td>\
                            <td><a>${id}</a></td>\
                            <td>${name}</td>\
                            <td>${sex}</td>\
                            <td>${birthday}</td>\
                            <td>${email}</td>\
                            <td>${phoneNumber}</td>\
                            <td>${region}</td>`;

                $("#user-table tbody").append(item);
            });
        })
        .catch((error) => {
            console.log(error);
            alert("Sorry, we have some error...\nPlease try again");
        })
}