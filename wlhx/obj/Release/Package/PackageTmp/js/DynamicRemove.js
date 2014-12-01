function DynamicRemove(id, index)
{
    $("#dg").datagrid("loading");
    $.ajax({
        type: "post",
        url: "/User/DynamicRemove",
        data: {
            id: id
        },
        success: function (data)
        {
            $("#dg").datagrid("loaded");
            if (data.status == "1")
            {
                $.messager.alert("提示", data.msg, "info");
                $("#dg").datagrid("deleteRow", index);
                $("#edit").linkbutton('disable');
                $("#remove").linkbutton("disable");
            }
            else
            {
                $.messager.alert("提示", data.msg, "error");
            }

        }
    })
}