
// 1. 页面初始化
$(function () {

    //1.初始化Table
    var userTable = new TableInitUser();
    userTable.Init();

    //2.初始化Button的点击事件
    var userButtonInit = new ButtonInit();
    userButtonInit.Init();
});

var userTableInit = new Object();

userTableInit.fieldName = {
    Id: "账户ID",
    UserName: "用户名",
    PassWord: "密码",
    UserLevel: "用户身份",
    Account: "登录账号",
    Remark: "备注"
}

var TableInitUser = function () {
   

    //初始化Table
    userTableInit.Init = function () {
        $('#tb_userinfo').bootstrapTable({
            url: '/api/Users',              //请求后台的URL（*）
            method: 'get',                      //请求方式（*）
            toolbar: '#toolbar',                //工具按钮用哪个容器
            striped: true,                      //是否显示行间隔色
            cache: false,                       //是否使用缓存，默认为true，所以一般情况下需要设置一下这个属性（*）
            pagination: true,                   //是否显示分页（*）
            queryParams: userTableInit.queryParams,//传递参数（*）
            sidePagination: "client",           //分页方式：client客户端分页，server服务端分页（*）
            pageNumber: 1,                      //初始化加载第一页，默认第一页
            pageSize: 10,                       //每页的记录行数（*）
            pageList: [10, 25, 50, 100],        //可供选择的每页的行数（*）
            search: false,                       //是否显示表格搜索，此搜索是客户端搜索，不会进服务端，所以，个人感觉意义不大
            strictSearch: true,
            showColumns: true,                  //是否显示所有的列
            showRefresh: true,                  //是否显示刷新按钮
            minimumCountColumns: 2,             //最少允许的列数
            clickToSelect: true,                //是否启用点击选中行
            //height: 600,                        //行高，如果没有设置height属性，表格自动根据记录条数觉得表格高度
            uniqueId: "Id",                     //每一行的唯一标识，一般为主键列
            showToggle: true,                  //是否显示详细视图和列表视图的切换按钮
            cardView: false,                    //是否显示详细视图
            detailView: false,                  //是否显示父子表
            columns: [{
                checkbox: true,
                align: 'center',
                valign: 'middle',
            }, {
                field: 'Id',
                title: userTableInit.fieldName['Id'],
                align: 'center',
                valign: 'middle',
            }, {
                field: 'UserName',
                title: userTableInit.fieldName['UserName'],
                align: 'center',
                valign: 'middle',
            }, {
                field: 'Account',
                title: userTableInit.fieldName['Account'],
                align: 'center',
                valign: 'middle',
            },{
                field: 'UserLevel',
                title: userTableInit.fieldName['UserLevel'],
                align: 'center',
                valign: 'middle',
                formatter: function (value, row, index) {
                        var data = value == 1 ? "管理员" : "普通用户";
                    return data;
                }
            }, {
                field: 'Remark',
                title: userTableInit.fieldName['Remark'],
                align: 'center',
                valign: 'middle',
            },
            //    field: 'PassWord',
            //    title: userTableInit.fieldName['PassWord'],
            //    align: 'center',
            //    valign: 'middle',
              {
                field: 'operate',
                title: '操作',
                width: 100,
                align: 'center',
                valign: 'middle',
                //sortable: true,
                formatter: operateFormatter,
                events: operateEvents
            }],
            onEditableSave: function (field, row, oldValue, $el) {
                $.ajax({
                    type: "post",
                    url: "/Home/GetUserInfo",
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

    function operateFormatter(value, row, index) {
        return [
                            //'<a class="edit btn btn-xs btn-default" style="margin-left:5px" href="javascript:void(0)" title="编辑">',
                            //    '<i class="fa fa-pencil"></i>',
                            //'</a>',
                            '<a class="remove btn btn-xs btn-default" style="margin-left:5px" href="javascript:void(0)" title="删除">',
                                '<i class="fa fa-trash-o"></i>',
                            '</a>'
        ].join('');
    }

    window.operateEvents = {
        //'click .like': function (e, value, row, index) {
        //    alert(row.id);
        ////},
        //'click .edit': function (e, value, row, index) {
        //    if (row == null || typeof row == 'undefine') {
        //        alert("不能获取选中行，请重新选择！")
        //    }
        //    var joinStr = "";
        //    $.each(row, function (key, value) {
        //        if (key != "0") {
        //            joinStr +=
        //               ['<div class="form-group">',
        //                        '<label for="txt_', key, '">', userTableInit.fieldName[key], '</label>',
        //                        '<input type="text" name="txt_', key, '" class="form-control" id="txt_' + key, '" placeholder="', value, '">',
        //                ' </div>'].join('');
        //        }
        //    });
        //    $("#appendModel")[0].innerHTML = joinStr;
        //    $('#popupModal').modal();
        //},
        'click .remove': function (e, value, row, index) {
            Ewin.confirm({ message: "确认要删除选择的用户吗？" }).on(function (e) {
                if (!e) {
                    return;
                }
                $.ajax({
                    type: "post",
                    url: "/Home/Delete",
                    data: { "": JSON.stringify(arrselections) },
                    success: function (data, status) {
                        if (status == "success") {
                            toastr.success('提交数据成功');
                            $("#tb_departments").bootstrapTable('refresh');
                        }
                    },
                    error: function () {
                        toastr.error('Error');
                    },
                    complete: function () {

                    }

                });
            });
        }
    };


    //得到查询的参数
    userTableInit.queryParams = function (params) {
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
    return userTableInit;
};


var ButtonInit = function () {
    var oInit = new Object();
    var postdata = {};

    oInit.Init = function () {
        $("#btn_addUser").click(function () {
            //$("#addUserModal").text("新增");
            //$("#addUserModal").find(".form-control").val("");
            $('#addUserModal').modal()

            //postdata.DEPARTMENT_ID = "";
        });

        $("#btn_edit").click(function () {
            var arrselections = $("#tb_departments").bootstrapTable('getSelections');
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
            var arrselections = $("#tb_departments").bootstrapTable('getSelections');
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
                    data: { "": JSON.stringify(arrselections) },
                    success: function (data, status) {
                        if (status == "success") {
                            toastr.success('提交数据成功');
                            $("#tb_departments").bootstrapTable('refresh');
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
            $.each(userTableInit.fieldName, function (key, value) {
                postdata[key] = $("#txt_" + key).val();
            });

            // 验证
            if (postdata["UserName"] == "") {
                toastr.error("用户名不能为空");
                return;
            }

            if (postdata["Account"] == "")
            {                
                toastr.error("登录账号不能为空\n建议使用手机号作为登录账号");
                return;
            }

            if (postdata["PassWord"] == "") {
                toastr.error("密码不能为空");
                return;
            }
            if (postdata["PassWord"] != $("#txt_ConfirmPassWord").val() ){
                toastr.error("两次输入密码不一致，请重新输入");
                return;
            }

            $.ajax({
                type: "post",
                url: "/api/Users",
                data: postdata,
                success: function (data, status) {
                    if (status == "success") {                      
                        toastr.success('提交数据成功');
                        $("#tb_departments").bootstrapTable('refresh');
                    }
                },
                error: function (data) {
                    //JSON.stringify()
                    toastr.error(JSON.parse(data.responseText).Message);
                },
                complete: function () {

                }

            });
        });

        $("#btn_query").click(function () {
            $("#tb_departments").bootstrapTable('refresh');
        });
    };

    return oInit;
};