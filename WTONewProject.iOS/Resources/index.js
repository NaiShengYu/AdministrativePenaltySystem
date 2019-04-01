

function buttonClick()
{
    // 调OC方法
    alert("1111");

//    testobject.ZTHShowPicker();
//    testobject.wxpay("");
    
    window.webkit.messageHandlers.filstClick.postMessage(null)
}

function XHbuttonClick()
{
    testobject.ZTHpay();
}

function HbuttonClick(str,str1)
{
    alert("调OC方法  顺便传递参数");
    // 调OC方法  顺便传递参数
    window.webkit.messageHandlers.getLocation.postMessage({'goodsId': str,'name':str1})

}

function ZTHbuttonClick()
{
    // 调OC方法  顺便传递两个参数，比如，姓名，订单号
    getLocation();
    
}


function setLocation(str,str2){
    alert(str);
HbuttonClick(str,str2);

}
