﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CN.Jpush.Android.Api;

namespace WTONewProject.Droid.JPush
{
    /// <summary>
    /// SDK 对自定义消息，只是传递，不会有任何界面上的展示。(可以根据消息内容进行处理)
    /// </summary>
    public class MessageHandler : BaseHandler
    {
        public override string Action
        {
            get
            {
                return "cn.jpush.android.intent.MESSAGE_RECEIVED";
            }
        }

        public override void Handle(Intent bundle)
        {

            //保存服务器推送下来的消息的标题
            var title = bundle.GetStringExtra(JPushInterface.ExtraTitle);

            //保存服务器推送下来的消息内容（最好用Json，方便处理）
            var msg = bundle.GetStringExtra(JPushInterface.ExtraMessage);

            //保存服务器推送下来的附加字段。这是个 JSON 字符串。
            //对应 API 消息内容的 extras 字段。
            //对应 Portal 推送消息界面上的“可选设置”里的附加字段。
            var extra = bundle.GetStringExtra(JPushInterface.ExtraExtra);

            //保存服务器推送下来的内容类型。
            //对应 API 消息内容的 content_type 字段。
            var contentType = bundle.GetStringExtra(JPushInterface.ExtraContentType);

            //SDK 1.4.0 以上版本支持。
            //富媒体通消息推送下载后的文件路径和文件名。
            var richFilePath = bundle.GetStringExtra(JPushInterface.ExtraRichpushFilePath);

            //SDK 1.6.1 以上版本支持。
            //唯一标识消息的 ID, 可用于上报统计等。
            var msgID = bundle.GetStringExtra(JPushInterface.ExtraMsgId);
        }
    }
}