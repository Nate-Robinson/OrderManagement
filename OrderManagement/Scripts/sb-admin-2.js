// vue,js 学习
////vue Model
//var data = {
//    rows: [
//    { Id: 1, Name: '小明', Age: 18, School: '光明小学', Remark: '三好学生' },
//    { Id: 2, Name: '小刚', Age: 20, School: '复兴中学', Remark: '优秀班干部' },
//    { Id: 3, Name: '吉姆格林', Age: 19, School: '光明小学', Remark: '吉姆做了汽车公司经理' },
//    { Id: 4, Name: '李雷', Age: 25, School: '复兴中学', Remark: '不老实的家伙' },
//    { Id: 5, Name: '韩梅梅', Age: 22, School: '光明小学', Remark: '在一起' },
//    ],
//    rowtemplate: { Id: 0, Name: '', Age: '', School: '', Remark: '' }
//};
////ViewModel
//var vue = new Vue({
//    el: '#wrapper',
//    data: data,
//    methods: {
//        Save: function (event) {
//            if (this.rowtemplate.Id == 0) {
//                //设置当前新增行的Id
//                this.rowtemplate.Id = this.rows.length + 1;
//                this.rows.push(this.rowtemplate);
//            }

//            //还原模板
//            this.rowtemplate = { Id: 0, Name: '', Age: '', School: '', Remark: '' }
//        },
//        Delete: function (id) {
//            //实际项目中参数操作肯定会涉及到id去后台删除，这里只是展示，先这么处理。
//            for (var i = 0; i < this.rows.length; i++) {
//                if (this.rows[i].Id == id) {
//                    this.rows.splice(i, 1);
//                    break;
//                }
//            }
//        },
//        Edit: function (row) {
//            this.rowtemplate = row;
//        }
//    }
//});


$(function () {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
// Sets the min-height of #page-wrapper to window size
$(function() {
    $(window).bind("load resize", function() {
        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $('div.navbar-collapse').addClass('collapse');
            topOffset = 100; // 2-row-menu
        } else {
            $('div.navbar-collapse').removeClass('collapse');
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            $("#page-wrapper").css("min-height", (height) + "px");
        }
    });

    var url = window.location;
    var element = $('ul.nav a').filter(function() {
        return this.href == url;
    }).addClass('active').parent().parent().addClass('in').parent();
    if (element.is('li')) {
        element.addClass('active');
    }
});