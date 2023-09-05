$(function () {
    $("#no-list").hide();
    CheckLoginRequired().then(function(profile) {
        GetOrderList();
    });
});

function GetOrderList() {
    axios.get(`/api/v1.0/Order/GetUserOrderList?userId=${profile.id}`, config)
        .then((response) => {
            var orders = response.data;
            orders.forEach((order) => {
                let total = (order.price * order.qty).toLocaleString('zh-tw', {
                    style: 'currency',
                    currency: 'TWD',
                    minimumFractionDigits: 0
                }); 

                var options = {
                    timeZone: 'Asia/Taipei',
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit',
                    hour: '2-digit',
                    minute: '2-digit',
                    second: '2-digit',
                    hour12: true
                };
                var utcDate = new Date(order.orderDate + "Z");
                var orderDate = utcDate.toLocaleString('en-US', options);

                var checkStatus = "";
                if (order.checkStatus === 0) {
                    checkStatus = `<td>�ݼf��<br/>(�w�p��T�Ӥu�@�Ѥ�����)</td>`;
                }
                else {
                    checkStatus = `<td>�w����</td>`;
                }

                var item = `<tr>
                            <td>#</td>
                            <td>${order.title}</td>
                            <td>${order.price}</td>
                            <td>${order.qty}</td>
                            <td>${total}</td>
                            <td>${orderDate}</td>
                            <td>
                                <button class="btn btn-info btn-sm user-info-btn" onclick="openOrderInfoOverlay(${order.orderId})">�q��Ա�</button>
                            </td>
                            ${checkStatus}
                            </tr>`;

                $("#orders tbody").append(item);
            });
        })
        .catch((error) => {
            console.log(error);

            if (error.response.status === 404) {
                $("#no-list").show();
                return;
            }
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        })
}

function openOrderInfoOverlay(orderId) {
    axios.get(`/api/v1.0/Order/GetOrderDetail?orderId=${orderId}`)
        .then((response) => {
            var data = response.data;
            $("#order-qty").html(data.qty);

            $("#order-traveler-content").html("");
            var travel_i = 1;

            data.travelers.forEach((traveler) => {
                var birthday = new Date(traveler.birthday.toLocaleString());
                var day = birthday.getDate();
                var month = birthday.getMonth() + 1;
                var year = birthday.getFullYear();

                birthday = year + "/" + month + "/" + day;

                var item = `<!-- card -->
                        <div id="traveler-container">
                            <div class="card card-info" style="margin: 5px;">
                                <div class="card-header">
                                    <h3 class="card-title">�ȫ� - ${travel_i}</h3>
                                </div>
                                <div class="traveler-row">
                                    <label class="traveler-label-text" for="traveler-name-${travel_i}">�m�W</label>
                                    <input type="text" class="form-control col-3" id="traveler-name-${travel_i}" readonly="readonly" value="${traveler.name}" />
                                    <label class="traveler-label-text">�ʧO</label>
                                    <input type="text" class="form-control col-3" id="traveler-sex-${travel_i}" readonly="readonly" value="${traveler.sex}" />
                                </div>
                                <div class="traveler-row">
                                    <label class="traveler-label-text">�X�ͤ��</label>
                                    <input type="text" class="form-control col-3" id="traveler-birthday-${travel_i}" readonly="readonly" value="${birthday}" />
                                    <label class="traveler-label-text" for="traveler-phone-${travel_i}">���</label>
                                    <input type="text" class="form-control col-3" id="traveler-phone-${travel_i}" readonly="readonly" value="${traveler.phoneNumber}" />
                                </div>
                            </div>
                        </div>
                        <!-- /.card -->`;

                $("#order-traveler-content").append(item);
                travel_i++;
            });

            document.getElementById("overlay-order-info").style.display = "block";
        })
        .catch((error) => {
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        })
}

function closeOrderInfoOverlay(orderId) {
    document.getElementById("overlay-order-info").style.display = "none";
}