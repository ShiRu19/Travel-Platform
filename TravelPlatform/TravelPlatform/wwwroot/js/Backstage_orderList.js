var unchecked = 1;
var checked = 1;
var canceled = 1;
var productNumber = "all";

$(function () {
    const urlParams = new URLSearchParams(window.location.search);
    unchecked = Number(urlParams.get('unchecked')) === 0 ? 1 : Number(urlParams.get('unchecked'));
    checked = Number(urlParams.get('checked')) === 0 ? 1 : Number(urlParams.get('checked'));
    canceled = Number(urlParams.get('canceled')) === 0 ? 1 : Number(urlParams.get('canceled'));
    productNumber = urlParams.get('productNumber') == null ? "all" : urlParams.get('productNumber');

    if (productNumber != "all") {
        $("#product-number-search").val(productNumber);
    }

    ShowPagination();

    $("#product-number-search").on("keydown", function (event) {
        if (event.keyCode === 13) {
            var productNumber = $("#product-number-search").val();
            if (productNumber == '') {
                productNumber = "all";
            }
            window.location.replace(`/admin/Backstage_OrderList.html?unchecked=1&checked=1&canceled=1&productNumber=${productNumber}`);
        }
    });
});

var userInfo = {};

function ShowPagination() {
    axios.get(`/api/v1.0/BackstageOrder/GetOrderPageCount?productNumber=${productNumber}`, config)
        .then((response) => {
            var count = response.data;
            var pagings_unchecked = Number(count.pagings_unchecked) === 0 ? 1 : Number(count.pagings_unchecked);
            var pagings_checked = Number(count.pagings_checked) === 0 ? 1 : Number(count.pagings_checked);
            var pagings_canceled = Number(count.pagings_canceled) === 0 ? 1 : Number(count.pagings_canceled);

            if (unchecked < 1) {
                unchecked = 1;
            }
            else if (unchecked > pagings_unchecked) {
                unchecked = pagings_unchecked;
            }

            if (checked < 1) {
                checked = 1;
            }
            else if (checked > pagings_checked) {
                checked = pagings_checked;
            }

            if (canceled < 1) {
                canceled = 1;
            }
            else if (canceled > pagings_canceled) {
                canceled = pagings_canceled;
            }

            GeneratePagination($("#pagination-content-unchecked"), unchecked, pagings_unchecked, 0);
            GeneratePagination($("#pagination-content-checked"), checked, pagings_checked, 1);
            GeneratePagination($("#pagination-content-canceled"), canceled, pagings_canceled, 2);
            GetOrderList();
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        });
}

function GeneratePagination(pagination, paging, count, type) { // type: 0=unchecked, 1=checked, 2=canceled
    if (count !== 0) {
        pagination.html("");

        var pagingList = [unchecked, checked, canceled];
        pagingList[type] = 0;

        var paging_li = "";
        for (let i = 1; i < count + 1; i++) {
            pagingList[type] = i;
            if (i < paging || i > paging) {
                paging_li += `<li class="page-item"><a class="page-link" href="/admin/Backstage_OrderList.html?unchecked=${pagingList[0]}&checked=${pagingList[1]}&canceled=${pagingList[2]}&productNumber=${productNumber}">${i}</a></li>`;
            }
            else {
                paging_li += `<li class="page-item active"><a class="page-link" href="/admin/Backstage_OrderList.html?unchecked=${pagingList[0]}&checked=${pagingList[1]}&canceled=${pagingList[2]}&productNumber=${productNumber}">${i}</a></li>`;
            }
        }

        pagingList = [unchecked, checked, canceled];
        var previous = "";
        if (paging > 1) {
            pagingList[type]--;
            previous = `<li class="page-item">
                            <a class="page-link" href="/admin/Backstage_OrderList.html?unchecked=${pagingList[0]}&checked=${pagingList[1]}&canceled=${pagingList[2]}&productNumber=${productNumber}" tabindex="-1">Previous</a>
                        </li>`;
        }
        else {
            previous = `<li class="page-item disabled">
                            <a class="page-link" href="#" tabindex="-1">Previous</a>
                        </li>`;
        }

        pagingList = [unchecked, checked, canceled];
        var next = "";
        if (paging < count) {
            pagingList[type]++;
            next = `<li class="page-item">
                        <a class="page-link" href="/admin/Backstage_OrderList.html?unchecked=${pagingList[0]}&checked=${pagingList[1]}&canceled=${pagingList[2]}&productNumber=${productNumber}">Next</a>
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

        pagination.append(item);
    }
    else {
        var item = `<ul class="pagination justify-content-center">
                        <li class="page-item disabled">
                            <a class="page-link" href="#" tabindex="-1">Previous</a>
                        </li>
                        <li class="page-item active"><a class="page-link" href="#">1</a></li>
                        <li class="page-item disabled">
                            <a class="page-link" href="#">Next</a>
                        </li>
                    </ul>`;
        pagination.append(item);
    }
}

function GetOrderList() {
    axios.get(`/api/v1.0/BackstageOrder/GetOrderList?page_unchecked=${unchecked}&page_checked=${checked}&page_canceled=${canceled}&productNumber=${productNumber}`, config)
        .then((response) => {
            var unchecked = response.data.order_unchecked;
            var checked = response.data.order_checked;
            var canceled = response.data.order_canceled;

            GenerateOrderList($("#unchecked-table tbody"), $("#loading-unchecked"), unchecked, 0);
            GenerateOrderList($("#checked-table tbody"), $("#loading-checked"), checked, 1);
            GenerateOrderList($("#canceled-table tbody"), $("#loading-canceled"), canceled, 2);
        })
        .catch((error) => {
            console.log(error);
            toastr.error('抱歉...發生了一些錯誤，請再試一次！', '錯誤');
        })
}

function GenerateOrderList(listParent, loading, orders, orderType) {
    orders.forEach((order) => {
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

        var item_last = "";

        if (orderType == 0) {
            item_last = `<td class="project-actions text-right">
                <div class="btn btn-success btn-sm check-btn" data-orderid="${order.orderId}" data-sessionid="${order.sessionId}" data-orderSeats="${order.qty}" onclick="check(this)"><i class="fas fa-check"></i>確認</div>
                <div class="btn btn-dark btn-sm cancel-btn" data-orderid="${order.orderId}" onclick="cancel(this)"><i class="fas fa-ban"></i>取消</div>
            </td>`;
        }
        else {
            item_last = `<td>${checkDate}</td>`;
        }

        var item = `<tr>
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
                        ${item_last}
                    </tr>`;
        listParent.append(item);
    })
    loading.hide();
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