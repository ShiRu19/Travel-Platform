$(function () {
    toastr.options = {
        closeButton: true,
    debug: false,
    newestOnTop: false,
    progressBar: true,
    positionClass: "toast-top-right",
    preventDuplicates: false,
    onclick: null,
    showDuration: "300",
    hideDuration: "1000",
    timeOut: "2000",
    extendedTimeOut: "2000",
    showEasing: "swing",
    hideEasing: "linear",
    showMethod: "fadeIn",
    hideMethod: "fadeOut"
    };

    GetOrderList();
});

var userInfo = {};

function GetOrderList() {
    axios.get("/api/v1.0/BackstageOrder/GetOrderList", config)
        .then((response) => {
            var unchecked = response.data.order_unchecked;
            var checked = response.data.order_checked;
            var canceled = response.data.order_canceled;

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

                var utcDate = new Date(order.orderDate + "Z");
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
                var orderDate = utcDate.toLocaleString('en-US', options);

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
                                        <button class="btn btn-info btn-sm user-info-btn" onclick="openUserInfoOverlay(${order.orderId})">訂購人資訊</button>
                                    </td>
                                    <td>${orderDate}</td>
                                    ${payment}
                                    <td>
                                        <button class="btn btn-info btn-sm user-info-btn" onclick="openOrderInfoOverlay(${order.orderId})">訂單詳情</button>
                                    </td>
                                    <td class="project-actions text-right">
                                        <div class="btn btn-success btn-sm check-btn" data-orderid="${order.orderId}" data-sessionid="${order.sessionId}" data-orderSeats="${order.qty}" onclick="check(this)"><i class="fas fa-check"></i>確認</div>
                                        <div class="btn btn-dark btn-sm cancel-btn" data-orderid="${order.orderId}" onclick="cancel(this)"><i class="fas fa-ban"></i>取消</div>
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
                var utcOrderDate = new Date(order.orderDate + "Z");
                var utcCheckDate = new Date(order.checkDate + "Z");
                var orderDate = utcOrderDate.toLocaleString('en-US', options);
                var checkDate = utcCheckDate.toLocaleString('en-US', options);

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
                                            <button class="btn btn-info btn-sm user-info-btn" onclick="openUserInfoOverlay(${order.orderId})">訂購人資訊</button>
                                        </td>
                                        <td>${orderDate}</td>
                                        ${payment}
                                        <td>
                                            <button class="btn btn-info btn-sm user-info-btn" onclick="openOrderInfoOverlay(${order.orderId})">訂單詳情</button>
                                        </td>
                                        <td>${checkDate}</td>
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
                var utcOrderDate = new Date(order.orderDate + "Z");
                var orderDate = utcOrderDate.toLocaleString('en-US', options);
                var utcCheckDate = new Date(order.checkDate + "Z");
                var checkDate = utcCheckDate.toLocaleString('en-US', options);

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
                                            <button class="btn btn-info btn-sm user-info-btn" onclick="openUserInfoOverlay(${order.orderId})">訂購人資訊</button>
                                        </td>
                                        <td>${orderDate}</td>
                                        ${payment}
                                        <td>
                                            <button class="btn btn-info btn-sm user-info-btn" onclick="openOrderInfoOverlay(${order.orderId})">訂單詳情</button>
                                        </td>
                                        <td>${checkDate}</td>
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
            toastr.success(
                '訂單確認成功',
                '成功',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        window.location.reload();
                    }
                }
            );
        })
        .catch((error) => {
            console.log(error);
            if (error.response.data.error === "Not enough seats.") {
                toastr.warning('該場次座位餘額不足', '警告');
            }
            else {
                toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
            }
        });
}

function cancel(cancelBtn) {
    axios.post("/api/v1.0/BackstageOrder/CancelOrder", {
        orderId: cancelBtn.dataset.orderid
    }, config)
        .then((response) => {
            toastr.success(
                '訂單取消成功',
                '成功',
                {
                    timeOut: 2000,
                    fadeOut: 2000,
                    onHidden: function () {
                        window.location.reload();
                    }
                }
            );
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}

function openUserInfoOverlay(orderId) {
    document.getElementById("overlay-user-info").style.display = "block";
    $("#user-name").html(userInfo[orderId].name);
    $("#user-email").html(userInfo[orderId].email);
    $("#user-phone").html(userInfo[orderId].phone);
}

function closeUserInfoOverlay(orderId) {
    document.getElementById("overlay-user-info").style.display = "none";
}

function openOrderInfoOverlay(orderId) {
    axios.get(`/api/v1.0/Order/GetOrderDetail?orderId=${orderId}`)
        .then((response) => {
            var data = response.data;
            $("#order-id").html(data.orderId);
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
                                    <h3 class="card-title">旅客 - ${travel_i}</h3>
                                </div>
                                <div class="traveler-row">
                                    <label class="traveler-label-text" for="traveler-name-${travel_i}">姓名</label>
                                    <input type="text" class="form-control col-3" id="traveler-name-${travel_i}" readonly="readonly" value="${traveler.name}" />
                                    <label class="traveler-label-text">性別</label>
                                    <input type="text" class="form-control col-3" id="traveler-sex-${travel_i}" readonly="readonly" value="${traveler.sex}" />
                                </div>
                                <div class="traveler-row">
                                    <label class="traveler-label-text">出生日期</label>
                                    <input type="text" class="form-control col-3" id="traveler-birthday-${travel_i}" readonly="readonly" value="${birthday}" />
                                    <label class="traveler-label-text" for="traveler-phone-${travel_i}">手機</label>
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
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        })
}

function closeOrderInfoOverlay(orderId) {
    document.getElementById("overlay-order-info").style.display = "none";
}