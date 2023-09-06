$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    var paging = urlParams.get('paging');

    if (paging === null) {
        window.location.href = "/Order_HistoryList.html?paging=1";
        return;
    }

    $("#no-list").hide();
    CheckLoginRequired().then(function (profile) {
        ShowPagination(paging);
        GetOrderList(paging);
    });
});

function ShowPagination(paging) {
    axios.get(`/api/v1.0/Order/GetUserOrderPageCount?userId=${profile.id}`, config)
        .then((response) => {
            var count = response.data;

            if (count !== 0) {
                if (paging < 1) {
                    window.location.href = "/Order_HistoryList.html?paging=1";
                    return;
                }

                if (paging > count) {
                    window.location.href = `/Order_HistoryList.html?paging=${count}`;
                    return;
                }

                $("#pagination-content").html("");
                var paging_li = "";
                for (let i = 1; i < count + 1; i++) {
                    if (i < paging || i > paging) {
                        paging_li += `<li class="page-item"><a class="page-link" href="/Order_HistoryList.html?paging=${i}">${i}</a></li>`;
                    }
                    else {
                        paging_li += `<li class="page-item active"><a class="page-link" href="/Order_HistoryList.html?paging=${i}">${i}</a></li>`;
                    }
                }

                var previous = "";
                if (paging > 1) {
                    previous = `<li class="page-item">
                                    <a class="page-link" href="/Order_HistoryList.html?paging=${paging - 1}" tabindex="-1">Previous</a>
                                </li>`;
                }
                else {
                    previous = `<li class="page-item disabled">
                                    <a class="page-link" href="#" tabindex="-1">Previous</a>
                                </li>`;
                }

                var next = "";
                if (paging < count) {
                    next = `<li class="page-item">
                                <a class="page-link" href="/Order_HistoryList.html?paging=${paging + 1}">Next</a>
                            </li>`;
                }
                else {
                    next = `<li class="page-item disabled">
                                    <a class="page-link" href="#" tabindex="-1">Next</a>
                                </li>`;
                }

                var item = `<ul class="pagination justify-content-center">
                                        ${previous}
                                        ${paging_li}
                                        ${next}
                                    </ul>`;

                $("#pagination-content").append(item);
            }
            else {
                $(".pagination").show();
            }
        })
        .catch((error) => {
            console.log(error);
            alert("抱歉...發生了一些錯誤，請再試一次！");
        });
}

function GetOrderList(paging) {
    axios.get(`/api/v1.0/Order/GetUserOrderList?userId=${profile.id}&paging=${paging}`, config)
        .then((response) => {
            var orders = response.data;
            orders.forEach((order) => {
                let total = (order.price * order.qty).toLocaleString('zh-tw', {
                    style: 'currency',
                    currency: 'TWD',
                    minimumFractionDigits: 0
                }); 

                let price = (order.price).toLocaleString('zh-tw', {
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
                    if (order.payStatus === 0) {
                        checkStatus = `<td><a class="btn btn-info btn-sm" href="/Order_Pay.html?qty=${order.qty}&productNumber=${order.productNumber}&orderId=${order.orderId}">前往付款</a></td>`
                    }
                    else if (order.payStatus === 1) {
                        checkStatus = `<td>待審核<br/>(預計於三個工作天內完成)</td>`;
                    }
                }
                else if(order.checkStatus === 1) {
                    checkStatus = `<td>已成立</td>`;
                }
                else if (order.checkStatus === 2) {
                    checkStatus = `<td>已取消</td>`;
                }

                var item = `<tr>
                            <td>#</td>
                            <td>${order.title}</td>
                            <td>${price}</td>
                            <td>${order.qty}</td>
                            <td>${total}</td>
                            <td>${orderDate}</td>
                            <td>
                                <button class="btn btn-info btn-sm user-info-btn" onclick="openOrderInfoOverlay(${order.orderId})">訂單詳情</button>
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
            alert("抱歉...發生了一些錯誤，請再試一次！");
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
            alert("抱歉...發生了一些錯誤，請再試一次！");
        })
}

function closeOrderInfoOverlay(orderId) {
    document.getElementById("overlay-order-info").style.display = "none";
}