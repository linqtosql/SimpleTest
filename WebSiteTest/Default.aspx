<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebSiteTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script>
        var arr = [];
        function Persion() {
            for (var i = 0; i < 10; i++) {
                var item = function () {
                    var j = i;
                    return function() {
                        return j;
                    };
                };
                arr.push(item());
            }
        }

        Persion();
        
        for (var i = 0; i < arr.length; i++) {
            console.log(arr[i]());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
