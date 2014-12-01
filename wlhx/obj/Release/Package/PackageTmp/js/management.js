$(document).ready(function ()
{
    $("li").click(function ()
    {
        $("#aa").find("li").attr("class", "");
        $(this).attr("class", "action");
        GetHtmlContext(GetAccessFunction($(this).val()), GetParameters($(this).val()));
    });
    function GetAccessFunction(value)
    {
        switch (value)
        {
            case 11: return "/User/AdminManager"; break;
            case 12: return "/User/RePassword"; break;
            case 13: return "/User/ClearSystem"; break;
            case 21: return "/User/AddNotice"; break;
            case 22: return "/User/DynamicList"; break;
            case 31: return "/User/AddNotice"; break;
            case 32: return "/User/DynamicList"; break;
            case 41: return "/User/AddNotice"; break;
            case 42: return "/User/DynamicList"; break;
            case 51: return "/User/AddNotice"; break;
            case 52: return "/User/DynamicList"; break;
            case 61: return "/User/AddNotice"; break;
            case 62: return "/User/DynamicList"; break;
            case 71: return "/User/AddFile"; break;
            case 72: return "/User/FileList"; break;
            case 81: return "/Student/Profession"; break;
            case 91: return "/Student/MyEx"; break;
            case 92: return "/Student/AppointmentEx"; break;
            case 101: return "/Student/MyProject"; break;
            case 102: return "/Student/SeniorProject"; break;
            case 111: return "/Student/MyProject"; break;
            case 112: return "/Student/InnovationAndEnterprise"; break;
            case 121: return "/Student/StudentMsg"; break;
            case 122: return "/Student/RePassword"; break;
            case 131: return "/User/StudentManagement"; break;
            case 132: return "/User/ExperimentMangement"; break;
            case 133: return "/User/ProfessionalEmphasisManagement"; break;
            case 134: return "/User/SeniorProjectManagement"; break;
            case 135: return "/User/CreatNew"; break;
            case 141: return "/User/AddNotice"; break;
            case 142: return "/User/DynamicList"; break;

        }
    }
    function GetParameters(value)
    {
        var parameters = new Array();
        switch (value)
        {
            case 11:
                {
                    parameters[0] = 1;
                    break;
                }
            case 21:
                {
                    parameters[0] = 2;
                    break;
                }
            case 22:
                {
                    parameters[0] = 2;
                    break;
                }
            case 31:
                {
                    parameters[0] = 1;
                    break;
                }
            case 32:
                {
                    parameters[0] = 1;
                    break;
                }
            case 51:
                {
                    parameters[0] = 6;
                    break;
                }
            case 52:
                {
                    parameters[0] = 6;
                    break;
                }
            case 61:
                {
                    parameters[0] = 4;
                    break;
                }
            case 62:
                {
                    parameters[0] = 4;
                    break;
                }
            case 41:
                {
                    parameters[0] = 3;
                    break;
                }
            case 42:
                {
                    parameters[0] = 3;
                    break;
                }
            case 72:
                {
                    parameters[0] = 5;
                    break;
                }
            case 101:
                {
                    parameters[0] = 2;
                    break;
                }
            case 111:
                {
                    parameters[0] = 3;
                    break;
                }
            case 141:
                {
                    parameters[0] = 8;
                    break;
                }
            case 142:
                {
                    parameters[0] = 8;
                    break;
                }
        }
        return parameters;
    }
    function GetHtmlContext(funname, parameters)
    {
        $("#operation").empty();
        $("#operation").prepend("<div style='windth:180px;margin:auto;margin-top:200px;text-align:center'><img src=\"../../images/loading.gif\"><p>玩命加载中....</p><div>");
        $.ajax({
            type: "post",
            url: funname,
            data: {
                parameter0: parameters[0],
                parameter1: parameters[1],
                parameter2: parameters[2],
                parameter3: parameters[3],
                parameter4: parameters[4],
                parameter5: parameters[5],
                parameter6: parameters[6],
                parameter7: parameters[7],
                parameter8: parameters[8],
                parameter9: parameters[9],
            },
            dataType: "html",
            success: function (data)
            {
                $("#operation").empty();
                $("#operation").prepend(data);
            }
        });
    }
});