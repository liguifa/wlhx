function CheckStr(html)
{
    return html.replace(/&lt;/g, "<").replace(/&gt;/g, ">").replace(/&quot;/g, "\"").replace(/amp;/g, "");
}