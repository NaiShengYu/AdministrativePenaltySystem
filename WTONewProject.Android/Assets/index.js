

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

function HbuttonClick()
{
    // 调OC方法  顺便传递参数
    //window.webkit.messageHandlers.secondClick.postMessage({'goodsId': '1212','name':'你大爷'})
    secondClick("你好啊","一点都不好");

}

function ZTHbuttonClick()
{
    
    // 调OC方法  顺便传递两个参数，比如，姓名，订单号
    ZTHTestParameteroneAndParametertwo("你好啊","一点都不好");
}


function pushCode(str,str2){
    alert(str);

}
