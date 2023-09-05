$(function () {
    GetOrderList();
});

var userInfo = {};

function GetOrderList() {
    axios.get("/api/v1.0/BackstageOrder/GetOrderList", config)
        .then((response) => {
            var unchecked = response.data.order_unchecked;
            var checked = response.data.order_checked;
            var canceled = response.data.order_canceled;

            console.log(unchecked);

            unchecked.forEach((order) => {
                let total = (order.total).toLocaleString('zh-tw', {
                    style: 'currency',
                    currency: 'TWD',
                    minimumFractionDigits: 0
                }); 

                userInfo[`${order.orderId}`] = new Object();
                userInfo[`${order.orderId}`].name = order.userName;
                userInfo[`${order.orderId}`].email = order.userEmail;
                userInfo[`${order.orderId}`].phone = order.userPhone;

                var payment = "";

                if (order.payStatus === 0) {
                    payment = `<th></th>`
                }
                else {
                    payment = `<th>${order.accountDigits}</th>`;
                }

                var item_unchecked = `<tr>
                                    <td>#</td>
                                    <td>${order.productNumber}</td>
                                    <td>${order.qty}</td>
                                    <td>${total}</td>
                                    <td>
                                        <button class="btn  btn-info btn-sm user-info-btn" onclick="openUserInfoOverlay(${order.orderId})">�q�ʤH��T</button>
                                    </td>
                                    <td>${order.orderDate}</td>
                                    ${payment}
                                    <td>
                                        <a class="btn btn-info btn-sm" href="#">�q��Ա�</a>
                                    </td>
                                    <td class="project-actions text-right">
                                        <div class="btn btn-success btn-sm check-btn" data-orderid="${order.orderId}" data-sessionid="${order.sessionId}" data-orderSeats="${order.qty}" onclick="check(this)"><i class="fas fa-check"></i>�T�{</div>
                                        <div class="btn btn-dark btn-sm cancel-btn" data-orderid="${order.orderId}" onclick="cancel(this)"><i class="fas fa-ban"></i>����</div>
                                    </td>
                                </tr>`;
                $("#unchecked-table tbody").append(item_unchecked);
            })

            checked.forEach((order) => {
                let total = (order.total).toLocaleString('zh-tw', {
                    style: 'currency',
                    currency: 'TWD',
                    minimumFractionDigits: 0
                }); 

                userInfo[`${order.orderId}`] = new Object();
                userInfo[`${order.orderId}`].name = order.userName;
                userInfo[`${order.orderId}`].email = order.userEmail;
                userInfo[`${order.orderId}`].phone = order.userPhone;

                var payment = "";

                if (order.payStatus === 0) {
                    payment = `<th></th>`
                }
                else {
                    payment = `<th>${order.accountDigits}</th>`;
                }

                var item_checked = `<tr>
                                        <td>#</td>
                                        <td>${order.productNumber}</td>
                                        <td>${order.qty}</td>
                                        <td>${total}</td>
                                        <td>
                                            <button class="btn  btn-info btn-sm user-info-btn" onclick="openUserInfoOverlay(${order.orderId})">�q�ʤH��T</button>
                                        </td>
                                        <td>${order.orderDate}</td>
                                        ${payment}
                                        <td>
                                            <a class="btn btn-info btn-sm" href="#">�q��Ա�</a>
                                        </td>
                                        <td>${order.checkDate}</td>
                                    </tr>`;

                

                $("#checked-table tbody").append(item_checked);
            })

            canceled.forEach((order) => {
                let total = (order.total).toLocaleString('zh-tw', {
                    style: 'currency',
                    currency: 'TWD',
                    minimumFractionDigits: 0
                }); 

                userInfo[`${order.orderId}`] = new Object();
                userInfo[`${order.orderId}`].name = order.userName;
                userInfo[`${order.orderId}`].email = order.userEmail;
                userInfo[`${order.orderId}`].phone = order.userPhone;

                var payment = "";

                if (order.payStatus === 0) {
                    payment = `<th></th>`
                }
                else {
                    payment = `<th>${order.accountDigits}</th>`;
                }

                var item_canceled = `<tr>
                                        <td>#</td>
                                        <td>${order.productNumber}</td>
                                        <td>${order.qty}</td>
                                        <td>${total}</td>
                                        <td>
                                            <button class="btn  btn-info btn-sm user-info-btn" onclick="openUserInfoOverlay(${order.orderId})">�q�ʤH��T</button>
                                        </td>
                                        <td>${order.orderDate}</td>
                                        ${payment}
                                        <td>
                                            <a class="btn btn-info btn-sm" href="#">�q��Ա�</a>
                                        </td>
                                        <td>${order.checkDate}</td>
                                    </tr>`;
                $("#canceled-table tbody").append(item_canceled);
            })

        })
        .catch((error) => {
            console.log(error);
        })
}

function check(checkBtn) {
    axios.post("/api/v1.0/BackstageOrder/CheckOrder", {
        OrderId: checkBtn.dataset.orderid,
        SessionId: checkBtn.dataset.sessionid,
        OrderSeats: checkBtn.dataset.orderseats
    }, config)
        .then((response) => {
            alert("�J�榨�\");
            location.reload();
        })
        .catch((error) => {
            console.log(error);
            if (error.response.data.error === "Not enough seats.") {
                alert("�ӳ����y��l�B����");
            }
            else {
                alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
            }
        });
}

function cancel(cancelBtn) {
    axios.post("/api/v1.0/BackstageOrder/CancelOrder", {
        orderId: cancelBtn.dataset.orderid
    }, config)
        .then((response) => {
            alert("�J�榨�\");
            location.reload();
        })
        .catch((error) => {
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        });
}

function openUserInfoOverlay(orderId) {
    document.getElementById("myOverlay").style.display = "block";
    $("#user-name").html(userInfo[orderId].name);
    $("#user-email").html(userInfo[orderId].email);
    $("#user-phone").html(userInfo[orderId].phone);
}

function closeUserInfoOverlay(orderId) {
    document.getElementById("myOverlay").style.display = "none";
}