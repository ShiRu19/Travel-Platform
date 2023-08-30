$(function () {
    GetOrderList();
});

function GetOrderList() {
    axios.get("/api/v1.0/Order/GetOrderList", config)
        .then((response) => {
            var unchecked = response.data.order_unchecked;
            var checked = response.data.order_checked;
            var canceled = response.data.order_canceled;

            unchecked.forEach((order) => {
                var item_unchecked = `<tr>
                                        <td>#</td>
                                        <td>${order.productNumber}</td>
                                        <td>${order.qty}</td>
                                        <td>${order.total}</td>
                                        <td>${order.userName}</td>
                                        <td>${order.userEmail}</td>
                                        <td>${order.orderDate}</td>
                                        <td>
                                            <a class="btn btn-info btn-sm" href="#">�q��Ա�</a>
                                        </td>
                                        <td class="project-actions text-right">
                                            <div class="btn btn-success btn-sm check-btn" data-orderid="${order.orderId}" onclick="check(this)"><i class="fas fa-check"></i>�T�{</div>
                                            <div class="btn btn-dark btn-sm cancel-btn" data-orderid="${order.orderId}" onclick="cancel(this)"><i class="fas fa-ban"></i>����</div>
                                        </td>
                                    </tr>`;
                $("#unchecked-table tbody").append(item_unchecked);
            })

            checked.forEach((order) => {
                var item_checked = `<tr>
                                        <td>#</td>
                                        <td>${order.productNumber}</td>
                                        <td>${order.qty}</td>
                                        <td>${order.total}</td>
                                        <td>${order.userName}</td>
                                        <td>${order.userEmail}</td>
                                        <td>${order.orderDate}</td>
                                        <td>
                                            <a class="btn btn-info btn-sm" href="#">�q��Ա�</a>
                                        </td>
                                        <td>${order.checkDate}</td>
                                    </tr>`;
                $("#checked-table tbody").append(item_checked);
            })

            canceled.forEach((order) => {
                var item_canceled = `<tr>
                                        <td>#</td>
                                        <td>${order.productNumber}</td>
                                        <td>${order.qty}</td>
                                        <td>${order.total}</td>
                                        <td>${order.userName}</td>
                                        <td>${order.userEmail}</td>
                                        <td>${order.orderDate}</td>
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
    axios.post("/api/v1.0/Order/ChangeCheckedStatus", {
            orderId: checkBtn.dataset.orderid,
            status: "checked"
        }, config)
        .then((response) => {
            location.reload();
        })
        .catch((error) => {
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        });
}

function cancel(cancelBtn) {
    axios.post("/api/v1.0/Order/ChangeCheckedStatus", {
        orderId: cancelBtn.dataset.orderid,
        status: "canceled"
    }, config)
        .then((response) => {
            location.reload();
        })
        .catch((error) => {
            console.log(error);
            alert("��p...�o�ͤF�@�ǿ��~�A�ЦA�դ@���I");
        });
}