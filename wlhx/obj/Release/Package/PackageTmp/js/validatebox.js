function validata(value)
{
    return value.search(/^[A-Za-z0-9]{1,25}$/) != -1;
}

function CheckData(value, regex)
{
    return value.search(regex) != -1;
}

$.extend($.fn.validatebox.defaults.rules, {
    equals: {
        validator: function (value, param)
        {
            var id = "#" + param[0];
            return value == $(id).val();
        },
        message: '两次输入的密码不一致'
    }
});


$.extend($.fn.validatebox.defaults.rules, {
    pwd: {
        validator: function (value)
        {
            return value.search(/^[A-Za-z0-9]{1,10}$/) != -1;
        },
        message: '请输入最多于10个数字或字母！'
    }
});






