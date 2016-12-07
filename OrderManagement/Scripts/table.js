
// 1. 页面初始化
$(function () {
    //1.初始化Table
    var oTable = new TableInit();
    oTable.Init();

    //2.初始化Button的点击事件
    var oButtonInit = new ButtonInit();
    oButtonInit.Init();
});

var dataBind = {};
dataBind.fieldName = {
    Id: "订单号",
    Color: "颜色",
    ShoeSize: "鞋码",
    Qty: "数量",
    Name: "姓名",
    CellPhone: "电话",
    Address: "地址",
    TotalMoney: "总价",
    Message: "买家留言",
    CreateTime: "下单时间",
    LogisticsCompany: "物流公司",
    LogisticsCode: "物流单号",
    Price: "单价",
    Province: "省份",
    City: "城市",
    District: "地区",
    CustomerIP: "IP",
    Status: "订单状态",
    Remark: "备注"
}

dataBind.StatusName = {
    1: "等待确认",
    2: "确认假单",
    3: "等待发货",
    4: "已经发货",
    5: "已经签收",
    6: "已经退货"
}

var TableInit = function () {
    var oTableInit = new Object();

    //初始化Table
    oTableInit.Init = function () {
        $('#tb_orders').bootstrapTable({
            url: '/Home/GetOrder',              //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            //sortable: true,                   //是否启用排序
            //sortOrder: "asc",                 //排序方式
            queryParams: oTableInit.queryParams,//传递参数（*）
            sidePagination: "server",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,                     
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            height: 600,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [{
                field: 'Id',
                title: dataBind.fieldName['Id'],
                sortable: true,                       // 不排序的话设置固定高度则行宽度对不齐 
                align: 'center',
                valign: 'middle',
            }, {
                field: 'Color',
                title: dataBind.fieldName['Color'],
                align: 'center',
                valign: 'middle',
                sortable: true,
                // visible: false   // 控制当前列是否显示
            }, {
                field: 'ShoeSize',
                title: dataBind.fieldName['ShoeSize'],
                align: 'center',
                valign: 'middle',
                sortable: true,
            }, {
                field: 'Qty',
                title: dataBind.fieldName['Qty'],
                align: 'center',
                valign: 'middle',
                sortable: true,
            }, {
                field: 'TotalMoney',
                title: dataBind.fieldName['TotalMoney'],
                align: 'center',
                valign: 'middle',
                sortable: true,
            }, {
                field: 'Name',
                title: dataBind.fieldName['Name'],
                sortable: true,
                align: 'center',
                valign: 'middle',
            }, {
                field: 'CellPhone',
                title: dataBind.fieldName['CellPhone'],
                sortable: true,
                valign: 'middle',
            }, {
                field: 'Address',
                title: dataBind.fieldName['Address'],
                sortable: true,
            }, {
                field: 'Status',
                title: dataBind.fieldName['Status'],
                sortable: true,
                align: 'center',
                valign: 'middle',
                editable: {
                    type: 'select',
                    title: dataBind.fieldName['Status'],
                    source: [
                        { value: "1", text: "等待确认" },
                        { value: "2", text: "确认假单" },
                        { value: "3", text: "等待发货" },
                        { value: "4", text: "已经发货" },
                        { value: "5", text: "已经签收" },
                        { value: "6", text: "已经退货" }
                    ]
                }
            }, {
                field: 'CreateTime',
                title: dataBind.fieldName['CreateTime'],
                sortable: true,
                formatter: changeDateFormat
            }, {
                field: 'LogisticsCompany',
                title: dataBind.fieldName['LogisticsCompany'],
                sortable: true,
            }, {
                field: 'LogisticsCode',
                title: dataBind.fieldName['LogisticsCode'],
                sortable: true,
            }, {
                field: 'CustomerIP',
                title: dataBind.fieldName['CustomerIP'],
                sortable: true,
                visible: true,
            }, {
                field: 'Remark',
                title: dataBind.fieldName['Remark'],
                sortable: true,
                visible: true,
            }, {
                field: 'operate',
                title: '操作',
                width: 80,
                align: 'center',
                valign: 'middle',
                sortable: true,
                formatter: operateFormatter,
                events: operateEvents
            }],
            onEditableSave: function (field, row, oldValue, $el) {
                $.ajax({
                    type: "post",
                    url: "/Home/GetOrder",
                    data: row,
                    dataType: 'JSON',
                    success: function (data, status) {
                        if (status == "success") {
                            alert('提交数据成功');
                        }
                    },
                    error: function () {
                        alert('编辑失败');
                    },
                    complete: function () {

                    }

                });
            }
        });
    };

    function changeDateFormat(cellval) {
        if (cellval != null) {
            var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10));
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var hour = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var min = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            var sec = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
            return date.getFullYear() + "-" + month + "-" + currentDate + " " + hour + ":" + min + ":" + sec;
        }
    }

    function operateFormatter(value, row, index) {
        return [
                            '<a class="edit btn btn-xs btn-default" style="margin-left:5px" href="javascript:void(0)" title="编辑">',
                                '<i class="fa fa-pencil"></i>',
                            '</a>',
                            '<a class="remove btn btn-xs btn-default" style="margin-left:5px" href="javascript:void(0)" title="删除">',
                                '<i class="fa fa-trash-o"></i>',
                            '</a>'
        ].join('');
    }

    window.operateEvents = {
        //'click .like': function (e, value, row, index) {
        //    alert(row.id);
        //},
        'click .edit': function (e, value, row, index) {
            if (row == null || typeof row == 'undefine')
            {
                alert("不能获取选中行，请重新选择！")
            }
            var joinStr = "";
            $.each(row, function (key, value) {
                if (key == "0" || key == "Province" || key == "City" || key == "District" || key == "Status") {
                } else {
                    joinStr +=
                       ['<div class="form-group">',
                                '<label for="txt_', key, '">', dataBind.fieldName[key], '</label>',
                                '<input type="text" name="txt_', key, '" class="form-control" id="txt_' + key, '" value="', value,'"'].join('');// , value, '"'].join('');

                    //joinStr += (key == "Status" ? (dataBind.StatusName[value] + '" value="' + dataBind.StatusName[value] + '"') : (value + '" value="' + value + '"'));
                    joinStr += (key == "CreateTime" ? (changeDateFormat(value)) : (value)) + '"';

                    joinStr += ((key == "Id" || key == "CreateTime" || key == "CustomerIP") ? "disabled" : "") + '>  </div>';
                        //' </div>'].join('');
                }
            });
            $("#appendModel")[0].innerHTML = joinStr;
            $('#popupModal').modal();
        },
        'click .remove': function (e, value, row, index) {
            mif.showQueryMessageBox("将删除本条记录，是否确认删除？", function () {
                var url = '@Url.Content("~/Welcome/DeleteRecord/")' + row.id + '?rnd=' + Math.random();
                mif.ajax(url, null, afterDelete);
            });
        }
    };


    //得到查询的参数
    oTableInit.queryParams = function (params) {
        var temp = {
            //这里的键的名字和控制器的变量名必须一直，这边改动，控制器也需要改成一样的
            limit: params.limit,                                      // 页面大小
            offset: params.offset,                                    // 页码
            departmentname: $("#txt_search_departmentname").val(),    // 搜索的名字
            status: $("#txt_search_statu").val(),                     // 搜索的状态
            sortName: params.sort,                                    // 排序的字段
            sortOrder: params.order                                   // 排序的方式
        };
        return temp;
    };
    return oTableInit;
};

var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        $("#btn_add").click(function () {
            $("#myModalLabel").text("新增");
            $("#myModal").find(".form-control").val("");
            $('#myModal').modal()

            postdata.DEPARTMENT_ID = "";
        });

        $("#btn_edit").click(function () {
            var arrselections = $("#tb_orders").bootstrapTable('getSelections');
            if (arrselections.length > 1) {
                toastr.warning('只能选择一行进行编辑');

                return;
            }
            if (arrselections.length <= 0) {
                toastr.warning('请选择有效数据');

                return;
            }
            $("#myModalLabel").text("编辑");
            $("#txt_departmentname").val(arrselections[0].DEPARTMENT_NAME);
            $("#txt_parentdepartment").val(arrselections[0].PARENT_ID);
            $("#txt_departmentlevel").val(arrselections[0].DEPARTMENT_LEVEL);
            $("#txt_statu").val(arrselections[0].STATUS);

            postdata.DEPARTMENT_ID = arrselections[0].DEPARTMENT_ID;
            $('#myModal').modal();
        });

        $("#btn_delete").click(function () {
            var arrselections = $("#tb_orders").bootstrapTable('getSelections');
            if (arrselections.length <= 0) {
                toastr.warning('请选择有效数据');
                return;
            }

            Ewin.confirm({ message: "确认要删除选择的数据吗？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/Home/Delete",
                    data: { "":JSON.stringify(arrselections) },
                    success: function (data, status) {
                        if (status == "success") {
                            toastr.success('提交数据成功');
                            $("#tb_orders").bootstrapTable('refresh');
                        }
                    },
                    error: function () {
                        toastr.error('Error');
                    },
                    complete: function () {

                    }

                });
            });
        });

        $("#btn_submit").click(function () {
            $.each(dataBind.fieldName, function (key, value) {
                postdata[key] = $("#txt_" + key).val();
            });

            $.ajax({
                type: "post",
                url: "/api/Orders/PostOrders",
                data:postdata ,
                success: function (data, status) {
                    if (status == "success") {
                        toastr.success('提交数据成功');
                        $("#tb_orders").bootstrapTable('refresh');
                    }
                },
                error: function () {
                    toastr.error('Error');
                },
                complete: function () {

                }

            });
        });

        $("#btn_query").click(function () {
            $("#tb_orders").bootstrapTable('refresh');
        });
    };

    return oInit;
};