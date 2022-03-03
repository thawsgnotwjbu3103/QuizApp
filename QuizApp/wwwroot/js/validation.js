$(document).ready(function () {
    $("#submit").click(function (e) {
        $("#postUsers").validate({
            rules: {
                FullName: "required",
                Gender: "required",
                Birthday: "required",
                IdNum: {
                    required: true,
                    maxlength: 12
                },
                Address: "required",
                PhoneNum: {
                    required: true,
                    maxlength: 10
                }
            },

            messages: {
                FullName: "Vui lòng nhập họ và tên",
                Gender: "Vui lòng chọn giới tính",
                Birthday: "Vui lòng nhập ngày sinh",
                IdNum: {
                    required: "Vui lòng điền CMND/CCCD",
                    maxlength: "Độ dài tối đa là 12 ký tự"
                },
                Address: "Vui lòng nhập địa chỉ",
                PhoneNum: {
                    required: "Vui lòng nhập số điện thoại",
                    maxlength: "Độ dài tối đa là 10 ký tự"
                }
            }
        });
    });
});