$(function () {
    GetUserList();
});

async function GetUserList() {
    let config = {
        headers: {
            Authorization: `Bearer ${localStorage.getItem("access_token")}`,
        }
    }

    await axios.get("/api/v1.0/User/GetUserList", config)
        .then((response) => {
            var datas = response.data.data;

            datas.forEach(function (data) {
                var id = data.id;
                var name = data.name;
                var email = data.email;
                var provider = data.provider == 'native' ? "����" : "Facebook";

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
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        })
}